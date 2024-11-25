using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using Infrastructure.Repository.MongoDb;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    public class RepaymentRepository : BaseRepository<Repayment>, IRepaymentRepository {
        public RepaymentRepository(IMongoDbCommand db) : base(db) {
        }
    }
}
