using System.Threading.Tasks;
using MongoDB.Driver;
using PasswordManager.Data.Entities;

namespace PasswordManager.Data.DataAccessService
{
    public interface IDataAccessService
    {
        Task<T> Get<T>(string id) where T : BaseEntity;

        Task<T> Get<T>(FilterDefinition<T> filterDefinition) where T : BaseEntity;
        
        Task<T> Create<T>(T entity) where T : BaseEntity;

        Task Delete<T>(T entity) where T : BaseEntity;

        Task<T> Update<T>(T entity) where T : BaseEntity;
    }
}