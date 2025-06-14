using Core.Application.Interfaces.Infrastructure.Repository;
using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using Infrastructure.Repository.MongoDb;
using NetCore.AutoRegisterDi;

namespace Infrastructure.Repository.MongoDB {
    [RegisterAsScoped]
    public class RepaymentRepository : BaseRepository<Repayment>, IRepaymentRepository {
        public RepaymentRepository(IMongoDbCommand db) : base(db) {
        }
    }
}
