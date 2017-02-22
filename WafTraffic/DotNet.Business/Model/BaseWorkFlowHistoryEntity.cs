namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseWorkFlowHistoryEntity
    {
        private string activityFullName;
        private int? activityId;
        private DateTime? auditDate;
        private int? auditDepartmentId;
        private string auditDepartmentName;
        private string auditIdea;
        private int? auditRoleId;
        private string auditRoleRealName;
        private string auditState;
        private int? auditUserId;
        private string auditUserRealName;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private string currentFlowId;
        private int? deletionStateCode;
        private string departmentFullName;
        private int? departmentId;
        private string description;
        private int? enabled;
        private int? id;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private int? roleId;
        private string roleRealName;
        private DateTime? sendDate;
        private int? sortCode;
        private int? userId;
        private string userRealName;
        private int? workFlowId;

        public BaseWorkFlowHistoryEntity()
        {
            this.id = null;
            this.workFlowId = null;
            this.activityId = null;
            this.departmentId = null;
            this.userId = null;
            this.roleId = null;
            this.auditDepartmentId = null;
            this.auditUserId = null;
            this.auditRoleId = null;
            this.sendDate = null;
            this.auditDate = null;
            this.sortCode = null;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseWorkFlowHistoryEntity(DataRow dataRow)
        {
            this.id = null;
            this.workFlowId = null;
            this.activityId = null;
            this.departmentId = null;
            this.userId = null;
            this.roleId = null;
            this.auditDepartmentId = null;
            this.auditUserId = null;
            this.auditRoleId = null;
            this.sendDate = null;
            this.auditDate = null;
            this.sortCode = null;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseWorkFlowHistoryEntity(DataTable dataTable)
        {
            this.id = null;
            this.workFlowId = null;
            this.activityId = null;
            this.departmentId = null;
            this.userId = null;
            this.roleId = null;
            this.auditDepartmentId = null;
            this.auditUserId = null;
            this.auditRoleId = null;
            this.sendDate = null;
            this.auditDate = null;
            this.sortCode = null;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseWorkFlowHistoryEntity(IDataReader dataReader)
        {
            this.id = null;
            this.workFlowId = null;
            this.activityId = null;
            this.departmentId = null;
            this.userId = null;
            this.roleId = null;
            this.auditDepartmentId = null;
            this.auditUserId = null;
            this.auditRoleId = null;
            this.sendDate = null;
            this.auditDate = null;
            this.sortCode = null;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseWorkFlowHistoryEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldId]);
            this.CurrentFlowId = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldCurrentFlowId]);
            this.WorkFlowId = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldWorkFlowId]);
            this.ActivityId = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldActivityId]);
            this.ActivityFullName = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldActivityFullName]);
            this.ToDepartmentId = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldToDepartmentId]);
            this.ToDepartmentName = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldToDepartmentName]);
            this.ToUserId = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldToUserId]);
            this.ToUserRealName = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldToUserRealName]);
            this.ToRoleId = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldToRoleId]);
            this.ToRoleRealName = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldToRoleRealName]);
            this.AuditDepartmentId = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldAuditDepartmentId]);
            this.AuditDepartmentName = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldAuditDepartmentName]);
            this.AuditUserId = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldAuditUserId]);
            this.AuditUserRealName = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldAuditUserRealName]);
            this.AuditRoleId = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldAuditRoleId]);
            this.AuditRoleRealName = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldAuditRoleRealName]);
            this.SendDate = BaseBusinessLogic.ConvertToDateTime(dataRow[BaseWorkFlowHistoryTable.FieldSendDate]);
            this.AuditDate = BaseBusinessLogic.ConvertToDateTime(dataRow[BaseWorkFlowHistoryTable.FieldAuditDate]);
            this.AuditIdea = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldAuditIdea]);
            this.AuditStatus = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldAuditStatus]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldSortCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[BaseWorkFlowHistoryTable.FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[BaseWorkFlowHistoryTable.FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[BaseWorkFlowHistoryTable.FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[BaseWorkFlowHistoryTable.FieldModifiedBy]);
            return this;
        }

        public BaseWorkFlowHistoryEntity GetFrom(DataTable dataTable)
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

        public BaseWorkFlowHistoryEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldId]);
            this.CurrentFlowId = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldCurrentFlowId]);
            this.WorkFlowId = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldWorkFlowId]);
            this.ActivityId = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldActivityId]);
            this.ActivityFullName = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldActivityFullName]);
            this.ToDepartmentId = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldToDepartmentId]);
            this.ToDepartmentName = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldToDepartmentName]);
            this.ToUserId = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldToUserId]);
            this.ToUserRealName = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldToUserRealName]);
            this.ToRoleId = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldToRoleId]);
            this.ToRoleRealName = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldToRoleRealName]);
            this.AuditDepartmentId = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldAuditDepartmentId]);
            this.AuditDepartmentName = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldAuditDepartmentName]);
            this.AuditUserId = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldAuditUserId]);
            this.AuditUserRealName = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldAuditUserRealName]);
            this.AuditRoleId = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldAuditRoleId]);
            this.AuditRoleRealName = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldAuditRoleRealName]);
            this.SendDate = BaseBusinessLogic.ConvertToDateTime(dataReader[BaseWorkFlowHistoryTable.FieldSendDate]);
            this.AuditDate = BaseBusinessLogic.ConvertToDateTime(dataReader[BaseWorkFlowHistoryTable.FieldAuditDate]);
            this.AuditIdea = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldAuditIdea]);
            this.AuditStatus = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldAuditStatus]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldSortCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[BaseWorkFlowHistoryTable.FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataReader[BaseWorkFlowHistoryTable.FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataReader[BaseWorkFlowHistoryTable.FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataReader[BaseWorkFlowHistoryTable.FieldModifiedBy]);
            return this;
        }

        public string ActivityFullName
        {
            get
            {
                return this.activityFullName;
            }
            set
            {
                this.activityFullName = value;
            }
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

        public DateTime? AuditDate
        {
            get
            {
                return this.auditDate;
            }
            set
            {
                this.auditDate = value;
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

        public string AuditIdea
        {
            get
            {
                return this.auditIdea;
            }
            set
            {
                this.auditIdea = value;
            }
        }

        public int? AuditRoleId
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

        public string AuditStatus
        {
            get
            {
                return this.auditState;
            }
            set
            {
                this.auditState = value;
            }
        }

        public int? AuditUserId
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

        public string CurrentFlowId
        {
            get
            {
                return this.currentFlowId;
            }
            set
            {
                this.currentFlowId = value;
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

        public DateTime? SendDate
        {
            get
            {
                return this.sendDate;
            }
            set
            {
                this.sendDate = value;
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

        public int? ToDepartmentId
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

        public string ToDepartmentName
        {
            get
            {
                return this.departmentFullName;
            }
            set
            {
                this.departmentFullName = value;
            }
        }

        public int? ToRoleId
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

        public string ToRoleRealName
        {
            get
            {
                return this.roleRealName;
            }
            set
            {
                this.roleRealName = value;
            }
        }

        public int? ToUserId
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

        public string ToUserRealName
        {
            get
            {
                return this.userRealName;
            }
            set
            {
                this.userRealName = value;
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

