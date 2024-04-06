using System.ComponentModel.DataAnnotations;

namespace course_edu_api.Data.RequestModels;

public class RegisterCourseRequestDto
{
    [Required]
    public long CourseId { get; set; }
    [Required]
    public long UserId { get; set; }
    public bool? IsPayment { get; set; } = false;
}