namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class ItemsService : MarshalByRefObject, IItemsService
    {
        private string serviceName = AppMessage.ItemsService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string Add(BaseUserInfo userInfo, BaseItemsEntity itemsEntity, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            statusCode = string.Empty;
            statusMessage = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseItemsManager manager = new BaseItemsManager(helper, userInfo);
                        str = manager.Add(itemsEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemsService_Add, MethodBase.GetCurrentMethod());
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

        public int BatchDelete(BaseUserInfo userInfo, string tableName, string[] ids)
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
                        num = new BaseItemsManager(helper, userInfo, tableName).Delete((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemsService_BatchDelete, MethodBase.GetCurrentMethod());
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

        public int BatchMoveTo(BaseUserInfo userInfo, string tableName, string[] ids, string targetId)
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
                        BaseItemsManager manager = new BaseItemsManager(helper, userInfo, tableName);
                        for (int i = 0; i < ids.Length; i++)
                        {
                            num += manager.SetProperty(ids[i], BaseItemsEntity.FieldParentId, targetId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemsService_BatchMoveTo, MethodBase.GetCurrentMethod());
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
                        num = new BaseItemsManager(helper, userInfo, dataTable.TableName).BatchSave(dataTable);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemsService_BatchSave, MethodBase.GetCurrentMethod());
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

        public void CreateTable(BaseUserInfo userInfo, string tableName, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    helper.Open(this.UserCenterDbConnection);
                    BaseItemsManager manager = new BaseItemsManager(helper, userInfo);
                    manager.CreateTable(tableName, out statusCode);
                    statusMessage = manager.GetStateMessage(statusCode);
                    BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemsService_CreateTable, MethodBase.GetCurrentMethod());
                }
                catch (Exception exception)
                {
                    BaseExceptionManager.LogException(helper, userInfo, exception);
                    throw exception;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int Delete(BaseUserInfo userInfo, string tableName, string id)
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
                        num = new BaseItemsManager(helper, userInfo, tableName).Delete(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemsService_Delete, MethodBase.GetCurrentMethod());
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
            DataTable dT = new DataTable(BaseItemsEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseItemsManager manager = new BaseItemsManager(helper, userInfo);
                        if (userInfo.IsAdministrator)
                        {
                            dT = manager.GetDT();
                        }
                        else
                        {
                            string[] ids = new BasePermissionScopeManager(helper, userInfo).GetResourceScopeIds(userInfo.Id, BaseItemsEntity.TableName, "Resource.ManagePermission");
                            dT = manager.GetDT(ids);
                        }
                        dT.TableName = BaseItemsEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemsService_GetDT, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByParent(BaseUserInfo userInfo, string parentId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dTByParent = new DataTable(BaseItemsEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        dTByParent = new BaseItemsManager(helper, userInfo).GetDTByParent(parentId);
                        dTByParent.TableName = BaseItemsEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemsService_GetDTByParent, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dTByParent;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public BaseItemsEntity GetEntity(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseItemsEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        entity = new BaseItemsManager(helper, userInfo).GetEntity(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemsService_GetEntity, MethodBase.GetCurrentMethod());
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

        public int Update(BaseUserInfo userInfo, BaseItemsEntity itemsEntity, out string statusCode, out string statusMessage)
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
                        BaseItemsManager manager = new BaseItemsManager(helper, userInfo);
                        num = manager.Update(itemsEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemsService_Update, MethodBase.GetCurrentMethod());
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

