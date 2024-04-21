using course_edu_api.Data;
using course_edu_api.Data.RequestModels;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace course_edu_api.Service.impl;

public class ImplUserService : IUserService
{
    private readonly DataContext _context;

    public ImplUserService(DataContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUser(string email, string password, Role role)
    {
        var user = new User(email, password, role);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetUserById(long id)
    {
        return (await _context.Users
            .Include(u => u.UserSetting)  
            .FirstOrDefaultAsync(u => u.Id == id))!;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> UpdateUser(long id, User user)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
            return false;

        existingUser.Email = user.Email;
        existingUser.Password = user.Password;
        existingUser.Role = user.Role;
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Avatar = user.Avatar;
        existingUser.FullName = user.FullName;
        existingUser.Bio = user.Bio;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUser(long id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PaginatedResponse<User>> GetUserPagination(PaginationRequestDto<UserQueryDto> paginationRequestDto)
    {
        
        var skip = paginationRequestDto.PageNumber == 0 ? 0 : (paginationRequestDto.PageNumber - 1) * paginationRequestDto.PageSize;
        var take = paginationRequestDto.PageSize;
        var userQuery = paginationRequestDto.Where;
        IQueryable<User> query = _context.Users!;

        if (!String.IsNullOrEmpty(userQuery.Query))
        {
            query = query.Where(user =>
                (user.FirstName != null && user.FirstName.Contains(userQuery.Query)) ||
                (user.LastName != null && user.LastName.Contains(userQuery.Query)) ||
                user.Email.Contains(userQuery.Query) ||
                (user.FullName != null && user.FullName.Contains(userQuery.Query)) ||
                (user.Bio != null && user.Bio.Contains(userQuery.Query))
            );
        }

        query = query.OrderBy(user => user.Id);

        var paginatedData = await query.Skip(skip).Take(take).ToListAsync();

        return new PaginatedResponse<User>()
        {
            Data = paginatedData,
            PageNumber = paginationRequestDto.PageNumber,
            PageSize = paginationRequestDto.PageSize,
            TotalItems = await query.CountAsync() 
        };
    }

    public async Task<long> GetTotalUserByQuery(UserQueryDto userQueryDto)
    {
        var userQuery = userQueryDto;
        IQueryable<User> query = _context.Users!;
        if (!String.IsNullOrEmpty(userQuery.Query))
        {
            query = query.Where(user =>
                (user.FirstName != null && user.FirstName.Contains(userQuery.Query)) ||
                (user.LastName != null && user.LastName.Contains(userQuery.Query)) ||
                user.Email.Contains(userQuery.Query) ||
                (user.FullName != null && user.FullName.Contains(userQuery.Query)) ||
                (user.Bio != null && user.Bio.Contains(userQuery.Query))
            );
        }
        return await query.CountAsync();
    }
}