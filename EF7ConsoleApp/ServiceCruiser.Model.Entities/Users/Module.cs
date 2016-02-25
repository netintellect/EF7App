using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Users
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Module : ValidatedEntity<Module>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			
        private ModuleType _type;
		[JsonProperty]
		public ModuleType Type 
		{ 
 			get {   return _type;   } 
 			set {   SetProperty(value,ref _type, () => Type);   } 
		}
    			
		private string _name;
		[JsonProperty]
		public string Name 
		{ 
 			get {   return _name;   } 
 			set {   SetProperty(value,ref _name, () => Name);   } 
		}

        private bool _needsPermissions;
        [JsonProperty]
        public bool NeedsPermissions
        {
            get { return _needsPermissions; }
            set { SetProperty(value, ref _needsPermissions, () => NeedsPermissions); }
        }

        private bool _onlyInNonProduction;
        [JsonProperty]
        public bool OnlyInNonProduction
        {
            get { return _onlyInNonProduction; }
            set { SetProperty(value, ref _onlyInNonProduction, () => OnlyInNonProduction); }
        }

		private ObservableCollection<ModuleAction> _actions = new ObservableCollection<ModuleAction>();
		[JsonProperty]
		public ICollection<ModuleAction> Actions 
		{
			get {   return _actions;    } 
			set {   _actions= value != null ? value.ToObservableCollection() : null;    }    
		}
        
		private ObservableCollection<Product> _products = new ObservableCollection<Product>();
		[JsonProperty]
		public ICollection<Product> Products 
		{
			get {   return _products;} 
			set {   _products= value != null ? value.ToObservableCollection() : null;   }
        }
        #endregion
    }
}
