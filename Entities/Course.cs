namespace course_edu_api.Entities
{
    public class Course
    {
        public Course(string title, string description, double price, string thumbnails, string adviseVideo)
        {
            Title = title;
            Description = description;
            Price = price;
            Thumbnails = thumbnails;
            AdviseVideo = adviseVideo;
        }

        public Course()
        {
            this.Title = string.Empty;
            this.Description = string.Empty;
            this.Price = 0;
            this.AdviseVideo = "";
            this.Thumbnails = "";
        }

        public Course(string title, string description, double price, string thumbnails, string adviseVideo, TypeCourse typeCourse)
        {
            Title = title;
            Description = description;
            Price = price;
            Thumbnails = thumbnails;
            AdviseVideo = adviseVideo;
            TypeCourse = typeCourse;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Thumbnails { get; set; }
        public string AdviseVideo { get; set; }
        
        public TypeCourse TypeCourse { get; set; }
    }
}
