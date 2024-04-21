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
using Microsoft.AspNetCore.Http.HttpResults;

namespace course_edu_api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserSettingService _userSettingService;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IUserSettingService userSettingService)
        {
            this._userSettingService = userSettingService;
            this._userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(long id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            var createdUser = await _userService.CreateUser(user.Email, user.Password, user.Role);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] User user)
        {
            if (user == null || id != user.Id)
                return BadRequest();

            var result = await _userService.UpdateUser(id, user);
            if (!result)
                return NotFound();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        
        
        [HttpPut("user-setting/update/{id}")]
        public async Task<IActionResult> UpdateUserSetting(long id, [FromBody] UserSetting userSetting)
        {
            if (userSetting == null)
            {
                return BadRequest("User setting data is missing.");
            }

            if (id != userSetting.Id)
            {
                return BadRequest("ID in the request path does not match ID in the userSetting object.");
            }

            var updatedUserSetting = await _userSettingService.UpdateUserSetting(id, userSetting);
            return Ok(updatedUserSetting);
        }
        
        [HttpPost("pagination")]
        public async Task<ActionResult> GetPostPagination(PaginationRequestDto<UserQueryDto> paginationRequestDto)
        {
            try
            {
                var users = await this._userService.GetUserPagination(paginationRequestDto);
                return Ok(users);
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }
        
        [HttpPost("count")]
        public async Task<ActionResult> CountTotalUserByQuery(UserQueryDto userQueryDto)
        {
            var total = await this._userService.GetTotalUserByQuery(userQueryDto);
            return Ok(total);
        }

    }
}
