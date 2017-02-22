namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Reflection;

    public class BaseLogManager : BaseManager
    {
        private static BaseLogManager instance = null;
        private static object locker = new object();

        public BaseLogManager()
        {
            base.Identity = true;
            base.ReturnId = false;
            base.CurrentTableName = BaseLogEntity.TableName;
        }

        public BaseLogManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseLogManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public void Add(IDbHelper dbHelper, BaseLogEntity logEntity)
        {
            base.DbHelper = dbHelper;
            this.AddEntity(logEntity);
        }

        public void Add(IDbHelper dbHelper, BaseUserInfo userInfo, MethodBase methodBase)
        {
            base.DbHelper = dbHelper;
            base.UserInfo = userInfo;
            this.Add(base.DbHelper, base.UserInfo, methodBase.ReflectedType.FullName, methodBase.ReflectedType.Name, methodBase);
        }

        public void Add(IDbHelper dbHelper, BaseUserInfo userInfo, string serviceName, MethodBase methodBase)
        {
            base.DbHelper = dbHelper;
            base.UserInfo = userInfo;
            this.Add(base.DbHelper, base.UserInfo, serviceName, methodBase.ReflectedType.FullName, methodBase);
        }

        public void Add(IDbHelper dbHelper, BaseUserInfo userInfo, string serviceName, string methodName, MethodBase methodBase)
        {
            base.DbHelper = dbHelper;
            base.UserInfo = userInfo;
            this.Add(dbHelper, userInfo, serviceName, methodName, methodBase.ReflectedType.Name, methodBase.Name, string.Empty);
        }

        public void Add(IDbHelper dbHelper, BaseUserInfo userInfo, string processName, string methodName, string processId, string methodId, string parameters)
        {
            base.DbHelper = dbHelper;
            base.UserInfo = userInfo;
            if (BaseSystemInfo.RecordLog)
            {
                BaseLogEntity logEntity = new BaseLogEntity {
                    UserId = userInfo.Id,
                    UserRealName = userInfo.RealName,
                    ProcessId = processId,
                    ProcessName = processName,
                    MethodId = methodId,
                    MethodName = methodName,
                    Parameters = parameters,
                    IPAddress = userInfo.IPAddress
                };
                this.Add(dbHelper, logEntity);
            }
        }

        public void Add(string userId, string realName, string processId, string processName, string methodId, string methodName, string parameters, string ipAddress, string description)
        {
            BaseLogEntity logEntity = new BaseLogEntity {
                UserId = userId,
                UserRealName = realName,
                ProcessId = processId,
                ProcessName = processName,
                MethodName = methodName,
                MethodId = methodId,
                Parameters = parameters,
                IPAddress = ipAddress,
                Description = description
            };
            this.AddEntity(logEntity);
        }

        public void Add(IDbHelper dbHelper, string userId, string realName, string processId, string processName, string methodId, string methodName, string parameters, string ipAddress, string description)
        {
            base.DbHelper = dbHelper;
            this.Add(userId, realName, processId, processName, methodId, methodName, parameters, ipAddress, description);
        }

        public int AddEntity(BaseLogEntity logEntity)
        {
            string sequence = string.Empty;
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginInsert(BaseLogEntity.TableName, base.Identity);
            if (!base.Identity)
            {
                builder.SetValue(BaseLogEntity.FieldId, logEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    builder.SetFormula(BaseLogEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    builder.SetFormula(BaseLogEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (string.IsNullOrEmpty(logEntity.Id))
                {
                    if (string.IsNullOrEmpty(sequence))
                    {
                        sequence = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    logEntity.Id = sequence;
                }
                builder.SetValue(BaseLogEntity.FieldId, logEntity.Id, null);
            }
            if (string.IsNullOrEmpty(logEntity.UserId))
            {
                logEntity.UserId = logEntity.IPAddress;
            }
            builder.SetValue(BaseLogEntity.FieldUserId, logEntity.UserId, null);
            builder.SetValue(BaseLogEntity.FieldUserRealName, logEntity.UserRealName, null);
            builder.SetValue(BaseLogEntity.FieldProcessId, logEntity.ProcessId, null);
            builder.SetValue(BaseLogEntity.FieldProcessName, logEntity.ProcessName, null);
            builder.SetValue(BaseLogEntity.FieldMethodId, logEntity.MethodId, null);
            builder.SetValue(BaseLogEntity.FieldMethodName, logEntity.MethodName, null);
            builder.SetValue(BaseLogEntity.FieldParameters, logEntity.Parameters, null);
            builder.SetValue(BaseLogEntity.FieldUrlReferrer, logEntity.UrlReferrer, null);
            builder.SetValue(BaseLogEntity.FieldWebUrl, logEntity.WebUrl, null);
            builder.SetValue(BaseLogEntity.FieldIPAddress, logEntity.IPAddress, null);
            builder.SetValue(BaseLogEntity.FieldDescription, logEntity.Description, null);
            builder.SetDBNow(BaseLogEntity.FieldCreateOn);
            if (base.DbHelper.CurrentDbType == CurrentDbType.SqlServer)
            {
                return builder.EndInsert();
            }
            builder.EndInsert();
            if (base.ReturnId)
            {
                return int.Parse(logEntity.Id);
            }
            return 0;
        }

        public void AddWebLog(string urlReferrer, string adId, string webUrl)
        {
            string userId = string.Empty;
            if (!base.UserInfo.Id.Equals(base.UserInfo.IPAddress))
            {
                userId = base.UserInfo.Id;
            }
            string userName = string.Empty;
            if (!base.UserInfo.UserName.Equals(base.UserInfo.IPAddress))
            {
                userName = base.UserInfo.UserName;
            }
            this.AddWebLog(urlReferrer, adId, webUrl, base.UserInfo.IPAddress, userId, userName);
        }

        public void AddWebLog(string urlReferrer, string adId, string webUrl, string ipAddress, string userId, string userName)
        {
            BaseLogEntity logEntity = new BaseLogEntity {
                ProcessId = "WebLog",
                UrlReferrer = urlReferrer
            };
            if (!string.IsNullOrEmpty(adId))
            {
                logEntity.MethodName = "AD";
                logEntity.Parameters = adId;
            }
            logEntity.WebUrl = webUrl;
            logEntity.IPAddress = ipAddress;
            logEntity.UserId = userId;
            logEntity.UserRealName = userName;
            this.AddEntity(logEntity);
        }

        public DataTable GetDTByDate(IDbHelper dbHelper, string createOn, string processId, string createUserId)
        {
            string commandText = (" SELECT * FROM " + BaseLogEntity.TableName + " WHERE CONVERT(NVARCHAR, " + BaseLogEntity.FieldCreateOn + ", 111) = " + dbHelper.GetParameter(BaseLogEntity.FieldCreateOn) + " AND " + BaseLogEntity.FieldProcessId + " = " + dbHelper.GetParameter(BaseLogEntity.FieldProcessId) + " AND " + BaseLogEntity.FieldUserId + " = " + dbHelper.GetParameter(BaseLogEntity.FieldUserId)) + " ORDER BY " + BaseLogEntity.FieldCreateOn;
            string[] targetFileds = new string[] { BaseLogEntity.FieldCreateOn, BaseLogEntity.FieldProcessName, BaseLogEntity.FieldUserId };
            object[] targetValues = new object[] { createOn, processId, createUserId };
            DataTable dataTable = new DataTable(BaseLogEntity.TableName);
            dbHelper.Fill(dataTable, commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public DataTable GetDTByDate(string name, string value, string beginDate, string endDate)
        {
            string commandText = this.GetDTSql(null, name, value, beginDate, endDate);
            return base.DbHelper.Fill(commandText);
        }

        public DataTable GetDTByDateByUserIds(string[] userIds, string name, string value, string beginDate, string endDate)
        {
            string commandText = this.GetDTSql(userIds, name, value, beginDate, endDate);
            return base.DbHelper.Fill(commandText);
        }

        private string GetDTSql(string[] userIds, string name, string value, string beginDate, string endDate)
        {
            string str = " SELECT * FROM " + BaseLogEntity.TableName + " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(value))
            {
                string str2 = str;
                str = str2 + " AND " + name + " = '" + value + "' ";
            }
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
            {
                beginDate = DateTime.Parse(beginDate.ToString()).ToShortDateString();
                endDate = DateTime.Parse(endDate.ToString()).AddDays(1.0).ToShortDateString();
            }
            if (userIds != null)
            {
                string str3 = str;
                str = str3 + " AND " + BaseLogEntity.FieldUserId + " IN (" + BaseBusinessLogic.ObjectsToList(userIds) + ") ";
            }
            switch (base.DbHelper.CurrentDbType)
            {
                case CurrentDbType.Oracle:
                    if (beginDate.Trim().Length > 0)
                    {
                        str = str + " AND CreateOn >= TO_DATE( '" + beginDate + "','yyyy-mm-dd hh24-mi-ss') ";
                    }
                    if (endDate.Trim().Length > 0)
                    {
                        str = str + " AND CreateOn <= TO_DATE('" + endDate + "','yyyy-mm-dd hh24-mi-ss')";
                    }
                    break;

                case CurrentDbType.SqlServer:
                case CurrentDbType.Access:
                    if (beginDate.Trim().Length > 0)
                    {
                        str = str + " AND CreateOn >= '" + beginDate + "'";
                    }
                    if (endDate.Trim().Length > 0)
                    {
                        str = str + " AND CreateOn <= '" + endDate + "'";
                    }
                    break;
            }
            return (str + " ORDER BY CreateOn DESC ");
        }

        public static BaseLogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new BaseLogManager();
                        }
                    }
                }
                return instance;
            }
        }
    }
}

