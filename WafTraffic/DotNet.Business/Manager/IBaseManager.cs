namespace DotNet.Business
{
    using System;
    using System.Data;

    public interface IBaseManager
    {
        bool AddAfter();
        bool AddBefore();
        string AddEntity(object entity);
        int BatchSave(DataTable dataTable);
        int Delete();
        int Delete(object[] ids);
        int Delete(object id);
        int Delete(object[] ids, bool force);
        int Delete(string[] names, object[] values);
        int Delete(object id, bool force);
        int Delete(string name, object value);
        int Delete(string[] names, object[] values, bool force);
        int Delete(string name, object value, bool force);
        int Delete(string name1, object value1, string name2, object value2);
        int Delete(string name1, object value1, string name2, object value2, bool force);
        bool DeleteAfter(string id);
        bool DeleteBefore(string id);
        int DeleteEntity(object id);
        int ExecuteNonQuery(string commandText);
        int ExecuteNonQuery(string commandText, IDbDataParameter[] dbParameters);
        object ExecuteScalar(string commandText);
        object ExecuteScalar(string commandText, IDbDataParameter[] dbParameters);
        bool Exists(object id);
        bool Exists(object[] ids);
        bool Exists(string[] names, object[] values);
        bool Exists(string name, object value);
        bool Exists(string name, object value, object id);
        bool Exists(string name1, object value1, string name2, object value2);
        bool Exists(string name1, object value1, string name2, object value2, object id);
        DataTable Fill(string commandText);
        DataTable Fill(string commandText, IDbDataParameter[] dbParameters);
        bool GetAfter(string id);
        bool GetBefore(string id);
        DataTable GetDT();
        DataTable GetDT(string[] ids);
        DataTable GetDT(string order);
        DataTable GetDT(int topLimit, string order);
        DataTable GetDT(string name, object value);
        DataTable GetDT(string name, object[] values);
        DataTable GetDT(string[] names, object[] values);
        DataTable GetDT(string name, object value, string order);
        DataTable GetDT(string[] names, object[] values, string order);
        DataTable GetDT(string[] names, object[] values, int topLimit, string order);
        DataTable GetDT(string name, object value, int topLimit, string order);
        DataTable GetDT(string name1, object value1, string name2, object value2);
        DataTable GetDT(string name1, object value1, string name2, object value2, string order);
        DataTable GetDT(string name1, object value1, string name2, object value2, int topLimit, string order);
        DataTable GetDTById(string id);
        object GetFrom(DataRow dataRow);
        object GetFrom(DataTable dataTable);
        object GetFrom(DataTable dataTable, string id);
        string GetId(string[] names, object[] values);
        string GetId(string name, object value);
        string GetId(string name1, object value1, string name2, object value2);
        string[] GetIds();
        string[] GetIds(string order);
        string[] GetIds(int topLimit, string order);
        string[] GetIds(string name, object value);
        string[] GetIds(string[] names, object[] values);
        string[] GetIds(string name, object[] values);
        string[] GetIds(string[] names, object[] values, string order);
        string[] GetIds(string name, object value, string order);
        string[] GetIds(string name, object value, int topLimit, string order);
        string[] GetIds(string name1, object value1, string name2, object value2);
        string[] GetIds(string[] names, object[] values, int topLimit, string order);
        string[] GetIds(string name1, object value1, string name2, object value2, string order);
        string[] GetIds(string name1, object value1, string name2, object value2, int topLimit, string order);
        string GetProperty(object id, string targetField);
        string GetProperty(string[] names, object[] values, string targetField);
        string GetProperty(string name1, object value1, string name2, object value2, string targetField);
        int SetProperty(object[] ids, string targetField, object targetValue);
        int SetProperty(object id, string targetField, object targetValue);
        int SetProperty(object id, string[] targetFields, object[] targetValues);
        int SetProperty(string[] names, object[] values, string targetField, object targetValue);
        int SetProperty(string name1, object value1, string name2, object value2, string targetField, object targetValue);
        int Truncate();
        bool UpdateAfter();
        bool UpdateBefore();
        int UpdateEntity(object entity);
    }
}

