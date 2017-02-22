//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

/// 修改纪录
/// 
///		2012.10.15 版本：2.0 sunmiao 修改GetOrganizeIds,支持取下级组织结构
///		
/// 版本：2.0
///

namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BasePermissionScopeManager : BaseManager, IBaseManager
    {
        public static bool UseGetChildrensByCode;

        public BasePermissionScopeManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BasePermissionScopeEntity.TableName;
        }

        public BasePermissionScopeManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BasePermissionScopeManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BasePermissionScopeManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BasePermissionScopeManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BasePermissionScopeManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BasePermissionScopeEntity baseResourcePermissionScopeEntity)
        {
            return this.AddEntity(baseResourcePermissionScopeEntity);
        }

        public string Add(BasePermissionScopeEntity baseResourcePermissionScopeEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseResourcePermissionScopeEntity);
        }

        public string AddEntity(BasePermissionScopeEntity baseResourcePermissionScopeEntity)
        {
            string sequence = string.Empty;
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(BasePermissionScopeEntity.TableName, BasePermissionScopeEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BasePermissionScopeEntity.FieldId, baseResourcePermissionScopeEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BasePermissionScopeEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BasePermissionScopeEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!baseResourcePermissionScopeEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(sequence))
                    {
                        sequence = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseResourcePermissionScopeEntity.Id = new int?(int.Parse(sequence));
                }
                sqlBuilder.SetValue(BasePermissionScopeEntity.FieldId, baseResourcePermissionScopeEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseResourcePermissionScopeEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BasePermissionScopeEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BasePermissionScopeEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BasePermissionScopeEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BasePermissionScopeEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BasePermissionScopeEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BasePermissionScopeEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return sequence;
        }

        public string AddPermission(BasePermissionScopeEntity resourcePermissionScope)
        {
            string str = string.Empty;
            if (!this.PermissionScopeExists(resourcePermissionScope.PermissionId.ToString(), resourcePermissionScope.ResourceCategory, resourcePermissionScope.ResourceId, resourcePermissionScope.TargetCategory, resourcePermissionScope.TargetId))
            {
                str = this.AddEntity(resourcePermissionScope);
            }
            return str;
        }

        public string AddPermission(string resourceCategory, string resourceId, string targetCategory, string targetId)
        {
            BasePermissionScopeEntity resourcePermissionScope = new BasePermissionScopeEntity {
                ResourceCategory = resourceCategory,
                ResourceId = resourceId,
                TargetCategory = targetCategory,
                TargetId = targetId,
                Enabled = 1,
                DeletionStateCode = 0
            };
            return this.AddPermission(resourcePermissionScope);
        }

        public bool CheckDateScope(string smallDate, string bigDate)
        {
            return (DateTime.Parse(smallDate) < DateTime.Parse(bigDate));
        }

        private bool CheckResourcePermissionScope(string resourceCategory, string resourceId, string targetCategory, string targetId, string permissionItemId)
        {
            string commandText = " SELECT COUNT(*)   FROM Base_PermissionScope  WHERE (Base_PermissionScope.ResourceCategory = '" + resourceCategory + "')       AND (Base_PermissionScope.Enabled = 1)        AND (Base_PermissionScope.DeletionStateCode = 0 )       AND (Base_PermissionScope.ResourceId = '" + resourceId + "')       AND (Base_PermissionScope.TargetCategory = '" + targetCategory + "')       AND (Base_PermissionScope.TargetId = '" + targetId + "')       AND (Base_PermissionScope.PermissionId = " + permissionItemId + "))";
            int num = 0;
            object obj2 = base.DbHelper.ExecuteScalar(commandText);
            if (obj2 != null)
            {
                num = int.Parse(obj2.ToString());
            }
            return (num > 0);
        }

        private bool CheckRoleModulePermission(string userId, string moduleId, string permissionItemId)
        {
            return this.CheckRolePermissionScope(userId, BaseModuleEntity.TableName, moduleId, permissionItemId);
        }

        private bool CheckRolePermissionScope(string userId, string targetCategory, string targetId, string permissionItemId)
        {
            string commandText = " SELECT COUNT(*)    FROM Base_PermissionScope   WHERE (Base_PermissionScope.ResourceCategory = '" + BaseRoleEntity.TableName + "')         AND (Base_PermissionScope.Enabled = 1         AND (Base_PermissionScope.DeletionStateCode = 0         AND (Base_PermissionScope.ResourceId IN (  SELECT Base_UserRole.RoleId    FROM Base_UserRole   WHERE Base_UserRole.UserId = '" + userId + "'        AND Base_UserRole.Enabled = 1   UNION  SELECT " + BaseUserEntity.FieldRoleId + "   FROM " + BaseUserEntity.TableName + "  WHERE " + BaseUserEntity.FieldId + " = '" + userId + "'))  AND (Base_PermissionScope.TargetCategory = '" + targetCategory + "')  AND (Base_PermissionScope.TargetId = '" + targetId + "')  AND (Base_PermissionScope.PermissionId = " + permissionItemId + ")) ";
            int num = 0;
            object obj2 = base.DbHelper.ExecuteScalar(commandText);
            if (obj2 != null)
            {
                num = int.Parse(obj2.ToString());
            }
            return (num > 0);
        }

        private bool CheckUserModulePermission(string userId, string moduleId, string permissionItemId)
        {
            return this.CheckResourcePermissionScope(BaseModuleEntity.TableName, userId, BaseModuleEntity.TableName, moduleId, permissionItemId);
        }

        public int Delete(int id)
        {
            return this.Delete(BasePermissionScopeEntity.FieldId, id);
        }

        public DataTable GetAuthoriedList(string resourceCategory, string permissionItemId, string targetCategory, string targetId)
        {
            string commandText = string.Empty;
            commandText = "SELECT * FROM " + base.CurrentTableName + " WHERE " + BasePermissionScopeEntity.FieldDeletionStateCode + " =0  AND " + BasePermissionScopeEntity.FieldEnabled + " =1 ";
            if (!string.IsNullOrEmpty(resourceCategory))
            {
                string str2 = commandText;
                commandText = str2 + " AND " + BasePermissionScopeEntity.FieldResourceCategory + " = '" + resourceCategory + "'";
            }
            if (!string.IsNullOrEmpty(permissionItemId))
            {
                string str3 = commandText;
                commandText = str3 + " AND " + BasePermissionScopeEntity.FieldPermissionItemId + " = '" + permissionItemId + "'";
            }
            if (!string.IsNullOrEmpty(targetCategory))
            {
                string str4 = commandText;
                commandText = str4 + " AND " + BasePermissionScopeEntity.FieldTargetCategory + " = '" + targetCategory + "'";
            }
            if (!string.IsNullOrEmpty(targetId))
            {
                string str5 = commandText;
                commandText = str5 + " AND " + BasePermissionScopeEntity.FieldTargetId + " = '" + targetId + "'";
            }
            if (BaseSystemInfo.UserCenterDbType.Equals(CurrentDbType.SqlServer))
            {
                string str6 = commandText;
                string str7 = str6 + " AND ((SELECT DATEDIFF(day, " + BasePermissionScopeEntity.FieldStartDate + ", GETDATE()))>=0 OR (SELECT DATEDIFF(day, " + BasePermissionScopeEntity.FieldStartDate + ", GETDATE())) IS NULL)";
                commandText = str7 + " AND ((SELECT DATEDIFF(day, " + BasePermissionScopeEntity.FieldEndDate + ", GETDATE()))<=0 OR (SELECT DATEDIFF(day, " + BasePermissionScopeEntity.FieldEndDate + ", GETDATE())) IS NULL)";
            }
            commandText = commandText + " ORDER BY " + BasePermissionScopeEntity.FieldCreateOn + " DESC ";
            return base.DbHelper.Fill(commandText);
        }

        public BasePermissionScopeEntity GetEntity(int id)
        {
            return new BasePermissionScopeEntity(this.GetDT(BasePermissionScopeEntity.FieldId, id));
        }

        public DataTable GetOrganizeDT(string managerUserId, string permissionItemCode)
        {
            string organizeIdsSqlByCode = string.Empty;
            if (UseGetChildrensByCode)
            {
                organizeIdsSqlByCode = this.GetOrganizeIdsSqlByCode(managerUserId, permissionItemCode);
            }
            else if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
            {
                organizeIdsSqlByCode = this.GetOrganizeIdsSqlByParentId(managerUserId, permissionItemCode);
            }
            else
            {
                organizeIdsSqlByCode = BaseBusinessLogic.ArrayToList(this.GetTreeResourceScopeIds(managerUserId, BaseOrganizeEntity.TableName, permissionItemCode, false));
            }
            if (string.IsNullOrEmpty(organizeIdsSqlByCode))
            {
                organizeIdsSqlByCode = " NULL ";
            }
            organizeIdsSqlByCode = " SELECT * FROM " + BaseOrganizeEntity.TableName + " WHERE " + BaseOrganizeEntity.TableName + "." + BaseOrganizeEntity.FieldId + " IN (" + organizeIdsSqlByCode + ")  ORDER BY " + BaseOrganizeEntity.FieldSortCode;
            return base.DbHelper.Fill(organizeIdsSqlByCode);
        }

        public string[] GetOrganizeIds(string managerUserId, string permissionItemCode)
        {
            string commandText = string.Empty;
            if (UseGetChildrensByCode)
            {
                commandText = this.GetOrganizeIdsSqlByCode(managerUserId, permissionItemCode);
            }
            else if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
            {
                commandText = this.GetOrganizeIdsSqlByParentId(managerUserId, permissionItemCode);
            }
            else
            {
                //string[] ids = this.GetTreeResourceScopeIds(managerUserId, BaseOrganizeEntity.TableName, permissionItemCode, false);
                //使用递归查询，适用于MS SQL Server 2005以上
                string[] ids = this.GetTreeResourceScopeIds(managerUserId, BaseOrganizeEntity.TableName, permissionItemCode, true);
                if ((ids != null) && (ids.Length > 0))
                {
                    BaseOrganizeManager manager = new BaseOrganizeManager(base.DbHelper, base.UserInfo);
                    string[] names = new string[] { BaseOrganizeEntity.FieldId, BaseOrganizeEntity.FieldEnabled, BaseOrganizeEntity.FieldDeletionStateCode };
                    object[] values = new object[] { ids, 1, 0 };
                    ids = manager.GetIds(names, values);
                }
                return ids;
            }
            return BaseBusinessLogic.FieldToArray(base.DbHelper.Fill(commandText), BaseOrganizeEntity.FieldId);
        }

        public string GetOrganizeIdsSql(string managerUserId, string permissionItemCode)
        {
            string idByCode = new BasePermissionItemManager(base.DbHelper).GetIdByCode(permissionItemCode);
            return (" SELECT " + BasePermissionScopeEntity.FieldTargetId + "   FROM " + BasePermissionScopeEntity.TableName + "  WHERE (" + BasePermissionScopeEntity.FieldTargetCategory + " = '" + BaseOrganizeEntity.TableName + "')         AND ( " + BasePermissionScopeEntity.TableName + "." + BasePermissionScopeEntity.FieldDeletionStateCode + " = 0)         AND ( " + BasePermissionScopeEntity.TableName + "." + BasePermissionScopeEntity.FieldEnabled + " = 1)         AND ( " + BasePermissionScopeEntity.TableName + "." + BasePermissionScopeEntity.FieldTargetId + " IS NOT NULL)         AND ((" + BasePermissionScopeEntity.FieldResourceCategory + " = '" + BaseUserEntity.TableName + "'         AND " + BasePermissionScopeEntity.FieldResourceId + " = '" + managerUserId + "') OR (" + BasePermissionScopeEntity.FieldResourceCategory + " = '" + BaseRoleEntity.TableName + "'        AND " + BasePermissionScopeEntity.FieldResourceId + " IN (  SELECT " + BaseUserRoleEntity.FieldRoleId + "   FROM " + BaseUserRoleEntity.TableName + "  WHERE " + BaseUserRoleEntity.FieldUserId + " = '" + managerUserId + "'        AND " + BaseUserRoleEntity.FieldDeletionStateCode + " = 0         AND " + BaseUserRoleEntity.FieldEnabled + " = 1 )))  AND (" + BasePermissionScopeEntity.FieldPermissionItemId + " = '" + idByCode + "') ");
        }

        public string GetOrganizeIdsSqlByCode(string managerUserId, string permissionItemCode)
        {
            return (" SELECT " + BaseOrganizeEntity.FieldId + " AS " + BaseBusinessLogic.FieldId + "   FROM " + BaseOrganizeEntity.TableName + "         , ( SELECT " + base.DbHelper.PlusSign(new string[] { BaseOrganizeEntity.FieldCode, "'%'" }) + " AS " + BaseOrganizeEntity.FieldCode + "      FROM " + BaseOrganizeEntity.TableName + "     WHERE " + BaseOrganizeEntity.FieldId + " IN (" + this.GetOrganizeIdsSql(managerUserId, permissionItemCode) + ")) ManageOrganize  WHERE (" + BaseOrganizeEntity.TableName + "." + BaseOrganizeEntity.FieldEnabled + " = 1        AND " + BaseOrganizeEntity.TableName + "." + BaseOrganizeEntity.FieldCode + " LIKE ManageOrganize." + BaseOrganizeEntity.FieldCode + ")");
        }

        public string GetOrganizeIdsSqlByParentId(string managerUserId, string permissionItemCode)
        {
            return (" SELECT Id    FROM " + BaseOrganizeEntity.TableName + "  WHERE " + BaseOrganizeEntity.TableName + "." + BaseOrganizeEntity.FieldEnabled + " = 1         AND " + BaseOrganizeEntity.TableName + "." + BaseOrganizeEntity.FieldDeletionStateCode + " = 0   START WITH Id IN (" + this.GetOrganizeIdsSql(managerUserId, permissionItemCode) + ")  CONNECT BY PRIOR " + BaseOrganizeEntity.FieldId + " = " + BaseOrganizeEntity.FieldParentId);
        }

        public string[] GetResourceScopeIds(string userId, string targetCategory, string permissionItemCode)
        {
            string id = new BasePermissionItemManager(base.DbHelper, base.UserInfo).GetId(BasePermissionItemEntity.FieldCode, permissionItemCode);
            BaseUserManager manager2 = new BaseUserManager(base.DbHelper, base.UserInfo);
            string property = manager2.GetProperty(userId, BaseUserEntity.FieldRoleId);
            string commandText = string.Empty;
            commandText = " SELECT " + BasePermissionScopeEntity.FieldTargetId + " FROM " + BasePermissionScopeEntity.TableName +" WHERE (" + BasePermissionScopeEntity.FieldResourceCategory + " = '" + BaseUserEntity.TableName + "')         AND (" + BasePermissionScopeEntity.FieldResourceId + " = '" + userId + "') AND (" +BasePermissionScopeEntity.FieldTargetCategory + " = '" + targetCategory + "') AND (" + BasePermissionScopeEntity.FieldPermissionItemId + " = '" + id + "') AND (" + BasePermissionScopeEntity.FieldEnabled + " = 1) AND (" +BasePermissionScopeEntity.FieldDeletionStateCode+ " = 0) UNION  SELECT " + BasePermissionScopeEntity.FieldTargetId + " FROM " + BasePermissionScopeEntity.TableName +" WHERE (" + BasePermissionScopeEntity.FieldResourceCategory +"  = '" + BaseRoleEntity.TableName + "') AND (" +BasePermissionScopeEntity.FieldTargetCategory + " = '" + targetCategory + "') AND (" + BasePermissionScopeEntity.FieldPermissionItemId+ " = '" + id + "')  AND (" +BasePermissionScopeEntity.FieldDeletionStateCode +"= 0) AND (" + BasePermissionScopeEntity.FieldEnabled +" = 1) AND ((" + BasePermissionScopeEntity.FieldResourceId + " IN ( SELECT " + BaseUserRoleEntity.FieldRoleId + " FROM " + BaseUserRoleEntity.TableName + " WHERE (" + BaseUserRoleEntity.FieldUserId + " = '" + userId + "') AND (" +BaseUserRoleEntity.FieldEnabled +" = 1) AND (" + BaseUserRoleEntity.FieldDeletionStateCode +" = 0) ) ";
            if (!string.IsNullOrEmpty(property))
            {
                commandText = commandText + " OR (Base_PermissionScope.ResourceId = '" + property + "')";
            }
            commandText = commandText + " )  ) ";
            string[] strArray = BaseBusinessLogic.FieldToArray(base.DbHelper.Fill(commandText), BasePermissionScopeEntity.FieldTargetId);
            if ((targetCategory.Equals(BaseOrganizeEntity.TableName) && (strArray != null)) && (strArray.Length > 0))
            {
                BaseUserEntity entity = manager2.GetEntity(userId);
                for (int i = 0; i < strArray.Length; i++)
                {
                    int num2 = -2;
                    if (strArray[i].Equals(num2.ToString()))
                    {
                        strArray[i] = entity.CompanyId.ToString();
                    }
                    else
                    {
                        int num3 = -4;
                        if (strArray[i].Equals(num3.ToString()))
                        {
                            strArray[i] = entity.DepartmentId.ToString();
                        }
                        else
                        {
                            int num4 = -5;
                            if (strArray[i].Equals(num4.ToString()))
                            {
                                strArray[i] = entity.WorkgroupId.ToString();
                            }
                        }
                    }
                }
            }
            return strArray;
        }

        public DataTable GetRoleDT(string userId, string permissionItemCode)
        {
            DataTable table = new DataTable(BaseRoleEntity.TableName);
            string[] names = null;
            object[] values = null;
            BaseUserRoleManager manager = new BaseUserRoleManager(base.DbHelper, base.UserInfo);
            if (manager.UserInRole(userId, "UserAdmin") || manager.UserInRole(userId, "Admin"))
            {
                BaseRoleManager manager2 = new BaseRoleManager(base.DbHelper, base.UserInfo);
                names = new string[] { BaseRoleEntity.FieldIsVisible, BaseRoleEntity.FieldDeletionStateCode, BaseRoleEntity.FieldEnabled };
                values = new object[] { 1, 0, 1 };
                table = manager2.GetDT(names, values, BaseModuleEntity.FieldSortCode);
                table.TableName = base.CurrentTableName;
                return table;
            }
            string commandText = string.Empty;
            commandText = " SELECT *   FROM " + BaseRoleEntity.TableName + " WHERE " + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldId + " IN (" + this.GetRoleIdsSql(userId, permissionItemCode) + " ) AND (" + BaseRoleEntity.FieldDeletionStateCode + " = 0)  AND (" + BaseRoleEntity.FieldIsVisible + " = 1)  ORDER BY " + BaseRoleEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public string[] GetRoleIds(string managerUserId, string permissionItemCode)
        {
            string roleIdsSql = this.GetRoleIdsSql(managerUserId, permissionItemCode);
            string[] ids = BaseBusinessLogic.FieldToArray(base.DbHelper.Fill(roleIdsSql), BaseBusinessLogic.FieldId);
            if ((ids != null) && (ids.Length > 0))
            {
                BaseRoleManager manager = new BaseRoleManager(base.DbHelper, base.UserInfo);
                string[] names = new string[] { BaseRoleEntity.FieldId, BaseRoleEntity.FieldEnabled, BaseRoleEntity.FieldDeletionStateCode };
                object[] values = new object[] { ids, 1, 0 };
                ids = manager.GetIds(names, values);
            }
            return ids;
        }

        public string GetRoleIdsSql(string managerUserId, string permissionItemCode)
        {
            string idByCode = new BasePermissionItemManager(base.DbHelper).GetIdByCode(permissionItemCode);
            string str4 = string.Empty;
            string str2 = str4 + " SELECT Base_PermissionScope.TargetId AS " + BaseBusinessLogic.FieldId + "   FROM Base_PermissionScope   WHERE (Base_PermissionScope.TargetId IS NOT NULL         AND Base_PermissionScope.TargetCategory = '" + BaseRoleEntity.TableName + "'         AND ((Base_PermissionScope.ResourceCategory = '" + BaseUserEntity.TableName + "'              AND Base_PermissionScope.ResourceId = '" + managerUserId + "')        OR (Base_PermissionScope.ResourceCategory = '" + BaseRoleEntity.TableName + "'            AND Base_PermissionScope.ResourceId IN (  SELECT RoleId    FROM " + BaseUserRoleEntity.TableName + "  WHERE (" + BaseUserRoleEntity.FieldUserId + " = '" + managerUserId + "'         AND " + BaseUserRoleEntity.FieldEnabled + " = 1))))        AND " + BasePermissionScopeEntity.FieldPermissionItemId + " = '" + idByCode + "')";
            string[] organizeIds = this.GetOrganizeIds(managerUserId, permissionItemCode);
            if (organizeIds.Length > 0)
            {
                string str3 = BaseBusinessLogic.ObjectsToList(organizeIds);
                if (!string.IsNullOrEmpty(str3))
                {
                    string str5 = str2;
                    str2 = str5 + "  UNION  SELECT " + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldId + " AS " + BaseBusinessLogic.FieldId + "   FROM " + BaseRoleEntity.TableName + "  WHERE " + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldEnabled + " = 1     AND " + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldDeletionStateCode + " = 0     AND " + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldOrganizeId + " IN (" + str3 + ") ";
                }
            }
            return str2;
        }

        public string[] GetTargetIds(string userId, string targetCategory, string permissionItemId)
        {
            string[] strArray = new string[0];
            string[] names = new string[5];
            string[] values = new string[5];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[2] = permissionItemId;
            names[3] = BasePermissionScopeEntity.FieldTargetCategory;
            values[3] = targetCategory;
            return BaseBusinessLogic.FieldToArray(this.GetDT(names, values), BasePermissionScopeEntity.FieldTargetId);
        }

        public string[] GetTreeResourceScopeIds(string userId, string tableName, string permissionItemCode, bool childrens)
        {
            string[] ids = null;
            ids = this.GetResourceScopeIds(userId, tableName, permissionItemCode);
            if (childrens)
            {
                string str = BaseBusinessLogic.ArrayToList(ids);
                if (!string.IsNullOrEmpty(str))
                {
                    string commandText = " WITH PermissionScopeTree AS (SELECT ID \r\n                                                                FROM " + tableName + "\r\n                                                                WHERE (Id IN (" + str + ") )\r\n                                                                UNION ALL\r\n                                                               SELECT ResourceTree.Id\r\n                                                                 FROM " + tableName + " AS ResourceTree INNER JOIN\r\n                                                                      PermissionScopeTree AS A ON A.Id = ResourceTree.ParentId)\r\n                                                   SELECT Id\r\n                                                     FROM PermissionScopeTree ";
                    string[] strArray2 = BaseBusinessLogic.FieldToArray(base.DbHelper.Fill(commandText), "ID");
                    return BaseBusinessLogic.Concat(new string[][] { ids, strArray2 });
                }
            }
            return ids;
        }

        public DataTable GetUserDT(string userId, string permissionItemCode)
        {
            string[] names = null;
            object[] values = null;
            DataTable table = new DataTable(BaseRoleEntity.TableName);
            BaseUserRoleManager manager = new BaseUserRoleManager(base.DbHelper, base.UserInfo);
            if (manager.UserInRole(userId, "UserAdmin") || manager.UserInRole(userId, "Admin"))
            {
                BaseUserManager manager2 = new BaseUserManager(base.DbHelper, base.UserInfo);
                names = new string[] { BaseUserEntity.FieldIsVisible, BaseUserEntity.FieldDeletionStateCode, BaseUserEntity.FieldEnabled };
                values = new object[] { 1, 0, 1 };
                table = manager2.GetDT(names, values, BaseModuleEntity.FieldSortCode);
                table.TableName = base.CurrentTableName;
                return table;
            }
            string commandText = string.Empty;
            string str2 = " SELECT * FROM " + BaseUserEntity.TableName;
            commandText = str2 + " WHERE " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDeletionStateCode + " = 0  AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldIsVisible + " = 1  AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldEnabled + " = 1  AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + " IN (" + this.GetUserIdsSql(userId, permissionItemCode) + " )  ORDER BY " + BaseUserEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public string[] GetUserIds(string managerUserId, string permissionItemCode)
        {
            string[] ids = this.GetTreeResourceScopeIds(managerUserId, BaseOrganizeEntity.TableName, permissionItemCode, true);
            int num2 = -6;
            if (BaseBusinessLogic.Exists(ids, num2.ToString()))
            {
                return new string[] { managerUserId };
            }
            string userIdsSql = this.GetUserIdsSql(managerUserId, permissionItemCode);
            base.DbHelper.Fill(userIdsSql);
            if ((ids != null) && (ids.Length > 0))
            {
                BaseUserEntity entity = new BaseUserManager(base.DbHelper, base.UserInfo).GetEntity(managerUserId);
                for (int i = 0; i < ids.Length; i++)
                {
                    int num3 = -6;
                    if (ids[i].Equals(num3.ToString()))
                    {
                        ids[i] = entity.Id.ToString();
                        break;
                    }
                }
            }
            if ((ids != null) && (ids.Length > 0))
            {
                BaseUserManager manager2 = new BaseUserManager(base.DbHelper, base.UserInfo);
                string[] names = new string[] { BaseUserEntity.FieldId, BaseUserEntity.FieldEnabled, BaseUserEntity.FieldDeletionStateCode };
                object[] values = new object[] { ids, 1, 0 };
                ids = manager2.GetIds(names, values);
            }
            return ids;
        }

        public string GetUserIdsSql(string managerUserId, string permissionItemCode)
        {
            string idByCode = new BasePermissionItemManager(base.DbHelper).GetIdByCode(permissionItemCode);
            string str2 = string.Empty;
            str2 = " SELECT Base_PermissionScope.TargetId AS " + BaseBusinessLogic.FieldId + "   FROM Base_PermissionScope   WHERE (Base_PermissionScope.TargetCategory = '" + BaseUserEntity.TableName + "'        AND Base_PermissionScope.ResourceId = '" + managerUserId + "'        AND Base_PermissionScope.ResourceCategory = '" + BaseUserEntity.TableName + "'        AND Base_PermissionScope.PermissionId = '" + idByCode + "'        AND Base_PermissionScope.TargetId IS NOT NULL) ";
            string[] organizeIds = this.GetOrganizeIds(managerUserId, permissionItemCode);
            if (organizeIds.Length > 0)
            {
                string str3 = BaseBusinessLogic.ObjectsToList(organizeIds);
                if (!string.IsNullOrEmpty(str3))
                {
                    string str5 = str2;
                    str2 = str5 + " UNION  SELECT " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + " AS " + BaseBusinessLogic.FieldId + "   FROM " + BaseUserEntity.TableName + "  WHERE (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDeletionStateCode + " = 0 )         AND (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldEnabled + " = 1 )         AND (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldCompanyId + " IN (" + str3 + ")             OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDepartmentId + " IN (" + str3 + ")             OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldWorkgroupId + " IN (" + str3 + ")) ";
                }
            }
            string[] roleIds = this.GetRoleIds(managerUserId, permissionItemCode);
            if (roleIds.Length > 0)
            {
                string str4 = BaseBusinessLogic.ObjectsToList(roleIds);
                if (!string.IsNullOrEmpty(str4))
                {
                    string str6 = str2;
                    str2 = str6 + " UNION  SELECT " + BaseUserRoleEntity.TableName + "." + BaseUserRoleEntity.FieldUserId + " AS " + BaseBusinessLogic.FieldId + "   FROM " + BaseUserRoleEntity.TableName + "  WHERE (" + BaseUserRoleEntity.TableName + "." + BaseUserRoleEntity.FieldEnabled + " = 1         AND " + BaseUserRoleEntity.TableName + "." + BaseUserRoleEntity.FieldDeletionStateCode + " = 0         AND " + BaseUserRoleEntity.TableName + "." + BaseUserRoleEntity.FieldRoleId + " IN (" + str4 + ")) ";
                }
            }
            return str2;
        }

        public PermissionScope GetUserPermissionScope(string managerUserId, string permissionItemCode)
        {
            string organizeIdsSql = this.GetOrganizeIdsSql(managerUserId, permissionItemCode);
            return BaseBusinessLogic.GetPermissionScope(BaseBusinessLogic.FieldToArray(base.DbHelper.Fill(organizeIdsSql), BasePermissionScopeEntity.FieldTargetId));
        }

        public int GrantResourcePermissionScopeTarget(string resourceCategory, string[] resourceIds, string targetCategory, string grantTargetId, string permissionItemId)
        {
            int num = 0;
            string[] names = new string[7];
            object[] values = new object[7];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = resourceCategory;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = targetCategory;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            values[3] = grantTargetId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = permissionItemId;
            names[5] = BasePermissionScopeEntity.FieldEnabled;
            values[5] = 1;
            names[6] = BasePermissionScopeEntity.FieldDeletionStateCode;
            values[6] = 0;
            BasePermissionScopeEntity baseResourcePermissionScopeEntity = new BasePermissionScopeEntity {
                ResourceCategory = resourceCategory,
                TargetCategory = targetCategory,
                PermissionId = new int?(int.Parse(permissionItemId)),
                TargetId = grantTargetId,
                Enabled = 1,
                DeletionStateCode = 0
            };
            for (int i = 0; i < resourceIds.Length; i++)
            {
                baseResourcePermissionScopeEntity.ResourceId = resourceIds[i];
                values[1] = resourceIds[i];
                if (!this.Exists(names, values))
                {
                    this.Add(baseResourcePermissionScopeEntity);
                    num++;
                }
            }
            return num;
        }

        //public int GrantResourcePermissionScopeTarget(string resourceCategory, string resourceId, string targetCategory, string[] grantTargetIds, string permissionItemId, DateTime? startDate = new DateTime?(), DateTime? endDate = new DateTime?())//C# 4.0 才支持缺省参数
        public int GrantResourcePermissionScopeTarget(string resourceCategory, string resourceId, string targetCategory, string[] grantTargetIds, string permissionItemId, DateTime? startDate, DateTime? endDate)
        {
            int num = 0;
            string[] names = new string[7];
            object[] values = new object[7];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = resourceCategory;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = resourceId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = targetCategory;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = permissionItemId;
            names[5] = BasePermissionScopeEntity.FieldEnabled;
            values[5] = 1;
            names[6] = BasePermissionScopeEntity.FieldDeletionStateCode;
            values[6] = 0;
            BasePermissionScopeEntity baseResourcePermissionScopeEntity = new BasePermissionScopeEntity {
                ResourceCategory = resourceCategory,
                ResourceId = resourceId,
                TargetCategory = targetCategory,
                PermissionId = new int?(int.Parse(permissionItemId)),
                StartDate = startDate,
                EndDate = endDate,
                Enabled = 1,
                DeletionStateCode = 0
            };
            for (int i = 0; i < grantTargetIds.Length; i++)
            {
                baseResourcePermissionScopeEntity.TargetId = grantTargetIds[i];
                values[3] = grantTargetIds[i];
                if (!this.Exists(names, values))
                {
                    this.Add(baseResourcePermissionScopeEntity);
                    num++;
                }
            }
            return num;
        }

        public int GrantResourcePermissionScopeTarget(string resourceCategory, string resourceId, string targetCategory, string[] grantTargetIds, string permissionItemId)
        {
            DateTime? startDate = new DateTime?();
            DateTime? endDate = new DateTime?();
            return GrantResourcePermissionScopeTarget(resourceCategory, resourceId, targetCategory, grantTargetIds, permissionItemId, startDate, endDate);
        }

        public bool HasAuthorized(string[] names, object[] values, string startDate, string endDate)
        {
            BasePermissionScopeManager manager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return manager.Exists(names, values);
        }

        public bool IsModuleAuthorized(BaseUserInfo userInfo, string moduleCode, string permissionItemCode)
        {
            return (userInfo.IsAdministrator || this.IsModuleAuthorized(userInfo.Id, moduleCode, permissionItemCode));
        }

        public bool IsModuleAuthorized(string userId, string moduleCode, string permissionItemCode)
        {
            string idByCode = new BaseModuleManager(base.DbHelper).GetIdByCode(moduleCode);
            string permissionItemId = new BasePermissionItemManager(base.DbHelper).GetIdByCode(permissionItemCode);
            return (this.CheckUserModulePermission(userId, idByCode, permissionItemId) || this.CheckRoleModulePermission(userId, idByCode, permissionItemId));
        }

        public int PermissionScopeDelete(string permissionItemId, string resourceCategory, string resourceId, string targetCategory, string targetId)
        {
            string[] names = new string[5];
            string[] values = new string[5];
            names[0] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[0] = permissionItemId;
            names[1] = BasePermissionScopeEntity.FieldResourceCategory;
            values[1] = resourceCategory;
            names[2] = BasePermissionScopeEntity.FieldResourceId;
            values[2] = resourceId;
            names[3] = BasePermissionScopeEntity.FieldTargetCategory;
            values[3] = targetCategory;
            names[4] = BasePermissionScopeEntity.FieldTargetId;
            values[4] = targetId;
            return this.Delete(names, values);
        }

        public bool PermissionScopeExists(string permissionItemId, string resourceCategory, string resourceId, string targetCategory, string targetId)
        {
            bool flag = true;
            string[] names = new string[5];
            string[] values = new string[5];
            names[0] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[0] = permissionItemId;
            names[1] = BasePermissionScopeEntity.FieldResourceCategory;
            values[1] = resourceCategory;
            names[2] = BasePermissionScopeEntity.FieldResourceId;
            values[2] = resourceId;
            names[3] = BasePermissionScopeEntity.FieldTargetCategory;
            values[3] = targetCategory;
            names[4] = BasePermissionScopeEntity.FieldTargetId;
            values[4] = targetId;
            if (!this.Exists(names, values))
            {
                flag = false;
            }
            return flag;
        }

        public int RevokeResourcePermissionScopeTarget(string resourceCategory, string resourceId, string targetCategory, string permissionItemId)
        {
            string[] names = new string[5];
            object[] values = new object[5];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = resourceCategory;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = resourceId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = targetCategory;
            names[3] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[3] = permissionItemId;
            names[4] = BasePermissionScopeEntity.FieldEnabled;
            values[4] = 1;
            return this.SetDeleted(names, values);
        }

        public int RevokeResourcePermissionScopeTarget(string resourceCategory, string resourceId, string targetCategory, string[] revokeTargetIds, string permissionItemId)
        {
            int num = 0;
            string[] names = new string[6];
            object[] values = new object[6];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = resourceCategory;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = resourceId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = targetCategory;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = permissionItemId;
            names[5] = BasePermissionScopeEntity.FieldEnabled;
            values[5] = 1;
            for (int i = 0; i < revokeTargetIds.Length; i++)
            {
                values[3] = revokeTargetIds[i];
                num += this.SetDeleted(names, values);
            }
            return num;
        }

        public int RevokeResourcePermissionScopeTarget(string resourceCategory, string[] resourceIds, string targetCategory, string revokeTargetId, string permissionItemId)
        {
            int num = 0;
            string[] names = new string[6];
            object[] values = new object[6];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = resourceCategory;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = targetCategory;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            values[3] = revokeTargetId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = permissionItemId;
            names[5] = BasePermissionScopeEntity.FieldEnabled;
            values[5] = 1;
            for (int i = 0; i < resourceIds.Length; i++)
            {
                values[1] = resourceIds[i];
                num += this.SetDeleted(names, values);
            }
            return num;
        }

        public DataTable Search(string resourceId, string resourceCategory, string targetCategory)
        {
            string commandText = string.Empty;
            commandText = "SELECT * FROM " + base.CurrentTableName + " WHERE " + BasePermissionScopeEntity.FieldDeletionStateCode + " =0  AND " + BasePermissionScopeEntity.FieldEnabled + " =1 ";
            new List<IDbDataParameter>();
            if (!string.IsNullOrEmpty(resourceId))
            {
                string str2 = commandText;
                commandText = str2 + " AND " + BasePermissionScopeEntity.FieldResourceId + " = '" + resourceId + "'";
            }
            if (!string.IsNullOrEmpty(resourceCategory))
            {
                string str3 = commandText;
                commandText = str3 + " AND " + BasePermissionScopeEntity.FieldResourceCategory + " = '" + resourceCategory + "'";
            }
            if (!string.IsNullOrEmpty(targetCategory))
            {
                string str4 = commandText;
                commandText = str4 + " AND " + BasePermissionScopeEntity.FieldTargetCategory + " = '" + targetCategory + "'";
            }
            commandText = commandText + " ORDER BY " + BasePermissionScopeEntity.FieldCreateOn + " DESC ";
            return base.DbHelper.Fill(commandText);
        }

        private void SetEntity(SQLBuilder sqlBuilder, BasePermissionScopeEntity baseResourcePermissionScopeEntity)
        {
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldResourceCategory, baseResourcePermissionScopeEntity.ResourceCategory, null);
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldResourceId, baseResourcePermissionScopeEntity.ResourceId, null);
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldTargetCategory, baseResourcePermissionScopeEntity.TargetCategory, null);
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldTargetId, baseResourcePermissionScopeEntity.TargetId, null);
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldPermissionItemId, baseResourcePermissionScopeEntity.PermissionId, null);
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldPermissionConstraint, baseResourcePermissionScopeEntity.PermissionConstraint, null);
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldStartDate, baseResourcePermissionScopeEntity.StartDate, null);
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldEndDate, baseResourcePermissionScopeEntity.EndDate, null);
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldEnabled, baseResourcePermissionScopeEntity.Enabled, null);
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldDeletionStateCode, baseResourcePermissionScopeEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BasePermissionScopeEntity.FieldDescription, baseResourcePermissionScopeEntity.Description, null);
        }

        public int Update(BasePermissionScopeEntity baseResourcePermissionScopeEntity)
        {
            return this.UpdateEntity(baseResourcePermissionScopeEntity);
        }

        public int UpdateEntity(BasePermissionScopeEntity baseResourcePermissionScopeEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BasePermissionScopeEntity.TableName);
            this.SetEntity(sqlBuilder, baseResourcePermissionScopeEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BasePermissionScopeEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BasePermissionScopeEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BasePermissionScopeEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BasePermissionScopeEntity.FieldId, baseResourcePermissionScopeEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

