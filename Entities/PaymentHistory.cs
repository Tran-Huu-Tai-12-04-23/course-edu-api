using course_edu_api.Entities.Enum;

namespace course_edu_api.Entities;

public class PaymentHistory
{
    public PaymentHistory()
    {
    }

    public long Id { get; set; }
    public User User { get; set; }
    public TypePayment PaymentType { get; set; } = TypePayment.VNPAY;
    public DateTime PaymentAt { get; set; }
    public double? Amount { get; set; }
    public bool IsPayment { get; set; } = false;
}