namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class ModuleService : MarshalByRefObject, IModuleService
    {
        private string serviceName = AppMessage.ModuleService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string Add(BaseUserInfo userInfo, BaseModuleEntity moduleEntity, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseModuleManager manager = new BaseModuleManager(helper, userInfo);
                        str = manager.Add(moduleEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_Add, MethodBase.GetCurrentMethod());
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

        public int BatchAddModules(BaseUserInfo userInfo, string permissionItemId, string[] moduleIds)
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
                        num = new BasePermissionModuleManager(helper, userInfo).Add(moduleIds, permissionItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_BatchAddModules, MethodBase.GetCurrentMethod());
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

        public int BatchAddPermissions(BaseUserInfo userInfo, string moduleId, string[] permissionItemIds)
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
                        num = new BasePermissionModuleManager(helper, userInfo).Add(moduleId, permissionItemIds);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_BatchAddPermissions, MethodBase.GetCurrentMethod());
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

        public int BatchDelete(BaseUserInfo userInfo, string[] ids)
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
                        num = new BaseModuleManager(helper, userInfo).Delete((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_BatchDelete, MethodBase.GetCurrentMethod());
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

        public int BatchDleteModules(BaseUserInfo userInfo, string permissionItemId, string[] modulesIds)
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
                        num = new BasePermissionModuleManager(helper, userInfo).Delete(modulesIds, permissionItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_BatchDleteModules, MethodBase.GetCurrentMethod());
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

        public int BatchDletePermissions(BaseUserInfo userInfo, string moduleId, string[] permissionItemIds)
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
                        num = new BasePermissionModuleManager(helper, userInfo).Delete(moduleId, permissionItemIds);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_BatchDletePermissions, MethodBase.GetCurrentMethod());
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

        public int BatchMoveTo(BaseUserInfo userInfo, string[] moduleIds, string parentId)
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
                        BaseModuleManager manager = new BaseModuleManager(helper, userInfo);
                        for (int i = 0; i < moduleIds.Length; i++)
                        {
                            num += manager.SetProperty(moduleIds[i], BaseModuleEntity.FieldParentId, parentId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_BatchMoveTo, MethodBase.GetCurrentMethod());
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

        public int BatchSave(BaseUserInfo userInfo, DataTable dataTable)
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
                        num = new BaseModuleManager(helper, userInfo).BatchSave(dataTable);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_BatchSave, MethodBase.GetCurrentMethod());
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

        public int Delete(BaseUserInfo userInfo, string id)
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
                        num = new BaseModuleManager(helper, userInfo).Delete(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_Delete, MethodBase.GetCurrentMethod());
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

        public DataTable GetDT(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseModuleEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseModuleManager(helper, userInfo).GetDT(BaseModuleEntity.FieldDeletionStateCode, 0, BaseModuleEntity.FieldSortCode);
                        table.DefaultView.Sort = BaseModuleEntity.FieldSortCode;
                        table.TableName = BaseModuleEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_GetDT, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return table;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetDTByIds(BaseUserInfo userInfo, string[] ids)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseRoleEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseModuleManager(helper, userInfo).GetDT(BaseModuleEntity.FieldId, (object[]) ids, BaseModuleEntity.FieldSortCode);
                        table.TableName = BaseModuleEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_GetDTByIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return table;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetDTByParent(BaseUserInfo userInfo, string parentId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseModuleEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseModuleManager(helper, userInfo).GetDT(BaseModuleEntity.FieldDeletionStateCode, 0, BaseModuleEntity.FieldParentId, parentId);
                        table.DefaultView.Sort = BaseModuleEntity.FieldSortCode;
                        table.TableName = BaseModuleEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_GetDTByParent, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return table;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public BaseModuleEntity GetEntity(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseModuleEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        entity = new BaseModuleManager(helper, userInfo).GetEntity(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_GetEntity, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return entity;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string GetFullNameByCode(BaseUserInfo userInfo, string code)
        {
            LogOnService.UserIsLogOn(userInfo);
            string fullNameByCode = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        fullNameByCode = new BaseModuleManager(helper, userInfo).GetFullNameByCode(code);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_GetFullNameByCode, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return fullNameByCode;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string[] GetIdsByPermission(BaseUserInfo userInfo, string permissionItemId)
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
                        moduleIds = new BasePermissionModuleManager(helper, userInfo).GetModuleIds(permissionItemId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_GetIdsByPermission, MethodBase.GetCurrentMethod());
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

        public DataTable GetPermissionDT(BaseUserInfo userInfo, string moduleId)
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
                        string[] permissionIds = new BasePermissionModuleManager(helper, userInfo).GetPermissionIds(moduleId);
                        dT = new BasePermissionItemManager(helper, userInfo).GetDT(permissionIds);
                        dT.TableName = BasePermissionItemEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_GetPermissionDT, MethodBase.GetCurrentMethod());
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

        public int MoveTo(BaseUserInfo userInfo, string moduleId, string parentId)
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
                        num = new BaseModuleManager(helper, userInfo).SetProperty(moduleId, BaseModuleEntity.FieldParentId, parentId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_MoveTo, MethodBase.GetCurrentMethod());
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

        public int SetDeleted(BaseUserInfo userInfo, string[] ids)
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
                        num = new BaseModuleManager(helper, userInfo).SetDeleted((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_SetDeleted, MethodBase.GetCurrentMethod());
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

        public int SetSortCode(BaseUserInfo userInfo, string[] ids)
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
                        if (userInfo.IsAdministrator)
                        {
                            num = new BaseModuleManager(helper, userInfo).BatchSetSortCode(ids);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_SetSortCode, MethodBase.GetCurrentMethod());
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

        public int Update(BaseUserInfo userInfo, BaseModuleEntity moduleEntity, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseModuleManager manager = new BaseModuleManager(helper, userInfo);
                        num = manager.Update(moduleEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ModuleService_Update, MethodBase.GetCurrentMethod());
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

