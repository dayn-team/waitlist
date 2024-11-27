using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    public interface IBaseRepository<T> where T : BaseEntity {
        Task<bool> create(T data);
        Task<bool> delete(Expression<Func<T, bool>> clause);
        Task<bool> delete(T data);
        Task<IEnumerable<T>> getAll();
        Task<IEnumerable<T>> getByCondition(Expression<Func<T, bool>> clause);
        Task<IEnumerable<T>> getByCondition(IEnumerable<Expression<Func<T, bool>>> clause);
        Task<bool> isExist(Expression<Func<T, bool>> clause);
        Task<bool> update(Dictionary<string, dynamic> data, Expression<Func<T, bool>> clause);
        Task<bool> update(T data);
        Task<bool> update(object data, Expression<Func<T, bool>> clause);
    }
}
