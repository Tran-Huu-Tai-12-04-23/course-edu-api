using course_edu_api.Data;
using course_edu_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace course_edu_api.Service.impl;

public class ImplUserSettingService : IUserSettingService
{
    private readonly DataContext _context;
    
    public ImplUserSettingService(DataContext context)
    {
        _context = context;
        
    }
   
    public async Task<UserSetting> CreateUserSetting(UserSetting userSetting)
    {
        if (userSetting == null)
            throw new ArgumentNullException(nameof(userSetting));

        var addedUserSetting = _context.UserSettings.Add(userSetting);
        await _context.SaveChangesAsync();

        return addedUserSetting.Entity;
    }

    public async Task<UserSetting> UpdateUserSetting(long id, UserSetting userSetting)
    {
        if (userSetting == null)
            throw new ArgumentNullException(nameof(userSetting));

        var existingUserSetting = await _context.UserSettings.FindAsync(id);
        if (existingUserSetting == null)
            throw new KeyNotFoundException($"UserSetting with ID {id} not found.");

        // Update existingUserSetting with values from userSetting
        existingUserSetting.FacebookLink = userSetting.FacebookLink;
        existingUserSetting.GithubLink = userSetting.GithubLink;
        existingUserSetting.IsEmailForNewCourse = userSetting.IsEmailForNewCourse;
        existingUserSetting.IsNotificationForNewCourse = userSetting.IsNotificationForNewCourse;
        existingUserSetting.IsNotificationForReplyCmt = userSetting.IsNotificationForReplyCmt;
        existingUserSetting.IsNotificationForCmtOfYourBlog = userSetting.IsNotificationForCmtOfYourBlog;
        existingUserSetting.IsNotificationForPinInDiscuss = userSetting.IsNotificationForPinInDiscuss;

        
        await _context.SaveChangesAsync();

        return existingUserSetting; 
    }

    public async Task<UserSetting> GetUserSettingById(long id)
    {
        return await _context.UserSettings.FirstOrDefaultAsync(u => u.Id == id);
    }
}

