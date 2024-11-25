using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using Infrastructure.Repository.MongoDb;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository {
        public TransactionRepository(IMongoDbCommand db) : base(db) {
        }
    }
}
