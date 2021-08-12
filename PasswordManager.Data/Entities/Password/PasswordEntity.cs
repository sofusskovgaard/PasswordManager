using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PasswordManager.Data.Attributes.CollectionName;

namespace PasswordManager.Data.Entities.Password
{
    [CollectionName("Passwords")]
    public class PasswordEntity : BaseEntity, IPasswordEntity
    {
        public string URL { get; set; }
        
        public string Name { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
    }
}