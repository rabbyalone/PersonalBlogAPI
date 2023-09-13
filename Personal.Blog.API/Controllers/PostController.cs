
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Personal.Blog.Application.Services;
using Personal.Blog.Domain.Entities;

namespace Personal.Blog.API.Controllers
{

    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICacheService _cacheService;

        public PostController(IPostService postService, ICacheService cacheService)
        {
            _postService = postService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {

            var cachedPosts = await _cacheService.GetOrSetAsync(
                    "all_posts",
                     () => _postService.GetAllPostsSummary(),
                    TimeSpan.FromDays(30)
);

            if (cachedPosts == null || !cachedPosts.Any())
            {
                return NotFound("No posts found.");
            }

            return Ok(cachedPosts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(string id)
        {
            if (!ObjectId.TryParse(id, out var postId))
                return BadRequest("Invalid ID format");

            var cachedPost = await _cacheService.GetOrSetAsync(
                    $"post_{postId}",
                    () => _postService.GetPostByIdAsync(postId),
                    TimeSpan.FromMinutes(10)
                );

            if (cachedPost == null)
            {
                return NotFound($"Post with ID {postId} not found.");
            }

            return Ok(cachedPost);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPosts([FromQuery] string query)
        {
            var filteredPosts = await _postService.GetPostsAsync(p => p.Title.Contains(query) || p.Content.Contains(query));
            return Ok(filteredPosts);
        }

        [HttpGet("postbytag")]
        public async Task<IActionResult> TaggedPosts([FromQuery] string tag)
        {
            var filteredPosts = await _postService.GetPostsAsync(p => p.Tags.Contains(tag));
            return Ok(filteredPosts);
        }


        [HttpGet("tags")]
        public async Task<IActionResult> GetTags()
        {
            var cachedTags = await _cacheService.GetOrSetAsync(
                    "tags",
                     () => _postService.GetTagsAsync(),
                    TimeSpan.FromMinutes(10)
                        );

            if (cachedTags == null || !cachedTags.Any())
            {
                return NotFound("No tags found.");
            }

            return Ok(cachedTags);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            post.Tags.ForEach(a => a.ToLower());
            await _postService.InsertPostAsync(post);
            await _cacheService.CreateOrUpdateAsync($"post{post.Id}", post, TimeSpan.FromMinutes(10));
            await _cacheService.DeleteAsync("all_posts");
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(string id, [FromBody] Post post)
        {
            if (!ObjectId.TryParse(id, out var postId))
                return BadRequest("Invalid ID format");

            var existingPost = await _postService.GetPostByIdAsync(postId);
            if (existingPost == null)
                return NotFound();

            existingPost.Title = post.Title;
            existingPost.Content = post.Content;

            await _postService.UpdatePostAsync(postId, existingPost);
            await _cacheService.CreateOrUpdateAsync($"post{post.Id}", post, TimeSpan.FromMinutes(10));
            await _cacheService.DeleteAsync("all_posts");
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            if (!ObjectId.TryParse(id, out var postId))
                return BadRequest("Invalid ID format");

            await _postService.DeletePostAsync(postId);
            await _cacheService.DeleteAsync($"post{postId}");
            return NoContent();
        }

        [HttpGet("check")]
        public async Task<IActionResult> Check()
        {
            return Ok("Check Success");
        }


    }

}
