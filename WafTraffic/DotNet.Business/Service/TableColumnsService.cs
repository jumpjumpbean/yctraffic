namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class TableColumnsService : MarshalByRefObject, ITableColumnsService
    {
        private string serviceName = AppMessage.TableColumnsService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public int BatchDeleteConstraint(BaseUserInfo userInfo, string[] ids)
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
                        num = new BasePermissionScopeManager(helper, userInfo).SetDeleted((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.TableColumnsService_BatchDeleteConstraint, MethodBase.GetCurrentMethod());
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

        //public string[] GetColumns(BaseUserInfo userInfo, string tableCode, string permissionCode = "Column.Access") //C# 4.0 才支持缺省参数
        public string[] GetColumns(BaseUserInfo userInfo, string tableCode, string permissionCode)
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
                        strArray = new BaseTableColumnsManager(helper, userInfo).GetColumns(userInfo.Id, tableCode, permissionCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.TableColumnsService_GetDTByTable, MethodBase.GetCurrentMethod());
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

        public string[] GetColumns(BaseUserInfo userInfo, string tableCode)
        {
            return GetColumns(userInfo, tableCode, "Column.Access");
        }

        public string GetConstraint(BaseUserInfo userInfo, string resourceCategory, string resourceId, string tableName)
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
                        str = new BaseTableColumnsManager(helper, userInfo).GetConstraint(resourceCategory, resourceId, tableName, "Resource.AccessPermission");
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.TableColumnsService_SetConstraint, MethodBase.GetCurrentMethod());
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

        public DataTable GetConstraintDT(BaseUserInfo userInfo, string resourceCategory, string resourceId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseTableColumnsEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseTableColumnsManager(helper, userInfo).GetConstraintDT(resourceCategory, resourceId, "Resource.AccessPermission");
                        table.TableName = BaseTableColumnsEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.TableColumnsService_GetConstraintDT, MethodBase.GetCurrentMethod());
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

        //public BasePermissionScopeEntity GetConstraintEntity(BaseUserInfo userInfo, string resourceCategory, string resourceId, string tableName, string permissionCode = "Resource.AccessPermission") //C# 4.0 才支持缺省参数
        public BasePermissionScopeEntity GetConstraintEntity(BaseUserInfo userInfo, string resourceCategory, string resourceId, string tableName, string permissionCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            BasePermissionScopeEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        entity = new BaseTableColumnsManager(helper, userInfo).GetConstraintEntity(resourceCategory, resourceId, tableName, permissionCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.TableColumnsService_SetConstraint, MethodBase.GetCurrentMethod());
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

        public BasePermissionScopeEntity GetConstraintEntity(BaseUserInfo userInfo, string resourceCategory, string resourceId, string tableName)
        {
            string permissionCode = "Resource.AccessPermission";
            return GetConstraintEntity(userInfo, resourceCategory, resourceId, tableName, permissionCode);
        }

        public DataTable GetDTByTable(BaseUserInfo userInfo, string tableCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseTableColumnsEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseTableColumnsManager manager = new BaseTableColumnsManager(helper, userInfo);
                        string[] names = new string[] { BaseTableColumnsEntity.FieldTableCode, BaseTableColumnsEntity.FieldUseConstraint, BaseTableColumnsEntity.FieldDeletionStateCode };
                        object[] values = new object[] { tableCode, 1, 0 };
                        table = manager.GetDT(names, values, BaseTableColumnsEntity.FieldSortCode);
                        table.TableName = BaseTableColumnsEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.TableColumnsService_GetDTByTable, MethodBase.GetCurrentMethod());
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

        public string GetUserConstraint(BaseUserInfo userInfo, string tableName)
        {
            LogOnService.UserIsLogOn(userInfo);
            string userConstraint = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        userConstraint = new BaseTableColumnsManager(helper, userInfo).GetUserConstraint(tableName, "Resource.AccessPermission");
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.TableColumnsService_GetUserConstraint, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return userConstraint;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        //public string SetConstraint(BaseUserInfo userInfo, string resourceCategory, string resourceId, string tableName, string permissionCode, string constraint, bool enabled = true)//C# 4.0 才支持缺省参数
        public string SetConstraint(BaseUserInfo userInfo, string resourceCategory, string resourceId, string tableName, string permissionCode, string constraint, bool enabled)
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
                        str = new BaseTableColumnsManager(helper, userInfo).SetConstraint(resourceCategory, resourceId, tableName, permissionCode, constraint, enabled);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.TableColumnsService_SetConstraint, MethodBase.GetCurrentMethod());
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

        public string SetConstraint(BaseUserInfo userInfo, string resourceCategory, string resourceId, string tableName, string permissionCode, string constraint)
        {
            return SetConstraint(userInfo, resourceCategory, resourceId, tableName, permissionCode, constraint, true);
        }

    }
}

