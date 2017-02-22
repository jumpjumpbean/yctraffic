namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IPermissionItemService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, BasePermissionItemEntity permissionItemEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        string AddByDetail(BaseUserInfo userInfo, string code, string fullName, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchMoveTo(BaseUserInfo userInfo, string[] ids, string parentId);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        int BatchSetSortCode(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetDTByParent(BaseUserInfo userInfo, string parentId);
        [OperationContract]
        BasePermissionItemEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        BasePermissionItemEntity GetEntityByCode(BaseUserInfo userInfo, string code);
        [OperationContract]
        string[] GetIdsByModule(BaseUserInfo userInfo, string moduleId);
        [OperationContract]
        int MoveTo(BaseUserInfo userInfo, string id, string parentId);
        [OperationContract]
        int SetDeleted(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Update(BaseUserInfo userInfo, BasePermissionItemEntity permissionItemEntity, out string statusCode, out string statusMessage);
    }
}

