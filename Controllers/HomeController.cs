using course_edu_api.Data;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace course_edu_api.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly DataContext _context;
        
        public HomeController(DataContext context)
        {
            this._context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult> GetHome()
        {
            int size = 3;
            var typeCourses = await _context.TypeCourses.ToListAsync();
            var homeResponse = new HomeResponse();
            var courseResList = new List<CourseResponse>();

            foreach (var type in typeCourses)
            {
                var courseRes = new CourseResponse();
                var courses = await GetCourseByType(type, size); // Sử dụng await ở đây
                courseRes.TypeCourse = type;
                courseRes.Courses = courses;
                courseResList.Add(courseRes);
            }

            var banners = _context.Banners.ToListAsync();

            homeResponse.CourseRes = courseResList;
            homeResponse.TypeCourses = typeCourses;
            homeResponse.Banners = banners.Result;
    
            return Ok(homeResponse);
        }

        private async Task<Course[]> GetCourseByType(TypeCourse typeCourse, int size)
        {
            var courses = await _context.Courses
                .Include(course => course.TypeCourse)
                .Where(course => course.TypeCourse.TypeName == typeCourse.TypeName)
                .Take(size) // Sử dụng Take(size) thay vì Skip và Take
                .ToArrayAsync();

            return courses;
        }

        
    }
}

