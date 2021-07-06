using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Utilities;

namespace Services
{
    public class CourseManagerService : ICourseManagerService
    {
        private readonly AppDbContext _dbContext;

        public CourseManagerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CourseVm CreateCourseVm(Course course)
        {
            return new CourseVm
            {
                CourseId = course.CourseId,
                Name = course.Name,
                Location = new LocationVm
                {
                    Name = course.Location.Name,
                    StreetNumbers = course.Location.StreetNumbers,
                    StreetName = course.Location.StreetName,
                    State = course.Location.State,
                    City = course.Location.City,
                    PostalCode = course.Location.PostalCode,
                },
                Rounds = course.Rounds.Select(r => new RoundVm
                {
                    RoundId = r.RoundId,
                    CourseId = r.CourseId
                }).ToList(),
            };
        }

        public async Task<ServiceResponse<CourseVm>> AddCourse(AddCourseDto addCourseDto)
        {
            var serviceResponse = new ServiceResponse<CourseVm>();

            try
            {
                var foundCourse = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Name == addCourseDto.Name);
                if (foundCourse != null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Already A Course Created With that Name.";
                    return serviceResponse;
                }


                var created = await _dbContext.Courses.AddAsync(new Course
                {
                    Name = addCourseDto.Name,
                    Location = new Location(),
                    Rounds = new List<Round>()
                });

                await _dbContext.SaveChangesAsync();
                var createdCourse = CreateCourseVm(created.Entity);

                serviceResponse.Data = createdCourse;
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