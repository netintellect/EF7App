using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RmaReason : ValidatedEntity<RmaReason>
    {
        #region state
        private int _id;
        [JsonProperty, Key(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private int _contractModelId;
        [JsonProperty]
        public int ContractModelId
        {
            get { return _contractModelId; }
            set { SetProperty(value, ref _contractModelId, () => ContractModelId); }
        }
        private ContractModel _contractModel;
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: false)]
        public ContractModel ContractModel
        {
            get { return _contractModel; }
            set
            {
                _contractModel = value;
                OnPropertyChanged(() => ContractModel);
            }
        }
        private string _code;
        [JsonProperty]
        public string Code
        {
            get { return _code; }
            set { SetProperty(value, ref _code, () => Code); }
        }
        private string _description;
        [JsonProperty]
        public string Description
        {
            get { return _description; }
            set { SetProperty(value, ref _description, () => Description); }
        }

        private ObservableCollection<Rma> _rmas = new ObservableCollection<Rma>();
        [JsonProperty]
        public ICollection<Rma> Rmas
        {
            get { return _rmas; }
            set { _rmas = value != null ? value.ToObservableCollection() : null; }
        }

        public string DisplayDescription
        {
            get { return string.Format("({0}) {1}", Code, Description); }
        }
        #endregion
    }
}
