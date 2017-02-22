namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class SequenceService : MarshalByRefObject, ISequenceService
    {
        private static object LOCK = new object();
        private string serviceName = AppMessage.SequenceService;
        private readonly string UserCenterDbConnection = BaseSystemInfo.UserCenterDbConnection;

        public string Add(BaseUserInfo userInfo, BaseSequenceEntity sequenceEntity, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            statusCode = string.Empty;
            statusMessage = string.Empty;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        BaseSequenceManager manager = new BaseSequenceManager(helper, userInfo);
                        str = manager.Add(sequenceEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.SequenceService_Add, MethodBase.GetCurrentMethod());
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

        public string Add(BaseUserInfo userInfo, DataTable dataTable, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseSequenceEntity sequenceEntity = new BaseSequenceEntity(dataTable);
            return this.Add(userInfo, sequenceEntity, out statusCode, out statusMessage);
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
                        num = new BaseSequenceManager(helper).Delete((object[]) ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.SequenceService_BatchDelete, MethodBase.GetCurrentMethod());
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
                        num = new BaseSequenceManager(helper).Delete(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.SequenceService_Delete, MethodBase.GetCurrentMethod());
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

        public string[] GetBatchSequence(BaseUserInfo userInfo, string fullName, int count)
        {
            LogOnService.UserIsLogOn(userInfo);
            string[] strArray = new string[0];
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        strArray = this.GetBatchSequence(helper, userInfo, fullName, count);
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

        public string[] GetBatchSequence(IDbHelper dbHelper, BaseUserInfo userInfo, string fullName, int count)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseSequenceManager manager = new BaseSequenceManager(dbHelper);
            return manager.GetBatchSequence(fullName, count);
        }

        public DataTable GetDT(BaseUserInfo userInfo)
        {
            LogOnService.UserIsLogOn(userInfo);
            DataTable dT = new DataTable(BaseSequenceEntity.TableName);
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        dT = new BaseSequenceManager(helper).GetDT();
                        dT.TableName = BaseSequenceEntity.TableName;
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.SequenceService_GetDT, MethodBase.GetCurrentMethod());
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

        public BaseSequenceEntity GetEntity(BaseUserInfo userInfo, string id)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseSequenceEntity entity = null;
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(this.UserCenterDbConnection);
                        entity = new BaseSequenceManager(helper, userInfo).GetEntity(id);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.SequenceService_GetEntity, MethodBase.GetCurrentMethod());
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

        public string GetNewSequence(BaseUserInfo userInfo, string fullName, int defaultSequence, int sequenceLength, bool fillZeroPrefix)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            lock (LOCK)
            {
                using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
                {
                    try
                    {
                        try
                        {
                            helper.Open(this.UserCenterDbConnection);
                            str = new BaseSequenceManager(helper).GetSequence(fullName, defaultSequence, sequenceLength, fillZeroPrefix);
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

        //public string GetOldSequence(BaseUserInfo userInfo, string fullName, int defaultSequence, int sequenceLength, bool fillZeroPrefix)
        //{
        //    LogOnService.UserIsLogOn(userInfo);
        //    string str = string.Empty;
        //    lock (LOCK)
        //    {
        //        using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
        //        {
        //            try
        //            {
        //                try
        //                {
        //                    helper.Open(this.UserCenterDbConnection);
        //                    str = new BaseSequenceManager(helper).GetOldSequence(fullName, defaultSequence, sequenceLength, fillZeroPrefix);
        //                }
        //                catch (Exception exception)
        //                {
        //                    BaseExceptionManager.LogException(helper, userInfo, exception);
        //                    throw exception;
        //                }
        //                return str;
        //            }
        //            finally
        //            {
        //                helper.Close();
        //            }
        //        }
        //    }
        //}

        public string GetReduction(BaseUserInfo userInfo, string fullName)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            lock (LOCK)
            {
                using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
                {
                    try
                    {
                        try
                        {
                            helper.Open(this.UserCenterDbConnection);
                            str = this.GetReduction(helper, userInfo, fullName);
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

        public string GetReduction(IDbHelper dbHelper, BaseUserInfo userInfo, string fullName)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseSequenceManager manager = new BaseSequenceManager(dbHelper);
            return manager.GetReduction(fullName);
        }

        public string GetSequence(BaseUserInfo userInfo, string fullName)
        {
            LogOnService.UserIsLogOn(userInfo);
            string str = string.Empty;
            lock (LOCK)
            {
                using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
                {
                    try
                    {
                        try
                        {
                            helper.Open(this.UserCenterDbConnection);
                            str = this.GetSequence(helper, userInfo, fullName);
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

        public string GetSequence(IDbHelper dbHelper, BaseUserInfo userInfo, string fullName)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseSequenceManager manager = new BaseSequenceManager(dbHelper);
            return manager.GetSequence(fullName);
        }

        public int Reset(BaseUserInfo userInfo, string[] ids)
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
                        num = new BaseSequenceManager(helper).Reset(ids);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.SequenceService_Reset, MethodBase.GetCurrentMethod());
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

        public int Update(BaseUserInfo userInfo, BaseSequenceEntity baseSequenceEntity, out string statusCode, out string statusMessage)
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
                        BaseSequenceManager manager = new BaseSequenceManager(helper, userInfo);
                        num = manager.Update(baseSequenceEntity, out statusCode);
                        statusMessage = manager.GetStateMessage(statusCode);
                        BaseLogManager.Instance.Add(helper, userInfo, this.serviceName, AppMessage.SequenceService_Update, MethodBase.GetCurrentMethod());
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

        public int Update(BaseUserInfo userInfo, DataTable dataTable, out string statusCode, out string statusMessage)
        {
            LogOnService.UserIsLogOn(userInfo);
            BaseSequenceEntity baseSequenceEntity = new BaseSequenceEntity(dataTable);
            return this.Update(userInfo, baseSequenceEntity, out statusCode, out statusMessage);
        }
    }
}

