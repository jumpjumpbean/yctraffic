namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class PermissionItemService : MarshalByRefObject, IPermissionItemService
    {
        private string serviceName = AppMessage.PermissionItemService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string Add(BaseUserInfo userInfo, BasePermissionItemEntity permissionItemEntity, out string statusCode, out string statusMessage)
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
                        BasePermissionItemManager manager = new BasePermissionItemManager(helper, userInfo);
                        str = manager.Add(permissionItemEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_Add, MethodBase.GetCurrentMethod());
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

        public string AddByDetail(BaseUserInfo userInfo, string code, string fullName, out string statusCode, out string statusMessage)
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
                        BasePermissionItemManager manager = new BasePermissionItemManager(helper, userInfo);
                        str = manager.AddByDetail(code, fullName, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
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
                        num = new BasePermissionItemManager(helper, userInfo).Delete((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_BatchDelete, MethodBase.GetCurrentMethod());
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

        public int BatchMoveTo(BaseUserInfo userInfo, string[] permissionItemIds, string parentId)
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
                        for (int i = 0; i < permissionItemIds.Length; i++)
                        {
                            num += manager.MoveTo(permissionItemIds[i], parentId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_BatchMoveTo, MethodBase.GetCurrentMethod());
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
                        num = new BasePermissionItemManager(helper, userInfo).BatchSave(dataTable);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_BatchSave, MethodBase.GetCurrentMethod());
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

        public int BatchSetSortCode(BaseUserInfo userInfo, string[] ids)
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
                        num = new BasePermissionItemManager(helper, userInfo).BatchSetSortCode(ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_BatchSetSortCode, MethodBase.GetCurrentMethod());
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
                        num = new BasePermissionItemManager(helper, userInfo).Delete(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_Delete, MethodBase.GetCurrentMethod());
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
            DataTable table = new DataTable(BasePermissionItemEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BasePermissionItemManager(helper, userInfo).GetDT(BasePermissionItemEntity.FieldDeletionStateCode, 0, BasePermissionItemEntity.FieldSortCode);
                        table.TableName = BasePermissionItemEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_GetDT, MethodBase.GetCurrentMethod());
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
            DataTable table = new DataTable(BasePermissionItemEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BasePermissionItemManager(helper, userInfo).GetDT(BasePermissionItemEntity.FieldParentId, parentId, BasePermissionItemEntity.FieldDeletionStateCode, 0, BasePermissionItemEntity.FieldSortCode);
                        table.TableName = BasePermissionItemEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_GetDTByParent, MethodBase.GetCurrentMethod());
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

        public BasePermissionItemEntity GetEntity(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            BasePermissionItemEntity entity = new BasePermissionItemEntity();
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BasePermissionItemManager manager = new BasePermissionItemManager(helper, userInfo);
                        DataTable table = new DataTable(BasePermissionItemEntity.TableName);
                        entity = new BasePermissionItemEntity(manager.GetDTById(id));
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_GetEntity, MethodBase.GetCurrentMethod());
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

        public BasePermissionItemEntity GetEntityByCode(BaseUserInfo userInfo, string code)
        {
            LogOnService.UserIsLogOn(userInfo);
            BasePermissionItemEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BasePermissionItemManager manager = new BasePermissionItemManager(helper, userInfo);
                        entity = new BasePermissionItemEntity(manager.GetDTByCode(code));
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_GetEntityByCode, MethodBase.GetCurrentMethod());
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

        public string[] GetIdsByModule(BaseUserInfo userInfo, string moduleId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] permissionIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        permissionIds = new BasePermissionModuleManager(helper, userInfo).GetPermissionIds(moduleId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_GetIdsByModule, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return permissionIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetLicensedDT(BaseUserInfo userInfo, string userId)
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
                        string permissionItemCode = manager.GetId(BasePermissionItemEntity.FieldDeletionStateCode, 0, BasePermissionItemEntity.FieldCode, "Resource.ManagePermission");
                        dTByUser = manager.GetDTByUser(userId, permissionItemCode);
                        dTByUser.TableName = BasePermissionItemEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_GetLicensedDT, MethodBase.GetCurrentMethod());
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

        public int MoveTo(BaseUserInfo userInfo, string permissionItemId, string parentId)
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
                        num = new BasePermissionItemManager(helper, userInfo).MoveTo(permissionItemId, parentId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_MoveTo, MethodBase.GetCurrentMethod());
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
                        num = new BasePermissionItemManager(helper, userInfo).SetDeleted((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_SetDeleted, MethodBase.GetCurrentMethod());
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

        public int Update(BaseUserInfo userInfo, BasePermissionItemEntity permissionItemEntity, out string statusCode, out string statusMessage)
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
                        BasePermissionItemManager manager = new BasePermissionItemManager(helper, userInfo);
                        num = manager.Update(permissionItemEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.PermissionItemService_Update, MethodBase.GetCurrentMethod());
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

