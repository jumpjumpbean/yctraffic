namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseWorkFlowStepEntity
    {
        private int? activityId;
        private string activityType;
        private int? allowEditDocuments;
        private int? allowPrint;
        private int? auditDepartmentId;
        private string auditDepartmentName;
        private string auditRoleId;
        private string auditRoleRealName;
        private string auditUserCode;
        private string auditUserId;
        private string auditUserRealName;
        private string categoryCode;
        private string code;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string description;
        private int? enabled;
        [NonSerialized]
        public static string FieldActivityId = "ActivityId";
        [NonSerialized]
        public static string FieldActivityType = "ActivityType";
        [NonSerialized]
        public static string FieldAllowEditDocuments = "AllowEditDocuments";
        [NonSerialized]
        public static string FieldAllowPrint = "AllowPrint";
        [NonSerialized]
        public static string FieldAuditDepartmentId = "AuditDepartmentId";
        [NonSerialized]
        public static string FieldAuditDepartmentName = "AuditDepartmentName";
        [NonSerialized]
        public static string FieldAuditRoleId = "AuditRoleId";
        [NonSerialized]
        public static string FieldAuditRoleRealName = "AuditRoleRealName";
        [NonSerialized]
        public static string FieldAuditUserCode = "AuditUserCode";
        [NonSerialized]
        public static string FieldAuditUserId = "AuditUserId";
        [NonSerialized]
        public static string FieldAuditUserRealName = "AuditUserRealName";
        [NonSerialized]
        public static string FieldCategoryCode = "CategoryCode";
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
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldObjectId = "ObjectId";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldWorkFlowId = "WorkFlowId";
        private string fullName;
        private int? id;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string objectId;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_WorkFlowStep";
        private int? workFlowId;

        public BaseWorkFlowStepEntity()
        {
            this.id = null;
            this.activityId = null;
            this.workFlowId = null;
            this.auditDepartmentId = null;
            this.allowPrint = 0;
            this.allowEditDocuments = 0;
            this.sortCode = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseWorkFlowStepEntity(DataRow dataRow)
        {
            this.id = null;
            this.activityId = null;
            this.workFlowId = null;
            this.auditDepartmentId = null;
            this.allowPrint = 0;
            this.allowEditDocuments = 0;
            this.sortCode = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseWorkFlowStepEntity(DataTable dataTable)
        {
            this.id = null;
            this.activityId = null;
            this.workFlowId = null;
            this.auditDepartmentId = null;
            this.allowPrint = 0;
            this.allowEditDocuments = 0;
            this.sortCode = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseWorkFlowStepEntity(IDataReader dataReader)
        {
            this.id = null;
            this.activityId = null;
            this.workFlowId = null;
            this.auditDepartmentId = null;
            this.allowPrint = 0;
            this.allowEditDocuments = 0;
            this.sortCode = null;
            this.enabled = 1;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseWorkFlowStepEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.WorkFlowId = BaseBusinessLogic.ConvertToInt(dataRow[FieldWorkFlowId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataRow[FieldCode]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataRow[FieldFullName]);
            this.AuditDepartmentId = BaseBusinessLogic.ConvertToInt(dataRow[FieldAuditDepartmentId]);
            this.AuditDepartmentName = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditDepartmentName]);
            this.AuditUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditUserId]);
            this.AuditUserCode = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditUserCode]);
            this.AuditUserRealName = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditUserRealName]);
            this.AuditRoleId = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditRoleId]);
            this.AuditRoleRealName = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditRoleRealName]);
            this.ActivityType = BaseBusinessLogic.ConvertToString(dataRow[FieldActivityType]);
            this.AllowPrint = BaseBusinessLogic.ConvertToInt(dataRow[FieldAllowPrint]);
            this.AllowEditDocuments = BaseBusinessLogic.ConvertToInt(dataRow[FieldAllowEditDocuments]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldSortCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            return this;
        }

        public BaseWorkFlowStepEntity GetFrom(DataTable dataTable)
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

        public BaseWorkFlowStepEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.WorkFlowId = BaseBusinessLogic.ConvertToInt(dataReader[FieldWorkFlowId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataReader[FieldCode]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataReader[FieldFullName]);
            this.AuditDepartmentId = BaseBusinessLogic.ConvertToInt(dataReader[FieldAuditDepartmentId]);
            this.AuditDepartmentName = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditDepartmentName]);
            this.AuditUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditUserId]);
            this.AuditUserCode = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditUserCode]);
            this.AuditUserRealName = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditUserRealName]);
            this.AuditRoleId = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditRoleId]);
            this.AuditRoleRealName = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditRoleRealName]);
            this.ActivityType = BaseBusinessLogic.ConvertToString(dataReader[FieldActivityType]);
            this.AllowPrint = BaseBusinessLogic.ConvertToInt(dataReader[FieldAllowPrint]);
            this.AllowEditDocuments = BaseBusinessLogic.ConvertToInt(dataReader[FieldAllowEditDocuments]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldSortCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedBy]);
            return this;
        }

        public int? ActivityId
        {
            get
            {
                return this.activityId;
            }
            set
            {
                this.activityId = value;
            }
        }

        public string ActivityType
        {
            get
            {
                return this.activityType;
            }
            set
            {
                this.activityType = value;
            }
        }

        public int? AllowEditDocuments
        {
            get
            {
                return this.allowEditDocuments;
            }
            set
            {
                this.allowEditDocuments = value;
            }
        }

        public int? AllowPrint
        {
            get
            {
                return this.allowPrint;
            }
            set
            {
                this.allowPrint = value;
            }
        }

        public int? AuditDepartmentId
        {
            get
            {
                return this.auditDepartmentId;
            }
            set
            {
                this.auditDepartmentId = value;
            }
        }

        public string AuditDepartmentName
        {
            get
            {
                return this.auditDepartmentName;
            }
            set
            {
                this.auditDepartmentName = value;
            }
        }

        public string AuditRoleId
        {
            get
            {
                return this.auditRoleId;
            }
            set
            {
                this.auditRoleId = value;
            }
        }

        public string AuditRoleRealName
        {
            get
            {
                return this.auditRoleRealName;
            }
            set
            {
                this.auditRoleRealName = value;
            }
        }

        public string AuditUserCode
        {
            get
            {
                return this.auditUserCode;
            }
            set
            {
                this.auditUserCode = value;
            }
        }

        public string AuditUserId
        {
            get
            {
                return this.auditUserId;
            }
            set
            {
                this.auditUserId = value;
            }
        }

        public string AuditUserRealName
        {
            get
            {
                return this.auditUserRealName;
            }
            set
            {
                this.auditUserRealName = value;
            }
        }

        public string CategoryCode
        {
            get
            {
                return this.categoryCode;
            }
            set
            {
                this.categoryCode = value;
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

        public string ObjectId
        {
            get
            {
                return this.objectId;
            }
            set
            {
                this.objectId = value;
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

        public int? WorkFlowId
        {
            get
            {
                return this.workFlowId;
            }
            set
            {
                this.workFlowId = value;
            }
        }
    }
}

