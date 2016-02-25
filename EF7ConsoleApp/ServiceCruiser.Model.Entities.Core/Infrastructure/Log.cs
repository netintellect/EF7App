using System;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.Core.Infrastructure
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Log : ValidatedEntity<Log>
    {
        #region state
        private int _id;
		[JsonProperty] [Key(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}

		private DateTimeOffset _logDate;
		[JsonProperty]
		public DateTimeOffset LogDate 
		{ 
 			get {   return _logDate;    } 
 			set {   SetProperty(value,ref _logDate, () => LogDate); } 
		}
    			

		private string _logApplication;
		[JsonProperty]
		public string LogApplication 
		{ 
 			get {   return _logApplication; } 
 			set {   SetProperty(value,ref _logApplication, () => LogApplication);   } 
		}

		private string _logLevel;
		[JsonProperty]
		public string LogLevel 
		{ 
 			get {   return _logLevel;   } 
 			set {   SetProperty(value,ref _logLevel, () => LogLevel);   } 
		}

		private string _logger;
		[JsonProperty]
		public string Logger 
		{ 
 			get {   return _logger; } 
 			set {   SetProperty(value,ref _logger, () => Logger);   } 
		}
    			

		private string _message;
		[JsonProperty]
		public string Message 
		{ 
 			get {   return _message;    } 
 			set {   SetProperty(value,ref _message, () => Message); } 
		}
    			

		private string _machineName;
		[JsonProperty]
		public string MachineName 
		{ 
 			get {   return _machineName;    } 
 			set {   SetProperty(value,ref _machineName, () => MachineName); } 
		}

		private string _userName;
		[JsonProperty]
		public string UserName 
		{ 
 			get {   return _userName;   } 
 			set {   SetProperty(value,ref _userName, () => UserName);   } 
		}

		private string _callSite;
		[JsonProperty]
		public string CallSite 
		{ 
 			get {   return _callSite;   } 
 			set {   SetProperty(value,ref _callSite, () => CallSite);   } 
		}
    			

		private string _thread;
		[JsonProperty]
		public string Thread 
		{ 
 			get {   return _thread; } 
 			set {   SetProperty(value,ref _thread, () => Thread);   } 
		}
    			
		private string _exception;
		[JsonProperty]
		public string Exception 
		{ 
 			get {   return _exception;  } 
 			set {   SetProperty(value,ref _exception, () => Exception); } 
		}
    			

		private string _stacktrace;
		[JsonProperty]
		public string Stacktrace 
		{ 
 			get {   return _stacktrace; } 
 			set {   SetProperty(value,ref _stacktrace, () => Stacktrace);   }
        }
        #endregion
    }
}
