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
        public async Task<bool> Create(T data) {
            bool inserted = await _db.insert<T>(data);
            return inserted;
        }

        public async Task<bool> Delete(Expression<Func<T, bool>> clause) {
            var filter = _db.getFilter<T>(clause);
            bool resp = await _db.delete<T>(filter);
            return resp;
        }

        public async Task<bool> Delete(T data) {
            Expression<Func<T, bool>> cond = F => F.Id == data.Id;
            return await Delete(cond);
        }

        public async Task<IEnumerable<T>> GetAll() {
            return await _db.select<T>();
        }

        public async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> clause) {
            var filter = _db.getFilter<T>(clause);
            return await _db.select<T>(filter);
        }

        public async Task<IEnumerable<T>> GetByCondition(IEnumerable<Expression<Func<T, bool>>> clause) {
            var filter = _db.getFilter<T>(clause);
            return await _db.select<T>(filter);
        }

        public async Task<bool> IsExist(Expression<Func<T, bool>> clause) {
            var filter = _db.getFilter<T>(clause);
            List<T> results = (List<T>)await _db.select<T>(filter);
            return results.Count > 0;
        }

        public async Task<bool> Update(object data, Expression<Func<T, bool>> clause) {
            bool resp = await _db.update<T>(clause, data);
            return resp;
        }
        public async Task<bool> Update(Dictionary<string, dynamic> data, Expression<Func<T, bool>> clause) {
            bool resp = await _db.update<T>(clause, data);
            return resp;
        }

        public async Task<bool> Update(T data) {
            Expression<Func<T, bool>> cond = F => F.Id == data.Id;
            return await Update(data, cond);
        }
    }
}
