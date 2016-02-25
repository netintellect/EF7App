using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Extensions;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    [HasSelfValidation]
    public class BookingRemark : WorkOrderRemark
    {
        #region state
        private int? _resultCodeId;
        [RangeValidator(1, RangeBoundaryType.Inclusive, 1, RangeBoundaryType.Ignore,
                        MessageTemplateResourceType = typeof(Translations),
                        MessageTemplateResourceName = "CallAgentRemarkResultCodeRequired")]
        [Display(ResourceType = typeof(Translations), Name = "BookingRemarkResultCodeId")]
        [JsonProperty]
        public int? ResultCodeId
        {
            get { return _resultCodeId; }
            set { SetProperty(value, ref _resultCodeId, () => ResultCodeId); }
        }

        private ResultCode _resultCode;
        [JsonProperty, Aggregation]
        public ResultCode ResultCode
        {
            get { return _resultCode; }
            set
            {
                _resultCode = value;
                OnPropertyChanged(() => ResultCode);
            }
        }
        
        public bool IsCurrentCall { get; set; }
        #endregion

        #region behavior
        public static BookingRemark Create(WorkOrder workOrder, UserRole currentUserRole)
        {
            if (workOrder == null) return null;
            if (currentUserRole == null) return null;

            var bookingRemark = new BookingRemark
            {
                Type = RemarkType.AppointmentBooking,
                EnteredDate = DateTime.Now,
                UserRoleId = currentUserRole.Id,
                UserRole = currentUserRole
            };
            var user = bookingRemark.GetCurrentUser(RepositoryFinder) ?? currentUserRole.User;
            if (user == null) return bookingRemark;

            bookingRemark.UserId = user.Id;
            bookingRemark.User = user;

            return bookingRemark;
        }

        public string DisplayAuditWhen
        {
            get
            {
                string message = Translations.CallAgentRemarkDisplayAudit1;
                return string.Format(message, EnteredDate != DateTimeOffset.MinValue ? EnteredDate.ToString("t") : "",
                                              EnteredDate != DateTimeOffset.MinValue ? EnteredDate.ToString("d") : "");
            }
        }

        public string DisplayAuditWho
        {
            get
            {
                string message = Translations.CallAgentRemarkDisplayAudit2;
                return string.Format(message, UserRole != null ? UserRole.DisplayRole : "",
                                              User != null ? User.DisplayName : "");
            }
        }
        
        #endregion
    }
}
