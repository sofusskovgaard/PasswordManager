using System;
using System.Threading.Tasks;
using PasswordManager.Data.DataAccessService;
using PasswordManager.Data.Entities;

namespace PasswordManager.Services.EntityServices
{
    public abstract class BaseEntityService<T> : IBaseEntityService<T> where T : BaseEntity
    {
        private readonly IDataAccessService _dataAccessService;

        protected BaseEntityService(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }

        public virtual T Create()
        {
            var entity = Activator.CreateInstance<T>();
            return entity;
        }

        public virtual async Task<T> Save(T entity)
        {
            var result = await _dataAccessService.Create(entity);
            return result;
        }
        
        public virtual async Task Remove(T entity)
        {
            await _dataAccessService.Delete(entity);
        }
        
        public virtual async Task Remove(string id)
        {
            var entity = await _dataAccessService.Get<T>(id);
            await _dataAccessService.Delete(entity);
        }
    }
}