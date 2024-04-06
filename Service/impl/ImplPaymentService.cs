using course_edu_api.Data;
using course_edu_api.Data.RequestModels;
using course_edu_api.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;

namespace course_edu_api.Service.impl;

public class ImplPaymentService : IPaymentService
{
    private readonly DataContext _context;
    private readonly ICourseService _courseService;

    public ImplPaymentService(DataContext context, ICourseService courseService)
    {
        this._context = context;
        this._courseService = courseService;
    }


    public async Task<PaymentHistory> CreatePayment(RegisterCourseRequestDto registerCourseRequestDto)
    {
        var userCourseExist = await _context.UserCourse.FirstOrDefaultAsync(u =>
            u.User.Id == registerCourseRequestDto.UserId && u.Course.Id == registerCourseRequestDto.CourseId);

        if (userCourseExist == null)
        {
            userCourseExist = await this._courseService.RegisterCourse(registerCourseRequestDto);
        }
        var userExist = await _context.Users.FindAsync(registerCourseRequestDto.UserId);
        if (userExist == null) throw new Exception("Người dùng không tồn tại trên hệ thống !");
        var paymentHis = new PaymentHistory();
        paymentHis.User = userExist;
        userCourseExist.PaymentHistory = paymentHis;
        paymentHis.Amount = userCourseExist.Course.Price;
         _context.PaymentHistorie.Add(paymentHis);
         await _context.SaveChangesAsync();
         return paymentHis;
    }

    public async Task<bool> ConfirmPayment(long paymentHistoryId)
    {
        try
        {
            var paymentHistory = await _context.PaymentHistorie.FindAsync(paymentHistoryId);
            paymentHistory.IsPayment = true;
            paymentHistory.PaymentAt = new DateTime();
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}