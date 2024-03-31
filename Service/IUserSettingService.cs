using course_edu_api.Entities;

namespace course_edu_api.Service;

public interface IUserSettingService
{
    Task<UserSetting> CreateUserSetting(UserSetting userSetting);
    Task< UserSetting> UpdateUserSetting(long id, UserSetting userSetting);
    Task<UserSetting> GetUserSettingById(long id);
}