namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IModuleService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, BaseModuleEntity moduleEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchAddModules(BaseUserInfo userInfo, string permissionItemId, string[] moduleIds);
        [OperationContract]
        int BatchAddPermissions(BaseUserInfo userInfo, string moduleId, string[] permissionItemIds);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchDleteModules(BaseUserInfo userInfo, string permissionItemId, string[] modulesIds);
        [OperationContract]
        int BatchDletePermissions(BaseUserInfo userInfo, string moduleId, string[] permissionItemIds);
        [OperationContract]
        int BatchMoveTo(BaseUserInfo userInfo, string[] moduleIds, string parentId);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetDTByIds(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        DataTable GetDTByParent(BaseUserInfo userInfo, string parentId);
        [OperationContract]
        BaseModuleEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        string GetFullNameByCode(BaseUserInfo userInfo, string code);
        [OperationContract]
        string[] GetIdsByPermission(BaseUserInfo userInfo, string permissionItemId);
        [OperationContract]
        DataTable GetPermissionDT(BaseUserInfo userInfo, string moduleId);
        [OperationContract]
        int MoveTo(BaseUserInfo userInfo, string moduleId, string parentId);
        [OperationContract]
        int SetDeleted(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int SetSortCode(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Update(BaseUserInfo userInfo, BaseModuleEntity moduleEntity, out string statusCode, out string statusMessage);
    }
}

