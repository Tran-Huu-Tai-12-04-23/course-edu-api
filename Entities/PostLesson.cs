namespace course_edu_api.Entities;

public class PostLesson
{
    public PostLesson()
    {
    }

    public long Id { get; set; }
    public List<SubItemPost> items { get; set; } = new List<SubItemPost>();
}