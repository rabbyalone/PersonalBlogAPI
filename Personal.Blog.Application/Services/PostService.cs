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

        public async Task UpdatePostAsync(Post post)
        {
            await _postRepository.UpdateAsync(post);
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
            return await _postRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(Expression<Func<Post, bool>> filter)
        {
            return await _postRepository.GetAsync(filter);
        }
    }

}
