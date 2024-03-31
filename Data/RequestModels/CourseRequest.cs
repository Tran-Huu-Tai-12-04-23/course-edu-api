using course_edu_api.Entities.Enum;

namespace course_edu_api.Data.RequestModels;

public class CourseRequest
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string SubTitle { get; set; } =  string.Empty;
    public string Target { get; set; } = string.Empty;
    public string RequireSkill { get; set; } = string.Empty;
    public int? Status { get; set; } 
    public string Thumbnail { get; set; }
    public string AdviseVideo { get; set; } = string.Empty;
    public long CategoryId { get; set; }
}