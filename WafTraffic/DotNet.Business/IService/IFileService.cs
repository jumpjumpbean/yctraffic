namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IFileService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, string folderId, string fileName, byte[] file, string description, string category, bool enabled, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int BatchMoveTo(BaseUserInfo userInfo, string[] ids, string folderId);
        [OperationContract]
        int BatchSave(BaseUserInfo userInfo, DataTable dataTable);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        int DeleteByFolder(BaseUserInfo userInfo, string folderId);
        [OperationContract]
        byte[] Download(BaseUserInfo userInfo, string id);
        [OperationContract]
        bool Exists(BaseUserInfo userInfo, string folderId, string fileName);
        [OperationContract]
        DataTable GetDTByFolder(BaseUserInfo userInfo, string folderId);
        [OperationContract]
        BaseFileEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        int MoveTo(BaseUserInfo userInfo, string id, string folderId);
        [OperationContract]
        int Rename(BaseUserInfo userInfo, string id, string newName, bool enabled, out string statusCode, out string statusMessage);
        [OperationContract]
        DataTable Search(BaseUserInfo userInfo, string searchValue);
        [OperationContract]
        int Update(BaseUserInfo userInfo, string id, string folderId, string fileName, string description, bool enabled, out string statusCode, out string statusMessage);
        [OperationContract]
        int UpdateFile(BaseUserInfo userInfo, string id, string fileName, byte[] file, out string statusCode, out string statusMessage);
        [OperationContract]
        string Upload(BaseUserInfo userInfo, string folderId, string fileName, byte[] file, bool enabled);
    }
}

