
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

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(string id)
        {
            if (!ObjectId.TryParse(id, out var postId))
                return BadRequest("Invalid ID format");

            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();

            return Ok(post);
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
            var tags = await _postService.GetTagsAsync();
            return Ok(tags);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            await _postService.InsertPostAsync(post);
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }


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

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            if (!ObjectId.TryParse(id, out var postId))
                return BadRequest("Invalid ID format");

            await _postService.DeletePostAsync(postId);
            return NoContent();
        }


    }

}
