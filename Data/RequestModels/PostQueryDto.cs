using course_edu_api.Entities.Enum;

namespace course_edu_api.Data.RequestModels;

public class PostQueryDto
{
    public string? Query { get; set; } = string.Empty;
    public int? Status { get; set; } = null;
    public string? Tags { get; set; } = null;
    public bool? IsApprove { get; set; } = null;
    
}