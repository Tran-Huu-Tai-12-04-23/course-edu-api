using course_edu_api.Entities;

namespace course_edu_api.Service;

public interface ICommentService
{
    Task<Comment> CreateComment(long postId, Comment comment);
    Task<bool> RemoveComment(long CommentId);
    Task<Comment> UpdateComment(Comment comment);
    Task<List<Comment>> GetAllCommentByPostId(long PostId);
}