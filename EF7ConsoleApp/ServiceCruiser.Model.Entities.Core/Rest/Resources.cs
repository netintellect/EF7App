using System.Net;

namespace ServiceCruiser.Model.Entities.Core.Rest
{
    public static class Routings
    {
        public const string Filter = "/filter";

        public static class Applications
        {
            public const string Root = "api/applications";

            public static class AppInformation
            {
                public const string Self = "AppInfo";
            }
        }

        public static class Contracts
        {
            public const string Root = "api/contracts";
            
            public static class ContractModel
            {
                public const string Us = "ContractModels";
                public const string Self = "ContractModel";
                public const string Filter = Us + Routings.Filter;

                public const string GetContractingCompanies = "contractmodels";
            }

            public static class Company
            {
                public const string Us = "Companies";
                public const string Self = "Company";
                public const string Filter = Us + Routings.Filter;
            }
            
            public static class ContractorCompany
            {
                public const string Us = "ContractorCompanies";
                public const string Self = "ContractorCompany";
                public const string Filter = Us + Routings.Filter;
            }

            public static class ServiceSpecification
            {
                public const string Us = "ServiceSpecifications";
                public const string Self = "ServiceSpecification";
                public const string Filter = Us + Routings.Filter;
            }
            
            public static class WorkSpecification
            {
                public const string Us = "WorkSpecifications";
                public const string Self = "WorkSpecification";
                public const string Filter = Us + Routings.Filter;
            }

            public static class AttributeGroup
            {
                public const string Us = "AttributeGroups";
                public const string Self = "AttributeGroup";
                public const string Filter = Us + Routings.Filter;                 
            }

            public static class AppointmentWindow
            {
                public const string Us = "AppointmentWindows";
                public const string Self = "AppointmentWindow";
                public const string Filter = Us + Routings.Filter;                 
            }

            public static class Item
            {
                public const string Us = "Items";
                public const string Self = "Item";
                public const string Filter = Us + Routings.Filter;
            }

            public static class ZipCode
            {
                public const string Us = "ZipCodes";
                public const string Self = "ZipCode";
                public const string Filter = Us + Routings.Filter;
            }
        }
        
        public static class Capacities
        {
            public const string Root = "api/capacities";

            public static class CapacityCycleConfiguration
            {
                public const string Us = "CapacityCycleConfigurations";
                public const string Self = "CapacityCycleConfiguration";
                public const string Filter = Us + Routings.Filter;
            }

            public static class CapacityCycle
            {
                public const string Us = "CapacityCycles";
                public const string Self = "CapacityCycle";
                public const string Filter = Us + Routings.Filter;
            }

            public static class CapacityRequest
            {
                public const string Us = "CapacityRequests";
                public const string Self = "CapacityRequest";
                public const string Filter = Us + Routings.Filter;
            }

            public static class Assignment
            {
                public const string Us = "Assignments";
                public const string Self = "Assignment";
                public const string Filter = Us + Routings.Filter;
            }
        }

        public static class Appointments
        {
            public const string Root = "api/appointments";

            public static class WorkOrder
            {
                public const string Us = "WorkOrders";
                public const string Self = "WorkOrder";
                public const string Filter = Us + Routings.Filter;
            }
        }

        public static class FieldServices
        {
            public const string Root = "api/fieldservices";

            public static class ServiceOrder
            {
                public const string Us = "ServiceOrders";
                public const string Self = "ServiceOrder";
                public const string Filter = Us + Routings.Filter;
            }
            public static class WorkOrder
            {
                public const string Us = "WorkOrders";
                public const string Self = "WorkOrder";
                public const string Filter = Us + Routings.Filter;
            }

            public static class 
                Visit
            {
                public const string Us = "Visits";
                public const string Self = "Visit";
                public const string Filter = Us + Routings.Filter;
            }

