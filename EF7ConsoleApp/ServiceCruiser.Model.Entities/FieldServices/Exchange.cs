using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Exchange : InventoryAction
    {
        #region state
        private string _exchangeNumber;
        [JsonProperty]
        public string ExchangeNumber
        {
            get { return _exchangeNumber; }
            set { SetProperty(value, ref _exchangeNumber, () => ExchangeNumber); }
        }
        private bool _outgoing  ;
        [JsonProperty]
        public bool Outgoing
        {
            get { return _outgoing; }
            set { SetProperty(value, ref _outgoing, () => Outgoing); }
        }
        private int _siblingId;
        [JsonProperty]
        public int SiblingId
        {
            get { return _siblingId; }
            set { SetProperty(value, ref _siblingId, () => SiblingId); }
        }
        private Exchange _sibling;
        [JsonProperty]
        public Exchange Sibling
        {
            get { return _sibling; }
            set { SetProperty(value, ref _sibling, () => Sibling); }
        }
        #endregion
    }
}
