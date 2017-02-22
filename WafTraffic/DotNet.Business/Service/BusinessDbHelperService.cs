namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.ServiceModel;

    [ServiceBehavior(IncludeExceptionDetailInFaults=true)]
    public class BusinessDbHelperService : DbHelperService
    {
        public BusinessDbHelperService()
        {
            base.ServiceDbConnection = BaseSystemInfo.BusinessDbConnection;
            base.ServiceDbType = BaseSystemInfo.BusinessDbType;
        }

        public BusinessDbHelperService(string dbConnection)
        {
            base.ServiceDbConnection = dbConnection;
        }
    }
}

