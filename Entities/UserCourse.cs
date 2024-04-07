namespace course_edu_api.Entities;

public class UserCourse
{
    public UserCourse()
    {
    }

    public long Id { get; set; } 
    public User User{ get; set; }
    public Course Course { get; set; }
    public DateTime RegisterAt { get; set; } = new DateTime();
    public bool IsPayment { get; set; }
    public List<Lesson> LessonPassed { get; set; } = new List<Lesson>();
    public List<NoteLesson>? Notes { get; set; } = new List<NoteLesson>();
    public PaymentHistory? PaymentHistory { get; set; }
    public Lesson? CurrentLesson { get; set; }
    
}