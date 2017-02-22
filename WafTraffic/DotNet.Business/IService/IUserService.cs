namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        string AddUser(BaseUserInfo userInfo, BaseUserEntity userEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        string AddUserToOrganize(BaseUserInfo userInfo, BaseUserOrganizeEntity userOrganizeEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        int AddUserToRole(BaseUserInfo userInfo, string userId, string[] addToRoleIds);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchDeleteUserOrganize(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        int BatchSetDefaultRole(BaseUserInfo userInfo, string[] userIds, string roleId);
        [OperationContract]
        int ClearUserRole(BaseUserInfo userInfo, string userId);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        bool Exists(BaseUserInfo userInfo, string[] fieldNames, object[] fieldValues);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetDTByDepartment(BaseUserInfo userInfo, string departmentId, bool containChildren);
        [OperationContract]
        DataTable GetDTByIds(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        DataTable GetDTByRole(BaseUserInfo userInfo, string roleId);
        [OperationContract]
        BaseUserEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetRoleDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetUserOrganizeDT(BaseUserInfo userInfo, string userId);
        [OperationContract]
        string[] GetUserRoleIds(BaseUserInfo userInfo, string userId);
        [OperationContract]
        int RemoveUserFromRole(BaseUserInfo userInfo, string userId, string[] removeRoleIds);
        [OperationContract]
        DataTable Search(BaseUserInfo userInfo, string userName, string auditStates, string[] roleIds);
        [OperationContract]
        int SetDefaultRole(BaseUserInfo userInfo, string userId, string roleId);
        [OperationContract]
        int SetDeleted(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int SetUserAuditStates(BaseUserInfo userInfo, string[] ids, AuditStatus auditStatus);
        [OperationContract]
        int UpdateUser(BaseUserInfo userInfo, BaseUserEntity userEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        bool UserInRole(BaseUserInfo userInfo, string userId, string roleCode);
        [OperationContract]
        int BatchMoveTo(BaseUserInfo userInfo, string[] ids, string organizeId);
    }
}

