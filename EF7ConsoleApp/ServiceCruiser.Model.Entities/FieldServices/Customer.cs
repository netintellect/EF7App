using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Users;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Customer : ValidatedEntity<Customer>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
        
        private string _externalReference;
        [JsonProperty]
        public string ExternalReference
        {
            get { return _externalReference; }
            set { SetProperty(value, ref _externalReference, () => ExternalReference);  }
        }
        
        private bool _isMaster;
        [JsonProperty]
        public bool IsMaster
        {
            get { return _isMaster; }
            set { SetProperty(value, ref _isMaster, () => IsMaster); }
        }
        
        private string _companyName;
        [JsonProperty]
        public string CompanyName
        {
            get { return _companyName; }
            set { SetProperty(value, ref _companyName, () => CompanyName); }
        }
        
        private string _vat;
        [JsonProperty]
        public string Vat
        {
            get { return _vat; }
            set { SetProperty(value, ref _vat, () => Vat); }
        }
        
        private ObservableCollection<Location> _locations= new ObservableCollection<Location>();
		[JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
		public ICollection<Location> Locations 
		{
			get{return _locations;} 
			set{_locations= value != null ? value.ToObservableCollection() : null;}
        }

        private ObservableCollection<ServiceOrder> _serviceOrders = new ObservableCollection<ServiceOrder>();
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
        public ICollection<ServiceOrder> ServiceOrders
        {
            get { return _serviceOrders; }
            set { _serviceOrders = value != null ? value.ToObservableCollection() : null; }
        }

        private int _contractingCompanyId;
        [JsonProperty, Aggregation]
        public int ContractingCompanyId
        {
            get { return _contractingCompanyId; }
            set
            {
                _contractingCompanyId = value;
                OnPropertyChanged(() => ContractingCompanyId);
            }
        }

        private ContractingCompany _contractingCompany;
        [JsonProperty]
        public ContractingCompany ContractingCompany
        {
            get { return _contractingCompany; }
            set
            {
                _contractingCompany = value;
                OnPropertyChanged(() => ContractingCompany);
            }
        }

        public static Customer Create(User user)
        {
            var customer = new Customer();
            
            if (user != null)
                customer.SetAuditInfo(user.Login);
            return customer;
        }

        #endregion
    }
}
