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

        public PostController(IPostService postService, 
            ISubItemPostService subItemPostService,
                IUserService userService
            )
        {
            _postService = postService;
            _subItemPostService = subItemPostService;
            _userService =userService;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(long id)
        {
            var post = await this._postService.GetPostById(id);

            if (post == null)
            {
                return NotFound("Post not found!");
            }

            return Ok(post);
        }

        /*[Authorize]*/
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(PostRequest post)
        {
            Console.Write(post.Title);
            post.StatePost = PostStatus.Pending.ToString();
            User user = await _userService.GetUserById(post.UserId);
            var newSubItemPosts = await _subItemPostService.CreateSubItemPosts(post.Items!);

            Post newPost = new Post
            {
                User = user,
                Status = PostStatus.Pending,
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

       
    }
}
