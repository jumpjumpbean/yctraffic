//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

/// 修改纪录
/// 
///		2012.06.29 版本：2.0 sunmiao 修改
///		
/// 

namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    /// <summary>
    /// 权限服务
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class PermissionService : MarshalByRefObject, IPermissionService
    {
        private string serviceName = AppMessage.PermissionService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string AddPermission(BaseUserInfo userInfo, string permissionCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BasePermissionItemManager manager = new BasePermissionItemManager(helper, userInfo);
                        string statusCode = string.Empty;
                        BasePermissionItemEntity permissionItemEntity = new BasePermissionItemEntity {
                            Code = permissionCode,
                            Enabled = 1,
                            AllowDelete = 1,
                            AllowEdit = 1,
                            IsScope = 0
                        };
                        str = manager.Add(permissionItemEntity, out statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.MSG0091, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string AddRole(BaseUserInfo userInfo, string role)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleManager manager = new BaseRoleManager(helper, userInfo);
                        string statusCode = string.Empty;
                        BaseRoleEntity roleEntity = new BaseRoleEntity {
                            RealName = role,
                            Category = "Role",
                            Enabled = 1
                        };
                        str = manager.Add(roleEntity, out statusCode);
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string AddUserToRole(BaseUserInfo userInfo, string userName, string roleName)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string id = new BaseUserManager(helper, userInfo).GetId(BaseUserEntity.FieldUserName, userName);
                        string str3 = new BaseRoleManager(helper, userInfo).GetId(BaseRoleEntity.FieldRealName, roleName);
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(str3))
                        {
                            str = new BaseUserRoleManager(helper, userInfo).AddToRole(id, str3);
                        }
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int ClearPermissionScopeTarget(BaseUserInfo userInfo, string resourceCategory, string resourceId, string targetCategory, string permissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BasePermissionScopeManager(helper, userInfo).RevokeResourcePermissionScopeTarget(resourceCategory, resourceId, targetCategory, permissionItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_ClearPermissionScopeTarget, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int ClearRolePermission(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserRoleManager manager = new BaseUserRoleManager(helper, userInfo);
                        num += manager.ClearRoleUser(id);
                        BaseRolePermissionManager manager2 = new BaseRolePermissionManager(helper, userInfo);
                        num += manager2.RevokeAll(id);
                        BaseRoleScopeManager manager3 = new BaseRoleScopeManager(helper, userInfo);
                        num += manager3.RevokeAll(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_ClearRolePermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int ClearRolePermissionScope(BaseUserInfo userInfo, string roleId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseRoleScopeManager(helper, userInfo).ClearRolePermissionScope(roleId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_ClearRolePermissionScope, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int ClearUserPermission(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserRoleManager manager = new BaseUserRoleManager(helper, userInfo);
                        num += manager.ClearRoleUser(id);
                        BaseUserPermissionManager manager2 = new BaseUserPermissionManager(helper, userInfo);
                        num += manager2.RevokeAll(id);
                        BaseUserScopeManager manager3 = new BaseUserScopeManager(helper, userInfo);
                        num += manager3.RevokeAll(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_ClearUserPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int ClearUserPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseUserScopeManager(helper, userInfo).ClearUserPermissionScope(userId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_ClearUserPermissionScope, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int DeletePermission(BaseUserInfo userInfo, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BasePermissionItemManager manager = new BasePermissionItemManager(helper, userInfo);
                        string id = manager.GetId(BasePermissionItemEntity.FieldCode, permissionItemCode);
                        if (!string.IsNullOrEmpty(id))
                        {
                            num = manager.Delete(id);
                        }
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int DeleteRole(BaseUserInfo userInfo, string role)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleManager manager = new BaseRoleManager(helper, userInfo);
                        string id = manager.GetId(BaseRoleEntity.FieldRealName, role);
                        if (!string.IsNullOrEmpty(id))
                        {
                            num = manager.Delete(id);
                        }
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetModuleDT(BaseUserInfo userInfo)
        {
            return this.GetModuleDTByUser(userInfo, userInfo.Id);
        }

        public DataTable GetModuleDTByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dTByPermission = new DataTable(BaseModuleEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        dTByPermission = new BaseModuleManager(helper, userInfo).GetDTByPermission(userId, permissionItemCode);
                        dTByPermission.TableName = BaseModuleEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetModuleDTByPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dTByPermission;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetModuleDTByUser(BaseUserInfo userInfo, string userId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dTByUser = new DataTable(BaseModuleEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseModuleManager manager = new BaseModuleManager(helper, userInfo);
                        if (userInfo.IsAdministrator)
                        {
                            dTByUser = manager.GetDT(BaseModuleEntity.FieldDeletionStateCode, 0, BaseModuleEntity.FieldEnabled, 1, BaseModuleEntity.FieldSortCode);
                        }
                        else
                        {
                            dTByUser = manager.GetDTByUser(userId);
                        }
                        dTByUser.TableName = BaseModuleEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetModuleDTByUser, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dTByUser;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetModuleIdsByUser(BaseUserInfo userInfo, string userId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] iDsByUser = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseModuleManager manager = new BaseModuleManager(helper, userInfo);
                        if (userInfo.IsAdministrator)
                        {
                            iDsByUser = manager.GetIds(BaseModuleEntity.FieldDeletionStateCode, "0", BaseModuleEntity.FieldEnabled, "1", BaseModuleEntity.FieldId);
                        }
                        else
                        {
                            iDsByUser = manager.GetIDsByUser(userId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetModuleDTByUser, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return iDsByUser;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetOrganizeDTByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dT = new DataTable(BaseOrganizeEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        if (string.IsNullOrEmpty(permissionItemCode))
                        {
                            dT = new BaseOrganizeManager(helper, userInfo).GetDT();
                            dT.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                        }
                        else
                        {
                            dT = new BasePermissionScopeManager(helper, userInfo).GetOrganizeDT(userInfo.Id, permissionItemCode);
                            dT.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
                        }
                        dT.TableName = BaseOrganizeEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetOrganizeDTByPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dT;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetOrganizeIdsByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] organizeIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        if (string.IsNullOrEmpty(permissionItemCode))
                        {
                            return organizeIds;
                        }
                        organizeIds = new BasePermissionScopeManager(helper, userInfo).GetOrganizeIds(userId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetOrganizeIdsByPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return organizeIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetPermissionDT(BaseUserInfo userInfo)
        {
            return this.GetPermissionDTByUser(userInfo, userInfo.Id);
        }

        public DataTable GetPermissionDTByUser(BaseUserInfo userInfo, string userId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dT = new DataTable(BasePermissionItemEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        if (manager.IsAdministrator(userId))
                        {
                            dT = new BasePermissionItemManager(helper, userInfo).GetDT();
                        }
                        else
                        {
                            dT = new BasePermissionManager(helper).GetPermissionByUser(userId);
                        }
                        dT.TableName = BasePermissionItemEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetPermissionDTByUser, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dT;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetPermissionItemDTByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dTByUser = new DataTable(BasePermissionItemEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BasePermissionItemManager manager = new BasePermissionItemManager(helper, userInfo);
                        if (string.IsNullOrEmpty(manager.GetId(BasePermissionItemEntity.FieldCode, permissionItemCode)) && permissionItemCode.Equals("Resource.ManagePermission"))
                        {
                            BasePermissionItemEntity basePermissionItemEntity = new BasePermissionItemEntity {
                                Code = "Resource.ManagePermission",
                                FullName = "资源管理范围权限（系统默认）",
                                IsScope = 1,
                                Enabled = 1,
                                AllowDelete = 0
                            };
                            manager.AddEntity(basePermissionItemEntity);
                        }
                        dTByUser = manager.GetDTByUser(userId, permissionItemCode);
                        dTByUser.TableName = BasePermissionItemEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetPermissionItemDTByPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dTByUser;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetPermissionScopeResourceIds(BaseUserInfo userInfo, string resourceCategory, string targetId, string targetResourceCategory, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] strArray = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string id = new BasePermissionItemManager(helper, userInfo).GetId(BasePermissionItemEntity.FieldCode, permissionItemCode);
                        string[] names = new string[6];
                        object[] values = new object[6];
                        names[0] = BasePermissionScopeEntity.FieldResourceCategory;
                        values[0] = resourceCategory;
                        names[1] = BasePermissionScopeEntity.FieldTargetId;
                        values[1] = targetId;
                        names[2] = BasePermissionScopeEntity.FieldPermissionItemId;
                        values[2] = id;
                        names[3] = BasePermissionScopeEntity.FieldTargetCategory;
                        values[3] = targetResourceCategory;
                        names[4] = BasePermissionScopeEntity.FieldDeletionStateCode;
                        values[4] = 0;
                        names[5] = BasePermissionScopeEntity.FieldEnabled;
                        values[5] = 1;
                        strArray = DbLogic.GetIds(helper, BasePermissionScopeEntity.TableName, names, values, BasePermissionScopeEntity.FieldResourceId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return strArray;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetPermissionScopeTargetIds(BaseUserInfo userInfo, string resourceCategory, string resourceId, string targetCategory, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] strArray = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string id = new BasePermissionItemManager(helper, userInfo).GetId(BasePermissionItemEntity.FieldCode, permissionItemCode);
                        string[] names = new string[6];
                        object[] values = new object[6];
                        names[0] = BasePermissionScopeEntity.FieldResourceCategory;
                        values[0] = resourceCategory;
                        names[1] = BasePermissionScopeEntity.FieldResourceId;
                        values[1] = resourceId;
                        names[2] = BasePermissionScopeEntity.FieldPermissionItemId;
                        values[2] = id;
                        names[3] = BasePermissionScopeEntity.FieldTargetCategory;
                        values[3] = targetCategory;
                        names[4] = BasePermissionScopeEntity.FieldDeletionStateCode;
                        values[4] = 0;
                        names[5] = BasePermissionScopeEntity.FieldEnabled;
                        values[5] = 1;
                        strArray = DbLogic.GetIds(helper, BasePermissionScopeEntity.TableName, names, values, BasePermissionScopeEntity.FieldTargetId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetPermissionScopeTargetIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return strArray;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetResourcePermissionItemIds(BaseUserInfo userInfo, string resourceCategory, string resourceId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] strArray = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string[] names = new string[2];
                        string[] values = new string[2];
                        names[0] = BasePermissionEntity.FieldResourceCategory;
                        values[0] = resourceCategory;
                        names[1] = BasePermissionEntity.FieldResourceId;
                        values[1] = resourceId;
                        strArray = BaseBusinessLogic.FieldToArray(DbLogic.GetDT(helper, BasePermissionEntity.TableName, names, values), BasePermissionEntity.FieldPermissionItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetResourcePermissionItemIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return strArray;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetResourceScopeIds(BaseUserInfo userInfo, string userId, string targetCategory, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] strArray = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        strArray = new BasePermissionScopeManager(helper, userInfo).GetResourceScopeIds(userId, targetCategory, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetResourceScopeIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return strArray;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetRoleDTByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable roleDT = new DataTable(BaseRoleEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        if (userInfo.IsAdministrator || string.IsNullOrEmpty(permissionItemCode))
                        {
                            roleDT = new BaseRoleManager(helper, userInfo).GetDT(BaseRoleEntity.FieldDeletionStateCode, 0, BaseRoleEntity.FieldIsVisible, 1, BaseRoleEntity.FieldSortCode);
                        }
                        else
                        {
                            roleDT = new BasePermissionScopeManager(helper, userInfo).GetRoleDT(userInfo.Id, permissionItemCode);
                        }
                        roleDT.TableName = BaseRoleEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetRoleDTByPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return roleDT;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetRoleIdsByPermission(BaseUserInfo userInfo, string permissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] roleIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                        roleIds = new BaseRolePermissionManager(helper, userInfo).GetRoleIds(permissionItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetRoleIdsByPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return roleIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetRoleIdsByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] roleIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        if (string.IsNullOrEmpty(permissionItemCode))
                        {
                            return roleIds;
                        }
                        roleIds = new BasePermissionScopeManager(helper, userInfo).GetRoleIds(userId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetRoleIdsByPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return roleIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetRolePermissionItemIds(BaseUserInfo userInfo, string roleId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] permissionItemIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                        permissionItemIds = new BaseRolePermissionManager(helper, userInfo).GetPermissionItemIds(roleId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetRolePermissionItemIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return permissionItemIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetRoleScopeModuleIds(BaseUserInfo userInfo, string roleId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] moduleIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        moduleIds = new BaseRoleScopeManager(helper, userInfo).GetModuleIds(roleId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetRoleScopeModuleIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return moduleIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetRoleScopeOrganizeIds(BaseUserInfo userInfo, string roleId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] organizeIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        organizeIds = new BaseRoleScopeManager(helper, userInfo).GetOrganizeIds(roleId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetRoleScopeOrganizeIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return organizeIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetRoleScopePermissionItemIds(BaseUserInfo userInfo, string roleId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] permissionItemIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                        permissionItemIds = new BaseRoleScopeManager(helper, userInfo).GetPermissionItemIds(roleId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetRoleScopePermissionItemIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return permissionItemIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetRoleScopeRoleIds(BaseUserInfo userInfo, string roleId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] roleIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        roleIds = new BaseRoleScopeManager(helper, userInfo).GetRoleIds(roleId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetRoleScopeRoleIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return roleIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetRoleScopeUserIds(BaseUserInfo userInfo, string roleId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] userIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        userIds = new BaseRoleScopeManager(helper, userInfo).GetUserIds(roleId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetRoleScopeUserIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return userIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetTreeResourceScopeIds(BaseUserInfo userInfo, string userId, string targetCategory, string permissionItemCode, bool childrens)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] strArray = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        strArray = new BasePermissionScopeManager(helper, userInfo).GetTreeResourceScopeIds(userId, targetCategory, permissionItemCode, childrens);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetTreeResourceScopeIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return strArray;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetUserDTByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable userDT = new DataTable(BaseUserEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BasePermissionScopeManager manager = new BasePermissionScopeManager(helper, userInfo);
                        if (!string.IsNullOrEmpty(permissionItemCode))
                        {
                            userDT = manager.GetUserDT(userInfo.Id, permissionItemCode);
                        }
                        userDT.TableName = BaseUserEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetUserDTByPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return userDT;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetUserIdsByPermission(BaseUserInfo userInfo, string permissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] userIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                        userIds = new BaseUserPermissionManager(helper, userInfo).GetUserIds(permissionItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetUserIdsByPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return userIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetUserIdsByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] userIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        if (string.IsNullOrEmpty(permissionItemCode))
                        {
                            return userIds;
                        }
                        userIds = new BasePermissionScopeManager(helper, userInfo).GetUserIds(userId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetUserIdsByPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return userIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetUserPermissionItemIds(BaseUserInfo userInfo, string userId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] permissionItemIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                        permissionItemIds = new BaseUserPermissionManager(helper, userInfo).GetPermissionItemIds(userId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetUserPermissionItemIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return permissionItemIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public PermissionScope GetUserPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            PermissionScope none = PermissionScope.None;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        none = new BasePermissionScopeManager(helper, userInfo).GetUserPermissionScope(userId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetUserPermissionScope, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return none;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetUserScopeModuleIds(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] moduleIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        moduleIds = new BaseUserScopeManager(helper, userInfo).GetModuleIds(userId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetUserScopeModuleIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return moduleIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetUserScopeOrganizeIds(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] organizeIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        organizeIds = new BaseUserScopeManager(helper, userInfo).GetOrganizeIds(userId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetUserScopeOrganizeIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return organizeIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetUserScopePermissionItemIds(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] permissionItemIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                        permissionItemIds = new BaseUserScopeManager(helper, userInfo).GetPermissionItemIds(userId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetUserScopePermissionItemIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return permissionItemIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetUserScopeRoleIds(BaseUserInfo userInfo, string userId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] roleIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        roleIds = new BaseUserScopeManager(helper, userInfo).GetRoleIds(userId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetUserScopeRoleIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return roleIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetUserScopeUserIds(BaseUserInfo userInfo, string userId, string permissionScopeItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] userIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        userIds = new BaseUserScopeManager(helper, userInfo).GetUserIds(userId, permissionScopeItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GetUserScopeUserIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return userIds;
                }
                finally
                {
                    helper.Close();
                }
            };
        }

        public int GrantPermissionScopeTarget(BaseUserInfo userInfo, string resourceCategory, string[] resourceIds, string targetCategory, string grantTargetId, string permissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BasePermissionScopeManager(helper, userInfo).GrantResourcePermissionScopeTarget(resourceCategory, resourceIds, targetCategory, grantTargetId, permissionItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantPermissionScopeTargets(BaseUserInfo userInfo, string resourceCategory, string resourceId, string targetCategory, string[] grantTargetIds, string permissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BasePermissionScopeManager(helper, userInfo).GrantResourcePermissionScopeTarget(resourceCategory, resourceId, targetCategory, grantTargetIds, permissionItemId, null, null);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantResourcePermission(BaseUserInfo userInfo, string resourceCategory, string resourceId, string[] grantPermissionItemIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        new BaseRoleScopeManager(helper, userInfo);
                        if (grantPermissionItemIds != null)
                        {
                            BasePermissionManager manager = new BasePermissionManager(helper, userInfo);
                            for (int i = 0; i < grantPermissionItemIds.Length; i++)
                            {
                                BasePermissionEntity resourcePermissionEntity = new BasePermissionEntity {
                                    ResourceCategory = resourceCategory,
                                    ResourceId = resourceId,
                                    PermissionId = new int?(int.Parse(grantPermissionItemIds[i])),
                                    Enabled = 1,
                                    DeletionStateCode = 0
                                };
                                manager.Add(resourcePermissionEntity);
                                num++;
                            }
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantResourcePermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string GrantRoleModuleScope(BaseUserInfo userInfo, string roleId, string permissionItemCode, string grantModuleId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (grantModuleId != null)
                        {
                            str = manager.GrantModule(roleId, permissionItemCode, grantModuleId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantRoleModuleScope, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantRoleModuleScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] grantModuleIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (grantModuleIds != null)
                        {
                            num += manager.GrantModules(roleId, permissionItemCode, grantModuleIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantRoleModuleScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantRoleOrganizeScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] grantOrganizeIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (grantOrganizeIds != null)
                        {
                            num += manager.GrantOrganizes(roleId, permissionItemCode, grantOrganizeIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantRoleOrganizeScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string GrantRolePermission(BaseUserInfo userInfo, string roleName, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string id = new BaseRoleManager(helper, userInfo).GetId(BaseRoleEntity.FieldRealName, roleName);
                        string str3 = new BasePermissionItemManager(helper, userInfo).GetId(BasePermissionItemEntity.FieldCode, permissionItemCode);
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(str3))
                        {
                            str = new BaseRolePermissionManager(helper, userInfo).Grant(id, str3);
                        }
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string GrantRolePermissionById(BaseUserInfo userInfo, string roleId, string grantPermissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRolePermissionManager manager = new BaseRolePermissionManager(helper, userInfo);
                        if (grantPermissionItemId != null)
                        {
                            str = manager.Grant(roleId, grantPermissionItemId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantRolePermissionById, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantRolePermissionItemScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] grantPermissionItemIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (grantPermissionItemIds != null)
                        {
                            num += manager.GrantPermissionItemes(roleId, permissionItemCode, grantPermissionItemIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantRolePermissionItemScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantRolePermissions(BaseUserInfo userInfo, string[] roleIds, string[] grantPermissionItemIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRolePermissionManager manager = new BaseRolePermissionManager(helper, userInfo);
                        if ((roleIds != null) && (grantPermissionItemIds != null))
                        {
                            num += manager.Grant(roleIds, grantPermissionItemIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantRolePermissions, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantRoleRoleScopes(BaseUserInfo userInfo, string roleId, string permissionItemId, string[] grantRoleIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (grantRoleIds != null)
                        {
                            num += manager.GrantRoles(roleId, permissionItemId, grantRoleIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantRoleRoleScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantRoleUserScopes(BaseUserInfo userInfo, string roleId, string permissionItemId, string[] grantUserIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (grantUserIds != null)
                        {
                            num += manager.GrantUsers(roleId, permissionItemId, grantUserIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantRoleUserScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string GrantUserModuleScope(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string grantModuleId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (grantModuleId != null)
                        {
                            str = manager.GrantModule(userId, permissionScopeItemCode, grantModuleId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantUserModuleScope, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantUserModuleScopes(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string[] grantModuleIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (grantModuleIds != null)
                        {
                            new BasePermissionItemManager(helper, userInfo);
                            num += manager.GrantModules(userId, permissionScopeItemCode, grantModuleIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantUserModuleScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantUserOrganizeScopes(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string[] grantOrganizeIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (grantOrganizeIds != null)
                        {
                            num += manager.GrantOrganizes(userId, permissionScopeItemCode, grantOrganizeIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantUserOrganizeScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string GrantUserPermission(BaseUserInfo userInfo, string userName, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string id = new BaseUserManager(helper, userInfo).GetId(BaseUserEntity.FieldUserName, userName);
                        string str3 = new BasePermissionItemManager(helper, userInfo).GetId(BasePermissionItemEntity.FieldCode, permissionItemCode);
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(str3))
                        {
                            str = new BaseUserPermissionManager(helper, userInfo).Grant(id, str3);
                        }
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string GrantUserPermissionById(BaseUserInfo userInfo, string userId, string grantPermissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserPermissionManager manager = new BaseUserPermissionManager(helper, userInfo);
                        if (grantPermissionItemId != null)
                        {
                            str = manager.Grant(userId, grantPermissionItemId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantUserPermissionById, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantUserPermissionItemScopes(BaseUserInfo userInfo, string userId, string permissionItemCode, string[] grantPermissionItemIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (grantPermissionItemIds != null)
                        {
                            num += manager.GrantPermissionItemes(userId, permissionItemCode, grantPermissionItemIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantUserPermissionItemScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantUserPermissions(BaseUserInfo userInfo, string[] userIds, string[] grantPermissionItemIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserPermissionManager manager = new BaseUserPermissionManager(helper, userInfo);
                        if ((userIds != null) && (grantPermissionItemIds != null))
                        {
                            num += manager.Grant(userIds, grantPermissionItemIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantUserPermissions, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantUserRoleScopes(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string[] grantRoleIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (grantRoleIds != null)
                        {
                            num += manager.GrantRoles(userId, permissionScopeItemCode, grantRoleIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantUserRoleScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int GrantUserUserScopes(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string[] grantUserIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (grantUserIds != null)
                        {
                            num += manager.GrantUsers(userId, permissionScopeItemCode, grantUserIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_GrantUserUserScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public bool IsAdministrator(BaseUserInfo userInfo)
        {
            return this.IsAdministratorByUser(userInfo, userInfo.Id);
        }

        public bool IsAdministratorByUser(BaseUserInfo userInfo, string userId)
        {
            LogOnService.UserIsLogOn(userInfo);
            bool flag = false;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        flag = new BaseUserManager(helper, userInfo).IsAdministrator(userId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_IsAdministratorByUser, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return flag;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        //public bool IsAuthorized(BaseUserInfo userInfo, string permissionItemCode, string permissionItemName = null) //C# 4.0 才支持缺省参数
        public bool IsAuthorized(BaseUserInfo userInfo, string permissionItemCode)
        {
            //return this.IsAuthorizedByUser(userInfo, userInfo.Id, permissionItemCode, permissionItemName);
            return this.IsAuthorizedByUser(userInfo, userInfo.Id, permissionItemCode);
        }

        public bool IsAuthorizedByRole(BaseUserInfo userInfo, string roleId, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            bool flag = false;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        flag = roleId.Equals("Administrators");
                        if (flag)
                        {
                            return flag;
                        }
                        flag = new BasePermissionManager(helper, userInfo).CheckPermissionByRole(roleId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_IsAuthorizedByRole, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return flag;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        //public bool IsAuthorizedByUser(BaseUserInfo userInfo, string userId, string permissionItemCode, string permissionItemName = null) //C# 4.0 才支持缺省参数
        public bool IsAuthorizedByUser(BaseUserInfo userInfo, string userId, string permissionItemCode) 
        {
            LogOnService.UserIsLogOn(userInfo);
            bool flag = false;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        flag = new BaseUserManager(helper, userInfo).IsAdministrator(userId);
                        if (flag)
                        {
                            return flag;
                        }
                        //flag = new BasePermissionManager(helper, userInfo).CheckPermissionByUser(userId, permissionItemCode, permissionItemName);
                        flag = new BasePermissionManager(helper, userInfo).CheckPermissionByUser(userId, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_IsAuthorizedByUser, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return flag;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public bool IsInRole(BaseUserInfo userInfo, string userId, string roleName)
        {
            LogOnService.UserIsLogOn(userInfo);
            bool flag = false;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string str = new BaseRoleManager(helper, userInfo).GetProperty(BaseRoleEntity.FieldRealName, roleName, BaseRoleEntity.FieldCode);
                        if (!string.IsNullOrEmpty(str))
                        {
                            flag = new BaseUserRoleManager(helper, userInfo).UserInRole(userId, str);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_IsInRole, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return flag;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public bool IsModuleAuthorized(BaseUserInfo userInfo, string moduleCode)
        {
            return this.IsModuleAuthorizedByUser(userInfo, userInfo.Id, moduleCode);
        }

        public bool IsModuleAuthorizedByUser(BaseUserInfo userInfo, string userId, string moduleCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            bool flag = false;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        if (manager.IsAdministrator(userId))
                        {
                            return true;
                        }
                        BaseModuleManager manager2 = new BaseModuleManager(helper, userInfo);
                        foreach (DataRow row in manager2.GetDTByUser(userId).Rows)
                        {
                            if (row[BaseModuleEntity.FieldCode].ToString().Equals(moduleCode))
                            {
                                flag = true;
                                break;
                            }
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_IsModuleAuthorizedByUser, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return flag;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public bool IsModuleAuthorizedByUser(BaseUserInfo userInfo, string userId, string moduleCode, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            bool flag = false;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        flag = new BaseUserManager(helper, userInfo).IsAdministrator(userId);
                        if (flag)
                        {
                            return flag;
                        }
                        flag = new BasePermissionScopeManager(helper).IsModuleAuthorized(userId, moduleCode, permissionItemCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_IsModuleAuthorizedByUser, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return flag;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RemoveUserFromRole(BaseUserInfo userInfo, string userName, string roleName)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string id = new BaseUserManager(helper, userInfo).GetId(BaseUserEntity.FieldUserName, userName);
                        string str2 = new BaseRoleManager(helper, userInfo).GetId(BaseRoleEntity.FieldRealName, roleName);
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(str2))
                        {
                            num = new BaseUserRoleManager(helper, userInfo).RemoveFormRole(id, str2);
                        }
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokePermissionScopeTarget(BaseUserInfo userInfo, string resourceCategory, string[] resourceIds, string targetCategory, string revokeTargetId, string permissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BasePermissionScopeManager(helper, userInfo).RevokeResourcePermissionScopeTarget(resourceCategory, resourceIds, targetCategory, revokeTargetId, permissionItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokePermissionScopeTargets(BaseUserInfo userInfo, string resourceCategory, string resourceId, string targetCategory, string[] revokeTargetIds, string permissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BasePermissionScopeManager(helper, userInfo).RevokeResourcePermissionScopeTarget(resourceCategory, resourceId, targetCategory, revokeTargetIds, permissionItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokePermissionScopeTargets, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeResourcePermission(BaseUserInfo userInfo, string resourceCategory, string resourceId, string[] revokePermissionItemIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        if (revokePermissionItemIds != null)
                        {
                            BasePermissionManager manager = new BasePermissionManager(helper, userInfo);
                            string[] names = new string[3];
                            string[] values = new string[3];
                            names[0] = BasePermissionEntity.FieldResourceCategory;
                            values[0] = resourceCategory;
                            names[1] = BasePermissionEntity.FieldResourceId;
                            values[1] = resourceId;
                            names[2] = BasePermissionEntity.FieldPermissionItemId;
                            for (int i = 0; i < revokePermissionItemIds.Length; i++)
                            {
                                values[2] = revokePermissionItemIds[i];
                                num += manager.Delete(names, values);
                            }
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeResourcePermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeRoleModuleScope(BaseUserInfo userInfo, string roleId, string permissionItemCode, string revokeModuleId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (revokeModuleId != null)
                        {
                            num += manager.RevokeModule(roleId, permissionItemCode, revokeModuleId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeRoleModuleScope, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeRoleModuleScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] revokeModuleIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (revokeModuleIds != null)
                        {
                            num += manager.RevokeModules(roleId, permissionItemCode, revokeModuleIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeRoleModuleScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeRoleOrganizeScopes(BaseUserInfo userInfo, string roleId, string permissionItemId, string[] revokeOrganizeIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (revokeOrganizeIds != null)
                        {
                            num += manager.RevokeOrganizes(roleId, permissionItemId, revokeOrganizeIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeRoleOrganizeScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeRolePermission(BaseUserInfo userInfo, string roleName, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string id = new BaseRoleManager(helper, userInfo).GetId(BaseRoleEntity.FieldRealName, roleName);
                        string str2 = new BasePermissionItemManager(helper, userInfo).GetId(BasePermissionItemEntity.FieldCode, permissionItemCode);
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(str2))
                        {
                            num = new BaseRolePermissionManager(helper, userInfo).Revoke(id, str2);
                        }
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeRolePermissionById(BaseUserInfo userInfo, string roleId, string revokePermissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRolePermissionManager manager = new BaseRolePermissionManager(helper, userInfo);
                        if (revokePermissionItemId != null)
                        {
                            num += manager.Revoke(roleId, revokePermissionItemId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeRolePermissionById, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeRolePermissionItemScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] revokePermissionItemIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (revokePermissionItemIds != null)
                        {
                            num += manager.RevokePermissionItems(roleId, permissionItemCode, revokePermissionItemIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeRolePermissionItemScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeRolePermissions(BaseUserInfo userInfo, string[] roleIds, string[] revokePermissionItemIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRolePermissionManager manager = new BaseRolePermissionManager(helper, userInfo);
                        if ((roleIds != null) && (revokePermissionItemIds != null))
                        {
                            num += manager.Revoke(roleIds, revokePermissionItemIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeRolePermissions, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeRoleRoleScopes(BaseUserInfo userInfo, string roleId, string permissionItemId, string[] revokeRoleIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (revokeRoleIds != null)
                        {
                            num += manager.RevokeRoles(roleId, permissionItemId, revokeRoleIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeRoleRoleScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeRoleUserScopes(BaseUserInfo userInfo, string roleId, string permissionItemId, string[] revokeUserIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleScopeManager manager = new BaseRoleScopeManager(helper, userInfo);
                        if (revokeUserIds != null)
                        {
                            num += manager.RevokeUsers(roleId, permissionItemId, revokeUserIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeRoleUserScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeUserModuleScope(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string revokeModuleId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (revokeModuleId != null)
                        {
                            num += manager.RevokeModule(userId, permissionScopeItemCode, revokeModuleId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeUserModuleScope, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeUserModuleScopes(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string[] revokeModuleIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (revokeModuleIds != null)
                        {
                            num = manager.RevokeModules(userId, permissionScopeItemCode, revokeModuleIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeUserModuleScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeUserOrganizeScopes(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string[] revokeOrganizeIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (revokeOrganizeIds != null)
                        {
                            num += manager.RevokeOrganizes(userId, permissionScopeItemCode, revokeOrganizeIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeUserOrganizeScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeUserPermission(BaseUserInfo userInfo, string userName, string permissionItemCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string id = new BaseUserManager(helper, userInfo).GetId(BaseUserEntity.FieldUserName, userName);
                        string str2 = new BasePermissionItemManager(helper, userInfo).GetId(BasePermissionItemEntity.FieldCode, permissionItemCode);
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(str2))
                        {
                            num = new BaseUserPermissionManager(helper, userInfo).Revoke(id, str2);
                        }
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeUserPermissionById(BaseUserInfo userInfo, string userId, string revokePermissionItemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserPermissionManager manager = new BaseUserPermissionManager(helper, userInfo);
                        if (revokePermissionItemId != null)
                        {
                            num += manager.Revoke(userId, revokePermissionItemId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeUserPermissionById, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeUserPermissionItemScopes(BaseUserInfo userInfo, string userId, string permissionItemCode, string[] revokePermissionItemIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (revokePermissionItemIds != null)
                        {
                            num += manager.RevokePermissionItems(userId, permissionItemCode, revokePermissionItemIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeUserPermissionItemScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeUserPermissions(BaseUserInfo userInfo, string[] userIds, string[] revokePermissionItemIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserPermissionManager manager = new BaseUserPermissionManager(helper, userInfo);
                        if ((userIds != null) && (revokePermissionItemIds != null))
                        {
                            num += manager.Revoke(userIds, revokePermissionItemIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeUserPermission, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeUserRoleScopes(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string[] revokeRoleIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (revokeRoleIds != null)
                        {
                            num += manager.RevokeRoles(userId, permissionScopeItemCode, revokeRoleIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeUserRoleScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int RevokeUserUserScopes(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string[] revokeUserIds)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserScopeManager manager = new BaseUserScopeManager(helper, userInfo);
                        if (revokeUserIds != null)
                        {
                            num += manager.RevokeUsers(userId, permissionScopeItemCode, revokeUserIds);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionService_RevokeUserUserScopes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }
    }
}

