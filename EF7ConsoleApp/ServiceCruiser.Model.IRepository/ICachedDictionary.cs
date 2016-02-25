using System;
using ServiceCruiser.Model.Entities.Core;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ServiceCruiser.Model.IRepository
{
    public interface ICachedDictionary<TEntity> where TEntity : BaseEntity  
    {
        Type GetEntityType();
        IEnumerable<TEntity> GetEntitiesToCommit();
        bool IsDirty();
        void Remove(TEntity entity);
        void Remove(int id);
        TEntity Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);
        TEntity FindEntity(int id);
        void Clear(Func<TEntity, bool> filter);
        IList<TEntity> Filter(Expression<Func<TEntity, bool>> filter, int limit = 0);
        TEntity CreateInstance();
        int TotalCachedEntities { get;  }
    }
}
