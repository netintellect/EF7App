using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ContractingCompany : Company
    {
        #region state
        private ObservableCollection<ContractModel> _contractModels = new ObservableCollection<ContractModel>();
        [JsonProperty]
        public ICollection<ContractModel> ContractModels
        {
            get { return _contractModels; }
            set { _contractModels = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        [JsonProperty]
        public ICollection<Customer> Customers
        {
            get { return _customers; }
            set { _customers = value != null ? value.ToObservableCollection() : null; }
        }

        #endregion
    }
}
