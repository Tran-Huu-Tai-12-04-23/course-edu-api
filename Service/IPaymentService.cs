using course_edu_api.Data.RequestModels;
using course_edu_api.Entities;

namespace course_edu_api.Service;

public interface IPaymentService
{
    public Task<PaymentHistory> CreatePayment(RegisterCourseRequestDto registerCourseRequestDto);
    public Task<bool> ConfirmPayment(long paymentHistoryId);
}