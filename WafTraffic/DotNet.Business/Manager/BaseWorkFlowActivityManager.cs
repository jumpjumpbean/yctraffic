namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseWorkFlowActivityManager : BaseManager, IBaseManager
    {
        public BaseWorkFlowActivityManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, BaseSystemInfo.WorkFlowDbConnection);
            }
            base.CurrentTableName = BaseWorkFlowActivityEntity.TableName;
        }

        public BaseWorkFlowActivityManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseWorkFlowActivityManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseWorkFlowActivityManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseWorkFlowActivityManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseWorkFlowActivityManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseWorkFlowActivityEntity baseWorkFlowActivityEntity)
        {
            return this.AddEntity(baseWorkFlowActivityEntity);
        }

        public string Add(BaseWorkFlowActivityEntity baseWorkFlowActivityEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseWorkFlowActivityEntity);
        }

        public string AddEntity(BaseWorkFlowActivityEntity workFlowActivityEntity)
        {
            string s = string.Empty;
            if (!workFlowActivityEntity.SortCode.HasValue || (workFlowActivityEntity.SortCode == 0))
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                workFlowActivityEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseWorkFlowActivityEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldId, workFlowActivityEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseWorkFlowActivityEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseWorkFlowActivityEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!workFlowActivityEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    workFlowActivityEntity.Id = new int?(int.Parse(s));
                }
                sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldId, workFlowActivityEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, workFlowActivityEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowActivityEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowActivityEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public override int BatchSave(DataTable dataTable)
        {
            int num = 0;
            BaseWorkFlowActivityEntity baseWorkFlowActivityEntity = new BaseWorkFlowActivityEntity();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BaseRoleEntity.FieldId, DataRowVersion.Original].ToString();
                    if (id.Length > 0)
                    {
                        num += this.Delete(id);
                    }
                }
                if ((row.RowState == DataRowState.Modified) && !string.IsNullOrEmpty(row[BaseRoleEntity.FieldId, DataRowVersion.Original].ToString()))
                {
                    baseWorkFlowActivityEntity.GetFrom(row);
                    num += this.UpdateEntity(baseWorkFlowActivityEntity);
                }
                if (row.RowState == DataRowState.Added)
                {
                    baseWorkFlowActivityEntity.GetFrom(row);
                    num += (this.AddEntity(baseWorkFlowActivityEntity).Length > 0) ? 1 : 0;
                }
                if (row.RowState != DataRowState.Unchanged)
                {
                    DataRowState rowState = row.RowState;
                }
            }
            return num;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseWorkFlowActivityEntity.FieldId, id);
        }

        public DataTable GetActivityDT(string workFlowId)
        {
            DataTable table = null;
            if (!string.IsNullOrEmpty(workFlowId))
            {
                string[] names = new string[] { BaseWorkFlowActivityEntity.FieldWorkFlowId, BaseWorkFlowActivityEntity.FieldDeletionStateCode };
                object[] values = new object[] { workFlowId, 0 };
                table = this.GetDT(names, values, BaseWorkFlowActivityEntity.FieldSortCode);
            }
            return table;
        }

        public DataTable GetActivityDTByCode(string workFlowCode)
        {
            string workFlowId = new BaseWorkFlowProcessManager(base.DbHelper, base.UserInfo).GetId(BaseWorkFlowProcessEntity.FieldDeletionStateCode, 0, BaseWorkFlowProcessEntity.FieldCode, workFlowCode);
            return this.GetActivityDT(workFlowId);
        }

        //public DataTable GetAuthorizeDT(string permissionItemCode, string userId = null) //C# 4.0 才支持缺省参数
        public DataTable GetAuthorizeDT(string permissionItemCode, string userId) 
        {
            if (userId == null)
            {
                userId = base.UserInfo.Id;
            }
            string permissionItemId = string.Empty;
            permissionItemId = new BasePermissionItemManager(base.UserInfo).GetIdByCode(permissionItemCode);
            BasePermissionScopeManager manager2 = new BasePermissionScopeManager(base.UserInfo);
            string fieldDeletionStateCode = BasePermissionScopeEntity.FieldDeletionStateCode;
            string fieldEnabled = BasePermissionScopeEntity.FieldEnabled;
            string fieldResourceCategory = BasePermissionScopeEntity.FieldResourceCategory;
            string fieldPermissionItemId = BasePermissionScopeEntity.FieldPermissionItemId;
            string fieldTargetCategory = BasePermissionScopeEntity.FieldTargetCategory;
            string fieldTargetId = BasePermissionScopeEntity.FieldTargetId;
            string tableName = BaseUserEntity.TableName;
            string text8 = BaseUserEntity.TableName;
            string[] ids = BaseBusinessLogic.FieldToArray(manager2.GetAuthoriedList(BaseUserEntity.TableName, permissionItemId, BaseUserEntity.TableName, userId), BasePermissionScopeEntity.FieldResourceId);
            BaseUserManager manager3 = new BaseUserManager(base.UserInfo);
            return manager3.GetDT(ids);
        }

        public DataTable GetBackToDT(BaseWorkFlowCurrentEntity entity)
        {
            string str = entity.WorkFlowId.ToString();
            string str2 = entity.SortCode.ToString();
            string commandText = "  SELECT MAX(Id) AS Id, ActivityId, AuditUserId, AuditUserRealname\r\n                                    FROM Base_WorkFlowHistory\r\n                                    WHERE ( AuditStatus != 'AuditReject'\r\n                                            AND CurrentFlowId = '" + entity.Id + "'\r\n                                            AND WorkFlowId = " + str + "\r\n                                            -- AND AuditUserId = ToUserId\r\n                                            AND ActivityId IN\r\n                                                (\r\n                                                   SELECT Id\r\n                                                     FROM Base_WorkFlowActivity\r\n                                                    WHERE (DeletionStateCode = 0) \r\n                                                          AND (Enabled = 1) \r\n                                                          AND (WorkFlowId = " + str + ") \r\n                                                          AND (SortCode < " + str2 + ")                         \r\n                                                )\r\n                                          )\r\n                                 GROUP BY ActivityId, AuditUserId, AuditUserRealname\r\n                                 ORDER BY MAX(SortCode) ";
            return base.DbHelper.Fill(commandText);
        }

        public DataTable GetBackToDT(string currentFlowId, string workFlowId)
        {
            string commandText = "  SELECT MAX(Id) AS Id, ActivityId, AuditUserId, AuditUserRealname\r\n                                    FROM Base_WorkFlowHistory\r\n                                    WHERE ( AuditStatus != 'AuditReject'\r\n                                            AND CurrentFlowId = '" + currentFlowId + "'\r\n                                            AND WorkFlowId = " + workFlowId + "\r\n                                            -- AND AuditUserId = ToUserId\r\n                                          )\r\n                                 GROUP BY ActivityId, AuditUserId, AuditUserRealname\r\n                                 ORDER BY MAX(SortCode) ";
            return base.DbHelper.Fill(commandText);
        }

        public string GetBackToUserId(BaseWorkFlowCurrentEntity entity)
        {
            string str = string.Empty;
            DataTable backToDT = this.GetBackToDT(entity);
            if (backToDT.Rows.Count > 0)
            {
                str = backToDT.Rows[0][BaseWorkFlowHistoryTable.FieldAuditUserId].ToString();
            }
            return str;
        }

        public BaseWorkFlowActivityEntity GetEntity(int id)
        {
            return this.GetEntity(id.ToString());
        }

        public BaseWorkFlowActivityEntity GetEntity(int? id)
        {
            return this.GetEntity(id.Value);
        }

        public BaseWorkFlowActivityEntity GetEntity(string id)
        {
            return new BaseWorkFlowActivityEntity(this.GetDT(BaseWorkFlowActivityEntity.FieldId, id));
        }

        public string GetWorkFlowActivity(string workFlowId)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            if (!string.IsNullOrEmpty(workFlowId))
            {
                foreach (DataRow row in this.GetActivityDT(workFlowId).Rows)
                {
                    if (row[BaseWorkFlowActivityEntity.FieldEnabled].ToString().Equals("1"))
                    {
                        string str3 = str;
                        str = str3 + row[BaseWorkFlowActivityEntity.FieldAuditDepartmentName].ToString() + "[" + row[BaseWorkFlowActivityEntity.FieldAuditUserRealName].ToString() + "." + row[BaseWorkFlowActivityEntity.FieldAuditUserCode].ToString() + "] → ";
                    }
                    else
                    {
                        string str4 = str2;
                        str2 = str4 + row[BaseWorkFlowActivityEntity.FieldAuditDepartmentName].ToString() + "[" + row[BaseWorkFlowActivityEntity.FieldAuditUserRealName].ToString() + "." + row[BaseWorkFlowActivityEntity.FieldAuditUserCode].ToString() + "] → ";
                    }
                }
                str = str.TrimEnd(new char[0]).TrimEnd(new char[] { '→' });
                str2 = str2.TrimEnd(new char[0]).TrimEnd(new char[] { '→' });
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = "审核：" + str;
            }
            if (!string.IsNullOrEmpty(str2))
            {
                str = str + "<br>共享：" + str2;
            }
            return str;
        }

        public string GetWorkFlowActivityByCode(string workFlowCode)
        {
            string workFlowId = new BaseWorkFlowProcessManager(base.DbHelper, base.UserInfo).GetId(BaseWorkFlowProcessEntity.FieldDeletionStateCode, 0, BaseWorkFlowProcessEntity.FieldCode, workFlowCode);
            return this.GetWorkFlowActivity(workFlowId);
        }

        public int ReplaceUser(string oldUserId, string newUserId)
        {
            BaseUserEntity entity = new BaseUserManager(base.UserInfo).GetEntity(newUserId);
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(base.CurrentTableName);
            builder.SetValue(BaseWorkFlowActivityEntity.FieldAuditUserId, entity.Id, null);
            builder.SetValue(BaseWorkFlowActivityEntity.FieldAuditUserCode, entity.Code, null);
            builder.SetValue(BaseWorkFlowActivityEntity.FieldAuditUserRealName, entity.RealName, null);
            builder.SetValue(BaseWorkFlowActivityEntity.FieldAuditDepartmentId, entity.DepartmentId, null);
            builder.SetValue(BaseWorkFlowActivityEntity.FieldAuditDepartmentName, entity.DepartmentName, null);
            builder.SetWhere(BaseWorkFlowActivityEntity.FieldAuditUserId, oldUserId, "OldUserId", " AND ");
            return builder.EndUpdate();
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseWorkFlowActivityEntity baseWorkFlowActivityEntity)
        {
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldWorkFlowId, baseWorkFlowActivityEntity.WorkFlowId, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldCode, baseWorkFlowActivityEntity.Code, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldFullName, baseWorkFlowActivityEntity.FullName, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldAuditDepartmentId, baseWorkFlowActivityEntity.AuditDepartmentId, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldAuditDepartmentName, baseWorkFlowActivityEntity.AuditDepartmentName, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldAuditUserId, baseWorkFlowActivityEntity.AuditUserId, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldAuditUserCode, baseWorkFlowActivityEntity.AuditUserCode, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldAuditUserRealName, baseWorkFlowActivityEntity.AuditUserRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldAuditRoleId, baseWorkFlowActivityEntity.AuditRoleId, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldAuditRoleRealName, baseWorkFlowActivityEntity.AuditRoleRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldActivityType, baseWorkFlowActivityEntity.ActivityType, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldAllowPrint, baseWorkFlowActivityEntity.AllowPrint, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldAllowEditDocuments, baseWorkFlowActivityEntity.AllowEditDocuments, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldSortCode, baseWorkFlowActivityEntity.SortCode, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldEnabled, baseWorkFlowActivityEntity.Enabled, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldDeletionStateCode, baseWorkFlowActivityEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldDescription, baseWorkFlowActivityEntity.Description, null);
        }

        public int Update(BaseWorkFlowActivityEntity baseWorkFlowActivityEntity)
        {
            return this.UpdateEntity(baseWorkFlowActivityEntity);
        }

        public int UpdateEntity(BaseWorkFlowActivityEntity baseWorkFlowActivityEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseWorkFlowActivityEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowActivityEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowActivityEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseWorkFlowActivityEntity.FieldId, baseWorkFlowActivityEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

