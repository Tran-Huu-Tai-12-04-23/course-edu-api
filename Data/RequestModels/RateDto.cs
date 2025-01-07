using System.ComponentModel.DataAnnotations;

namespace course_edu_api.Data.RequestModels;

public class RateDto
{
    [Required]
    public long CourseId { get; set; }
    [Required]
    public long UserId { get; set; }
    [Required]
    public string Content { get; set; }
}