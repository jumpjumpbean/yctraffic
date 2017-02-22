namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IItemsService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, BaseItemsEntity itemsEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string tableName, string[] ids);
        [OperationContract]
        int BatchMoveTo(BaseUserInfo userInfo, string tableName, string[] ids, string targetId);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        void CreateTable(BaseUserInfo userInfo, string tableName, out string statusCode, out string statusMessage);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string tableName, string id);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetDTByParent(BaseUserInfo userInfo, string parentId);
        [OperationContract]
        BaseItemsEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        int Update(BaseUserInfo userInfo, BaseItemsEntity itemsEntity, out string statusCode, out string statusMessage);
    }
}

