using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    public interface IBaseRepository<T> where T : BaseEntity {
        Task<bool> Create(T data);
        Task<bool> Delete(Expression<Func<T, bool>> clause);
        Task<bool> Delete(T data);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> clause);
        Task<IEnumerable<T>> GetByCondition(IEnumerable<Expression<Func<T, bool>>> clause);
        Task<bool> IsExist(Expression<Func<T, bool>> clause);
        Task<bool> Update(Dictionary<string, dynamic> data, Expression<Func<T, bool>> clause);
        Task<bool> Update(T data);
        Task<bool> Update(object data, Expression<Func<T, bool>> clause);
    }
}
