namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseLogEntity
    {
        private string createOn = string.Empty;
        private string description = string.Empty;
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldDescription = "Description";
        [NonSerialized]
        public static string FieldId = "LogId";
        [NonSerialized]
        public static string FieldIPAddress = "IPAddress";
        [NonSerialized]
        public static string FieldMethodId = "MethodId";
        [NonSerialized]
        public static string FieldMethodName = "MethodName";
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldParameters = "Parameters";
        [NonSerialized]
        public static string FieldProcessId = "ProcessId";
        [NonSerialized]
        public static string FieldProcessName = "ProcessName";
        [NonSerialized]
        public static string FieldUrlReferrer = "UrlReferrer";
        [NonSerialized]
        public static string FieldUserId = "UserId";
        [NonSerialized]
        public static string FieldUserRealName = "UserRealName";
        [NonSerialized]
        public static string FieldWebUrl = "WebUrl";
        private string id = string.Empty;
        private string ipAddress = string.Empty;
        private string methodId = string.Empty;
        private string methodName = string.Empty;
        private string parameters = string.Empty;
        private string processId = string.Empty;
        private string processName = string.Empty;
        [NonSerialized]
        public static string TableName = "Base_Log";
        private string urlReferrer = string.Empty;
        private string userId = string.Empty;
        private string userRealName = string.Empty;
        private string webUrl = string.Empty;

        public BaseLogEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.ProcessId = BaseBusinessLogic.ConvertToString(dataRow[FieldProcessId]);
            this.ProcessName = BaseBusinessLogic.ConvertToString(dataRow[FieldProcessName]);
            this.MethodName = BaseBusinessLogic.ConvertToString(dataRow[FieldMethodName]);
            this.Parameters = BaseBusinessLogic.ConvertToString(dataRow[FieldParameters]);
            this.IPAddress = BaseBusinessLogic.ConvertToString(dataRow[FieldIPAddress]);
            this.UserId = BaseBusinessLogic.ConvertToString(dataRow[FieldUserId]);
            this.UrlReferrer = BaseBusinessLogic.ConvertToString(dataRow[FieldUrlReferrer]);
            this.WebUrl = BaseBusinessLogic.ConvertToString(dataRow[FieldWebUrl]);
            this.UserRealName = BaseBusinessLogic.ConvertToString(dataRow[FieldUserRealName]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateOn]);
            return this;
        }

        public string CreateOn
        {
            get
            {
                return this.createOn;
            }
            set
            {
                this.createOn = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string IPAddress
        {
            get
            {
                return this.ipAddress;
            }
            set
            {
                this.ipAddress = value;
            }
        }

        public string MethodId
        {
            get
            {
                return this.methodId;
            }
            set
            {
                this.methodId = value;
            }
        }

        public string MethodName
        {
            get
            {
                return this.methodName;
            }
            set
            {
                this.methodName = value;
            }
        }

        public string Parameters
        {
            get
            {
                return this.parameters;
            }
            set
            {
                this.parameters = value;
            }
        }

        public string ProcessId
        {
            get
            {
                return this.processId;
            }
            set
            {
                this.processId = value;
            }
        }

        public string ProcessName
        {
            get
            {
                return this.processName;
            }
            set
            {
                this.processName = value;
            }
        }

        public string UrlReferrer
        {
            get
            {
                return this.urlReferrer;
            }
            set
            {
                this.urlReferrer = value;
            }
        }

        public string UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
            }
        }

        public string UserRealName
        {
            get
            {
                return this.userRealName;
            }
            set
            {
                this.userRealName = value;
            }
        }

        public string WebUrl
        {
            get
            {
                return this.webUrl;
            }
            set
            {
                this.webUrl = value;
            }
        }
    }
}

