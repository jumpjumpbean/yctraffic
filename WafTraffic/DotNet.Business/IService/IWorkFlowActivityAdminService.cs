namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.ServiceModel;

    [ServiceContract]
    public interface IWorkFlowActivityAdminService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, BaseWorkFlowActivityEntity workFlowActivityEntity);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo, string workFlowId);
    }
}

