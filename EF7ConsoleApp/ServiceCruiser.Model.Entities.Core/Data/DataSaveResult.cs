using ServiceCruiser.Model.Entities.Core;

namespace ServiceCruiser.Model.Entities.Core.Data
{
    public class DataSaveResult : DataSaveResult<BaseEntity>
    {
        #region behavior
        public DataSaveResult(BaseEntity entity, string errorMessage)
        {
            if (entity == null) return;

            Entity = entity;
            ErrorMessage = errorMessage;
        }

        public static DataSaveResult ConvertToUntyped<TEntity>(DataSaveResult<TEntity> dataSaveResult) where TEntity : BaseEntity
        {
            if (dataSaveResult == null) return null;
            
            return new DataSaveResult(dataSaveResult.Entity as BaseEntity, dataSaveResult.ErrorMessage);
        }


        public static DataSaveResult<TEntity> ConvertToTyped<TEntity>(DataSaveResult dataSaveResult) where TEntity : BaseEntity
        {
            if (dataSaveResult == null) return null;

            var item = dataSaveResult.Entity as TEntity;
            if (item != null) 
                item.EndEdit();

            return new DataSaveResult<TEntity>(item, dataSaveResult.ErrorMessage);
        }
        #endregion
    }

    public class DataSaveResult<TEntity> where TEntity : BaseEntity
    {
        #region state
        public TEntity Entity { get; set; }

        public bool HasErrors
        {
            get { return !string.IsNullOrEmpty(ErrorMessage); }
        }

        public string ErrorMessage { get; set; } 
        #endregion

        #region behavior
        public DataSaveResult() { }
        public DataSaveResult(TEntity entity)
        {
            Entity = entity;
        }

        public DataSaveResult(TEntity entity, string errorMessage)
        {
            Entity = entity;
            ErrorMessage = errorMessage;
        }
        #endregion

    }
}
