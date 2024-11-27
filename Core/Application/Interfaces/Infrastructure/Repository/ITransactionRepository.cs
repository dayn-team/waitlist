using Core.Domain.DTOs.Filter;
using Core.Domain.Entities;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    public interface ITransactionRepository : IBaseRepository<Transaction> {
        Task<Transaction?> get(string id);
        Task<List<Transaction>> get(TransactionFilter filter);
    }
}
