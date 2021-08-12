using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using PasswordManager.Data.Attributes.CollectionName;
using PasswordManager.Data.Entities;

namespace PasswordManager.Data.DataAccessService
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IConfiguration _configuration;
        
        private IMongoClient _client;

        private IMongoDatabase _database;
        
        public DataAccessService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new MongoClient(_configuration.GetConnectionString("mongo"));
            _database = _client.GetDatabase(_configuration["MongoDatabase"]);
        }

        public async Task<T> Get<T>(string id) where T : BaseEntity
        {
            var collection = _getCollection<T>();
            
            var query = await collection.FindAsync(Builders<T>.Filter.Eq(x => x.Id, id));

            return await query.FirstOrDefaultAsync();
        }
        
        public async Task<T> Get<T>(FilterDefinition<T> filterDefinition) where T : BaseEntity
        {
            var collection = _getCollection<T>();
            
            var query = await collection.FindAsync(filterDefinition);

            return await query.FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<T>> GetAll<T>(FilterDefinition<T> filterDefinition, int limit = 10, int skip = 0) where T : BaseEntity
        {
            var collection = _getCollection<T>();
            
            var query = await collection.FindAsync(filterDefinition, new FindOptions<T>() { Limit = limit, Skip = skip});

            return await query.ToListAsync();
        }

        public async Task<T> Create<T>(T entity) where T : BaseEntity
        {
            var collection = _getCollection<T>();

            entity.CreatedAt = DateTime.Now;
            await collection.InsertOneAsync(entity);
            
            return entity;
        }

        public async Task Delete<T>(T entity) where T : BaseEntity
        {
            var collection = _getCollection<T>();

            await collection.FindOneAndDeleteAsync(Builders<T>.Filter.Eq(x => x.Id, entity.Id));
        }

        public async Task<T> Update<T>(T entity) where T : BaseEntity
        {
            var collection = _getCollection<T>();

            var properties = Activator.CreateInstance<T>().GetType().GetProperties().Where(x => x.GetCustomAttribute(typeof(BsonIgnoreAttribute)) == null);

            var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);
            var updates = new List<UpdateDefinition<T>>();
            
            foreach (var property in properties)
            {
                if (_shouldNotUpdate(property.Name)) continue;
                
                updates.Add(Builders<T>.Update.Set(property.Name, property.GetValue(entity)));
            }
            
            updates.Add(Builders<T>.Update.Set("UpdatedAt", DateTime.Now));

            await collection.UpdateOneAsync(filter, Builders<T>.Update.Combine(updates));
            
            return entity;
        }

        private IMongoCollection<T> _getCollection<T>()
        {
            CollectionName attr = (CollectionName) Attribute.GetCustomAttribute(typeof(T), typeof(CollectionName));
            return _database.GetCollection<T>(attr.Name);
        }

        private bool _shouldNotUpdate(string propertyName)
        {
            switch (propertyName)
            {
                case "Id":
                    return true;
                case "CreatedAt":
                    return true;
                case "UpdatedAt":
                    return true;
                default:
                    return false;
            }
        }
    }
}