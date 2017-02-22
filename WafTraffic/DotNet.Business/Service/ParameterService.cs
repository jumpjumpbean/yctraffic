namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class ParameterService : MarshalByRefObject, IParameterService
    {
        private string serviceName = AppMessage.ParameterService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

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
                        BaseParameterManager manager = new BaseParameterManager(helper, userInfo);
                        for (int i = 0; i < ids.Length; i++)
                        {
                            num += manager.Delete(ids[i]);
                        }
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ParameterService_BatchDelete, MethodBase.GetCurrentMethod());
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
                        num = new BaseParameterManager(helper, userInfo).Delete(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ParameterService_Delete, MethodBase.GetCurrentMethod());
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

        public int DeleteByParameter(BaseUserInfo userInfo, string categoryId, string parameterId)
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
                        num = new BaseParameterManager(helper, userInfo).DeleteByParameter(categoryId, parameterId);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ParameterService_DeleteByParameter, MethodBase.GetCurrentMethod());
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

        public int DeleteByParameterCode(BaseUserInfo userInfo, string categoryId, string parameterId, string parameterCode)
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
                        num = new BaseParameterManager(helper, userInfo).DeleteByParameterCode(categoryId, parameterId, parameterCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ParameterService_DeleteByParameterCode, MethodBase.GetCurrentMethod());
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

        public DataTable GetDTByParameter(BaseUserInfo userInfo, string categoryId, string parameterId)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dTByParameter = new DataTable(BaseParameterEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        dTByParameter = new BaseParameterManager(helper, userInfo).GetDTByParameter(categoryId, parameterId);
                        dTByParameter.TableName = BaseParameterEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, AppMessage.ParameterService_GetDTByParameter, this.serviceName, MethodBase.GetCurrentMethod());
                    }
                    catch (Exception exception)
                    {
                        BaseExceptionManager.LogException(helper, userInfo, exception);
                        throw exception;
                    }
                    return dTByParameter;
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        public DataTable GetDTParameterCode(BaseUserInfo userInfo, string categoryId, string parameterId, string parameterCode)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable table = new DataTable(BaseParameterEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        table = new BaseParameterManager(helper, userInfo).GetDTParameterCode(categoryId, parameterId, parameterCode);
                        table.TableName = BaseParameterEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ParameterService_GetDTParameterCode, MethodBase.GetCurrentMethod());
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

        public string GetParameter(BaseUserInfo userInfo, string categoryId, string parameterId, string parameterCode)
        {
            string str = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        str = new BaseParameterManager(helper, userInfo).GetParameter(categoryId, parameterId, parameterCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ParameterService_GetParameter, MethodBase.GetCurrentMethod());
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

        public string GetServiceConfig(BaseUserInfo userInfo, string key)
        {
            return UserConfigHelper.GetValue(key);
        }

        public int SetParameter(BaseUserInfo userInfo, string categoryId, string parameterId, string parameterCode, string parameterContent)
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
                        num = new BaseParameterManager(helper, userInfo).SetParameter(categoryId, parameterId, parameterCode, parameterContent);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.ParameterService_SetParameter, MethodBase.GetCurrentMethod());
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
                        table = new BaseParameterManager(helper, userInfo).GetDT(BaseParameterEntity.FieldDeletionStateCode, 0, BaseRoleEntity.FieldSortCode);
                        table.TableName = BaseParameterEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, "获取参数列表", MethodBase.GetCurrentMethod());
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
    }
}

