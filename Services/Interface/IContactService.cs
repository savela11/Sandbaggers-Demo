using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels;

using Utilities;

namespace Services.Interface
{
    public interface IContactService
    {
        Task<ServiceResponse<List<ContactVm>>> Contacts();
    }
}
