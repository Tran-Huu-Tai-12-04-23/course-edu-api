namespace course_edu_api.Entities
{
    public class User
    {
        public User(int id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public User()
        {
        }

        public int Id { get; set; }

        public string Email{ get; set; } = string.Empty;
        public string Password { get; set; } =  string.Empty;
    }
}
