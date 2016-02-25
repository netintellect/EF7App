using System;

namespace ServiceCruiser.Model.Entities.Core.Repositories
{
    public interface IBaseEntityRepository
    {

        bool IsDirty();
        bool Handles(Type type);
        string LocalId { get; }
        void UnregisterKey(string key);

    }
}
