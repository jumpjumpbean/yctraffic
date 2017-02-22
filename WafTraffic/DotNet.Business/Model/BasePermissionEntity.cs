namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BasePermissionEntity
    {
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string description;
        private int? enabled;
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
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldPermissionConstraint = "PermissionConstraint";
        [NonSerialized]
        public static string FieldPermissionItemId = "PermissionId";
        [NonSerialized]
        public static string FieldResourceCategory = "ResourceCategory";
        [NonSerialized]
        public static string FieldResourceId = "ResourceId";
        private int? id;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string permissionConstraint;
        private int? permissionId;
        private string resourceCategory;
        private string resourceId;
        [NonSerialized]
        public static string TableName = "Base_Permission";

        public BasePermissionEntity()
        {
            this.id = null;
            this.permissionId = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BasePermissionEntity(DataRow dataRow)
        {
            this.id = null;
            this.permissionId = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BasePermissionEntity(DataTable dataTable)
        {
            this.id = null;
            this.permissionId = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BasePermissionEntity(IDataReader dataReader)
        {
            this.id = null;
            this.permissionId = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BasePermissionEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.ResourceId = BaseBusinessLogic.ConvertToString(dataRow[FieldResourceId]);
            this.ResourceCategory = BaseBusinessLogic.ConvertToString(dataRow[FieldResourceCategory]);
            this.PermissionId = BaseBusinessLogic.ConvertToInt(dataRow[FieldPermissionItemId]);
            this.PermissionConstraint = BaseBusinessLogic.ConvertToString(dataRow[FieldPermissionConstraint]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            return this;
        }

        public BasePermissionEntity GetFrom(DataTable dataTable)
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

        public BasePermissionEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.ResourceId = BaseBusinessLogic.ConvertToString(dataReader[FieldResourceId]);
            this.ResourceCategory = BaseBusinessLogic.ConvertToString(dataReader[FieldResourceCategory]);
            this.PermissionId = BaseBusinessLogic.ConvertToInt(dataReader[FieldPermissionItemId]);
            this.PermissionConstraint = BaseBusinessLogic.ConvertToString(dataReader[FieldPermissionConstraint]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldCreateOn]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateBy]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateUserId]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldModifiedOn]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedBy]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedUserId]);
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

        public string PermissionConstraint
        {
            get
            {
                return this.permissionConstraint;
            }
            set
            {
                this.permissionConstraint = value;
            }
        }

        public int? PermissionId
        {
            get
            {
                return this.permissionId;
            }
            set
            {
                this.permissionId = value;
            }
        }

        public string ResourceCategory
        {
            get
            {
                return this.resourceCategory;
            }
            set
            {
                this.resourceCategory = value;
            }
        }

        public string ResourceId
        {
            get
            {
                return this.resourceId;
            }
            set
            {
                this.resourceId = value;
            }
        }
    }
}

