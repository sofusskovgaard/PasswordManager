using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PasswordManager.Data.Attributes.CollectionName;
using PasswordManager.Data.Entities;
using PasswordManager.Data.Entities.Password;
using PasswordManager.Data.Entities.User;
using PasswordManager.TestUtils;
using Xunit;
using Xunit.Abstractions;

namespace PasswordManager.Services.Tests.DataAccessService
{
    public class DataAccessService
    {
        private readonly ITestOutputHelper output;

        private readonly IConfiguration _configuration;
        
        private readonly IMongoClient _client;

        private readonly IMongoDatabase _db;
        
        private readonly IMongoCollection<UserEntity> _users;
        
        private readonly IMongoCollection<PasswordEntity> _passwords;

        public DataAccessService(ITestOutputHelper output)
        {
            this.output = output;
            _configuration = MockUtil.CreateConfiguration();
            
            // initialize some database stuff to properly test the data access service.
            _client = new MongoClient(_configuration.GetConnectionString("mongo"));
            _db = _client.GetDatabase(_configuration.GetValue<string>("MongoDatabase"));
            _users = _db.GetCollection<UserEntity>("Users");
            _passwords = _db.GetCollection<PasswordEntity>("Passwords");
        }

        [Fact]
        public async Task GetById()
        {
            var passwordHashingService = MockUtil.CreatePasswordHashingService().Object;
            var dataAccessService = new Data.DataAccessService.DataAccessService(MockUtil.CreateConfiguration());

            var expectedEntity = new UserEntity()
            {
                FirstName = "Sofus",
                LastName = "Skovgaard",
                EMail = "sofus.skovgaard@gmail.com",
                Password = passwordHashingService.CreateHash("P@ssw0rd!")
            };

            await _users.InsertOneAsync(expectedEntity);
            output.WriteLine($"Entity ID: {expectedEntity.Id}");

            var result = await dataAccessService.Get<UserEntity>(expectedEntity.Id);
            
            Assert.Equal(expectedEntity.Id, result.Id);
            
            // Cleanup
            var cleanup = new List<BaseEntity>() { expectedEntity };
            await _cleanupEntities(cleanup, _db);
        }
        
        [Fact]
        public async Task GetByFilter()
        {
            var passwordHashingService = MockUtil.CreatePasswordHashingService().Object;
            var dataAccessService = new Data.DataAccessService.DataAccessService(MockUtil.CreateConfiguration());

            var expectedEntity = new UserEntity()
            {
                FirstName = "Sofus",
                LastName = "Skovgaard",
                EMail = "sofus.skovgaard@gmail.com",
                Password = passwordHashingService.CreateHash("P@ssw0rd!")
            };

            await _users.InsertOneAsync(expectedEntity);
            output.WriteLine($"Entity ID: {expectedEntity.Id}");

            var result = await dataAccessService.Get<UserEntity>(Builders<UserEntity>.Filter.Eq(x => x.Id, expectedEntity.Id));
            
            Assert.Equal(expectedEntity.Id, result.Id);
            
            // Cleanup
            var cleanup = new List<BaseEntity>() { expectedEntity };
            await _cleanupEntities(cleanup, _db);
        }
        
        [Fact]
        public async Task GetAll()
        {
            var passwordHashingService = MockUtil.CreatePasswordHashingService().Object;
            var dataAccessService = new Data.DataAccessService.DataAccessService(MockUtil.CreateConfiguration());

            var user = new UserEntity()
            {
                FirstName = "Sofus",
                LastName = "Skovgaard",
                EMail = "sofus.skovgaard@gmail.com",
                Password = passwordHashingService.CreateHash("P@ssw0rd!")
            };

            await _users.InsertOneAsync(user);
            output.WriteLine($"User ID: {user.Id}\n");

            var expectedEntities = new List<PasswordEntity>()
            {
                new PasswordEntity()
                {
                    Name = "Password 1",
                    OwnerId = user.Id
                },
                new PasswordEntity()
                {
                    Name = "Password 2",
                    OwnerId = user.Id
                }
            };
            await _passwords.InsertManyAsync(expectedEntities);
            expectedEntities.ForEach(x => output.WriteLine($"Password ID: {x.Id}"));
            
            output.WriteLine("");
            
            var result = await dataAccessService.GetAll(Builders<PasswordEntity>.Filter.Eq(x => x.OwnerId, user.Id));
            result.ToList().ForEach(x => output.WriteLine($"Found Password ID: {x.Id}"));
            
            Assert.Equal(expectedEntities.OrderBy(x => x.Id).Select(x => x.Id), result.OrderBy(x => x.Id).Select(x => x.Id));
            
            // Cleanup
            var cleanup = new List<BaseEntity>(expectedEntities) { user };
            await _cleanupEntities(cleanup, _db);
        }
        
