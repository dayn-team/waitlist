using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using Infrastructure.Repository.MongoDb;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    public class UserRepository : BaseRepository<User>, IUserRepository {
        public UserRepository(IMongoDbCommand db) : base(db) {
        }
    }
}
