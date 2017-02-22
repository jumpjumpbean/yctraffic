namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IFolderService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, BaseFolderEntity folderEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        string AddByFolderName(BaseUserInfo userInfo, string parentId, string folderName, bool enabled, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchMoveTo(BaseUserInfo userInfo, string[] folderIds, string parentId);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetDTByParent(BaseUserInfo userInfo, string id);
        [OperationContract]
        BaseFolderEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        int MoveTo(BaseUserInfo userInfo, string folderId, string parentId);
        [OperationContract]
        int Rename(BaseUserInfo userInfo, string id, string newName, bool enabled, out string statusCode, out string statusMessage);
        [OperationContract]
        DataTable Search(BaseUserInfo userInfo, string searchValue);
        [OperationContract]
        int Update(BaseUserInfo userInfo, BaseFolderEntity folderEntity, out string statusCode, out string statusMessage);
    }
}

