using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ValueMaterial : Part, ITrackedPart, ILoggedPart
    {
        #region state
        private bool _serialRequired;
        [JsonProperty]
        public bool SerialRequired
        {
            get { return _serialRequired; }
            set { SetProperty(value, ref _serialRequired, () => SerialRequired); }
        }
        private int? _groupId;
        [JsonProperty]
        public int? GroupId
        {
            get { return _groupId; }
            set { SetProperty(value, ref _groupId, () => GroupId); }
        }
        private MaterialGroup _group;
        [HandleOnNesting]
        [Aggregation(isComposite: false)]
        [JsonProperty]
        public MaterialGroup Group
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged(() => Group);
            }
        }

        #region ITrackedPart
        private string _reversePartNo;
        [JsonProperty]
        public string ReversePartNo
        {
            get { return _reversePartNo; }
            set { SetProperty(value, ref _reversePartNo, () => ReversePartNo); }
        }
        private ObservableCollection<PartNoAlias> _aliases = new ObservableCollection<PartNoAlias>();
        [HandleOnNesting, Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(PartNoAlias))]
        [JsonProperty]
        public ICollection<PartNoAlias> Aliases
        {
            get { return _aliases; }
            set { _aliases = value != null ? value.ToObservableCollection() : null; }
        }
        #endregion

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
