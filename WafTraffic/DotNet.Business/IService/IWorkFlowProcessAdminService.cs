namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IWorkFlowProcessAdminService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, BaseWorkFlowProcessEntity workFlowProcessEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetBillTemplateDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        BaseWorkFlowProcessEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        int SetDeleted(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Update(BaseUserInfo userInfo, BaseWorkFlowProcessEntity workFlowProcessEntity, out string statusCode, out string statusMessage);
    }
}

