using course_edu_api.Data;
using course_edu_api.Data.RequestModels;
using course_edu_api.Entities;
using course_edu_api.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace course_edu_api.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        
        public CourseController(ICourseService courseService, ICategoryService categoryService)
        {
            this._courseService = courseService;
            this._categoryService = categoryService;
        }
        
        /// <summary>
        /// Pagination for course
        /// </summary>
        /// <param name="paginationRequestDto"></param>
        /// <returns></returns>
        [HttpPost("pagination")]
        public async Task<ActionResult> GetCoursesPagination(PaginationRequestDto<CourseQueryDto> paginationRequestDto)
        {
            var groupedCourses = await _courseService.GetCoursePagination(paginationRequestDto);
            return Ok(groupedCourses);
        }
        
        [HttpPost("count")]
        public async Task<ActionResult> CountCourse(CourseQueryDto courseQueryDto)
        {
            var totalCourse = await _courseService.CountTotalCourse(courseQueryDto);
            return Ok(totalCourse);
        }
        
        // GET: api/course/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(long id)
        {
            var course = await _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        // POST: api/course
        [HttpPost]
        public async Task<ActionResult> CreateCourse(CourseRequest? course)
        {

            var category = await _categoryService.GetCategoryById(course.CategoryId);

            var newCourse = new Course();
            newCourse.CategoryCourse = category;
            newCourse.Title = course.Title;
            newCourse.Thumbnail = course.Thumbnail;
            newCourse.SubTitle = course.SubTitle;
            newCourse.Target = course.Target;
            newCourse.RequireSkill = course.RequireSkill;
            newCourse.Price = course.Price;
            newCourse.Description = course.Description;
            newCourse.AdviseVideo = course.AdviseVideo;

            var newCourseRes = await _courseService.CreateCourse(newCourse);
            return Ok(newCourseRes);

        }

        // PUT: api/course/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(long id, CourseRequest updatedCourse)
        {
            if (id != updatedCourse.Id)
            {
                return Ok(false);
            }
            var success = await _courseService.UpdateCourse(id, updatedCourse);

            return Ok(success);
        }

        // DELETE: api/course/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(long id)
        {
            var isOke = await _courseService.DeleteCourse(id);
            return  Ok(isOke);
        }
        // post add new course group lesson
        [HttpPost("add-group-lesson/{courseId}")]
        public async Task<IActionResult> AddNewGroupLesson([FromRoute]long courseId, [FromBody]GroupLesson groupLesson)
        {
            var newGroupLesson = await _courseService.AddGroupLesson(courseId, groupLesson);
            return  Ok(newGroupLesson);
        }
        
        // post add new course group lesson
        [HttpPut("update-group-lesson/{groupLessonId}")]
        public async Task<IActionResult> UpdateGroupLesson([FromRoute]long groupLessonId, [FromBody]GroupLesson updateGroupLesson)
        {
            var res = await _courseService.UpdateGroupLesson(groupLessonId, updateGroupLesson);
            return  Ok(res);
        }
        // delete group lesson - lesson in group lesson
        [HttpDelete("delete-group-lesson/{groupLessonId}")]
        public async Task<IActionResult> DeleteGroupLesson([FromRoute]long groupLessonId)
        {
            var res = await _courseService.RemoveGroupLesson(groupLessonId);
            return  Ok(res);
        }
        
        
        /// add leson for group lessons
        [HttpPost("add-new-lesson/{groupLessonId}")]
        public async Task<IActionResult> AddLesson([FromRoute]long groupLessonId, [FromBody] Lesson lesson)
        {
            return  Ok("add lesson");
        }
        
        /// updateLesson leson for group lessons
        [HttpPut("update-lesson/{lessonId}")]
        public async Task<IActionResult> UpdateLesson([FromRoute]long lessonId, [FromBody] Lesson updateLesson)
        {
            return  Ok("update lesson");
        }
        
        /// remove leson for group lessons
        [HttpDelete("delete-lesson/{lessonId}")]
        public async Task<IActionResult> DeleteLesson([FromRoute]long lessonId)
        {
            return  Ok("delete lesson");
        }
    }
}

