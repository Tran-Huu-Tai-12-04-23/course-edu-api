
using course_edu_api.Entities.Enum;

namespace course_edu_api.Entities
{
    public class Post
    {
        public Post()
        {
            IsPin = false;
        }

        public Post(string? title, string? tags, string? Thumbnail, User? user)
        {
            Title = title;
            Thumbnail = Thumbnail;
            User = user;
            Tags = tags;
            IsPin = false;
            isApproved = false;
        }

        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Tags { get; set; }
        public string? Description { get; set; } = string.Empty;
        public PostStatus Status { get; set; }
        public string? Thumbnail { get; set; }
        public bool IsPin { get; set; } = false;
        public bool isApproved { get; set; } = false;
        public User User { get; set; }
        
        public List<SubItemPost>? Items { get; set; }
        
    }
}
