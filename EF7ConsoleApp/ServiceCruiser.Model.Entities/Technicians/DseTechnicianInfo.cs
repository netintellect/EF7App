using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.Technicians
{
    public class DseTechnicianInfo:ValidatedEntity<DseTechnicianInfo>
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

        private TimeSpan _maxTravel;
        [JsonProperty]
        public TimeSpan MaxTravel
        {
            get { return _maxTravel; }
            set { SetProperty(value, ref _maxTravel, () => MaxTravel); }
        }

        private TimeSpan _travelTo;
        [JsonProperty]
        public TimeSpan TravelTo
        {
            get { return _travelTo; }
            set { SetProperty(value, ref _travelTo, () => TravelTo); }
        }

        private TimeSpan _travelFrom;
        [JsonProperty]
        public TimeSpan TravelFrom
        {
            get { return _travelFrom; }
            set { SetProperty(value, ref _travelFrom, () => TravelFrom); }
        }

        private double _costPerHour;
        [JsonProperty]
        public double CostPerHour
        {
            get { return _costPerHour; }
            set { SetProperty(value, ref _costPerHour, () => CostPerHour); }
        }

        private double _costPerHourOvertime;
        [JsonProperty]
        public double CostPerHourOvertime
        {
            get { return _costPerHourOvertime; }
            set { SetProperty(value, ref _costPerHourOvertime, () => CostPerHourOvertime); }
        }

        private double _costPerKm;
        [JsonProperty]
        public double CostPerKm
        {
            get { return _costPerKm; }
            set { SetProperty(value, ref _costPerKm, () => CostPerKm); }
        }

        private double _speedfactor;
        [JsonProperty]
        public double Speedfactor
        {
            get { return _speedfactor; }
            set { SetProperty(value, ref _speedfactor, () => Speedfactor); }
        }

        private double _outOfRegionMultiplier;
        [JsonProperty]
        public double OutOfRegionMultiplier
        {
            get { return _outOfRegionMultiplier; }
            set { SetProperty(value, ref _outOfRegionMultiplier, () => OutOfRegionMultiplier); }
        }

        #endregion
    }
}
