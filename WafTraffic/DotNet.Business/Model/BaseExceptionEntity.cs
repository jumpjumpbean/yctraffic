namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseExceptionEntity
    {
        private string createBy;
        private string createOn;
        private string createUserId;
        [NonSerialized]
        public static string FieldAppDomainName = "AppDomainName";
        [NonSerialized]
        public static string FieldCategory = "Category";
        [NonSerialized]
        public static string FieldCreateBy = "CreateBy";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldEventID = "EventId";
        [NonSerialized]
        public static string FieldFormattedMessage = "FormattedMessage";
        [NonSerialized]
        public static string FieldId = "LogId";
        [NonSerialized]
        public static string FieldIPAddress = "IPAddress";
        [NonSerialized]
        public static string FieldMachineName = "MachineName";
        [NonSerialized]
        public static string FieldMessage = "Message";
        [NonSerialized]
        public static string FieldPriority = "Priority";
        [NonSerialized]
        public static string FieldProcessId = "ProcessId";
        [NonSerialized]
        public static string FieldProcessName = "ProcessName";
        [NonSerialized]
        public static string FieldSeverity = "Severity";
        [NonSerialized]
        public static string FieldThreadName = "ThreadName";
        [NonSerialized]
        public static string FieldTimestamp = "Timestamp";
        [NonSerialized]
        public static string FieldTitle = "Title";
        [NonSerialized]
        public static string FieldWin32ThreadId = "Win32ThreadId";
        private string formattedMessage;
        private string id;
        private string ipAddress;
        private string message;
        [NonSerialized]
        public static string TableName = "Base_Exception";
        private string title;

        public BaseExceptionEntity()
        {
            this.id = string.Empty;
            this.title = string.Empty;
            this.message = string.Empty;
            this.formattedMessage = string.Empty;
            this.ipAddress = string.Empty;
            this.createUserId = string.Empty;
            this.createBy = string.Empty;
            this.createOn = string.Empty;
        }

        public BaseExceptionEntity(DataRow dataRow)
        {
            this.id = string.Empty;
            this.title = string.Empty;
            this.message = string.Empty;
            this.formattedMessage = string.Empty;
            this.ipAddress = string.Empty;
            this.createUserId = string.Empty;
            this.createBy = string.Empty;
            this.createOn = string.Empty;
            this.GetFrom(dataRow);
        }

        public BaseExceptionEntity(DataTable dataTable)
        {
            this.id = string.Empty;
            this.title = string.Empty;
            this.message = string.Empty;
            this.formattedMessage = string.Empty;
            this.ipAddress = string.Empty;
            this.createUserId = string.Empty;
            this.createBy = string.Empty;
            this.createOn = string.Empty;
            this.GetFrom(dataTable);
        }

        public void ClearProperty()
        {
            this.Id = string.Empty;
            this.CreateBy = string.Empty;
        }

        public BaseExceptionEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.Message = BaseBusinessLogic.ConvertToString(dataRow[FieldMessage]);
            this.FormattedMessage = BaseBusinessLogic.ConvertToString(dataRow[FieldFormattedMessage]);
            this.IPAddress = BaseBusinessLogic.ConvertToString(dataRow[FieldIPAddress]);
            this.CreateOn = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            return this;
        }

        public BaseExceptionEntity GetFrom(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                this.GetFrom(row);
                break;
            }
            return this;
        }

        public string CreateBy
        {
            get
            {
                return this.createBy;
            }
            set
            {
                this.createBy = value;
            }
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

        public string CreateUserId
        {
            get
            {
                return this.createUserId;
            }
            set
            {
                this.createUserId = value;
            }
        }

        public string FormattedMessage
        {
            get
            {
                return this.formattedMessage;
            }
            set
            {
                this.formattedMessage = value;
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

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }
    }
}

