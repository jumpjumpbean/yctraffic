namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    public class BaseWorkFlowHistoryManager : BaseManager, IBaseManager
    {
        public BaseWorkFlowHistoryManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, BaseSystemInfo.WorkFlowDbConnection);
            }
            base.CurrentTableName = BaseWorkFlowHistoryTable.TableName;
        }

        public BaseWorkFlowHistoryManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseWorkFlowHistoryManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseWorkFlowHistoryManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseWorkFlowHistoryManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseWorkFlowHistoryManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseWorkFlowHistoryEntity baseWorkFlowHistoryEntity)
        {
            return this.AddEntity(baseWorkFlowHistoryEntity);
        }

        public string Add(BaseWorkFlowHistoryEntity baseWorkFlowHistoryEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseWorkFlowHistoryEntity);
        }

        public string AddEntity(BaseWorkFlowHistoryEntity baseWorkFlowHistoryEntity)
        {
            string s = string.Empty;
            if (baseWorkFlowHistoryEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseWorkFlowHistoryEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseWorkFlowHistoryTable.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldId, baseWorkFlowHistoryEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseWorkFlowHistoryTable.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseWorkFlowHistoryTable.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!baseWorkFlowHistoryEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseWorkFlowHistoryEntity.Id = new int?(int.Parse(s));
                }
                sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldId, baseWorkFlowHistoryEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseWorkFlowHistoryEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowHistoryTable.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowHistoryTable.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseWorkFlowHistoryTable.FieldId, id);
        }

        public string GetAuditRecord(string currentFlowId)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(currentFlowId))
            {
                return str;
            }
            string[] names = new string[] { BaseWorkFlowHistoryTable.FieldCurrentFlowId, BaseWorkFlowHistoryTable.FieldDeletionStateCode };
            object[] values = new object[] { currentFlowId, 0 };
            foreach (DataRow row in this.GetDT(names, values, BaseWorkFlowHistoryTable.FieldSortCode).Rows)
            {
                string str2 = str;
                str = str2 + row[BaseWorkFlowHistoryTable.FieldAuditDepartmentName].ToString() + "[" + row[BaseWorkFlowHistoryTable.FieldAuditUserRealName].ToString() + " " + BaseBusinessLogic.GetAuditStatus(row[BaseWorkFlowHistoryTable.FieldAuditStatus].ToString()) + row[BaseWorkFlowHistoryTable.FieldAuditDate].ToString() + "] → ";
            }
            return str.TrimEnd(new char[0]).TrimEnd(new char[] { '→' });
        }

        public BaseWorkFlowHistoryEntity GetEntity(int? id)
        {
            return this.GetEntity(id.Value);
        }

        public BaseWorkFlowHistoryEntity GetEntity(int id)
        {
            return new BaseWorkFlowHistoryEntity(this.GetDT(BaseWorkFlowHistoryTable.FieldId, id));
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseWorkFlowHistoryEntity baseWorkFlowHistoryEntity)
        {
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldCurrentFlowId, baseWorkFlowHistoryEntity.CurrentFlowId, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldWorkFlowId, baseWorkFlowHistoryEntity.WorkFlowId, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldActivityId, baseWorkFlowHistoryEntity.ActivityId, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldActivityFullName, baseWorkFlowHistoryEntity.ActivityFullName, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldToDepartmentId, baseWorkFlowHistoryEntity.ToDepartmentId, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldToDepartmentName, baseWorkFlowHistoryEntity.ToDepartmentName, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldToUserId, baseWorkFlowHistoryEntity.ToUserId, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldToUserRealName, baseWorkFlowHistoryEntity.ToUserRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldToRoleId, baseWorkFlowHistoryEntity.ToRoleId, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldToRoleRealName, baseWorkFlowHistoryEntity.ToRoleRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldAuditDepartmentId, baseWorkFlowHistoryEntity.AuditDepartmentId, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldAuditDepartmentName, baseWorkFlowHistoryEntity.AuditDepartmentName, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldAuditUserId, baseWorkFlowHistoryEntity.AuditUserId, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldAuditUserRealName, baseWorkFlowHistoryEntity.AuditUserRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldAuditRoleId, baseWorkFlowHistoryEntity.AuditRoleId, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldAuditRoleRealName, baseWorkFlowHistoryEntity.AuditRoleRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldSendDate, baseWorkFlowHistoryEntity.SendDate, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldAuditDate, baseWorkFlowHistoryEntity.AuditDate, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldAuditIdea, baseWorkFlowHistoryEntity.AuditIdea, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldAuditStatus, baseWorkFlowHistoryEntity.AuditStatus, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldSortCode, baseWorkFlowHistoryEntity.SortCode, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldEnabled, baseWorkFlowHistoryEntity.Enabled, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldDeletionStateCode, baseWorkFlowHistoryEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldDescription, baseWorkFlowHistoryEntity.Description, null);
        }

        public int Update(BaseWorkFlowHistoryEntity baseWorkFlowHistoryEntity)
        {
            return this.UpdateEntity(baseWorkFlowHistoryEntity);
        }

        public int UpdateEntity(BaseWorkFlowHistoryEntity baseWorkFlowHistoryEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseWorkFlowHistoryEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowHistoryTable.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowHistoryTable.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseWorkFlowHistoryTable.FieldId, baseWorkFlowHistoryEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

