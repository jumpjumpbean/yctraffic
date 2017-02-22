namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IItemDetailsService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, string tableName, BaseItemDetailsEntity itemDetailsEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string tableName, string[] ids);
        [OperationContract]
        int BatchMoveTo(BaseUserInfo userInfo, string tableName, string[] ids, string targetId);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string tableName, string id);
        [OperationContract]
        int SetDeleted(BaseUserInfo userInfo, string tableName, string[] ids);
        [OperationContract]
        DataSet GetDSByCodes(BaseUserInfo userInfo, string[] codes);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo, string tableName);
        [OperationContract]
        DataTable GetDTByCode(BaseUserInfo userInfo, string code);
        [OperationContract]
        DataTable GetDTByParent(BaseUserInfo userInfo, string tableName, string parentId);
        [OperationContract]
        BaseItemDetailsEntity GetEntity(BaseUserInfo userInfo, string tableName, string id);
        [OperationContract]
        int Update(BaseUserInfo userInfo, string tableName, BaseItemDetailsEntity itemDetailsEntity, out string statusCode, out string statusMessage);
    }
}

