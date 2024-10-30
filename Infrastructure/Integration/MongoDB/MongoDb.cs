using Core.Domain.Attributes;
using Core.Domain.DTOs.Configurations;
using Infrastructure.Abstraction.Database.MongoDb;
using LinqKit;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NetCore.AutoRegisterDi;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Infrastructure.Integration.MongoDB {
    [RegisterAsScoped]
    public class MongoDB : IMongoDbCommand, IDisposable {
        public IMongoDatabase _dataStore { get; private set; }
        private MongoClient _client;
        private DBConfig _config;
        private IClientSessionHandle _session;
        public Database _cosmosDB { get; private set; }
        private string _connectionString;

        public MongoDB(IOptionsMonitor<SystemVariables> config) {
            configure(config.CurrentValue.MongoDB);
        }
        public MongoDB(DBConfig config) {
            configure(config);
        }

        private void configure(DBConfig config) {
            _config = config;
            _connectionString = getConnectionString(config);
            _client = new MongoClient(_connectionString);
            _connect();
            //initializeCosmosInstance();
        }
        private string getConnectionString(DBConfig config) {
            if (!string.IsNullOrEmpty(_connectionString))
                return _connectionString;
            string server = string.Concat(config.server);
            server = !string.IsNullOrEmpty(config.username) ? string.Concat(config.username, ":", config.password, "@", server) : server;
            string protocol = config.protocol ?? "mongodb://";
            _connectionString = string.Concat(protocol, server);
            if (!string.IsNullOrEmpty(config.authMechanism)) {
                _connectionString += $"/?authMechanism={config.authMechanism}";
            }
            return _connectionString;
        }

        private bool _connect() {
            _dataStore = _dataStore ?? _client.GetDatabase(_config.database);
            return true;
        }


        public async Task<bool> createUniqueIndexes<T>(IMongoCollection<T> collection) {
            var uniqueStringIndexProperties = typeof(T).GetProperties().Where(
            prop => Attribute.IsDefined(prop, typeof(DBIndex))).ToList();
            if (uniqueStringIndexProperties.Any()) {
                foreach (var propertyInfo in uniqueStringIndexProperties) {
                    var propertyInfoName = propertyInfo.Name;
                    DBIndex? indexAttr = (DBIndex)Attribute.GetCustomAttribute(propertyInfo, typeof(DBIndex));
                    if (indexAttr.indexAttribute != IndexAttributes.FULL_TEXT_INDEX) {
                        bool unique = indexAttr.indexAttribute == IndexAttributes.UNIQUE;
                        var options = new CreateIndexOptions { Unique = unique };
                        var field = new StringFieldDefinition<T>(propertyInfoName);
                        var indexDefinition = new IndexKeysDefinitionBuilder<T>().Ascending(field);
                        var indexModel = new CreateIndexModel<T>(indexDefinition, options);
                        await collection.Indexes.CreateOneAsync(indexModel);
                    } else {
                        var field = new StringFieldDefinition<T>(propertyInfoName);
                        var indexModel = new CreateIndexModel<T>(Builders<T>.IndexKeys.Text(field));
                        await collection.Indexes.CreateOneAsync(indexModel);
                    }
                }
            }
            return true;
        }
        public async Task<bool> collectionExists(string collectionName) {
            var filter = new BsonDocument("name", collectionName);
            var collections = await _dataStore.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
            return await collections.AnyAsync();
        }
        public async Task<bool> insert<T>(T data) {
            try {
                bool collectionCreatedBefore = await collectionExists(typeof(T).Name);
                var collection = _dataStore.GetCollection<T>(typeof(T).Name);
                if (!collectionCreatedBefore)
                    await createUniqueIndexes(collection);
                await collection.InsertOneAsync(data);
                return true;
            } catch {
                if (_session != null)
                    await _session.AbortTransactionAsync();
                throw;
            }
        }
        public FilterDefinition<T> getFilter<T>(string textSearch) {
            return Builders<T>.Filter.Text(textSearch);
        }
        public FilterDefinition<T> getFilter<T>(Expression<Func<T, bool>> condition, string? textSearch = null) {
            var filter = Builders<T>.Filter.Where(condition);
            if (!string.IsNullOrEmpty(textSearch)) {
                var textSearchFilter = Builders<T>.Filter.Text(textSearch);
                return Builders<T>.Filter.And(filter, textSearchFilter);
            }
            return filter;
        }
        public FilterDefinition<T> getFilter<T>(IEnumerable<Expression<Func<T, bool>>> conditions, string? textSearch = null) {
            var predicates = PredicateBuilder.New<T>();
            var builder = Builders<T>.Filter;
            FilterDefinition<T> textSearchFilter;
            FilterDefinition<T> boolCond;
            foreach (Expression<Func<T, bool>> condition in conditions) {
                predicates = predicates.And(condition);
            }
            boolCond = conditions.Count() > 0 ? Builders<T>.Filter.Where((Expression<Func<T, bool>>)predicates.Expand()) : builder.Empty;
            if (!string.IsNullOrEmpty(textSearch)) {
                textSearchFilter = Builders<T>.Filter.Text(textSearch);
                return builder.And(boolCond, textSearchFilter);
            }
            return boolCond;
        }

        public FindOptions<T> getFindOptions<T>(int limit) {
            FindOptions<T> options = new FindOptions<T> { Limit = limit };
            return options;
        }
        public FindOptions<T> getFindOptions<T>(Dictionary<string, int> sorting) {
            List<SortDefinition<T>> definitions = new List<SortDefinition<T>>();
            if (sorting != null) {
                foreach (KeyValuePair<string, int> kvp in sorting) {
                    if (kvp.Value == 1) {
                        definitions.Add(Builders<T>.Sort.Ascending(kvp.Key));
                    } else {
                        definitions.Add(Builders<T>.Sort.Descending(kvp.Key));
                    }
                }
            }
            FindOptions<T> options = new FindOptions<T> { Sort = Builders<T>.Sort.Combine(definitions) };
            return options;
        }
        public FindOptions<T> getFindOptions<T>(int limit, Dictionary<string, int> sorting) {
            FindOptions<T> limitOptions = getFindOptions<T>(limit);
            FindOptions<T> sortOptions = getFindOptions<T>(sorting);
            limitOptions.Sort = sortOptions.Sort;
            return limitOptions;
        }

        public async Task<bool> update<T>(FilterDefinition<T> filter, object updateField) {
            try {
                var collection = _dataStore.GetCollection<T>(typeof(T).Name);
                var updateDefination = new List<UpdateDefinition<T>>();
                foreach (var property in updateField.GetType().GetProperties()) {
                    var value = property.GetValue(updateField);
                    updateDefination.Add(Builders<T>.Update.Set(property.Name, value));
                }
                var combinedUpdate = Builders<T>.Update.Combine(updateDefination);
                var result = await collection.UpdateManyAsync(filter, combinedUpdate);
                return result.IsAcknowledged;
            } catch {
                if (_session != null)
                    await _session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task<bool> update<T>(FilterDefinition<T> filter, Dictionary<string, dynamic> updateField) {
            try {
                var collection = _dataStore.GetCollection<T>(typeof(T).Name);
                var updateDefination = new List<UpdateDefinition<T>>();
                foreach (KeyValuePair<string, dynamic> kvp in updateField) {
                    var value = kvp.Value;
                    try {
                        updateDefination.Add(Builders<T>.Update.Set(kvp.Key, value));
                    } catch {
                        updateDefination.Add(Builders<T>.Update.Set(kvp.Key, ""));
                    }

                }
                var combinedUpdate = Builders<T>.Update.Combine(updateDefination);
                var result = await collection.UpdateManyAsync(filter, combinedUpdate);
                return result.IsAcknowledged;
            } catch {
                if (_session != null)
                    await _session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task<bool> updateIncremental<T>(FilterDefinition<T> filter, object updateField) {
            try {
                var collection = _dataStore.GetCollection<T>(typeof(T).Name);
                var updateDefination = new List<UpdateDefinition<T>>();
                foreach (var property in updateField.GetType().GetProperties()) {
                    var value = property.GetValue(updateField);
                    updateDefination.Add(Builders<T>.Update.Inc(property.Name, value));
                }
                var combinedUpdate = Builders<T>.Update.Combine(updateDefination);
                var result = await collection.UpdateManyAsync(filter, combinedUpdate);
                return result.IsAcknowledged;
            } catch {
                if (_session != null)
                    await _session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<T>> select<T>() {
            return await select<T>(null, null);
        }

        public async Task<IEnumerable<T>> select<T>(FilterDefinition<T> filter) {
            return await select(filter, null);
        }

        public async Task<IEnumerable<T>> select<T>(FindOptions<T> filter) {
            return await select(null, filter);
        }

        public async Task<IEnumerable<T>> select<T>(FilterDefinition<T>? filter, FindOptions<T>? options) {
            var collection = _dataStore.GetCollection<T>(typeof(T).Name);
            List<T> result;
            if (filter == null) {
                filter = Builders<T>.Filter.Empty;
            }
            if (options != null) {
                result = (await collection.FindAsync(filter, options)).ToList();
            } else {
                result = (await collection.FindAsync(filter)).ToList();
            }
            return result;
        }

        public async Task<List<T>> select<T>(FilterDefinition<T> filter, int limit, int page) {
            var collection = _dataStore.GetCollection<T>(typeof(T).Name);
            if (filter == null) {
                filter = Builders<T>.Filter.Empty;
            }
            var result = collection.Find(filter).Skip(limit * page).Limit(limit).ToListAsync();
            await Task.WhenAll(result);
            return result.Result;
        }

        public async Task<long> count<T>(FilterDefinition<T> filter) {
            var collection = _dataStore.GetCollection<T>(typeof(T).Name);
            var r = await collection.CountDocumentsAsync(filter);
            return r;
        }
        public async Task<bool> delete<T>(FilterDefinition<T> filter) {
            var collection = _dataStore.GetCollection<T>(typeof(T).Name);
            var result = await collection.DeleteManyAsync(filter);
            return result.IsAcknowledged;
        }

        public void Dispose() {
            try {
                //_client does not need to be disposed of/ But maybe something else
            } catch { }
        }

        public async Task startTransaction() {
            closeSession();
            _session = await _client.StartSessionAsync();
        }

        public async Task commitTransaction() {
            await _session.CommitTransactionAsync();
            closeSession();
        }

        private void closeSession() {
            try {
                _session.Dispose();
            } catch { }
        }
    }
}
