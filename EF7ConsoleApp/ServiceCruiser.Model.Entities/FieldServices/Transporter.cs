using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Transporter : InventoryLocation
    {
    }
}
