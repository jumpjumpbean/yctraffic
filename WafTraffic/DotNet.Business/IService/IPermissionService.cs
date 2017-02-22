namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.ServiceModel;

    [ServiceContract]
    public interface IPermissionService
    {
        [OperationContract]
        int ClearPermissionScopeTarget(BaseUserInfo userInfo, string resourceCategory, string resourceId, string targetCategory, string permissionItemId);
        [OperationContract]
        int ClearRolePermission(BaseUserInfo userInfo, string id);
        [OperationContract]
        int ClearRolePermissionScope(BaseUserInfo userInfo, string id, string permissionItemCode);
        [OperationContract]
        int ClearUserPermission(BaseUserInfo userInfo, string id);
        [OperationContract]
        int ClearUserPermissionScope(BaseUserInfo userInfo, string id, string permissionItemCode);
        [OperationContract]
        DataTable GetModuleDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetModuleDTByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        DataTable GetModuleDTByUser(BaseUserInfo userInfo, string userId);
        [OperationContract]
        string[] GetModuleIdsByUser(BaseUserInfo userInfo, string userId);
        [OperationContract]
        DataTable GetOrganizeDTByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetOrganizeIdsByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        DataTable GetPermissionDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetPermissionDTByUser(BaseUserInfo userInfo, string userId);
        [OperationContract]
        DataTable GetPermissionItemDTByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetPermissionScopeResourceIds(BaseUserInfo userInfo, string resourceCategory, string targetResourceId, string targetResourceCategory, string permissionItemCode);
        [OperationContract]
        string[] GetPermissionScopeTargetIds(BaseUserInfo userInfo, string resourceCategory, string resourceId, string targetCategory, string permissionItemCode);
        [OperationContract]
        string[] GetResourcePermissionItemIds(BaseUserInfo userInfo, string resourceCategory, string resourceId);
        [OperationContract]
        string[] GetResourceScopeIds(BaseUserInfo userInfo, string userId, string targetCategory, string permissionItemCode);
        [OperationContract]
        DataTable GetRoleDTByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetRoleIdsByPermission(BaseUserInfo userInfo, string permissionItemId);
        [OperationContract]
        string[] GetRoleIdsByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetRolePermissionItemIds(BaseUserInfo userInfo, string roleId);
        [OperationContract]
        string[] GetRoleScopeModuleIds(BaseUserInfo userInfo, string roleId, string permissionItemCode);
        [OperationContract]
        string[] GetRoleScopeOrganizeIds(BaseUserInfo userInfo, string roleId, string permissionItemCode);
        [OperationContract]
        string[] GetRoleScopePermissionItemIds(BaseUserInfo userInfo, string roleId, string permissionItemCode);
        [OperationContract]
        string[] GetRoleScopeRoleIds(BaseUserInfo userInfo, string roleId, string permissionItemCode);
        [OperationContract]
        string[] GetRoleScopeUserIds(BaseUserInfo userInfo, string roleId, string permissionItemCode);
        [OperationContract]
        string[] GetTreeResourceScopeIds(BaseUserInfo userInfo, string userId, string targetCategory, string permissionItemCode, bool childrens);
        [OperationContract]
        DataTable GetUserDTByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetUserIdsByPermission(BaseUserInfo userInfo, string permissionItemId);
        [OperationContract]
        string[] GetUserIdsByPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetUserPermissionItemIds(BaseUserInfo userInfo, string userId);
        [OperationContract]
        PermissionScope GetUserPermissionScope(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetUserScopeModuleIds(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetUserScopeOrganizeIds(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetUserScopePermissionItemIds(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetUserScopeRoleIds(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        string[] GetUserScopeUserIds(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        int GrantPermissionScopeTarget(BaseUserInfo userInfo, string resourceCategory, string[] resourceIds, string targetCategory, string grantTargetId, string permissionItemId);
        [OperationContract]
        int GrantPermissionScopeTargets(BaseUserInfo userInfo, string resourceCategory, string resourceId, string targetCategory, string[] grantTargetIds, string permissionItemId);
        [OperationContract]
        int GrantResourcePermission(BaseUserInfo userInfo, string resourceCategory, string resourceId, string[] grantPermissionItemIds);
        [OperationContract]
        string GrantRoleModuleScope(BaseUserInfo userInfo, string roleId, string permissionItemCode, string grantModuleId);
        [OperationContract]
        int GrantRoleModuleScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] grantModuleIds);
        [OperationContract]
        int GrantRoleOrganizeScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] grantOrganizeIds);
        [OperationContract]
        string GrantRolePermission(BaseUserInfo userInfo, string roleId, string grantPermissionItemId);
        [OperationContract]
        string GrantRolePermissionById(BaseUserInfo userInfo, string roleId, string grantPermissionItemId);
        [OperationContract]
        int GrantRolePermissionItemScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] grantPermissionItemIds);
        [OperationContract]
        int GrantRolePermissions(BaseUserInfo userInfo, string[] roleIds, string[] grantPermissionItemIds);
        [OperationContract]
        int GrantRoleRoleScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] grantRoleIds);
        [OperationContract]
        int GrantRoleUserScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] grantUserIds);
        [OperationContract]
        string GrantUserModuleScope(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string grantModuleId);
        [OperationContract]
        int GrantUserModuleScopes(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string[] grantModuleIds);
        [OperationContract]
        int GrantUserOrganizeScopes(BaseUserInfo userInfo, string userId, string permissionItemCode, string[] grantOrganizeIds);
        [OperationContract]
        string GrantUserPermissionById(BaseUserInfo userInfo, string userId, string grantPermissionItemId);
        [OperationContract]
        int GrantUserPermissionItemScopes(BaseUserInfo userInfo, string userId, string permissionItemCode, string[] grantPermissionItemIds);
        [OperationContract]
        int GrantUserPermissions(BaseUserInfo userInfo, string[] userIds, string[] grantPermissionItemIds);
        [OperationContract]
        int GrantUserRoleScopes(BaseUserInfo userInfo, string userId, string permissionItemCode, string[] grantRoleIds);
        [OperationContract]
        int GrantUserUserScopes(BaseUserInfo userInfo, string userId, string permissionItemCode, string[] grantUserIds);
        [OperationContract]
        bool IsAdministrator(BaseUserInfo userInfo);
        [OperationContract]
        bool IsAdministratorByUser(BaseUserInfo userInfo, string userId);
        [OperationContract]
        //bool IsAuthorized(BaseUserInfo userInfo, string permissionItemCode, string permissionItemName);
        bool IsAuthorized(BaseUserInfo userInfo, string permissionItemCode);
        [OperationContract]
        bool IsAuthorizedByRole(BaseUserInfo userInfo, string roleId, string permissionItemCode);
        [OperationContract]
        //bool IsAuthorizedByUser(BaseUserInfo userInfo, string userId, string permissionItemCode, string permissionItemName);
        bool IsAuthorizedByUser(BaseUserInfo userInfo, string userId, string permissionItemCode);
        [OperationContract]
        bool IsInRole(BaseUserInfo userInfo, string userId, string roleName);
        [OperationContract]
        bool IsModuleAuthorized(BaseUserInfo userInfo, string moduleCode);
        [OperationContract]
        bool IsModuleAuthorizedByUser(BaseUserInfo userInfo, string userId, string moduleCode);
        [OperationContract]
        int RevokePermissionScopeTarget(BaseUserInfo userInfo, string resourceCategory, string[] resourceIds, string targetCategory, string revokeTargetId, string permissionItemId);
        [OperationContract]
        int RevokePermissionScopeTargets(BaseUserInfo userInfo, string resourceCategory, string resourceId, string targetCategory, string[] revokeTargetIds, string permissionItemId);
        [OperationContract]
        int RevokeResourcePermission(BaseUserInfo userInfo, string resourceCategory, string resourceId, string[] revokePermissionItemIds);
        [OperationContract]
        int RevokeRoleModuleScope(BaseUserInfo userInfo, string roleId, string permissionItemCode, string revokeModuleId);
        [OperationContract]
        int RevokeRoleModuleScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] revokeModuleIds);
        [OperationContract]
        int RevokeRoleOrganizeScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] revokeOrganizeIds);
        [OperationContract]
        int RevokeRolePermission(BaseUserInfo userInfo, string roleId, string revokePermissionItemId);
        [OperationContract]
        int RevokeRolePermissionById(BaseUserInfo userInfo, string roleId, string revokePermissionItemId);
        [OperationContract]
        int RevokeRolePermissionItemScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] revokePermissionItemIds);
        [OperationContract]
        int RevokeRolePermissions(BaseUserInfo userInfo, string[] roleIds, string[] revokePermissionItemIds);
        [OperationContract]
        int RevokeRoleRoleScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] revokeRoleIds);
        [OperationContract]
        int RevokeRoleUserScopes(BaseUserInfo userInfo, string roleId, string permissionItemCode, string[] revokeUserIds);
        [OperationContract]
        int RevokeUserModuleScope(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string revokeModuleId);
        [OperationContract]
        int RevokeUserModuleScopes(BaseUserInfo userInfo, string userId, string permissionScopeItemCode, string[] revokeModuleIds);
        [OperationContract]
        int RevokeUserOrganizeScopes(BaseUserInfo userInfo, string userId, string permissionItemCode, string[] revokeOrganizeIds);
        [OperationContract]
        int RevokeUserPermissionById(BaseUserInfo userInfo, string userId, string revokePermissionItemId);
        [OperationContract]
        int RevokeUserPermissionItemScopes(BaseUserInfo userInfo, string userId, string permissionItemCode, string[] revokePermissionItemIds);
        [OperationContract]
        int RevokeUserPermissions(BaseUserInfo userInfo, string[] userIds, string[] revokePermissionItemIds);
        [OperationContract]
        int RevokeUserRoleScopes(BaseUserInfo userInfo, string userId, string permissionItemCode, string[] revokeRoleds);
        [OperationContract]
        int RevokeUserUserScopes(BaseUserInfo userInfo, string userId, string permissionItemCode, string[] revokeUserIds);
    }
}

