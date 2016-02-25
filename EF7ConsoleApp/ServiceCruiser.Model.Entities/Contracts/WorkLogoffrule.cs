using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkLogoffRule : ValidatedEntity<WorkLogoffRule>
    {
        #region state
        private int _id;
        [JsonProperty, Key(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private int _workSpecificationId;
        [JsonProperty]
        public int WorkSpecificationId
        {
            get { return _workSpecificationId; }
            set { SetProperty(value, ref _workSpecificationId, () => WorkSpecificationId); }
        }

        private WorkSpecification _workSpecification;
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: false)]
        public WorkSpecification WorkSpecification
        {
            get { return _workSpecification; }
            set
            {
                _workSpecification = value;
                OnPropertyChanged(() => WorkSpecification);
            }
        }
        private int _partId;
        [JsonProperty]
        public int PartId
        {
            get { return _partId; }
            set { SetProperty(value, ref _partId, () => PartId); }
        }

        private Part _part;
        [JsonProperty, Aggregation(isComposite: false)]
        public Part Part
        {
            get { return _part; }
            set
            {
                _part = value;
                OnPropertyChanged(() => Part);
            }
        }

        private bool _mandatory;
        [JsonProperty]
        public bool Mandatory
        {
            get { return _mandatory; }
            set
            {
                _mandatory = value;
                OnPropertyChanged(() => Mandatory);
            }
        }

        #endregion
    }
}
