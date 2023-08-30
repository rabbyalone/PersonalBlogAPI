using MongoDB.Bson;
using Personal.Blog.Domain.Entities;
using System.Linq.Expressions;

namespace Personal.Blog.Application.Services
{
    public interface IPostService
    {
        Task InsertPostAsync(Post post);
        Task UpdatePostAsync(ObjectId postId, Post post);
        Task DeletePostAsync(ObjectId postId);
        Task<Post> GetPostByIdAsync(ObjectId postId);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<IEnumerable<Post>> GetPostsAsync(Expression<Func<Post, bool>> filter);
    }

}
