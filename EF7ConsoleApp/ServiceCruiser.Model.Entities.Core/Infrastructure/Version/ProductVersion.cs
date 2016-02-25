using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Infrastructure.Version;

namespace ServiceCruiser.Model.Entities.Core.Infrastructure
{
    [JsonObject(MemberSerialization.OptIn)]
    public  class ProductVersion : AppVersion
    {
        #region state

        [JsonProperty]
        public ObservableCollection<ModuleVersion> ModuleVersions { get; } = new ObservableCollection<ModuleVersion>();

        #endregion
    }
}
