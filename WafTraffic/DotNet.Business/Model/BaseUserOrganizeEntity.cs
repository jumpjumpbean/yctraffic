namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseUserOrganizeEntity
    {
        private int? companyId;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private int? departmentId;
        private string description;
        private int? enabled;
        [NonSerialized]
        public static string FieldCompanyId = "CompanyId";
        [NonSerialized]
        public static string FieldCreateBy = "CreateBy";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDeletionStateCode = "DeletionStateCode";
        [NonSerialized]
        public static string FieldDepartmentId = "DepartmentId";
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
        public static string FieldRoleId = "RoleId";
        [NonSerialized]
        public static string FieldUserId = "UserId";
        [NonSerialized]
        public static string FieldWorkgroupId = "WorkgroupId";
        private int? id;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private int? roleId;
        [NonSerialized]
        public static string TableName = "Base_UserOrganize";
        private int? userId;
        private int? workgroupId;

        public BaseUserOrganizeEntity()
        {
            this.id = 0;
            this.userId = 0;
            this.companyId = 0;
            this.departmentId = 0;
            this.workgroupId = 0;
            this.roleId = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseUserOrganizeEntity(DataRow dataRow)
        {
            this.id = 0;
            this.userId = 0;
            this.companyId = 0;
            this.departmentId = 0;
            this.workgroupId = 0;
            this.roleId = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseUserOrganizeEntity(DataTable dataTable)
        {
            this.id = 0;
            this.userId = 0;
            this.companyId = 0;
            this.departmentId = 0;
            this.workgroupId = 0;
            this.roleId = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseUserOrganizeEntity(IDataReader dataReader)
        {
            this.id = 0;
            this.userId = 0;
            this.companyId = 0;
            this.departmentId = 0;
            this.workgroupId = 0;
            this.roleId = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseUserOrganizeEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.UserId = BaseBusinessLogic.ConvertToInt(dataRow[FieldUserId]);
            this.CompanyId = BaseBusinessLogic.ConvertToInt(dataRow[FieldCompanyId]);
            this.DepartmentId = BaseBusinessLogic.ConvertToInt(dataRow[FieldDepartmentId]);
            this.WorkgroupId = BaseBusinessLogic.ConvertToInt(dataRow[FieldWorkgroupId]);
            this.RoleId = BaseBusinessLogic.ConvertToInt(dataRow[FieldRoleId]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            return this;
        }

        public BaseUserOrganizeEntity GetFrom(DataTable dataTable)
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

        public BaseUserOrganizeEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.UserId = BaseBusinessLogic.ConvertToInt(dataReader[FieldUserId]);
            this.CompanyId = BaseBusinessLogic.ConvertToInt(dataReader[FieldCompanyId]);
            this.DepartmentId = BaseBusinessLogic.ConvertToInt(dataReader[FieldDepartmentId]);
            this.WorkgroupId = BaseBusinessLogic.ConvertToInt(dataReader[FieldWorkgroupId]);
            this.RoleId = BaseBusinessLogic.ConvertToInt(dataReader[FieldRoleId]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[FieldDescription]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedBy]);
            return this;
        }

        public int? CompanyId
        {
            get
            {
                return this.companyId;
            }
            set
            {
                this.companyId = value;
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

        public int? DepartmentId
        {
            get
            {
                return this.departmentId;
            }
            set
            {
                this.departmentId = value;
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

        public int? RoleId
        {
            get
            {
                return this.roleId;
            }
            set
            {
                this.roleId = value;
            }
        }

        public int? UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
            }
        }

        public int? WorkgroupId
        {
            get
            {
                return this.workgroupId;
            }
            set
            {
                this.workgroupId = value;
            }
        }
    }
}

