using System.Net;
using System.Text;
using System.Web;
using course_edu_api.config;
using course_edu_api.Data;
using course_edu_api.Data.RequestModels;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Entities;
using course_edu_api.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace course_edu_api.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IPaymentService _paymentService;
        
        public CourseController(ICourseService courseService, ICategoryService categoryService, IPaymentService paymentService)
        {
            this._courseService = courseService;
            this._categoryService = categoryService;
            this._paymentService = paymentService;
        }
        
        /// <summary>
        /// Pagination for course
        /// </summary>
        /// <param name="paginationRequestDto"></param>
        /// <returns></returns>
        [HttpPost("pagination")]
        public async Task<ActionResult> GetCoursesPagination(PaginationRequestDto<CourseQueryDto> paginationRequestDto)
        {
            var groupedCourses = await _courseService.GetCoursePagination(paginationRequestDto);
            return Ok(groupedCourses);
        }
        
        [HttpPost("count")]
        public async Task<ActionResult> CountCourse(CourseQueryDto courseQueryDto)
        {
            var totalCourse = await _courseService.CountTotalCourse(courseQueryDto);
            return Ok(totalCourse);
        }
        
        // GET: api/course/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(long id)
        {
            var course = await _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        // POST: api/course
        [HttpPost]
        public async Task<ActionResult> CreateCourse(CourseRequest? course)
        {

            var category = await _categoryService.GetCategoryById(course.CategoryId);

            var newCourse = new Course();
            newCourse.CategoryCourse = category;
            newCourse.Title = course.Title;
            newCourse.Thumbnail = course.Thumbnail;
            newCourse.SubTitle = course.SubTitle;
            newCourse.Target = course.Target;
            newCourse.RequireSkill = course.RequireSkill;
            newCourse.Price = course.Price;
            newCourse.Description = course.Description;
            newCourse.AdviseVideo = course.AdviseVideo;

            var newCourseRes = await _courseService.CreateCourse(newCourse);
            return Ok(newCourseRes);

        }

        // PUT: api/course/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(long id, CourseRequest updatedCourse)
        {
            if (id != updatedCourse.Id)
            {
                return Ok(false);
            }
            var success = await _courseService.UpdateCourse(id, updatedCourse);

            return Ok(success);
        }

        // DELETE: api/course/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(long id)
        {
            var isOke = await _courseService.DeleteCourse(id);
            return  Ok(isOke);
        }
        // post add new course group lesson
        [HttpPost("add-group-lesson/{courseId}")]
        public async Task<IActionResult> AddNewGroupLesson([FromRoute]long courseId, [FromBody]GroupLesson groupLesson)
        {
            var newGroupLesson = await _courseService.AddGroupLesson(courseId, groupLesson);
            return  Ok(newGroupLesson);
        }
        
        // post add new course group lesson
        [HttpPut("update-group-lesson/{groupLessonId}")]
        public async Task<IActionResult> UpdateGroupLesson([FromRoute]long groupLessonId, [FromBody]GroupLesson updateGroupLesson)
        {
            var res = await _courseService.UpdateGroupLesson(groupLessonId, updateGroupLesson);
            return  Ok(res);
        }
        // delete group lesson - lesson in group lesson
        [HttpDelete("delete-group-lesson/{groupLessonId}")]
        public async Task<IActionResult> DeleteGroupLesson([FromRoute]long groupLessonId)
        {
            var res = await _courseService.RemoveGroupLesson(groupLessonId);
            return  Ok(res);
        }
        
        
        /// add leson for group lessons
        [HttpPost("add-new-lesson/{groupLessonId}/{courseId}")]
        public async Task<IActionResult> AddLesson([FromRoute]long groupLessonId, [FromRoute] long courseId, [FromBody] Lesson lesson)
        {
            var addLesson = await this._courseService.AddNewLesson(groupLessonId, courseId, lesson);
            return Ok(addLesson);
        }
        
        /// updateLesson leson for group lessons
        [HttpPut("update-lesson/{lessonId}")]
        public async Task<IActionResult> UpdateLesson([FromRoute]long lessonId, [FromBody] Lesson updateLesson)
        {
            var newLesson = await _courseService.UpdateLesson(lessonId, updateLesson);
            return  Ok(newLesson);
        }
        
        /// remove leson for group lessons
        [HttpDelete("delete-lesson/{lessonId}")]
        public async Task<IActionResult> DeleteLesson([FromRoute]long lessonId)
        {
            var isSuccess = await _courseService.RemoveLesson(lessonId);
            
            return Ok(isSuccess);
        }
        
        // user register course 
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCourseRequestDto  registerCourseRequestDto)
        {
            try
            {
                var userCourse = await _courseService.RegisterCourse(registerCourseRequestDto);
                return Ok(userCourse);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
        
        // payment course 
        [HttpPost("register/create-payment")]
        public async Task<IActionResult> CreatePayment([FromServices] IHttpContextAccessor httpContextAccessor, [FromBody] RegisterCourseRequestDto registerCourseRequestDto)
        {
            try
            {
                if (registerCourseRequestDto.IsPayment == false) return BadRequest("Phương thức thanh toán không hợp lệ!");
                var coursePayment = await _courseService.GetCourseById(registerCourseRequestDto.CourseId);
                if (coursePayment == null) return BadRequest("Thanh toán thất bại!");
                var paymentHistory = await _paymentService.CreatePayment(registerCourseRequestDto);
                paymentHistory.Amount = (long)coursePayment.Price * 100000;
                
                var vnp_Version = "2.1.0";
                var vnp_Command = "pay";
                var orderType = "other";
                var amountInVnd = (long)coursePayment.Price * 100000;
                var vnp_TxnRef = ConfigVnPay.GetRandomNumber(8);
                var vnp_TmnCode = ConfigVnPay.vnp_TmnCode;

                var vnp_Params = new Dictionary<string, string>();
                vnp_Params.Add("vnp_Version", vnp_Version);
                vnp_Params.Add("vnp_Command", vnp_Command);
                vnp_Params.Add("vnp_TmnCode", vnp_TmnCode);
                vnp_Params.Add("vnp_Amount", amountInVnd.ToString());
                vnp_Params.Add("vnp_CurrCode", "VND");
                vnp_Params.Add("vnp_BankCode", "NCB");

                vnp_Params.Add("vnp_TxnRef", vnp_TxnRef);
                vnp_Params.Add("vnp_OrderInfo", paymentHistory.Id.ToString());
                vnp_Params.Add("vnp_OrderType", orderType);
                vnp_Params.Add("vnp_Locale", "vn");

                vnp_Params.Add("vnp_ReturnUrl", ConfigVnPay.vnp_ReturnUrl);
                vnp_Params.Add("vnp_IpAddr", ConfigVnPay.GetIpAddress(httpContextAccessor.HttpContext.Request));

                var currentDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
                var vnp_CreateDate = currentDate.ToString("yyyyMMddHHmmss");
                vnp_Params.Add("vnp_CreateDate", vnp_CreateDate);

                currentDate = currentDate.AddMinutes(15);
                var vnp_ExpireDate = currentDate.ToString("yyyyMMddHHmmss");
                vnp_Params.Add("vnp_ExpireDate", vnp_ExpireDate);

                var fieldNames = new List<string>(vnp_Params.Keys);
                fieldNames.Sort();

                var hashData = new StringBuilder();
                var query = new StringBuilder();

                foreach (var fieldName in fieldNames)
                {
                    var fieldValue = vnp_Params[fieldName];
                    if (!string.IsNullOrEmpty(fieldValue))
                    {
                        // Build hash data
                        hashData.Append(fieldName)
                            .Append('=')
                            .Append(WebUtility.UrlEncode(fieldValue));

                        // Build query
                        query.Append(WebUtility.UrlEncode(fieldName))
                            .Append('=')
                            .Append(WebUtility.UrlEncode(fieldValue));

                        if (fieldNames.IndexOf(fieldName) < fieldNames.Count - 1)
                        {
                            query.Append('&');
                            hashData.Append('&');
                        }
                    }
                }
                var queryUrl = query.ToString();
                var vnp_SecureHash = ConfigVnPay.HmacSHA512(ConfigVnPay.secretKey, hashData.ToString());
                queryUrl += "&vnp_SecureHash=" + vnp_SecureHash;
                var paymentUrl = ConfigVnPay.vnp_PayUrl + "?" + queryUrl;
                var response = new PaymentResponse();
                response.Code = "00";
                response.PaymentURL = paymentUrl;
                response.Message = "success!";

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
             
        // confirm payment 
        [HttpPost("register/confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([AsParameters] long paymentId)
        {
            try
            {
                var res = await _paymentService.ConfirmPayment(paymentId);
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        
        // get course registed by user 
        [HttpGet("check-user-course-exits/{userId}/{courseId}")]
        public async Task<IActionResult> GetUserCourse([FromRoute] long userId, [FromRoute] long courseId)
        {
            try
            {
                var res = await _courseService.CheckUserCourseExist(userId, courseId);
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("get-user-course/{userId}/{courseId}")]
        public async Task<IActionResult> GetCourseLearningProcess([FromRoute] long userId, [FromRoute] long courseId)
        {
            try
            {
                var res = await _courseService.GetUserCourse(userId, courseId);
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        [HttpPost("change-current-process-learning")]
        public async Task<IActionResult> ChangeCurrentProcessCourse([FromBody] ChangeCurrentLessonRequestDto changeCurrentLessonRequestDto)
        {
            try
            {
                var res = await _courseService.ChangeCurrentLesson(changeCurrentLessonRequestDto);
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("get-user-course-by-user/{userId}")]
        public async Task<IActionResult> GetUserCourseByUser([FromRoute] long userId)
        {
            try
            {
                var res = await _courseService.GetUserCourseByUser(userId);
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        
    }
}

