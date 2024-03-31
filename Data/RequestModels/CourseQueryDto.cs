using course_edu_api.Entities.Enum;

namespace course_edu_api.Data.RequestModels;

public class CourseQueryDto
{
    public string? Query { get; set; } = string.Empty;
    public int? Status { get; set; } = null;
    public double? MinPrice { get; set; } = null;
    public double? MaxPrice { get; set; } = null;
    public long? CategoryId { get; set; } = null;
    
}