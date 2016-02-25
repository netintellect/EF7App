using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceCruiser.Model.Entities.Core.Data;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.IRepository;

namespace ServiceCruiser.Model.Entities.Repositories
{
    public interface IUserRepository : IEntityRepository<User>
    {
        Task<User> GetCurrentUserAsync(string login);
    }
}
