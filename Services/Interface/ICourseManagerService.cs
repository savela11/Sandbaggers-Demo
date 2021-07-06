using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels;
using Utilities;

namespace Services.Interface
{
    public interface ICourseManagerService
    {
        Task<ServiceResponse<CourseVm>> AddCourse(AddCourseDto addCourseDto);
    }
}