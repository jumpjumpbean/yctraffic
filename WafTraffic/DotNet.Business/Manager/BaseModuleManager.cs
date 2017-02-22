namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseModuleManager : BaseManager, IBaseManager
    {
        public BaseModuleManager()
        {
            base.CurrentTableName = BaseModuleEntity.TableName;
            base.PrimaryKey = "Id";
        }

        public BaseModuleManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseModuleManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseModuleManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseModuleManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseModuleManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseModuleEntity baseModuleEntity)
        {
            return this.AddEntity(baseModuleEntity);
        }

        public string Add(string fullName)
        {
            string statusCode = string.Empty;
            BaseModuleEntity moduleEntity = new BaseModuleEntity {
                FullName = fullName
            };
            return this.Add(moduleEntity, out statusCode);
        }

        public string Add(BaseModuleEntity moduleEntity, out string statusCode)
        {
            string str = string.Empty;
            string[] names = new string[] { BaseModuleEntity.FieldDeletionStateCode, BaseModuleEntity.FieldCode, BaseModuleEntity.FieldFullName };
            object[] values = new object[] { 0, moduleEntity.Code, moduleEntity.FullName };
            if (this.Exists(names, values))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return str;
            }
            str = this.AddEntity(moduleEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseModuleEntity baseModuleEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseModuleEntity);
        }

        public string AddEntity(BaseModuleEntity baseModuleEntity)
        {
            string s = string.Empty;
            if (!baseModuleEntity.SortCode.HasValue || (baseModuleEntity.SortCode == 0))
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseModuleEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseModuleEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseModuleEntity.FieldId, baseModuleEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseModuleEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseModuleEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!baseModuleEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseModuleEntity.Id = new int?(int.Parse(s));
                }
                sqlBuilder.SetValue(BaseModuleEntity.FieldId, baseModuleEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseModuleEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseModuleEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseModuleEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseModuleEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseModuleEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseModuleEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseModuleEntity.FieldModifiedOn);
            if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) || (base.DbHelper.CurrentDbType == CurrentDbType.Access)))
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public override int BatchSave(DataTable dataTable)
        {
            int num = 0;
            BaseModuleEntity baseModuleEntity = new BaseModuleEntity();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BaseModuleEntity.FieldId, DataRowVersion.Original].ToString();
                    if ((id.Length > 0) && row[BaseModuleEntity.FieldAllowDelete, DataRowVersion.Original].ToString().Equals("1"))
                    {
                        num += this.DeleteEntity(id);
                    }
                }
                if ((row.RowState == DataRowState.Modified) && (row[BaseModuleEntity.FieldId, DataRowVersion.Original].ToString().Length > 0))
                {
                    baseModuleEntity.GetFrom(row);
                    if (baseModuleEntity.AllowEdit == 1)
                    {
                        num += this.UpdateEntity(baseModuleEntity);
                    }
                }
                if (row.RowState == DataRowState.Added)
                {
                    baseModuleEntity.GetFrom(row);
                    num += (this.AddEntity(baseModuleEntity).Length > 0) ? 1 : 0;
                }
                if (row.RowState != DataRowState.Unchanged)
                {
                    DataRowState rowState = row.RowState;
                }
            }
            base.ReturnStatusCode = StatusCode.OK.ToString();
            return num;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseModuleEntity.FieldId, id);
        }

        public DataTable GetDTByPermission(string userId, string permissionItemScopeCode)
        {
            DataTable table = new DataTable(base.CurrentTableName);
            string[] names = null;
            object[] values = null;
            BaseUserRoleManager manager = new BaseUserRoleManager(base.DbHelper, base.UserInfo);
            if (manager.UserInRole(userId, "UserAdmin"))
            {
                names = new string[] { BaseModuleEntity.FieldCategory, BaseModuleEntity.FieldDeletionStateCode, BaseModuleEntity.FieldEnabled };
                values = new object[] { "System", 0, 1 };
                table = this.GetDT(names, values, BaseModuleEntity.FieldSortCode);
                table.TableName = base.CurrentTableName;
                return table;
            }
            if (manager.UserInRole(userId, "Admin"))
            {
                names = new string[] { BaseModuleEntity.FieldCategory, BaseModuleEntity.FieldDeletionStateCode, BaseModuleEntity.FieldEnabled };
                values = new object[] { "Application", 0, 1 };
                table = this.GetDT(names, values, BaseModuleEntity.FieldSortCode);
                table.TableName = base.CurrentTableName;
                return table;
            }
            string[] strArray2 = new BasePermissionScopeManager(base.DbHelper, base.UserInfo).GetTreeResourceScopeIds(userId, BaseModuleEntity.TableName, permissionItemScopeCode, true);
            names = new string[] { BaseModuleEntity.FieldId, BaseModuleEntity.FieldDeletionStateCode, BaseModuleEntity.FieldEnabled };
            values = new object[] { strArray2, 0, 1 };
            table = this.GetDT(names, values, BaseModuleEntity.FieldSortCode);
            table.TableName = base.CurrentTableName;
            return table;
        }

        public DataTable GetDTByUser(string userId)
        {
            string[] iDsByUser = this.GetIDsByUser(userId);
            return this.GetDT(BaseModuleEntity.FieldId, iDsByUser, BaseModuleEntity.FieldDeletionStateCode, 0, BaseModuleEntity.FieldSortCode);
        }

        public BaseModuleEntity GetEntity(int id)
        {
            return this.GetEntity(id.ToString());
        }

        public BaseModuleEntity GetEntity(string id)
        {
            return new BaseModuleEntity(this.GetDT(BaseModuleEntity.FieldId, id));
        }

        public string[] GetIDsByUser(string userId)
        {
            string[] strArray = new string[0];
            string[] names = new string[] { BaseModuleEntity.FieldIsPublic, BaseModuleEntity.FieldEnabled, BaseModuleEntity.FieldDeletionStateCode };
            object[] values = new object[] { 1, 1, 0 };
            strArray = this.GetIds(names, values, BaseModuleEntity.FieldId);
            string[] strArray3 = null;
            if (!string.IsNullOrEmpty(userId))
            {
                BasePermissionScopeManager manager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
                string permissionItemCode = "Resource.AccessPermission";
                strArray3 = manager.GetResourceScopeIds(userId, BaseModuleEntity.TableName, permissionItemCode);
            }
            return BaseBusinessLogic.Concat(new string[][] { strArray, strArray3 });
        }

        public DataTable GetRootList()
        {
            return this.GetDTByParent(string.Empty);
        }

        public int ResetSortCode(string parentId)
        {
            int num = 0;
            DataTable dTByParent = this.GetDTByParent(parentId);
            string id = string.Empty;
            string[] batchSequence = new BaseSequenceManager(base.DbHelper).GetBatchSequence(BaseModuleEntity.TableName, dTByParent.Rows.Count);
            int index = 0;
            foreach (DataRow row in dTByParent.Rows)
            {
                id = row[BaseModuleEntity.FieldId].ToString();
                num += this.SetProperty(id, BaseModuleEntity.FieldSortCode, batchSequence[index]);
                index++;
            }
            return num;
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseModuleEntity baseModuleEntity)
        {
            sqlBuilder.SetValue(BaseModuleEntity.FieldParentId, baseModuleEntity.ParentId, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldCode, baseModuleEntity.Code, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldFullName, baseModuleEntity.FullName, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldCategory, baseModuleEntity.Category, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldImageIndex, baseModuleEntity.ImageIndex, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldSelectedImageIndex, baseModuleEntity.SelectedImageIndex, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldNavigateUrl, baseModuleEntity.NavigateUrl, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldTarget, baseModuleEntity.Target, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldIsPublic, baseModuleEntity.IsPublic, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldIsMenu, baseModuleEntity.IsMenu, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldExpand, baseModuleEntity.Expand, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldPermissionItemCode, baseModuleEntity.PermissionItemCode, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldPermissionScopeTables, baseModuleEntity.PermissionScopeTables, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldAllowEdit, baseModuleEntity.AllowEdit, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldAllowDelete, baseModuleEntity.AllowDelete, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldSortCode, baseModuleEntity.SortCode, null);
            if (baseModuleEntity.DeletionStateCode == null) baseModuleEntity.DeletionStateCode = 0;
            sqlBuilder.SetValue(BaseModuleEntity.FieldDeletionStateCode, baseModuleEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldEnabled, baseModuleEntity.Enabled, null);
            sqlBuilder.SetValue(BaseModuleEntity.FieldDescription, baseModuleEntity.Description, null);
        }

        public int Update(BaseModuleEntity baseModuleEntity)
        {
            return this.UpdateEntity(baseModuleEntity);
        }

        public int Update(BaseModuleEntity moduleEntity, out string statusCode)
        {
            int num = 0;
            string[] names = new string[] { BaseModuleEntity.FieldDeletionStateCode, BaseModuleEntity.FieldCode, BaseModuleEntity.FieldFullName };
            object[] values = new object[] { 0, moduleEntity.Code, moduleEntity.FullName };
            if ((moduleEntity.Code.Length > 0) && this.Exists(names, values, moduleEntity.Id))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return num;
            }
            num = this.UpdateEntity(moduleEntity);
            if (num == 1)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseModuleEntity baseModuleEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseModuleEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseModuleEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseModuleEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseModuleEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseModuleEntity.FieldId, baseModuleEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

