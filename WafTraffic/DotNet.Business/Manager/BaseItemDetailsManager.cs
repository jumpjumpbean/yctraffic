namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseItemDetailsManager : BaseManager, IBaseManager
    {
        public BaseItemDetailsManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            if (string.IsNullOrEmpty(base.CurrentTableName))
            {
                base.CurrentTableName = BaseItemDetailsEntity.TableName;
            }
        }

        public BaseItemDetailsManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseItemDetailsManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseItemDetailsManager(string tableName) : this()
        {
            base.CurrentTableName = tableName;
        }

        public BaseItemDetailsManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseItemDetailsManager(IDbHelper dbHelper, string tableName) : this(dbHelper)
        {
            base.CurrentTableName = tableName;
        }

        public BaseItemDetailsManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseItemDetailsEntity baseItemDetailsEntity)
        {
            return this.AddEntity(baseItemDetailsEntity);
        }

        public string Add(BaseItemDetailsEntity itemDetailsEntity, out string statusCode)
        {
            string str = string.Empty;
            string[] names = new string[] { BaseItemDetailsEntity.FieldItemCode, BaseItemDetailsEntity.FieldItemName, BaseItemDetailsEntity.FieldDeletionStateCode };
            object[] values = new object[] { itemDetailsEntity.ItemCode, itemDetailsEntity.ItemName, 0 };
            if (this.Exists(names, values))
            {
                statusCode = StatusCode.Exist.ToString();
                return str;
            }
            str = this.AddEntity(itemDetailsEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseItemDetailsEntity baseItemDetailsEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseItemDetailsEntity);
        }

        public string AddEntity(BaseItemDetailsEntity baseItemDetailsEntity)
        {
            string s = string.Empty;
            if (baseItemDetailsEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseItemDetailsEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseItemDetailsEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseItemDetailsEntity.FieldId, baseItemDetailsEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseItemDetailsEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseItemDetailsEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (string.IsNullOrEmpty(s))
                {
                    s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                }
                baseItemDetailsEntity.Id = new int?(int.Parse(s));
                sqlBuilder.SetValue(BaseItemDetailsEntity.FieldId, baseItemDetailsEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseItemDetailsEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseItemDetailsEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseItemDetailsEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseItemDetailsEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseItemDetailsEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseItemDetailsEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseItemDetailsEntity.FieldModifiedOn);
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
            BaseItemDetailsEntity baseItemDetailsEntity = new BaseItemDetailsEntity();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BaseItemDetailsEntity.FieldId, DataRowVersion.Original].ToString();
                    if ((id.Length > 0) && (baseItemDetailsEntity.AllowDelete == 1))
                    {
                        num += this.Delete(id);
                    }
                }
                if (row.RowState == DataRowState.Modified)
                {
                    string str2 = row[BaseItemDetailsEntity.FieldId, DataRowVersion.Original].ToString();
                    if (str2.Length > 0)
                    {
                        baseItemDetailsEntity.GetFrom(row);
                        if (baseItemDetailsEntity.AllowEdit == 1)
                        {
                            num += this.UpdateEntity(baseItemDetailsEntity);
                        }
                        else
                        {
                            num += this.SetProperty(str2, BaseItemDetailsEntity.FieldSortCode, baseItemDetailsEntity.SortCode);
                        }
                    }
                }
                if (row.RowState == DataRowState.Added)
                {
                    this.GetFrom(row);
                    num += (this.AddEntity(baseItemDetailsEntity).Length > 0) ? 1 : 0;
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
            return this.Delete(BaseItemDetailsEntity.FieldId, id);
        }

        public DataTable GetDTByPermission(string userId, string resourceCategory, string permissionItemCode)
        {
            string[] ids = new BasePermissionScopeManager(base.DbHelper, base.UserInfo).GetResourceScopeIds(userId, resourceCategory, permissionItemCode);
            DataTable dT = this.GetDT(ids);
            dT.DefaultView.Sort = BaseItemDetailsEntity.FieldSortCode;
            return dT;
        }

        public BaseItemDetailsEntity GetEntity(int id)
        {
            return this.GetEntity(id.ToString());
        }

        public BaseItemDetailsEntity GetEntity(string id)
        {
            return new BaseItemDetailsEntity(this.GetDT(BaseItemDetailsEntity.FieldId, id));
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseItemDetailsEntity baseItemDetailsEntity)
        {
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldParentId, baseItemDetailsEntity.ParentId, null);
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldItemCode, baseItemDetailsEntity.ItemCode, null);
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldItemName, baseItemDetailsEntity.ItemName, null);
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldItemValue, baseItemDetailsEntity.ItemValue, null);
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldAllowEdit, baseItemDetailsEntity.AllowEdit, null);
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldAllowDelete, baseItemDetailsEntity.AllowDelete, null);
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldIsPublic, baseItemDetailsEntity.IsPublic, null);
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldEnabled, baseItemDetailsEntity.Enabled, null);
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldDeletionStateCode, baseItemDetailsEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldSortCode, baseItemDetailsEntity.SortCode, null);
            sqlBuilder.SetValue(BaseItemDetailsEntity.FieldDescription, baseItemDetailsEntity.Description, null);
        }

        public int Update(BaseItemDetailsEntity baseItemDetailsEntity)
        {
            return this.UpdateEntity(baseItemDetailsEntity);
        }

        public int Update(BaseItemDetailsEntity itemDetailsEntity, out string statusCode)
        {
            int num = 0;
            string[] names = new string[] { BaseItemDetailsEntity.FieldItemCode, BaseItemDetailsEntity.FieldItemName, BaseItemDetailsEntity.FieldDeletionStateCode };
            object[] values = new object[] { itemDetailsEntity.ItemCode, itemDetailsEntity.ItemName, 0 };
            if (this.Exists(names, values, itemDetailsEntity.Id))
            {
                statusCode = StatusCode.Exist.ToString();
                return num;
            }
            num = this.UpdateEntity(itemDetailsEntity);
            if (num == 1)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseItemDetailsEntity baseItemDetailsEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseItemDetailsEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseItemDetailsEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseItemDetailsEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseItemDetailsEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseItemDetailsEntity.FieldId, baseItemDetailsEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

