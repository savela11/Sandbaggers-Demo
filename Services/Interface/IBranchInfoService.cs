using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels;
using Utilities;

namespace Services.Interface;

public interface IBranchInfoService
{
  
  Task<ServiceResponse<BranchInfoVM>> BranchInfo(int id);
  Task<ServiceResponse<List<BranchInfoVM>>> BranchInfoList();
}