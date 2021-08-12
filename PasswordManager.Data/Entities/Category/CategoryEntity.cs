using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PasswordManager.Data.Entities.Category
{
    public class CategoryEntity : BaseEntity, ICategoryEntity
    {
        public string Name { get; set; }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string ParentCategoryId { get; set; }
    }
}