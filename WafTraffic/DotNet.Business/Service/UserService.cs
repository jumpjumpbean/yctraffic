//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

/// 修改纪录
/// 
///     2012.12.12 版本：2.0 sunmiao 修改 添加部门调动方法
///		2012.06.29 版本：2.0 sunmiao 修改
///		
/// 版本：2.0
/// 

namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    /// <summary>
    /// 用户管理服务
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class UserService : MarshalByRefObject, IUserService
    {
        private string serviceName = AppMessage.UserService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string AddUser(BaseUserInfo userInfo, BaseUserEntity userEntity, out string statusCode, out string statusMessage)
        {
            BaseSystemInfo.IsAuthorized(userInfo);
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        str = this.AddUser(helper, userInfo, userEntity, out statusCode, out statusMessage);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_AddUser, MethodBase.GetCurrentMethod());
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

        public string AddUser(IDbHelper dbHelper, BaseUserInfo userInfo, BaseUserEntity userEntity, out string statusCode, out string statusMessage)
        {
            BaseSystemInfo.IsAuthorized(userInfo);
            string str = string.Empty;
            BaseUserManager manager = new BaseUserManager(dbHelper, userInfo);

            if (BaseSystemInfo.ServerEncryptPassword)
            {
                userEntity.UserPassword = manager.EncryptUserPassword(userEntity.UserPassword);
                userEntity.CommunicationPassword = manager.EncryptUserPassword(userEntity.CommunicationPassword);
            }

            str = manager.Add(userEntity, out statusCode); //添加用户记录
            statusMessage = manager.GetStateMessage(statusCode);

            //若为用户自申请，向系统管理员发出审核通知
            if (((userEntity.Enabled == 0) && statusCode.Equals(StatusCode.OKAdd.ToString())) && !userInfo.IsAdministrator)
            {
                string[] roleIds = new BaseRoleManager(dbHelper, userInfo).GetIds(BaseRoleEntity.FieldCode, "Administrators", BaseRoleEntity.FieldId);
                string[] receiverIds = manager.GetIds(BaseUserEntity.FieldCode, "Administrator", BaseUserEntity.FieldId);
                BaseMessageEntity messageEntity = new BaseMessageEntity {
                    Id = BaseBusinessLogic.NewGuid(),
                    FunctionCode = MessageFunction.WaitForAudit.ToString(),
                    Contents = userInfo.RealName + "(" + userInfo.IPAddress + ")" + AppMessage.UserService_Application + userEntity.RealName + AppMessage.UserService_Check,
                    IsNew = 1,
                    ReadCount = 0,
                    Enabled = 1,
                    DeletionStateCode = 0
                };
                new BaseMessageManager(dbHelper, userInfo).BatchSend(receiverIds, null, roleIds, messageEntity, false);
            }
            return str;
        }

        public string AddUserToOrganize(BaseUserInfo userInfo, BaseUserOrganizeEntity userOrganizeEntity, out string statusCode, out string statusMessage)
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
                        BaseUserOrganizeManager manager = new BaseUserOrganizeManager(helper, userInfo);
                        str = manager.Add(userOrganizeEntity, out statusCode);
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

        public int AddUserToRole(BaseUserInfo userInfo, string userId, string[] addRoleIds)
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
                        BaseUserRoleManager manager = new BaseUserRoleManager(helper, userInfo);
                        if (addRoleIds != null)
                        {
                            num += manager.AddToRole(userId, addRoleIds);
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
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        num = manager.Delete((object[]) ids);
                        manager.CheckUserStaff();
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

        public int BatchDeleteUserOrganize(BaseUserInfo userInfo, string[] ids)
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
                        num = new BaseUserOrganizeManager(helper, userInfo).Delete((object[]) ids);
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

        /// <summary>
        /// 批量储存dataTable中的操作(包括添、删、改)
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
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
                        num = new BaseUserManager(helper, userInfo).BatchSave(dataTable);
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

        /// <summary>
        /// 为一批用户设置缺省角色(User表中的RoleId)
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="userIds"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int BatchSetDefaultRole(BaseUserInfo userInfo, string[] userIds, string roleId)
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
                        num = new BaseUserManager(helper, userInfo).SetProperty((object[]) userIds, BaseUserEntity.FieldRoleId, roleId);
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

        /// <summary>
        /// 清除用户的所有角色信息（包括该用户的缺省角色）
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ClearUserRole(BaseUserInfo userInfo, string userId)
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
                        num = new BaseUserRoleManager(helper, userInfo).ClearUserRole(userId);
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

        /// <summary>
        /// 删除用户（不会删除关联的Staff，但会去掉关联关系）
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        num = manager.Delete(id);
                        manager.CheckUserStaff();
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

        public bool Exists(BaseUserInfo userInfo, string[] fieldNames, object[] fieldValues)
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
                        flag = new BaseUserManager(helper).Exists(fieldNames, fieldValues);
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

        /// <summary>
        /// 获取用户拥有的所有角色的ID集合
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string[] GetUserRoleIds(BaseUserInfo userInfo, string userId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] allRoleIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        allRoleIds = new BaseUserRoleManager(helper, userInfo).GetAllRoleIds(userId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return allRoleIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetDT(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dT = new DataTable(BaseUserEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        dT = new BaseUserManager(helper, userInfo).GetDT();
                        dT.TableName = BaseUserEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_GetDT, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dT;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 获得部门下的所有用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="departmentId"></param>
        /// <param name="containChildren">是否包括下级部门</param>
        /// <returns></returns>
        public DataTable GetDTByDepartment(BaseUserInfo userInfo, string departmentId, bool containChildren)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable childrenUsers = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        if (containChildren)
                        {
                            childrenUsers = manager.GetChildrenUsers(departmentId);
                        }
                        else
                        {
                            childrenUsers = manager.GetDTByDepartment(departmentId);
                        }
                        childrenUsers.TableName = BaseUserEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_GetDTByDepartment, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return childrenUsers;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetDTByIds(BaseUserInfo userInfo, string[] ids)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseUserManager(helper, userInfo).GetDT(BaseUserEntity.FieldId, (object[]) ids, BaseUserEntity.FieldSortCode);
                        table.TableName = BaseUserEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_GetDTByIds, MethodBase.GetCurrentMethod());
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
        /// 获取拥有某角色的所有用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetDTByRole(BaseUserInfo userInfo, string roleId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dTByRole = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        dTByRole = new BaseUserManager(helper, userInfo).GetDTByRole(roleId);
                        dTByRole.TableName = BaseUserEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_GetDTByRole, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dTByRole;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public BaseUserEntity GetEntity(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseUserEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        entity = new BaseUserManager(helper, userInfo).GetEntity(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_GetEntity, MethodBase.GetCurrentMethod());
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

        //这个函数的功能有问题
        public DataTable GetRoleDT(BaseUserInfo userInfo)
        {
            BaseSystemInfo.IsAuthorized(userInfo);
            DataTable table = new DataTable(BaseRoleEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseRoleManager manager = new BaseRoleManager(helper, userInfo);
                        string[] names = new string[] { BaseRoleEntity.FieldDeletionStateCode, BaseRoleEntity.FieldEnabled };
                        object[] values = new object[] { 0, 1 };
                        table = manager.GetDT(names, values, BaseRoleEntity.FieldSortCode);
                        if (!userInfo.IsAdministrator)
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                if (row[BaseRoleEntity.FieldCode].ToString().Equals(DefaultRole.Administrators.ToString()))
                                {
                                    row.Delete();
                                }
                            }
                            table.AcceptChanges();
                        }
                        table.TableName = BaseUserEntity.TableName;
                        table.DefaultView.Sort = BaseUserEntity.FieldSortCode;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_GetRoleDT, MethodBase.GetCurrentMethod());
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

        public DataTable GetUserOrganizeDT(BaseUserInfo userInfo, string userId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable userOrganizeDT = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        userOrganizeDT = new BaseUserOrganizeManager(helper, userInfo).GetUserOrganizeDT(userId);
                        userOrganizeDT.TableName = BaseUserOrganizeEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return userOrganizeDT;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        /// <summary>
        /// 清除某用户拥有的某些角色
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="userId"></param>
        /// <param name="removeRoleIds"></param>
        /// <returns></returns>
        public int RemoveUserFromRole(BaseUserInfo userInfo, string userId, string[] removeRoleIds)
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
                        BaseUserRoleManager manager = new BaseUserRoleManager(helper, userInfo);
                        if (removeRoleIds != null)
                        {
                            num += manager.RemoveFormRole(userId, removeRoleIds);
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

        public DataTable Search(BaseUserInfo userInfo, string searchValue, string auditStates, string[] roleIds)
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
                        table = new BaseUserManager(helper, userInfo).Search(searchValue, roleIds, null, auditStates);
                        table.TableName = BaseUserEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_Search, MethodBase.GetCurrentMethod());
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
        /// 为用户设置缺省角色(User表中的RoleId)
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int SetDefaultRole(BaseUserInfo userInfo, string userId, string roleId)
        {
            LogOnService.UserIsLogOn(userInfo);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    helper.Open(this.UserCenterDbConnection);
                    new BaseUserManager(helper, userInfo).SetProperty(userId, BaseUserEntity.FieldRoleId, roleId);
                    BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_SetDefaultRole, MethodBase.GetCurrentMethod());
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
            return 0;
        }

        /// <summary>
        /// 批量标示删除标志
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int SetDeleted(BaseUserInfo userInfo, string[] ids)
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
                        num = new BaseUserManager(helper, userInfo).SetDeleted((object[]) ids);
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

        /// <summary>
        /// 设置用户审核状态
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ids"></param>
        /// <param name="auditStates"></param>
        /// <returns></returns>
        public int SetUserAuditStates(BaseUserInfo userInfo, string[] ids, AuditStatus auditStates)
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
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        num = manager.SetProperty((object[]) ids, BaseUserEntity.FieldAuditStatus, auditStates.ToString());
                        if (auditStates == AuditStatus.AuditPass)
                        {
                            num = manager.SetProperty((object[]) ids, BaseUserEntity.FieldEnabled, 1);
                        }
                        else if (auditStates == AuditStatus.AuditReject)
                        {
                            string[] targetFields = new string[2];
                            object[] targetValues = new object[2];
                            targetFields[0] = BaseUserEntity.FieldEnabled;
                            targetFields[1] = BaseUserEntity.FieldAuditStatus;
                            targetValues[0] = 0;
                            targetValues[1] = StatusCode.UserLocked.ToString();
                            num = manager.SetProperty((object[])ids, targetFields, targetValues);
                            //num = manager.SetProperty((object[]) ids, BaseUserEntity.FieldEnabled, 0);
                            //num = manager.SetProperty((object[]) ids, BaseUserEntity.FieldAuditStatus, StatusCode.UserLocked.ToString());
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_SetUserAuditStates, MethodBase.GetCurrentMethod());
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

        public int UpdateUser(BaseUserInfo userInfo, BaseUserEntity userEntity, out string statusCode, out string statusMessage)
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
                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        num = manager.Update(userEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_UpdateUser, MethodBase.GetCurrentMethod());
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

        public bool UserInRole(BaseUserInfo userInfo, string userId, string roleCode)
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
                        flag = new BaseUserRoleManager(helper, userInfo).UserInRole(userId, roleCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.UserService_UserInRole, MethodBase.GetCurrentMethod());
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

        /// <summary>
        /// 用户调动部门
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="ids">用户id列表</param>
        /// <param name="organizeId">要转移到的部门id</param>
        /// <returns></returns>
        public int BatchMoveTo(BaseUserInfo userInfo, string[] ids, string organizeId)
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

                        int companyId=0;
                        string companyFullName ="";
                        string departmentFullName="";

                        BaseOrganizeEntity oEntity = DotNetService.Instance.OrganizeService.GetCompanyByDepID(userInfo, organizeId);
                        if (oEntity != null)
                        {
                            companyFullName = oEntity.FullName;
                            companyId = oEntity.Id.Value;
                        }
                        departmentFullName = new BaseOrganizeManager(helper, userInfo).GetEntity(organizeId).FullName;

                        string[] targetFileds = new string[4];
                        object[] targetValues = new object[4];
                        targetFileds[0] = BaseUserEntity.FieldCompanyId;
                        targetValues[0] = companyId;
                        targetFileds[1] = BaseUserEntity.FieldCompanyName;
                        targetValues[1] = companyFullName;
                        targetFileds[2] = BaseUserEntity.FieldDepartmentId;
                        targetValues[2] = organizeId;
                        targetFileds[3] = BaseUserEntity.FieldDepartmentName;
                        targetValues[3] = departmentFullName;

                        BaseUserManager manager = new BaseUserManager(helper, userInfo);
                        num = manager.SetProperty(ids, targetFileds, targetValues);
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
    }

}

