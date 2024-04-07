using course_edu_api.Data.RequestModels;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Entities;

namespace course_edu_api.Service;

public interface IPostService
{
    Task<Post?> CreatePost(Post post);
    Task<Post?> GetPostById(long id);
    Task<List<Post?>> GetAllPosts();
    Task<List<Post?>> GetAllBlogByUserId(long userId);
    Task<bool> DeletePost(long id);
    Task<Post> AddSubItemToPost(long postId, SubItemPost subItem);
    Task<PaginatedResponse<Post>> GetPostPagination(PaginationRequestDto<PostQueryDto> paginationRequestDto);
    Task<long> GetTotalPost(PostQueryDto postQueryDto);
    Task<List<Post>> GetPostApproved();
    Task<Post> ApprovePost(long postId);
    Task<Post> RejectPost(long postId);

}