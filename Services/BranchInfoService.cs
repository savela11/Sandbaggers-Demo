using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;
using Services.Interface;
using Utilities;

namespace Services;

public class BranchInfoService : IBranchInfoService
{
  private readonly AppDbContext _dbContext;

  public BranchInfoService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<ServiceResponse<BranchInfoVM>> BranchInfo(int id)
  {
    var serviceResponse = new ServiceResponse<BranchInfoVM>();

    try
    {
      var branchInfo = await _dbContext.BranchInformation.FirstOrDefaultAsync(b => b.Id == id);

      if (branchInfo == null)
      {
        serviceResponse.Success = false;
        serviceResponse.Message = "No Branch found by Id" + id;
        return serviceResponse;
      }


      var branchInfoVm = new BranchInfoVM
      {
        Id = branchInfo.Id,
        BranchId = branchInfo.BranchId,
        IsActive = branchInfo.IsActive,

      };


      serviceResponse.Data = branchInfoVm;
    }
    catch (Exception e)
    {
      serviceResponse.Message = e.Message;
      serviceResponse.Success = false;
    }

    return serviceResponse;
  }

  public async Task<ServiceResponse<List<BranchInfoVM>>> BranchInfoList()
  {
    var serviceResponse = new ServiceResponse<List<BranchInfoVM>>();

    try
    {
      var branches = await _dbContext.BranchInformation.ToListAsync();


      if (branches.Count < 1)
      {
        serviceResponse.Message = "No Branches found";
      }

      var branchInfoVMList = branches.Select(b => new BranchInfoVM
      {
        Id = b.Id,
        BranchId = b.BranchId,
        IsActive = b.IsActive
      }).ToList();

      serviceResponse.Data = branchInfoVMList;
    }
    catch (Exception e)
    {
      serviceResponse.Message = e.Message;
      serviceResponse.Success = false;
    }

    return serviceResponse;
  }
}