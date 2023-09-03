namespace Personal.Blog.Application.Services
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, TimeSpan expirationTime);
        Task CreateOrUpdateAsync<T>(string key, T newItem, TimeSpan expirationTime);
        Task DeleteAsync(string key);
    }
}