            public static class Task
            {
                public const string Us = "Tasks";
                public const string Self = "Task";
                public const string Filter = Us + Routings.Filter;
            }
            public static class Remark
            {
                public const string Us = "Remarks";
                public const string Self = "Remark";
                public const string Filter = Us + Routings.Filter;
            }

            public static class ServiceOrderAttachment
            {
                public const string Us = "ServiceOrderAttachments";
                public const string Self = "ServiceOrderAttachment";
                public const string Filter = Us + Routings.Filter;
            }

            public static class WorkOrderAttachment
            {
                public const string Us = "WorkOrderAttachments";
                public const string Self = "WorkOrderAttachment";
                public const string Filter = Us + Routings.Filter;
            }

            public static class ResultCategory
            {
                public const string Us = "ResultCategories";
                public const string Filter = Us + Routings.Filter;
            }

            public static class VisitByExlusiveTaskSummary
            {
                public const string Us = "VisitByExlusiveTaskSummary";
                public const string Filter = Us + Routings.Filter;
            }
        }

        public static class Users
        {
            public const string Root = "api/users";

            public static class Unavailability
            {
                public const string Us = "Unavailabilities";
                public const string Self = "Unavailability";
                public const string Filter = Us + Routings.Filter;
            }

            public static class User
            {
                public const string Us = "Users";
                public const string Self = "User";
                public const string Filter = Us + Routings.Filter;
            }

            public static class Licence
            {
                public const string Us = "Licenses";
                public const string Self = "License";
                public const string Filter = Us + Routings.Filter;
            }

            public static class Product
            {
                public const string Us = "Products";
                public const string Self = "Product";
                public const string Filter = Us + Routings.Filter;
            }

            public static class Module
            {
                public const string Us = "Modules";
                public const string Self = "Module";
                public const string Filter = Us + Routings.Filter;
            }
        }
        public static class Technicians
        {
            public const string Root = "api/technicians";

            public static class Technician
            {
                public const string Us = "Technicians";
                public const string Self = "Technician";
                public const string Filter = Us + Routings.Filter;
            }
            public static class WorkDay
            {
                public const string Us = "WorkDays";
                public const string Self = "WorkDay";
                public const string Filter = Us + Routings.Filter;
                public const string Assign = Us + "/Assign";
            }
        }

        public static class Scheduling
        {
            public const string Root = "api/scheduling";

            public const string Self = "ThreeSixty";
            public const string Filter = Self + Routings.Filter;
            public const string Login = Self + "/Login";
            public const string Logout = Self + "/Logout";

        }

        public static class Logs
        {
            public const string Root = "api/logs";

            public static class ErrorLog
            {
                public const string GetErrorLogs = "error";
            }
        }

        public static class Demo
        {
            public const string Root = "api/demo";

            public static class Database
            {
                public const string Reset = "ResetDatabase";
            }
            
            public static class Dse
            {
                public const string SetCallBackUrl = "SetCallBackUrl";
                public const string InitializePlan = "InitializePlan";
                public const string RetreivePlan = "RetreivePlan";
            }
        }
    }

    public static class Resources
    {
        public static readonly string ParamOne = "|parameter|";
        public static readonly string ParamTwo = "|parameter2|";

        public static class Applications
        {
            public static class AppInfo
            {
                public static readonly string Self =
                    $"{Routings.Applications.Root}/{Routings.Applications.AppInformation.Self}";
            }
        }

        public static class Contracts
        {
            public static class ContractModel
            {
                public static readonly string Self =
                    $"{Routings.Contracts.Root}/{Routings.Contracts.ContractModel.Self}";

                public static readonly string Filter =
                    $"{Routings.Contracts.Root}/{Routings.Contracts.ContractModel.Filter}";
            }
            public static class Company
            {
                public static readonly string Self = $"{Routings.Contracts.Root}/{Routings.Contracts.Company.Self}";

                public static readonly string Filter = $"{Routings.Contracts.Root}/{Routings.Contracts.Company.Filter}";
            }

