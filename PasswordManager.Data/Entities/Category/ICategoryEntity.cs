namespace PasswordManager.Data.Entities.Category
{
    public interface ICategoryEntity : IBaseEntity
    {
        string Name { get; set; }
        
        string ParentCategoryId { get; set; }
    }
}