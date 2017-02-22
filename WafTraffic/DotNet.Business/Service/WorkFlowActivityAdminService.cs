namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class WorkFlowActivityAdminService : MarshalByRefObject, IWorkFlowActivityAdminService
    {
        private string serviceName = AppMessage.WorkFlowActivityAdminService;
        private readonly string WorkFlowDbConnection = BaseSystemInfo.WorkFlowDbConnection;

        public string Add(BaseUserInfo userInfo, BaseWorkFlowActivityEntity workFlowActivityEntity)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        str = new BaseWorkFlowActivityManager(helper, userInfo).Add(workFlowActivityEntity);
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
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        num = new BaseWorkFlowActivityManager(helper, userInfo).Delete((object[]) ids);
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

        public int BatchSave(BaseUserInfo userInfo, DataTable dataTable)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        num = new BaseWorkFlowActivityManager(helper, userInfo).BatchSave(dataTable);
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

        public DataTable GetDT(BaseUserInfo userInfo, string workFlowId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseWorkFlowProcessEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        table = new BaseWorkFlowActivityManager(helper, userInfo).GetDT(BaseWorkFlowActivityEntity.FieldWorkFlowId, workFlowId, BaseWorkFlowActivityEntity.FieldDeletionStateCode, 0, BaseWorkFlowActivityEntity.FieldSortCode);
                        table.TableName = BaseWorkFlowActivityEntity.TableName;
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
    }
}

