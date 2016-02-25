using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Entities.Repositories;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ServiceSpecification : ValidatedEntity<ServiceSpecification>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id
        {
            get { return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }

        private string _code;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "ServiceSpecificationCode")]
        public string Code
        {
            get {   return _code; }
            set {
                if (SetProperty(value, ref _code, () => Code))
                {
                    OnPropertyChanged(() => DisplaySpecification);
                }  
            }
        }


        private string _description;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "ServiceSpecificationDescription")]
        public string Description
        {
            get {   return _description; }
            set
            {
                if (SetProperty(value, ref _description, () => Description))
                {
                    OnPropertyChanged(() => DisplaySpecification);
                }
            }
        }
        
        private DateTimeOffset? _validFrom;
        [JsonProperty]
        public DateTimeOffset? ValidFrom
        {
            get { return _validFrom; }
            set {   SetProperty(value, ref _validFrom, () => ValidFrom);    }
        }


        private DateTimeOffset? _validTo;
        [JsonProperty]
        public DateTimeOffset? ValidTo
        {
            get {   return _validTo; }
            set {   SetProperty(value, ref _validTo, () => ValidTo);    }
        }
       
         
        private ObservableCollection<ServiceOrder> _serviceOrders = new ObservableCollection<ServiceOrder>();
        [JsonProperty]
        public ICollection<ServiceOrder> ServiceOrders
        {
            get { return _serviceOrders; }
            set { _serviceOrders = value?.ToObservableCollection(); }
        }

        private ObservableCollection<WorkSpecification> _workSpecifications = new ObservableCollection<WorkSpecification>();
        [HandleOnNesting] [Aggregation(isComposite:true)] [ObjectCollectionValidator(typeof(WorkSpecification))]
        [JsonProperty]
        public ICollection<WorkSpecification> WorkSpecifications
        {
            get
            {
                if (_workSpecifications.Any() || !UseRepositoryFinder) return _workSpecifications;

                var repository = RepositoryFinder.GetRepository<IWorkSpecificationRepository>() as IWorkSpecificationRepository;
                return repository?.Filter(ws => ws.ServiceSpecificationId == Id);
            }
            set
            {
                _workSpecifications = value?.ToObservableCollection();
            }
        }


        private int _contractModelId;
        [JsonProperty]
        public int ContractModelId
        {
            get { return _contractModelId; }
            set { SetProperty(value, ref _contractModelId, () => ContractModelId); }
        }

        private ContractModel _contractModel;
        [JsonProperty]
        public ContractModel ContractModel
        {
            get
            {
                if (_contractModel != null || !UseRepositoryFinder) return _contractModel;

                var repository = RepositoryFinder.GetRepository<IContractModelRepository>() as IContractModelRepository;
                return repository?.Filter(cm => cm.Id == ContractModelId).FirstOrDefault();
            }
            set 
            {   
                _contractModel = value;
                OnPropertyChanged(() => ContractModel);
            }
        }

        private int? _attributeSpecId;
        [JsonProperty]
        public int? AttributeSpecId
        {
            get { return _attributeSpecId; }
            set { SetProperty(value, ref _attributeSpecId, () => AttributeSpecId); }
        }
        private AttributeGroup _attributeSpec;
        [JsonProperty]
        public AttributeGroup AttributeSpec
        {
            get { return _attributeSpec; }
            set { SetProperty(value, ref _attributeSpec, () => AttributeSpec); }
        }

        public string DisplaySpecification => $"{Code} - {Description}";

        #endregion
    }
}
