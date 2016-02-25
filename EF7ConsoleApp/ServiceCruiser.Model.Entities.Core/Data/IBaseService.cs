using System;
using System.Threading.Tasks;

namespace ServiceCruiser.Model.Entities.Core.Data
{
    public interface IBaseService
    {
        Uri GetServiceUri();
        Task<DataListResult<TEntity>> List<TEntity>(InclusionDataFilter<TEntity> filter) where TEntity: BaseEntity;
    }
}
