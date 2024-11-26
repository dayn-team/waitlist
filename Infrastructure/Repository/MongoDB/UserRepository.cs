using Core.Domain.DTOs.Filter;
using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using Infrastructure.Repository.MongoDb;
using NetCore.AutoRegisterDi;
using System.Linq.Expressions;

namespace Core.Application.Interfaces.Infrastructure.Repository {
    [RegisterAsScoped]
    public class UserRepository : BaseRepository<User>, IUserRepository {
        private readonly IMongoDbCommand _db;
        public UserRepository(IMongoDbCommand db) : base(db) {
            _db = db;
        }

        public async Task<bool> accountExists(AccountFilter filter) {
            List<Expression<Func<User, bool>>> clauses = getCombinedClauses(filter);
            var filterOnj = _db.getFilter<User>(clauses);
            var findops = _db.getFindOptions<User>(1);
            var data = await _db.select<User>(filterOnj, findops);
            return data.Any();
        }

        private List<Expression<Func<User, bool>>> getCombinedClauses(AccountFilter filter) {
            List<Expression<Func<User, bool>>> clauses = new List<Expression<Func<User, bool>>>();
            Expression<Func<User, bool>> cond = null;
            if (filter.fieldIsSet(nameof(filter.username))) {
                cond = H => H.username == filter.username;
                clauses.Add(cond);
            }

            if (filter.fieldIsSet(nameof(filter.password))) {
                cond = H => H.password == filter.password;
                clauses.Add(cond);
            }

            if (filter.fieldIsSet(nameof(filter.phone))) {
                cond = H => H.phone == filter.phone;
                clauses.Add(cond);
            }
            if (filter.fieldIsSet(nameof(filter.email))) {
                cond = H => H.email == filter.email;
                clauses.Add(cond);
            }
            if (filter.fieldIsSet(nameof(filter.externalID))) {
                cond = H => H.id == filter.externalID;
                clauses.Add(cond);
            }
            if (filter.fieldIsSet(nameof(filter.publicKey))) {
                cond = H => H.publicKey == filter.publicKey;
                clauses.Add(cond);
            }
            if (filter.fieldIsSet(nameof(filter.type))) {
                cond = H => H.type == filter.type;
                clauses.Add(cond);
            }
            if (filter.fieldIsSet(nameof(filter.privilege))) {
                cond = H => H.privilege == filter.privilege;
                clauses.Add(cond);
            }
            return clauses;
        }

        public async Task<List<User>> get(AccountFilter filter) {
            return (List<User>)(await getByCondition(getCombinedClauses(filter)));
        }

        public async Task<bool> loginUpdate(User user) {
            var updateObj = new { user.publicKey, user.ip, user.lastLogin, user.useragent };
            Expression<Func<User, bool>> cond = F => F.id == user.id;
            return await update(updateObj, cond);
        }
        public async Task<bool> updatePassword(string password, string username, int passwordChanged = 1) {
            var updateObj = new { password, passwordChanged };
            Expression<Func<User, bool>> cond = F => F.username.ToLower() == username.ToLower();
            return await update(updateObj, cond);
        }
    }
}
