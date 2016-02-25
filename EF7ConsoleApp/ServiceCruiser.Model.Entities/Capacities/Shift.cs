using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using period = ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Capacities
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Shift : ValidatedEntity<Shift>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }

        private string _name;
        [JsonProperty]
        public string Name
        {
            get {   return _name; }
            set {   SetProperty(value, ref _name, () => Name);  }
        }
        
        private TimeSpan _start;
        [JsonProperty]
        public TimeSpan Start
        {
            get {   return _start; }
            set
            {
                if (SetProperty(value, ref _start, () => Start))
                {
                    OnPropertyChanged(() => WorkDayRange);
                }
            }
        }
        
        private TimeSpan _duration;
        [JsonProperty]
        public TimeSpan Duration
        {
            get {   return _duration; }
            set
            {
                if (SetProperty(value, ref _duration, () => Duration))
                {
                    OnPropertyChanged(() => WorkDayRange);
                }
            }
        }


        private TimeSpan _breakStart;
        [JsonProperty]
        public TimeSpan BreakStart
        {
            get {   return _breakStart; }
            set {   SetProperty(value, ref _breakStart, () => BreakStart);  }
        }


        private TimeSpan _breakWindowDuration;
        [JsonProperty]
        public TimeSpan BreakWindowDuration
        {
            get {   return _breakWindowDuration; }
            set {   SetProperty(value, ref _breakWindowDuration, () => BreakWindowDuration);    }
        }


        private int _breakDuration;
        [JsonProperty]
        public int BreakDuration
        {
            get {   return _breakDuration; }
            set {   SetProperty(value, ref _breakDuration, () => BreakDuration);    }
        }


        private TimeSpan _breakStartCap;
        [JsonProperty]
        public TimeSpan BreakStartCap
        {
            get {   return _breakStartCap; }
            set {   SetProperty(value, ref _breakStartCap, () => BreakStartCap);    }
        }
        
        public period.TimeRange WorkDayRange
        {
            get
            {
                return new period.TimeRange(Start, Start.Add(Duration));
            }
        }

        public period.ITimeRange BreakWindowRange
        {
            get
            {
                return new period.TimeRange(BreakStart, BreakStart.Add(BreakWindowDuration));
            }
        }

        public period.ITimeRange MostOftenBreakRange
        {
            get
            {
                return new period.TimeRange(BreakStartCap, BreakStartCap.Add(BreakDurationRange.Duration));
            }
        }

        public period.ITimeRange BreakDurationRange
        {
            get
            {
                return new period.TimeRange(period.Duration.Minutes(0), period.Duration.Minutes(BreakDuration));
            }
        }

        public period.ITimeRange WorkingHoursRange
        {
            get
            {
                var copyWorkDayRange = new period.TimeRange(WorkDayRange);
                copyWorkDayRange.ShrinkTo(new period.TimeRange(BreakDurationRange));
                return copyWorkDayRange;
            }
        }

        public double WorkingHoursDuration
        {
            get { return WorkingHoursRange.Duration.TotalMinutes; }
        }

        public TimeSpan WorkDuration
        {
            get { return Duration.Subtract(new TimeSpan(0, BreakDuration, 0)); }
        }

        public string Slot
        {
            get { return Start.Hours + " - " + (Start.Hours + Duration.Hours); }
        }

        public string Win
        {
            get { return BreakStart.Hours + " - " + (BreakWindowDuration.Hours + BreakStart.Hours); }
        }

        public override string ToString()
        {
            return "[WorkDayBaseDef id=" + Id + " start=" + Start + " dur.=" + Duration + " breakStart=" + BreakStart + " breakDur=" + BreakDuration + ']';
        }
        #endregion
    }
}