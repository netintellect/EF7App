using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ServiceCruiser.Model.Entities.Contracts
{

    [JsonObject(MemberSerialization.OptIn), DeletePriority(90)]
    public class AppointmentWindow : ValidatedEntity<AppointmentWindow>
    {
        #region state
        private int _id;
        [JsonProperty]
        [Key(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
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

        private TimeSpan _windowStart;
        [JsonProperty]
        public TimeSpan WindowStart
        {
            get { return _windowStart; }
            set { SetProperty(value, ref _windowStart, () => WindowStart); }
        }

        private TimeSpan _windowEnd;
        [JsonProperty]
        public TimeSpan WindowEnd
        {
            get { return _windowEnd; }
            set { SetProperty(value, ref  _windowEnd, () => WindowEnd); }
        }

        private ObservableCollection<Visit> _visits = new ObservableCollection<Visit>();
        [JsonProperty, HandleOnNesting, Aggregation]
        public ICollection<Visit> Visits
        {
            get { return _visits; }
            set { _visits = value != null ? value.ToObservableCollection() : null; }
        }

        private ContractModel _contractModel;
        [JsonProperty]
        public ContractModel ContractModel
        {
            get { return _contractModel; }
            set
            {
                _contractModel = value;
                OnPropertyChanged(() => ContractModel);
            }
        }

        private int? _contractModelid;
        [JsonProperty]
        public int? ContractModelId
        {
            get { return _contractModelid; }
            set { SetProperty(value, ref _contractModelid, () => ContractModelId); }
        }

        private ObservableCollection<WorkSpecification> _workSpecifications = new ObservableCollection<WorkSpecification>();
        [JsonProperty, HandleOnNesting, Aggregation]
        public ICollection<WorkSpecification> WorkSpecifications
        {
            get { return _workSpecifications; }
            set { _workSpecifications = value != null ? value.ToObservableCollection() : null; }
        }
        #endregion

        #region behavior
       
        #endregion
    }
}
