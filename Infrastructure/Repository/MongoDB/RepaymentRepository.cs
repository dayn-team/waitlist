using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using Infrastructure.Repository.MongoDb;
using NetCore.AutoRegisterDi;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    [RegisterAsScoped]
    public class RepaymentRepository : BaseRepository<Repayment>, IRepaymentRepository {
        public RepaymentRepository(IMongoDbCommand db) : base(db) {
        }
    }
}
