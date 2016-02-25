using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.Core.Infrastructure.Version
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ModuleVersion : AppVersion
    {
        #region state
        private string _name;

        [JsonProperty]
        private string Name
        {
            get { return _name;}
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }    
        #endregion
    }
}
