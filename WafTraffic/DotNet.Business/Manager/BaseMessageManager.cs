namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class BaseMessageManager : BaseManager, IBaseManager
    {
        private string Query;

        public BaseMessageManager()
        {
            this.Query = " SELECT * FROM " + BaseMessageEntity.TableName;
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseMessageEntity.TableName;
        }

        public BaseMessageManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseMessageManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseMessageManager(string tableName)
        {
            this.Query = " SELECT * FROM " + BaseMessageEntity.TableName;
            base.CurrentTableName = tableName;
        }

        public BaseMessageManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseMessageManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseMessageEntity baseMessageEntity)
        {
            return this.AddEntity(baseMessageEntity);
        }

        public string Add(BaseMessageEntity baseMessageEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseMessageEntity);
        }

        public string AddEntity(BaseMessageEntity baseMessageEntity)
        {
            string s = string.Empty;
            s = baseMessageEntity.Id;
            if (baseMessageEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseMessageEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(BaseMessageEntity.TableName, BaseMessageEntity.FieldId);
            if (baseMessageEntity.Id != null)
            {
                base.Identity = false;
            }
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseMessageEntity.FieldId, baseMessageEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseMessageEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseMessageEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (string.IsNullOrEmpty(baseMessageEntity.Id))
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseMessageEntity.Id = s;
                }
                sqlBuilder.SetValue(BaseMessageEntity.FieldId, baseMessageEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseMessageEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseMessageEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseMessageEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseMessageEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseMessageEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseMessageEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseMessageEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public int BatchSend(string[] receiverIds, string[] organizeIds, string[] roleIds, BaseMessageEntity messageEntity, bool saveSend)
        {
            string[] strArray = null;
            string[] strArray2 = null;
            string[] strArray3 = null;
            if (organizeIds != null)
            {
                BaseUserManager manager = new BaseUserManager(base.DbHelper);
                strArray = manager.GetIds(BaseUserEntity.FieldCompanyId, organizeIds, BaseUserEntity.FieldEnabled, 1);
                strArray2 = manager.GetIds(BaseUserEntity.FieldDepartmentId, organizeIds, BaseUserEntity.FieldEnabled, 1);
                strArray3 = manager.GetIds(BaseUserEntity.FieldWorkgroupId, organizeIds, BaseUserEntity.FieldEnabled, 1);
            }
            string[] userIds = null;
            if (roleIds != null)
            {
                userIds = new BaseUserRoleManager(base.DbHelper).GetUserIds(roleIds);
            }
            receiverIds = BaseBusinessLogic.Concat(new string[][] { receiverIds, strArray, strArray2, strArray3, userIds });
            return this.Send(messageEntity, receiverIds, saveSend);
        }

        public int BatchSend(string receiverId, string organizeId, string roleId, BaseMessageEntity messageEntity, bool saveSend)
        {
            string[] receiverIds = null;
            string[] organizeIds = null;
            string[] roleIds = null;
            if (!string.IsNullOrEmpty(receiverId))
            {
                receiverIds = new string[] { receiverId };
            }
            if (!string.IsNullOrEmpty(organizeId))
            {
                organizeIds = new string[] { organizeId };
            }
            if (!string.IsNullOrEmpty(roleId))
            {
                roleIds = new string[] { roleId };
            }
            return this.BatchSend(receiverIds, organizeIds, roleIds, messageEntity, saveSend);
        }

        public int Delete(string id)
        {
            return this.Delete(BaseMessageEntity.FieldId, id);
        }

        public DataTable GetDeletedDT()
        {
            DataTable dataTable = new DataTable(BaseMessageEntity.TableName);
            string commandText = this.Query + " WHERE (" + BaseMessageEntity.FieldDeletionStateCode + " = 1 )  AND (" + BaseMessageEntity.FieldReceiverId + " = " + base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId) + " )  ORDER BY " + BaseMessageEntity.FieldSortCode;
            base.DbHelper.Fill(dataTable, commandText, new IDbDataParameter[] { base.DbHelper.MakeParameter(BaseMessageEntity.FieldReceiverId, base.UserInfo.Id) });
            return dataTable;
        }

        public DataTable GetDTByFunction(string categoryCode, string functionCode, string userId)
        {
            string commandText = this.Query + "    WHERE (" + BaseMessageEntity.FieldDeletionStateCode + " = 0 )           AND (" + BaseMessageEntity.FieldCategoryCode + " = '" + categoryCode + "') ";
            if (!string.IsNullOrEmpty(functionCode))
            {
                string str2 = commandText;
                commandText = str2 + "          AND (" + BaseMessageEntity.FieldFunctionCode + " = '" + functionCode + "' ) ";
            }
            if (categoryCode.Equals(MessageCategory.Send.ToString()))
            {
                string str3 = commandText;
                commandText = str3 + "          AND (" + BaseMessageEntity.FieldCreateUserId + " = " + base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId) + ") ";
            }
            else
            {
                string str4 = commandText;
                commandText = str4 + "          AND (" + BaseMessageEntity.FieldReceiverId + " = " + base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId) + ") ";
            }
            string str5 = commandText;
            commandText = str5 + " ORDER BY " + BaseMessageEntity.FieldIsNew + " DESC           ," + BaseMessageEntity.FieldCreateOn;
            DataTable dataTable = new DataTable(BaseMessageEntity.TableName);
            base.DbHelper.Fill(dataTable, commandText, new IDbDataParameter[] { base.DbHelper.MakeParameter(BaseMessageEntity.FieldReceiverId, userId) });
            return dataTable;
        }

        public DataTable GetDTNew()
        {
            string[] strArray = new string[] { 
                this.Query, "    WHERE ", BaseMessageEntity.FieldIsNew, " = ", 1.ToString(), "          AND ", BaseMessageEntity.FieldReceiverId, " = ", base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId), "          AND ", BaseMessageEntity.FieldDeletionStateCode, " = 0           AND ", BaseMessageEntity.FieldEnabled, " = 1  ORDER BY ", BaseMessageEntity.FieldCreateUserId, "          ,", 
                BaseMessageEntity.FieldCreateOn
             };
            string commandText = string.Concat(strArray);
            DataTable dataTable = new DataTable(BaseMessageEntity.TableName);
            base.DbHelper.Fill(dataTable, commandText, new IDbDataParameter[] { base.DbHelper.MakeParameter(BaseMessageEntity.FieldReceiverId, base.UserInfo.Id) });
            return dataTable;
        }

        public BaseMessageEntity GetEntity(string id)
        {
            return new BaseMessageEntity(this.GetDT(BaseMessageEntity.FieldId, id));
        }

        public int GetNewCount()
        {
            return this.GetNewCount(MessageFunction.Message);
        }

        public int GetNewCount(MessageFunction messageFunction)
        {
            int num = 0;
            string[] strArray = new string[] { 
                " SELECT COUNT(*)    FROM ", BaseMessageEntity.TableName, "  WHERE (", BaseMessageEntity.FieldIsNew, " = ", 1.ToString(), " )         AND (", BaseMessageEntity.FieldCategoryCode, " = 'Receiver' )        AND (", BaseMessageEntity.FieldReceiverId, " = '", base.UserInfo.Id, "' )        AND (", BaseMessageEntity.FieldDeletionStateCode, " = 0 )        AND (", BaseMessageEntity.FieldFunctionCode, 
                " = '", messageFunction.ToString(), "' )"
             };
            string commandText = string.Concat(strArray);
            object obj2 = base.DbHelper.ExecuteScalar(commandText);
            if (obj2 != null)
            {
                num = int.Parse(obj2.ToString());
            }
            return num;
        }

        public BaseMessageEntity GetNewOne()
        {
            BaseMessageEntity entity = new BaseMessageEntity();
            string[] strArray = new string[] { " SELECT *    FROM (SELECT * FROM ", BaseMessageEntity.TableName, " WHERE (", BaseMessageEntity.FieldIsNew, " = ", 1.ToString(), " )          AND (", BaseMessageEntity.FieldReceiverId, " = '", base.UserInfo.Id, "')  ORDER BY ", BaseMessageEntity.FieldCreateOn, " DESC)  WHERE ROWNUM = 1 " };
            string commandText = string.Concat(strArray);
            DataTable dataTable = base.DbHelper.Fill(commandText);
            return entity.GetFrom(dataTable);
        }

        public DataTable GetOldDT()
        {
            DataTable dataTable = new DataTable(BaseMessageEntity.TableName);
            string[] strArray2 = new string[] { this.Query, " WHERE (", BaseMessageEntity.FieldIsNew, " = ", 0.ToString(), " )  AND (", BaseMessageEntity.FieldReceiverId, " = ", base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId), " )  ORDER BY ", BaseMessageEntity.FieldSortCode };
            string commandText = string.Concat(strArray2);
            string[] targetFileds = new string[1];
            object[] targetValues = new object[1];
            targetFileds[0] = BaseMessageEntity.FieldReceiverId;
            targetValues[0] = base.UserInfo.Id;
            base.DbHelper.Fill(dataTable, commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public DataTable GetReceiveDT()
        {
            return this.GetDTByFunction(MessageCategory.Receiver.ToString(), MessageFunction.Message.ToString(), base.UserInfo.Id);
        }

        public DataTable GetSendDT()
        {
            string commandText = this.Query + " WHERE (" + BaseMessageEntity.FieldCategoryCode + " = '" + MessageCategory.Send.ToString() + "')  AND (" + BaseMessageEntity.FieldDeletionStateCode + " = 0)  AND (" + BaseMessageEntity.FieldCreateUserId + " = '" + base.UserInfo.Id + "')  ORDER BY " + BaseMessageEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public DataTable GetWarningDT()
        {
            return this.GetDTByFunction(MessageCategory.Receiver.ToString(), MessageFunction.Warning.ToString(), base.UserInfo.Id);
        }

        public DataTable GetWarningDT(string userId)
        {
            return this.GetDTByFunction(MessageCategory.Receiver.ToString(), MessageFunction.Warning.ToString(), userId);
        }

        public DataTable GetWarningDT(string userId, int topN)
        {
            return this.SearchWarningDT(string.Empty, userId, topN);
        }

        public string[] MessageChek()
        {
            string[] strArray = new string[6];
            int newCount = this.GetNewCount();
            strArray[0] = newCount.ToString();
            if (newCount > 0)
            {
                BaseMessageEntity newOne = this.GetNewOne();
                if (newOne.CreateOn.HasValue)
                {
                    strArray[1] = Convert.ToDateTime(newOne.CreateOn).ToString(BaseSystemInfo.DateTimeFormat);
                }
                strArray[2] = newOne.CreateUserId;
                strArray[3] = newOne.CreateBy;
                strArray[4] = newOne.Id;
                strArray[5] = newOne.Contents;
            }
            return strArray;
        }

        private int OnRead(BaseMessageEntity messageEntity, string id)
        {
            int num = 0;
            if ((messageEntity.ReceiverId == base.UserInfo.Id) && (messageEntity.IsNew == 1))
            {
                SQLBuilder builder = new SQLBuilder(base.DbHelper);
                builder.BeginUpdate(base.CurrentTableName);
                builder.SetValue(BaseMessageEntity.FieldIsNew, 0.ToString(), null);
                builder.SetDBNow(BaseMessageEntity.FieldReadDate);
                builder.SetWhere(BaseMessageEntity.FieldId, id);
                builder.EndUpdate();
            }
            messageEntity.ReadCount += 1;
            this.SetProperty(id, BaseMessageEntity.FieldReadCount, messageEntity.ReadCount.ToString());
            num++;
            return num;
        }

        public DataTable Read(string id)
        {
            BaseMessageEntity messageEntity = new BaseMessageEntity(this.GetDTById(id));
            this.OnRead(messageEntity, id);
            return this.GetDTById(id);
        }

        public DataTable ReadFromReceiver(string receiverId)
        {
            string[] strArray = new string[] { this.Query, " WHERE (", BaseMessageEntity.FieldIsNew, " = ", 1.ToString(), " )  AND (", BaseMessageEntity.FieldReceiverId, " = '", base.UserInfo.Id, "' )  AND (", BaseMessageEntity.FieldCreateUserId, " = '", receiverId, "' )  ORDER BY ", BaseMessageEntity.FieldSortCode };
            string commandText = string.Concat(strArray);
            DataTable table = base.DbHelper.Fill(commandText);
            string id = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                if (row[BaseMessageEntity.FieldReceiverId].ToString() == base.UserInfo.Id)
                {
                    id = row[BaseMessageEntity.FieldId].ToString();
                    this.SetProperty(id, BaseMessageEntity.FieldIsNew, 0);
                }
            }
            return table;
        }

        public DataTable SearchDeletedDT(string searchValue)
        {
            if (searchValue.Length == 0)
            {
                return this.GetDeletedDT();
            }
            DataTable dataTable = new DataTable(BaseMessageEntity.TableName);
            string commandText = this.Query + " WHERE ((" + BaseMessageEntity.FieldContents + " LIKE ? )  OR ( " + BaseMessageEntity.FieldReceiverRealName + " LIKE ? )  OR (" + BaseMessageEntity.FieldCreateOn + " LIKE ? ))  AND (" + BaseMessageEntity.FieldDeletionStateCode + " = 1 )  AND (" + BaseMessageEntity.FieldReceiverId + " = ? )  ORDER BY " + BaseMessageEntity.FieldSortCode;
            string[] targetFileds = new string[4];
            object[] targetValues = new object[4];
            for (int i = 0; i < 3; i++)
            {
                targetFileds[i] = BaseMessageEntity.FieldContents;
                targetValues[i] = searchValue;
            }
            targetFileds[3] = BaseMessageEntity.FieldReceiverId;
            targetValues[3] = base.UserInfo.Id;
            base.DbHelper.Fill(dataTable, commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public DataTable SearchNewList(string searchValue)
        {
            if (searchValue.Length == 0)
            {
                return this.GetDTNew();
            }
            DataTable dataTable = new DataTable(BaseMessageEntity.TableName);
            string[] strArray2 = new string[] { 
                this.Query, " WHERE ((", BaseMessageEntity.FieldContents, " LIKE ", base.DbHelper.GetParameter(BaseMessageEntity.FieldContents), " )  OR ( ", BaseMessageEntity.FieldTitle, " LIKE ", base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId), " )  OR ( ", BaseMessageEntity.FieldReceiverRealName, " LIKE ", base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId), " ))  AND (", BaseMessageEntity.FieldIsNew, " = ", 
                1.ToString(), " )  AND (", BaseMessageEntity.FieldReceiverId, " = ", base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId), " )  ORDER BY ", BaseMessageEntity.FieldSortCode
             };
            string commandText = string.Concat(strArray2);
            string[] targetFileds = new string[4];
            object[] targetValues = new object[4];
            for (int i = 0; i < 3; i++)
            {
                targetFileds[i] = BaseMessageEntity.FieldContents;
                targetValues[i] = searchValue;
            }
            targetFileds[3] = BaseMessageEntity.FieldReceiverId;
            targetValues[3] = base.UserInfo.Id;
            base.DbHelper.Fill(dataTable, commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public DataTable SearchOldDT(string searchValue)
        {
            if (searchValue.Length == 0)
            {
                return this.GetOldDT();
            }
            DataTable dataTable = new DataTable(BaseMessageEntity.TableName);
            string[] strArray2 = new string[] { 
                this.Query, " WHERE ((", BaseMessageEntity.FieldContents, " LIKE ", base.DbHelper.GetParameter(BaseMessageEntity.FieldContents), " )  OR (", BaseMessageEntity.FieldReceiverRealName, " LIKE ", base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId), " )  OR (", BaseMessageEntity.FieldCreateOn, " LIKE ", base.DbHelper.GetParameter(BaseMessageEntity.FieldCreateOn), " ))  AND (", BaseMessageEntity.FieldIsNew, " = ", 
                0.ToString(), " )  AND (", BaseMessageEntity.FieldReceiverId, " = ", base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId), " )  ORDER BY ", BaseMessageEntity.FieldSortCode
             };
            string commandText = string.Concat(strArray2);
            string[] targetFileds = new string[4];
            object[] targetValues = new object[4];
            for (int i = 0; i < 3; i++)
            {
                targetFileds[i] = BaseMessageEntity.FieldContents;
                targetValues[i] = searchValue;
            }
            targetFileds[3] = BaseMessageEntity.FieldReceiverId;
            targetValues[3] = base.UserInfo.Id;
            base.DbHelper.Fill(dataTable, commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public DataTable SearchReceiveDT(string searchValue)
        {
            if (searchValue.Length == 0)
            {
                return this.GetReceiveDT();
            }
            searchValue = BaseBusinessLogic.GetSearchString(searchValue);
            string str2 = this.Query + "    WHERE (" + BaseMessageEntity.FieldDeletionStateCode + " = 0 )           AND (" + BaseMessageEntity.FieldCategoryCode + " = '" + MessageCategory.Receiver.ToString() + "') ";
            string str3 = str2 + "          AND (" + BaseMessageEntity.FieldFunctionCode + " = '" + MessageFunction.Message.ToString() + "' ) ";
            string str4 = str3 + "          AND (" + BaseMessageEntity.FieldReceiverId + " = " + base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId) + ") ";
            string str5 = str4 + " AND ((" + BaseMessageEntity.FieldTitle + " LIKE " + base.DbHelper.GetParameter(BaseMessageEntity.FieldTitle) + " )  OR (" + BaseMessageEntity.FieldContents + " LIKE " + base.DbHelper.GetParameter(BaseMessageEntity.FieldContents) + " )  OR (" + BaseMessageEntity.FieldCreateBy + " LIKE " + base.DbHelper.GetParameter(BaseMessageEntity.FieldCreateBy) + " )) ";
            string commandText = str5 + " ORDER BY " + BaseMessageEntity.FieldIsNew + " DESC           ," + BaseMessageEntity.FieldCreateOn + " DESC           ," + BaseMessageEntity.FieldCreateUserId;
            DataTable dataTable = new DataTable(BaseMessageEntity.TableName);
            string[] targetFileds = new string[4];
            object[] targetValues = new object[4];
            targetFileds[0] = BaseMessageEntity.FieldReceiverId;
            targetValues[0] = base.UserInfo.Id;
            targetFileds[1] = BaseMessageEntity.FieldTitle;
            targetValues[1] = searchValue;
            targetFileds[2] = BaseMessageEntity.FieldContents;
            targetValues[2] = searchValue;
            targetFileds[3] = BaseMessageEntity.FieldCreateBy;
            targetValues[3] = base.UserInfo.Id;
            base.DbHelper.Fill(dataTable, commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public DataTable SearchSendDT(string searchValue)
        {
            if (searchValue.Length == 0)
            {
                return this.GetSendDT();
            }
            searchValue = BaseBusinessLogic.GetSearchString(searchValue);
            DataTable dataTable = new DataTable(BaseMessageEntity.TableName);
            string commandText = this.Query + " WHERE ((" + BaseMessageEntity.FieldContents + " LIKE " + base.DbHelper.GetParameter(BaseMessageEntity.FieldContents) + " )  OR (" + BaseMessageEntity.FieldReceiverRealName + " LIKE " + base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverRealName) + " )  OR (" + BaseMessageEntity.FieldCreateOn + " LIKE " + base.DbHelper.GetParameter(BaseMessageEntity.FieldCreateOn) + " ))  AND (" + BaseMessageEntity.FieldDeletionStateCode + " = 0)  AND (" + BaseMessageEntity.FieldCategoryCode + " = '" + MessageCategory.Send.ToString() + "')  AND (" + BaseMessageEntity.FieldCreateUserId + " = " + base.DbHelper.GetParameter(BaseMessageEntity.FieldCreateUserId) + " )  ORDER BY " + BaseMessageEntity.FieldSortCode;
            string[] targetFileds = new string[4];
            object[] targetValues = new object[4];
            targetFileds[0] = BaseMessageEntity.FieldContents;
            targetValues[0] = searchValue;
            targetFileds[1] = BaseMessageEntity.FieldReceiverRealName;
            targetValues[1] = searchValue;
            targetFileds[2] = BaseMessageEntity.FieldCreateOn;
            targetValues[2] = searchValue;
            targetFileds[3] = BaseMessageEntity.FieldCreateUserId;
            targetValues[3] = base.UserInfo.Id;
            base.DbHelper.Fill(dataTable, commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public DataTable SearchWarningDT(string searchValue)
        {
            return this.SearchWarningDT(searchValue, base.UserInfo.Id);
        }

        public DataTable SearchWarningDT(string search, string userId)
        {
            return this.SearchWarningDT(search, userId, 0);
        }

        public DataTable SearchWarningDT(string search, string userId, int topN)
        {
            if ((search.Length == 0) && (topN == 0))
            {
                return this.GetWarningDT();
            }
            search = BaseBusinessLogic.GetSearchString(search);
            string commandText = " SELECT ";
            if (topN != 0)
            {
                commandText = commandText + " TOP " + topN.ToString();
            }
            string str2 = commandText;
            string str3 = str2 + " * FROM " + BaseMessageEntity.TableName + "    WHERE (" + BaseMessageEntity.FieldDeletionStateCode + " = 0 )           AND (" + BaseMessageEntity.FieldCategoryCode + " = '" + MessageCategory.Receiver.ToString() + "') ";
            string str4 = str3 + "          AND (" + BaseMessageEntity.FieldFunctionCode + " = '" + MessageFunction.Warning.ToString() + "' ) ";
            commandText = str4 + "          AND (" + BaseMessageEntity.FieldReceiverId + " = " + base.DbHelper.GetParameter(BaseMessageEntity.FieldReceiverId) + ") ";
            List<IDbDataParameter> list = new List<IDbDataParameter> {
                base.DbHelper.MakeParameter(BaseMessageEntity.FieldReceiverId, userId)
            };
            if (!string.IsNullOrEmpty(search))
            {
                string str5 = commandText;
                commandText = str5 + " AND ((" + BaseMessageEntity.FieldTitle + " LIKE " + base.DbHelper.GetParameter(BaseMessageEntity.FieldTitle) + " )  OR (" + BaseMessageEntity.FieldContents + " LIKE " + base.DbHelper.GetParameter(BaseMessageEntity.FieldContents) + " )  OR (" + BaseMessageEntity.FieldCreateBy + " LIKE " + base.DbHelper.GetParameter(BaseMessageEntity.FieldCreateBy) + " )) ";
                list.Add(base.DbHelper.MakeParameter(BaseMessageEntity.FieldTitle, search));
                list.Add(base.DbHelper.MakeParameter(BaseMessageEntity.FieldContents, search));
                list.Add(base.DbHelper.MakeParameter(BaseMessageEntity.FieldCreateBy, search));
            }
            string str6 = commandText;
            commandText = str6 + " ORDER BY " + BaseMessageEntity.FieldIsNew + " DESC           ," + BaseMessageEntity.FieldCreateOn + " DESC ";
            DataTable dataTable = new DataTable(BaseMessageEntity.TableName);
            base.DbHelper.Fill(dataTable, commandText, list.ToArray());
            return dataTable;
        }

        public int Send(BaseMessageEntity messageEntity, bool saveSend)
        {
            string[] receiverIds = new string[] { messageEntity.ReceiverId.ToString() };
            return this.Send(messageEntity, receiverIds, saveSend);
        }

        public int Send(BaseMessageEntity messageEntity, string[] receiverIds, bool saveSend)
        {
            BaseUserManager manager = new BaseUserManager(base.DbHelper, base.UserInfo);
            int num = 0;
            messageEntity.CategoryCode = MessageCategory.Receiver.ToString();
            messageEntity.IsNew = 1;
            messageEntity.IPAddress = base.UserInfo.IPAddress;
            messageEntity.ParentId = null;
            messageEntity.DeletionStateCode = 0;
            messageEntity.Enabled = 1;
            num++;
            for (int i = 0; i < receiverIds.Length; i++)
            {
                messageEntity.ParentId = null;
                messageEntity.Id = Guid.NewGuid().ToString();
                messageEntity.CategoryCode = MessageCategory.Receiver.ToString();
                messageEntity.ReceiverId = receiverIds[i];
                messageEntity.ReceiverRealName = manager.GetProperty(messageEntity.ReceiverId, BaseUserEntity.FieldRealName);
                messageEntity.Enabled = 1;
                if (messageEntity.ReceiverId.Equals(base.UserInfo.Id))
                {
                    messageEntity.IsNew = 0;
                }
                string str = this.Add(messageEntity, false, false);
                if (saveSend)
                {
                    messageEntity.Id = Guid.NewGuid().ToString();
                    messageEntity.ParentId = str;
                    messageEntity.CategoryCode = MessageCategory.Send.ToString();
                    messageEntity.DeletionStateCode = 0;
                    messageEntity.Enabled = 0;
                    this.Add(messageEntity, false, false);
                }
                num++;
            }
            return num;
        }

        public int Send(BaseMessageEntity messageEntity, string organizeId, bool saveSend)
        {
            int num2 = 0;
            DataTable childrenUsers = new BaseUserManager(base.DbHelper, base.UserInfo).GetChildrenUsers(organizeId);
            string[] receiverIds = new string[childrenUsers.Rows.Count];
            foreach (DataRow row in childrenUsers.Rows)
            {
                receiverIds[num2++] = row[BaseMessageEntity.FieldId].ToString();
            }
            return this.Send(messageEntity, receiverIds, saveSend);
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseMessageEntity baseMessageEntity)
        {
            sqlBuilder.SetValue(BaseMessageEntity.FieldParentId, baseMessageEntity.ParentId, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldFunctionCode, baseMessageEntity.FunctionCode, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldCategoryCode, baseMessageEntity.CategoryCode, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldObjectId, baseMessageEntity.ObjectId, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldTitle, baseMessageEntity.Title, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldContents, baseMessageEntity.Contents, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldReceiverId, baseMessageEntity.ReceiverId, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldReceiverRealName, baseMessageEntity.ReceiverRealName, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldIsNew, baseMessageEntity.IsNew, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldReadCount, baseMessageEntity.ReadCount, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldReadDate, baseMessageEntity.ReadDate, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldTargetURL, baseMessageEntity.TargetURL, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldIPAddress, baseMessageEntity.IPAddress, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldDeletionStateCode, baseMessageEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldEnabled, baseMessageEntity.Enabled, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldDescription, baseMessageEntity.Description, null);
            sqlBuilder.SetValue(BaseMessageEntity.FieldSortCode, baseMessageEntity.SortCode, null);
        }

        public int Update(BaseMessageEntity baseMessageEntity)
        {
            return this.UpdateEntity(baseMessageEntity);
        }

        public int UpdateEntity(BaseMessageEntity baseMessageEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BaseMessageEntity.TableName);
            this.SetEntity(sqlBuilder, baseMessageEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseMessageEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseMessageEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseMessageEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseMessageEntity.FieldId, baseMessageEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

