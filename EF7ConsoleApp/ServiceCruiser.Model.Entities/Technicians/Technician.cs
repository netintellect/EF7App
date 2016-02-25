using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Capacities;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Validations.Core.Common.Utility;
using ServiceCruiser.Model.Validations.Core.Validators;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Entities.FieldServices;

namespace ServiceCruiser.Model.Entities.Technicians
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Technician : User
    {
        #region state
        private int? _vanInventoryLocId;
        [JsonProperty]
        public int? VanInventoryLocId
        {
            get { return _vanInventoryLocId; }
            set { SetProperty(value, ref _vanInventoryLocId, () => VanInventoryLocId); }
        }
        private VanInventoryLoc _vanInventoryLoc;
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
        public VanInventoryLoc VanInventoryLoc
        {
            get { return _vanInventoryLoc; }
            set
            {
                _vanInventoryLoc = value;
                OnPropertyChanged(() => VanInventoryLoc);
            }
        }

        private ObservableCollection<Unavailability> _unavailabilities = new ObservableCollection<Unavailability>();
        [HandleOnNesting] [Aggregation(isComposite: true)] 
        [ObjectCollectionValidator(typeof(Unavailability))]
        [JsonProperty]
        public ICollection<Unavailability> Unavailabilities
        {
            get { return _unavailabilities; }
            set { _unavailabilities = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<TechnicianAddress> _technicianAddresses = new ObservableCollection<TechnicianAddress>();
        [HandleOnNesting] [Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(TechnicianAddress))]
        [JsonProperty]
        public ICollection<TechnicianAddress> TechnicianAddresses
        {
            get { return _technicianAddresses; }
            set { _technicianAddresses = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<WorkDay> _workDays = new ObservableCollection<WorkDay>();
        [HandleOnNesting] [Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(WorkDay))]
        [JsonProperty]
        public ICollection<WorkDay> WorkDays
        {
            get { return _workDays; }
            set { _workDays = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<TechnicianRole> _technicianRoles = new ObservableCollection<TechnicianRole>();
        [HandleOnNesting] [Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(TechnicianRole))]
        [JsonProperty]
        public ICollection<TechnicianRole> TechnicianRoles
        {
            get { return _technicianRoles; }
            set { _technicianRoles = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<Visit> _visits = new ObservableCollection<Visit>();
        [HandleOnNesting]
        [Aggregation(isComposite: false)]
        [JsonProperty]
        public ICollection<Visit> Visits
        {
            get { return _visits; }
            set { _visits = value != null ? value.ToObservableCollection() : null; }
        }
        
        public ObservableCollection<ContractModel> ContractModels
        {
            get
            {
                var contractModels = new ObservableCollection<ContractModel>();
                if (!TechnicianRoles.Any()) return null;

                var contracts = TechnicianRoles.Select(tr => tr.ContractModel).ToList();
                contracts.ForEach(c => { if (contractModels.All(cm => cm.Id != c.Id)) contractModels.Add(c); });

                // check the contractmodels attached to technician
                var skills = TechnicianRoles.SelectMany(tr => tr.Skills)
                                            .ToList();
                // check all the regions attached to technician
                var regions = TechnicianRoles.SelectMany(tr => tr.Regions)
                                             .ToList();

                foreach (var contract in contracts)
                {
                    var currentContract = contract;
                    currentContract.Skills.Clear();
                    currentContract.Regions.Clear();
                    skills.Where(sd => sd.SkillDefinition.ContractModelId == contract.Id)
                          .ToList()
                          .ForEach(sd => currentContract.Skills.Add(sd));
                    regions.Where(r => r.ContractModelId == contract.Id)
                                        .ToList()
                                        .ForEach(r => currentContract.Regions.Add(r));

                }
                return contractModels;
            }
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
        
        private DateTimeOffset  _targetWorkDayDate;
        public DateTimeOffset TargetWorkDayDate
        {
            get { return _targetWorkDayDate; }
            set
            {
                _targetWorkDayDate = value;
                OnPropertyChanged(() => TargetWorkDayDate);
                OnPropertyChanged(() => TargetAssignment);
            }
        }
        
        private int _targetRegionId;
        public int TargetRegionId
        {
            get { return _targetRegionId; }
            set
            {
                _targetRegionId = value;
                OnPropertyChanged(() => TargetRegionId);
                OnPropertyChanged(() => TargetAssignment);
            }
        }
        
        public Assignment TargetAssignment
        {
            get
            {
                var targetShift = TechnicianRoles.Where(tr => tr.Regions.Any(r => r.Id == TargetRegionId))
                                                 .SelectMany(tr => tr.Assignments)
                                                 .FirstOrDefault(s => s.WorkDay != null &&
                                                                      s.WorkDay.Date.Date == TargetWorkDayDate.Date);

                return targetShift;
            }
        }
        #endregion
        
        #region behavior
        public void SetTargets(ContractShift contractShift, DateTimeOffset workDayDate, int regionId)
        {
            TargetWorkDayDate = workDayDate;
            TargetRegionId = regionId;
            if (TargetAssignment != null)
                TargetAssignment.ContractShift = contractShift;
        }
        
        public Assignment SetTargetAssignment(int regionId, DateTimeOffset dateTime)
        {
            if (regionId == 0) return null;
            if (dateTime == DateTimeOffset.MinValue) return null;

            var technicianRole = TechnicianRoles.FirstOrDefault(tr => tr.Regions.Any(r => r.Id == regionId));
            var workDay = WorkDays.FirstOrDefault(wd => wd.Date.Date == dateTime.Date);
            if (technicianRole == null || workDay == null) return null;

            var shift = Assignment.Create(technicianRole, workDay);

            technicianRole.Assignments.Add(shift);

            return shift;
        }

        public void SetTargetShift(Assignment assignment)
        {
            if (TargetRegionId == 0) return;
            if (TargetWorkDayDate.Date == DateTime.MinValue) return;

            var technicianRole = TechnicianRoles.FirstOrDefault(tr => tr.Regions.Any(r => r.Id == TargetRegionId));
            var workDay = WorkDays.FirstOrDefault(wd => wd.Date.Date == TargetWorkDayDate.Date);
            if (technicianRole == null || workDay == null) return;

            var oldAssignment = technicianRole.Assignments.FirstOrDefault(s => s.WorkDay.Date.Equals(workDay.Date));
            if (oldAssignment != null) technicianRole.Assignments.Remove(oldAssignment);
            technicianRole.Assignments.Add(assignment);

            OnPropertyChanged(() => TargetAssignment);
        }

        public void RefreshState()
        {
            OnPropertyChanged(() => DisplayName);
        }
        #endregion
    }
}
