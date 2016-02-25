using System.Collections.Generic;
using System.Collections.ObjectModel;
using ServiceCruiser.Model.Entities.Core.Extensibility;

namespace ServiceCruiser.Model.Entities.Core.Data
{
    public class DataListResult : DataListResult<BaseEntity>
    {
        #region behavior
        public DataListResult(IEnumerable<BaseEntity> entities, int totalEntities)
        {
            if (entities == null) return;

            _entities = entities.ToObservableCollection();
            TotalEntities = totalEntities;
        }

        public static DataListResult ConvertToUntyped<TEntity>(DataListResult<TEntity> dataListResult) where TEntity : BaseEntity
        {
            if (dataListResult == null) return null;

            var entities = new ObservableCollection<BaseEntity>();
            foreach (var entity in dataListResult.Entities)
            {
                entities.Add(entity);
            }
            return new DataListResult(entities, dataListResult.TotalEntities);
        }


        public static DataListResult<TEntity> ConvertToTyped<TEntity>(DataListResult dataListResult) where TEntity : BaseEntity
        {
            if (dataListResult == null) return null;

            var entities = new ObservableCollection<TEntity>();
            foreach (var entity in dataListResult.Entities)
            {
                var item = entity as TEntity;
                if (item == null) continue;
                
                item.EndEdit();

                entities.Add(item);
            }
            return new DataListResult<TEntity>(entities, dataListResult.TotalEntities);
        }
        #endregion
    }

    /// <summary>
    /// Container class to return a list of entities, typically used when retrieving 
    /// one or more entities and we want to add information about the collection, ordering
    /// paging etc.
    /// </summary>
    public class DataListResult<TEntity> where TEntity : BaseEntity
    {
        #region state

        protected ObservableCollection<TEntity> _entities;

        public IEnumerable<TEntity> Entities
        {
            get { return _entities; }
            set { _entities = value != null ? value.ToObservableCollection() : null; }
        }

        public int TotalEntities { get; set; }
        #endregion

        #region behavior

        public DataListResult() { }

        public DataListResult(IEnumerable<TEntity> entities)
        {
            if (entities == null) return;

            _entities = entities.ToObservableCollection();
            setToUnmodified(_entities);
            TotalEntities = _entities.Count;
        }

        public DataListResult(IEnumerable<TEntity> entities, int totalEntities)
        {
            _entities = entities.ToObservableCollection();
            setToUnmodified(_entities);
            TotalEntities = totalEntities;
        }

        private void setToUnmodified(IEnumerable<TEntity> entityList)
        {
            foreach (var e in entityList)
            {
                var entity = e as BaseEntity;
                if (entity != null) entity.EndEdit();
            }
        }
        #endregion
    }
}
