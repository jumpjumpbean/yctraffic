namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseContactEntity
    {
        private DateTime? commentDate;
        private string commentUserId;
        private string commentUserRealName;
        private string content;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string description;
        private int? enabled;
        [NonSerialized]
        public static string FieldCommentDate = "CommentDate";
        [NonSerialized]
        public static string FieldCommentUserId = "CommentUserId";
        [NonSerialized]
        public static string FieldCommentUserRealName = "CommentUserRealName";
        [NonSerialized]
        public static string FieldContent = "Content";
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
        public static string FieldIsOpen = "IsOpen";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldParentId = "ParentId";
        [NonSerialized]
        public static string FieldPriority = "Priority";
        [NonSerialized]
        public static string FieldReadCount = "ReadCount";
        [NonSerialized]
        public static string FieldSendCount = "SendCount";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldTitle = "Title";
        private string id;
        private int? isOpen;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string parentId;
        private string priority;
        private int? readCount;
        private int? sendCount;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_Contact";
        private string title;

        public BaseContactEntity()
        {
            this.sendCount = 0;
            this.readCount = 0;
            this.isOpen = 0;
            this.commentDate = null;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseContactEntity(DataRow dataRow)
        {
            this.sendCount = 0;
            this.readCount = 0;
            this.isOpen = 0;
            this.commentDate = null;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseContactEntity(DataTable dataTable)
        {
            this.sendCount = 0;
            this.readCount = 0;
            this.isOpen = 0;
            this.commentDate = null;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseContactEntity(IDataReader dataReader)
        {
            this.sendCount = 0;
            this.readCount = 0;
            this.isOpen = 0;
            this.commentDate = null;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseContactEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToString(dataRow[FieldParentId]);
            this.Title = BaseBusinessLogic.ConvertToString(dataRow[FieldTitle]);
            this.Content = BaseBusinessLogic.ConvertToString(dataRow[FieldContent]);
            this.Priority = BaseBusinessLogic.ConvertToString(dataRow[FieldPriority]);
            this.SendCount = BaseBusinessLogic.ConvertToInt(dataRow[FieldSendCount]);
            this.ReadCount = BaseBusinessLogic.ConvertToInt(dataRow[FieldReadCount]);
            this.IsOpen = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsOpen]);
            this.CommentUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCommentUserId]);
            this.CommentUserRealName = BaseBusinessLogic.ConvertToString(dataRow[FieldCommentUserRealName]);
            this.CommentDate = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCommentDate]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
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

        public BaseContactEntity GetFrom(DataTable dataTable)
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

        public BaseContactEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataReader[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToString(dataReader[FieldParentId]);
            this.Title = BaseBusinessLogic.ConvertToString(dataReader[FieldTitle]);
            this.Content = BaseBusinessLogic.ConvertToString(dataReader[FieldContent]);
            this.Priority = BaseBusinessLogic.ConvertToString(dataReader[FieldPriority]);
            this.SendCount = BaseBusinessLogic.ConvertToInt(dataReader[FieldSendCount]);
            this.ReadCount = BaseBusinessLogic.ConvertToInt(dataReader[FieldReadCount]);
            this.IsOpen = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsOpen]);
            this.CommentUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldCommentUserId]);
            this.CommentUserRealName = BaseBusinessLogic.ConvertToString(dataReader[FieldCommentUserRealName]);
            this.CommentDate = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldCommentDate]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
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

        public DateTime? CommentDate
        {
            get
            {
                return this.commentDate;
            }
            set
            {
                this.commentDate = value;
            }
        }

        public string CommentUserId
        {
            get
            {
                return this.commentUserId;
            }
            set
            {
                this.commentUserId = value;
            }
        }

        public string CommentUserRealName
        {
            get
            {
                return this.commentUserRealName;
            }
            set
            {
                this.commentUserRealName = value;
            }
        }

        public string Content
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

        public int? IsOpen
        {
            get
            {
                return this.isOpen;
            }
            set
            {
                this.isOpen = value;
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

        public string Priority
        {
            get
            {
                return this.priority;
            }
            set
            {
                this.priority = value;
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

        public int? SendCount
        {
            get
            {
                return this.sendCount;
            }
            set
            {
                this.sendCount = value;
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

