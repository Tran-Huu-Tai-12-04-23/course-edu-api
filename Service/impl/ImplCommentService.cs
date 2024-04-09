using System.Collections;
using System.Diagnostics;
using course_edu_api.Data;
using course_edu_api.Data.RequestModels;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Entities;
using course_edu_api.Entities.Enum;
using course_edu_api.Helper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace course_edu_api.Service.impl;

public class ImplCommentService : ICommentService
{
    private readonly DataContext _context;

    public ImplCommentService(DataContext context)
    {
        _context = context;
    }


    public async Task<Comment> CreateComment(long postId, Comment comment)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null) 
            throw new Exception("Post not found!");

        if (comment.User == null)
            throw new Exception("Comment user is null!");

        var userExist = await _context.Users.FindAsync(comment.User.Id);
        if (userExist == null) 
            throw new Exception("User not found!");

        comment.User = userExist;
        post.Comments ??= new List<Comment>();
        post.Comments.Add(comment);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving comment to the database.", ex);
        }

        return comment;
    }


    public async Task<bool> RemoveComment(long CommentId)
    {
        try
        {
            var comment = await _context.Comments.FindAsync(CommentId);
            if (comment == null) throw new Exception("Comment not found!");
             _context.Comments.Remove(comment);
             await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Comment> UpdateComment(Comment comment)
    {
          _context.Comments.Update(comment);
          await _context.SaveChangesAsync();
          return comment;
    }

    public async Task<List<Comment>> GetAllCommentByPostId(long PostId)
    {
        try
        {
            var post = await _context.Posts.FindAsync(PostId);
            if (post == null) throw new Exception("Post not found!");
            return post.Comments;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}