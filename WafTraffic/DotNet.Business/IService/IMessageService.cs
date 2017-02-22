namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface IMessageService
    {
        [OperationContract]
        int BatchSend(BaseUserInfo userInfo, string[] receiverIds, string[] organizeIds, string[] roleIds, BaseMessageEntity messageEntity);
        [OperationContract]
        int Broadcast(BaseUserInfo userInfo, BaseMessageEntity messageEntity);
        [OperationContract]
        int CheckOnLine(BaseUserInfo userInfo, int onLineState);
        [OperationContract]
        DataTable GetDTNew(BaseUserInfo userInfo, out string openId);
        [OperationContract]
        DataTable GetInnerOrganizeDT(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetOnLineState(BaseUserInfo userInfo);
        [OperationContract]
        DataTable GetUserDTByOrganize(BaseUserInfo userInfo, string organizeId);
        [OperationContract]
        string[] MessageChek(BaseUserInfo userInfo, int onLineState, string lastChekTime);
        [OperationContract]
        void Read(BaseUserInfo userInfo, string id);
        [OperationContract]
        DataTable ReadFromReceiver(BaseUserInfo userInfo, string receiverId);
        [OperationContract]
        string Send(BaseUserInfo userInfo, string receiverId, string content);
    }
}

