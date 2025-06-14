using Core.Application.Interfaces.Infrastructure.Repository;
using Core.Domain.DTOs.Filter;
using Core.Domain.Entities;
using Infrastructure.Abstraction.Database.MongoDb;
using Infrastructure.Repository.MongoDb;
using NetCore.AutoRegisterDi;
using System.Linq.Expressions;

namespace Infrastructure.Repository.MongoDB {
    [RegisterAsScoped]
    public class UserRepository : BaseRepository<User>, IUserRepository {
        private readonly IMongoDbCommand _db;
        public UserRepository(IMongoDbCommand db) : base(db) {
            _db = db;
        }

        public async Task<bool> AccountExists(AccountFilter filter) {
            List<Expression<Func<User, bool>>> clauses = getCombinedClauses(filter);
            var filterOnj = _db.getFilter(clauses);
            var findops = _db.getFindOptions<User>(1);
            var data = await _db.select(filterOnj, findops);
            return data.Any();
        }

        private List<Expression<Func<User, bool>>> getCombinedClauses(AccountFilter filter) {
            List<Expression<Func<User, bool>>> clauses = new List<Expression<Func<User, bool>>>();
            Expression<Func<User, bool>> cond = null;
            if (filter.fieldIsSet(nameof(filter.username))) {
                cond = H => H.Username == filter.username;
                clauses.Add(cond);
            }

            if (filter.fieldIsSet(nameof(filter.password))) {
                cond = H => H.Password == filter.password;
                clauses.Add(cond);
            }

            if (filter.fieldIsSet(nameof(filter.phone))) {
                cond = H => H.Phone == filter.phone;
                clauses.Add(cond);
            }
            if (filter.fieldIsSet(nameof(filter.email))) {
                cond = H => H.Email == filter.email;
                clauses.Add(cond);
            }
            if (filter.fieldIsSet(nameof(filter.externalID))) {
                cond = H => H.Id == filter.externalID;
                clauses.Add(cond);
            }
            if (filter.fieldIsSet(nameof(filter.publicKey))) {
                cond = H => H.PublicKey == filter.publicKey;
                clauses.Add(cond);
            }
            if (filter.fieldIsSet(nameof(filter.type))) {
                cond = H => H.Type == filter.type;
                clauses.Add(cond);
            }
            if (filter.fieldIsSet(nameof(filter.privilege))) {
                cond = H => H.Privilege == filter.privilege;
                clauses.Add(cond);
            }
            return clauses;
        }

        public async Task<List<User>> Get(AccountFilter filter) {
            return (List<User>)await GetByCondition(getCombinedClauses(filter));
        }

        public async Task<bool> LoginUpdate(User user) {
            var updateObj = new { user.PublicKey, user.Ip, user.LastLogin, user.Useragent };
            Expression<Func<User, bool>> cond = F => F.Id == user.Id;
            return await Update(updateObj, cond);
        }

        public async Task<bool> UpdatePassword(string password, string username, int passwordChanged = 1) {
            var updateObj = new { password, passwordChanged };
            Expression<Func<User, bool>> cond = F => F.Username.ToLower() == username.ToLower();
            return await Update(updateObj, cond);
        }
    }
}
