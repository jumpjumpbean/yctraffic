namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IOrganizeService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, BaseOrganizeEntity organizeEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        string AddByDetail(BaseUserInfo userInfo, string parentId, string code, string fullName, string categoryId, string outerPhone, string innerPhone, string fax, bool enabled, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchMoveTo(BaseUserInfo userInfo, string[] organizeIds, string parentId);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        int BatchSetCode(BaseUserInfo userInfo, string[] ids, string[] codes);
        [OperationContract]
        int BatchSetSortCode(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetCompanyDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetDepartmentDT(BaseUserInfo userInfo, string organizeId);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetDTByParent(BaseUserInfo userInfo, string parentId);
        [OperationContract]
        BaseOrganizeEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetInnerOrganizeDT(BaseUserInfo userInfo);
        [OperationContract]
        int MoveTo(BaseUserInfo userInfo, string organizeId, string parentId);
        [OperationContract]
        DataTable Search(BaseUserInfo userInfo, string organizeId, string searchValue);
        [OperationContract]
        int SetDeleted(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Update(BaseUserInfo userInfo, BaseOrganizeEntity organizeEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        BaseOrganizeEntity GetCompanyByDepID(BaseUserInfo userInfo, string depID);
    }
}

