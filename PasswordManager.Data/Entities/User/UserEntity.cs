using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using PasswordManager.Data.Attributes.CollectionName;

namespace PasswordManager.Data.Entities.User
{
    [CollectionName("Users")]
    public class UserEntity : BaseEntity, IUserEntity
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string EMail { get; set; }
        
        public string Password { get; set; }

        [BsonIgnore]
        public string PasswordHash => Password.Split("$").Last();
        
        [BsonIgnore]
        public string PasswordSalt => Password.Split("$").First();
    }
}