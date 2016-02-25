using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.IRepository;

namespace ServiceCruiser.Model.Entities.Repositories
{
    public interface IServiceSpecificationRepository : IEntityRepository<ServiceSpecification>
    {
        Task<ICollection<ServiceSpecification>> GetAsync(IEnumerable<int> contractModelIds);
    }
}
