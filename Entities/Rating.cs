namespace course_edu_api.Entities;

public class Rating
{
    public long Id { get; set; }
    public string Content { get; set; }
    public DateTime RateAt { get; set; } = DateTime.UtcNow;
    public int Star  { get; set; }
}