namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseWorkFlowProcessManager : BaseManager, IBaseManager
    {
        public BaseWorkFlowProcessManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, BaseSystemInfo.WorkFlowDbConnection);
            }
            base.CurrentTableName = BaseWorkFlowProcessEntity.TableName;
        }

        public BaseWorkFlowProcessManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseWorkFlowProcessManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseWorkFlowProcessManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseWorkFlowProcessManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseWorkFlowProcessManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseWorkFlowProcessEntity baseWorkFlowProcessEntity)
        {
            return this.AddEntity(baseWorkFlowProcessEntity);
        }

        public string Add(BaseWorkFlowProcessEntity workFlowProcessEntity, out string statusCode)
        {
            string str = string.Empty;
            if (this.Exists(BaseWorkFlowProcessEntity.FieldDeletionStateCode, 0, BaseWorkFlowProcessEntity.FieldCode, workFlowProcessEntity.Code))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return str;
            }
            str = this.AddEntity(workFlowProcessEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseWorkFlowProcessEntity baseWorkFlowProcessEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseWorkFlowProcessEntity);
        }

        public string AddEntity(BaseWorkFlowProcessEntity baseWorkFlowProcessEntity)
        {
            string s = string.Empty;
            if (baseWorkFlowProcessEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseWorkFlowProcessEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseWorkFlowProcessEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldId, baseWorkFlowProcessEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseWorkFlowProcessEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseWorkFlowProcessEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!baseWorkFlowProcessEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseWorkFlowProcessEntity.Id = new int?(int.Parse(s));
                }
                sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldId, baseWorkFlowProcessEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseWorkFlowProcessEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowProcessEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowProcessEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public int BatchSave(DataTable dataTable)
        {
            int num = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                BaseWorkFlowProcessEntity baseWorkFlowProcessEntity = new BaseWorkFlowProcessEntity(row);
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BaseWorkFlowProcessEntity.FieldId, DataRowVersion.Original].ToString();
                    if (id.Length > 0)
                    {
                        num += this.Delete(id);
                    }
                }
                if ((row.RowState == DataRowState.Modified) && (row[BaseWorkFlowProcessEntity.FieldId, DataRowVersion.Original].ToString().Length > 0))
                {
                    this.GetFrom(row);
                    num += this.Update(baseWorkFlowProcessEntity);
                }
                if (row.RowState == DataRowState.Added)
                {
                    this.GetFrom(row);
                    num += (this.Add(baseWorkFlowProcessEntity).Length > 0) ? 1 : 0;
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
            return this.Delete(BaseWorkFlowProcessEntity.FieldId, id);
        }

        public BaseWorkFlowProcessEntity GetEntity(int id)
        {
            return this.GetEntity(id.ToString());
        }

        public BaseWorkFlowProcessEntity GetEntity(string id)
        {
            return new BaseWorkFlowProcessEntity(this.GetDT(BaseWorkFlowProcessEntity.FieldId, id));
        }

        public DataTable GetListByOrganize(string organizeId)
        {
            return DbLogic.GetDT(base.DbHelper, BaseWorkFlowProcessEntity.TableName, BaseWorkFlowProcessEntity.FieldOrganizeId, organizeId);
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseWorkFlowProcessEntity baseWorkFlowProcessEntity)
        {
            sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldOrganizeId, baseWorkFlowProcessEntity.OrganizeId, null);
            sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldCategoryCode, baseWorkFlowProcessEntity.CategoryCode, null);
            sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldCode, baseWorkFlowProcessEntity.Code, null);
            sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldFullName, baseWorkFlowProcessEntity.FullName, null);
            sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldContents, baseWorkFlowProcessEntity.Contents, null);
            sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldSortCode, baseWorkFlowProcessEntity.SortCode, null);
            sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldEnabled, baseWorkFlowProcessEntity.Enabled, null);
            sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldDeletionStateCode, baseWorkFlowProcessEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldDescription, baseWorkFlowProcessEntity.Description, null);
        }

        public int Update(BaseWorkFlowProcessEntity baseWorkFlowProcessEntity)
        {
            return this.UpdateEntity(baseWorkFlowProcessEntity);
        }

        public int Update(BaseWorkFlowProcessEntity workFlowProcessEntity, out string statusCode)
        {
            int num = 0;
            if (this.Exists(BaseWorkFlowProcessEntity.FieldDeletionStateCode, 0, BaseWorkFlowProcessEntity.FieldCode, workFlowProcessEntity.Code, workFlowProcessEntity.Id))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return num;
            }
            num = this.UpdateEntity(workFlowProcessEntity);
            if (num == 1)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseWorkFlowProcessEntity baseWorkFlowProcessEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseWorkFlowProcessEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseWorkFlowProcessEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseWorkFlowProcessEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseWorkFlowProcessEntity.FieldId, baseWorkFlowProcessEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

