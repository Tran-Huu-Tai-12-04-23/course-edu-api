using course_edu_api.Data;
using course_edu_api.Data.RequestModels;
using course_edu_api.Entities;

namespace course_edu_api.Service.impl;

public class ImplSubItemPostService : ISubItemPostService
{
    private readonly DataContext _context;

    public ImplSubItemPostService(DataContext context)
    {
        _context = context;
    }

    public async Task<SubItemPost?> CreateSubItemPost(SubItemPost? subItemPost)
    {
        _context.SubItemPosts.Add(subItemPost);
        await _context.SaveChangesAsync();
        return subItemPost;
    }

    public async Task<SubItemPost?> GetSubItemPostById(long id)
    {
        return await _context.SubItemPosts.FindAsync(id);
    }

    public async Task<bool> UpdateSubItemPost(long id, SubItemPost subItemPost)
    {
        var existingSubItemPost = await _context.SubItemPosts.FindAsync(id);
        if (existingSubItemPost == null)
            return false;

        existingSubItemPost.Type = subItemPost.Type;
        existingSubItemPost.Index = subItemPost.Index;
        existingSubItemPost.Content = subItemPost.Content;
        existingSubItemPost.Alt = subItemPost.Alt;
        existingSubItemPost.ImgURL = subItemPost.ImgURL;
        existingSubItemPost.Link = subItemPost.Link;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSubItemPost(long id)
    {
        var subItemPost = await _context.SubItemPosts.FindAsync(id);
        if (subItemPost == null)
            return false;

        _context.SubItemPosts.Remove(subItemPost);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<IEnumerable<SubItemPost>> CreateSubItemPosts(IEnumerable<SubItemPost> subItemPosts)
    {
        var itemPosts = subItemPosts as SubItemPost[] ?? subItemPosts.ToArray();
        _context.SubItemPosts.AddRange(itemPosts);
        await _context.SaveChangesAsync();
        return itemPosts;
    }
    
   
}