            public static class ContractorCompany
            {
                public static readonly string Self =
                    $"{Routings.Contracts.Root}/{Routings.Contracts.ContractorCompany.Self}";

                public static readonly string Filter =
                    $"{Routings.Contracts.Root}/{Routings.Contracts.ContractorCompany.Filter}";
            }

            public static class ServiceSpecification
            {
                public static readonly string Self =
                    $"{Routings.Contracts.Root}/{Routings.Contracts.ServiceSpecification.Self}";
                public static readonly string Filter =
                    $"{Routings.Contracts.Root}/{Routings.Contracts.ServiceSpecification.Filter}";
            }

            public static class WorkSpecification
            {
                public static readonly string Self =
                    $"{Routings.Contracts.Root}/{Routings.Contracts.WorkSpecification.Self}";
                public static readonly string Filter =
                    $"{Routings.Contracts.Root}/{Routings.Contracts.WorkSpecification.Filter}";
            }

            public static class AttributeGroup
            {
                public static readonly string Self =
                    $"{Routings.Contracts.Root}/{Routings.Contracts.AttributeGroup.Self}";
                public static readonly string Filter =
                    $"{Routings.Contracts.Root}/{Routings.Contracts.AttributeGroup.Filter}";
            }

            public static class Item
            {
                public static readonly string Self = $"{Routings.Contracts.Root}/{Routings.Contracts.Item.Self}";
                public static readonly string Filter = $"{Routings.Contracts.Root}/{Routings.Contracts.Item.Filter}";
            }

            public static class ZipCode
            {
                public static readonly string Self = $"{Routings.Contracts.Root}/{Routings.Contracts.ZipCode.Self}";
                public static readonly string Filter = $"{Routings.Contracts.Root}/{Routings.Contracts.ZipCode.Filter}";
            }
        }

        public static class Capacities
        {
            public static class CapacityCycleConfiguration
            {
                public static readonly string Self =
                    $"{Routings.Capacities.Root}/{Routings.Capacities.CapacityCycleConfiguration.Self}";

                public static readonly string Filter =
                    $"{Routings.Capacities.Root}/{Routings.Capacities.CapacityCycleConfiguration.Filter}";
            }

            public static class CapacityCycle
            {
                public static readonly string Self =
                    $"{Routings.Capacities.Root}/{Routings.Capacities.CapacityCycle.Self}";

                public static readonly string Filter =
                    $"{Routings.Capacities.Root}/{Routings.Capacities.CapacityCycle.Filter}";
            }

            public static class CapacityRequest
            {
                public static readonly string Self =
                    $"{Routings.Capacities.Root}/{Routings.Capacities.CapacityRequest.Self}";

                public static readonly string Filter =
                    $"{Routings.Capacities.Root}/{Routings.Capacities.CapacityRequest.Filter}";
            }

            public static class Assignment
            {
                public static readonly string Self = $"{Routings.Capacities.Root}/{Routings.Capacities.Assignment.Self}";

                public static readonly string Filter =
                    $"{Routings.Capacities.Root}/{Routings.Capacities.Assignment.Filter}";
            }
        }

        public static class Appointments
        {
            public static class WorkOrder
            {
                public static readonly string Self =
                    $"{Routings.Appointments.Root}/{Routings.Appointments.WorkOrder.Self}";

                public static readonly string Filter =
                    $"{Routings.Appointments.Root}/{Routings.Appointments.WorkOrder.Filter}";
            }
        }

        public static class FieldServices
        {
            public static class ServiceOrder
            {
                public static readonly string Self =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.ServiceOrder.Self}";

                public static readonly string Filter =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.ServiceOrder.Filter}";
            }

