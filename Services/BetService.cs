using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Services.Mapper;
using Utilities;
using Data.Models;


namespace Services
{
    public class BetService : IBetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<ServiceResponse<BetVm>> CreateBet(CreateBetDto createBetDto)
        {
            var serviceResponse = new ServiceResponse<BetVm>();

            try
            {
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == createBetDto.UserId, "UserProfile");
                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User Found";
                    return serviceResponse;
                }

                var bet = new Bet
                {
                    Title = createBetDto.Title,
                    Description = createBetDto.Description,
                    Amount = createBetDto.Amount,
                    CreatedByUserId = foundUser.Id,
                    CanAcceptNumber = createBetDto.CanAcceptNumber,
                    IsActive = createBetDto.IsActive,
                    DoesRequirePassCode = createBetDto.DoesRequirePassCode,
                    AcceptedByUserIds = new List<string>(),
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                };

                var createdBet = await _unitOfWork.Bet.AddAsync(bet);

                var betVm = await BetMapper.BetVm(createdBet);


                serviceResponse.Data = betVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<BetVm>> Bet(int betId)
        {
            var serviceResponse = new ServiceResponse<BetVm>();
            try
            {
                var foundBet = await _unitOfWork.Bet.GetFirstOrDefault(b => b.BetId == betId);
                if (foundBet == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Bet found";
                    return serviceResponse;
                }


                var betVm = await BetMapper.BetVm(foundBet);
                serviceResponse.Data = betVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<BetVm>>> AllActiveBets()
        {
            var serviceResponse = new ServiceResponse<List<BetVm>>();

            try
            {
                var activeBets = await _unitOfWork.Bet.GetAll(filter: b => b.IsActive, bets => bets.OrderByDescending(b => b.CreatedOn));

                var betVmList = new List<BetVm>();

                foreach (var bet in activeBets)
                {
                    var betVm = await BetMapper.BetVm(bet);

                    betVmList.Add(betVm);
                }

                serviceResponse.Data = betVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<BetVm>>> UserBets(string id)
        {
            var serviceResponse = new ServiceResponse<List<BetVm>>();
            try
            {
                var userBets = await _unitOfWork.Bet.GetAll(b => b.CreatedByUserId == id);

                var betVmList = new List<BetVm>();
                foreach (var userBet in userBets)
                {
                    var betVm = await BetMapper.BetVm(userBet);
                    betVmList.Add(betVm);
                }


                serviceResponse.Data = betVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteBet(DeleteBetDto deleteBetDto)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var bet = await _unitOfWork.Bet.GetFirstOrDefault(b => b.BetId == deleteBetDto.betId);


                if (bet == null)
                {
                    serviceResponse.Message = "No bet found.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                if (bet.CreatedByUserId != deleteBetDto.userId)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Users can only delete their own bet.";
                    return serviceResponse;
                }

                await _unitOfWork.Bet.RemoveAsync(bet.BetId);

                await _unitOfWork.Save();

                serviceResponse.Data = "Bet has been deleted";
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<AcceptedByUserVm>> AcceptBet(UserAcceptedBetDto userAcceptedBetDto)
        {
            var serviceResponse = new ServiceResponse<AcceptedByUserVm>();

            try
            {
                var foundBet = await _unitOfWork.Bet.GetFirstOrDefault(b => b.BetId == userAcceptedBetDto.BetId);
                if (foundBet == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Bet Found";
                    return serviceResponse;
                }

                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == userAcceptedBetDto.UserId);

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by Id";
                    return serviceResponse;
                }

                // Stop user from being able to accept their own bet
                if (foundBet.CreatedByUserId == foundUser.Id)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Cannot Accept your own bet.";
                    return serviceResponse;
                }


                // Are users still able to accept the bet
                // Is the Bet Active
                if (foundBet.AcceptedByUserIds.Count < foundBet.CanAcceptNumber && foundBet.IsActive)
                {
                    // Have any users accepted the bet
                    if (foundBet.AcceptedByUserIds.Count > 0)
                    {
                        // Has user already accepted bet
                        var userAlreadyExists = foundBet.AcceptedByUserIds.Exists(id => id == userAcceptedBetDto.UserId);

                        if (userAlreadyExists)
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = "You have already accepted this bet.";
                            return serviceResponse;
                        }

                        foundBet.AcceptedByUserIds.Add(foundUser.Id);
                    }
                    // No users accepted the bet - add user
                    else
                    {
                        foundBet.AcceptedByUserIds.Add(foundUser.Id);
                    }

                    var acceptedByUserVm = UserMapper.AcceptedByUserVm(foundUser);
                    await _unitOfWork.Save();
                    serviceResponse.Data = acceptedByUserVm;
                }

                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Bet is not active or is not allowing anyone else to accept the bet";
                }
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<BetVm>> UpdateBet(BetVm betVm)
        {
            var serviceResponse = new ServiceResponse<BetVm>();

            try
            {
                var foundBet = await _unitOfWork.Bet.GetFirstOrDefault(b => b.BetId == betVm.BetId);
                if (foundBet == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Bet found";
                    return serviceResponse;
                }

                if (foundBet.CreatedByUserId != betVm.CreatedBy.Id)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Only the person that created the bet can update.";
                    return serviceResponse;
                }


                // do not allow to change bet amount if there are users that have already accepted
                if (foundBet.AcceptedByUserIds.Count > 0)
                {
                    foundBet.Description = betVm.Description;
                    foundBet.Title = betVm.Title;
                    foundBet.IsActive = betVm.IsActive;
                    foundBet.DoesRequirePassCode = betVm.DoesRequirePassCode;
                    foundBet.UpdatedOn = DateTime.UtcNow;

                    // disallow lowering the number of accepted bets to below the current amount of accepted user
                    if (betVm.CanAcceptNumber > foundBet.AcceptedByUserIds.Count)
                    {
                        foundBet.CanAcceptNumber = betVm.CanAcceptNumber;
                    }
                }
                else
                {
                    foundBet.Amount = betVm.Amount;
                    foundBet.Description = betVm.Description;
                    foundBet.Title = betVm.Title;
                    foundBet.IsActive = betVm.IsActive;
                    foundBet.CanAcceptNumber = betVm.CanAcceptNumber;
                    foundBet.DoesRequirePassCode = betVm.DoesRequirePassCode;
                    foundBet.UpdatedOn = DateTime.Now;
                }


                await _unitOfWork.Save();
                serviceResponse.Data = await BetMapper.BetVm(foundBet);
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }
    }
}
