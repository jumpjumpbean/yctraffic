namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class FileService : MarshalByRefObject, IFileService
    {
        private string serviceName = AppMessage.FileService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string Add(BaseUserInfo userInfo, string folderId, string fileName, byte[] file, string description, string category, bool enabled, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseFileManager manager = new BaseFileManager(helper, userInfo);
                        str = manager.Add(folderId, fileName, file, description, category, enabled, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int BatchDelete(BaseUserInfo userInfo, string[] ids)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseFileManager manager = new BaseFileManager(helper, userInfo);
                        for (int i = 0; i < ids.Length; i++)
                        {
                            num += manager.Delete(ids[i]);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int BatchMoveTo(BaseUserInfo userInfo, string[] ids, string folderId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseFileManager manager = new BaseFileManager(helper, userInfo);
                        for (int i = 0; i < ids.Length; i++)
                        {
                            num += manager.MoveTo(ids[i], folderId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int BatchSave(BaseUserInfo userInfo, DataTable dataTable)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseFileManager(helper, userInfo).BatchSave(dataTable);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int Delete(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseFileManager(helper, userInfo).Delete(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int DeleteByFolder(BaseUserInfo userInfo, string folderId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseFileManager(helper, userInfo).DeleteByFolder(folderId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.FileService_DeleteByFolder, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public byte[] Download(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            byte[] buffer = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        buffer = new BaseFileManager(helper, userInfo).Download(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.FileService_Download, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return buffer;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public bool Exists(BaseUserInfo userInfo, string folderId, string fileName)
        {
            LogOnService.UserIsLogOn(userInfo);
            bool flag = false;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        flag = new BaseFileManager(helper, userInfo).Exists(BaseFileEntity.FieldFolderId, folderId, BaseFileEntity.FieldFileName, fileName);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.FileService_Exists, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return flag;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetDTByFolder(BaseUserInfo userInfo, string folderId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dTByFolder = new DataTable(BaseFileEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        dTByFolder = new BaseFileManager(helper, userInfo).GetDTByFolder(folderId);
                        dTByFolder.TableName = BaseFolderEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.FileService_GetDTByFolder, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dTByFolder;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public BaseFileEntity GetEntity(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseFileEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        entity = new BaseFileManager(helper, userInfo).GetEntity(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.FileService_GetEntity, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return entity;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int MoveTo(BaseUserInfo userInfo, string id, string folderId)
        {
            LogOnService.UserIsLogOn(userInfo);
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseFileManager(helper, userInfo).MoveTo(id, folderId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int Rename(BaseUserInfo userInfo, string id, string newName, bool enabled, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseFileEntity fileEntity = new BaseFileEntity();
                        BaseFileManager manager = new BaseFileManager(helper, userInfo);
                        DataTable dTById = manager.GetDTById(id);
                        fileEntity.GetFrom(dTById);
                        fileEntity.FileName = newName;
                        fileEntity.Enabled = new int?(enabled ? 1 : 0);
                        num = manager.Update(fileEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable Search(BaseUserInfo userInfo, string searchValue)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseFileEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseFileManager(helper, userInfo).Search(searchValue);
                        table.TableName = BaseFileEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return table;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int Update(BaseUserInfo userInfo, string id, string folderId, string fileName, string description, bool enabled, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseFileManager manager = new BaseFileManager(helper, userInfo);
                        num = manager.Update(id, folderId, fileName, description, enabled, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int UpdateFile(BaseUserInfo userInfo, string id, string fileName, byte[] file, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            statusCode = string.Empty;
            statusMessage = string.Empty;
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseFileManager manager = new BaseFileManager(helper, userInfo);
                        num = manager.UpdateFile(id, fileName, file, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return num;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public string Upload(BaseUserInfo userInfo, string folderId, string fileName, byte[] file, bool enabled)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        str = new BaseFileManager(helper, userInfo).Upload(folderId, fileName, file, enabled);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.FileService_Upload, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return str;
                }
                finally
                {
                    helper.Close();
                }
            }
        }
    }
}

