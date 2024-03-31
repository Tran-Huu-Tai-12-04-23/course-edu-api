namespace course_edu_api.Entities;

public class VideoLesson
{
    
    public VideoLesson()
    {
    }
    public VideoLesson(string videoUrl)
    {
        VideoURL = videoUrl;
    }

    public long Id { get; set; }
    public string VideoURL { get; set; } = string.Empty;
    public string VideoId { get; set; } = string.Empty;
}