using Core.Domain.DTOs.Filter;
using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using Infrastructure.Repository.MongoDb;
using NetCore.AutoRegisterDi;
using System.Linq.Expressions;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    [RegisterAsScoped]
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository {
        public TransactionRepository(IMongoDbCommand db) : base(db) {
        }

        public async Task<Transaction?> get(string id) {
            Expression<Func<Transaction, bool>> cond = F => F.id == id;
            return ((List<Transaction>)await getByCondition(cond)).FirstOrDefault();
        }

        public async Task<List<Transaction>> get(TransactionFilter filter) {
            throw new NotImplementedException();
        }
    }
}
