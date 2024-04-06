using course_edu_api.Data.RequestModels;
using course_edu_api.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace course_edu_api.Service;

public interface IAuthService
{
    public Task<bool> VerifyAccount(VerifyAccountRequestDto verifyAccountRequestDto);
    public Task<bool> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto);
    public Task<string?> GetTokenVerifyByEmail(string email);
}