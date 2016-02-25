using System;
using System.Collections.Generic;

namespace ServiceCruiser.Model.IRepository
{
    public interface IEntityMapper
    {
        IDictionary<Type, Type> Mapping { get; }
        
    }
}
