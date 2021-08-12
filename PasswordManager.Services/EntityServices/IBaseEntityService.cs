using System.Threading.Tasks;
using PasswordManager.Data.Entities;

namespace PasswordManager.Services.EntityServices
{
    public interface IBaseEntityService<T> where T : IBaseEntity
    {
        T Create();

        Task<T> Save(T entity);
        
        Task Remove(T entity);
        
        Task Remove(string id);
    }
}