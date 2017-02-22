namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class LogService : MarshalByRefObject, ILogService
    {
        private readonly string BusinessDbConnection = BaseSystemInfo.BusinessDbConnection;
        private string serviceName = AppMessage.LogService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

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
                        num = new BaseLogManager(helper, userInfo).Delete(BaseLogEntity.FieldId, (object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_BatchDelete, MethodBase.GetCurrentMethod());
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

        public int BatchDeleteApplication(BaseUserInfo userInfo, string[] ids)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.BusinessDbConnection);
                        num = new BaseLogManager(helper, userInfo).Delete(BaseLogEntity.FieldId, (object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_BatchDeleteApplication, MethodBase.GetCurrentMethod());
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
                        num = new BaseLogManager(helper, userInfo).Delete(BaseLogEntity.FieldId, id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_Delete, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTApplicationByDate(BaseUserInfo userInfo, string beginDate, string endDate)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseLogEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.BusinessDbConnection);
                        table = new BaseLogManager(helper, userInfo).GetDTByDate(string.Empty, string.Empty, beginDate, endDate);
                        table.TableName = BaseLogEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_GetDTApplicationByDate, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByDate(BaseUserInfo userInfo, string beginDate, string endDate, string userId, string moduleId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseLogEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseLogManager manager = new BaseLogManager(helper, userInfo);
                        if (!string.IsNullOrEmpty(userId))
                        {
                            table = manager.GetDTByDateByUserIds(new string[] { userId }, BaseLogEntity.FieldProcessId, moduleId, beginDate, endDate);
                        }
                        else if (userInfo.IsAdministrator)
                        {
                            table = manager.GetDTByDate(BaseLogEntity.FieldProcessId, moduleId, beginDate, endDate);
                        }
                        else
                        {
                            string[] userIds = new BasePermissionScopeManager(helper, userInfo).GetUserIds(userInfo.Id, "Resource.ManagePermission");
                            table = manager.GetDTByDateByUserIds(userIds, BaseLogEntity.FieldProcessId, moduleId, beginDate, endDate);
                        }
                        table.TableName = BaseLogEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_GetDTByDate, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByModule(BaseUserInfo userInfo, string processId, string beginDate, string endDate)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseLogEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseLogManager manager = new BaseLogManager(helper, userInfo);
                        if (userInfo.IsAdministrator)
                        {
                            table = manager.GetDTByDate(BaseLogEntity.FieldProcessId, processId, beginDate, endDate);
                        }
                        else
                        {
                            string[] userIds = new BasePermissionScopeManager(helper, userInfo).GetUserIds(userInfo.Id, "Resource.ManagePermission");
                            table = manager.GetDTByDateByUserIds(userIds, BaseLogEntity.FieldProcessId, processId, beginDate, endDate);
                        }
                        table.TableName = BaseLogEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_GetDTByModule, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByUser(BaseUserInfo userInfo, string userId, string beginDate, string endDate)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseLogEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseLogManager(helper, userInfo).GetDTByDate(BaseLogEntity.FieldUserId, userId, beginDate, endDate);
                        table.TableName = BaseLogEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_GetDTByUser, MethodBase.GetCurrentMethod());
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

        public DataTable GetLogGeneral(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dT = new DataTable(BaseLogEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                BaseUserManager manager = new BaseUserManager(helper, userInfo);
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        if (userInfo.IsAdministrator)
                        {
                            dT = manager.GetDT();
                        }
                        else
                        {
                            string[] userIds = new BasePermissionScopeManager(helper, userInfo).GetUserIds(userInfo.Id, "Resource.ManagePermission");
                            dT = manager.GetDTByIds(userIds);
                        }
                        dT.TableName = BaseLogEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_GetLogGeneral, MethodBase.GetCurrentMethod());
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

        public DataTable ResetVisitInfo(BaseUserInfo userInfo, string[] ids)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dT = new DataTable(BaseLogEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        helper.BeginTransaction();
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        manager.ResetVisitInfo(ids);
                        dT = manager.GetDT();
                        dT.TableName = BaseLogEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_ResetVisitInfo, MethodBase.GetCurrentMethod());
                        helper.CommitTransaction();
                    }
                    catch (Exception exception)
                    {
                        helper.RollbackTransaction();
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

        public void Truncate(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    helper.Open(this.UserCenterDbConnection);
                    new BaseLogManager(helper, userInfo).Truncate();
                    BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_Truncate, MethodBase.GetCurrentMethod());
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

        public void TruncateApplication(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    helper.Open(this.BusinessDbConnection);
                    new BaseLogManager(helper, userInfo).Truncate();
                    BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogService_TruncateApplication, MethodBase.GetCurrentMethod());
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

        public void WriteExit(BaseUserInfo userInfo, string logId)
        {
            LogOnService.UserIsLogOn(userInfo);
            if (!string.IsNullOrEmpty(logId))
            {
                using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        SQLBuilder builder = new SQLBuilder(helper);
                        builder.BeginUpdate(BaseLogEntity.TableName);
                        builder.SetDBNow(BaseLogEntity.FieldModifiedOn);
                        builder.SetWhere(BaseLogEntity.FieldId, logId);
                        builder.EndUpdate();
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
        }

        public void WriteLog(BaseUserInfo userInfo, string processId, string processName, string methodId, string methodName)
        {
            LogOnService.UserIsLogOn(userInfo);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    helper.Open(this.UserCenterDbConnection);
                    BaseLogManager.Instance.Add(helper, userInfo, processName, methodName, processId, methodId, string.Empty);
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
    }
}

