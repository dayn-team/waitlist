using MongoDB.Driver;
using System.Linq.Expressions;

namespace Infrastructure.Abstraction.Database.MongoDb
{
    public interface IMongoDbCommand
    {
        Task<bool> delete<T>(FilterDefinition<T> filter);
        FilterDefinition<T> getFilter<T>(Expression<Func<T, bool>> condition, string? textsearch = null);
        FilterDefinition<T> getFilter<T>(string textsearch);
        FilterDefinition<T> getFilter<T>(IEnumerable<Expression<Func<T, bool>>> condition, string? textsearch = null);
        FindOptions<T> getFindOptions<T>(int limit, Dictionary<string, int> sorting);
        FindOptions<T> getFindOptions<T>(int limit);
        FindOptions<T> getFindOptions<T>(Dictionary<string, int> sorting);
        Task<bool> insert<T>(T data);
        Task<IEnumerable<T>> select<T>();
        Task<IEnumerable<T>> select<T>(FilterDefinition<T> filter);
        Task<IEnumerable<T>> select<T>(FilterDefinition<T> filter, FindOptions<T> options);
        Task<IEnumerable<T>> select<T>(FindOptions<T> filter);
        Task<bool> update<T>(FilterDefinition<T> filter, object updateField);
        Task<bool> update<T>(FilterDefinition<T> filter, Dictionary<string, dynamic> updateField);
        Task<bool> updateIncremental<T>(FilterDefinition<T> filter, object updateField);
        Task<long> count<T>(FilterDefinition<T> filter);
        IMongoDatabase _dataStore { get; }
        Task<List<T>> select<T>(FilterDefinition<T> filter, int limit, int page);
        Task startTransaction();
        Task commitTransaction();
    }
}
