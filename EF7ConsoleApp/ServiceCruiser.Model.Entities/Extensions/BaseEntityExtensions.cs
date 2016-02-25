using System.Linq;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Repositories;
using ServiceCruiser.Model.Entities.Repositories;
using ServiceCruiser.Model.Entities.Users;

namespace ServiceCruiser.Model.Entities.Extensions
{
    public static class BaseEntityExtensions
    {
        public static User GetCurrentUser(this BaseEntity entity, IRepositoryFinder repostoryFinder, User user = null)
        {
            if (repostoryFinder == null) return user;
            
            var repository = repostoryFinder.GetRepository<IUserRepository>() as IUserRepository;
            return repository != null ? repository.Filter(u => u.IsCurrentUser).FirstOrDefault() : user;
        }
    }
}
