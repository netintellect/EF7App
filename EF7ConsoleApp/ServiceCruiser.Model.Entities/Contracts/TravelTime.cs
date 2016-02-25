using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.FieldServices;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TravelTime : ValidatedEntity<TravelTime>
    {
        #region state
        private int _id;
        [JsonProperty, Key(isIdentity: true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        
        private int _meanTime;
        [JsonProperty]
        public int MeanTime
        {
            get { return _meanTime; }
            set
            {
                _meanTime = value;
                OnPropertyChanged(() => MeanTime);
            }
        }

        #region region
        private int _regionId;
        [JsonProperty]
        public int RegionId
        {
            get { return _regionId; }
            set { SetProperty(value, ref _regionId, () => RegionId); }
        }
        
        private Region  _region;
        [JsonProperty, Aggregation]
        public Region Region
        {
            get { return _region; }
            set
            {
                _region = value;
                OnPropertyChanged(() => Region);
            }
        }
        #endregion

        #region work specification
        private int _workSpecificationId;
        [JsonProperty]
        public int WorkSpecificationId
        {
            get { return _workSpecificationId; }
            set { SetProperty(value, ref _workSpecificationId, () => WorkSpecificationId);  }
        }


        private WorkSpecification _workSpecification;
        [JsonProperty, Aggregation]
        public WorkSpecification WorkSpecification
        {
            get { return _workSpecification; }
            set
            {
                _workSpecification = value;
                OnPropertyChanged(() => WorkSpecification);
            }
        }
        #endregion

        #endregion
    }
}
