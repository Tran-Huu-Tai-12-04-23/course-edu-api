using course_edu_api.Data.RequestModels;
using course_edu_api.Entities;

namespace course_edu_api.Service;

public interface ISubItemPostService
{
    Task<SubItemPost?> CreateSubItemPost(SubItemPost? subItemPost);
    Task<SubItemPost?> GetSubItemPostById(long id);
    Task<bool> UpdateSubItemPost(long id, SubItemPost subItemPost);
    Task<bool> DeleteSubItemPost(long id);
    Task<IEnumerable<SubItemPost>> CreateSubItemPosts(IEnumerable<SubItemPost> subItemPosts);
}