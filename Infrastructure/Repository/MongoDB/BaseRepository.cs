using Core.Application.Interfaces.Infrastructure.Repository;
using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using System.Linq.Expressions;

namespace Infrastructure.Repository.MongoDb {

    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity {
        private IMongoDbCommand _db;
        public BaseRepository(IMongoDbCommand db) {
            this._db = db;
        }
        public async Task<bool> create(T data) {
            bool inserted = await _db.insert<T>(data);
            return inserted;
        }

        public async Task<bool> delete(Expression<Func<T, bool>> clause) {
            var filter = _db.getFilter<T>(clause);
            bool resp = await _db.delete<T>(filter);
            return resp;
        }

        public async Task<bool> delete(T data) {
            Expression<Func<T, bool>> cond = F => F.id == data.id;
            return await delete(cond);
        }

        public async Task<IEnumerable<T>> getAll() {
            return await _db.select<T>();
        }

        public async Task<IEnumerable<T>> getByCondition(Expression<Func<T, bool>> clause) {
            var filter = _db.getFilter<T>(clause);
            return await _db.select<T>(filter);
        }

        public async Task<IEnumerable<T>> getByCondition(IEnumerable<Expression<Func<T, bool>>> clause) {
            var filter = _db.getFilter<T>(clause);
            return await _db.select<T>(filter);
        }

        public async Task<bool> isExist(Expression<Func<T, bool>> clause) {
            var filter = _db.getFilter<T>(clause);
            List<T> results = (List<T>)await _db.select<T>(filter);
            return results.Count > 0;
        }

        public async Task<bool> update(object data, Expression<Func<T, bool>> clause) {
            bool resp = await _db.update<T>(clause, data);
            return resp;
        }
        public async Task<bool> update(Dictionary<string, dynamic> data, Expression<Func<T, bool>> clause) {
            bool resp = await _db.update<T>(clause, data);
            return resp;
        }

        public async Task<bool> update(T data) {
            Expression<Func<T, bool>> cond = F => F.id == data.id;
            return await update(data, cond);
        }
    }
}
