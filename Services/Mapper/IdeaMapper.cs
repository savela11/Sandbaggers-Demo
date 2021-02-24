using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.ViewModels;

namespace Services.Mapper
{
    public static class IdeaMapper
    {
        public static IUnitOfWork OfWork { get; }


        public static async Task<List<IdeaVm>> IdeaVmList(IEnumerable<Idea> ideas)
        {
            var ideaVmList = new List<IdeaVm>();
            foreach (var idea in ideas)
            {
                var ideaVm = await IdeaVm(idea);

                ideaVmList.Add(ideaVm);
            }

            return ideaVmList;
        }

        public static async Task<IdeaVm> IdeaVm(Idea idea)
        {
            var createdBy = await OfWork.User.GetFirstOrDefault(u => u.Id == idea.CreatedByUserId);
            return new IdeaVm
            {
                Id = idea.Id,
                Title = idea.Title,
                Description = idea.Description,
                CreatedBy = UserMapper.CreatedByUserVm(createdBy),
                CreatedOn = idea.CreatedOn,
                UpdatedOn = idea.UpdatedOn
            };
        }
    }
}
