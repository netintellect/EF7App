using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Resources;
using ContractModel = ServiceCruiser.Model.Entities.Contracts.ContractModel;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResultCode : ValidatedEntity<ResultCode>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			
		private string _code;
		[JsonProperty]
		public string Code 
		{ 
 			get {   return _code;   }
		    set
		    {
		        if (SetProperty(value, ref _code, () => Code))
		        {
		            OnPropertyChanged(() => DisplayItem);   
		        }
		    } 
		}
    			
		private ResultCodeOutcomeType _outcome;
		[JsonProperty]
		public ResultCodeOutcomeType Outcome 
		{ 
 			get {   return _outcome;    } 
 			set {   SetProperty(value,ref _outcome, () => Outcome); } 
		}

        private string _name;
        [JsonProperty]
        public string Name
        {
            get { return _name; }
            set { SetProperty(value, ref _name, () => Name); }
        }

        private string _description;
        [JsonProperty]
        public string Description
        {
            get { return _description; }
            set
            {
                if (SetProperty(value, ref _description, () => Description))
                {
                    OnPropertyChanged(() => DisplayItem);
                    OnPropertyChanged(() => ShortDescription);
                }
            }
        }

		private int _contractModelId;
		[JsonProperty]
		public int ContractModelId 
		{ 
 			get {   return _contractModelId;    } 
 			set {   SetProperty(value,ref _contractModelId, () => ContractModelId); } 
		}

        private bool _taskDerived;
        [JsonProperty]
        public bool TaskDerived
        {
            get { return _taskDerived; }
            set { SetProperty(value, ref _taskDerived, () => TaskDerived); }
        }

        private bool _signatureRequired;
        [JsonProperty]
        public bool SignatureRequired
        {
            get { return _signatureRequired; }
            set { SetProperty(value, ref _signatureRequired, () => SignatureRequired); }
        }
        
		private ObservableCollection<Task> _tasks = new ObservableCollection<Task>();
		[JsonProperty]
		public ICollection<Task> Tasks 
		{
			get {   return _tasks;  } 
			set {   _tasks= value != null ? value.ToObservableCollection() : null;} 
		}
        
		private ObservableCollection<Visit> _visits = new ObservableCollection<Visit>();
		[JsonProperty]
		public ICollection<Visit> Visits 
		{
			get {   return _visits; } 
			set {   _visits= value != null ? value.ToObservableCollection() : null; } 
		}
        
		private ContractModel _contractModel;
		[JsonProperty]
		public ContractModel ContractModel 
		{
			get {   return _contractModel;} 
			set
            {
				_contractModel= value;
				OnPropertyChanged(() => ContractModel);
		    } 
		}
        
		private ObservableCollection<ResultCategory> _resultCategories = new ObservableCollection<ResultCategory>();
		[JsonProperty]
		public ICollection<ResultCategory> ResultCategories 
		{
			get {   return _resultCategories;   } 
			set {   _resultCategories= value != null ? value.ToObservableCollection() : null;   }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(() => IsSelected);
            }
        }

        private static IDictionary<int, string> _possibleOutComeValues;
        public static IDictionary<int, string> PossibleOutComeValues
        {
            get { return _possibleOutComeValues = _possibleOutComeValues ?? CreatePossibleOutComeValues(); }
        }

        public string DisplayItem
        {
            get
            {
                return string.Format("{0} - {1}", Code ?? "?", Description ?? "?");
            }
        }
        public string DisplayOutCome
        {
            get
            {
                string translation;
                if (PossibleOutComeValues.TryGetValue((int)Outcome, out translation))
                    return translation;
                return "?";
            }
        }
        #endregion

        #region behavior

        private static IDictionary<int, string> CreatePossibleOutComeValues()
        {
            IDictionary<int, string> outComeValues = new Dictionary<int, string>();
            foreach (var enumValue in Enum.GetValues(typeof(ResultCodeOutcomeType)))
            {
                string translation;
                switch ((int)enumValue)
                {
                    case (int)ResultCodeOutcomeType.Done:
                        translation = Translations.ResultCodeOutcomeDone;
                        break;
                    case (int)ResultCodeOutcomeType.Redo:
                        translation = Translations.ResultCodeOutcomeRedo;
                        break;
                    case (int)ResultCodeOutcomeType.Abort:
                        translation = Translations.ResultCodeOutcomeAbort;
                        break;
                    default:
                        translation = "?";
                        break;
                }
                outComeValues.Add((int)enumValue, translation);
            }
            return outComeValues;
        }

        public string ShortDescription
        {
            get
            {
                if (string.IsNullOrEmpty(Description)) return null;
                if (Description.Length < 31) return Description;
                return $"{Description.Substring(0, 30)}...";
            }
        }
        #endregion
    }
}
