using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Data;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Company : ValidatedEntity<Company>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			
		private string _name;
		[JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "CompanyName")]
		public string Name 
		{ 
 			get {   return _name;   } 
 			set {   SetProperty(value,ref _name, () => Name);   } 
		}

		private string _vat;
		[JsonProperty]
		[Display(ResourceType = typeof(Translations), Name ="CompanyVAT")]
		public string Vat 
		{ 
 			get {   return _vat;    } 
 			set {   SetProperty(value,ref _vat, () => Vat); } 
		}
    			
		private ObservableCollection<User> _workers = new ObservableCollection<User>();
		[JsonProperty]
		public ICollection<User> Workers 
		{
			get {   return _workers;    } 
			set {   _workers= value != null ? value.ToObservableCollection() : null;    } 
		}
        
		private ObservableCollection<Product> _products = new ObservableCollection<Product>();
		[JsonProperty]
		public ICollection<Product> Products 
		{
			get {   return _products;   } 
			set {   _products= value != null ? value.ToObservableCollection() : null;   }
        }
        private ObservableCollection<Part> _parts = new ObservableCollection<Part>();
        [JsonProperty]
        public ICollection<Part> Parts
        {
            get { return _parts; }
            set { _parts = value != null ? value.ToObservableCollection() : null; }
        }
        #endregion
    }
}
