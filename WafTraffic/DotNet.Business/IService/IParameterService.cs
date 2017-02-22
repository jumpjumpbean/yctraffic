namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.ServiceModel;

    [ServiceContract]
    public interface IParameterService
    {
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        int DeleteByParameter(BaseUserInfo userInfo, string categoryId, string parameterId);
        [OperationContract]
        int DeleteByParameterCode(BaseUserInfo userInfo, string categoryId, string parameterId, string parameterCode);
        [OperationContract]
        DataTable GetDTByParameter(BaseUserInfo userInfo, string categoryId, string parameterId);
        [OperationContract]
        DataTable GetDTParameterCode(BaseUserInfo userInfo, string categoryId, string parameterId, string parameterCode);
        [OperationContract]
        string GetParameter(BaseUserInfo userInfo, string categoryId, string parameterId, string parameterCode);
        [OperationContract]
        string GetServiceConfig(BaseUserInfo userInfo, string key);
        [OperationContract]
        int SetParameter(BaseUserInfo userInfo, string categoryId, string parameterId, string parameterCode, string parameterContent);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
    }
}