        [Fact]
        public async Task Create()
        {
            var passwordHashingService = MockUtil.CreatePasswordHashingService().Object;
            var dataAccessService = new Data.DataAccessService.DataAccessService(MockUtil.CreateConfiguration());
        
            var expectedEntity = new UserEntity()
            {
                FirstName = "Sofus",
                LastName = "Skovgaard",
                EMail = "sofus.skovgaard@gmail.com",
                Password = passwordHashingService.CreateHash("P@ssw0rd!")
            };

            await dataAccessService.Create(expectedEntity);
            output.WriteLine($"User ID: {expectedEntity.Id}\n");

            var result = await _users.FindAsync(Builders<UserEntity>.Filter.Eq(x => x.Id, expectedEntity.Id));
            
            Assert.Equal(expectedEntity.Id, result.First().Id);
            
            // Cleanup
            var cleanup = new List<BaseEntity>() { expectedEntity };
            await _cleanupEntities(cleanup, _db);
        }
        
        [Fact]
        public async Task Delete()
        {
            var passwordHashingService = MockUtil.CreatePasswordHashingService().Object;
            var dataAccessService = new Data.DataAccessService.DataAccessService(MockUtil.CreateConfiguration());
        
            var expectedEntity = new UserEntity()
            {
                FirstName = "Sofus",
                LastName = "Skovgaard",
                EMail = "sofus.skovgaard@gmail.com",
                Password = passwordHashingService.CreateHash("P@ssw0rd!")
            };

            await _users.InsertOneAsync(expectedEntity);
            output.WriteLine($"User ID: {expectedEntity.Id}\n");

            await dataAccessService.Delete(expectedEntity);
            var result = await _users.FindAsync(Builders<UserEntity>.Filter.Eq(x => x.Id, expectedEntity.Id));

            Assert.Throws<InvalidOperationException>(() => result.First());
        }
        
        [Fact]
        public async Task Update()
        {
            var passwordHashingService = MockUtil.CreatePasswordHashingService().Object;
            var dataAccessService = new Data.DataAccessService.DataAccessService(MockUtil.CreateConfiguration());
        
            var expectedEntity = new UserEntity()
            {
                FirstName = "Sofus",
                LastName = "Skovgaard",
                EMail = "sofus.skovgaard@gmail.com",
                Password = passwordHashingService.CreateHash("P@ssw0rd!")
            };

            await _users.InsertOneAsync(expectedEntity);
            output.WriteLine($"User ID: {expectedEntity.Id}\n");
            output.WriteLine($"User Firstname: {expectedEntity.FirstName}");
            output.WriteLine($"User Lastname: {expectedEntity.LastName}");
            output.WriteLine($"User Email: {expectedEntity.EMail}\n");

            expectedEntity.FirstName = "Joachim";
            expectedEntity.LastName = "Jordbærhjelm";
            expectedEntity.EMail = "joachim.jordbærhejlm@mail.com";
           
            await dataAccessService.Update(expectedEntity);
            var result = await _users.FindAsync(Builders<UserEntity>.Filter.Eq(x => x.Id, expectedEntity.Id));
            var resultObj = result.First();
            
            output.WriteLine($"Result Firstname: {resultObj.FirstName}");
            output.WriteLine($"Result Lastname: {resultObj.LastName}");
            output.WriteLine($"Result Email: {resultObj.EMail}\n");

            Assert.Equal(expectedEntity.FirstName, resultObj.FirstName);
            Assert.Equal(expectedEntity.LastName, resultObj.LastName);
            Assert.Equal(expectedEntity.EMail, resultObj.EMail);
            
            // Cleanup
            var cleanup = new List<BaseEntity>() { expectedEntity };
            await _cleanupEntities(cleanup, _db);
        }
        
        private async Task _cleanupEntities(IEnumerable<BaseEntity> entities, IMongoDatabase db)
        {
            var collections = new Dictionary<string, IMongoCollection<BaseEntity>>();
            foreach (var entity in entities)
            {
                var key = (CollectionName)Attribute.GetCustomAttribute(entity.GetType(), typeof(CollectionName));
                if (!collections.TryGetValue(key.Name, out IMongoCollection<BaseEntity> collection))
                {
                    collection = db.GetCollection<BaseEntity>(key.Name);
                    collections.Add(key.Name, collection);
                }
                await collection.DeleteOneAsync(Builders<BaseEntity>.Filter.Eq(x => x.Id, entity.Id));
            }
        }
    }
}