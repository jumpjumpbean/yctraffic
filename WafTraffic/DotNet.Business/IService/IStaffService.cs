namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IStaffService
    {
        [OperationContract]
        string AddStaff(BaseUserInfo userInfo, BaseStaffEntity staffEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchMoveTo(BaseUserInfo userInfo, string[] ids, string organizeId);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        int BatchUpdateAddress(BaseUserInfo userInfo, List<BaseStaffEntity> staffEntites, out string statusCode, out string statusMessage);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        int DeleteUser(BaseUserInfo userInfo, string staffId);
        [OperationContract]
        DataTable GetAddressDT(BaseUserInfo userInfo, string organizeId, string searchValue);
        [OperationContract]
        DataTable GetAddressPageDT(BaseUserInfo userInfo, string organizeId, string searchValue, int pageSize, int currentPage, out int recordCount);
        [OperationContract]
        DataTable GetChildrenStaffs(BaseUserInfo userInfo, string organizeId);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetDTByDepartment(BaseUserInfo userInfo, string departmentId, bool containChildren);
        [OperationContract]
        DataTable GetDTByIds(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        DataTable GetDTByOrganize(BaseUserInfo userInfo, string organizeId, bool containChildren);
        [OperationContract]
        BaseStaffEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        string GetId(BaseUserInfo userInfo, string name, object value);
        [OperationContract]
        DataTable GetParentChildrenStaffs(BaseUserInfo userInfo, string organizeId);
        [OperationContract]
        int MoveTo(BaseUserInfo userInfo, string id, string organizeId);
        [OperationContract]
        int ResetSortCode(BaseUserInfo userInfo);
        [OperationContract]
        DataTable Search(BaseUserInfo userInfo, string organizeId, string searchValue);
        [OperationContract]
        int SetDeleted(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int SetStaffUser(BaseUserInfo userInfo, string staffId, string userId);
        [OperationContract]
        int UpdateAddress(BaseUserInfo userInfo, BaseStaffEntity staffEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        int UpdateStaff(BaseUserInfo userInfo, BaseStaffEntity staffEntity, out string statusCode, out string statusMessage);
    }
}

