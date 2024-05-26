using System.Collections;
using System.Diagnostics;
using course_edu_api.Data;
using course_edu_api.Data.RequestModels;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Entities;
using course_edu_api.Entities.Enum;
using course_edu_api.Helper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace course_edu_api.Service.impl;

public class ImplAuthService : IAuthService
{
    private readonly DataContext _context;

    public ImplAuthService(DataContext context)
    {
        _context = context;
    }


    public async  Task<bool> VerifyAccount(VerifyAccountRequestDto verifyAccountRequestDto)
    {
        var userEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == verifyAccountRequestDto.Email);
        if (userEmail == null)
            throw new KeyNotFoundException("Không tìm thấy tài khoản với email " + verifyAccountRequestDto.Email);
        
        if (userEmail.TokenVerify != null && userEmail.TokenVerify.Equals(verifyAccountRequestDto.Token))
        {
            userEmail.TokenVerify =
                Helper.JwtHelper.GenerateHash(userEmail.Email + userEmail.Password + verifyAccountRequestDto.Token);
            userEmail.VerifyAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }else
        {
            return false;
        }
    }

    public async Task<bool> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
    {
        var userEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordRequestDto.Email);

        if (userEmail == null)
            throw new Exception("Không thể tìm thấy người dùng với email = " + resetPasswordRequestDto.Email);

        if (userEmail.TokenVerify == null || !userEmail.TokenVerify.Equals(resetPasswordRequestDto.Token))
        {
            throw new Exception("Token không hợp lệ!");
        }

        userEmail.TokenVerify = Helper.JwtHelper.GenerateHash(resetPasswordRequestDto.Email +
                                                              resetPasswordRequestDto.Password + userEmail.TokenVerify);

        userEmail.Password = JwtHelper.HashPassword(resetPasswordRequestDto.Password);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<string?> GetTokenVerifyByEmail(string email)
    {
        var userEmail = await _context.Users.FirstOrDefaultAsync(u=> u.Email == email);

        if (userEmail == null) throw new Exception("Không tìm thấy người dùng với email = " + email);

        return userEmail.TokenVerify ?? "";
    }
}