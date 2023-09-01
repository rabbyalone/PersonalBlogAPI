using System.Linq.Expressions;

namespace Personal.Blog.Storage
{
    public interface IRepository<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(object id, T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(Expression<Func<T, bool>> filter);
        Task<T> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter);
        Task<List<IEnumerable<TResult>>> GetListProperty<TResult>(Expression<Func<T, IEnumerable<TResult>>> listPropertySelector, Expression<Func<T, bool>> filter);
    }
}
