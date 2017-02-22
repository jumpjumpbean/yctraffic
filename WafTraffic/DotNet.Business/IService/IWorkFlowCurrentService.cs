namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IWorkFlowCurrentService
    {
        [OperationContract]
        int AuditComplete(BaseUserInfo userInfo, string id, string auditIdea);
        [OperationContract]
        int AuditQuash(BaseUserInfo userInfo, string currentWorkFlowId, string auditIdea);
        [OperationContract]
        int AuditReject(BaseUserInfo userInfo, string id, string auditIdea);
        [OperationContract]
        int AutoAuditPass(BaseUserInfo userInfo, string flowId, string auditIdea);
        [OperationContract]
        string AutoStatr(BaseUserInfo userInfo, string categoryCode, string categoryFullName, string[] objectIds, string objectFullName, string workFlowCode, string auditIdea, out string returnStatusCode);
        [OperationContract]
        DataTable GetAuditDetailDT(BaseUserInfo userInfo, string categoryId, string objectId);
        [OperationContract]
        string GetCurrentId(BaseUserInfo userInfo, string categoryId, string objectId);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetMonitorDT(BaseUserInfo userInfo);
        [OperationContract]
        //DataTable GetMonitorPagedDT(BaseUserInfo userInfo, int pageSize, int currentPage, out int recordCount, string categoryCode = null, string searchValue = null);//C# 4.0 才支持缺省参数
        DataTable GetMonitorPagedDT(BaseUserInfo userInfo, int pageSize, int currentPage, out int recordCount, string categoryCode, string searchValue);
        [OperationContract]
        DataTable GetWaitForAudit(BaseUserInfo userInfo);
        [OperationContract]
        int Replace(BaseUserInfo userInfo, string oldCode, string newCode);
        [OperationContract]
        int RoleAuditPass(BaseUserInfo userInfo, string id, string sendToRoleId, string auditIdea);
        [OperationContract]
        string RoleStatr(BaseUserInfo userInfo, string categoryId, string categoryFullName, string[] objectIds, string objectFullName, string sendToRoleId, string auditIdea, out string returnStatusCode);
        [OperationContract]
        string StartAudit(BaseUserInfo userInfo, int flowId, string categoryId, string categoryFullName, string objectId, string objectFullName);
        [OperationContract]
        int TransmitRole(BaseUserInfo userInfo, string id, string sendToRoleId, string auditIdea);
        [OperationContract]
        int TransmitUser(BaseUserInfo userInfo, string id, string sendToUserId, string auditIdea);
        [OperationContract]
        int UserAuditPass(BaseUserInfo userInfo, string id, string sendToUserId, string auditIdea);
        [OperationContract]
        string UserStatr(BaseUserInfo userInfo, string categoryId, string categoryFullName, string[] objectIds, string objectFullName, string sendToUserId, string auditIdea, out string returnStatusCode);
    }
}

