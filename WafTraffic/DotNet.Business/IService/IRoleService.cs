namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IRoleService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, BaseRoleEntity roleEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        int AddUserToRole(BaseUserInfo userInfo, string roleId, string[] addUserIds);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchMoveTo(BaseUserInfo userInfo, string[] ids, string targetId);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, List<BaseRoleEntity> roleEntites);
        [OperationContract]
        int ClearRoleUser(BaseUserInfo userInfo, string roleId);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetDTByIds(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        DataTable GetDTByOrganize(BaseUserInfo userInfo, string organizeId);
        [OperationContract]
        BaseRoleEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        string[] GetRoleUserIds(BaseUserInfo userInfo, string roleId);
        [OperationContract]
        int MoveTo(BaseUserInfo userInfo, string id, string targetId);
        [OperationContract]
        int RemoveUserFromRole(BaseUserInfo userInfo, string roleId, string[] userIds);
        [OperationContract]
        int ResetSortCode(BaseUserInfo userInfo, string organizeId);
        [OperationContract]
        DataTable Search(BaseUserInfo userInfo, string organizeId, string searchValue);
        [OperationContract]
        int SetDeleted(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Update(BaseUserInfo userInfo, BaseRoleEntity roleEntity, out string statusCode, out string statusMessage);
    }
}

