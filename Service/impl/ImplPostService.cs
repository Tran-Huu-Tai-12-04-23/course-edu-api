using course_edu_api.Data;
using course_edu_api.Data.RequestModels;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Entities;
using course_edu_api.Entities.Enum;
using Microsoft.EntityFrameworkCore;

namespace course_edu_api.Service.impl;

public class ImplPostService : IPostService
{
    private readonly DataContext _context;

    public ImplPostService(DataContext context)
    {
        _context = context;
    }

    public async Task<Post?> CreatePost(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<Post?> GetPostById(long id)
    {
        return await _context.Posts
            .Include(p => p.Items) 
            .Include(p => p.User) 
            .FirstOrDefaultAsync(p => p.Id == id); 
    }

    public async Task<List<Post?>> GetAllPosts()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<List<Post?>> GetAllBlogByUserId(long userId)
    {
        return await _context.Posts
            .Where(po => po!.User.Id == userId)
            .ToListAsync();
    }

    public async Task<bool> DeletePost(long id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
            return false;
            
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Post> AddSubItemToPost(long postId, SubItemPost subItem)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
            throw new ArgumentException("Post not found");

        if (post.Items == null)
            post.Items = new List<SubItemPost>();

        post.Items.Add(subItem);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<PaginatedResponse<Post>> GetPostPagination(PaginationRequestDto<PostQueryDto> paginationRequestDto)
    {
        var skip = (paginationRequestDto.PageNumber - 1) * paginationRequestDto.PageSize;
        var take = paginationRequestDto.PageSize;
        var postQuery = paginationRequestDto.Where;
        IQueryable<Post> query = _context.Posts!;
        
        if (!String.IsNullOrEmpty(postQuery.Query))
        {
            query = query.Where(p => p.Title!.Contains(postQuery.Query) || p.Description!.Contains(postQuery.Query) );
        }

        if (postQuery.Status != null )
        {
            var statusValue = (PostStatus)postQuery.Status;
            query = query.Where(p => p.Status == statusValue );
        }
        
        if (postQuery.Tags != null )
        {
            query = query.Where(p => p.Tags!.Contains(postQuery.Tags) );
        }

        var paginatedData = await query.Skip(skip).Take(take).ToListAsync();

        return new PaginatedResponse<Post>()
        {
            Data = paginatedData,
            PageNumber = paginationRequestDto.PageNumber,
            PageSize = paginationRequestDto.PageSize,
            TotalItems = paginatedData.Count
        };
    }

    public async Task<long> GetTotalPost(PostQueryDto postQueryDto)
    {
        IQueryable<Post> query = _context.Posts!;
        
        if (!String.IsNullOrEmpty(postQueryDto.Query))
        {
            query = query.Where(p => p.Title!.Contains(postQueryDto.Query) || p.Description!.Contains(postQueryDto.Query) );
        }

        if (postQueryDto.Status != null )
        {
            var statusValue = (PostStatus)postQueryDto.Status;
            query = query.Where(p => p.Status == statusValue );
        }
        
        if (postQueryDto.Tags != null )
        {
            query = query.Where(p => p.Tags!.Contains(postQueryDto.Tags) );
        }
        return await query.CountAsync();
    }

    public async Task<List<Post>> GetPostApproved()
    {
        return await _context.Posts
            .Include(post => post.User)
            .Where(post => post.IsApproved == true).ToListAsync();
    }

    public async Task<Post> ApprovePost(long postId)
    {
        var postExist = await _context.Posts.FindAsync(postId);
        if (postExist == null) throw new Exception("Bài viết không tồn tại!");

        postExist.IsApproved = true;
        postExist.Status = PostStatus.Published;
        postExist.ApproveDate = new DateTime();
        await _context.SaveChangesAsync();

        return postExist;
    }

    public async Task<Post> RejectPost(long postId)
    {
        var postExist = await _context.Posts.FindAsync(postId);
        if (postExist == null) throw new Exception("Bài viết không tồn tại!");

        postExist.IsApproved = false;
        postExist.Status = PostStatus.REJECT;
        postExist.ApproveDate = new DateTime();
        await _context.SaveChangesAsync();

        return postExist;
    }
}