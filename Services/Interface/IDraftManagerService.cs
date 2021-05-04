using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels.Views;
using Utilities;

namespace Services.Interface
{
    public interface IDraftManagerService
    {
        Task<ServiceResponse<DraftManagerViewData>> AdminDraftManagerData();
        Task<ServiceResponse<string>> RemoveTeamCaptainFromDraft(AddOrRemoveTeamCaptainDto addOrRemoveTeamCaptainDto);
        Task<ServiceResponse<string>> AddTeamCaptainToDraft(AddOrRemoveTeamCaptainDto addOrRemoveTeamCaptainDto);
        Task<ServiceResponse<bool>> UpdateDraftStatus(UpdateDraftStatusDto updateDraftStatusDto);
    }
}
