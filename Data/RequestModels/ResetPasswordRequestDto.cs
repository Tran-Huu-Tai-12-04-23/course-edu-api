using System.ComponentModel.DataAnnotations;

namespace course_edu_api.Data.RequestModels;

public class ResetPasswordRequestDto
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }

}