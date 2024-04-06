using course_edu_api.Data;
using course_edu_api.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Azure;
using course_edu_api.Data.RequestModels;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Helper;
using course_edu_api.Service;
using course_edu_api.Service.impl;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using MimeKit;

namespace course_edu_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserSettingService _userSettingService ;
        private readonly IEmailService _emailService ;
        private readonly IAuthService _authService ;

        public AuthController(IAuthService authService, DataContext context,IConfiguration configuration, IUserSettingService userSettingService, IEmailService emailService)
        {
            this._context = context;
            this._configuration = configuration;
            this._userSettingService = userSettingService;
            this._emailService = emailService;
            this._authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterRequest model)
        {
            var response = new Response<RegisterRequest, User>();
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null && existingUser.Password != string.Empty)
            {
                response.Status = BadRequest().StatusCode;
                response.Message = "Email đã tồn tại trong hệ thống! Vui lòng thử email khác.";
                response.Meta = model;
                return Ok(response);
            }else if (existingUser is { Password: "" })
            { 
                var hashPass = JwtHelper.HashPassword(model.Password);
                existingUser.Password = hashPass;
                await _context.SaveChangesAsync();
                return Ok();
            }
            
            // Hash the password
            var hashedPassword = JwtHelper.HashPassword(model.Password);
            // Create a new user
            var token = JwtHelper.GenerateHash(model.Email + model.Password);
            var newUser = new User
            {
                Email = model.Email,
                Password = hashedPassword,  
                TokenVerify = token
            };
            var emailDto = new EmailRequestDto();
            emailDto.Email = model.Email;
            emailDto.Title = "Kích hoạt tài khoản!";
            emailDto.Content = "<div><a href='http://localhost:5173/verify-account?token=" +
                               token + "&&email="+model.Email + "'>Kích hoạt tài khoản của bạn ở đây!</a></div>";
            await _emailService.SendEmail(emailDto);
            var userSetting = await this._userSettingService.CreateUserSetting(new UserSetting());
            
            newUser.UserSetting = userSetting;
            // Add the user to the database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            response.Data = newUser;
            response.Meta = model;
            response.Status = 200;
            response.Message = "Đăng ký thành công!";

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest model)
        {
            var response = new Response<User?, Token>();
            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !Helper.JwtHelper.VerifyPassword(model.Password, user.Password))
            {
                response.Message = "Email hoặc mật khẩu không hợp lệ!";
                response.Meta = user;
                response.Status = Unauthorized().StatusCode;
                // Authentication failed
                return Ok(response);
            }
            var issuer = this._configuration["JwtTokenSettings:Issuer"] ?? string.Empty; 
            var audience = this._configuration["JwtTokenSettings:Audience"] ?? string.Empty;
            var key = this._configuration["JwtTokenSettings:Key"] ?? string.Empty;
            var jwtData = new JwtData(issuer, audience, key);
            
            if (user.VerifyAt == null) return BadRequest("Người dùng chưa kích hoạt tài khoản!");
            
            var token = JwtHelper.GenerateToken(jwtData, user);
            response.Data = token;
            response.Message = "Đăng nhập thành công!";
            response.Status = 200;
            response.Meta = user;
            // Authentication successful
            return Ok(response);
        }
       [HttpPost("login-with-google")]
       public async Task<ActionResult> LoginWithGoogle([FromBody] User model)
       {
           var response = new Response<User,Token>();
           // Find the user by email
           var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
           if (user == null )
           {
               var userSetting = await this._userSettingService.CreateUserSetting(new UserSetting());
                model.UserSetting = userSetting;
                var newUser = model;
                model.VerifyAt = new DateTime();
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
           }
           var issuer = this._configuration["JwtTokenSettings:Issuer"] ?? string.Empty; 
           var audience = this._configuration["JwtTokenSettings:Audience"] ?? string.Empty;
           var key = this._configuration["JwtTokenSettings:Key"] ?? string.Empty;
           var jwtData = new JwtData(issuer, audience, key);
           var token = JwtHelper.GenerateToken(jwtData, user ?? model);
           response.Data = token;
           response.Message = "Đăng nhập thành công!";
           response.Status = 200;
           response.Meta = user ?? model;
           // Authentication successful
           return Ok(response);
       }
        
        
        /// <summary>
        ///  send emaiol verify 
        /// </summary>
        /// <param name="enteredPassword"></param>
        /// <param name="storedPasswordHash"></param>
        /// <returns></returns>
        [HttpPost("verify-account")]
        public async Task<ActionResult> VerifyAccount([FromBody] VerifyAccountRequestDto verifyData)
        {
            try
            {
                var res = await _authService.VerifyAccount(verifyData);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }  
        }

        
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            try
            {
                var res = await _authService.ResetPassword(resetPasswordRequestDto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }  
        }

        
        [HttpPost("send-link-reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] EmailRequestDto emailRequestDto)
        {
            try
            {
                var token = await _authService.GetTokenVerifyByEmail(emailRequestDto.Email);
                var emailDto = new EmailRequestDto();
                emailDto.Email = emailRequestDto.Email;
                emailDto.Title = "KHôi phục tài khoản của bạn!";
                emailDto.Content = "<div><a href='http://localhost:5173/reset-password?token=" +
                                   token + "&&email="+emailRequestDto.Email + "'>Khôi phục mật khẩu của bạn ở đây</a></div>";
                await _emailService.SendEmail(emailDto);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }  
        }

       
    }
}
