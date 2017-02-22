namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseModuleEntity
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
        private int? expand;
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
        public static string FieldExpand = "Expand";
        [NonSerialized]
        public static string FieldFullName = "FullName";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldImageIndex = "ImageIndex";
        [NonSerialized]
        public static string FieldIsMenu = "IsMenu";
        [NonSerialized]
        public static string FieldIsPublic = "IsPublic";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldNavigateUrl = "NavigateUrl";
        [NonSerialized]
        public static string FieldParentId = "ParentId";
        [NonSerialized]
        public static string FieldPermissionItemCode = "PermissionItemCode";
        [NonSerialized]
        public static string FieldPermissionScopeTables = "PermissionScopeTables";
        [NonSerialized]
        public static string FieldSelectedImageIndex = "SelectedImageIndex";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldTarget = "Target";
        private string fullName;
        private int? id;
        private string imageIndex;
        private int? isMenu;
        private int? isPublic;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string navigateUrl;
        private int? parentId;
        private string permissionItemCode;
        private string permissionScopeTables;
        private string selectedImageIndex;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_Module";
        private string target;

        public BaseModuleEntity()
        {
            this.id = null;
            this.parentId = null;
            this.code = string.Empty;
            this.fullName = string.Empty;
            this.category = string.Empty;
            this.imageIndex = string.Empty;
            this.selectedImageIndex = string.Empty;
            this.navigateUrl = string.Empty;
            this.target = string.Empty;
            this.isPublic = 1;
            this.isMenu = 1;
            this.expand = 0;
            this.permissionItemCode = "Resource.AccessPermission";
            this.permissionScopeTables = string.Empty;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.sortCode = null;
            this.deletionStateCode = null;
            this.enabled = 1;
            this.description = string.Empty;
            this.createOn = null;
            this.createUserId = string.Empty;
            this.createBy = string.Empty;
            this.modifiedOn = null;
            this.modifiedUserId = string.Empty;
            this.modifiedBy = string.Empty;
        }

        public BaseModuleEntity(DataRow dataRow)
        {
            this.id = null;
            this.parentId = null;
            this.code = string.Empty;
            this.fullName = string.Empty;
            this.category = string.Empty;
            this.imageIndex = string.Empty;
            this.selectedImageIndex = string.Empty;
            this.navigateUrl = string.Empty;
            this.target = string.Empty;
            this.isPublic = 1;
            this.isMenu = 1;
            this.expand = 0;
            this.permissionItemCode = "Resource.AccessPermission";
            this.permissionScopeTables = string.Empty;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.sortCode = null;
            this.deletionStateCode = null;
            this.enabled = 1;
            this.description = string.Empty;
            this.createOn = null;
            this.createUserId = string.Empty;
            this.createBy = string.Empty;
            this.modifiedOn = null;
            this.modifiedUserId = string.Empty;
            this.modifiedBy = string.Empty;
            this.GetFrom(dataRow);
        }

        public BaseModuleEntity(DataTable dataTable)
        {
            this.id = null;
            this.parentId = null;
            this.code = string.Empty;
            this.fullName = string.Empty;
            this.category = string.Empty;
            this.imageIndex = string.Empty;
            this.selectedImageIndex = string.Empty;
            this.navigateUrl = string.Empty;
            this.target = string.Empty;
            this.isPublic = 1;
            this.isMenu = 1;
            this.expand = 0;
            this.permissionItemCode = "Resource.AccessPermission";
            this.permissionScopeTables = string.Empty;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.sortCode = null;
            this.deletionStateCode = null;
            this.enabled = 1;
            this.description = string.Empty;
            this.createOn = null;
            this.createUserId = string.Empty;
            this.createBy = string.Empty;
            this.modifiedOn = null;
            this.modifiedUserId = string.Empty;
            this.modifiedBy = string.Empty;
            this.GetFrom(dataTable);
        }

        public BaseModuleEntity(IDataReader dataReader)
        {
            this.id = null;
            this.parentId = null;
            this.code = string.Empty;
            this.fullName = string.Empty;
            this.category = string.Empty;
            this.imageIndex = string.Empty;
            this.selectedImageIndex = string.Empty;
            this.navigateUrl = string.Empty;
            this.target = string.Empty;
            this.isPublic = 1;
            this.isMenu = 1;
            this.expand = 0;
            this.permissionItemCode = "Resource.AccessPermission";
            this.permissionScopeTables = string.Empty;
            this.allowEdit = 1;
            this.allowDelete = 1;
            this.sortCode = null;
            this.deletionStateCode = null;
            this.enabled = 1;
            this.description = string.Empty;
            this.createOn = null;
            this.createUserId = string.Empty;
            this.createBy = string.Empty;
            this.modifiedOn = null;
            this.modifiedUserId = string.Empty;
            this.modifiedBy = string.Empty;
            this.GetFrom(dataReader);
        }

        public BaseModuleEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToInt(dataRow[FieldParentId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataRow[FieldCode]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataRow[FieldFullName]);
            this.Category = BaseBusinessLogic.ConvertToString(dataRow[FieldCategory]);
            this.ImageIndex = BaseBusinessLogic.ConvertToString(dataRow[FieldImageIndex]);
            this.SelectedImageIndex = BaseBusinessLogic.ConvertToString(dataRow[FieldSelectedImageIndex]);
            this.NavigateUrl = BaseBusinessLogic.ConvertToString(dataRow[FieldNavigateUrl]);
            this.Target = BaseBusinessLogic.ConvertToString(dataRow[FieldTarget]);
            this.IsPublic = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsPublic]);
            this.IsMenu = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsMenu]);
            this.Expand = BaseBusinessLogic.ConvertToInt(dataRow[FieldExpand]);
            this.PermissionItemCode = BaseBusinessLogic.ConvertToString(dataRow[FieldPermissionItemCode]);
            this.PermissionScopeTables = BaseBusinessLogic.ConvertToString(dataRow[FieldPermissionScopeTables]);
            this.AllowEdit = BaseBusinessLogic.ConvertToInt(dataRow[FieldAllowEdit]);
            this.AllowDelete = BaseBusinessLogic.ConvertToInt(dataRow[FieldAllowDelete]);
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

        public BaseModuleEntity GetFrom(DataTable dataTable)
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

        public BaseModuleEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToInt(dataReader[FieldParentId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataReader[FieldCode]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataReader[FieldFullName]);
            this.Category = BaseBusinessLogic.ConvertToString(dataReader[FieldCategory]);
            this.ImageIndex = BaseBusinessLogic.ConvertToString(dataReader[FieldImageIndex]);
            this.SelectedImageIndex = BaseBusinessLogic.ConvertToString(dataReader[FieldSelectedImageIndex]);
            this.NavigateUrl = BaseBusinessLogic.ConvertToString(dataReader[FieldNavigateUrl]);
            this.Target = BaseBusinessLogic.ConvertToString(dataReader[FieldTarget]);
            this.IsPublic = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsPublic]);
            this.IsMenu = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsMenu]);
            this.Expand = BaseBusinessLogic.ConvertToInt(dataReader[FieldExpand]);
            this.PermissionItemCode = BaseBusinessLogic.ConvertToString(dataReader[FieldPermissionItemCode]);
            this.PermissionScopeTables = BaseBusinessLogic.ConvertToString(dataReader[FieldPermissionScopeTables]);
            this.AllowEdit = BaseBusinessLogic.ConvertToInt(dataReader[FieldAllowEdit]);
            this.AllowDelete = BaseBusinessLogic.ConvertToInt(dataReader[FieldAllowDelete]);
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

        public int? Expand
        {
            get
            {
                return this.expand;
            }
            set
            {
                this.expand = value;
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

        public string ImageIndex
        {
            get
            {
                return this.imageIndex;
            }
            set
            {
                this.imageIndex = value;
            }
        }

        public int? IsMenu
        {
            get
            {
                return this.isMenu;
            }
            set
            {
                this.isMenu = value;
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

        public string NavigateUrl
        {
            get
            {
                return this.navigateUrl;
            }
            set
            {
                this.navigateUrl = value;
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

        public string PermissionItemCode
        {
            get
            {
                return this.permissionItemCode;
            }
            set
            {
                this.permissionItemCode = value;
            }
        }

        public string PermissionScopeTables
        {
            get
            {
                return this.permissionScopeTables;
            }
            set
            {
                this.permissionScopeTables = value;
            }
        }

        public string SelectedImageIndex
        {
            get
            {
                return this.selectedImageIndex;
            }
            set
            {
                this.selectedImageIndex = value;
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

        public string Target
        {
            get
            {
                return this.target;
            }
            set
            {
                this.target = value;
            }
        }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}

