namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.ServiceModel;

    [ServiceContract]
    public interface ISequenceService
    {
        [OperationContract]
        string Add(BaseUserInfo userInfo, BaseSequenceEntity sequenceEntity, out string statusCode, out string statusMessage);
        [OperationContract]
        int BatchDelete(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Delete(BaseUserInfo userInfo, string id);
        [OperationContract]
        string[] GetBatchSequence(BaseUserInfo userInfo, string fullName, int count);
        [OperationContract]
        DataTable GetDT(BaseUserInfo userInfo);
        [OperationContract]
        BaseSequenceEntity GetEntity(BaseUserInfo userInfo, string id);
        [OperationContract]
        string GetNewSequence(BaseUserInfo userInfo, string fullName, int defaultSequence, int sequenceLength, bool fillZeroPrefix);
        //[OperationContract]
        //string GetOldSequence(BaseUserInfo userInfo, string fullName, int defaultSequence, int sequenceLength, bool fillZeroPrefix);
        [OperationContract]
        string GetReduction(BaseUserInfo userInfo, string fullName);
        [OperationContract]
        string GetSequence(BaseUserInfo userInfo, string fullName);
        [OperationContract]
        int Reset(BaseUserInfo userInfo, string[] ids);
        [OperationContract]
        int Update(BaseUserInfo userInfo, BaseSequenceEntity baseSequenceEntity, out string statusCode, out string statusMessage);
    }
}

