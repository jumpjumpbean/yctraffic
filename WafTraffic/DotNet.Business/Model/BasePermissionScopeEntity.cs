namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BasePermissionScopeEntity
    {
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string description;
        private int? enabled;
        private DateTime? endDate;
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
        public static string FieldEndDate = "EndDate";
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
        [NonSerialized]
        public static string FieldStartDate = "StartDate";
        [NonSerialized]
        public static string FieldTargetCategory = "TargetCategory";
        [NonSerialized]
        public static string FieldTargetId = "TargetId";
        private int? id;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string permissionConstraint;
        private int? permissionId;
        private string resourceCategory;
        private string resourceId;
        private DateTime? startDate;
        [NonSerialized]
        public static string TableName = "Base_PermissionScope";
        private string targetCategory;
        private string targetId;

        public BasePermissionScopeEntity()
        {
            this.id = null;
            this.permissionId = null;
            this.startDate = null;
            this.endDate = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BasePermissionScopeEntity(DataRow dataRow)
        {
            this.id = null;
            this.permissionId = null;
            this.startDate = null;
            this.endDate = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BasePermissionScopeEntity(DataTable dataTable)
        {
            this.id = null;
            this.permissionId = null;
            this.startDate = null;
            this.endDate = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BasePermissionScopeEntity(IDataReader dataReader)
        {
            this.id = null;
            this.permissionId = null;
            this.startDate = null;
            this.endDate = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BasePermissionScopeEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.ResourceCategory = BaseBusinessLogic.ConvertToString(dataRow[FieldResourceCategory]);
            this.ResourceId = BaseBusinessLogic.ConvertToString(dataRow[FieldResourceId]);
            this.TargetCategory = BaseBusinessLogic.ConvertToString(dataRow[FieldTargetCategory]);
            this.TargetId = BaseBusinessLogic.ConvertToString(dataRow[FieldTargetId]);
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

        public BasePermissionScopeEntity GetFrom(DataTable dataTable)
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

        public BasePermissionScopeEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.ResourceCategory = BaseBusinessLogic.ConvertToString(dataReader[FieldResourceCategory]);
            this.ResourceId = BaseBusinessLogic.ConvertToString(dataReader[FieldResourceId]);
            this.TargetCategory = BaseBusinessLogic.ConvertToString(dataReader[FieldTargetCategory]);
            this.TargetId = BaseBusinessLogic.ConvertToString(dataReader[FieldTargetId]);
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

        public DateTime? EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                this.endDate = value;
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

        public DateTime? StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
            }
        }

        public string TargetCategory
        {
            get
            {
                return this.targetCategory;
            }
            set
            {
                this.targetCategory = value;
            }
        }

        public string TargetId
        {
            get
            {
                return this.targetId;
            }
            set
            {
                this.targetId = value;
            }
        }
    }
}

