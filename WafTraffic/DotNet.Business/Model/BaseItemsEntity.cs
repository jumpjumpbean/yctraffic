namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseItemsEntity
    {
        private int? allowDelete;
        private int? allowEdit;
        private string code;
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
        public static string FieldCode = "Code";
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
        public static string FieldFullName = "FullName";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldIsTree = "IsTree";
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
        [NonSerialized]
        public static string FieldTargetTable = "TargetTable";
        [NonSerialized]
        public static string FieldUseItemCode = "UseItemCode";
        [NonSerialized]
        public static string FieldUseItemName = "UseItemName";
        [NonSerialized]
        public static string FieldUseItemValue = "UseItemValue";
        private string fullName;
        private int? id;
        private int? isTree;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private int? parentId;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_Items";
        private string targetTable;
        private int? useItemCode;
        private int? useItemName;
        private int? useItemValue;

        public BaseItemsEntity()
        {
            this.id = null;
            this.parentId = null;
            this.isTree = 0;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseItemsEntity(DataRow dataRow)
        {
            this.id = null;
            this.parentId = null;
            this.isTree = 0;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseItemsEntity(DataTable dataTable)
        {
            this.id = null;
            this.parentId = null;
            this.isTree = 0;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseItemsEntity(IDataReader dataReader)
        {
            this.id = null;
            this.parentId = null;
            this.isTree = 0;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseItemsEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToInt(dataRow[FieldParentId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataRow[FieldCode]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataRow[FieldFullName]);
            this.TargetTable = BaseBusinessLogic.ConvertToString(dataRow[FieldTargetTable]);
            this.IsTree = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsTree]);
            this.UseItemCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldUseItemCode]);
            this.UseItemName = BaseBusinessLogic.ConvertToInt(dataRow[FieldUseItemName]);
            this.UseItemValue = BaseBusinessLogic.ConvertToInt(dataRow[FieldUseItemValue]);
            this.AllowEdit = BaseBusinessLogic.ConvertToInt(dataRow[FieldAllowEdit]);
            this.AllowDelete = BaseBusinessLogic.ConvertToInt(dataRow[FieldAllowDelete]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldSortCode]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            return this;
        }

        public BaseItemsEntity GetFrom(DataTable dataTable)
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

        public BaseItemsEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToInt(dataReader[FieldParentId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataReader[FieldCode]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataReader[FieldFullName]);
            this.TargetTable = BaseBusinessLogic.ConvertToString(dataReader[FieldTargetTable]);
            this.IsTree = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsTree]);
            this.UseItemCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldUseItemCode]);
            this.UseItemName = BaseBusinessLogic.ConvertToInt(dataReader[FieldUseItemName]);
            this.UseItemValue = BaseBusinessLogic.ConvertToInt(dataReader[FieldUseItemValue]);
            this.AllowEdit = BaseBusinessLogic.ConvertToInt(dataReader[FieldAllowEdit]);
            this.AllowDelete = BaseBusinessLogic.ConvertToInt(dataReader[FieldAllowDelete]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[FieldDescription]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldSortCode]);
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

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
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

        public string FullName
        {
            get
            {
                return this.fullName;
            }
            set
            {
                this.fullName = value;
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

        public int? IsTree
        {
            get
            {
                return this.isTree;
            }
            set
            {
                this.isTree = value;
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

        public string TargetTable
        {
            get
            {
                return this.targetTable;
            }
            set
            {
                this.targetTable = value;
            }
        }

        public int? UseItemCode
        {
            get
            {
                return this.useItemCode;
            }
            set
            {
                this.useItemCode = value;
            }
        }

        public int? UseItemName
        {
            get
            {
                return this.useItemName;
            }
            set
            {
                this.useItemName = value;
            }
        }

        public int? UseItemValue
        {
            get
            {
                return this.useItemValue;
            }
            set
            {
                this.useItemValue = value;
            }
        }
    }
}

