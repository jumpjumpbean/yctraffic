namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;

    public class BaseWorkFlowStepManager : BaseManager, IBaseManager
    {
        public BaseWorkFlowStepManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, BaseSystemInfo.WorkFlowDbConnection);
            }
            base.CurrentTableName = BaseWorkFlowStepEntity.TableName;
        }

        public BaseWorkFlowStepManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseWorkFlowStepManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseWorkFlowStepManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseWorkFlowStepManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseWorkFlowStepManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseWorkFlowStepEntity BaseWorkFlowStepEntity)
        {
            return this.AddEntity(BaseWorkFlowStepEntity);
        }

        public string Add(BaseWorkFlowStepEntity BaseWorkFlowStepEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(BaseWorkFlowStepEntity);
        }

        public string AddEntity(BaseWorkFlowStepEntity workFlowActivityEntity)
        {
            string s = string.Empty;
            if (!workFlowActivityEntity.SortCode.HasValue || (workFlowActivityEntity.SortCode == 0))
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                workFlowActivityEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseWorkFlowStepEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldId, workFlowActivityEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseWorkFlowStepEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseWorkFlowStepEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
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
                sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldId, workFlowActivityEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, workFlowActivityEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowStepEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowStepEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseWorkFlowStepEntity.FieldId, id);
        }

        public BaseWorkFlowStepEntity GetEntity(int? id)
        {
            return this.GetEntity(id.Value);
        }

        public BaseWorkFlowStepEntity GetEntity(int id)
        {
            return this.GetEntity(id.ToString());
        }

        public BaseWorkFlowStepEntity GetEntity(string id)
        {
            return new BaseWorkFlowStepEntity(this.GetDT(BaseWorkFlowStepEntity.FieldId, id));
        }

        public int ReplaceUser(string oldUserId, string newUserId)
        {
            BaseUserEntity entity = new BaseUserManager(base.UserInfo).GetEntity(newUserId);
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(base.CurrentTableName);
            builder.SetValue(BaseWorkFlowStepEntity.FieldAuditUserId, entity.Id, null);
            builder.SetValue(BaseWorkFlowStepEntity.FieldAuditUserCode, entity.Code, null);
            builder.SetValue(BaseWorkFlowStepEntity.FieldAuditUserRealName, entity.RealName, null);
            builder.SetValue(BaseWorkFlowStepEntity.FieldAuditDepartmentId, entity.DepartmentId, null);
            builder.SetValue(BaseWorkFlowStepEntity.FieldAuditDepartmentName, entity.DepartmentName, null);
            builder.SetWhere(BaseWorkFlowStepEntity.FieldAuditUserId, oldUserId, "OldUserId", " AND ");
            return builder.EndUpdate();
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseWorkFlowStepEntity BaseWorkFlowStepEntity)
        {
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldCategoryCode, BaseWorkFlowStepEntity.CategoryCode, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldObjectId, BaseWorkFlowStepEntity.ObjectId, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldWorkFlowId, BaseWorkFlowStepEntity.WorkFlowId, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldActivityId, BaseWorkFlowStepEntity.ActivityId, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldCode, BaseWorkFlowStepEntity.Code, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldFullName, BaseWorkFlowStepEntity.FullName, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldAuditDepartmentId, BaseWorkFlowStepEntity.AuditDepartmentId, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldAuditDepartmentName, BaseWorkFlowStepEntity.AuditDepartmentName, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldAuditUserId, BaseWorkFlowStepEntity.AuditUserId, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldAuditUserCode, BaseWorkFlowStepEntity.AuditUserCode, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldAuditUserRealName, BaseWorkFlowStepEntity.AuditUserRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldAuditRoleId, BaseWorkFlowStepEntity.AuditRoleId, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldAuditRoleRealName, BaseWorkFlowStepEntity.AuditRoleRealName, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldActivityType, BaseWorkFlowStepEntity.ActivityType, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldAllowPrint, BaseWorkFlowStepEntity.AllowPrint, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldAllowEditDocuments, BaseWorkFlowStepEntity.AllowEditDocuments, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldSortCode, BaseWorkFlowStepEntity.SortCode, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldEnabled, BaseWorkFlowStepEntity.Enabled, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldDeletionStateCode, BaseWorkFlowStepEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldDescription, BaseWorkFlowStepEntity.Description, null);
        }

        public int Update(BaseWorkFlowStepEntity BaseWorkFlowStepEntity)
        {
            return this.UpdateEntity(BaseWorkFlowStepEntity);
        }

        public int UpdateEntity(BaseWorkFlowStepEntity BaseWorkFlowStepEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, BaseWorkFlowStepEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowStepEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowStepEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseWorkFlowStepEntity.FieldId, BaseWorkFlowStepEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

