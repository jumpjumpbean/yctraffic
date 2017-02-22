namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public abstract class BaseManager : IBaseManager
    {
        public string CurrentTableName;
        protected IDbHelper dbHelper;
        public bool Identity;
        private static object locker = new object();
        public string NextId;
        public string PreviousId;
        public string PrimaryKey;
        public bool ReturnId;
        private string returnStatusCode;
        private string returnStatusMessage;
        protected BaseUserInfo UserInfo;

        public BaseManager()
        {
            this.PreviousId = string.Empty;
            this.NextId = string.Empty;
            this.PrimaryKey = "Id";
            this.Identity = true;
            this.ReturnId = true;
            this.CurrentTableName = string.Empty;
            this.returnStatusCode = StatusCode.Error.ToString();
            this.returnStatusMessage = string.Empty;
        }

        public BaseManager(IDbHelper dbHelper) : this()
        {
            this.DbHelper = dbHelper;
        }

        public BaseManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            this.UserInfo = userInfo;
        }

        public virtual bool AddAfter()
        {
            return true;
        }

        public virtual bool AddBefore()
        {
            return true;
        }

        public virtual string AddEntity(object entity)
        {
            return string.Empty;
        }

        public virtual int BatchSave(DataTable dataTable)
        {
            return 0;
        }

        public int BatchSetCode(string[] ids, string[] codes)
        {
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                num += this.SetProperty(ids[i], BaseBusinessLogic.FieldCode, codes[i]);
            }
            return num;
        }

        public int BatchSetSortCode(string[] ids)
        {
            int num = 0;
            string[] batchSequence = new BaseSequenceManager(this.dbHelper).GetBatchSequence(this.CurrentTableName, ids.Length);
            for (int i = 0; i < ids.Length; i++)
            {
                num += this.SetProperty(ids[i], BaseBusinessLogic.FieldSortCode, batchSequence[i]);
            }
            return num;
        }

        public virtual int ChangeEnabled(object id)
        {
            string commandText = " UPDATE " + this.CurrentTableName + " SET " + BaseBusinessLogic.FieldEnabled + " = (CASE " + BaseBusinessLogic.FieldEnabled + " WHEN 0 THEN 1 WHEN 1 THEN 0 END)  WHERE (" + BaseBusinessLogic.FieldId + " = " + this.DbHelper.GetParameter(BaseBusinessLogic.FieldId) + ")";
            string[] targetFileds = new string[1];
            object[] targetValues = new object[1];
            targetFileds[0] = BaseBusinessLogic.FieldId;
            targetValues[0] = id;
            return this.DbHelper.ExecuteNonQuery(commandText, this.DbHelper.MakeParameters(targetFileds, targetValues));
        }

        public virtual int Delete()
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName);
        }

        public virtual int Delete(object id)
        {
            return this.DeleteEntity(id);
        }

        public virtual int Delete(object[] ids)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldId, ids);
        }

        public virtual int Delete(object[] ids, bool force)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldId, ids);
        }

        public virtual int Delete(string[] names, object[] values)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, names, values);
        }

        public virtual int Delete(object id, bool force)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldId, id);
        }

        public virtual int Delete(string name, object[] values)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, name, values);
        }

        public virtual int Delete(string name, object value)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, name, value);
        }

        public virtual int Delete(string[] names, object[] values, bool force)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, names, values);
        }

        public virtual int Delete(string name, object value, bool force)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, name, value);
        }

        public virtual int Delete(string name1, object value1, string name2, object value2)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2);
        }

        public virtual int Delete(string name1, object value1, string name2, object value2, bool force)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2);
        }

        public virtual bool DeleteAfter(string id)
        {
            return true;
        }

        public virtual bool DeleteBefore(string id)
        {
            return true;
        }

        public virtual int DeleteEntity(object id)
        {
            return this.DeleteEntity(BaseBusinessLogic.FieldId, id);
        }

        public virtual int DeleteEntity(string name, object value)
        {
            return DbLogic.Delete(this.DbHelper, this.CurrentTableName, name, value);
        }

        public virtual int ExecuteNonQuery(string commandText)
        {
            return this.DbHelper.ExecuteNonQuery(commandText);
        }

        public virtual int ExecuteNonQuery(string commandText, IDbDataParameter[] dbParameters)
        {
            return this.DbHelper.ExecuteNonQuery(commandText, dbParameters);
        }

        public virtual object ExecuteScalar(string commandText)
        {
            return this.DbHelper.ExecuteScalar(commandText);
        }

        public virtual object ExecuteScalar(string commandText, IDbDataParameter[] dbParameters)
        {
            return this.DbHelper.ExecuteScalar(commandText, dbParameters);
        }

        public virtual bool Exists(object id)
        {
            return DbLogic.Exists(this.DbHelper, this.CurrentTableName, this.PrimaryKey, id);
        }

        public virtual bool Exists(object[] ids)
        {
            return DbLogic.Exists(this.DbHelper, this.CurrentTableName, this.PrimaryKey, ids);
        }

        public virtual bool Exists(string name, object value)
        {
            return DbLogic.Exists(this.DbHelper, this.CurrentTableName, name, value);
        }

        public virtual bool Exists(string[] names, object[] values)
        {
            return DbLogic.Exists(this.DbHelper, this.CurrentTableName, names, values);
        }

        public virtual bool Exists(string name, object value, object id)
        {
            string[] names = new string[] { name };
            object[] values = new object[] { value };
            return DbLogic.Exists(this.DbHelper, this.CurrentTableName, names, values, this.PrimaryKey, id);
        }

        public virtual bool Exists(string[] names, object[] values, object id)
        {
            return DbLogic.Exists(this.DbHelper, this.CurrentTableName, names, values, this.PrimaryKey, id);
        }

        public virtual bool Exists(string name1, object value1, string name2, object value2)
        {
            return DbLogic.Exists(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2);
        }

        public virtual bool Exists(string name1, object value1, string name2, object value2, object id)
        {
            return DbLogic.Exists(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2, this.PrimaryKey, id);
        }

        public virtual DataTable Fill(string commandText)
        {
            return this.DbHelper.Fill(commandText);
        }

        public virtual DataTable Fill(string commandText, IDbDataParameter[] dbParameters)
        {
            return this.DbHelper.Fill(commandText, dbParameters);
        }

        public virtual bool GetAfter(string id)
        {
            return true;
        }

        public virtual bool GetBefore(string id)
        {
            return true;
        }

        public DataTable GetChildrens(string fieldId, string id, string fieldParentId, string order)
        {
            return DbLogic.GetChildrens(this.DbHelper, this.CurrentTableName, fieldId, id, fieldParentId, order);
        }

        public DataTable GetChildrensByCode(string fieldCode, string code, string order)
        {
            return DbLogic.GetChildrensByCode(this.DbHelper, this.CurrentTableName, fieldCode, code, order);
        }

        public string[] GetChildrensId(string fieldId, string id, string fieldParentId)
        {
            return DbLogic.GetChildrensId(this.DbHelper, this.CurrentTableName, fieldId, id, fieldParentId, string.Empty);
        }

        public string[] GetChildrensIdByCode(string fieldCode, string code)
        {
            return DbLogic.GetChildrensIdByCode(this.DbHelper, this.CurrentTableName, fieldCode, code, string.Empty);
        }

        public virtual string GetCodeById(string id)
        {
            return DbLogic.GetProperty(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldCode);
        }

        public virtual DataTable GetDT()
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName);
        }

        public virtual DataTable GetDT(string[] ids)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, ids);
        }

        public virtual DataTable GetDT(string order)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, order);
        }

        public virtual DataTable GetDT(string[] names, object[] values)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, names, values);
        }

        public virtual DataTable GetDT(int topLimit, string order)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, topLimit, order);
        }

        public virtual DataTable GetDT(string name, object value)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, name, value);
        }

        public virtual DataTable GetDT(string name, object[] values)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, name, values);
        }

        public virtual DataTable GetDT(string name, object value, string order)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, name, value, order);
        }

        public virtual DataTable GetDT(string name, object[] values, string order)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, name, values, order);
        }

        public virtual DataTable GetDT(string[] names, object[] values, string order)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, names, values, order);
        }

        public virtual DataTable GetDT(string[] names, object[] values, int topLimit, string order)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, names, values, topLimit, order);
        }

        public virtual DataTable GetDT(string name, object value, int topLimit, string order)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, name, value, topLimit, order);
        }

        public virtual DataTable GetDT(string name1, object value1, string name2, object value2)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2);
        }

        public virtual DataTable GetDT(string name1, object value1, string name2, object value2, string order)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2, order);
        }

        public virtual DataTable GetDT(out int recordCount, int currentPage, int perPage, string whereConditional, string order)
        {
            recordCount = DbLogic.GetCount(this.DbHelper, this.CurrentTableName, whereConditional);
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, currentPage, perPage, whereConditional, order);
        }

        public virtual DataTable GetDT(string name1, object value1, string name2, object value2, int topLimit, string order)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2, topLimit, order);
        }

        public virtual DataTable GetDTByCategory(string categoryId)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldCategoryId, categoryId, BaseBusinessLogic.FieldSortCode);
        }

        public virtual DataTable GetDTById(string id)
        {
            return this.GetDT(BaseBusinessLogic.FieldId, id);
        }

        public virtual DataTable GetDTByPage(out int recordCount, int currentPage, int pageSize, string sqlQuery, string orderby)
        {
            string commandText = string.Format("SELECT COUNT(1) FROM ({0}) T ", sqlQuery);
            object obj2 = this.dbHelper.ExecuteScalar(commandText);
            if (obj2 != null)
            {
                recordCount = int.Parse(obj2.ToString());
            }
            else
            {
                recordCount = 0;
            }
            if (recordCount <= ((currentPage - 1) * pageSize) )
            {
                currentPage = 1;
            }
            return DbLogic.GetDTByPage(this.DbHelper, this.CurrentTableName, currentPage, pageSize, sqlQuery, orderby, recordCount);
        }

        public virtual DataTable GetDTByPage(out int recordCount, int currentPage, int perPage, string whereConditional, List<IDbDataParameter> dbParameters, string order)
        {
            recordCount = DbLogic.GetCount(this.DbHelper, this.CurrentTableName, whereConditional, dbParameters);
            return DbLogic.GetDTByPage(this.DbHelper, this.CurrentTableName, currentPage, perPage, whereConditional, dbParameters, order);
        }

        public virtual DataTable GetDTByParent(string parentId)
        {
            return DbLogic.GetDT(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldParentId, parentId, BaseBusinessLogic.FieldSortCode);
        }

        public virtual object GetFrom(DataRow dataRow)
        {
            return this;
        }

        public virtual object GetFrom(DataTable dataTable)
        {
            return this.GetFrom(dataTable, string.Empty);
        }

        public virtual object GetFrom(DataTable dataTable, string id)
        {
            return this.GetFrom(dataTable, BaseBusinessLogic.FieldId, id);
        }

        public virtual object GetFrom(DataTable dataTable, string name, object value)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (((value == null) || (value.ToString().Length == 0)) || row[name].ToString().Equals(value))
                {
                    this.GetFrom(row);
                    break;
                }
            }
            return this;
        }

        public virtual string GetFullNameByCategory(string categoryId, string code)
        {
            return this.GetProperty(BaseBusinessLogic.FieldCategoryId, categoryId, BaseBusinessLogic.FieldCode, code, BaseBusinessLogic.FieldFullName);
        }

        public virtual string GetFullNameByCode(string code)
        {
            return this.GetProperty(BaseBusinessLogic.FieldCode, code, BaseBusinessLogic.FieldFullName);
        }

        public virtual string GetFullNameById(string id)
        {
            return this.GetProperty(BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldFullName);
        }

        public virtual string GetId(string name, object value)
        {
            return DbLogic.GetProperty(this.DbHelper, this.CurrentTableName, name, value, BaseBusinessLogic.FieldId);
        }

        public virtual string GetId(string[] names, object[] values)
        {
            return DbLogic.GetProperty(this.DbHelper, this.CurrentTableName, names, values, BaseBusinessLogic.FieldId);
        }

        public virtual string GetId(string name1, object value1, string name2, object value2)
        {
            return DbLogic.GetProperty(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2, BaseBusinessLogic.FieldId);
        }

        public virtual string GetIdByCode(string code)
        {
            return DbLogic.GetProperty(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldCode, code, BaseBusinessLogic.FieldId);
        }

        public virtual string[] GetIds()
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName);
        }

        public virtual string[] GetIds(string targetField)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, targetField);
        }

        public virtual string[] GetIds(int topLimit, string targetField)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, topLimit, targetField);
        }

        public virtual string[] GetIds(string[] names, object[] values)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, names, values);
        }

        public virtual string[] GetIds(string name, object[] values)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, name, values);
        }

        public virtual string[] GetIds(string name, object value)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, name, value);
        }

        public virtual string[] GetIds(string name, object value, string targetField)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, name, value, targetField);
        }

        public virtual string[] GetIds(string[] names, object[] values, string targetField)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, names, values, targetField);
        }

        public virtual string[] GetIds(string name, object[] values, string targetField)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, name, values, targetField);
        }

        public virtual string[] GetIds(string[] names, object[] values, int topLimit, string targetField)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, names, values, new int?(topLimit), targetField);
        }

        public virtual string[] GetIds(string name, object value, int topLimit, string targetField)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, name, value, topLimit, targetField);
        }

        public virtual string[] GetIds(string name1, object value1, string name2, object value2)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2);
        }

        public virtual string[] GetIds(string name1, object value1, string name2, object value2, string targetField)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2, targetField);
        }

        public virtual string[] GetIds(string name1, object value1, string name2, object value2, int topLimit, string targetField)
        {
            return DbLogic.GetIds(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2, topLimit, targetField);
        }

        private string GetNextId(string id)
        {
            string str = string.Empty;
            string str3 = " SELECT TOP 1 " + BaseBusinessLogic.FieldId + " FROM " + this.CurrentTableName + " WHERE " + BaseBusinessLogic.FieldCreateOn + " = (SELECT MIN(" + BaseBusinessLogic.FieldCreateOn + ") FROM " + this.CurrentTableName + " WHERE (" + BaseBusinessLogic.FieldCreateOn + "> (SELECT " + BaseBusinessLogic.FieldCreateOn + " FROM " + this.CurrentTableName + " WHERE " + BaseBusinessLogic.FieldId + " = " + this.DbHelper.GetParameter(BaseBusinessLogic.FieldId) + "))";
            string commandText = str3 + " AND (" + BaseBusinessLogic.FieldCreateUserId + " = " + this.DbHelper.GetParameter(BaseBusinessLogic.FieldCreateUserId) + ") AND ( " + BaseBusinessLogic.FieldDeletionStateCode + " = 0 )) ";
            string[] targetFileds = new string[2];
            object[] targetValues = new object[2];
            targetFileds[0] = BaseBusinessLogic.FieldId;
            targetValues[0] = id;
            targetFileds[1] = BaseBusinessLogic.FieldCreateUserId;
            targetValues[1] = this.UserInfo.Id;
            object obj2 = this.DbHelper.ExecuteScalar(commandText, this.DbHelper.MakeParameters(targetFileds, targetValues));
            if (obj2 != null)
            {
                str = obj2.ToString();
            }
            return str;
        }

        public string GetNextId(DataTable dataTable, string id)
        {
            string str = string.Empty;
            bool flag = false;
            foreach (DataRowView view in dataTable.DefaultView)
            {
                if (flag)
                {
                    return view[BaseBusinessLogic.FieldId].ToString();
                }
                if (view[BaseBusinessLogic.FieldId].ToString().Equals(id))
                {
                    flag = true;
                }
            }
            return str;
        }

        public DataTable GetParentChildrensByCode(string fieldCode, string code, string order)
        {
            return DbLogic.GetParentChildrensByCode(this.DbHelper, this.CurrentTableName, fieldCode, code, order);
        }

        public string[] GetParentChildrensIdByCode(string fieldCode, string code)
        {
            return DbLogic.GetParentChildrensIdByCode(this.DbHelper, this.CurrentTableName, fieldCode, code, string.Empty);
        }

        public virtual string GetParentId(string id)
        {
            return this.GetProperty(BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldParentId);
        }

        public virtual string GetParentIdByCategory(string categoryId, string code)
        {
            return this.GetProperty(BaseBusinessLogic.FieldCategoryId, categoryId, BaseBusinessLogic.FieldCode, code, BaseBusinessLogic.FieldParentId);
        }

        public virtual string GetParentIdByCode(string code)
        {
            return this.GetProperty(BaseBusinessLogic.FieldCode, code, BaseBusinessLogic.FieldParentId);
        }

        public string GetParentIdByCode(string fieldCode, string code)
        {
            return DbLogic.GetParentIdByCode(this.DbHelper, this.CurrentTableName, fieldCode, code);
        }

        public DataTable GetParentsByCode(string fieldCode, string code, string order)
        {
            return DbLogic.GetParentsByCode(this.DbHelper, this.CurrentTableName, fieldCode, code, order);
        }

        public string[] GetParentsIdByCode(string fieldCode, string code)
        {
            return DbLogic.GetParentsIDByCode(this.DbHelper, this.CurrentTableName, fieldCode, code, string.Empty);
        }

        private string GetPreviousId(string id)
        {
            string str = string.Empty;
            string str3 = " SELECT TOP 1 " + BaseBusinessLogic.FieldId + " FROM " + this.CurrentTableName + " WHERE " + BaseBusinessLogic.FieldCreateOn + " = (SELECT MAX(" + BaseBusinessLogic.FieldCreateOn + ") FROM " + this.CurrentTableName + " WHERE (" + BaseBusinessLogic.FieldCreateOn + "< (SELECT " + BaseBusinessLogic.FieldCreateOn + " FROM " + this.CurrentTableName + " WHERE " + BaseBusinessLogic.FieldId + " = " + this.DbHelper.GetParameter(BaseBusinessLogic.FieldId) + "))";
            string commandText = str3 + " AND (" + BaseBusinessLogic.FieldCreateUserId + " = " + this.DbHelper.GetParameter(BaseBusinessLogic.FieldCreateUserId) + " ) AND ( " + BaseBusinessLogic.FieldDeletionStateCode + " = 0 )) ";
            string[] targetFileds = new string[2];
            object[] targetValues = new object[2];
            targetFileds[0] = BaseBusinessLogic.FieldId;
            targetValues[0] = id;
            targetFileds[1] = BaseBusinessLogic.FieldCreateUserId;
            targetValues[1] = this.UserInfo.Id;
            object obj2 = this.DbHelper.ExecuteScalar(commandText, this.DbHelper.MakeParameters(targetFileds, targetValues));
            if (obj2 != null)
            {
                str = obj2.ToString();
            }
            return str;
        }

        public string GetPreviousId(DataTable dataTable, string id)
        {
            string str = string.Empty;
            foreach (DataRowView view in dataTable.DefaultView)
            {
                if (view[BaseBusinessLogic.FieldId].ToString().Equals(id))
                {
                    return str;
                }
                str = view[BaseBusinessLogic.FieldId].ToString();
            }
            return str;
        }

        public void GetPreviousNextId(DataTable dataTable, string id)
        {
            this.PreviousId = this.GetPreviousId(dataTable, id);
            this.NextId = this.GetNextId(dataTable, id);
        }

        public DataTable GetPreviousNextId(bool deletionStateCode, string id, string order)
        {
            string commandText = string.Concat(new object[] { " SELECT Id  FROM ", this.CurrentTableName, " WHERE (", BaseBusinessLogic.FieldCreateUserId, " = ", this.DbHelper.GetParameter(BaseBusinessLogic.FieldCreateUserId), " AND ", BaseBusinessLogic.FieldDeletionStateCode, " = ", deletionStateCode ? 1 : 0, ") ORDER BY ", order });
            string[] targetFileds = new string[1];
            object[] targetValues = new object[1];
            targetFileds[0] = BaseBusinessLogic.FieldCreateUserId;
            targetValues[0] = this.UserInfo.Id;
            DataTable dataTable = new DataTable(this.CurrentTableName);
            this.DbHelper.Fill(dataTable, commandText, this.DbHelper.MakeParameters(targetFileds, targetValues));
            this.NextId = this.GetNextId(dataTable, id);
            this.PreviousId = this.GetPreviousId(dataTable, id);
            return dataTable;
        }

        public virtual string GetProperty(object id, string targetField)
        {
            return DbLogic.GetProperty(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldId, id, targetField);
        }

        public virtual string GetProperty(string[] names, object[] values, string targetField)
        {
            return DbLogic.GetProperty(this.DbHelper, this.CurrentTableName, names, values, targetField);
        }

        public virtual string GetProperty(string name, object value, string targetField)
        {
            return DbLogic.GetProperty(this.DbHelper, this.CurrentTableName, name, value, targetField);
        }

        public virtual string GetProperty(string name1, object value1, string name2, object value2, string targetField)
        {
            return DbLogic.GetProperty(this.DbHelper, this.CurrentTableName, name1, value1, name2, value2, targetField);
        }

        public string GetStateMessage()
        {
            return this.GetStateMessage(this.ReturnStatusCode);
        }

        public string GetStateMessage(StatusCode statusCode)
        {
            string str = string.Empty;
            switch (statusCode)
            {
                case StatusCode.DbError:
                    return AppMessage.MSG0002;

                case ((StatusCode) 1):
                case ((StatusCode) 2):
                case ((StatusCode) 3):
                case ((StatusCode) 4):
                case ((StatusCode) 5):
                case ((StatusCode) 6):
                case ((StatusCode) 7):
                case ((StatusCode) 8):
                case ((StatusCode) 0x15):
                    return str;

                case StatusCode.Error:
                    return AppMessage.MSG0001;

                case StatusCode.OK:
                    return AppMessage.MSG9965;

                case StatusCode.OKAdd:
                    return AppMessage.MSG0009;

                case StatusCode.CanNotLock:
                    return AppMessage.MSG0043;

                case StatusCode.LockOK:
                    return AppMessage.MSG0044;

                case StatusCode.OKUpdate:
                    return AppMessage.MSG0010;

                case StatusCode.OKDelete:
                    return AppMessage.MSG0013;

                case StatusCode.Exist:
                    return AppMessage.Format(AppMessage.MSG0008, new string[] { AppMessage.MSG9955 });

                case StatusCode.ErrorCodeExist:
                    return AppMessage.Format(AppMessage.MSG0008, new string[] { AppMessage.MSG9977 });

                case StatusCode.ErrorNameExist:
                    return AppMessage.Format(AppMessage.MSG0008, new string[] { AppMessage.MSG9978 });

                case StatusCode.ErrorValueExist:
                    return AppMessage.Format(AppMessage.MSG0008, new string[] { AppMessage.MSG9800 });

                case StatusCode.ErrorUserExist:
                    return AppMessage.Format(AppMessage.MSG0008, new string[] { AppMessage.MSG9957 });

                case StatusCode.ErrorDataRelated:
                    return AppMessage.MSG0033;

                case StatusCode.ErrorDeleted:
                    return AppMessage.MSG0005;

                case StatusCode.ErrorChanged:
                    return AppMessage.MSG0006;

                case StatusCode.NotFound:
                    return AppMessage.MSG9956;

                case StatusCode.UserNotFound:
                    return AppMessage.MSG9966;

                case StatusCode.PasswordError:
                    return AppMessage.MSG9967;

                case StatusCode.LogOnDeny:
                    return AppMessage.MSG9968;

                case StatusCode.ErrorOnLine:
                    return AppMessage.MSG0048;

                case StatusCode.ErrorMacAddress:
                    return AppMessage.MSG0049;

                case StatusCode.ErrorIPAddress:
                    return AppMessage.MSG0050;

                case StatusCode.ErrorOnLineLimit:
                    return AppMessage.MSG0051;

                case StatusCode.PasswordCanNotBeNull:
                    return AppMessage.Format(AppMessage.MSG0007, new string[] { AppMessage.MSG9961 });

                case StatusCode.SetPasswordOK:
                    return AppMessage.Format(AppMessage.MSG9963, new string[] { AppMessage.MSG9964 });

                case StatusCode.OldPasswordError:
                    return AppMessage.Format(AppMessage.MSG0040, new string[] { AppMessage.MSG9961 });

                case StatusCode.ChangePasswordOK:
                    return AppMessage.Format(AppMessage.MSG9962, new string[] { AppMessage.MSG9964 });

                case StatusCode.UserNotEmail:
                    return AppMessage.MSG9910;

                case StatusCode.UserLocked:
                    return AppMessage.MSG9911;

                case StatusCode.UserNotActive:
                case StatusCode.WaitForAudit:
                    return AppMessage.MSG9912;

                case StatusCode.UserIsActivate:
                    return AppMessage.MSG9913;

                case StatusCode.ErrorLogOn:
                    return AppMessage.MSG9000;

                case StatusCode.UserDuplicate:
                    return AppMessage.Format(AppMessage.MSG0008, new string[] { AppMessage.MSG9957 });
            }
            return str;
        }

        public string GetStateMessage(string statusCode)
        {
            if (string.IsNullOrEmpty(statusCode))
            {
                return string.Empty;
            }
            StatusCode code = (StatusCode) Enum.Parse(typeof(StatusCode), statusCode, true);
            return this.GetStateMessage(code);
        }

        public virtual int ResetSortCode()
        {
            int num = 0;
            DataTable dT = this.GetDT(BaseBusinessLogic.FieldSortCode);
            string[] batchSequence = new BaseSequenceManager(this.dbHelper).GetBatchSequence(this.CurrentTableName, dT.Rows.Count);
            int index = 0;
            foreach (DataRow row in dT.Rows)
            {
                num += this.SetProperty(row[BaseBusinessLogic.FieldId].ToString(), BaseBusinessLogic.FieldSortCode, batchSequence[index]);
                index++;
            }
            return num;
        }

        public void SetConnection(BaseUserInfo userInfo)
        {
            this.UserInfo = userInfo;
        }

        public void SetConnection(IDbHelper dbHelper)
        {
            this.DbHelper = dbHelper;
        }

        public void SetConnection(IDbHelper dbHelper, BaseUserInfo userInfo)
        {
            this.SetConnection(dbHelper);
            this.UserInfo = userInfo;
        }

        public virtual int SetDeleted()
        {
            return this.SetProperty(BaseBusinessLogic.FieldCreateUserId, this.UserInfo.Id, BaseBusinessLogic.FieldDeletionStateCode, "1");
        }

        public virtual int SetDeleted(object[] ids)
        {
            return this.SetProperty(BaseBusinessLogic.FieldId, ids, BaseBusinessLogic.FieldDeletionStateCode, "1");
        }

        public virtual int SetDeleted(object id)
        {
            return this.SetProperty(BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldDeletionStateCode, "1");
        }

        public virtual int SetDeleted(string[] names, object[] values)
        {
            return this.SetProperty(names, values, BaseBusinessLogic.FieldDeletionStateCode, "1");
        }

        public virtual void SetParameter(BaseUserInfo userInfo)
        {
            this.UserInfo = userInfo;
        }

        public virtual void SetParameter(IDbHelper dbHelper)
        {
            this.DbHelper = dbHelper;
        }

        public virtual void SetParameter(IDbHelper dbHelper, BaseUserInfo userInfo)
        {
            this.DbHelper = dbHelper;
            this.UserInfo = userInfo;
        }

        public virtual int SetProperty(string targetField, object targetValue)
        {
            return DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, targetField, targetValue);
        }

        public virtual int SetProperty(object[] ids, string[] targetFields, object[] targetValues)
        {
            return DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldId, ids, targetFields, targetValues);
        }

        public virtual int SetProperty(object[] ids, string targetField, object targetValue)
        {
            return DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldId, ids, targetField, targetValue);
        }

        public virtual int SetProperty(object id, string targetField, object targetValue)
        {
            return DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldId, id, targetField, targetValue);
        }

        public virtual int SetProperty(object id, string[] targetFields, object[] targetValues)
        {
            return DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, BaseBusinessLogic.FieldId, id, targetFields, targetValues);
        }

        public virtual int SetProperty(string name, string[] values, string[] targetFields, object[] targetValues)
        {
            return DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, name, (object[]) values, targetFields, targetValues);
        }

        public virtual int SetProperty(string[] names, object[] values, string targetField, object targetValue)
        {
            return DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, names, values, targetField, targetValue);
        }

        public virtual int SetProperty(string name, object value, string[] targetFields, object[] targetValues)
        {
            return DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, name, value, targetFields, targetValues);
        }

        public virtual int SetProperty(string name, object[] values, string targetField, object targetValue)
        {
            int num = 0;
            if (values == null)
            {
                return (num + DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, name, string.Empty, targetField, targetValue));
            }
            for (int i = 0; i < values.Length; i++)
            {
                num += DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, name, values[i], targetField, targetValue);
            }
            return num;
        }

        public virtual int SetProperty(string whereName, object whereValue, string targetField, object targetValue)
        {
            return DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, whereName, whereValue, targetField, targetValue);
        }

        public virtual int SetProperty(string whereName1, object whereValue1, string whereName2, object whereValue2, string targetField, object targetValue)
        {
            return DbLogic.SetProperty(this.DbHelper, this.CurrentTableName, whereName1, whereValue1, whereName2, whereValue2, targetField, targetValue);
        }

        public virtual int SetTableColumns(string tableCode, string tableName, string columnCode, string columnName)
        {
            string sequence = new BaseSequenceManager(this.DbHelper).GetSequence(BaseTableColumnsEntity.TableName);
            SQLBuilder builder = new SQLBuilder(this.DbHelper);
            builder.BeginInsert(BaseTableColumnsEntity.TableName);
            builder.SetValue(BaseTableColumnsEntity.FieldTableCode, tableCode, null);
            builder.SetValue(BaseTableColumnsEntity.FieldColumnCode, columnCode, null);
            builder.SetValue(BaseTableColumnsEntity.FieldColumnName, columnName, null);
            builder.SetValue(BaseTableColumnsEntity.FieldSortCode, sequence, null);
            builder.SetValue(BaseTableColumnsEntity.FieldCreateUserId, this.UserInfo.Id, null);
            builder.SetValue(BaseTableColumnsEntity.FieldCreateBy, this.UserInfo.RealName, null);
            builder.SetDBNow(BaseTableColumnsEntity.FieldCreateOn);
            builder.SetValue(BaseTableColumnsEntity.FieldModifiedUserId, this.UserInfo.Id, null);
            builder.SetValue(BaseTableColumnsEntity.FieldModifiedBy, this.UserInfo.RealName, null);
            builder.SetDBNow(BaseTableColumnsEntity.FieldModifiedOn);
            return builder.EndInsert();
        }

        public virtual int Truncate()
        {
            return DbLogic.Truncate(this.DbHelper, this.CurrentTableName);
        }

        public virtual bool UpdateAfter()
        {
            return true;
        }

        public virtual bool UpdateBefore()
        {
            return true;
        }

        public virtual int UpdateEntity(object entity)
        {
            return 0;
        }

        public IDbHelper DbHelper
        {
            get
            {
                if (this.dbHelper == null)
                {
                    lock (locker)
                    {
                        if (this.dbHelper == null)
                        {
                            this.dbHelper = DbHelperFactory.GetHelper(CurrentDbType.SqlServer, null);
                            this.dbHelper.AutoOpenClose = true;
                        }
                    }
                }
                return this.dbHelper;
            }
            set
            {
                this.dbHelper = value;
            }
        }

        public string ReturnStatusCode
        {
            get
            {
                return this.returnStatusCode;
            }
            set
            {
                this.returnStatusCode = value;
            }
        }

        public string ReturnStatusMessage
        {
            get
            {
                return this.returnStatusMessage;
            }
            set
            {
                this.returnStatusMessage = value;
            }
        }
    }
}

