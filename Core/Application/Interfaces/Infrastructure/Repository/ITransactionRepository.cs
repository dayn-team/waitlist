using Core.Domain.DTOs.Filter;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    public interface ITransactionRepository : IBaseRepository<Transaction> {
        Task<Transaction?> Get(string id);
        Task<List<Transaction>> Get(TransactionFilter filter);
    }
}
