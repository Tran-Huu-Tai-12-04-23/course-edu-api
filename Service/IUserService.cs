using course_edu_api.Entities;

namespace course_edu_api.Service;

public interface IUserService
{
    Task<User> CreateUser(string email, string password, Role role);
    Task<User> GetUserById(long id);
    Task<IEnumerable<User>> GetAllUsers();
    Task<bool> UpdateUser(long id, User user);
    Task<bool> DeleteUser(long id);
}