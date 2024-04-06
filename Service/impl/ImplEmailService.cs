using course_edu_api.Data.RequestModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace course_edu_api.Service.impl;

public class ImplEmailService : IEmailService
{
    
    private readonly IConfiguration _configuration;

    public ImplEmailService(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    
    public async Task<bool> SendEmail(EmailRequestDto email)
    {
        try
        {
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"] ?? string.Empty), SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_configuration["SMTP:Username"], _configuration["SMTP:Password"]);

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(_configuration["SMTP:Username"]));
                message.To.Add(MailboxAddress.Parse(email.Email));
                message.Subject = email.Title;
                var builder = new BodyBuilder();
                builder.HtmlBody = email.Content;

                message.Body =  builder.ToMessageBody();
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);

                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }  
    }

    
}