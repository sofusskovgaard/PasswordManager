using PasswordManager.Data.DataAccessService;
using PasswordManager.Data.Entities.User;

namespace PasswordManager.Services.EntityServices.UsersEntityService
{
    public class UsersEntityService : BaseEntityService<UserEntity>, IUsersEntityService
    {
        public UsersEntityService(IDataAccessService dataAccessService) : base(dataAccessService)
        {
        }
    }
}