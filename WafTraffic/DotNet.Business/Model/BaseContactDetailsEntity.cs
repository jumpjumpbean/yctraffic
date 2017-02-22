namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseContactDetailsEntity
    {
        private string category;
        private string contactId;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string description;
        private int? enabled;
        [NonSerialized]
        public static string FieldCategory = "Category";
        [NonSerialized]
        public static string FieldContactId = "ContactId";
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
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldIsNew = "IsNew";
        [NonSerialized]
        public static string FieldLastViewDate = "LastViewDate";
        [NonSerialized]
        public static string FieldLastViewIP = "LastViewIP";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldNewComment = "NewComment";
        [NonSerialized]
        public static string FieldReceiverId = "ReceiverId";
        [NonSerialized]
        public static string FieldReceiverRealName = "ReceiverRealName";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        private string id;
        private int? isNew;
        private string lastViewDate;
        private string lastViewIP;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private int? newComment;
        private string receiverId;
        private string receiverRealName;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_ContactDetails";

        public BaseContactDetailsEntity()
        {
            this.isNew = 0;
            this.newComment = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseContactDetailsEntity(DataRow dataRow)
        {
            this.isNew = 0;
            this.newComment = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseContactDetailsEntity(DataTable dataTable)
        {
            this.isNew = 0;
            this.newComment = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseContactDetailsEntity(IDataReader dataReader)
        {
            this.isNew = 0;
            this.newComment = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseContactDetailsEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.ContactId = BaseBusinessLogic.ConvertToString(dataRow[FieldContactId]);
            this.Category = BaseBusinessLogic.ConvertToString(dataRow[FieldCategory]);
            this.ReceiverId = BaseBusinessLogic.ConvertToString(dataRow[FieldReceiverId]);
            this.ReceiverRealName = BaseBusinessLogic.ConvertToString(dataRow[FieldReceiverRealName]);
            this.IsNew = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsNew]);
            this.NewComment = BaseBusinessLogic.ConvertToInt(dataRow[FieldNewComment]);
            this.LastViewIP = BaseBusinessLogic.ConvertToString(dataRow[FieldLastViewIP]);
            this.LastViewDate = BaseBusinessLogic.ConvertToString(dataRow[FieldLastViewDate]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldSortCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            return this;
        }

        public BaseContactDetailsEntity GetFrom(DataTable dataTable)
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

        public BaseContactDetailsEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataReader[FieldId]);
            this.ContactId = BaseBusinessLogic.ConvertToString(dataReader[FieldContactId]);
            this.Category = BaseBusinessLogic.ConvertToString(dataReader[FieldCategory]);
            this.ReceiverId = BaseBusinessLogic.ConvertToString(dataReader[FieldReceiverId]);
            this.ReceiverRealName = BaseBusinessLogic.ConvertToString(dataReader[FieldReceiverRealName]);
            this.IsNew = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsNew]);
            this.NewComment = BaseBusinessLogic.ConvertToInt(dataReader[FieldNewComment]);
            this.LastViewIP = BaseBusinessLogic.ConvertToString(dataReader[FieldLastViewIP]);
            this.LastViewDate = BaseBusinessLogic.ConvertToString(dataReader[FieldLastViewDate]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldSortCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedBy]);
            return this;
        }

        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        public string ContactId
        {
            get
            {
                return this.contactId;
            }
            set
            {
                this.contactId = value;
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

        public string LastViewDate
        {
            get
            {
                return this.lastViewDate;
            }
            set
            {
                this.lastViewDate = value;
            }
        }

        public string LastViewIP
        {
            get
            {
                return this.lastViewIP;
            }
            set
            {
                this.lastViewIP = value;
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

        public int? NewComment
        {
            get
            {
                return this.newComment;
            }
            set
            {
                this.newComment = value;
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
    }
}

