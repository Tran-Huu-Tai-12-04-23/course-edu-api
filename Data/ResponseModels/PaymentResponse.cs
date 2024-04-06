using course_edu_api.Entities.Enum;

namespace course_edu_api.Data.ResponseModels;

public class PaymentResponse
{
    public PaymentResponse()
    {
    }
    public string PaymentURL { get; set; }
    public TypePayment Type { get; set; }
    public string Code { get; set; }
    public string Message { get; set; }
}