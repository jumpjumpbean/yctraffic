namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class MessageService : MarshalByRefObject, IMessageService
    {
        public static DataTable InnerOrganizeDT = null;
        public static DateTime LaseInnerOrganizeCheck = DateTime.MinValue;
        public static DateTime LaseOnLineStateCheck = DateTime.MinValue;
        private static object locker = new object();
        public static bool MessageSend = true;
        public static DataTable OnLineStateDT = null;
        private string serviceName = AppMessage.MessageService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public int BatchSend(BaseUserInfo userInfo, string[] receiverIds, string[] organizeIds, string[] roleIds, BaseMessageEntity messageEntity)
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
                        num = new BaseMessageManager(helper, userInfo).BatchSend(receiverIds, organizeIds, roleIds, messageEntity, true);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.MessageService_BatchSend, MethodBase.GetCurrentMethod());
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

        public int Broadcast(BaseUserInfo userInfo, BaseMessageEntity messageEntity)
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
                        string[] receiverIds = null;
                        receiverIds = new BaseUserManager(helper, userInfo).GetIds(BaseUserEntity.FieldEnabled, 1, BaseUserEntity.FieldDeletionStateCode, 0);
                        num = new BaseMessageManager(helper, userInfo).BatchSend(receiverIds, null, null, messageEntity, false);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.MessageService_BatchSend, MethodBase.GetCurrentMethod());
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

        public int CheckOnLine(BaseUserInfo userInfo, int onLineState)
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
                        BaseUserManager manager = new BaseUserManager(helper);
                        manager.OnLine(userInfo.Id, onLineState);
                        num = manager.CheckOnLine();
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

        public DataTable GetDTNew(BaseUserInfo userInfo, out string openId)
        {
            LogOnService.UserIsLogOn(userInfo);
            openId = userInfo.OpenId;
            DataTable dTNew = new DataTable(BaseMessageEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        if (!BaseSystemInfo.CheckOnLine)
                        {
                            openId = new BaseUserManager(helper, userInfo).GetProperty(userInfo.Id, BaseUserEntity.FieldOpenId);
                        }
                        if (userInfo.OpenId.Equals((string) openId))
                        {
                            dTNew = new BaseMessageManager(helper, userInfo).GetDTNew();
                            dTNew.TableName = BaseMessageEntity.TableName;
                        }
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dTNew;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetInnerOrganizeDT(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            bool flag = false;
            lock (locker)
            {
                using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseOrganizeManager manager = new BaseOrganizeManager(helper, userInfo);
                        if (LaseInnerOrganizeCheck == DateTime.MinValue)
                        {
                            flag = true;
                        }
                        else
                        {
                            TimeSpan span = (TimeSpan) (DateTime.Now - LaseInnerOrganizeCheck);
                            if (((span.Minutes * 60) + span.Seconds) >= (BaseSystemInfo.OnLineCheck * 100))
                            {
                                flag = true;
                            }
                        }
                        if ((OnLineStateDT == null) || flag)
                        {
                            string commandText = "SELECT *  FROM " + BaseOrganizeEntity.TableName + " WHERE " + BaseOrganizeEntity.FieldDeletionStateCode + " = 0   AND " + BaseOrganizeEntity.FieldIsInnerOrganize + " = 1   AND " + BaseOrganizeEntity.FieldEnabled + " = 1ORDER BY " + BaseOrganizeEntity.FieldSortCode;
                            InnerOrganizeDT = manager.Fill(commandText);
                            InnerOrganizeDT.TableName = BaseOrganizeEntity.TableName;
                            LaseInnerOrganizeCheck = DateTime.Now;
                        }
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
            return InnerOrganizeDT;
        }

        public DataTable GetOnLineState(BaseUserInfo userInfo)
        {
            bool flag = false;
            LogOnService.UserIsLogOn(userInfo);
            lock (locker)
            {
                using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        manager.OnLine(userInfo.Id, 1);
                        if (LaseOnLineStateCheck == DateTime.MinValue)
                        {
                            flag = true;
                        }
                        else
                        {
                            TimeSpan span = (TimeSpan) (DateTime.Now - LaseOnLineStateCheck);
                            if (((span.Minutes * 60) + span.Seconds) >= BaseSystemInfo.OnLineCheck)
                            {
                                flag = true;
                            }
                        }
                        if ((OnLineStateDT == null) || flag)
                        {
                            manager.CheckOnLine();
                            OnLineStateDT = manager.GetOnLineStateDT();
                            OnLineStateDT.TableName = BaseUserEntity.TableName;
                            LaseOnLineStateCheck = DateTime.Now;
                        }
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
            return OnLineStateDT;
        }

        public DataTable GetUserDTByOrganize(BaseUserInfo userInfo, string organizeId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseUserEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        string str2 = " SELECT " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + "," + BaseUserEntity.TableName + "." + BaseUserEntity.FieldRealName + "," + BaseUserEntity.TableName + "." + BaseUserEntity.FieldUserOnLine + " FROM " + BaseUserEntity.TableName;
                        string commandText = str2 + " WHERE (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDeletionStateCode + " = 0  AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldEnabled + " = 1   AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldIsVisible + " = 1 ) ";
                        if (!string.IsNullOrEmpty(organizeId))
                        {
                            string str3 = commandText;
                            string str4 = str3 + " AND ((" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDepartmentId + " = '" + organizeId + "' AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldWorkgroupId + " IS NULL ";
                            string str5 = str4 + "       OR (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldWorkgroupId + " = '" + organizeId + "')) ";
                            commandText = str5 + " OR " + BaseUserEntity.FieldId + " IN ( SELECT " + BaseUserOrganizeEntity.FieldUserId + "   FROM " + BaseUserOrganizeEntity.TableName + "  WHERE (" + BaseUserOrganizeEntity.TableName + "." + BaseUserOrganizeEntity.FieldDeletionStateCode + " = 0 )        AND (" + BaseUserOrganizeEntity.TableName + "." + BaseUserOrganizeEntity.FieldDepartmentId + " = '" + organizeId + "' AND " + BaseUserOrganizeEntity.TableName + "." + BaseUserOrganizeEntity.FieldWorkgroupId + " IS NULL             OR (" + BaseUserOrganizeEntity.TableName + "." + BaseUserOrganizeEntity.FieldWorkgroupId + " = '" + organizeId + "')))) ";
                        }
                        string str6 = commandText;
                        commandText = str6 + " ORDER BY " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldSortCode;
                        table = manager.Fill(commandText);
                        table.TableName = BaseUserEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.MessageService_GetUserDTByDepartment, MethodBase.GetCurrentMethod());
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

        public string[] MessageChek(BaseUserInfo userInfo, int onLineState, string lastChekDate)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] strArray = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        new BaseUserManager(helper, userInfo).OnLine(userInfo.Id, onLineState);
                        strArray = new BaseMessageManager(helper, userInfo).MessageChek();
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.MessageService_MessageChek, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return strArray;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public void Read(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    helper.Open(this.UserCenterDbConnection);
                    new BaseMessageManager(helper, userInfo).Read(id);
                    BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.MessageService_Read, MethodBase.GetCurrentMethod());
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

        public DataTable ReadFromReceiver(BaseUserInfo userInfo, string receiverId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseMessageEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseMessageManager(helper, userInfo).ReadFromReceiver(receiverId);
                        table.TableName = BaseMessageEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.MessageService_ReadFromReceiver, MethodBase.GetCurrentMethod());
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

        public string Send(BaseUserInfo userInfo, string receiverId, string content)
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
                        BaseMessageEntity baseMessageEntity = new BaseMessageEntity {
                            Id = Guid.NewGuid().ToString(),
                            CategoryCode = MessageCategory.Send.ToString(),
                            FunctionCode = MessageFunction.Message.ToString(),
                            ReceiverId = receiverId,
                            Contents = content,
                            IsNew = 1,
                            ReadCount = 0,
                            DeletionStateCode = 0,
                            Enabled = 1
                        };
                        str = new BaseMessageManager(helper, userInfo).Add(baseMessageEntity, false, false);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.MessageService_Send, MethodBase.GetCurrentMethod());
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

