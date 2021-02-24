using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels;
using Utilities;


namespace Services.Interface
{
    public interface IIdeaService
    {
        Task<ServiceResponse<List<IdeaVm>>> AllIdeas();

        Task<ServiceResponse<IdeaVm>> Idea(GetIdeaDto getIdeaDto);

        Task<ServiceResponse<IdeaVm>> AddIdea(AddIdeaDto addIdeaDto);


        Task<ServiceResponse<string>> RemoveIdea(int id);

        Task<ServiceResponse<IdeaVm>> UpdateIdea(IdeaVm ideaVm);
    }
}
