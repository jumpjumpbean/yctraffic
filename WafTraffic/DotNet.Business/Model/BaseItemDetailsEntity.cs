namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseItemDetailsEntity
    {
        private int? allowDelete;
        private int? allowEdit;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string description;
        private int? enabled;
        [NonSerialized]
        public static string FieldAllowDelete = "AllowDelete";
        [NonSerialized]
        public static string FieldAllowEdit = "AllowEdit";
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
        public static string FieldIsPublic = "IsPublic";
        [NonSerialized]
        public static string FieldItemCode = "ItemCode";
        [NonSerialized]
        public static string FieldItemName = "ItemName";
        [NonSerialized]
        public static string FieldItemValue = "ItemValue";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldParentId = "ParentId";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        private int? id;
        private int? isPublic;
        private string itemCode;
        private string itemName;
        private string itemValue;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private int? parentId;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_Items";

        public BaseItemDetailsEntity()
        {
            this.id = 0;
            this.parentId = 0;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.isPublic = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseItemDetailsEntity(DataRow dataRow)
        {
            this.id = 0;
            this.parentId = 0;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.isPublic = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseItemDetailsEntity(DataTable dataTable)
        {
            this.id = 0;
            this.parentId = 0;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.isPublic = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseItemDetailsEntity(IDataReader dataReader)
        {
            this.id = 0;
            this.parentId = 0;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.isPublic = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseItemDetailsEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToInt(dataRow[FieldParentId]);
            this.ItemCode = BaseBusinessLogic.ConvertToString(dataRow[FieldItemCode]);
            this.ItemName = BaseBusinessLogic.ConvertToString(dataRow[FieldItemName]);
            this.ItemValue = BaseBusinessLogic.ConvertToString(dataRow[FieldItemValue]);
            this.AllowEdit = BaseBusinessLogic.ConvertToInt(dataRow[FieldAllowEdit]);
            this.AllowDelete = BaseBusinessLogic.ConvertToInt(dataRow[FieldAllowDelete]);
            this.IsPublic = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsPublic]);
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

        public BaseItemDetailsEntity GetFrom(DataTable dataTable)
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

        public BaseItemDetailsEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToInt(dataReader[FieldParentId]);
            this.ItemCode = BaseBusinessLogic.ConvertToString(dataReader[FieldItemCode]);
            this.ItemName = BaseBusinessLogic.ConvertToString(dataReader[FieldItemName]);
            this.ItemValue = BaseBusinessLogic.ConvertToString(dataReader[FieldItemValue]);
            this.AllowEdit = BaseBusinessLogic.ConvertToInt(dataReader[FieldAllowEdit]);
            this.AllowDelete = BaseBusinessLogic.ConvertToInt(dataReader[FieldAllowDelete]);
            this.IsPublic = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsPublic]);
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

        public int? AllowDelete
        {
            get
            {
                return this.allowDelete;
            }
            set
            {
                this.allowDelete = value;
            }
        }

        public int? AllowEdit
        {
            get
            {
                return this.allowEdit;
            }
            set
            {
                this.allowEdit = value;
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

        public int? Id
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

        public int? IsPublic
        {
            get
            {
                return this.isPublic;
            }
            set
            {
                this.isPublic = value;
            }
        }

        public string ItemCode
        {
            get
            {
                return this.itemCode;
            }
            set
            {
                this.itemCode = value;
            }
        }

        public string ItemName
        {
            get
            {
                return this.itemName;
            }
            set
            {
                this.itemName = value;
            }
        }

        public string ItemValue
        {
            get
            {
                return this.itemValue;
            }
            set
            {
                this.itemValue = value;
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

        public int? ParentId
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

