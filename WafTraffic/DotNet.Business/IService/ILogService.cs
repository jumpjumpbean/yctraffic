namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.ServiceModel;

    [ServiceContract]
    public interface ILogService
    {
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchDeleteApplication(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetDTApplicationByDate(BaseUserInfo userInfo, string beginDate, string endDate);
        [OperationContract]
        DataTable GetDTByDate(BaseUserInfo userInfo, string beginDate, string endDate, string userId, string moduleId);
        [OperationContract]
        DataTable GetDTByModule(BaseUserInfo userInfo, string moduleId, string beginDate, string endDate);
        [OperationContract]
        DataTable GetDTByUser(BaseUserInfo userInfo, string userId, string beginDate, string endDate);
        [OperationContract]
        DataTable GetLogGeneral(BaseUserInfo userInfo);
        [OperationContract]
        DataTable ResetVisitInfo(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        void Truncate(BaseUserInfo userInfo);
        [OperationContract]
        void TruncateApplication(BaseUserInfo userInfo);
        [OperationContract(IsOneWay=true)]
        void WriteExit(BaseUserInfo userInfo, string logId);
        [OperationContract(IsOneWay=true)]
        void WriteLog(BaseUserInfo userInfo, string processId, string processName, string methodId, string methodName);
    }
}

