namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public abstract class DbHelperService : MarshalByRefObject, IDbHelperService
    {
        public string ServiceDbConnection;
        public CurrentDbType ServiceDbType;

        public DbHelperService()
        {
            this.ServiceDbConnection = BaseSystemInfo.BusinessDbConnection;
            this.ServiceDbType = BaseSystemInfo.BusinessDbType;
        }

        public DbHelperService(string dbConnection)
        {
            this.ServiceDbConnection = BaseSystemInfo.BusinessDbConnection;
            this.ServiceDbType = BaseSystemInfo.BusinessDbType;
            this.ServiceDbConnection = dbConnection;
        }

        public int ExecuteNonQuery(BaseUserInfo userInfo, string commandText)
        {
            LogOnService.UserIsLogOn(userInfo);
            return DbHelperFactory.GetHelper(this.ServiceDbType, this.ServiceDbConnection).ExecuteNonQuery(commandText);
        }

        public int ExecuteNonQuery(BaseUserInfo userInfo, string commandText, DbParameter[] dbParameters)
        {
            LogOnService.UserIsLogOn(userInfo);
            return DbHelperFactory.GetHelper(this.ServiceDbType, this.ServiceDbConnection).ExecuteNonQuery(commandText, dbParameters);
        }

        public object ExecuteScalar(BaseUserInfo userInfo, string commandText)
        {
            LogOnService.UserIsLogOn(userInfo);
            return DbHelperFactory.GetHelper(this.ServiceDbType, this.ServiceDbConnection).ExecuteScalar(commandText);
        }

        public object ExecuteScalar(BaseUserInfo userInfo, string commandText, DbParameter[] dbParameters)
        {
            LogOnService.UserIsLogOn(userInfo);
            return DbHelperFactory.GetHelper(this.ServiceDbType, this.ServiceDbConnection).ExecuteScalar(commandText, dbParameters);
        }

        public DataTable Fill(BaseUserInfo userInfo, string commandText)
        {
            LogOnService.UserIsLogOn(userInfo);
            return DbHelperFactory.GetHelper(this.ServiceDbType, this.ServiceDbConnection).Fill(commandText);
        }

        public DataTable Fill(BaseUserInfo userInfo, string commandText, DbParameter[] dbParameters)
        {
            LogOnService.UserIsLogOn(userInfo);
            return DbHelperFactory.GetHelper(this.ServiceDbType, this.ServiceDbConnection).Fill(commandText, dbParameters);
        }
    }
}

