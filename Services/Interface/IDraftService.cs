using System.Threading.Tasks;
using Models.ViewModels.Views;
using Utilities;

namespace Services.Interface
{
    public interface IDraftService
    {
        Task<ServiceResponse<DraftManagerViewData>> AdminDraftManagerData();
        Task<ServiceResponse<DraftManagerViewData>> EditDraft(DraftManagerViewData draftManagerViewData);
    }
}
