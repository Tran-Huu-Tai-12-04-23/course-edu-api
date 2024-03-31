namespace course_edu_api.Entities;

public class Question
{
    public Question()
    {
    }
    
    public long Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public List<string> Answers { get; set; } = [];
    public int CorrectAnswerIndex { get; set; }  
    public string Explain { get; set; } = string.Empty;
    public string ImgUrl { get; set; } = string.Empty;
    
}