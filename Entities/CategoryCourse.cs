namespace course_edu_api.Entities
{
    public class CategoryCourse
    {
        public CategoryCourse()
        {
            this.CategoryName = string.Empty;
            IsLock = false;
        }
        public CategoryCourse(string typeName)
        {
            CategoryName = typeName;
            IsLock = false;
        }
        
        public CategoryCourse(long id, string typeName)
        {
            this.Id = id;
            this.CategoryName = typeName;
            IsLock = false;
        }

        public long Id { get; set; }
        public string CategoryName { get; set; }
        public bool IsLock { get; set; }
    }
}

