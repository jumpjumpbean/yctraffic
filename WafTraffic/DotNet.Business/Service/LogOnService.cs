namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class LogOnService : MarshalByRefObject, ILogOnService
    {
        private string serviceName = AppMessage.LogOnService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public BaseUserInfo AccountActivation(BaseUserInfo userInfo, string openId, out string statusCode, out string statusMessage)
        {
            UserIsLogOn(userInfo);
            BaseUserInfo info = null;
            statusCode = string.Empty;
            statusMessage = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        manager.CheckOnLine();
                        info = manager.AccountActivation(openId, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return info;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int ChangeCommunicationPassword(BaseUserInfo userInfo, string oldPassword, string newPassword, out string statusCode, out string statusMessage)
        {
            UserIsLogOn(userInfo);
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
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_ChangeCommunicationPassword, MethodBase.GetCurrentMethod());
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        num = manager.ChangeCommunicationPassword(oldPassword, newPassword, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
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

        public int ChangePassword(BaseUserInfo userInfo, string oldPassword, string newPassword, out string statusCode, out string statusMessage)
        {
            UserIsLogOn(userInfo);
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
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_ChangePassword, MethodBase.GetCurrentMethod());
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        num = manager.ChangePassword(oldPassword, newPassword, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
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

        public int ChangeSignedPassword(BaseUserInfo userInfo, string oldPassword, string newPassword, out string statusCode, out string statusMessage)
        {
            UserIsLogOn(userInfo);
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
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        num = manager.ChangeSignedPassword(oldPassword, newPassword, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_ChangeSignedPassword, MethodBase.GetCurrentMethod());
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

        public bool CommunicationPassword(BaseUserInfo userInfo, string communicationPassword)
        {
            UserIsLogOn(userInfo);
            bool flag = false;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        flag = new BaseUserManager(helper, userInfo).CommunicationPassword(communicationPassword);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_CommunicationPassword, MethodBase.GetCurrentMethod());
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

        public string CreateDigitalSignature(BaseUserInfo userInfo, string password, out string returnStatusCode, out string returnStatusMessage)
        {
            UserIsLogOn(userInfo);
            string str = string.Empty;
            returnStatusCode = string.Empty;
            returnStatusMessage = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        str = manager.CreateDigitalSignature(password, out returnStatusCode);
                        returnStatusMessage = manager.GetStateMessage(returnStatusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_CreateDigitalSignature, MethodBase.GetCurrentMethod());
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

        public string GetPublicKey(BaseUserInfo userInfo, string userId)
        {
            UserIsLogOn(userInfo);
            string publicKey = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        publicKey = new BaseUserManager(helper, userInfo).GetPublicKey(userId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_GetPublicKey, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return publicKey;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetStaffUserDT(BaseUserInfo userInfo)
        {
            UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper);
                        manager.CheckOnLine();
                        string[] names = new string[] { BaseUserEntity.FieldEnabled, BaseUserEntity.FieldDeletionStateCode, BaseUserEntity.FieldIsStaff };
                        object[] values = new object[] { 1, 0, 1 };
                        table = manager.GetDT(names, values, BaseStaffEntity.FieldSortCode);
                        table.TableName = BaseUserEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_GetStaffUserDT, MethodBase.GetCurrentMethod());
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

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public DataTable GetUserDT(BaseUserInfo userInfo)
        {
            //UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseUserEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        manager.CheckOnLine();
                        string[] names = new string[] { BaseUserEntity.FieldEnabled, BaseUserEntity.FieldDeletionStateCode };
                        object[] values = new object[] { 1, 0 };
                        table = manager.GetDT(names, values, BaseUserEntity.FieldSortCode);
                        table.TableName = BaseUserEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_GetUserDT, MethodBase.GetCurrentMethod());
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

        public BaseUserInfo LogOnByOpenId(BaseUserInfo userInfo, string openId, out string returnStatusCode, out string returnStatusMessage)
        {
            BaseSystemInfo.IsAuthorized(userInfo);
            BaseUserInfo info = null;
            returnStatusCode = string.Empty;
            returnStatusMessage = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        manager.CheckOnLine();
                        info = manager.LogOnByOpenId(openId, userInfo.IPAddress, userInfo.MACAddress);
                        returnStatusCode = manager.ReturnStatusCode;
                        returnStatusMessage = manager.GetStateMessage(manager.ReturnStatusCode);
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return info;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public BaseUserInfo LogOnByUserName(BaseUserInfo userInfo, string userName, out string returnStatusCode, out string returnStatusMessage)
        {
            BaseSystemInfo.IsAuthorized(userInfo);
            BaseUserInfo info = null;
            returnStatusCode = string.Empty;
            returnStatusMessage = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        manager.CheckOnLine();
                        info = manager.LogOnByUserName(userName, userInfo.IPAddress, userInfo.MACAddress);
                        returnStatusCode = manager.ReturnStatusCode;
                        returnStatusMessage = manager.GetStateMessage();
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return info;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public void OnExit(BaseUserInfo userInfo)
        {
            UserIsLogOn(userInfo);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    helper.Open(this.UserCenterDbConnection);
                    BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_OnExit, MethodBase.GetCurrentMethod());
                    new BaseUserManager(helper, userInfo).OnExit(userInfo.Id);
                }
                catch (Exception exception)
                {
                    BaseExceptionManager.LogException(helper, userInfo, exception);
                    throw exception;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        //public void OnLine(BaseUserInfo userInfo, int onLineState = 1) //C# 4.0 才支持缺省参数
        public void OnLine(BaseUserInfo userInfo, int onLineState)
        {
            UserIsLogOn(userInfo);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    helper.Open(this.UserCenterDbConnection);
                    new BaseUserManager(helper, userInfo).OnLine(userInfo.Id, onLineState);
                }
                catch (Exception exception)
                {
                    BaseExceptionManager.LogException(helper, userInfo, exception);
                    throw exception;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public void OnLine(BaseUserInfo userInfo)
        {
            OnLine(userInfo, 1);
        }

        public int ServerCheckOnLine()
        {
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        num = new BaseUserManager(helper).CheckOnLine();
                    }
                    catch (Exception exception)
                    {
                        LogUtil.WriteException(exception);
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

        public int SetCommunicationPassword(BaseUserInfo userInfo, string[] userIds, string password, out string statusCode, out string statusMessage)
        {
            UserIsLogOn(userInfo);
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
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_SetPassword, MethodBase.GetCurrentMethod());
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        num = manager.BatchSetCommunicationPassword(userIds, password, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
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

        public int SetPassword(BaseUserInfo userInfo, string[] userIds, string password, out string returnStatusCode, out string returnStatusMessage)
        {
            UserIsLogOn(userInfo);
            returnStatusCode = string.Empty;
            returnStatusMessage = string.Empty;
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_SetPassword, MethodBase.GetCurrentMethod());
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        num = manager.BatchSetPassword(userIds, password);
                        returnStatusCode = manager.ReturnStatusCode;
                        returnStatusMessage = manager.GetStateMessage(returnStatusCode);
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

        public bool SignedPassword(BaseUserInfo userInfo, string signedPassword)
        {
            UserIsLogOn(userInfo);
            bool flag = false;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        flag = new BaseUserManager(helper, userInfo).SignedPassword(signedPassword);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.LogOnService_SignedPassword, MethodBase.GetCurrentMethod());
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

        public static bool UserIsLogOn(BaseUserInfo userInfo)
        {
            if (!BaseSystemInfo.IsAuthorized(userInfo))
            {
                throw new Exception(AppMessage.MSG0800);
            }
            if (string.IsNullOrEmpty(userInfo.OpenId))
            {
                throw new Exception(AppMessage.MSG0900);
            }
            return true;
        }

        public BaseUserInfo UserLogOn(BaseUserInfo userInfo, string userName, string password, bool createOpenId, out string returnStatusCode, out string returnStatusMessage)
        {
            returnStatusCode = StatusCode.DbError.ToString();
            returnStatusMessage = string.Empty;
            BaseSystemInfo.IsAuthorized(userInfo);
            BaseUserInfo info = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        manager.CheckOnLine();
                        info = manager.LogOn(userName, password, createOpenId, null, null, true);
                        returnStatusCode = manager.ReturnStatusCode;
                        returnStatusMessage = manager.GetStateMessage(returnStatusCode);
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return info;
                }
                finally
                {
                    helper.Close();
                }
            }
        }
    }
}

