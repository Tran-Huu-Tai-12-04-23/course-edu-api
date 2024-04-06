using System.ComponentModel.DataAnnotations;

namespace course_edu_api.Data.RequestModels;

public class VerifyAccountRequestDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Token { get; set; }
}