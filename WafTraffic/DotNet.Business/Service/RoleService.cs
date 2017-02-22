//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

/// 修改纪录
/// 
///		2012.06.29 版本：2.0 sunmiao 修改
///		
/// 版本：2.0
/// 

namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class RoleService : MarshalByRefObject, IRoleService
    {
        private string serviceName = AppMessage.RoleService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string Add(BaseUserInfo userInfo, BaseRoleEntity roleEntity, out string statusCode, out string statusMessage)
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
                        helper.BeginTransaction();
                        BaseRoleManager manager = new BaseRoleManager(helper, userInfo);
                        str = manager.Add(roleEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_Add, MethodBase.GetCurrentMethod());
                        helper.CommitTransaction();
                    }
                    catch (Exception exception)
                    {
                        helper.RollbackTransaction();
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

        public int AddUserToRole(BaseUserInfo userInfo, string roleId, string[] addUserIds)
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
                        if (addUserIds != null)
                        {
                            num += manager.AddToRole(addUserIds, roleId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_AddUserToRole, MethodBase.GetCurrentMethod());
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
                        helper.BeginTransaction();
                        num = new BaseRoleManager(helper, userInfo).BatchDelete(ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_BatchDelete, MethodBase.GetCurrentMethod());
                        helper.CommitTransaction();
                    }
                    catch (Exception exception)
                    {
                        helper.RollbackTransaction();
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

        public int BatchMoveTo(BaseUserInfo userInfo, string[] ids, string targetId)
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
                        num = new BaseRoleManager(helper, userInfo).BatchMoveTo(ids, targetId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_BatchMoveTo, MethodBase.GetCurrentMethod());
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

        public int BatchSave(BaseUserInfo userInfo, List<BaseRoleEntity> roleEntites)
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
                        helper.BeginTransaction();
                        num = new BaseRoleManager(helper, userInfo).BatchSave(roleEntites);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_BatchSave, MethodBase.GetCurrentMethod());
                        helper.CommitTransaction();
                    }
                    catch (Exception exception)
                    {
                        helper.RollbackTransaction();
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

        public int ClearRoleUser(BaseUserInfo userInfo, string roleId)
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
                        num = new BaseUserRoleManager(helper, userInfo).ClearRoleUser(roleId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_ClearRoleUser, MethodBase.GetCurrentMethod());
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
                        num = new BaseRoleManager(helper, userInfo).Delete(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_Delete, MethodBase.GetCurrentMethod());
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

        public DataTable GetDT(BaseUserInfo userInfo)
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
                        table = new BaseRoleManager(helper, userInfo).GetDT(BaseRoleEntity.FieldDeletionStateCode, 0, BaseRoleEntity.FieldIsVisible, 1, BaseRoleEntity.FieldSortCode);
                        table.TableName = BaseRoleEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_GetDT, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByIds(BaseUserInfo userInfo, string[] ids)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseRoleEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseRoleManager(helper, userInfo).GetDT(BaseRoleEntity.FieldId, (object[]) ids, BaseRoleEntity.FieldSortCode);
                        table.TableName = BaseRoleEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_GetDTByIds, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByOrganize(BaseUserInfo userInfo, string systemId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dTBySystem = new DataTable(BaseRoleEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        dTBySystem = new BaseRoleManager(helper, userInfo).GetDTBySystem(systemId);
                        dTBySystem.TableName = BaseRoleEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_GetDTByOrganize, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dTBySystem;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public BaseRoleEntity GetEntity(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseRoleEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        entity = new BaseRoleManager(helper, userInfo).GetEntity(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_GetEntity, MethodBase.GetCurrentMethod());
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

        /// <summary>
        /// 获取拥有某角色的所有用户ID列表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string[] GetRoleUserIds(BaseUserInfo userInfo, string roleId)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] userIds = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        userIds = new BaseUserRoleManager(helper, userInfo).GetUserIds(roleId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_GetRoleUserIds, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return userIds;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int MoveTo(BaseUserInfo userInfo, string id, string targetId)
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
                        num = new BaseRoleManager(helper, userInfo).MoveTo(id, targetId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_MoveTo, MethodBase.GetCurrentMethod());
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

        public int RemoveUserFromRole(BaseUserInfo userInfo, string roleId, string[] userIds)
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
                        helper.BeginTransaction();
                        BaseUserRoleManager manager = new BaseUserRoleManager(helper, userInfo);
                        if (userIds != null)
                        {
                            num += manager.RemoveFormRole(userIds, roleId);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_RemoveUserFromRole, MethodBase.GetCurrentMethod());
                        helper.CommitTransaction();
                    }
                    catch (Exception exception)
                    {
                        helper.RollbackTransaction();
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

        public int ResetSortCode(BaseUserInfo userInfo, string organizeId)
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
                        num = new BaseRoleManager(helper).ResetSortCode(organizeId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_ResetSortCode, MethodBase.GetCurrentMethod());
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

        public DataTable Search(BaseUserInfo userInfo, string organizeId, string searchValue)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseRoleEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseRoleManager(helper, userInfo).Search(organizeId, searchValue);
                        table.TableName = BaseRoleEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_Search, MethodBase.GetCurrentMethod());
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
                        num = new BaseRoleManager(helper, userInfo).SetDeleted((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_SetDeleted, MethodBase.GetCurrentMethod());
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

        public int Update(BaseUserInfo userInfo, BaseRoleEntity roleEntity, out string statusCode, out string statusMessage)
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
                        helper.BeginTransaction();
                        BaseRoleManager manager = new BaseRoleManager(helper, userInfo);
                        num = manager.Update(roleEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.RoleService_Update, MethodBase.GetCurrentMethod());
                        helper.CommitTransaction();
                    }
                    catch (Exception exception)
                    {
                        helper.RollbackTransaction();
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

