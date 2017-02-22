namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseTableColumnsManager : BaseManager, IBaseManager
    {
        public BaseTableColumnsManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseTableColumnsEntity.TableName;
        }

        public BaseTableColumnsManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseTableColumnsManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseTableColumnsManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseTableColumnsManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseTableColumnsManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseTableColumnsEntity baseTableColumnsEntity)
        {
            return this.AddEntity(baseTableColumnsEntity);
        }

        public string Add(BaseTableColumnsEntity baseTableColumnsEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseTableColumnsEntity);
        }

        public string AddEntity(BaseTableColumnsEntity baseTableColumnsEntity)
        {
            string s = string.Empty;
            if (baseTableColumnsEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseTableColumnsEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(BaseTableColumnsEntity.TableName, BaseTableColumnsEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseTableColumnsEntity.FieldId, baseTableColumnsEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseTableColumnsEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseTableColumnsEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!baseTableColumnsEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseTableColumnsEntity.Id = new int?(int.Parse(s));
                }
                sqlBuilder.SetValue(BaseTableColumnsEntity.FieldId, baseTableColumnsEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseTableColumnsEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseTableColumnsEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseTableColumnsEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseTableColumnsEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseTableColumnsEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseTableColumnsEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseTableColumnsEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseTableColumnsEntity.FieldId, id);
        }

        //public string[] GetColumns(string tableCode, string permissionCode = "Column.Access") //C# 4.0 才支持缺省参数
        public string[] GetColumns(string tableCode, string permissionCode) 
        {
            return this.GetColumns(base.UserInfo.Id, tableCode, permissionCode);
        }

        public string[] GetColumns(string tableCode) 
        {
            string permissionCode = "Column.Access";
            return this.GetColumns(base.UserInfo.Id, tableCode, permissionCode);
        }

        //public string[] GetColumns(string userId, string tableCode, string permissionCode = "Column.Access") //C# 4.0 才支持缺省参数
        public string[] GetColumns(string userId, string tableCode, string permissionCode) 
        {
            string[] strArray = null;
            if (permissionCode.Equals("Column.Deney") || permissionCode.Equals("Column.Edit"))
            {
                BasePermissionScopeManager manager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
                return manager.GetResourceScopeIds(userId, tableCode, permissionCode);
            }
            if (permissionCode.Equals("Column.Access"))
            {
                strArray = new BasePermissionScopeManager(base.DbHelper, base.UserInfo).GetResourceScopeIds(userId, tableCode, permissionCode);
                string[] strArray2 = this.GetIds(BaseTableColumnsEntity.FieldTableCode, tableCode, BaseTableColumnsEntity.FieldIsPublic, 1, BaseTableColumnsEntity.FieldColumnCode);
                strArray = BaseBusinessLogic.Concat(new string[][] { strArray, strArray2 });
            }
            return strArray;
        }

        //public string GetConstraint(string resourceCategory, string resourceId, string tableName, string permissionCode = "Resource.AccessPermission") //C# 4.0 才支持缺省参数
        public string GetConstraint(string resourceCategory, string resourceId, string tableName, string permissionCode)
        {
            string permissionConstraint = string.Empty;
            BasePermissionScopeEntity entity = this.GetConstraintEntity(resourceCategory, resourceId, tableName, permissionCode);
            if ((entity != null) && (entity.Enabled == 1))
            {
                permissionConstraint = entity.PermissionConstraint;
            }
            return permissionConstraint;
        }

        public string GetConstraint(string resourceCategory, string resourceId, string tableName)
        {
            string permissionCode = "Resource.AccessPermission";
            return GetConstraint(resourceCategory, resourceId, permissionCode);
        }

        //public DataTable GetConstraintDT(string resourceCategory, string resourceId, string permissionCode = "Resource.AccessPermission") //C# 4.0 才支持缺省参数
        public DataTable GetConstraintDT(string resourceCategory, string resourceId, string permissionCode) 
        {
            DataTable table = new DataTable(BaseTableColumnsEntity.TableName);
            string idByAdd = string.Empty;
            idByAdd = new BasePermissionItemManager(base.UserInfo).GetIdByAdd(permissionCode);
            string commandText = " SELECT Base_PermissionScope.Id\r\n\t\t                                    , Items_TablePermissionScope.ItemValue AS TableCode\r\n\t\t                                    , Items_TablePermissionScope.ItemName AS TableName\r\n\t\t                                    , Base_PermissionScope.PermissionConstraint\r\n\t\t                                    , Items_TablePermissionScope.SortCode\r\n                                    FROM  (\r\n\t                                    SELECT ItemValue\r\n\t\t                                     , ItemName\r\n\t\t                                     , SortCode\r\n\t                                    FROM Items_TablePermissionScope\r\n                                       WHERE (DeletionStateCode = 0) \r\n\t\t                                      AND (Enabled = 1)                                              \r\n                                        ) AS Items_TablePermissionScope LEFT OUTER JOIN\r\n                                        (SELECT Id\r\n\t\t\t                                    , TargetId\r\n\t\t\t                                    , PermissionConstraint  \r\n                                           FROM Base_PermissionScope\r\n                                         WHERE (ResourceCategory = '" + resourceCategory + "') \r\n\t\t\t                                    AND (ResourceId = " + resourceId + ") \r\n\t\t\t                                    AND (TargetCategory = 'Table') \r\n\t\t\t                                    AND (PermissionId = " + idByAdd.ToString() + ") \r\n\t\t\t                                    AND (DeletionStateCode = 0) \r\n\t\t\t                                    AND (Enabled = 1)\r\n\t                                     ) AS Base_PermissionScope \r\n                                    ON Items_TablePermissionScope.ItemValue = Base_PermissionScope.TargetId\r\n                                    ORDER BY Items_TablePermissionScope.SortCode ";
            table = this.Fill(commandText);
            table.TableName = BaseTableColumnsEntity.TableName;
            return table;
        }
        public DataTable GetConstraintDT(string resourceCategory, string resourceId) 
        {
            string permissionCode = "Resource.AccessPermission";
            return GetConstraintDT(resourceCategory, resourceId, permissionCode);
        }

        //public BasePermissionScopeEntity GetConstraintEntity(string resourceCategory, string resourceId, string tableName, string permissionCode = "Resource.AccessPermission") //C# 4.0 才支持缺省参数
        public BasePermissionScopeEntity GetConstraintEntity(string resourceCategory, string resourceId, string tableName, string permissionCode)
        {
            BasePermissionScopeEntity entity = null;
            string idByAdd = string.Empty;
            idByAdd = new BasePermissionItemManager(base.UserInfo).GetIdByAdd(permissionCode);
            BasePermissionScopeManager manager2 = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            string[] names = new string[] { BasePermissionScopeEntity.FieldResourceCategory, BasePermissionScopeEntity.FieldResourceId, BasePermissionScopeEntity.FieldTargetCategory, BasePermissionScopeEntity.FieldTargetId, BasePermissionScopeEntity.FieldPermissionItemId, BasePermissionScopeEntity.FieldDeletionStateCode };
            object[] values = new object[] { resourceCategory, resourceId, "Table", tableName, idByAdd, 0 };
            DataTable dT = manager2.GetDT(names, values);
            if (dT.Rows.Count > 0)
            {
                entity = new BasePermissionScopeEntity(dT);
            }
            return entity;
        }

        public BasePermissionScopeEntity GetConstraintEntity(string resourceCategory, string resourceId, string tableName)
        {
            string permissionCode = "Resource.AccessPermission";
            return GetConstraintEntity(resourceCategory, resourceId, tableName, permissionCode);
        }

        public BaseTableColumnsEntity GetEntity(int id)
        {
            return new BaseTableColumnsEntity(this.GetDT(BaseTableColumnsEntity.FieldId, id));
        }

        public DataTable GetTableColumns(string userId, string tableCode)
        {
            string[] ids = new BasePermissionScopeManager(base.DbHelper, base.UserInfo).GetResourceScopeIds(userId, "TableColumns", "ColumnAccess");
            string commandText = " SELECT * FROM Base_TableColumns WHERE (DeletionStateCode = 0 AND Enabled = 1) ";
            if (!string.IsNullOrEmpty(tableCode))
            {
                commandText = commandText + " AND (TableCode = '" + tableCode + "') ";
            }
            commandText = commandText + " AND (IsPublic = 1 ";
            if ((ids != null) && (ids.Length > 0))
            {
                string str2 = BaseBusinessLogic.ArrayToList(ids);
                commandText = commandText + " OR Id IN (" + str2 + ")";
            }
            commandText = commandText + ") ORDER BY SortCode ";
            return base.DbHelper.Fill(commandText);
        }

        //public string GetUserConstraint(string tableName, string permissionCode = "Resource.AccessPermission") //C# 4.0 才支持缺省参数
        public string GetUserConstraint(string tableName, string permissionCode) 
        {
            string str = string.Empty;
            string idByAdd = string.Empty;
            idByAdd = new BasePermissionItemManager(base.UserInfo).GetIdByAdd(permissionCode);
            string[] allRoleIds = new BaseUserRoleManager(base.DbHelper, base.UserInfo).GetAllRoleIds(base.UserInfo.Id);
            if ((allRoleIds != null) && (allRoleIds.Length != 0))
            {
                BasePermissionScopeManager manager3 = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
                string[] names = new string[] { BasePermissionScopeEntity.FieldResourceCategory, BasePermissionScopeEntity.FieldResourceId, BasePermissionScopeEntity.FieldTargetCategory, BasePermissionScopeEntity.FieldTargetId, BasePermissionScopeEntity.FieldPermissionItemId, BasePermissionScopeEntity.FieldEnabled, BasePermissionScopeEntity.FieldDeletionStateCode };
                object[] values = new object[] { BaseRoleEntity.TableName, allRoleIds, "Table", tableName, idByAdd, 1, 0 };
                DataTable dT = manager3.GetDT(names, values);
                string str3 = string.Empty;
                foreach (DataRow row in dT.Rows)
                {
                    str3 = row[BasePermissionScopeEntity.FieldPermissionConstraint].ToString().Trim();
                    if (!string.IsNullOrEmpty(str3))
                    {
                        str = str + " AND " + str3;
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Substring(5);
                    str = ConstraintUtil.PrepareParameter(base.UserInfo, str);
                }
            }
            return str;
        }
        public string GetUserConstraint(string tableName) 
        {
            string permissionCode = "Resource.AccessPermission";
            return GetUserConstraint(tableName, permissionCode);

        }

        //public string SetConstraint(string resourceCategory, string resourceId, string tableName, string permissionCode, string constraint, bool enabled = true) //C# 4.0 才支持缺省参数
        public string SetConstraint(string resourceCategory, string resourceId, string tableName, string permissionCode, string constraint, bool enabled) 
        {
            string id = string.Empty;
            string s = string.Empty;
            s = new BasePermissionItemManager(base.UserInfo).GetIdByAdd(permissionCode);
            BasePermissionScopeManager manager2 = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            string[] names = new string[] { BasePermissionScopeEntity.FieldResourceCategory, BasePermissionScopeEntity.FieldResourceId, BasePermissionScopeEntity.FieldTargetCategory, BasePermissionScopeEntity.FieldTargetId, BasePermissionScopeEntity.FieldPermissionItemId, BasePermissionScopeEntity.FieldDeletionStateCode };
            object[] values = new object[] { resourceCategory, resourceId, "Table", tableName, s, 0 };
            id = manager2.GetId(names, values);
            if (!string.IsNullOrEmpty(id))
            {
                string[] targetFields = new string[] { BasePermissionScopeEntity.FieldPermissionConstraint, BasePermissionScopeEntity.FieldEnabled };
                object[] targetValues = new object[] { constraint, enabled ? 1 : 0 };
                manager2.SetProperty(BasePermissionScopeEntity.FieldId, id, targetFields, targetValues);
                return id;
            }
            BasePermissionScopeEntity baseResourcePermissionScopeEntity = new BasePermissionScopeEntity {
                ResourceCategory = resourceCategory,
                ResourceId = resourceId,
                TargetCategory = "Table",
                TargetId = tableName,
                PermissionConstraint = constraint,
                PermissionId = new int?(int.Parse(s)),
                DeletionStateCode = 0,
                Enabled = new int?(enabled ? 1 : 0)
            };
            return manager2.Add(baseResourcePermissionScopeEntity);
        }

        public string SetConstraint(string resourceCategory, string resourceId, string tableName, string permissionCode, string constraint) 
        {
            return SetConstraint(resourceCategory, resourceId, tableName, permissionCode, constraint, true);
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseTableColumnsEntity baseTableColumnsEntity)
        {
            sqlBuilder.SetValue(BaseTableColumnsEntity.FieldTableCode, baseTableColumnsEntity.TableCode, null);
            sqlBuilder.SetValue(BaseTableColumnsEntity.FieldColumnCode, baseTableColumnsEntity.ColumnCode, null);
            sqlBuilder.SetValue(BaseTableColumnsEntity.FieldColumnName, baseTableColumnsEntity.ColumnName, null);
            sqlBuilder.SetValue(BaseTableColumnsEntity.FieldIsPublic, baseTableColumnsEntity.IsPublic, null);
            sqlBuilder.SetValue(BaseTableColumnsEntity.FieldEnabled, baseTableColumnsEntity.Enabled, null);
            sqlBuilder.SetValue(BaseTableColumnsEntity.FieldAllowEdit, baseTableColumnsEntity.AllowEdit, null);
            sqlBuilder.SetValue(BaseTableColumnsEntity.FieldAllowDelete, baseTableColumnsEntity.AllowDelete, null);
            sqlBuilder.SetValue(BaseTableColumnsEntity.FieldDeletionStateCode, baseTableColumnsEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseTableColumnsEntity.FieldSortCode, baseTableColumnsEntity.SortCode, null);
            sqlBuilder.SetValue(BaseTableColumnsEntity.FieldDescription, baseTableColumnsEntity.Description, null);
        }

        public int Update(BaseTableColumnsEntity baseTableColumnsEntity)
        {
            return this.UpdateEntity(baseTableColumnsEntity);
        }

        public int UpdateEntity(BaseTableColumnsEntity baseTableColumnsEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BaseTableColumnsEntity.TableName);
            this.SetEntity(sqlBuilder, baseTableColumnsEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseTableColumnsEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseTableColumnsEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseTableColumnsEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseTableColumnsEntity.FieldId, baseTableColumnsEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

