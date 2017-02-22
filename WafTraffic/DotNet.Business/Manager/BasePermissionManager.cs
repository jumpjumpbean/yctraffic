namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BasePermissionManager : BaseManager, IBaseManager
    {
        public BasePermissionManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BasePermissionEntity.TableName;
        }

        public BasePermissionManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BasePermissionManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BasePermissionManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BasePermissionManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BasePermissionManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BasePermissionEntity resourcePermissionEntity)
        {
            return this.AddEntity(resourcePermissionEntity);
        }

        public string Add(BasePermissionEntity resourcePermissionEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(resourcePermissionEntity);
        }

        public string AddEntity(BasePermissionEntity resourcePermissionEntity)
        {
            string sequence = string.Empty;
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(BasePermissionEntity.TableName, BasePermissionEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BasePermissionEntity.FieldId, resourcePermissionEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BasePermissionEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BasePermissionEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!resourcePermissionEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(sequence))
                    {
                        sequence = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    resourcePermissionEntity.Id = new int?(int.Parse(sequence));
                }
                sqlBuilder.SetValue(BasePermissionEntity.FieldId, resourcePermissionEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, resourcePermissionEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BasePermissionEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BasePermissionEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BasePermissionEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BasePermissionEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BasePermissionEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BasePermissionEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return sequence;
        }

        public string AddPermission(BasePermissionEntity resourcePermissionEntity)
        {
            string str = string.Empty;
            if (!this.PermissionExists(resourcePermissionEntity.PermissionId.ToString(), resourcePermissionEntity.ResourceCategory, resourcePermissionEntity.ResourceId))
            {
                str = this.AddEntity(resourcePermissionEntity);
            }
            return str;
        }

        public bool CheckPermissionByRole(string roleId, string permissionItemCode)
        {
            string str = new BasePermissionItemManager(base.DbHelper, base.UserInfo).GetProperty(BasePermissionItemEntity.FieldCode, permissionItemCode, BasePermissionItemEntity.FieldId);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            string commandText = " SELECT COUNT(*)    FROM " + BasePermissionEntity.TableName + "  WHERE (" + BasePermissionEntity.FieldResourceCategory + " = '" + BaseRoleEntity.TableName + "')         AND (" + BasePermissionEntity.FieldEnabled + " = 1)         AND (" + BasePermissionEntity.FieldResourceId + " = '" + roleId + "' )         AND (" + BasePermissionEntity.FieldPermissionItemId + " = '" + str + "') ";
            int num = 0;
            object obj2 = base.DbHelper.ExecuteScalar(commandText);
            if (obj2 != null)
            {
                num = int.Parse(obj2.ToString());
            }
            return (num > 0);
        }

        //public bool CheckPermissionByUser(string userId, string permissionItemCode, string permissionItemName = null) //C# 4.0 才支持缺省参数
        public bool CheckPermissionByUser(string userId, string permissionItemCode) 
        {
            BasePermissionItemManager manager = new BasePermissionItemManager(base.DbHelper, base.UserInfo);
            //string idByAdd = manager.GetIdByAdd(permissionItemCode, permissionItemName);
            string idByAdd = manager.GetIdByAdd(permissionItemCode);
            BasePermissionItemEntity entity = manager.GetEntity(idByAdd);
            if (base.UserInfo.IsAdministrator)
            {
                return true;
            }
            if (string.IsNullOrEmpty(idByAdd))
            {
                return false;
            }
            bool flag = false;
            BaseUserRoleManager manager2 = new BaseUserRoleManager(base.DbHelper, base.UserInfo);
            if (!string.IsNullOrEmpty(entity.CategoryCode) && entity.CategoryCode.Equals("System"))
            {
                flag = manager2.UserInRole(userId, "UserAdmin");
                if (flag)
                {
                    return flag;
                }
            }
            if (!string.IsNullOrEmpty(entity.CategoryCode) && entity.CategoryCode.Equals("Application"))
            {
                flag = manager2.UserInRole(userId, "Admin");
                if (flag)
                {
                    return flag;
                }
            }
            return (this.CheckUserPermission(userId, idByAdd) || this.CheckUserRolePermission(userId, idByAdd));
        }

        private bool CheckResourcePermission(string resourceCategory, string resourceId, string permissionItemId)
        {
            string commandText = " SELECT COUNT(1)    FROM " + BasePermissionEntity.TableName + "  WHERE (" + BasePermissionEntity.FieldResourceCategory + " = '" + resourceCategory + "')         AND (" + BasePermissionEntity.FieldEnabled + " = 1)         AND (" + BasePermissionEntity.FieldDeletionStateCode + " = 0)         AND (" + BasePermissionEntity.FieldResourceId + " = '" + resourceId + "')         AND (" + BasePermissionEntity.FieldPermissionItemId + " = '" + permissionItemId + "')";
            int num = 0;
            object obj2 = base.DbHelper.ExecuteScalar(commandText);
            if (obj2 != null)
            {
                num = int.Parse(obj2.ToString());
            }
            return (num > 0);
        }

        private bool CheckUserPermission(string userId, string permissionItemId)
        {
            return this.CheckResourcePermission(BaseUserEntity.TableName, userId, permissionItemId);
        }

        private bool CheckUserRolePermission(string userId, string permissionItemId)
        {
            string commandText = " SELECT COUNT(1)    FROM " + BasePermissionEntity.TableName + "  WHERE (" + BasePermissionEntity.FieldResourceCategory + " = '" + BaseRoleEntity.TableName + "')         AND (" + BasePermissionEntity.FieldEnabled + " = 1         AND  " + BasePermissionEntity.FieldDeletionStateCode + " = 0)         AND (" + BasePermissionEntity.FieldResourceId + " IN (  SELECT " + BaseUserRoleEntity.FieldRoleId + "   FROM " + BaseUserRoleEntity.TableName + "  WHERE " + BaseUserRoleEntity.FieldUserId + " = '" + userId + "'         AND " + BaseUserRoleEntity.FieldEnabled + " = 1         AND " + BaseUserRoleEntity.FieldDeletionStateCode + " = 0   UNION ALL  SELECT " + BaseUserEntity.FieldRoleId + "   FROM " + BaseUserEntity.TableName + "  WHERE " + BaseUserEntity.FieldId + " = '" + userId + "') )         AND (" + BasePermissionEntity.FieldPermissionItemId + " = '" + permissionItemId + "') ";
            int num = 0;
            object obj2 = base.DbHelper.ExecuteScalar(commandText);
            if (obj2 != null)
            {
                num = int.Parse(obj2.ToString());
            }
            return (num > 0);
        }

        public int Delete(int id)
        {
            return this.Delete(BasePermissionEntity.FieldId, id);
        }

        public BasePermissionEntity GetEntity(int id)
        {
            return new BasePermissionEntity(this.GetDT(BasePermissionEntity.FieldId, id));
        }

        public DataTable GetPermission(BaseUserInfo userInfo)
        {
            BasePermissionItemManager manager = new BasePermissionItemManager(base.DbHelper, userInfo);
            if (userInfo.IsAdministrator)
            {
                return manager.GetDT(BasePermissionItemEntity.FieldEnabled, "1", BasePermissionItemEntity.FieldSortCode);
            }
            return this.GetPermissionByUser(userInfo.Id);
        }

        public DataTable GetPermissionByUser(string userId)
        {
            string commandText = " SELECT *    FROM " + BasePermissionItemEntity.TableName + "  WHERE " + BasePermissionItemEntity.FieldEnabled + " = 1         AND " + BasePermissionItemEntity.FieldId + " IN (  SELECT " + BasePermissionEntity.FieldPermissionItemId + "   FROM " + BasePermissionEntity.TableName + "  WHERE (" + BasePermissionEntity.FieldResourceCategory + " = '" + BaseUserEntity.TableName + "')         AND (" + BasePermissionEntity.FieldEnabled + " = 1)         AND (" + BasePermissionEntity.FieldResourceId + " = '" + userId + "') UNION  SELECT " + BasePermissionEntity.FieldPermissionItemId + "   FROM " + BasePermissionEntity.TableName + "  WHERE (" + BasePermissionEntity.FieldResourceCategory + " = '" + BaseRoleEntity.TableName + "')         AND (" + BasePermissionEntity.FieldEnabled + " = 1)         AND (" + BasePermissionEntity.FieldResourceId + " IN (  SELECT " + BaseUserRoleEntity.FieldRoleId + "   FROM " + BaseUserRoleEntity.TableName + "  WHERE " + BaseUserRoleEntity.FieldUserId + " = '" + userId + "'         AND " + BaseUserRoleEntity.FieldEnabled + " = 1  UNION  SELECT " + BaseUserEntity.FieldRoleId + "   FROM " + BaseUserEntity.TableName + "  WHERE " + BaseUserEntity.FieldId + " = '" + userId + "')) )  ORDER BY " + BasePermissionItemEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public bool PermissionExists(string permissionItemId, string resourceCategory, string resourceId)
        {
            bool flag = true;
            string[] names = new string[3];
            string[] values = new string[3];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = resourceCategory;
            names[1] = BasePermissionEntity.FieldResourceId;
            values[1] = resourceId;
            names[2] = BasePermissionEntity.FieldPermissionItemId;
            values[2] = permissionItemId;
            if (!this.Exists(names, values))
            {
                flag = false;
            }
            return flag;
        }

        private void SetEntity(SQLBuilder sqlBuilder, BasePermissionEntity resourcePermissionEntity)
        {
            sqlBuilder.SetValue(BasePermissionEntity.FieldResourceId, resourcePermissionEntity.ResourceId, null);
            sqlBuilder.SetValue(BasePermissionEntity.FieldResourceCategory, resourcePermissionEntity.ResourceCategory, null);
            sqlBuilder.SetValue(BasePermissionEntity.FieldPermissionItemId, resourcePermissionEntity.PermissionId, null);
            sqlBuilder.SetValue(BasePermissionEntity.FieldPermissionConstraint, resourcePermissionEntity.PermissionConstraint, null);
            sqlBuilder.SetValue(BasePermissionEntity.FieldEnabled, resourcePermissionEntity.Enabled, null);
            sqlBuilder.SetValue(BasePermissionEntity.FieldDeletionStateCode, resourcePermissionEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BasePermissionEntity.FieldDescription, resourcePermissionEntity.Description, null);
        }

        public int Update(BasePermissionEntity resourcePermissionEntity)
        {
            return this.UpdateEntity(resourcePermissionEntity);
        }

        public int UpdateEntity(BasePermissionEntity resourcePermissionEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BasePermissionEntity.TableName);
            this.SetEntity(sqlBuilder, resourcePermissionEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BasePermissionEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BasePermissionEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BasePermissionEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BasePermissionEntity.FieldId, resourcePermissionEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

