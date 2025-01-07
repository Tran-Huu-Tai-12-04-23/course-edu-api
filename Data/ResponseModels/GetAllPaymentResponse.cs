using course_edu_api.Entities;

namespace course_edu_api.Data.ResponseModels;

public class GetAllPaymentResponse
{
    public GetAllPaymentResponse()
    {
    }

    public GetAllPaymentResponse(List<PaymentHistory> paymentHistories, double totalPayment)
    {
        PaymentHistories = paymentHistories;
        TotalPayment = totalPayment;
    }
    

    public List<PaymentHistory> PaymentHistories { get; set; }
    public double TotalPayment { get; set; }
}