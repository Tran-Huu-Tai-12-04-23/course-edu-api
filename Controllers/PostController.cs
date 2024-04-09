using System.Diagnostics;
using System.Net;
using course_edu_api.Data;
using course_edu_api.Data.RequestModels;
using course_edu_api.Entities;
using course_edu_api.Entities.Enum;
using course_edu_api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace course_edu_api.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ISubItemPostService _subItemPostService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;

        public PostController(IPostService postService, 
            ISubItemPostService subItemPostService,
            IUserService userService,
            ICommentService commentService
            )
        {
            _postService = postService;
            _subItemPostService = subItemPostService;
            _userService =userService;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAllPost()
        {
            var posts = await this._postService.GetAllPosts();
            return Ok(posts);
        }
        
        [HttpPost("pagination")]
        public async Task<ActionResult> GetPostPagination(PaginationRequestDto<PostQueryDto> paginationRequestDto)
        {
            var posts = await this._postService.GetPostPagination(paginationRequestDto);
            return Ok(posts);
        }
        
        [HttpPost("count")]
        public async Task<ActionResult> CountTotalPost(PostQueryDto postQueryDto)
        {
            var posts = await this._postService.GetTotalPost(postQueryDto);
            return Ok(posts);
        }
        
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Post>>> GetAllPostByUserId(long userId)
        {
            var posts = await this._postService.GetAllBlogByUserId(userId);
            return Ok(posts);
        }

        /*[Authorize]*/
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(PostRequest post)
        {
            Console.Write(post.Title);
            post.StatePost = PostStatus.WAIT_APPROVE.ToString();
            User user = await _userService.GetUserById(post.UserId);
            var newSubItemPosts = await _subItemPostService.CreateSubItemPosts(post.Items!);

            Post newPost = new Post
            {
                User = user,
                Status = PostStatus.WAIT_APPROVE,
                Description = post.Description,
                Tags = post.Tags,
                Thumbnail = post.Thumbnail,
                Title = post.Title,
                Items = newSubItemPosts.Where(item => item.Id != -1).ToList()
            };

            await _postService.CreatePost(newPost);
            return Ok(post);
        }

        /*[HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, Post updatedPost)
        {
            if (id != updatedPost.Id)
            {
                return BadRequest();
            }

            this._postService.
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Update successfully!");
        }*/

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(long id)
        {
            var post = await this._postService.GetPostById(id);

            if (post == null)
            {
                
                return Ok(false);
            }

            bool isOk = await this._postService.DeletePost(id);

            return Ok(isOk);
        }
        
        [HttpGet("approve")]
        public async Task<IActionResult> GetAllPostApprove()
        {
            var post = await this._postService.GetPostApproved();
            return Ok(post);
        }
        
        [HttpPut("approve-post/{postId}")]
        public async Task<IActionResult> ApprovePost([FromRoute]long postId)
        {
            var post = await this._postService.ApprovePost(postId);
            return Ok(post);
        }
        
        [HttpPut("reject-post/{postId}")]
        public async Task<IActionResult> RejectPost([FromRoute]long postId)
        {
            var post = await this._postService.RejectPost(postId);
            return Ok(post);
        }
        
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetDetailPost([FromRoute]long postId)
        {
            try
            {
                var post = await this._postService.GetPostById(postId);
                return Ok(post);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpPost("comment/{postId}")]
        public async Task<IActionResult> AddComment([FromRoute] long postId, [FromBody]Comment comment)
        {
            try
            {
                var post = await this._commentService.CreateComment(postId, comment);
                return Ok(post);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        
        [HttpPut("update-comment")]
        public async Task<IActionResult> UpdateComment([FromBody]Comment comment)
        {
            try
            {
                var post = await this._commentService.UpdateComment(comment);
                return Ok(post);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpDelete("remove-comment/{commentId}")]
        public async Task<IActionResult> RemoveComment([FromRoute]long commentId)
        {
            try
            {
                var post = await this._commentService.RemoveComment(commentId);
                return Ok(post);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpGet("comment/{postId}")]
        public async Task<IActionResult> GetAllCommentPost([FromRoute]long postId)
        {
            try
            {
                var post = await this._commentService.GetAllCommentByPostId(postId);
                return Ok(post);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
