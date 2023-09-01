using MongoDB.Bson;
using Personal.Blog.Domain.Entities;
using Personal.Blog.Storage;
using System.Linq.Expressions;

namespace Personal.Blog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _postRepository;

        public PostService(IRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task InsertPostAsync(Post post)
        {
            post.Id = Guid.NewGuid().ToString("N").Substring(0, 24);
            await _postRepository.InsertAsync(post);
        }

        public async Task UpdatePostAsync(ObjectId postId, Post post)
        {
            post.ModifiedDate = DateTime.UtcNow;
            await _postRepository.UpdateAsync(postId, post);
        }

        public async Task DeletePostAsync(ObjectId postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post != null)
            {
                await _postRepository.DeleteAsync(post);
            }
        }

        public async Task<Post> GetPostByIdAsync(ObjectId postId)
        {
            return await _postRepository.GetByIdAsync(postId);
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllAsync();
            posts.OrderByDescending(a => a.CreateDate).ToList().ForEach(a => a.Slug = a.Id);
            return posts;
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(Expression<Func<Post, bool>> filter)
        {
            return await _postRepository.GetAsync(filter);
        }

        public async Task<IEnumerable<string>> GetTagsAsync()
        {
            Expression<Func<Post, bool>> filter = u => u.Tags.Any();
            Expression<Func<Post, IEnumerable<string>>> listPropertySelector = u => u.Tags;

            var selectedTagsLists = await _postRepository.GetListProperty(listPropertySelector, filter);
            return selectedTagsLists;
        }
    }

}
