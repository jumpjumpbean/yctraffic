namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.ServiceModel;

    [ServiceContract]
    public interface IExceptionService
    {
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        DataTable Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        int Truncate(BaseUserInfo userInfo);
    }
}

