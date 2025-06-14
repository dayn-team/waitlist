using Core.Application.Interfaces.Infrastructure.Repository;
using Core.Domain.DTOs.Filter;
using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using Infrastructure.Repository.MongoDb;
using NetCore.AutoRegisterDi;
using System.Linq.Expressions;

namespace Infrastructure.Repository.MongoDB {
    [RegisterAsScoped]
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository {
        public TransactionRepository(IMongoDbCommand db) : base(db) {
        }

        public async Task<Transaction?> Get(string id) {
            Expression<Func<Transaction, bool>> cond = F => F.Id == id;
            return ((List<Transaction>)await GetByCondition(cond)).FirstOrDefault();
        }

        public async Task<List<Transaction>> Get(TransactionFilter filter) {
            throw new NotImplementedException();
        }
    }
}
