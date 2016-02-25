using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.FieldServices;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OnsiteService : Part, ILoggedPart
    {
        #region state
        private int _meanDuration;
        [JsonProperty]
        public int MeanDuration
        {
            get { return _meanDuration; }
            set { SetProperty(value, ref _meanDuration, () => MeanDuration); }
        }

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
