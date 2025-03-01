using Core.Application.Interfaces.Infrastructure.Repository;
using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using Infrastructure.Repository.MongoDb;
using NetCore.AutoRegisterDi;
using System.Linq.Expressions;

namespace Infrastructure.Repository.MongoDB {
    [RegisterAsScoped]
    public class WaitlistRepository : BaseRepository<Waitlist>, IWaitlistRepository {
        public WaitlistRepository(IMongoDbCommand db) : base(db) {
        }
        public async Task<bool> MailExists(string email) {
            Expression<Func<Waitlist, bool>> filter = F => F.Email == email;
            return (await getByCondition(filter)).Any();
        }
    }
}
