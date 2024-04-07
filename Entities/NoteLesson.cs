namespace course_edu_api.Entities;

public class NoteLesson
{
    public NoteLesson()
    {
    }

    public NoteLesson(string content, long timeSecond, long lessonId, long userId)
    {
        Content = content;
        LessonId = lessonId;
        UserId = userId;
    }

    public long Id { get; set; }
    public string Content { get; set; }
    public long LessonId { get; set; }
    public long UserId { get; set; }
    public DateTime NoteAt { get; set; }
}