using MongoDB.Bson;
using MongoDB.Driver;
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

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllAsync();
            posts.OrderByDescending(a => a.CreateDate).ToList().ForEach(a => a.Slug = a.Id);
            return posts;
        }

        public async Task<Post> GetPostByIdAsync(ObjectId postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            var posts = await GetAllPostsSummary();
            var currentPostIndex = posts.ToList().FindIndex(a => a.Id == post.Id);
            int previousIndex = currentPostIndex - 1;
            int nextIndex = currentPostIndex + 1;

            if (currentPostIndex > 0)
            {
                post.Previous = new PrevNext
                {
                    Title = posts.ElementAt(previousIndex).Title ?? "",
                    Id = posts.ElementAt(previousIndex).Id ?? "",
                };
            }

            if (nextIndex < posts.Count())
            {
                post.Next = new PrevNext
                {
                    Title = posts.ElementAt(nextIndex).Title ?? "",
                    Id = posts.ElementAt(nextIndex).Id ?? "",
                };
            }

            return post;
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(Expression<Func<Post, bool>> filter)
        {
            return await _postRepository.GetAsync(filter);
        }

        public async Task<Dictionary<string, int>> GetTagsAsync()
        {
            Expression<Func<Post, bool>> filter = u => u.Tags.Any();
            Expression<Func<Post, IEnumerable<string>>> listPropertySelector = u => u.Tags;

            var selectedTagsLists = await _postRepository.GetListProperty(listPropertySelector, filter);
            Dictionary<string, int> result = new();
            foreach (var item in selectedTagsLists)
            {
                foreach (var item2 in item)
                {
                    if (item2 is null) continue;

                    if (!result.Keys.Contains(item2.ToString()))
                    {
                        var tagCount = selectedTagsLists.Count(a => a.Contains(item2));
                        result.Add(item2.ToString(), tagCount);
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<Post>> GetAllPostsSummary()
        {
            Expression<Func<Post, bool>> filter = f => f.IsDraft == false;

            Expression<Func<Post, Post>> projection = doc =>
                new Post
                {
                    Summary = doc.Summary,
                    Title = doc.Title,
                    Id = doc.Id,
                    Tags = doc.Tags,
                    CreateDate = doc.CreateDate,
                    ModifiedDate = doc.ModifiedDate,
                };

            var posts = await _postRepository.GetSelectedPropertiesAsync(filter, projection);
            return posts.OrderByDescending(a => a.CreateDate);
        }

        public async Task InsertPostAsync(Post post)
        {
            post.Id = Guid.NewGuid().ToString("N").Substring(0, 24);
            post.Tags = new List<string>(post.Tags.ConvertAll(a => a.ToLower()));
            await _postRepository.InsertAsync(post);
        }

        public async Task UpdatePostAsync(ObjectId postId, Post post)
        {
            post.ModifiedDate = DateTime.UtcNow;
            post.Tags = new List<string>(post.Tags.ConvertAll(a => a.ToLower()));
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

    }

}
