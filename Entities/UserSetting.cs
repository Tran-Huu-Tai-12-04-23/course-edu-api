namespace course_edu_api.Entities;

public class UserSetting
{
    
    public UserSetting () {}
    public long Id { get; set; }
    public string FacebookLink { get; set; } = string.Empty;
    public string GithubLink { get; set; } = string.Empty;

    public bool IsEmailForNewCourse { get; set; } = true;
    public bool IsNotificationForNewCourse { get; set; } = true;
    public bool IsNotificationForReplyCmt { get; set; } = true;
    public bool IsNotificationForCmtOfYourBlog { get; set; } = true;
    public bool IsNotificationForPinInDiscuss { get; set; } = true;
}