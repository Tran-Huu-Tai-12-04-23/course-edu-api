using course_edu_api.Data.RequestModels;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Entities;

namespace course_edu_api.Service;

public interface ICourseService
{
    Task<Course?> GetCourseById(long id);
    Task<List<Course>> GetAllCourses(int page, int size);
    Task<PaginatedResponse<Course>> GetCoursePagination(PaginationRequestDto<CourseQueryDto> paginationRequestDto);
    Task<Course?> CreateCourse(Course? newCourse);
    Task<Course> UpdateCourse(long id, CourseRequest updatedCourse);
    Task<bool> DeleteCourse(long id);
    Task<long> CountTotalCourse(CourseQueryDto courseQueryDto);
    Task<GroupLesson> AddGroupLesson(long courseId, GroupLesson groupLesson);
    Task<GroupLesson> UpdateGroupLesson(long groupLessonId, GroupLesson updateGroupLesson);
    Task<bool> RemoveGroupLesson(long groupLessonId);
}