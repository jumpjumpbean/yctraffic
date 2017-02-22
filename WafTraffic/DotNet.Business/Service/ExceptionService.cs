namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class ExceptionService : MarshalByRefObject, IExceptionService
    {
        private string serviceName = AppMessage.ExceptionService;
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
                        num = new BaseExceptionManager(helper, userInfo).Delete(BaseExceptionEntity.FieldId, (object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ExceptionService_BatchDelete, MethodBase.GetCurrentMethod());
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

        public DataTable Delete(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dT = new DataTable(BaseExceptionEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseExceptionManager manager = new BaseExceptionManager(helper, userInfo);
                        manager.Delete(id);
                        dT = manager.GetDT();
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ExceptionService_Delete, MethodBase.GetCurrentMethod());
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

        public DataTable GetDT(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dT = new DataTable(BaseExceptionEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        dT = new BaseExceptionManager(helper, userInfo).GetDT();
                        dT.TableName = BaseExceptionEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ExceptionService_GetDT, MethodBase.GetCurrentMethod());
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

        public int Truncate(BaseUserInfo userInfo)
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
                        new BaseExceptionManager(helper, userInfo).Truncate();
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ExceptionService_Truncate, MethodBase.GetCurrentMethod());
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

