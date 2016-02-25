using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class DseSpecificationBase : ValidatedEntity<DseSpecificationBase>
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

        private bool _endBased;
        [JsonProperty]
        public bool EndBased
        {
            get { return _endBased; }
            set { SetProperty(value, ref _endBased, () => EndBased); }
        }

        private bool _jeopardyException;
        [JsonProperty]
        public bool JeopardyException
        {
            get { return _jeopardyException; }
            set { SetProperty(value, ref _jeopardyException, () => JeopardyException); }
        }

        [JsonProperty]
        public int DseInfoId { get; set; }

        #endregion
    }
}
