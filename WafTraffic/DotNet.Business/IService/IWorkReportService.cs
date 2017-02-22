namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IWorkReportService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, DataTable dataTable, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        DataTable BatchSave(BaseUserInfo userInfo, DataTable dataTable, int enabled, DateTime startDate, DateTime endDate);
        [OperationContract]
        bool BatchSetEnabled(BaseUserInfo userInfo, string[] ids, int enabled);
        [OperationContract]
        DateTime[] CheckWorkDate(BaseUserInfo userInfo, string staffId);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetDTByUser(BaseUserInfo userInfo, string staffId, string reportDate);
        [OperationContract]
        DataTable GetProjectDT(BaseUserInfo userInfo);
        [OperationContract]
        string GetProjectFullName(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable SearchAuditing(BaseUserInfo userInfo, DateTime startDate, DateTime endDate, int enabled);
        [OperationContract]
        double SumManHour(BaseUserInfo userInfo, string staffId, DateTime paramDate);
        [OperationContract]
        int Update(BaseUserInfo userInfo, DataTable dataTable, out string statusCode, out string statusMessage);
    }
}

