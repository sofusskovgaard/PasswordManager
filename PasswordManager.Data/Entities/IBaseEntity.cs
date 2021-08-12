using System;

namespace PasswordManager.Data.Entities
{
    public interface IBaseEntity
    {
        string Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}