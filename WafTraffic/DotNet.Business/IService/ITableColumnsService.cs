namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.ServiceModel;

    [ServiceContract]
    public interface ITableColumnsService
    {
        [OperationContract]
        int BatchDeleteConstraint(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        string GetConstraint(BaseUserInfo userInfo, string resourceCategory, string resourceId, string tableName);
        [OperationContract]
        DataTable GetConstraintDT(BaseUserInfo userInfo, string resourceCategory, string resourceId);
        [OperationContract]
        BasePermissionScopeEntity GetConstraintEntity(BaseUserInfo userInfo, string resourceCategory, string resourceId, string tableName, string permissionCode);
        [OperationContract]
        DataTable GetDTByTable(BaseUserInfo userInfo, string tableCode);
        [OperationContract]
        string GetUserConstraint(BaseUserInfo userInfo, string tableName);
        [OperationContract]
        string SetConstraint(BaseUserInfo userInfo, string resourceCategory, string resourceId, string tableName, string permissionCode, string constraint, bool enabled);
    }
}

