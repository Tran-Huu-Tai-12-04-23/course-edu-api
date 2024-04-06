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
    Task<GroupLesson> UpdateGroupLesson( long courseId, GroupLesson updateGroupLesson);
    Task<bool> RemoveGroupLesson(long groupLessonId);
    Task<Lesson> AddNewLesson(long groupLessonId,long courseId, Lesson newLesson);
    Task<Lesson> UpdateLesson(long lessonId, Lesson updateLesson);
    Task<bool> RemoveLesson(long lessonId);
    Task<UserCourse> RegisterCourse(RegisterCourseRequestDto registerCourseRequestDto);
    Task<UserCourse> PaymentCourse(RegisterCourseRequestDto registerCourseRequestDto);
    Task<bool> CheckUserCourseExist(long userId, long courseId);
    Task<UserCourse> GetUserCourse(long userId, long courseId);

    Task<UserCourse> ChangeCurrentProcessCourse(
        ChangeCurrentProcessCourseRequestDto changeCurrentProcessCourseRequestDto);
}