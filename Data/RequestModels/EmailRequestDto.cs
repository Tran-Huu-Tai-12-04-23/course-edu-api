using System.ComponentModel.DataAnnotations;
using course_edu_api.Entities.Enum;

namespace course_edu_api.Data.RequestModels;

public class EmailRequestDto
{
    [Required]
    public string Email { get; set; }
    public string? Content { get; set; }
    public string? Title { get; set; }

}