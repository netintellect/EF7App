using System.Collections.Generic;
using ServiceCruiser.Model.Entities.Contracts;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    //Interface used to implement multiple inheritance on Part (materials)
    public interface ITrackedPart
    {
        int Id { get; set; }
        string ReversePartNo { get; set; }
        ICollection<PartNoAlias> Aliases { get; set; }
    }
}
