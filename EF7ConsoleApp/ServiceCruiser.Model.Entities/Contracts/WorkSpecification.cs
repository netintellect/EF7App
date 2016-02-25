using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Entities.Repositories;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkSpecification : ValidatedEntity<WorkSpecification>
    {
        #region state
        private int _id;
        [JsonProperty, KeyNew(true)]
        [Display(ResourceType = typeof(Translations), Name = "WorkSpecificationId")]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }

        private string _code;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "WorkSpecificationCode")]
        public string Code
        {
            get {   return _code; }
            set {   SetProperty(value, ref _code, () => Code);  }
        }

        private string _description;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "WorkSpecificationDescription")]
        public string Description
        {
            get {   return _description; }
            set {   SetProperty(value, ref _description, () => Description);    }
        }

        [JsonProperty]
        public int? DseInfoId { get; set; }
        [JsonProperty]
        public DseInfo DseInfo { get; set; }
        
        private ServiceSpecification _serviceSpecification;
        [JsonProperty]
        public ServiceSpecification ServiceSpecification
        {
            get
            {
                if (_serviceSpecification != null || !UseRepositoryFinder) return _serviceSpecification;

                var repository = RepositoryFinder.GetRepository<IServiceSpecificationRepository>() as IServiceSpecificationRepository;
                return repository?.Filter(ws => ws.Id == ServiceSpecificationId).FirstOrDefault();
            }
            set 
            {   
                _serviceSpecification = value;
                OnPropertyChanged(() => ServiceSpecification);
            }
        }

        private int? _serviceSpecificationId;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "WorkSpecificationServiceSpecificationId")]
        public int? ServiceSpecificationId
        {
            get {   return _serviceSpecificationId; }
            set {   SetProperty(value, ref _serviceSpecificationId, () => ServiceSpecificationId);  }
        }

        private bool _areTasksMutuallyExclusive;
        [JsonProperty]
        public bool AreTasksMutuallyExclusive
        {
            get { return _areTasksMutuallyExclusive; }
            set
            {
                _areTasksMutuallyExclusive = value;
                OnPropertyChanged(() => AreTasksMutuallyExclusive);
            }
        }

        private int? _resultCategoryId;
        [JsonProperty]
        public int? ResultCategoryId
        {
            get {   return _resultCategoryId; }
            set {   SetProperty(value, ref _resultCategoryId, () => ResultCategoryId);  }
        }

        private ObservableCollection<WorkOrder> _workOrders = new ObservableCollection<WorkOrder>();
        [JsonProperty]
        public ICollection<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders = value?.ToObservableCollection(); }
        }

        private ObservableCollection<TaskSpecification> _taskSpecifications = new ObservableCollection<TaskSpecification>();
        [HandleOnNesting] [Aggregation(isComposite: true)] [ObjectCollectionValidator(typeof(TaskSpecification))]
        [JsonProperty]
        public ICollection<TaskSpecification> TaskSpecifications
        {
            get
            {
                if (_taskSpecifications != null || !UseRepositoryFinder) return _taskSpecifications;

                var repository = RepositoryFinder.GetRepository<ITaskSpecificationRepository>() as ITaskSpecificationRepository;
                return repository?.Filter(ts => ts.WorkSpecId == Id).ToObservableCollection();
            }
            set { _taskSpecifications = value?.ToObservableCollection(); }
        }

        private ResultCategory _resultCategory;
        [JsonProperty]
        public ResultCategory ResultCategory
        {
            get { return _resultCategory; }
            set 
            {   
                _resultCategory = value;
                OnPropertyChanged(() => ResultCategory);
            }
        }
                
        private int? _attributeSpecId;
        [JsonProperty]
        public int? AttributeSpecId
        {
            get { return _attributeSpecId; }
            set { SetProperty(value, ref _attributeSpecId, () => AttributeSpecId); }
        }
        private AttributeGroup _attributeSpecs;
        [JsonProperty]
        public AttributeGroup AttributeSpecs
        {
            get { return _attributeSpecs; }
            set { SetProperty(value, ref _attributeSpecs, () => AttributeSpecs); }
        }

        private int? _bookingCategoryId;
        [JsonProperty]
        public int? BookingCategoryId
        {
            get { return _bookingCategoryId; }
            set { SetProperty(value, ref _bookingCategoryId, () => BookingCategoryId); }
        }

        private ResultCategory _bookingCategory;
        [JsonProperty]
        public ResultCategory BookingCategory
        {
            get { return _bookingCategory; }
            set
            {
                _bookingCategory = value;
                OnPropertyChanged(() => BookingCategory);
            }
        }
        
        private ObservableCollection<WorkLogoffRule> _workLogoffRules = new ObservableCollection<WorkLogoffRule>();
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true), ObjectCollectionValidator(typeof(WorkLogoffRule))]
        public ICollection<WorkLogoffRule> WorkLogoffRules
        {
            get { return _workLogoffRules; }
            set { _workLogoffRules = value?.ToObservableCollection(); }
        }


        private ObservableCollection<AppointmentWindow> _appointmentWindows;
        [JsonProperty, HandleOnNesting, Aggregation(isIndependent: true)]
        public ICollection<AppointmentWindow> AppointmentWindows
        {
            get { return _appointmentWindows; }
            set
            { 
                _appointmentWindows = value?.ToObservableCollection();
                OnPropertyChanged(() => AppointmentWindows);
                OnPropertyChanged(() => WithAppointment);
            }
        }

        private ObservableCollection<TravelTime> _travelTimes = new ObservableCollection<TravelTime>();
        [JsonProperty, Aggregation]
        public ICollection<TravelTime> TravelTimes
        {
            get { return _travelTimes;  }
            set { _travelTimes = value?.ToObservableCollection(); }
        }

        public string DisplayItem
        {
            get
            {
                string description = string.Empty;
                if (Description != null)
                    description = Description.Length > 45 ? Description.Substring(0, 45) + "..." : Description;
                if (!string.IsNullOrEmpty(Code) &&
                    !string.IsNullOrEmpty(Description))
                {
                    return $"{Code} - {description}";
                }
                if (string.IsNullOrEmpty(Code))
                    return description;
                return Code;
            }
        }

        public string DisplaySpecification => $"{Code} - {Description}";

        public string DisplayContractModel => ServiceSpecification?.ContractModel?.ContractingCompany != null ? $"{ServiceSpecification.ContractModel.ContractingCompany.Name}/{ServiceSpecification.ContractModel.Name}" : "?";

        public int TotalOnsiteTime
        {
            get
            {
                return TaskSpecifications.Where(taskSpecification => taskSpecification.OnsiteTime.HasValue).Sum(taskSpecification => taskSpecification.OnsiteTime.Value);
            }
        }
        
        public bool WithAppointment
        {
            get {   return AppointmentWindows != null && AppointmentWindows.Any(); }
        }

        #endregion

        #region behavior

        public void SetAttributeGroupTree(AttributeGroup attributeGroup)
        {
            //Set the AttributeSpec without raising attributechange and keeping HasChanges() on false
            if (attributeGroup == null) return;
            if (_attributeSpecId == attributeGroup.Id)
                _attributeSpecs = attributeGroup;
        }

        public void RefreshState()
        {
            OnPropertyChanged(() => Code,
                              () => Description,
                              () => DisplayItem);
            OnPropertyChanged(() => WithAppointment);
        }

        #endregion
    }
}
