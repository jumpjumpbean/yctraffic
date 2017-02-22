namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BasePermissionItemManager : BaseManager, IBaseManager
    {
        private static readonly object PermissionItemLock = new object();

        public BasePermissionItemManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BasePermissionItemEntity.TableName;
        }

        public BasePermissionItemManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BasePermissionItemManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BasePermissionItemManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BasePermissionItemManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BasePermissionItemManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BasePermissionItemEntity basePermissionItemEntity)
        {
            return this.AddEntity(basePermissionItemEntity);
        }

        public string Add(BasePermissionItemEntity permissionItemEntity, out string statusCode)
        {
            string str = string.Empty;
            if (this.Exists(BasePermissionItemEntity.FieldDeletionStateCode, 0, BasePermissionItemEntity.FieldCode, permissionItemEntity.Code))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return str;
            }
            str = this.AddEntity(permissionItemEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BasePermissionItemEntity basePermissionItemEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(basePermissionItemEntity);
        }

        public string AddByDetail(string code, string fullName, out string statusCode)
        {
            BasePermissionItemEntity permissionItemEntity = new BasePermissionItemEntity {
                Code = code,
                FullName = fullName
            };
            return this.Add(permissionItemEntity, out statusCode);
        }

        public string AddEntity(BasePermissionItemEntity basePermissionItemEntity)
        {
            string s = string.Empty;
            if (basePermissionItemEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                basePermissionItemEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(BasePermissionItemEntity.TableName, BasePermissionItemEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BasePermissionItemEntity.FieldId, basePermissionItemEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BasePermissionItemEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BasePermissionItemEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!basePermissionItemEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    basePermissionItemEntity.Id = new int?(int.Parse(s));
                }
                sqlBuilder.SetValue(BasePermissionItemEntity.FieldId, basePermissionItemEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, basePermissionItemEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BasePermissionItemEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BasePermissionItemEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BasePermissionItemEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BasePermissionItemEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BasePermissionItemEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BasePermissionItemEntity.FieldModifiedOn);
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
            BasePermissionItemEntity basePermissionItemEntity = new BasePermissionItemEntity();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BasePermissionItemEntity.FieldId, DataRowVersion.Original].ToString();
                    if ((id.Length > 0) && row[BasePermissionItemEntity.FieldAllowDelete, DataRowVersion.Original].ToString().Equals("1"))
                    {
                        num += this.DeleteEntity(id);
                    }
                }
                if (row.RowState == DataRowState.Modified)
                {
                    string whereValue = row[BasePermissionItemEntity.FieldId, DataRowVersion.Original].ToString();
                    if (whereValue.Length > 0)
                    {
                        basePermissionItemEntity.GetFrom(row);
                        if (basePermissionItemEntity.AllowEdit == 1)
                        {
                            num += this.UpdateEntity(basePermissionItemEntity);
                        }
                        else
                        {
                            num += this.SetProperty(BasePermissionItemEntity.FieldId, whereValue, BasePermissionItemEntity.FieldSortCode, basePermissionItemEntity.SortCode);
                        }
                    }
                }
                if (row.RowState == DataRowState.Added)
                {
                    basePermissionItemEntity.GetFrom(row);
                    num += (this.AddEntity(basePermissionItemEntity).Length > 0) ? 1 : 0;
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
            return this.Delete(BasePermissionItemEntity.FieldId, id);
        }

        public int Delete(string id)
        {
            return this.DeleteEntity(id);
        }

        public DataTable GetByCode(string code, BasePermissionItemEntity permissionItemEntity)
        {
            DataTable dT = this.GetDT(BasePermissionItemEntity.FieldCode, code);
            permissionItemEntity.GetFrom(dT);
            return dT;
        }

        public DataTable GetDTByCode(string code)
        {
            return this.GetDT(BasePermissionItemEntity.FieldCode, code);
        }

        public DataTable GetDTByUser(string userId, string permissionItemCode)
        {
            DataTable table = new DataTable(base.CurrentTableName);
            string[] names = null;
            object[] values = null;
            BaseUserRoleManager manager = new BaseUserRoleManager(base.DbHelper, base.UserInfo);
            if (manager.UserInRole(userId, "UserAdmin"))
            {
                names = new string[] { BasePermissionItemEntity.FieldCategoryCode, BasePermissionItemEntity.FieldDeletionStateCode, BasePermissionItemEntity.FieldEnabled };
                values = new object[] { "System", 0, 1 };
                table = this.GetDT(names, values, BasePermissionItemEntity.FieldSortCode);
                table.TableName = base.CurrentTableName;
                return table;
            }
            if (manager.UserInRole(userId, "Admin"))
            {
                names = new string[] { BasePermissionItemEntity.FieldCategoryCode, BasePermissionItemEntity.FieldDeletionStateCode, BasePermissionItemEntity.FieldEnabled };
                values = new object[] { "Application", 0, 1 };
                table = this.GetDT(names, values, BasePermissionItemEntity.FieldSortCode);
                table.TableName = base.CurrentTableName;
                return table;
            }
            string[] strArray2 = new BasePermissionScopeManager(base.DbHelper, base.UserInfo).GetTreeResourceScopeIds(userId, BasePermissionItemEntity.TableName, permissionItemCode, true);
            names = new string[] { BasePermissionItemEntity.FieldId, BasePermissionItemEntity.FieldDeletionStateCode, BasePermissionItemEntity.FieldEnabled };
            values = new object[] { strArray2, 0, 1 };
            table = this.GetDT(names, values, BasePermissionItemEntity.FieldSortCode);
            table.TableName = base.CurrentTableName;
            return table;
        }

        public BasePermissionItemEntity GetEntity(int id)
        {
            return this.GetEntity(id.ToString());
        }

        public BasePermissionItemEntity GetEntity(string id)
        {
            return new BasePermissionItemEntity(this.GetDT(BasePermissionItemEntity.FieldId, id));
        }

        //public string GetIdByAdd(string permissionItemCode, string permissionItemName = null) //C# 4.0 才支持缺省参数
        public string GetIdByAdd(string permissionItemCode)
        {
            string[] names = new string[] { BasePermissionItemEntity.FieldDeletionStateCode, BasePermissionItemEntity.FieldEnabled, BasePermissionItemEntity.FieldCode };
            object[] values = new object[] { 0, 1, permissionItemCode };
            BasePermissionItemEntity entity = new BasePermissionItemEntity();
            entity = new BasePermissionItemEntity(this.GetDT(names, values, BasePermissionItemEntity.FieldId));
            string str = string.Empty;
            if (entity != null)
            {
                str = entity.Id.ToString();
            }
            return str;
        }

        public int MoveTo(string id, string parentId)
        {
            return this.SetProperty(id, BaseOrganizeEntity.FieldParentId, parentId);
        }

        private void SetEntity(SQLBuilder sqlBuilder, BasePermissionItemEntity basePermissionItemEntity)
        {
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldParentId, basePermissionItemEntity.ParentId, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldCode, basePermissionItemEntity.Code, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldFullName, basePermissionItemEntity.FullName, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldCategoryCode, basePermissionItemEntity.CategoryCode, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldIsScope, basePermissionItemEntity.IsScope, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldIsPublic, basePermissionItemEntity.IsPublic, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldAllowEdit, basePermissionItemEntity.AllowEdit, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldAllowDelete, basePermissionItemEntity.AllowDelete, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldLastCall, basePermissionItemEntity.LastCall, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldEnabled, basePermissionItemEntity.Enabled, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldDeletionStateCode, basePermissionItemEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldSortCode, basePermissionItemEntity.SortCode, null);
            sqlBuilder.SetValue(BasePermissionItemEntity.FieldDescription, basePermissionItemEntity.Description, null);
        }

        public int Update(BasePermissionItemEntity basePermissionItemEntity)
        {
            return this.UpdateEntity(basePermissionItemEntity);
        }

        public int Update(BasePermissionItemEntity permissionItemEntity, out string statusCode)
        {
            int num = 0;
            if (this.Exists(BasePermissionItemEntity.FieldDeletionStateCode, 0, BasePermissionItemEntity.FieldCode, permissionItemEntity.Code, permissionItemEntity.Id))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return num;
            }
            num = this.UpdateEntity(permissionItemEntity);
            if (num == 1)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BasePermissionItemEntity basePermissionItemEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BasePermissionItemEntity.TableName);
            this.SetEntity(sqlBuilder, basePermissionItemEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BasePermissionItemEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BasePermissionItemEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BasePermissionItemEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BasePermissionItemEntity.FieldId, basePermissionItemEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

