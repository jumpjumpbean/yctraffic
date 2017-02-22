namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseRoleEntity
    {
        private int? allowDelete;
        private int? allowEdit;
        private string category;
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
        public static string FieldCategory = "Category";
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
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldIsVisible = "IsVisible";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldOrganizeId = "OrganizeId";
        [NonSerialized]
        public static string FieldRealName = "RealName";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldSystemId = "SystemId";
        private int? id;
        private int? isVisible;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private int? organizeId;
        private string realName;
        private int? sortCode;
        private string systemId;
        [NonSerialized]
        public static string TableName = "Base_Role";

        public BaseRoleEntity()
        {
            this.id = null;
            this.organizeId = null;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.isVisible = 1;
            this.sortCode = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseRoleEntity(DataRow dataRow)
        {
            this.id = null;
            this.organizeId = null;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.isVisible = 1;
            this.sortCode = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseRoleEntity(DataTable dataTable)
        {
            this.id = null;
            this.organizeId = null;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.isVisible = 1;
            this.sortCode = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseRoleEntity(IDataReader dataReader)
        {
            this.id = null;
            this.organizeId = null;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.isVisible = 1;
            this.sortCode = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseRoleEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.SystemId = BaseBusinessLogic.ConvertToString(dataRow[FieldSystemId]);
            this.OrganizeId = BaseBusinessLogic.ConvertToInt(dataRow[FieldOrganizeId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataRow[FieldCode]);
            this.RealName = BaseBusinessLogic.ConvertToString(dataRow[FieldRealName]);
            this.Category = BaseBusinessLogic.ConvertToString(dataRow[FieldCategory]);
            this.AllowEdit = BaseBusinessLogic.ConvertToInt(dataRow[FieldAllowEdit]);
            this.AllowDelete = BaseBusinessLogic.ConvertToInt(dataRow[FieldAllowDelete]);
            this.IsVisible = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsVisible]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldSortCode]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            return this;
        }

        public BaseRoleEntity GetFrom(DataTable dataTable)
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

        public BaseRoleEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.SystemId = BaseBusinessLogic.ConvertToString(dataReader[FieldSystemId]);
            this.OrganizeId = BaseBusinessLogic.ConvertToInt(dataReader[FieldOrganizeId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataReader[FieldCode]);
            this.RealName = BaseBusinessLogic.ConvertToString(dataReader[FieldRealName]);
            this.Category = BaseBusinessLogic.ConvertToString(dataReader[FieldCategory]);
            this.AllowEdit = BaseBusinessLogic.ConvertToInt(dataReader[FieldAllowEdit]);
            this.AllowDelete = BaseBusinessLogic.ConvertToInt(dataReader[FieldAllowDelete]);
            this.IsVisible = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsVisible]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldSortCode]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
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

        public int? IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                this.isVisible = value;
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

        public int? OrganizeId
        {
            get
            {
                return this.organizeId;
            }
            set
            {
                this.organizeId = value;
            }
        }

        public string RealName
        {
            get
            {
                return this.realName;
            }
            set
            {
                this.realName = value;
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

        public string SystemId
        {
            get
            {
                return this.systemId;
            }
            set
            {
                this.systemId = value;
            }
        }

        public override string ToString()
        {
            return this.RealName;
        }
    }
}

