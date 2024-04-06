using course_edu_api.Data.RequestModels;

namespace course_edu_api.Service;

public interface IEmailService
{
    public Task<bool> SendEmail(EmailRequestDto email);
}