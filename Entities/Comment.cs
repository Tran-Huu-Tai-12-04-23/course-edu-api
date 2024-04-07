namespace course_edu_api.Entities;

public class Comment
{
    public Comment()
    {
    }

    public long Id { get; set; }
    public string Content { get; set; }
    public DateTime CommentAt { get; set; }
}