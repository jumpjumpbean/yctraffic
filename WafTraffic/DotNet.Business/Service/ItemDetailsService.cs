namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class ItemDetailsService : MarshalByRefObject, IItemDetailsService
    {
        private string serviceName = AppMessage.ItemDetailsService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string Add(BaseUserInfo userInfo, string tableName, BaseItemDetailsEntity itemDetailsEntity, out string statusCode, out string statusMessage)
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
                        BaseItemDetailsManager manager = new BaseItemDetailsManager(helper, userInfo, tableName);
                        str = manager.Add(itemDetailsEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_Add, MethodBase.GetCurrentMethod());
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
                        num = new BaseItemDetailsManager(helper, userInfo, tableName).Delete((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_BatchDelete, MethodBase.GetCurrentMethod());
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
                        BaseItemDetailsManager manager = new BaseItemDetailsManager(helper, userInfo, tableName);
                        for (int i = 0; i < ids.Length; i++)
                        {
                            num += manager.SetProperty(ids[i], BaseItemDetailsEntity.FieldParentId, targetId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_BatchMoveTo, MethodBase.GetCurrentMethod());
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
                        num = new BaseItemDetailsManager(helper, userInfo, dataTable.TableName).BatchSave(dataTable);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_BatchSave, MethodBase.GetCurrentMethod());
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
                        num = new BaseItemDetailsManager(helper, userInfo, tableName).Delete(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_Delete, MethodBase.GetCurrentMethod());
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

        /// <summary>
        /// 批量标示删除标志
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int SetDeleted(BaseUserInfo userInfo, string tableName, string[] ids)
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
                        num = new BaseItemDetailsManager(helper, userInfo, tableName).SetDeleted((object[])ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_SetDeleted, MethodBase.GetCurrentMethod());
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

        public DataSet GetDSByCodes(BaseUserInfo userInfo, string[] codes)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataSet set = new DataSet();
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        for (int i = 0; i < codes.Length; i++)
                        {
                            DataTable table = this.GetDTByCode(helper, userInfo, codes[i]);
                            table.TableName = codes[i];
                            set.Tables.Add(table);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_GetDSByCodes, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return set;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetDT(BaseUserInfo userInfo, string tableName)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(tableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseItemDetailsManager(helper, userInfo, tableName).GetDT(BaseItemDetailsEntity.FieldDeletionStateCode, 0, BaseItemDetailsEntity.FieldSortCode);
                        table.TableName = tableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_GetDT, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByCode(BaseUserInfo userInfo, string code)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseItemDetailsEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = this.GetDTByCode(helper, userInfo, code);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_GetDTByCode, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByCode(IDbHelper dbHelper, BaseUserInfo userInfo, string code)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseItemDetailsEntity.TableName);
            BaseItemDetailsManager manager = new BaseItemDetailsManager(dbHelper, userInfo);
            BaseItemsManager manager2 = new BaseItemsManager(dbHelper, userInfo);
            BaseItemsEntity entity = new BaseItemsEntity(manager2.GetDT(BaseItemsEntity.FieldCode, code));
            if (!string.IsNullOrEmpty(entity.TargetTable))
            {
                manager = new BaseItemDetailsManager(dbHelper, userInfo, entity.TargetTable);
            }
            table = manager.GetDT(BaseItemDetailsEntity.FieldDeletionStateCode, 0, BaseItemDetailsEntity.FieldEnabled, 1, BaseItemDetailsEntity.FieldSortCode);
            table.TableName = entity.TargetTable;
            return table;
        }

        public DataTable GetDTByParent(BaseUserInfo userInfo, string tableName, string parentId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dTByParent = new DataTable(tableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        dTByParent = new BaseItemDetailsManager(helper, userInfo, tableName).GetDTByParent(parentId);
                        dTByParent.TableName = tableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_GetDTByParent, MethodBase.GetCurrentMethod());
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

        public BaseItemDetailsEntity GetEntity(BaseUserInfo userInfo, string tableName, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseItemDetailsEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        entity = new BaseItemDetailsManager(helper, userInfo, tableName).GetEntity(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_GetEntity, MethodBase.GetCurrentMethod());
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

        public int Update(BaseUserInfo userInfo, string tableName, BaseItemDetailsEntity itemDetailsEntity, out string statusCode, out string statusMessage)
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
                        BaseItemDetailsManager manager = new BaseItemDetailsManager(helper, userInfo, tableName);
                        num = manager.Update(itemDetailsEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ItemDetailsService_Update, MethodBase.GetCurrentMethod());
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