            public static class WorkOrder
            {
                public static readonly string Self =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.WorkOrder.Self}";
                public static readonly string Filter =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.WorkOrder.Filter}";
            }

            public static class Visit
            {
                public static readonly string Self =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.Visit.Self}";
                public static readonly string Filter =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.Visit.Filter}";
            }

            public static class Remark
            {
                public static readonly string Self =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.Remark.Self}";
                 
            }

            public static class ServiceOrderAttachment
            {
                public static readonly string Self =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.ServiceOrderAttachment.Self}";

            }

            public static class WorkOrderAttachment
            {
                public static readonly string Self =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.WorkOrderAttachment.Self}";

            }
            
            public static class Task
            {
                public static readonly string Self = $"{Routings.FieldServices.Root}/{Routings.FieldServices.Task.Self}";                 
            }

            public static class ResultCategory
            {
                public static readonly string Filter =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.ResultCategory.Filter}";
            }

            public static class VisitByExlusiveTaskSummary
            {
                public static readonly string Filter =
                    $"{Routings.FieldServices.Root}/{Routings.FieldServices.VisitByExlusiveTaskSummary.Filter}";
            }
        }

        public static class Users
        {
            public static class Unavailability
            {
                public static readonly string Self = $"{Routings.Users.Root}/{Routings.Users.Unavailability.Self}";

                public static readonly string Filter = $"{Routings.Users.Root}/{Routings.Users.Unavailability.Filter}";
            }

            public static class User
            {
                public static readonly string Self = $"{Routings.Users.Root}/{Routings.Users.User.Self}";

                public static readonly string Filter = $"{Routings.Users.Root}/{Routings.Users.User.Filter}";
            }

            public static class License
            {
                public static readonly string Self = $"{Routings.Users.Root}/{Routings.Users.Licence.Self}";

                public static readonly string Filter = $"{Routings.Users.Root}/{Routings.Users.Licence.Filter}";
            }

            public static class Product
            {
                public static readonly string Self = $"{Routings.Users.Root}/{Routings.Users.Product.Self}";

                public static readonly string Filter = $"{Routings.Users.Root}/{Routings.Users.Product.Filter}";
            }

            public static class Module
            {
                public static readonly string Self = $"{Routings.Users.Root}/{Routings.Users.Module.Self}";

                public static readonly string Filter = $"{Routings.Users.Root}/{Routings.Users.Module.Filter}";
            }
        }

        public static class Technicians
        {
            public static class Technician
            {
                public static readonly string Self =
                    $"{Routings.Technicians.Root}/{Routings.Technicians.Technician.Self}";

                public static readonly string Filter =
                    $"{Routings.Technicians.Root}/{Routings.Technicians.Technician.Filter}";
            }

            public static class WorkDay
            {
                public static readonly string Self = $"{Routings.Technicians.Root}/{Routings.Technicians.WorkDay.Self}";

                public static readonly string Filter =
                    $"{Routings.Technicians.Root}/{Routings.Technicians.WorkDay.Filter}";
                
                public static readonly string Assign =
                    $"{Routings.Technicians.Root}/{Routings.Technicians.WorkDay.Assign}";
            }
        }
        
        public static class Logs
        {
            public static class ErrorLog
            {
                public static readonly string GetErrorLogs =
                    $"{Routings.Logs.Root}/{Routings.Logs.ErrorLog.GetErrorLogs}";
            }
        }

        public static class Scheduling
        {
            public static readonly  string Login = $"{Routings.Scheduling.Root}/{Routings.Scheduling.Login}";

            public static readonly string Logout = $"{Routings.Scheduling.Root}/{Routings.Scheduling.Logout}";
        }

        public static class Administration
        {
            public static readonly string ResetDatabase = $"{Routings.Demo.Root}/{Routings.Demo.Database.Reset}";

            public static readonly string DseSetCallBackUrl = $"{Routings.Demo.Root}/{Routings.Demo.Dse.SetCallBackUrl}";
            
            public static readonly string DseInitializePlan = $"{Routings.Demo.Root}/{Routings.Demo.Dse.InitializePlan}";

            public static readonly string DseRetrievePlan = $"{Routings.Demo.Root}/{Routings.Demo.Dse.RetreivePlan}";

        }
    }
}
