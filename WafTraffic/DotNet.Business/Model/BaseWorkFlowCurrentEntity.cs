namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseWorkFlowCurrentEntity
    {
        private string activityFullName;
        private int? activityId;
        private DateTime? auditDate;
        private int? auditDepartmentId;
        private string auditDepartmentName;
        private string auditIdea;
        private int? auditRoleId;
        private string auditRoleRealName;
        private string auditStatus;
        private int? auditUserId;
        private string auditUserRealName;
        private string callBack;
        private string categoryCode;
        private string categoryFullName;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string departmentFullName;
        private int? departmentId;
        private string description;
        private int? enabled;
        [NonSerialized]
        public static string FieldActivityFullName = "ActivityFullName";
        [NonSerialized]
        public static string FieldActivityId = "ActivityId";
        [NonSerialized]
        public static string FieldAuditDate = "AuditDate";
        [NonSerialized]
        public static string FieldAuditDepartmentId = "AuditDepartmentId";
        [NonSerialized]
        public static string FieldAuditDepartmentName = "AuditDepartmentName";
        [NonSerialized]
        public static string FieldAuditIdea = "AuditIdea";
        [NonSerialized]
        public static string FieldAuditRoleId = "AuditRoleId";
        [NonSerialized]
        public static string FieldAuditRoleRealName = "AuditRoleRealName";
        [NonSerialized]
        public static string FieldAuditStatus = "AuditStatus";
        [NonSerialized]
        public static string FieldAuditUserCode = "AuditUserCode";
        [NonSerialized]
        public static string FieldAuditUserId = "AuditUserId";
        [NonSerialized]
        public static string FieldAuditUserRealName = "AuditUserRealName";
        [NonSerialized]
        public static string FieldCallBack = "CallBack";
        [NonSerialized]
        public static string FieldCategoryCode = "CategoryCode";
        [NonSerialized]
        public static string FieldCategoryFullName = "CategoryFullName";
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
        public static string FieldObjectFullName = "ObjectFullName";
        [NonSerialized]
        public static string FieldObjectId = "ObjectId";
        [NonSerialized]
        public static string FieldSendDate = "SendDate";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldToDepartmentId = "ToDepartmentId";
        [NonSerialized]
        public static string FieldToDepartmentName = "ToDepartmentName";
        [NonSerialized]
        public static string FieldToRoleId = "ToRoleId";
        [NonSerialized]
        public static string FieldToRoleRealName = "ToRoleRealName";
        [NonSerialized]
        public static string FieldToUserId = "ToUserId";
        [NonSerialized]
        public static string FieldToUserRealName = "ToUserRealName";
        [NonSerialized]
        public static string FieldWorkFlowId = "WorkFlowId";
        private string id;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string objectFullName;
        private string objectId;
        private int? roleId;
        private string roleRealName;
        private DateTime? sendDate;
        private int? sortCode;
        private string suditUserCode;
        [NonSerialized]
        public static string TableName = "Base_WorkFlowCurrent";
        private int? userId;
        private string userRealName;
        private int? workFlowId;

        public BaseWorkFlowCurrentEntity()
        {
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
            this.sortCode = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseWorkFlowCurrentEntity(DataRow dataRow)
        {
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
            this.sortCode = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseWorkFlowCurrentEntity(DataTable dataTable)
        {
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
            this.sortCode = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseWorkFlowCurrentEntity(IDataReader dataReader)
        {
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
            this.sortCode = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseWorkFlowCurrentEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.CallBack = BaseBusinessLogic.ConvertToString(dataRow[FieldCallBack]);
            this.CategoryCode = BaseBusinessLogic.ConvertToString(dataRow[FieldCategoryCode]);
            this.CategoryFullName = BaseBusinessLogic.ConvertToString(dataRow[FieldCategoryFullName]);
            this.ObjectId = BaseBusinessLogic.ConvertToString(dataRow[FieldObjectId]);
            this.ObjectFullName = BaseBusinessLogic.ConvertToString(dataRow[FieldObjectFullName]);
            this.WorkFlowId = BaseBusinessLogic.ConvertToInt(dataRow[FieldWorkFlowId]);
            this.ActivityId = BaseBusinessLogic.ConvertToInt(dataRow[FieldActivityId]);
            this.ActivityFullName = BaseBusinessLogic.ConvertToString(dataRow[FieldActivityFullName]);
            this.ToDepartmentId = BaseBusinessLogic.ConvertToInt(dataRow[FieldToDepartmentId]);
            this.ToDepartmentName = BaseBusinessLogic.ConvertToString(dataRow[FieldToDepartmentName]);
            this.ToUserId = BaseBusinessLogic.ConvertToInt(dataRow[FieldToUserId]);
            this.ToUserRealName = BaseBusinessLogic.ConvertToString(dataRow[FieldToUserRealName]);
            this.ToRoleId = BaseBusinessLogic.ConvertToInt(dataRow[FieldToRoleId]);
            this.ToRoleRealName = BaseBusinessLogic.ConvertToString(dataRow[FieldToRoleRealName]);
            this.AuditDepartmentId = BaseBusinessLogic.ConvertToInt(dataRow[FieldAuditDepartmentId]);
            this.AuditDepartmentName = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditDepartmentName]);
            this.AuditUserId = BaseBusinessLogic.ConvertToInt(dataRow[FieldAuditUserId]);
            this.AuditUserCode = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditUserCode]);
            this.AuditUserRealName = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditUserRealName]);
            this.AuditRoleId = BaseBusinessLogic.ConvertToInt(dataRow[FieldAuditRoleId]);
            this.AuditRoleRealName = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditRoleRealName]);
            this.SendDate = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldSendDate]);
            this.AuditDate = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldAuditDate]);
            this.AuditIdea = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditIdea]);
            this.AuditStatus = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditStatus]);
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

        public BaseWorkFlowCurrentEntity GetFrom(DataTable dataTable)
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

        public BaseWorkFlowCurrentEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataReader[FieldId]);
            this.CallBack = BaseBusinessLogic.ConvertToString(dataReader[FieldCallBack]);
            this.CategoryCode = BaseBusinessLogic.ConvertToString(dataReader[FieldCategoryCode]);
            this.CategoryFullName = BaseBusinessLogic.ConvertToString(dataReader[FieldCategoryFullName]);
            this.ObjectId = BaseBusinessLogic.ConvertToString(dataReader[FieldObjectId]);
            this.ObjectFullName = BaseBusinessLogic.ConvertToString(dataReader[FieldObjectFullName]);
            this.WorkFlowId = BaseBusinessLogic.ConvertToInt(dataReader[FieldWorkFlowId]);
            this.ActivityId = BaseBusinessLogic.ConvertToInt(dataReader[FieldActivityId]);
            this.ActivityFullName = BaseBusinessLogic.ConvertToString(dataReader[FieldActivityFullName]);
            this.ToDepartmentId = BaseBusinessLogic.ConvertToInt(dataReader[FieldToDepartmentId]);
            this.ToDepartmentName = BaseBusinessLogic.ConvertToString(dataReader[FieldToDepartmentName]);
            this.ToUserId = BaseBusinessLogic.ConvertToInt(dataReader[FieldToUserId]);
            this.ToUserRealName = BaseBusinessLogic.ConvertToString(dataReader[FieldToUserRealName]);
            this.ToRoleId = BaseBusinessLogic.ConvertToInt(dataReader[FieldToRoleId]);
            this.ToRoleRealName = BaseBusinessLogic.ConvertToString(dataReader[FieldToRoleRealName]);
            this.AuditDepartmentId = BaseBusinessLogic.ConvertToInt(dataReader[FieldAuditDepartmentId]);
            this.AuditDepartmentName = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditDepartmentName]);
            this.AuditUserId = BaseBusinessLogic.ConvertToInt(dataReader[FieldAuditUserId]);
            this.AuditUserCode = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditUserCode]);
            this.AuditUserRealName = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditUserRealName]);
            this.AuditRoleId = BaseBusinessLogic.ConvertToInt(dataReader[FieldAuditRoleId]);
            this.AuditRoleRealName = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditRoleRealName]);
            this.SendDate = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldSendDate]);
            this.AuditDate = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldAuditDate]);
            this.AuditIdea = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditIdea]);
            this.AuditStatus = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditStatus]);
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
                return this.auditStatus;
            }
            set
            {
                this.auditStatus = value;
            }
        }

        public string AuditUserCode
        {
            get
            {
                return this.suditUserCode;
            }
            set
            {
                this.suditUserCode = value;
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

        public string CallBack
        {
            get
            {
                return this.callBack;
            }
            set
            {
                this.callBack = value;
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

        public string ObjectFullName
        {
            get
            {
                return this.objectFullName;
            }
            set
            {
                this.objectFullName = value;
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

