using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VanInventoryLoc : InventoryLocation
    {
        #region state
        private string _licensePlate;
        [JsonProperty]
        public string LicensePlate
        {
            get { return _licensePlate; }
            set { SetProperty(value, ref _licensePlate, () => LicensePlate); }
        }
        #endregion
    }
}
