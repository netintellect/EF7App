using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Data;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;


namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]

    public class DseSla : ValidatedEntity<DseSla>
    {
        #region state
        private int _id;
        [JsonProperty]
        [KeyNew(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }

        private DateTimeOffset _start;
        [JsonProperty]
        public DateTimeOffset Start
        {
            get { return _start; }
            set { SetProperty(value, ref _start, () => Start); }
        }

        private DateTimeOffset _end;
        [JsonProperty] 
        public DateTimeOffset End
        {
            get { return _end; }
            set { SetProperty(value, ref _end, () => End); }
        }


        private int _visitId;
        [JsonProperty]
        public int VisitId
        {
            get { return _visitId; }
            set { SetProperty(value, ref _visitId, () => VisitId); }
        }

        private Visit _visit;
        [JsonProperty, Aggregation]
        public Visit Visit
        {
            get { return _visit; }
            set { _visit = value; OnPropertyChanged(() => Visit); }
        }

        private int _dseSpecificationId;
        [JsonProperty]
        public int DseSpecificationId
        {
            get { return _dseSpecificationId; }
            set { SetProperty(value, ref _dseSpecificationId, () => DseSpecificationId); }
        }

        private DseSpecificationBase _dseSpecification;
        [JsonProperty, Aggregation]
        public DseSpecificationBase DseSpecification
        {
            get { return _dseSpecification; }
            set { _dseSpecification = value; OnPropertyChanged(() => DseSpecification); }
        }

        #endregion
    }
}
