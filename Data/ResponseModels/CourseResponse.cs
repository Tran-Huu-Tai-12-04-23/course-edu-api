using course_edu_api.Entities;

namespace course_edu_api.Data.ResponseModels;

public class CourseResponse
{
    public CourseResponse()
    {
    }

    public CourseResponse(TypeCourse typeCourse, Course[] courses)
    {
        TypeCourse = typeCourse;
        Courses = courses;
    }

    public CourseResponse(TypeCourse typeCourse, Course[] courses, int size, int page)
    {
        TypeCourse = typeCourse;
        Courses = courses;
        Size = size;
        Page = page;
    }

    public TypeCourse TypeCourse { get; set; }
    public Course[] Courses { get; set; }
    public int Size { get; set; }
    public int Page { get; set; }
}