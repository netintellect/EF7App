using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.FieldServices;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GrabMaterial : Part, ILoggedPart
    {
        #region state

        #region ILoggedPart
        private decimal _priceExtra;
        [JsonProperty]
        public decimal PriceExtra
        {
            get { return _priceExtra; }
            set { SetProperty(value, ref _priceExtra, () => PriceExtra); }
        }
        private int _vatExtra;
        [JsonProperty]
        public int VatExtra
        {
            get { return _vatExtra; }
            set { SetProperty(value, ref _vatExtra, () => VatExtra); }
        }
        #endregion

        #endregion
    }
}
