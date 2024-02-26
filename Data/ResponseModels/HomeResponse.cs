using course_edu_api.Entities;

namespace course_edu_api.Data.ResponseModels;

public class HomeResponse
{
    public HomeResponse()
    {
    }

    public HomeResponse(List<CourseResponse> courseRes, List<TypeCourse> typeCourses)
    {
        CourseRes = courseRes;
        TypeCourses = typeCourses;
    }
    

    public List<CourseResponse> CourseRes { get; set; }
    public List<TypeCourse> TypeCourses { get; set; }
    public List<Banner> Banners { get; set; }
}