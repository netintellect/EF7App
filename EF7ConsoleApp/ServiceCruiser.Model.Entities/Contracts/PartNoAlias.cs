using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PartNoAlias : ValidatedEntity<PartNoAlias>
    {
        #region state
        private int _id;
        [JsonProperty, KeyNew(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private string _partNo;
        [JsonProperty]
        public string PartNo
        {
            get { return _partNo; }
            set { SetProperty(value, ref _partNo, () => PartNo); }
        }
        private string _type;
        [JsonProperty]
        public string Type
        {
            get { return _type; }
            set { SetProperty(value, ref _type, () => Type); }
        }
        #endregion
    }
}
