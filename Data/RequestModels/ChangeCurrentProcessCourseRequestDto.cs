using System.ComponentModel.DataAnnotations;

namespace course_edu_api.Data.RequestModels;

public class ChangeCurrentProcessCourseRequestDto
{
    [Required]
    public long CourseId { get; set; }
    [Required]
    public long LessonId { get; set; }
   
}