using System.Collections.Generic;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    //Interface used to implement multiple inheritance on Part (materials)
    public interface ILoggedPart
    {
        int Id { get; set; }
        decimal PriceExtra { get; set; }
        int VatExtra { get; set; }

        ICollection<LogOff> LogOffs { get; set; }
    }
}
