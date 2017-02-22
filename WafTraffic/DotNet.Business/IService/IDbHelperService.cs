namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.ServiceModel;

    [ServiceContract]
    public interface IDbHelperService
    {
        [OperationContract(Name="ExecuteNonQuery")]
        int ExecuteNonQuery(BaseUserInfo userInfo, string commandText);
        [OperationContract(Name="ExecuteNonQuery2")]
        int ExecuteNonQuery(BaseUserInfo userInfo, string commandText, DbParameter[] dbParameters);
        [OperationContract(Name="ExecuteScalar")]
        object ExecuteScalar(BaseUserInfo userInfo, string commandText);
        [OperationContract(Name="ExecuteScalar2")]
        object ExecuteScalar(BaseUserInfo userInfo, string commandText, DbParameter[] dbParameters);
        [OperationContract(Name="Fill")]
        DataTable Fill(BaseUserInfo userInfo, string commandText);
        [OperationContract(Name="Fill2")]
        DataTable Fill(BaseUserInfo userInfo, string commandText, DbParameter[] dbParameters);
    }
}

