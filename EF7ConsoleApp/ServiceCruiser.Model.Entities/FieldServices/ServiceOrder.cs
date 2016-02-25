using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Infrastructure;
using ServiceCruiser.Model.Entities.Core.Utilities;
using ServiceCruiser.Model.Entities.Repositories;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn), DeletePriority(-1)]
    public class ServiceOrder : ValidatedEntity<ServiceOrder>, IAttributeContainer
    {
        #region state
        [JsonProperty, IgnoreOnMap]
        public int ContractModelId { get; set; }

        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}

        private string _externalRef;
		[JsonProperty]
		public string ExternalRef 
		{ 
 			get {   return _externalRef;    } 
 			set {   SetProperty(value,ref _externalRef, () => ExternalRef); } 
		}

        private DateTimeOffset _orderDate;
        [DateTimeOffsetRangeValidator("1900-01-01T00:00:00", RangeBoundaryType.Inclusive, "2014-01-01T00:00:01", RangeBoundaryType.Ignore,
                                      MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "ServiceOrderDateRequired")]
        [Display(ResourceType = typeof(Translations), Name ="ServiceOrderOrderDate")]
		[JsonProperty]
		public DateTimeOffset OrderDate 
		{ 
 			get {   return _orderDate;  }
            set
            {
                if (SetProperty(value,ref _orderDate, () => OrderDate))
                {
                    if (OrderDate > StartNoEarlierThan)
                        StartNoEarlierThan = OrderDate; 
                }
            } 
		}
        
        private DateTimeOffset _startNoEarlierThan;
        [DateTimeOffsetRangeValidator("1900-01-01T00:00:00", RangeBoundaryType.Inclusive, "2014-01-01T00:00:01", RangeBoundaryType.Ignore,
                                      MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "ServiceOrderEaliestDateRequired")]
        [PropertyComparisonValidator("OrderDate", ComparisonOperator.GreaterThanEqual,
                                     MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "ServiceOrderStartBeforeOrderDate")]
        [Display(ResourceType = typeof(Translations), Name ="ServiceOrderStartNoEarlierThan")]
        [JsonProperty]
		public DateTimeOffset StartNoEarlierThan 
		{ 
 			get {   return _startNoEarlierThan; } 
 			set {   SetProperty(value,ref _startNoEarlierThan, () => StartNoEarlierThan);   } 
		}

        
        private ServiceStatusType _status;
		[JsonProperty]
		public ServiceStatusType Status 
		{ 
 			get {   return _status;    } 
 			set {   SetProperty(value,ref _status, () => Status);   } 
		}

        private ObservableCollection<ServiceOrderRemark> _serviceOrderRemarks = new ObservableCollection<ServiceOrderRemark>();
        [HandleOnNesting] [Aggregation(isComposite: true)] 
        [ObjectCollectionValidator(typeof(ServiceOrderRemark))]
        [ObjectCollectionValidator(typeof(Remark))]
        [JsonProperty]
		public ICollection<ServiceOrderRemark> ServiceOrderRemarks 
		{
			get{return _serviceOrderRemarks;} 
			set{_serviceOrderRemarks= value?.ToObservableCollection();} 
		}

        private ObservableCollection<ServiceOrderAttachment> _serviceOrderAttachments = new ObservableCollection<ServiceOrderAttachment>();
        [HandleOnNesting, Aggregation(isComposite: true)] [ObjectCollectionValidator(typeof(ServiceOrderAttachment))]
        [JsonProperty]
        public ICollection<ServiceOrderAttachment> ServiceOrderAttachments
        {
            get { return _serviceOrderAttachments; }
            set { _serviceOrderAttachments = value?.ToObservableCollection(); }
        }

        private ObservableCollection<WorkOrder> _workOrders = new ObservableCollection<WorkOrder>();
        [HandleOnNesting] [Aggregation(isComposite: true)] [ObjectCollectionValidator(typeof(WorkOrder))]
        [JsonProperty]
		public ICollection<WorkOrder> WorkOrders 
		{
			get {   return _workOrders; } 
			set {   _workOrders= value?.ToObservableCollection(); } 
		}

        private int _specificationId;
		[JsonProperty]
		public int SpecificationId 
		{ 
 			get {   return _specificationId;    } 
 			set {   SetProperty(value,ref _specificationId, () => SpecificationId); } 
		}

        private ServiceSpecification _serviceSpecification;
        [JsonProperty, Aggregation, HandleOnNesting]
		public ServiceSpecification ServiceSpecification 
		{
            get
            {
                if (_serviceSpecification != null || !UseRepositoryFinder) return _serviceSpecification;
                var repository = RepositoryFinder.GetRepository<IServiceSpecificationRepository>() as IServiceSpecificationRepository;
                return repository?.Filter(ss => ss.Id == SpecificationId).FirstOrDefault();
            } 
			set 
            {   
                _serviceSpecification= value;
				OnPropertyChanged(() => ServiceSpecification);
		    } 
        }

        private ObservableCollection<ServiceOrderHistory> _serviceOrderHistories = new ObservableCollection<ServiceOrderHistory>();
        [JsonProperty]
        [HandleOnNesting, Aggregation(isComposite: true)]
        public ICollection<ServiceOrderHistory> ServiceOrderHistories
        {
            get { return _serviceOrderHistories; }
            set { _serviceOrderHistories = value?.ToObservableCollection(); }
        }

        private ObservableCollection<AttributeValueWrap> _attributes = new ObservableCollection<AttributeValueWrap>();
        [JsonProperty]
        [HandleOnNesting]
        [Aggregation(isComposite: true)]
        public ICollection<AttributeValueWrap> Attributes
        {
            get { return _attributes; }
            set { _attributes = value?.ToObservableCollection(); }
        }

        private int? _customerId;
        [JsonProperty]
        public int? CustomerId
        {
            get {   return _customerId; }
            set { SetProperty(value, ref _customerId, () => CustomerId); }
        }
        
        private Customer _customer;
        [JsonProperty, Aggregation]
        public Customer Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                OnPropertyChanged(() => Customer);
            }
        }
        
        public static ObservableCollection<CodeGroup> PossibleStatuses => StaticFactory.Instance.GetCodeGroups(CodeGroupType.ServiceStatus);

        public ObservableCollection<Remark> Remarks
        {
            get
            {

                return ServiceOrderRemarks.Cast<Remark>()
                                          .OrderByDescending(r => r.EnteredDate)
                                          .ToObservableCollection();
            }
        }

        public string FixCode => null;

        public int? AttributeSpecId => ServiceSpecification?.AttributeSpecId;

        public string DisplayOrderDate
        {
            get
            {
                var fmt = (CultureInfo.CurrentCulture.DateTimeFormat);
                return OrderDate.ToString("d", fmt);
            }
        }

        public string DisplayStartNoEarlierThan
        {
            get
            {
                var fmt = (CultureInfo.CurrentCulture.DateTimeFormat);
                return StartNoEarlierThan.ToString("d", fmt);
            }
        }


        public string DisplayServiceOrder
        {
            get
            {
                string display1 = string.Format(Translations.ServiceOrderDisplayString1, !IsNew ? Id.ToString() : "?",   
                                                                                         DisplayStatus);
                string display2 = string.Format(Translations.ServiceOrderDisplayString2,
                                                OrderDate.Date == DateTime.MinValue ? "?" : OrderDate.ToString("dd/MM/yyyy"));

                return $"{display1}\n{display2}";
            }
        }

        public string DisplaySpecification
        {
            get
            {
                var specification = ServiceSpecification != null ? ServiceSpecification.DisplaySpecification : "?";
                return $"{Id} - {specification}";
            }
        }

        public string DisplayStatus => StaticFactory.Instance.GetValue(CodeGroupType.ServiceStatus, Status.ToString()) ?? "?";

        #endregion

        #region behavior
        /// <summary>
        /// Update the ServericeOrder's Status based on the status from an updated WorkOrder
        /// </summary>
        /// <param name="workOrder">The workorder that will trigger the ServericeOrder's status change</param>
        /// <param name="currentUserRole">The user requesting the status change</param>
        public void UpdateStatus(UserRole currentUserRole, WorkOrder workOrder = null)
        {
            if (currentUserRole == null)
                throw new ArgumentNullException(nameof(currentUserRole));

            #region check transition NotReady => Ready
            if (Status == ServiceStatusType.NotReady)
            {
                if ((SpecificationId > 0 || ServiceSpecification != null) &&
                    (WorkOrders.Any()))
                {
                    ChangeStatusTo(ServiceStatusType.Ready, currentUserRole);
                }
            }
            #endregion

            #region check transition Ready => Busy
            if (Status == ServiceStatusType.Ready)
            {
                if (WorkOrders.Any(wo => wo.Status == WorkStatusType.VisitNeeded ||
                                         wo.Status == WorkStatusType.VisitOpen))
                {
                    ChangeStatusTo(ServiceStatusType.Busy, currentUserRole);
                }
            }
            #endregion

            #region check transition Busy => Closed
            if (Status == ServiceStatusType.Busy)
            {
                if (WorkOrders.All(wo => wo.Status == WorkStatusType.Closed))
                {
                    ChangeStatusTo(ServiceStatusType.Busy, currentUserRole);
                }
            }
            #endregion

            #region check transition Busy => Aborted
            if (Status == ServiceStatusType.Aborted)
            {
                if (WorkOrders.All(wo => wo.Status == WorkStatusType.Aborted))
                {
                    ChangeStatusTo(ServiceStatusType.Aborted, currentUserRole);
                }
            }
            #endregion
        }

        /// <summary>
        /// Change the ServericeOrder's status, and log an history record for the status change
        /// </summary>
        /// <param name="status">The new status for the ServericeOrder</param>
        /// <param name="currentUserRole">The user requesting the status change</param>
        public void ChangeStatusTo(ServiceStatusType status, UserRole currentUserRole)
        {
            if (currentUserRole == null) return;
            var fromStatus = Status;
            Status = status;
            var serviceOrderHistory = ServiceOrderHistory.Create(this, fromStatus, currentUserRole);
            ServiceOrderHistories.Add(serviceOrderHistory);
        }
        
        public static ServiceOrder Create(ServiceSpecification specification, User user)
        {
            if (specification == null) return null;

            var serviceOrder = new ServiceOrder
            {
                SpecificationId = specification.Id,
                ServiceSpecification = specification,
                OrderDate = DateTime.Now,
                StartNoEarlierThan = DateTime.Now,
                Status = ServiceStatusType.NotReady
            };
            if (user != null)
                serviceOrder.SetAuditInfo(user.Login);

            return serviceOrder;
        }

        public void RefreshState()
        {
            OnPropertyChanged(() => ExternalRef,
                              () => DisplayStatus);
            OnPropertyChanged(() => OrderDate,
                              () => StartNoEarlierThan);
            OnPropertyChanged(() => WorkOrders);
            OnPropertyChanged(() => Remarks);
        }

        #endregion
    }
}
