namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    public interface IWorkFlowManager
    {
        int AfterAutoStatr(string id);
        int AuditQuash(string[] ids, string auditIdea);
        int AuditQuash(string id, string auditIdea);
        int AutoStatr(string[] ids, string auditIdea);
        string AutoStatr(string id, string auditIdea, DataTable dtWorkFlowActivity);
        string AutoStatr(string id, string auditIdea, string toUserId);
        WorkFlowInfo BeforeAutoStatr(string id);
        IDbHelper GetDbHelper();
        string GetUrl(string currentId);
        BaseUserInfo GetUserInfo();
        bool OnAuditComplete(string currentId, string categoryCode, string auditIdea);
        bool OnAuditQuash(string[] currentIds, string categoryCode, string auditIdea);
        bool OnAuditQuash(string currentId, string categoryCode, string auditIdea);
        bool OnAuditReject(string currentId, string categoryCode, string auditIdea);
        bool OnAuditReject(string[] currentIds, string categoryCode, string auditIdea);
        bool OnAutoAuditPass(string currentId, string categoryCode, string auditIdea);
        bool OnAutoAuditPass(string[] currentIds, string categoryCode, string auditIdea);
        int SendRemindMessage(BaseWorkFlowCurrentEntity workFlowCurrentEntity, AuditStatus auditStatus, string auditIdea, string[] userIds, string[] roleIds);
        void SetUserInfo(BaseUserInfo userInfo);
    }
}

