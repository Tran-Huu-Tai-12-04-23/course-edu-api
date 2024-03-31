using course_edu_api.Entities.Enum;

namespace course_edu_api.Entities
{
    public class Course
    {
        public Course(string title, string description, double price, string thumbnail, string adviseVideo)
        {
            Title = title;
            Description = description;
            Price = price;
            Thumbnail = thumbnail;
            AdviseVideo = adviseVideo;
        }

        public Course()
        {
            this.Title = string.Empty;
            this.Description = string.Empty;
            this.Price = 0;
            this.AdviseVideo = "";
            this.Thumbnail = "";
        }

        public Course(string title, string description, double price, string thumbnail, string adviseVideo, CategoryCourse categoryCourse)
        {
            Title = title;
            Description = description;
            Price = price;
            Thumbnail = thumbnail;
            AdviseVideo = adviseVideo;
            CategoryCourse = categoryCourse;
        }

        public Course(string title, string description, double price, string subTitle, string target, string requireSkill, string thumbnail, string adviseVideo, CategoryCourse categoryCourse)
        {
            Title = title;
            Description = description;
            Price = price;
            SubTitle = subTitle;
            Target = target;
            RequireSkill = requireSkill;
            Thumbnail = thumbnail;
            AdviseVideo = adviseVideo;
            CategoryCourse = categoryCourse;
        }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string SubTitle { get; set; } =  string.Empty;
        public string Target { get; set; } = string.Empty;
        public string RequireSkill { get; set; } =  string.Empty;
        public string Thumbnail { get; set; }
        public string AdviseVideo { get; set; }
        public CourseStatus Status { get; set; } = CourseStatus.ComingSoon;
        public CategoryCourse CategoryCourse { get; set; }
        public List<GroupLesson>? GroupLessons { get; set; }
    }
}
