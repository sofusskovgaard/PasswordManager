using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PasswordManager.Data.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        protected BaseEntity()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }
        
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; }
    }
}