namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseMessageEntity
    {
        private string categoryCode;
        private string content;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string description;
        private int? enabled;
        [NonSerialized]
        public static string FieldCategoryCode = "CategoryCode";
        [NonSerialized]
        public static string FieldContents = "Contents";
        [NonSerialized]
        public static string FieldCreateBy = "CreateBy";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDeletionStateCode = "DeletionStateCode";
        [NonSerialized]
        public static string FieldDescription = "Description";
        [NonSerialized]
        public static string FieldEnabled = "Enabled";
        [NonSerialized]
        public static string FieldFunctionCode = "FunctionCode";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldIPAddress = "IPAddress";
        [NonSerialized]
        public static string FieldIsNew = "IsNew";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldObjectId = "ObjectId";
        [NonSerialized]
        public static string FieldParentId = "ParentId";
        [NonSerialized]
        public static string FieldReadCount = "ReadCount";
        [NonSerialized]
        public static string FieldReadDate = "ReadDate";
        [NonSerialized]
        public static string FieldReceiverId = "ReceiverId";
        [NonSerialized]
        public static string FieldReceiverRealName = "ReceiverRealName";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldTargetURL = "TargetURL";
        [NonSerialized]
        public static string FieldTitle = "Title";
        private string functionCode;
        private string id;
        private string iPAddress;
        private int? isNew;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string objectId;
        private string parentId;
        private int? readCount;
        private DateTime? readDate;
        private string receiverId;
        private string receiverRealName;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_Message";
        private string targetURL;
        private string title;

        public BaseMessageEntity()
        {
            this.isNew = 0;
            this.readCount = 0;
            this.readDate = null;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseMessageEntity(DataRow dataRow)
        {
            this.isNew = 0;
            this.readCount = 0;
            this.readDate = null;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseMessageEntity(DataTable dataTable)
        {
            this.isNew = 0;
            this.readCount = 0;
            this.readDate = null;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseMessageEntity(IDataReader dataReader)
        {
            this.isNew = 0;
            this.readCount = 0;
            this.readDate = null;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseMessageEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToString(dataRow[FieldParentId]);
            this.FunctionCode = BaseBusinessLogic.ConvertToString(dataRow[FieldFunctionCode]);
            this.CategoryCode = BaseBusinessLogic.ConvertToString(dataRow[FieldCategoryCode]);
            this.ObjectId = BaseBusinessLogic.ConvertToString(dataRow[FieldObjectId]);
            this.Title = BaseBusinessLogic.ConvertToString(dataRow[FieldTitle]);
            this.Contents = BaseBusinessLogic.ConvertToString(dataRow[FieldContents]);
            this.ReceiverId = BaseBusinessLogic.ConvertToString(dataRow[FieldReceiverId]);
            this.ReceiverRealName = BaseBusinessLogic.ConvertToString(dataRow[FieldReceiverRealName]);
            this.IsNew = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsNew]);
            this.ReadCount = BaseBusinessLogic.ConvertToInt(dataRow[FieldReadCount]);
            this.ReadDate = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldReadDate]);
            this.TargetURL = BaseBusinessLogic.ConvertToString(dataRow[FieldTargetURL]);
            this.IPAddress = BaseBusinessLogic.ConvertToString(dataRow[FieldIPAddress]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldSortCode]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            return this;
        }

        public BaseMessageEntity GetFrom(DataTable dataTable)
        {
            if ((dataTable == null) || (dataTable.Rows.Count == 0))
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                this.GetFrom(row);
                break;
            }
            return this;
        }

        public BaseMessageEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataReader[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToString(dataReader[FieldParentId]);
            this.FunctionCode = BaseBusinessLogic.ConvertToString(dataReader[FieldFunctionCode]);
            this.CategoryCode = BaseBusinessLogic.ConvertToString(dataReader[FieldCategoryCode]);
            this.ObjectId = BaseBusinessLogic.ConvertToString(dataReader[FieldObjectId]);
            this.Title = BaseBusinessLogic.ConvertToString(dataReader[FieldTitle]);
            this.Contents = BaseBusinessLogic.ConvertToString(dataReader[FieldContents]);
            this.ReceiverId = BaseBusinessLogic.ConvertToString(dataReader[FieldReceiverId]);
            this.ReceiverRealName = BaseBusinessLogic.ConvertToString(dataReader[FieldReceiverRealName]);
            this.IsNew = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsNew]);
            this.ReadCount = BaseBusinessLogic.ConvertToInt(dataReader[FieldReadCount]);
            this.ReadDate = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldReadDate]);
            this.TargetURL = BaseBusinessLogic.ConvertToString(dataReader[FieldTargetURL]);
            this.IPAddress = BaseBusinessLogic.ConvertToString(dataReader[FieldIPAddress]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[FieldDescription]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldSortCode]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedBy]);
            return this;
        }

        public string CategoryCode
        {
            get
            {
                return this.categoryCode;
            }
            set
            {
                this.categoryCode = value;
            }
        }

        public string Contents
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
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

        public DateTime? CreateOn
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

        public int? DeletionStateCode
        {
            get
            {
                return this.deletionStateCode;
            }
            set
            {
                this.deletionStateCode = value;
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

        public int? Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
            }
        }

        public string FunctionCode
        {
            get
            {
                return this.functionCode;
            }
            set
            {
                this.functionCode = value;
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
                return this.iPAddress;
            }
            set
            {
                this.iPAddress = value;
            }
        }

        public int? IsNew
        {
            get
            {
                return this.isNew;
            }
            set
            {
                this.isNew = value;
            }
        }

        public string ModifiedBy
        {
            get
            {
                return this.modifiedBy;
            }
            set
            {
                this.modifiedBy = value;
            }
        }

        public DateTime? ModifiedOn
        {
            get
            {
                return this.modifiedOn;
            }
            set
            {
                this.modifiedOn = value;
            }
        }

        public string ModifiedUserId
        {
            get
            {
                return this.modifiedUserId;
            }
            set
            {
                this.modifiedUserId = value;
            }
        }

        public string ObjectId
        {
            get
            {
                return this.objectId;
            }
            set
            {
                this.objectId = value;
            }
        }

        public string ParentId
        {
            get
            {
                return this.parentId;
            }
            set
            {
                this.parentId = value;
            }
        }

        public int? ReadCount
        {
            get
            {
                return this.readCount;
            }
            set
            {
                this.readCount = value;
            }
        }

        public DateTime? ReadDate
        {
            get
            {
                return this.readDate;
            }
            set
            {
                this.readDate = value;
            }
        }

        public string ReceiverId
        {
            get
            {
                return this.receiverId;
            }
            set
            {
                this.receiverId = value;
            }
        }

        public string ReceiverRealName
        {
            get
            {
                return this.receiverRealName;
            }
            set
            {
                this.receiverRealName = value;
            }
        }

        public int? SortCode
        {
            get
            {
                return this.sortCode;
            }
            set
            {
                this.sortCode = value;
            }
        }

        public string TargetURL
        {
            get
            {
                return this.targetURL;
            }
            set
            {
                this.targetURL = value;
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

