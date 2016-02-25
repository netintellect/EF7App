using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.IRepository;

namespace ServiceCruiser.Model.Entities.Repositories
{
    public interface IContractModelRepository : IEntityRepository<ContractModel>
    {
        Task<ICollection<ContractModel>> GetAsync(IEnumerable<int> contractModelIds);
    }
}
