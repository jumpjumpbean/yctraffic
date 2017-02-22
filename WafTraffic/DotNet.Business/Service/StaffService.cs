//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

/// 修改纪录
/// 
///		2012.07.06 版本：2.0 sunmiao 修改
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

    /// <summary>
    /// 员工管理服务
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class StaffService : MarshalByRefObject, IStaffService
    {
        private string serviceName = AppMessage.StaffService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string AddStaff(BaseUserInfo userInfo, BaseStaffEntity staffEntity, out string statusCode, out string statusMessage)
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
                        BaseStaffManager manager = new BaseStaffManager(helper, userInfo);
                        str = manager.Add(staffEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_AddStaff, MethodBase.GetCurrentMethod());
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
                        num = new BaseStaffManager(helper, userInfo).BatchDelete(ids);
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
                        BaseStaffManager manager = new BaseStaffManager(helper, userInfo);
                        for (int i = 0; i < ids.Length; i++)
                        {
                            num += manager.SetProperty(ids[i], BaseStaffEntity.FieldDepartmentId, organizeId);
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
                        num = new BaseStaffManager(helper, userInfo).BatchSave(dataTable);
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

        public int BatchUpdateAddress(BaseUserInfo userInfo, List<BaseStaffEntity> staffEntites, out string statusCode, out string statusMessage)
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
                        statusCode = string.Empty;
                        BaseStaffManager manager = new BaseStaffManager(helper, userInfo);
                        for (int i = 0; i < staffEntites.Count; i++)
                        {
                            num += manager.UpdateAddress(staffEntites[i], out statusCode);
                        }
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_BatchUpdateAddress, MethodBase.GetCurrentMethod());
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

        public int ChangeDepartment(BaseUserInfo userInfo, string id, string CompanyId, string DepartmentId)
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
                        BaseStaffManager manager = new BaseStaffManager(helper, userInfo);
                        string[] targetFields = new string[2];
                        object[] targetValues = new object[2];
                        targetFields[0] = BaseStaffEntity.FieldCompanyId;
                        targetValues[0] = CompanyId;
                        targetFields[1] = BaseStaffEntity.FieldDepartmentId;
                        targetValues[1] = DepartmentId;
                        num = manager.SetProperty(id, targetFields, targetValues);
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
                        num = new BaseStaffManager(helper, userInfo).Delete(id);
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

        public int DeleteUser(BaseUserInfo userInfo, string staffId)
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
                        num = new BaseStaffManager(helper, userInfo).DeleteUser(staffId);
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

        public DataTable GetAddressDT(BaseUserInfo userInfo, string organizeId, string searchValue)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable addressDT = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        addressDT = new BaseStaffManager(helper, userInfo).GetAddressDT(organizeId, searchValue);
                        addressDT.TableName = BaseStaffEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_GetAddressDT, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return addressDT;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetAddressPageDT(BaseUserInfo userInfo, string organizeId, string searchValue, int pageSize, int currentPage, out int recordCount)
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
                        table = new BaseStaffManager(helper, userInfo).GetAddressPageDT(out recordCount, organizeId, searchValue, pageSize, currentPage);
                        table.TableName = BaseStaffEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_GetAddressPageDT, MethodBase.GetCurrentMethod());
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

        public DataTable GetChildrenStaffs(BaseUserInfo userInfo, string organizeId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable childrenStaffs = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        childrenStaffs = new BaseStaffManager(helper, userInfo).GetChildrenStaffs(organizeId);
                        childrenStaffs.DefaultView.Sort = BaseStaffEntity.FieldSortCode;
                        childrenStaffs.TableName = BaseStaffEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return childrenStaffs;
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
            DataTable table = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseStaffManager(helper, userInfo).GetDT(BaseStaffEntity.FieldDeletionStateCode, 0, BaseStaffEntity.FieldSortCode);
                        table.TableName = BaseStaffEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_GetDT, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByDepartment(BaseUserInfo userInfo, string departmentId, bool containChildren)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable childrenStaffs = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseStaffManager manager = new BaseStaffManager(helper, userInfo);
                        if (containChildren)
                        {
                            childrenStaffs = manager.GetChildrenStaffs(departmentId);
                        }
                        else
                        {
                            childrenStaffs = manager.GetDTByDepartment(departmentId);
                        }
                        childrenStaffs.TableName = BaseStaffEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_GetDTByDepartment, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return childrenStaffs;
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
                        table = new BaseStaffManager(helper, userInfo).GetDT(BaseStaffEntity.FieldId, (object[]) ids, BaseStaffEntity.FieldSortCode);
                        table.TableName = BaseStaffEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_GetDTByIds, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByOrganize(BaseUserInfo userInfo, string organizeId, bool containChildren)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable childrenStaffs = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        string commandText = "UPDATE Base_Staff SET UserId = NULL WHERE (UserId NOT IN (SELECT Id FROM Base_User))";
                        helper.ExecuteNonQuery(commandText);
                        BaseStaffManager manager = new BaseStaffManager(helper, userInfo);
                        if (containChildren)
                        {
                            childrenStaffs = manager.GetChildrenStaffs(organizeId);
                        }
                        else
                        {
                            childrenStaffs = manager.GetDTByOrganize(organizeId);
                        }
                        childrenStaffs.TableName = BaseStaffEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_GetDTByOrganize, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return childrenStaffs;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public BaseStaffEntity GetEntity(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseStaffEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        entity = new BaseStaffManager(helper, userInfo).GetEntity(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_GetEntity, MethodBase.GetCurrentMethod());
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

        public string GetId(BaseUserInfo userInfo, string name, object value)
        {
            LogOnService.UserIsLogOn(userInfo);
            string id = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        id = new BaseStaffManager(helper).GetId(name, value);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return id;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetParentChildrenStaffs(BaseUserInfo userInfo, string organizeId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable parentChildrenStaffs = new DataTable(BaseStaffEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        parentChildrenStaffs = new BaseStaffManager(helper, userInfo).GetParentChildrenStaffs(organizeId);
                        parentChildrenStaffs.DefaultView.Sort = BaseStaffEntity.FieldSortCode;
                        parentChildrenStaffs.TableName = BaseStaffEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return parentChildrenStaffs;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public int MoveTo(BaseUserInfo userInfo, string id, string organizeId)
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
                        num = new BaseStaffManager(helper, userInfo).SetProperty(id, BaseStaffEntity.FieldDepartmentId, organizeId);
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

        public int ResetSortCode(BaseUserInfo userInfo)
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
                        num = new BaseStaffManager(helper).ResetSortCode();
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

        public DataTable Search(BaseUserInfo userInfo, string organizeId, string searchValue)
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
                        table = new BaseStaffManager(helper, userInfo).Search(organizeId, searchValue, false);
                        table.TableName = BaseStaffEntity.TableName;
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
                        num = new BaseStaffManager(helper, userInfo).SetDeleted((object[]) ids);
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

        public int SetStaffUser(BaseUserInfo userInfo, string staffId, string userId)
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
                        BaseStaffManager manager = new BaseStaffManager(helper, userInfo);
                        if (string.IsNullOrEmpty(userId))
                        {
                            num = manager.SetProperty(staffId, BaseStaffEntity.FieldUserId, userId);
                        }
                        else
                        {
                            string[] strArray = manager.GetIds(BaseStaffEntity.FieldUserId, userId, BaseStaffEntity.FieldDeletionStateCode, 0);
                            if ((strArray == null) || (strArray.Length == 0))
                            {
                                BaseUserEntity entity = new BaseUserManager(helper, userInfo).GetEntity(userId);
                                //num = manager.SetProperty(staffId, BaseStaffEntity.FieldUserId, userId);
                                //num = manager.SetProperty(staffId, BaseStaffEntity.FieldUserName, entity.UserName);

                                string[] names = new string[] { BaseStaffEntity.FieldUserId, BaseStaffEntity.FieldUserName };
                                object[] values = new object[] { userId, entity.UserName };
                                num = manager.SetProperty(staffId, names, values);
                            }
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_SetStaffUser, MethodBase.GetCurrentMethod());
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

        public int UpdateAddress(BaseUserInfo userInfo, BaseStaffEntity staffEntity, out string statusCode, out string statusMessage)
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
                        statusCode = string.Empty;
                        BaseStaffManager manager = new BaseStaffManager(helper, userInfo);
                        num += manager.UpdateAddress(staffEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_UpdateAddress, MethodBase.GetCurrentMethod());
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

        public int UpdateStaff(BaseUserInfo userInfo, BaseStaffEntity staffEntity, out string statusCode, out string statusMessage)
        {
            if (!BaseSystemInfo.IsAuthorized(userInfo))
            {
                throw new Exception(AppMessage.MSG0800);
            }
            int num = 0;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseStaffManager manager = new BaseStaffManager(helper, userInfo);
                        num = manager.Update(staffEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.StaffService_UpdateStaff, MethodBase.GetCurrentMethod());
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

