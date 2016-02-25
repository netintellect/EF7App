using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ServiceCruiser.Model.Entities.Core;
using System.Threading.Tasks;
using ServiceCruiser.Model.Entities.Core.Data;
using ServiceCruiser.Model.Entities.Core.Repositories;

namespace ServiceCruiser.Model.IRepository
{
    public interface IEntityRepository<TEntity> : IBaseEntityRepository, IDisposable where TEntity : BaseEntity
                                                                                     
    {
        void Commit(TEntity t);
        void Commit(IEnumerable<TEntity> entities);

        TEntity CreateInstance();

        Task<ICollection<TEntity>> GetAsync<TDataService>(Func<TEntity, object> order = null) where TDataService : IBaseService;
        Task<ICollection<TEntity>> GetAsync<TDataService, TFilter>(TFilter filter, bool doAskBackEnd = false) where TDataService: IBaseService
                                                                                                              where TFilter: InclusionDataFilter<TEntity>;
        Task<TEntity> GetAsync<TDataService>(int id, bool doAskBackEnd = false) where TDataService : IBaseService;
        
        ICollection<TEntity> Filter(Expression<Func<TEntity, bool>> filter, Func<TEntity, object> order, int limit);
        ICollection<TEntity> Filter(Expression<Func<TEntity, bool>> filter, int limit);
        ICollection<TEntity> Filter(Expression<Func<TEntity, bool>> filter, Func<TEntity, object> order);
        ICollection<TEntity> Filter(Expression<Func<TEntity, bool>> filter);
        
        Task RefreshAsync(Expression<Func<TEntity, bool>> filter);

        Task RefreshAsync();

        int TotalEntities { get; }
    }
}
