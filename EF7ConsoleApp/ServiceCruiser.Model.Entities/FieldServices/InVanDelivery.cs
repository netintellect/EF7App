using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InVanDelivery : InventoryAction
    {
        #region state
        private string _barcode;
        [JsonProperty]
        public string Barcode
        {
            get { return _barcode; }
            set { SetProperty(value, ref _barcode, () => Barcode); }
        }
        #endregion
    }
}
