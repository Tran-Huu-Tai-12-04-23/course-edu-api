using course_edu_api.Data;
using course_edu_api.Entities;
using Microsoft.AspNetCore.Mvc;
using course_edu_api.Data.RequestModels;
using course_edu_api.Service;


namespace course_edu_api.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }
       
        [HttpGet("all")]
        public async Task<ActionResult> CountTotalUserByQuery()
        {
            var res = await this._paymentService.GetAllPayment();
            return Ok(res);
        }

    }
}
