namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class UserCenterDbHelperService : DbHelperService
    {
        public UserCenterDbHelperService()
        {
            base.ServiceDbConnection = BaseSystemInfo.UserCenterDbConnection;
            base.ServiceDbType = BaseSystemInfo.UserCenterDbType;
        }

        public UserCenterDbHelperService(string dbConnection)
        {
            base.ServiceDbConnection = dbConnection;
        }
    }
}

