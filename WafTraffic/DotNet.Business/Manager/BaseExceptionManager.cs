namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;

    public class BaseExceptionManager : BaseManager
    {
        public BaseExceptionManager()
        {
            base.CurrentTableName = BaseExceptionEntity.TableName;
        }

        public BaseExceptionManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseExceptionManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public string AddEntity(Exception ex)
        {
            string targetValue = BaseBusinessLogic.NewGuid();
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginInsert(BaseExceptionEntity.TableName);
            builder.SetValue(BaseExceptionEntity.FieldId, targetValue, null);
            builder.SetValue(BaseExceptionEntity.FieldMessage, ex.Message, null);
            builder.SetValue(BaseExceptionEntity.FieldThreadName, ex.Source, null);
            builder.SetValue(BaseExceptionEntity.FieldFormattedMessage, ex.StackTrace, null);
            if (base.UserInfo != null)
            {
                builder.SetValue(BaseExceptionEntity.FieldIPAddress, base.UserInfo.IPAddress, null);
                builder.SetValue(BaseExceptionEntity.FieldCreateUserId, base.UserInfo.Id, null);
                builder.SetValue(BaseExceptionEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            builder.SetDBNow(BaseExceptionEntity.FieldCreateOn);
            if (builder.EndInsert() <= 0)
            {
                return string.Empty;
            }
            return targetValue;
        }

        public BaseExceptionEntity GetEntity(string id)
        {
            return new BaseExceptionEntity(this.GetDTById(id));
        }

        public string LogException(BaseUserInfo userInfo, Exception ex)
        {
            return LogException(base.DbHelper, userInfo, ex);
        }

        public static string LogException(IDbHelper dbHelper, BaseUserInfo userInfo, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(ex.InnerException);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(string.Empty);
            string str = string.Empty;
            if (BaseSystemInfo.LogException)
            {
                if (BaseSystemInfo.EventLog)
                {
                    if (!EventLog.SourceExists(BaseSystemInfo.SoftName))
                    {
                        EventLog.CreateEventSource(BaseSystemInfo.SoftName, BaseSystemInfo.SoftFullName);
                    }
                    new EventLog { Source = BaseSystemInfo.SoftName }.WriteEntry(ex.Message, EventLogEntryType.Error);
                }
                if (((dbHelper != null) && (dbHelper.GetDbConnection() != null)) && (dbHelper.GetDbConnection().State == ConnectionState.Open))
                {
                    str = new BaseExceptionManager(dbHelper, userInfo).AddEntity(ex);
                }
            }
            return str;
        }

        public DataTable Search(string searchValue)
        {
            string commandText = string.Empty;
            commandText = " SELECT *  FROM " + BaseExceptionEntity.TableName + " WHERE 1 = 1 ";
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                string str2 = commandText;
                string str3 = str2 + " AND (" + BaseExceptionEntity.FieldIPAddress + " LIKE " + base.DbHelper.GetParameter(BaseExceptionEntity.FieldIPAddress);
                string str4 = str3 + " OR " + BaseExceptionEntity.FieldFormattedMessage + " LIKE " + base.DbHelper.GetParameter(BaseExceptionEntity.FieldFormattedMessage);
                string str5 = str4 + " OR " + BaseExceptionEntity.FieldProcessName + " LIKE " + base.DbHelper.GetParameter(BaseExceptionEntity.FieldProcessName);
                string str6 = str5 + " OR " + BaseExceptionEntity.FieldMachineName + " LIKE " + base.DbHelper.GetParameter(BaseExceptionEntity.FieldMachineName);
                commandText = str6 + " OR " + BaseExceptionEntity.FieldMessage + " LIKE " + base.DbHelper.GetParameter(BaseExceptionEntity.FieldMessage) + ")";
                searchValue = searchValue.Trim();
                if (searchValue.IndexOf("%") < 0)
                {
                    searchValue = "%" + searchValue + "%";
                }
                list.Add(base.DbHelper.MakeParameter(BaseExceptionEntity.FieldIPAddress, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseExceptionEntity.FieldFormattedMessage, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseExceptionEntity.FieldProcessName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseExceptionEntity.FieldMachineName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseExceptionEntity.FieldMessage, searchValue));
            }
            DataTable dataTable = new DataTable(BaseExceptionEntity.TableName);
            base.DbHelper.Fill(dataTable, commandText, list.ToArray());
            return dataTable;
        }
    }
}

