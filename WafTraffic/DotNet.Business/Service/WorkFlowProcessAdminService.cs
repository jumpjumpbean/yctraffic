namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class WorkFlowProcessAdminService : MarshalByRefObject, IWorkFlowProcessAdminService
    {
        private string serviceName = AppMessage.WorkFlowProcessAdminService;
        private readonly string WorkFlowDbConnection = BaseSystemInfo.WorkFlowDbConnection;

        public string Add(BaseUserInfo userInfo, BaseWorkFlowProcessEntity workFlowProcessEntity, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        BaseWorkFlowProcessManager manager = new BaseWorkFlowProcessManager(helper, userInfo);
                        str = manager.Add(workFlowProcessEntity, out statusCode);
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
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        num = new BaseWorkFlowProcessManager(helper, userInfo).Delete((object[]) ids);
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
                        num = new BaseWorkFlowProcessManager(helper, userInfo).BatchSave(dataTable);
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

        public int Delete(BaseUserInfo userInfo, string id)
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
                        num = new BaseWorkFlowProcessManager(helper, userInfo).Delete(id);
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

        public DataTable GetBillTemplateDT(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(TemplateManager.TemplateTable);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        table = new TemplateManager(helper, userInfo).GetDT(BaseNewsEntity.FieldDeletionStateCode, 0, BaseNewsEntity.FieldSortCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
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

        public DataTable GetDT(BaseUserInfo userInfo)
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
                        table = new BaseWorkFlowProcessManager(helper, userInfo).GetDT(BaseWorkFlowProcessEntity.FieldDeletionStateCode, 0, BaseWorkFlowProcessEntity.FieldSortCode);
                        table.TableName = BaseWorkFlowProcessEntity.TableName;
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

        public BaseWorkFlowProcessEntity GetEntity(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseWorkFlowProcessEntity entity = new BaseWorkFlowProcessEntity();
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        entity = new BaseWorkFlowProcessManager(helper, userInfo).GetEntity(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
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

        public int SetDeleted(BaseUserInfo userInfo, string[] ids)
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
                        num = new BaseWorkFlowProcessManager(helper, userInfo).SetDeleted((object[]) ids);
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

        public int Update(BaseUserInfo userInfo, BaseWorkFlowProcessEntity workFlowProcessEntity, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        BaseWorkFlowProcessManager manager = new BaseWorkFlowProcessManager(helper, userInfo);
                        num = manager.Update(workFlowProcessEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
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
    }
}

