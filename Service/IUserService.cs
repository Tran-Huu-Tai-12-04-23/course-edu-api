using course_edu_api.Data.RequestModels;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Entities;

namespace course_edu_api.Service;

public interface IUserService
{
    Task<User> CreateUser(string email, string password, Role role);
    Task<User> GetUserById(long id);
    Task<IEnumerable<User>> GetAllUsers();
    Task<bool> UpdateUser(long id, User user);
    Task<bool> DeleteUser(long id);
    Task<PaginatedResponse<User>> GetUserPagination(PaginationRequestDto<UserQueryDto> paginationRequestDto);
    
    Task<long> GetTotalUserByQuery(UserQueryDto userQueryDto);
}