namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface ILogOnService
    {
        [OperationContract]
        BaseUserInfo AccountActivation(BaseUserInfo userInfo, string openId, out string statusCode, out string statusMessage);
        [OperationContract]
        int ChangeCommunicationPassword(BaseUserInfo userInfo, string oldPassword, string newPassword, out string statusCode, out string statusMessage);
        [OperationContract]
        int ChangePassword(BaseUserInfo userInfo, string oldPassword, string newPassword, out string statusCode, out string statusMessage);
        [OperationContract]
        int ChangeSignedPassword(BaseUserInfo userInfo, string oldPassword, string newPassword, out string statusCode, out string statusMessage);
        [OperationContract]
        bool CommunicationPassword(BaseUserInfo userInfo, string communicationPassword);
        [OperationContract]
        string CreateDigitalSignature(BaseUserInfo userInfo, string password, out string statusCode, out string statusMessage);
        [OperationContract]
        string GetPublicKey(BaseUserInfo userInfo, string userId);
        [OperationContract]
        DataTable GetStaffUserDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetUserDT(BaseUserInfo userInfo);
        [OperationContract]
        BaseUserInfo LogOnByOpenId(BaseUserInfo userInfo, string openId, out string statusCode, out string statusMessage);
        [OperationContract]
        BaseUserInfo LogOnByUserName(BaseUserInfo userInfo, string userName, out string statusCode, out string statusMessage);
        [OperationContract(IsOneWay=true)]
        void OnExit(BaseUserInfo userInfo);
        [OperationContract(IsOneWay=true)]
        void OnLine(BaseUserInfo userInfo, int onLineState);
        [OperationContract]
        int ServerCheckOnLine();
        [OperationContract]
        int SetCommunicationPassword(BaseUserInfo userInfo, string[] userIds, string password, out string statusCode, out string statusMessage);
        [OperationContract]
        int SetPassword(BaseUserInfo userInfo, string[] userIds, string password, out string statusCode, out string statusMessage);
        [OperationContract]
        bool SignedPassword(BaseUserInfo userInfo, string communicationPassword);
        [OperationContract]
        BaseUserInfo UserLogOn(BaseUserInfo userInfo, string userName, string password, bool createOpenId, out string statusCode, out string statusMessage);
    }
}

