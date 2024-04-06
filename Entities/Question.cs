using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace course_edu_api.Entities;

public class Question
{
    public Question()
    {
    }
    
    public long Id { get; set; }
    public int Index { get; set; }
    public string Content { get; set; } = string.Empty;
    public List<string> Answers { get; set; } = [];
    public int CorrectAnswerIndex { get; set; }  
    public string Explain { get; set; } = string.Empty;
    public string ImgURL { get; set; } = string.Empty;
}
