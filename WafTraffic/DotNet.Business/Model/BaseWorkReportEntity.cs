namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseWorkReportEntity
    {
        private string auditStaffId;
        private string categoryFullName;
        private string categoryId;
        private string companyId;
        private string content;
        private string createOn;
        private string createUserId;
        private string departmentId;
        private string description;
        private int? enabled;
        [NonSerialized]
        public static string FieldAuditStaffId = "AuditStaffId";
        [NonSerialized]
        public static string FieldCategoryFullName = "CategoryFullName";
        [NonSerialized]
        public static string FieldCategoryId = "CategoryId";
        [NonSerialized]
        public static string FieldCompanyId = "CompanyId";
        [NonSerialized]
        public static string FieldContent = "Content";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDepartmentId = "DepartmentId";
        [NonSerialized]
        public static string FieldDescription = "Description";
        [NonSerialized]
        public static string FieldEnabled = "Enabled";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldManHour = "ManHour";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldProjectFullName = "ProjectFullName";
        [NonSerialized]
        public static string FieldProjectId = "ProjectId";
        [NonSerialized]
        public static string FieldScore = "Score";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldStaffFullName = "StaffFullName";
        [NonSerialized]
        public static string FieldStaffId = "StaffId";
        [NonSerialized]
        public static string FieldStateCode = "StateCode";
        [NonSerialized]
        public static string FieldWorkDate = "WorkDate";
        private string id;
        private string manHour;
        private string modifiedOn;
        private string modifiedUserId;
        private string projectFullName;
        private string projectId;
        private int? score;
        private string sortCode;
        private string staffFullName;
        private string staffId;
        private string stateCode;
        [NonSerialized]
        public static string TableName = "Base_WorkReport";
        private string workDate;

        public BaseWorkReportEntity()
        {
            this.id = string.Empty;
            this.companyId = string.Empty;
            this.departmentId = string.Empty;
            this.staffId = string.Empty;
            this.staffFullName = string.Empty;
            this.categoryId = string.Empty;
            this.categoryFullName = string.Empty;
            this.projectId = string.Empty;
            this.projectFullName = string.Empty;
            this.workDate = string.Empty;
            this.content = string.Empty;
            this.manHour = string.Empty;
            this.score = 0;
            this.description = string.Empty;
            this.stateCode = string.Empty;
            this.enabled = 0;
            this.auditStaffId = string.Empty;
            this.sortCode = string.Empty;
            this.createUserId = string.Empty;
            this.createOn = string.Empty;
            this.modifiedUserId = string.Empty;
            this.modifiedOn = string.Empty;
        }

        public BaseWorkReportEntity(DataRow dataRow)
        {
            this.id = string.Empty;
            this.companyId = string.Empty;
            this.departmentId = string.Empty;
            this.staffId = string.Empty;
            this.staffFullName = string.Empty;
            this.categoryId = string.Empty;
            this.categoryFullName = string.Empty;
            this.projectId = string.Empty;
            this.projectFullName = string.Empty;
            this.workDate = string.Empty;
            this.content = string.Empty;
            this.manHour = string.Empty;
            this.score = 0;
            this.description = string.Empty;
            this.stateCode = string.Empty;
            this.enabled = 0;
            this.auditStaffId = string.Empty;
            this.sortCode = string.Empty;
            this.createUserId = string.Empty;
            this.createOn = string.Empty;
            this.modifiedUserId = string.Empty;
            this.modifiedOn = string.Empty;
            this.GetFrom(dataRow);
        }

        public BaseWorkReportEntity(DataTable dataTable)
        {
            this.id = string.Empty;
            this.companyId = string.Empty;
            this.departmentId = string.Empty;
            this.staffId = string.Empty;
            this.staffFullName = string.Empty;
            this.categoryId = string.Empty;
            this.categoryFullName = string.Empty;
            this.projectId = string.Empty;
            this.projectFullName = string.Empty;
            this.workDate = string.Empty;
            this.content = string.Empty;
            this.manHour = string.Empty;
            this.score = 0;
            this.description = string.Empty;
            this.stateCode = string.Empty;
            this.enabled = 0;
            this.auditStaffId = string.Empty;
            this.sortCode = string.Empty;
            this.createUserId = string.Empty;
            this.createOn = string.Empty;
            this.modifiedUserId = string.Empty;
            this.modifiedOn = string.Empty;
            foreach (DataRow row in dataTable.Rows)
            {
                this.GetFrom(row);
                break;
            }
        }

        public BaseWorkReportEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.WorkDate = BaseBusinessLogic.ConvertToString(dataRow[FieldWorkDate]);
            this.Content = BaseBusinessLogic.ConvertToString(dataRow[FieldContent]);
            this.ManHour = BaseBusinessLogic.ConvertToString(dataRow[FieldManHour]);
            this.ProjectId = BaseBusinessLogic.ConvertToString(dataRow[FieldProjectId]);
            if (dataRow.Table.Columns.Contains(FieldProjectFullName))
            {
                this.ProjectFullName = dataRow.IsNull(FieldProjectFullName) ? string.Empty : dataRow[FieldProjectFullName].ToString();
            }
            this.CategoryId = BaseBusinessLogic.ConvertToString(dataRow[FieldCategoryId]);
            if (dataRow.Table.Columns.Contains(FieldCategoryFullName))
            {
                this.CategoryFullName = dataRow.IsNull(FieldCategoryFullName) ? string.Empty : dataRow[FieldCategoryFullName].ToString();
            }
            this.CompanyId = BaseBusinessLogic.ConvertToString(dataRow[FieldCompanyId]);
            this.DepartmentId = BaseBusinessLogic.ConvertToString(dataRow[FieldDepartmentId]);
            this.Score = BaseBusinessLogic.ConvertToInt(dataRow[FieldScore]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.StaffId = BaseBusinessLogic.ConvertToString(dataRow[FieldStaffId]);
            if (dataRow.Table.Columns.Contains(FieldStaffFullName))
            {
                this.StaffFullName = dataRow.IsNull(FieldStaffFullName) ? string.Empty : dataRow[FieldStaffFullName].ToString();
            }
            this.AuditStaffId = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditStaffId]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.SortCode = BaseBusinessLogic.ConvertToString(dataRow[FieldSortCode]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateOn = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedOn]);
            return this;
        }

        public BaseWorkReportEntity GetFrom(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                this.GetFrom(row);
                break;
            }
            return this;
        }

        public string AuditStaffId
        {
            get
            {
                return this.auditStaffId;
            }
            set
            {
                this.auditStaffId = value;
            }
        }

        public string CategoryFullName
        {
            get
            {
                return this.categoryFullName;
            }
            set
            {
                this.categoryFullName = value;
            }
        }

        public string CategoryId
        {
            get
            {
                return this.categoryId;
            }
            set
            {
                this.categoryId = value;
            }
        }

        public string CompanyId
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

        public string CreateOn
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

        public string DepartmentId
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

        public string ManHour
        {
            get
            {
                return this.manHour;
            }
            set
            {
                this.manHour = value;
            }
        }

        public string ModifiedOn
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

        public string ProjectFullName
        {
            get
            {
                return this.projectFullName;
            }
            set
            {
                this.projectFullName = value;
            }
        }

        public string ProjectId
        {
            get
            {
                return this.projectId;
            }
            set
            {
                this.projectId = value;
            }
        }

        public int? Score
        {
            get
            {
                return this.score;
            }
            set
            {
                this.score = value;
            }
        }

        public string SortCode
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

        public string StaffFullName
        {
            get
            {
                return this.staffFullName;
            }
            set
            {
                this.staffFullName = value;
            }
        }

        public string StaffId
        {
            get
            {
                return this.staffId;
            }
            set
            {
                this.staffId = value;
            }
        }

        public string StateCode
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

        public string WorkDate
        {
            get
            {
                return this.workDate;
            }
            set
            {
                this.workDate = value;
            }
        }
    }
}

