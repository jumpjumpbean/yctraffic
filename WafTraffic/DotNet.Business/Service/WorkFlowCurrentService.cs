namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class WorkFlowCurrentService : MarshalByRefObject, IWorkFlowCurrentService
    {
        private string serviceName = AppMessage.WorkFlowCurrentService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;
        private readonly string WorkFlowDbConnection = BaseSystemInfo.WorkFlowDbConnection;

        public int AuditComplete(BaseUserInfo userInfo, string id, string auditIdea)
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
                        num = new BaseWorkFlowCurrentManager(helper, userInfo).AuditComplete(null, id, auditIdea);
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

        public int AuditQuash(BaseUserInfo userInfo, string currentWorkFlowId, string auditIdea)
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
                        num = new BaseWorkFlowCurrentManager(helper, userInfo).AuditQuash(currentWorkFlowId, auditIdea);
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

        public int AuditReject(BaseUserInfo userInfo, string id, string auditIdea)
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
                        num = new BaseWorkFlowCurrentManager(helper, userInfo).AuditReject(id, auditIdea, null);
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

        public int AutoAuditPass(BaseUserInfo userInfo, string flowId, string auditIdea)
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
                        IWorkFlowManager workFlowManager = new UserReportManager(userInfo);
                        BaseWorkFlowCurrentManager manager2 = new BaseWorkFlowCurrentManager(helper, userInfo);
                        helper.BeginTransaction();
                        num = manager2.AutoAuditPass(workFlowManager, flowId, auditIdea, null);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                        helper.CommitTransaction();
                    }
                    catch (Exception exception)
                    {
                        helper.RollbackTransaction();
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

        public string AutoStatr(BaseUserInfo userInfo, string categoryCode, string categoryFullName, string[] objectIds, string objectFullName, string workFlowCode, string auditIdea, out string returnStatusCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            returnStatusCode = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        IWorkFlowManager workFlowManager = new UserReportManager(userInfo);
                        BaseWorkFlowCurrentManager manager2 = new BaseWorkFlowCurrentManager(helper, userInfo);
                        helper.BeginTransaction();
                        for (int i = 0; i < objectIds.Length; i++)
                        {
                            str = manager2.AutoStatr(workFlowManager, categoryCode, categoryFullName, objectIds[i], objectFullName, workFlowCode, auditIdea, null);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                        helper.CommitTransaction();
                        if (!string.IsNullOrEmpty(str))
                        {
                            returnStatusCode = StatusCode.OK.ToString();
                        }
                    }
                    catch (Exception exception)
                    {
                        helper.RollbackTransaction();
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

        public DataTable GetAuditDetailDT(BaseUserInfo userInfo, string categoryId, string objectId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        string[] strArray = new BaseWorkFlowCurrentManager(helper, userInfo).GetIds(BaseWorkFlowCurrentEntity.FieldCategoryCode, categoryId, BaseWorkFlowCurrentEntity.FieldObjectId, objectId);
                        table = new BaseWorkFlowHistoryManager(helper, userInfo).GetDT(BaseWorkFlowHistoryTable.FieldCurrentFlowId, (object[]) strArray, BaseWorkFlowHistoryTable.FieldCreateOn);
                        table.TableName = BaseWorkFlowCurrentEntity.TableName;
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

        public string GetCurrentId(BaseUserInfo userInfo, string categoryId, string objectId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string currentId = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        currentId = new BaseWorkFlowCurrentManager(helper, userInfo).GetCurrentId(categoryId, objectId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return currentId;
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
            DataTable table = new DataTable(BaseWorkFlowCurrentEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        table = new BaseWorkFlowCurrentManager(helper, userInfo).GetDT(BaseWorkFlowCurrentEntity.FieldEnabled, 1, BaseWorkFlowCurrentEntity.FieldDeletionStateCode, 0, BaseWorkFlowCurrentEntity.FieldSendDate);
                        table.TableName = BaseWorkFlowCurrentEntity.TableName;
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

        public DataTable GetMonitorDT(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable monitorDT = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(helper, userInfo);
                        if (userInfo.IsAdministrator)
                        {
                            monitorDT = manager.GetDT(BaseWorkFlowCurrentEntity.FieldDeletionStateCode, 0, BaseWorkFlowCurrentEntity.FieldSendDate);
                        }
                        else
                        {
                            monitorDT = manager.GetMonitorDT(null, null);
                        }
                        monitorDT.TableName = BaseWorkFlowCurrentEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return monitorDT;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        //public DataTable GetMonitorPagedDT(BaseUserInfo userInfo, int pageSize, int currentPage, out int recordCount, string categoryCode = null, string searchValue = null) //C# 4.0 才支持缺省参数
        public DataTable GetMonitorPagedDT(BaseUserInfo userInfo, int pageSize, int currentPage, out int recordCount, string categoryCode, string searchValue) 
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(helper, userInfo);
                        if (userInfo.IsAdministrator)
                        {
                            table = manager.GetPagedDT(pageSize, currentPage, out recordCount, categoryCode, searchValue);
                        }
                        else
                        {
                            table = manager.GetMonitorPagedDT(pageSize, currentPage, out recordCount, categoryCode, searchValue);
                        }
                        table.TableName = BaseWorkFlowCurrentEntity.TableName;
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

        public DataTable GetWaitForAudit(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        new BaseUserRoleManager(helper).GetAllRoleIds(userInfo.Id);
                        helper.Close();
                        helper.Open(this.WorkFlowDbConnection);
                        table = new BaseWorkFlowCurrentManager(helper, userInfo).GetWaitForAudit(userInfo.Id, null, null);
                        table.TableName = BaseWorkFlowCurrentEntity.TableName;
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

        public int Replace(BaseUserInfo userInfo, string oldCode, string newCode)
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
                        num = new BaseWorkFlowCurrentManager(helper, userInfo).ReplaceUser(oldCode, newCode);
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

        public int RoleAuditPass(BaseUserInfo userInfo, string id, string sendToRoleId, string auditIdea)
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
                        num = new BaseWorkFlowCurrentManager(helper, userInfo).RoleAuditPass(id, sendToRoleId, auditIdea);
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

        public string RoleStatr(BaseUserInfo userInfo, string categoryId, string categoryFullName, string[] objectIds, string objectFullName, string sendToRoleId, string auditIdea, out string returnStatusCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            returnStatusCode = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(helper, userInfo);
                        helper.BeginTransaction();
                        for (int i = 0; i < objectIds.Length; i++)
                        {
                            str = manager.RoleStatr(null, categoryId, categoryFullName, objectIds[i], objectFullName, sendToRoleId, auditIdea, null, null);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                        helper.CommitTransaction();
                        returnStatusCode = StatusCode.OK.ToString();
                    }
                    catch (Exception exception)
                    {
                        helper.RollbackTransaction();
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

        public string StartAudit(BaseUserInfo userInfo, int flowId, string categoryId, string categoryFullName, string objectId, string objectFullName)
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
                        str = new BaseWorkFlowCurrentManager(helper, userInfo).StartAudit(flowId, categoryId, categoryFullName, objectId, objectFullName);
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

        public int TransmitRole(BaseUserInfo userInfo, string id, string sendToRoleId, string auditIdea)
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
                        num = new BaseWorkFlowCurrentManager(helper, userInfo).TransmitRole(id, sendToRoleId, auditIdea);
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

        public int TransmitUser(BaseUserInfo userInfo, string id, string sendToUserId, string auditIdea)
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
                        num = new BaseWorkFlowCurrentManager(helper, userInfo).TransmitUser(id, sendToUserId, auditIdea);
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

        public int UserAuditPass(BaseUserInfo userInfo, string id, string sendToUserId, string auditIdea)
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
                        num = new BaseWorkFlowCurrentManager(helper, userInfo).UserAuditPass(null, id, sendToUserId, auditIdea, null);
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

        public string UserStatr(BaseUserInfo userInfo, string categoryId, string categoryFullName, string[] objectIds, string objectFullName, string sendToUserId, string auditIdea, out string returnStatusCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            returnStatusCode = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.WorkFlowDbConnection);
                        BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(helper, userInfo);
                        helper.BeginTransaction();
                        for (int i = 0; i < objectIds.Length; i++)
                        {
                            str = manager.UserStatr(null, categoryId, categoryFullName, objectIds[i], objectFullName, sendToUserId, auditIdea, null, null);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                        helper.CommitTransaction();
                        returnStatusCode = StatusCode.OK.ToString();
                    }
                    catch (Exception exception)
                    {
                        helper.RollbackTransaction();
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
    }
}

