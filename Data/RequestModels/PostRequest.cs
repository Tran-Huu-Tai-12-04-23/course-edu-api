using course_edu_api.Entities;

namespace course_edu_api.Data.RequestModels;

public class PostRequest
{
    public long UserId { get; set; }
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Tags { get; set; }
    public string? StatePost { get; set; }
    public string? Thumbnail { get; set; }
    public string? Description { get; set; }
    public bool IsPin { get; set; } = false;
    public List<SubItemPost>? Items { get; set; }
}