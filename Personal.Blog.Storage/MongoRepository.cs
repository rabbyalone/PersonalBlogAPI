using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Personal.Blog.Domain.ConfigModels;
using System.Linq.Expressions;

namespace Personal.Blog.Storage
{
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IOptions<BlogDbSettings> blogSettings)
        {
            var client = new MongoClient(blogSettings.Value.MongoConnection);
            var database = client.GetDatabase(blogSettings.Value.DatabaseName);
            _collection = database.GetCollection<T>(blogSettings.Value.PostCollectionName);
        }

        public async Task InsertAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(object id, T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", GetId(entity));
            await _collection.DeleteOneAsync(filter);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> filter)
        {
            await _collection.DeleteManyAsync(filter);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetRecentPostAsync()
        {
            //var filter = Builders<T>.sor;
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }

        private object GetId(T entity)
        {
            var property = typeof(T).GetProperty("Id");
            if (property == null)
                throw new ArgumentException("The entity must have a property named 'Id'.");

            return property.GetValue(entity);
        }

        public async Task<List<IEnumerable<TResult>>> GetListProperty<TResult>(Expression<Func<T, IEnumerable<TResult>>> listPropertySelector, Expression<Func<T, bool>> filter)
        {
            var list = await _collection.Find(filter).Project(listPropertySelector).ToListAsync();
            return list;

        }
    }
}
