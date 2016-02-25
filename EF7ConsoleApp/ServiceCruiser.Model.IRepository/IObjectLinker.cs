using System.Collections.Generic;
using ServiceCruiser.Model.Entities.Core;

namespace ServiceCruiser.Model.IRepository
{
    /// <summary>
    /// Allows to link objects. (entities)
    /// </summary>
    public interface IObjectLinker
    {
        void LinkEntities(IEnumerable<BaseEntity> entities);
        void LinkEntity(BaseEntity entity);
    }
}
