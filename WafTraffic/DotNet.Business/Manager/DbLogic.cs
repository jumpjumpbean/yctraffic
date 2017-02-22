//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

/// 修改纪录
/// 
///		2012.06.29 版本：2.0 sunmiao GetChildrens添加SQL Server2005递归查询支持
///		
/// 版本：2.0

namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class DbLogic
    {
        public static int Delete(IDbHelper dbHelper, string tableName)
        {
            return Delete(dbHelper, tableName, new string[0], new object[0]);
        }

        public static int Delete(IDbHelper dbHelper, string tableName, object[] ids)
        {
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                num += Delete(dbHelper, tableName, BaseBusinessLogic.FieldId, ids[i]);
            }
            return num;
        }

        public static int Delete(IDbHelper dbHelper, string tableName, object id)
        {
            return Delete(dbHelper, tableName, BaseBusinessLogic.FieldId, id);
        }

        public static int Delete(IDbHelper dbHelper, string tableName, string name, object[] values)
        {
            int num = 0;
            for (int i = 0; i < values.Length; i++)
            {
                num += Delete(dbHelper, tableName, name, values[i]);
            }
            return num;
        }

        public static int Delete(IDbHelper dbHelper, string tableName, string name, object value)
        {
            string[] names = new string[1];
            object[] values = new object[1];
            names[0] = name;
            values[0] = value;
            return Delete(dbHelper, tableName, names, values);
        }

        public static int Delete(IDbHelper dbHelper, string tableName, string[] names, object[] values)
        {
            string commandText = " DELETE  FROM " + tableName;
            string str2 = GetWhereString(dbHelper, ref names, values, BaseBusinessLogic.SQLLogicConditional);
            if (str2.Length > 0)
            {
                commandText = commandText + " WHERE " + str2;
            }
            return dbHelper.ExecuteNonQuery(commandText, dbHelper.MakeParameters(names, values));
        }

        public static int Delete(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2)
        {
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            names[1] = name2;
            values[0] = value1;
            values[1] = value2;
            return Delete(dbHelper, tableName, names, values);
        }

        public static bool Exists(IDbHelper dbHelper, string tableName, object id)
        {
            string[] names = new string[1];
            object[] values = new object[1];
            names[0] = BaseBusinessLogic.FieldId;
            values[0] = id;
            return Exists(dbHelper, tableName, names, values);
        }

        public static bool Exists(IDbHelper dbHelper, string tableName, object[] ids)
        {
            bool flag = false;
            for (int i = 0; i < ids.Length; i++)
            {
                flag = Exists(dbHelper, tableName, BaseBusinessLogic.FieldId, ids[i]);
                if (flag)
                {
                    return flag;
                }
            }
            return flag;
        }

        public static bool Exists(IDbHelper dbHelper, string tableName, string[] names, object[] values)
        {
            bool flag = false;
            string commandText = " SELECT COUNT(*)  FROM " + tableName + " WHERE " + GetWhereString(dbHelper, ref names, values, BaseBusinessLogic.SQLLogicConditional);
            object obj2 = dbHelper.ExecuteScalar(commandText, dbHelper.MakeParameters(names, values));
            if (obj2 != null)
            {
                flag = int.Parse(obj2.ToString()) > 0;
            }
            return flag;
        }

        public static bool Exists(IDbHelper dbHelper, string tableName, string name, object[] values)
        {
            bool flag = false;
            for (int i = 0; i < values.Length; i++)
            {
                flag = Exists(dbHelper, tableName, name, values[i]);
                if (flag)
                {
                    return flag;
                }
            }
            return flag;
        }

        public static bool Exists(IDbHelper dbHelper, string tableName, string name, object value)
        {
            string[] names = new string[1];
            object[] values = new object[1];
            names[0] = name;
            values[0] = value;
            return Exists(dbHelper, tableName, names, values);
        }

        public static bool Exists(IDbHelper dbHelper, string tableName, string name, object value, object id)
        {
            bool flag = false;
            string commandText = " SELECT COUNT(*) FROM " + tableName;
            if (name == null)
            {
                commandText = commandText + " WHERE (" + name + " IS NULL ) ";
            }
            else
            {
                string str2 = commandText;
                commandText = str2 + " WHERE (" + name + " = " + dbHelper.GetParameter(name) + ") ";
            }
            if (id != null)
            {
                string str3 = commandText;
                commandText = str3 + BaseBusinessLogic.SQLLogicConditional + "( " + BaseBusinessLogic.FieldId + " <> " + dbHelper.GetParameter(BaseBusinessLogic.FieldId) + " ) ";
            }
            string[] targetFileds = new string[2];
            object[] targetValues = new object[2];
            targetFileds[0] = name;
            targetValues[0] = value;
            if (id != null)
            {
                targetFileds[1] = BaseBusinessLogic.FieldId;
                targetValues[1] = id;
            }
            object obj2 = dbHelper.ExecuteScalar(commandText, dbHelper.MakeParameters(targetFileds, targetValues));
            if (obj2 != null)
            {
                flag = int.Parse(obj2.ToString()) > 0;
            }
            return flag;
        }

        public static bool Exists(IDbHelper dbHelper, string tableName, string[] names, object[] values, string primaryKey, object id)
        {
            bool flag = false;
            object obj3 = " SELECT COUNT(*)  FROM " + tableName + " WHERE " + GetWhereString(dbHelper, ref names, values, BaseBusinessLogic.SQLLogicConditional);
            string commandText = string.Concat(new object[] { obj3, BaseBusinessLogic.SQLLogicConditional, "( ", primaryKey, " <> '", id, "' ) " });
            object obj2 = dbHelper.ExecuteScalar(commandText, dbHelper.MakeParameters(names, values));
            if (obj2 != null)
            {
                flag = int.Parse(obj2.ToString()) > 0;
            }
            return flag;
        }

        public static bool Exists(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2)
        {
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            names[1] = name2;
            values[0] = value1;
            values[1] = value2;
            return Exists(dbHelper, tableName, names, values);
        }

        public static bool Exists(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2, object id)
        {
            return Exists(dbHelper, tableName, name1, value1, name2, value2, BaseBusinessLogic.FieldId, id);
        }

        public static bool Exists(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2, string primaryKey, object id)
        {
            bool flag = false;
            string commandText = " SELECT COUNT(*) FROM " + tableName;
            if (value1 == null)
            {
                commandText = commandText + " WHERE (" + name1 + " IS NULL) ";
            }
            else
            {
                string str2 = commandText;
                commandText = str2 + " WHERE (" + name1 + " = " + dbHelper.GetParameter(name1) + ") ";
            }
            if (value2 == null)
            {
                string str3 = commandText;
                commandText = str3 + BaseBusinessLogic.SQLLogicConditional + "(" + name2 + " IS NULL) ";
            }
            else
            {
                string str4 = commandText;
                commandText = str4 + BaseBusinessLogic.SQLLogicConditional + "(" + name2 + " = " + dbHelper.GetParameter(name2) + ") ";
            }
            if (id != null)
            {
                string str5 = commandText;
                commandText = str5 + BaseBusinessLogic.SQLLogicConditional + "( " + primaryKey + " <> " + dbHelper.GetParameter(primaryKey) + ") ";
            }
            string[] targetFileds = new string[3];
            object[] targetValues = new object[3];
            targetFileds[0] = name1;
            targetValues[0] = value1;
            targetFileds[1] = name2;
            targetValues[1] = value2;
            if (id != null)
            {
                targetFileds[2] = primaryKey;
                targetValues[2] = id;
            }
            object obj2 = dbHelper.ExecuteScalar(commandText, dbHelper.MakeParameters(targetFileds, targetValues));
            if (obj2 != null)
            {
                flag = int.Parse(obj2.ToString()) > 0;
            }
            return flag;
        }

        public static DataTable GetChildrens(IDbHelper dbHelper, string tableName, string fieldId, string id, string fieldParentId, string order)
        {
            return GetChildrens(dbHelper, tableName, fieldId, id, fieldParentId, order, false);
        }

        /// <summary>
        /// 通过递归查询取得某一节点及其所有下级节点（支持Oracle和Sql server 2005以上）
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldId"></param>
        /// <param name="id"></param>
        /// <param name="fieldParentId"></param>
        /// <param name="order"></param>
        /// <param name="idOnly"></param>
        /// <returns></returns>
        public static DataTable GetChildrens(IDbHelper dbHelper, string tableName, string fieldId, string id, string fieldParentId, string order, bool idOnly)
        {
            string commandText = string.Empty;
            DataTable dataTable = new DataTable(tableName);
            if (dbHelper.CurrentDbType == CurrentDbType.Oracle)
            {
                if (idOnly)
                {
                    commandText = "   SELECT " + fieldId;
                }
                else
                {
                    commandText = "   SELECT * ";
                }
                string str2 = commandText;
                commandText = str2 + "          FROM " + tableName + "    START WITH " + fieldId + " = " + dbHelper.GetParameter(fieldId) + "  CONNECT BY PRIOR " + fieldId + " = " + fieldParentId;
                if (!string.IsNullOrEmpty(order))
                {
                    commandText = commandText + " ORDER BY " + order;
                }
                string[] targetFileds = new string[] { fieldId };
                object[] targetValues = new object[] { id };
                dbHelper.Fill(dataTable, commandText, dbHelper.MakeParameters(targetFileds, targetValues));      
            }
            else if (dbHelper.CurrentDbType == CurrentDbType.SqlServer)
            {
                string selectField = string.Empty;
                if (idOnly)
                {
                    selectField = fieldId;
                }
                else
                {
                    selectField = "*";
                }

                commandText = " WITH Tree AS (SELECT " + selectField + " FROM " + tableName + " WHERE " + fieldId + " IN ('" + id + "') UNION ALL SELECT a." + selectField + " FROM " + tableName + " AS a INNER JOIN Tree AS b ON b." + fieldId + " = a." + fieldParentId + ")  SELECT * FROM Tree ";
                if (!string.IsNullOrEmpty(order))
                {
                    commandText = commandText + " ORDER BY " + order;
                }

                dbHelper.Fill(dataTable, commandText);
            }
            return dataTable;
        }

        public static DataTable GetChildrens(IDbHelper dbHelper, string tableName, string fieldId, string[] ids, string fieldParentId, string order, bool idOnly)
        {
            string commandText = string.Empty;
            DataTable dataTable = new DataTable(tableName);
            if (dbHelper.CurrentDbType == CurrentDbType.Oracle)
            {
                if (idOnly)
                {
                    commandText = "   SELECT " + fieldId;
                }
                else
                {
                    commandText = "   SELECT * ";
                }
                string str2 = commandText;
                commandText = str2 + "          FROM " + tableName + "    START WITH " + fieldId + " IN (" + BaseBusinessLogic.ObjectsToList(ids) + ")  CONNECT BY PRIOR " + fieldId + " = " + fieldParentId;
                if (!string.IsNullOrEmpty(order))
                {
                    commandText = commandText + " ORDER BY " + order;
                }
                dbHelper.Fill(dataTable, commandText);
            }
            else if (dbHelper.CurrentDbType == CurrentDbType.SqlServer)
            {
                string selectField = string.Empty;
                if (idOnly)
                {
                    selectField = fieldId;
                }
                else
                {
                    selectField = "*";
                }

                commandText = " WITH Tree AS (SELECT " + selectField + " FROM " + tableName + " WHERE " + fieldId + " IN (" + BaseBusinessLogic.ObjectsToList(ids) + ") UNION ALL SELECT a." + selectField + " FROM " + tableName + " AS a INNER JOIN Tree AS b ON b." + fieldId + " = a." + fieldParentId + ")  SELECT * FROM Tree ";
                if (!string.IsNullOrEmpty(order))
                {
                    commandText = commandText + " ORDER BY " + order;
                }
                dbHelper.Fill(dataTable, commandText);
            }
            return dataTable;
        }

        /// <summary>
        /// 通过Code字段值(类似001.005.001这种结构)取得某一节点及其所有下级节点
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldCode"></param>
        /// <param name="code"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static DataTable GetChildrensByCode(IDbHelper dbHelper, string tableName, string fieldCode, string code, string order)
        {
            return GetChildrensByCode(dbHelper, tableName, fieldCode, code, order, false);
        }

        private static DataTable GetChildrensByCode(IDbHelper dbHelper, string tableName, string fieldCode, string code, string order, bool idOnly)
        {
            string commandText = string.Empty;
            if (idOnly)
            {
                commandText = "   SELECT " + BaseBusinessLogic.FieldId;
            }
            else
            {
                commandText = "   SELECT * ";
            }
            commandText = commandText + "     FROM " + tableName;
            switch (dbHelper.CurrentDbType)
            {
                case CurrentDbType.Oracle:
                {
                    string str3 = commandText;
                    commandText = str3 + "    WHERE (SUBSTR(" + fieldCode + ", 1, LENGTH(" + dbHelper.GetParameter(fieldCode) + ")) = " + dbHelper.GetParameter(fieldCode) + " ) ";
                    break;
                }
                case CurrentDbType.SqlServer:
                case CurrentDbType.Access:
                {
                    string str2 = commandText;
                    commandText = str2 + "    WHERE (LEFT(" + fieldCode + ", LEN(" + dbHelper.GetParameter(fieldCode) + ")) = " + dbHelper.GetParameter(fieldCode) + " ) ";
                    break;
                }
            }
            if (!string.IsNullOrEmpty(order))
            {
                commandText = commandText + " ORDER BY " + order;
            }
            string[] targetFileds = new string[] { fieldCode, fieldCode };
            object[] targetValues = new object[] { code, code };
            DataTable dataTable = new DataTable(tableName);
            dbHelper.Fill(dataTable, commandText, dbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public static string[] GetChildrensId(IDbHelper dbHelper, string tableName, string fieldId, string id, string fieldParentId, string order)
        {
            return BaseBusinessLogic.FieldToArray(GetChildrens(dbHelper, tableName, fieldId, id, fieldParentId, order, true), BaseBusinessLogic.FieldId);
        }

        public static string[] GetChildrensIdByCode(IDbHelper dbHelper, string tableName, string fieldCode, string code, string order)
        {
            return BaseBusinessLogic.FieldToArray(GetChildrensByCode(dbHelper, tableName, fieldCode, code, order, true), BaseBusinessLogic.FieldId);
        }

        //public static int GetCount(IDbHelper dbHelper, string tableName, string whereConditional = null) //C# 4.0 才支持缺省参数
        public static int GetCount(IDbHelper dbHelper, string tableName, string whereConditional)
        {
            int num = 0;
            string commandText = " SELECT COUNT(*)  FROM " + tableName;
            if (!string.IsNullOrEmpty(whereConditional))
            {
                commandText = commandText + " WHERE " + whereConditional;
            }
            object obj2 = dbHelper.ExecuteScalar(commandText);
            if (obj2 != null)
            {
                num = int.Parse(obj2.ToString());
            }
            return num;
        }

        //用来模拟默认参数
        public static int GetCount(IDbHelper dbHelper, string tableName)
        {
            string whereConditional = null;
            return GetCount(dbHelper, tableName, whereConditional);

        }

        public static int GetCount(IDbHelper dbHelper, string tableName, string whereConditional, List<IDbDataParameter> dbParameters)
        {
            int num = 0;
            string commandText = " SELECT COUNT(*)  FROM " + tableName;
            if (!string.IsNullOrEmpty(whereConditional))
            {
                commandText = commandText + " WHERE " + whereConditional;
            }
            object obj2 = null;
            if ((dbParameters != null) && (dbParameters.Count > 0))
            {
                obj2 = dbHelper.ExecuteScalar(commandText, dbParameters.ToArray());
            }
            else
            {
                obj2 = dbHelper.ExecuteScalar(commandText);
            }
            if (obj2 != null)
            {
                num = int.Parse(obj2.ToString());
            }
            return num;
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName)
        {
            string order = string.Empty;
            int topLimit = 0;
            string[] names = new string[0];
            object[] values = new object[0];
            return GetDT(dbHelper, tableName, names, values, topLimit, order);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string order)
        {
            int topLimit = 0;
            string[] names = new string[0];
            object[] values = new object[0];
            return GetDT(dbHelper, tableName, names, values, topLimit, order);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, object[] ids)
        {
            return GetDT(dbHelper, tableName, BaseBusinessLogic.FieldId, ids);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, int topLimit, string order)
        {
            string[] names = new string[0];
            object[] values = new object[0];
            return GetDT(dbHelper, tableName, names, values, topLimit, order);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, object[] ids, string order)
        {
            return GetDT(dbHelper, tableName, BaseBusinessLogic.FieldId, ids, order);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string[] names, object[] values)
        {
            string order = string.Empty;
            int topLimit = 0;
            return GetDT(dbHelper, tableName, names, values, topLimit, order);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string name, object value)
        {
            string order = string.Empty;
            int topLimit = 0;
            string[] names = new string[1];
            object[] values = new object[1];
            names[0] = name;
            values[0] = value;
            return GetDT(dbHelper, tableName, names, values, topLimit, order);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string name, object[] values)
        {
            return GetDT(dbHelper, tableName, name, values, string.Empty);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string[] names, object[] values, string order)
        {
            int topLimit = 0;
            return GetDT(dbHelper, tableName, names, values, topLimit, order);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string name, object value, string order)
        {
            DataTable table = new DataTable(tableName);
            string commandText = " SELECT *    FROM " + tableName;
            if (value == null)
            {
                commandText = commandText + "  WHERE " + name + " IS NULL";
            }
            else
            {
                string str2 = commandText;
                commandText = str2 + "  WHERE " + name + " = " + dbHelper.GetParameter(name);
            }
            if (!string.IsNullOrEmpty(order))
            {
                commandText = commandText + " ORDER BY " + order;
            }
            if (value == null)
            {
                return dbHelper.Fill(commandText);
            }
            return dbHelper.Fill(commandText, new IDbDataParameter[] { dbHelper.MakeParameter(name, value) });
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string name, object[] values, string order)
        {
            string commandText = " SELECT *    FROM " + tableName;
            if (values == null)
            {
                commandText = commandText + "  WHERE " + name + " IS NULL";
            }
            else
            {
                string str2 = commandText;
                commandText = str2 + "  WHERE " + name + " IN (" + BaseBusinessLogic.ObjectsToList(values) + ")";
            }
            if (!string.IsNullOrEmpty(order))
            {
                commandText = commandText + " ORDER BY " + order;
            }
            return dbHelper.Fill(commandText);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, int currentPage, int numPerPage, string conditions, string orderby)
        {
            string str = (currentPage * numPerPage).ToString();
            string str2 = ((currentPage - 1) * numPerPage).ToString();
            string commandText = string.Format("SELECT * FROM (SELECT T.*, ROWNUM RN FROM (SELECT * FROM {0} where {1} order by {2}) T WHERE ROWNUM <= {3}) WHERE RN > {4}", new object[] { tableName, conditions, orderby, str, str2 });
            DataTable table = new DataTable(tableName);
            return dbHelper.Fill(commandText);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string name, object value, int topLimit, string order)
        {
            string[] names = new string[1];
            object[] values = new object[1];
            names[0] = name;
            values[0] = value;
            return GetDT(dbHelper, tableName, names, values, topLimit, order);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2)
        {
            int topLimit = 0;
            string order = string.Empty;
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            values[0] = value1;
            names[1] = name2;
            values[1] = value2;
            return GetDT(dbHelper, tableName, names, values, topLimit, order);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string[] names, object[] values, int topLimit, string order)
        {
            string commandText = " SELECT * FROM " + tableName;
            string str2 = string.Empty;
            if (topLimit != 0)
            {
                switch (dbHelper.CurrentDbType)
                {
                    case CurrentDbType.Oracle:
                        str2 = " ROWNUM < = " + topLimit;
                        break;

                    case CurrentDbType.SqlServer:
                    case CurrentDbType.Access:
                        commandText = " SELECT TOP " + topLimit.ToString() + " * FROM " + tableName;
                        break;
                }
            }
            string str3 = GetWhereString(dbHelper, ref names, values, BaseBusinessLogic.SQLLogicConditional);
            if (str3.Length > 0)
            {
                if (str2.Length > 0)
                {
                    str2 = str2 + BaseBusinessLogic.SQLLogicConditional + str3;
                }
                else
                {
                    str2 = str3;
                }
            }
            if (str2.Length > 0)
            {
                commandText = commandText + " WHERE " + str2;
            }
            if ((order != null) && (order.Length > 0))
            {
                commandText = commandText + " ORDER BY " + order;
            }
            if ((topLimit != 0) && (dbHelper.CurrentDbType == CurrentDbType.MySql))
            {
                commandText = commandText + " LIMIT 0, " + topLimit;
            }
            DataTable table = new DataTable(tableName);
            if (((names != null) && (values != null)) && ((names.Length > 0) && (values.Length > 0)))
            {
                return dbHelper.Fill(commandText, dbHelper.MakeParameters(names, values));
            }
            return dbHelper.Fill(commandText);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2, string order)
        {
            int topLimit = 0;
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            values[0] = value1;
            names[1] = name2;
            values[1] = value2;
            return GetDT(dbHelper, tableName, names, values, topLimit, order);
        }

        public static DataTable GetDT(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2, int topLimit, string order)
        {
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            values[0] = value1;
            names[1] = name2;
            values[1] = value2;
            return GetDT(dbHelper, tableName, names, values, topLimit, order);
        }

        public static DataTable GetDTByPage(IDbHelper dbHelper, string tableName, int currentPage, int numPerPage, string conditions, List<IDbDataParameter> dbParameters, string orderby)
        {
            string str = (currentPage * numPerPage).ToString();
            string str2 = ((currentPage - 1) * numPerPage).ToString();
            string commandText = string.Format("SELECT * FROM (SELECT T.*, ROWNUM RN FROM (SELECT * FROM {0} where {1} order by {2}) T WHERE ROWNUM <= {3}) WHERE RN > {4}", new object[] { tableName, conditions, orderby, str, str2 });
            DataTable table = new DataTable(tableName);
            if ((dbParameters != null) && (dbParameters.Count > 0))
            {
                return dbHelper.Fill(commandText, dbParameters.ToArray());
            }
            return dbHelper.Fill(commandText);
        }

        public static DataTable GetDTByPage(IDbHelper dbHelper, string tableName, int currentPage, int pageSize, string sqlQuery, string orderby, int recordCount)
        {
            string str = ((recordCount - ((currentPage - 1) * pageSize)) > pageSize) ? pageSize.ToString() : ((recordCount - ((currentPage - 1) * pageSize))).ToString();
            string str2 = (currentPage * pageSize).ToString();
            string str3 = ((currentPage + 1) * pageSize).ToString();
            string commandText = string.Empty;
            switch (dbHelper.CurrentDbType)
            {
                case CurrentDbType.SqlServer:
                    commandText = string.Format("SELECT *  FROM  ( SELECT Top {0} * FROM   (SELECT Top {1} * FROM ({2}) T Order by {3} asc) T1  Order by {4} desc ) T2  Order by {5} asc ", new object[] { str, str2, sqlQuery, orderby, orderby, orderby });
                    break;

                case CurrentDbType.DB2:
                    str2 = ((currentPage - 1) * pageSize).ToString();
                    str3 = (currentPage * pageSize).ToString();
                    commandText = " SELECT * FROM (  SELECT ROW_NUMBER() OVER(ORDER BY " + orderby + ") AS ROWNUM, " + sqlQuery.Substring(7) + "  ) A  WHERE ROWNUM > " + str2 + " and ROWNUM <= " + str3;
                    break;
            }
            DataTable table = new DataTable(tableName);
            return dbHelper.Fill(commandText);
        }

        public static DataTable GetFromProcedure(IDbHelper dbHelper, string procedureName, string tableName)
        {
            return dbHelper.ExecuteProcedureForDataTable(procedureName, tableName, null);
        }

        public static DataTable GetFromProcedure(IDbHelper dbHelper, string procedureName, string tableName, string id)
        {
            string[] targetFileds = new string[1];
            object[] targetValues = new object[1];
            targetFileds[0] = BaseBusinessLogic.FieldId;
            targetValues[0] = id;
            return dbHelper.ExecuteProcedureForDataTable(procedureName, tableName, dbHelper.MakeParameters(targetFileds, targetValues));
        }

        public static string GetId(IDbHelper dbHelper, string tableName, string[] names, object[] values)
        {
            return GetProperty(dbHelper, tableName, names, values, BaseBusinessLogic.FieldId);
        }

        public static string GetId(IDbHelper dbHelper, string tableName, string name, object value)
        {
            return GetProperty(dbHelper, tableName, name, value, BaseBusinessLogic.FieldId);
        }

        public static string GetId(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2)
        {
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            names[1] = name2;
            values[0] = value1;
            values[1] = value2;
            return GetProperty(dbHelper, tableName, names, values, BaseBusinessLogic.FieldId);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName)
        {
            string targetField = string.Empty;
            int num = 0;
            string[] names = new string[0];
            object[] values = new object[0];
            return GetIds(dbHelper, tableName, names, values, new int?(num), targetField);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, string targetField)
        {
            int num = 0;
            string[] names = new string[0];
            object[] values = new object[0];
            return GetIds(dbHelper, tableName, names, values, new int?(num), targetField);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, string name, object[] values)
        {
            return GetIds(dbHelper, tableName, name, values, BaseBusinessLogic.FieldId);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, int topLimit, string targetField)
        {
            string[] names = new string[0];
            object[] values = new object[0];
            return GetIds(dbHelper, tableName, names, values, new int?(topLimit), targetField);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, string name, object value)
        {
            string targetField = string.Empty;
            int num = 0;
            string[] names = new string[1];
            object[] values = new object[1];
            names[0] = name;
            values[0] = value;
            return GetIds(dbHelper, tableName, names, values, new int?(num), targetField);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, string[] names, object[] values)
        {
            string targetField = string.Empty;
            int num = 0;
            return GetIds(dbHelper, tableName, names, values, new int?(num), targetField);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, string name, object[] values, string targetField)
        {
            string commandText = " SELECT " + targetField + "   FROM " + tableName + "  WHERE " + name + " IN (" + BaseBusinessLogic.ObjectsToList(values) + ")";
            return BaseBusinessLogic.FieldToArray(dbHelper.Fill(commandText), targetField);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, string name, object value, string targetField)
        {
            int num = 0;
            string[] names = new string[1];
            object[] values = new object[1];
            names[0] = name;
            values[0] = value;
            return GetIds(dbHelper, tableName, names, values, new int?(num), targetField);
        }

        //public static string[] GetIds(IDbHelper dbHelper, string tableName, string[] names, object[] values, string targetField = null) //C# 4.0 才支持缺省参数
        public static string[] GetIds(IDbHelper dbHelper, string tableName, string[] names, object[] values, string targetField)
        {
            int num = 0;
            return GetIds(dbHelper, tableName, names, values, new int?(num), targetField);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, string name, object value, int topLimit, string targetField)
        {
            string[] names = new string[1];
            object[] values = new object[1];
            names[0] = name;
            values[0] = value;
            return GetIds(dbHelper, tableName, names, values, new int?(topLimit), targetField);
        }

        //public static string[] GetIds(IDbHelper dbHelper, string tableName, string[] names, object[] values, int? topLimit = new int?(), string targetField = null) //C# 4.0 才支持缺省参数
        public static string[] GetIds(IDbHelper dbHelper, string tableName, string[] names, object[] values, int? topLimit, string targetField) 
        {
            if (string.IsNullOrEmpty(targetField))
            {
                targetField = BaseBusinessLogic.FieldId;
            }
            string commandText = " SELECT " + targetField + " FROM " + tableName;
            string str2 = string.Empty;
            if (topLimit.HasValue && (topLimit > 0))
            {
                switch (dbHelper.CurrentDbType)
                {
                    case CurrentDbType.Oracle:
                        str2 = " ROWNUM < = " + topLimit;
                        break;

                    case CurrentDbType.SqlServer:
                    case CurrentDbType.Access:
                        commandText = " SELECT TOP " + topLimit.ToString() + targetField + " FROM " + tableName;
                        break;
                }
            }
            string str3 = GetWhereString(dbHelper, ref names, values, BaseBusinessLogic.SQLLogicConditional);
            if (str3.Length > 0)
            {
                if (str2.Length > 0)
                {
                    str2 = str2 + BaseBusinessLogic.SQLLogicConditional + str3;
                }
                else
                {
                    str2 = str3;
                }
            }
            if (str2.Length > 0)
            {
                commandText = commandText + " WHERE " + str2;
            }
            if (topLimit.HasValue && (dbHelper.CurrentDbType == CurrentDbType.MySql))
            {
                commandText = commandText + " LIMIT 0, " + topLimit;
            }
            DataTable dataTable = new DataTable(tableName);
            dbHelper.Fill(dataTable, commandText, dbHelper.MakeParameters(names, values));
            return BaseBusinessLogic.FieldToArray(dataTable, targetField);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2)
        {
            int num = 0;
            string targetField = string.Empty;
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            values[0] = value1;
            names[1] = name2;
            values[1] = value2;
            return GetIds(dbHelper, tableName, names, values, new int?(num), targetField);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2, string targetField)
        {
            int num = 0;
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            values[0] = value1;
            names[1] = name2;
            values[1] = value2;
            return GetIds(dbHelper, tableName, names, values, new int?(num), targetField);
        }

        public static string[] GetIds(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2, int topLimit, string targetField)
        {
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            values[0] = value1;
            names[1] = name2;
            values[1] = value2;
            return GetIds(dbHelper, tableName, names, values, new int?(topLimit), targetField);
        }

        public static DataTable GetParentChildrensByCode(IDbHelper dbHelper, string tableName, string fieldCode, string code, string order)
        {
            return GetParentChildrensByCode(dbHelper, tableName, fieldCode, code, order, false);
        }

        private static DataTable GetParentChildrensByCode(IDbHelper dbHelper, string tableName, string fieldCode, string code, string order, bool idOnly)
        {
            string commandText = string.Empty;
            if (idOnly)
            {
                commandText = "   SELECT " + BaseBusinessLogic.FieldId;
            }
            else
            {
                commandText = "   SELECT * ";
            }
            commandText = commandText + "     FROM " + tableName;
            switch (dbHelper.CurrentDbType)
            {
                case CurrentDbType.Oracle:
                {
                    string str4 = commandText;
                    string str5 = str4 + "    WHERE (SUBSTR(" + fieldCode + ", 1, LENGTH(" + dbHelper.GetParameter(fieldCode) + ")) = " + dbHelper.GetParameter(fieldCode) + ") ";
                    commandText = str5 + "          OR (" + fieldCode + " = SUBSTR(" + dbHelper.GetParameter(fieldCode) + ", 1, LENGTH(" + fieldCode + "))) ";
                    break;
                }
                case CurrentDbType.SqlServer:
                case CurrentDbType.Access:
                {
                    string str2 = commandText;
                    string str3 = str2 + "    WHERE (LEFT(" + fieldCode + ", LEN(" + dbHelper.GetParameter(fieldCode) + ")) = " + dbHelper.GetParameter(fieldCode) + ") ";
                    commandText = str3 + "          OR (LEFT(" + dbHelper.GetParameter(fieldCode) + ", LEN(" + fieldCode + ")) = " + fieldCode + ") ";
                    break;
                }
            }
            if (!string.IsNullOrEmpty(order))
            {
                commandText = commandText + " ORDER BY " + order;
            }
            string[] targetFileds = new string[] { fieldCode, fieldCode, fieldCode };
            object[] targetValues = new object[] { code, code, code };
            DataTable dataTable = new DataTable("DotNet");
            dbHelper.Fill(dataTable, commandText, dbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public static string[] GetParentChildrensIdByCode(IDbHelper dbHelper, string tableName, string fieldCode, string code, string order)
        {
            return BaseBusinessLogic.FieldToArray(GetParentChildrensByCode(dbHelper, tableName, fieldCode, code, order, true), BaseBusinessLogic.FieldId);
        }

        public static string GetParentIdByCode(IDbHelper dbHelper, string tableName, string fieldCode, string code)
        {
            string str = string.Empty;
            string commandText = " SELECT MAX(Id) AS Id     FROM " + tableName + "  WHERE (LEFT(" + dbHelper.GetParameter(fieldCode) + ", LEN(" + fieldCode + ")) = " + fieldCode + ")     AND " + fieldCode + " <>  '" + code + " ' ";
            string[] targetFileds = new string[1];
            object[] targetValues = new object[1];
            targetFileds[0] = fieldCode;
            targetValues[0] = code;
            object obj2 = dbHelper.ExecuteScalar(commandText, dbHelper.MakeParameters(targetFileds, targetValues));
            if (obj2 != null)
            {
                str = obj2.ToString();
            }
            return str;
        }

        public static DataTable GetParentsByCode(IDbHelper dbHelper, string tableName, string fieldCode, string code, string order)
        {
            return GetParentsByCode(dbHelper, tableName, fieldCode, code, order, false);
        }

        public static DataTable GetParentsByCode(IDbHelper dbHelper, string tableName, string fieldCode, string code, string order, bool idOnly)
        {
            string commandText = string.Empty;
            if (idOnly)
            {
                commandText = "   SELECT " + BaseBusinessLogic.FieldId;
            }
            else
            {
                commandText = "   SELECT * ";
            }
            commandText = commandText + "     FROM " + tableName;
            switch (dbHelper.CurrentDbType)
            {
                case CurrentDbType.Oracle:
                {
                    string str3 = commandText;
                    commandText = str3 + "    WHERE (SUBSTR(" + dbHelper.GetParameter(fieldCode) + ", 1, LENGTH(" + fieldCode + ")) = " + fieldCode + ") ";
                    break;
                }
                case CurrentDbType.SqlServer:
                case CurrentDbType.Access:
                {
                    string str2 = commandText;
                    commandText = str2 + "    WHERE (LEFT(" + dbHelper.GetParameter(fieldCode) + ", LEN(" + fieldCode + ")) = " + fieldCode + ") ";
                    break;
                }
            }
            if (!string.IsNullOrEmpty(order))
            {
                commandText = commandText + " ORDER BY " + order;
            }
            string[] targetFileds = new string[1];
            object[] targetValues = new object[1];
            targetFileds[0] = fieldCode;
            targetValues[0] = code;
            DataTable dataTable = new DataTable(tableName);
            dbHelper.Fill(dataTable, commandText, dbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public static string[] GetParentsIDByCode(IDbHelper dbHelper, string tableName, string fieldCode, string code, string order)
        {
            return BaseBusinessLogic.FieldToArray(GetParentsByCode(dbHelper, tableName, fieldCode, code, order, true), BaseBusinessLogic.FieldId);
        }

        public static string GetProperty(IDbHelper dbHelper, string tableName, string name, object value, string targetField)
        {
            string[] names = new string[1];
            object[] values = new object[1];
            names[0] = name;
            values[0] = value;
            return GetProperty(dbHelper, tableName, names, values, targetField);
        }

        public static string GetProperty(IDbHelper dbHelper, string tableName, string[] names, object[] values, string targetField)
        {
            string str = string.Empty;
            string commandText = " SELECT " + targetField + " FROM " + tableName + " WHERE " + GetWhereString(dbHelper, ref names, values, BaseBusinessLogic.SQLLogicConditional);
            object obj2 = dbHelper.ExecuteScalar(commandText, dbHelper.MakeParameters(names, values));
            if (obj2 != null)
            {
                str = obj2.ToString();
            }
            return str;
        }

        public static string GetProperty(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2, string targetField)
        {
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            names[1] = name2;
            values[0] = value1;
            values[1] = value2;
            return GetProperty(dbHelper, tableName, names, values, targetField);
        }

        public static string GetWhereString(IDbHelper dbHelper, ref string[] names, object[] values, string relation)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            for (int i = 0; i < names.Length; i++)
            {
                if ((names[i] != null) && (names[i].Length > 0))
                {
                    if (values[i] == null)
                    {
                        str2 = " (" + names[i] + " IS NULL) ";
                        names[i] = null;
                    }
                    else if (values[i] is Array)
                    {
                        if (((Array) values[i]).Length > 0)
                        {
                            str2 = " (" + names[i] + " IN (" + BaseBusinessLogic.ArrayToList((string[]) values[i], "'") + ")) ";
                        }
                        else
                        {
                            str2 = " (" + names[i] + " IS NULL) ";
                        }
                        names[i] = null;
                    }
                    else
                    {
                        str2 = " (" + names[i] + " = " + dbHelper.GetParameter(names[i]) + ") ";
                    }
                    str = str + str2 + relation;
                }
            }
            if (str.Length > 0)
            {
                str = str.Substring(0, (str.Length - relation.Length) - 1);
            }
            return str;
        }

        public static int Insert(IDbHelper dbHelper, string tableName, string[] targetFields, object[] targetValues)
        {
            SQLBuilder builder = new SQLBuilder(dbHelper);
            builder.BeginInsert(tableName);
            for (int i = 0; i < targetFields.Length; i++)
            {
                builder.SetValue(targetFields[i], targetValues[i], null);
            }
            return builder.EndInsert();
        }

        public static bool IsModifed(DataRow dataRow, string oldModifiedUserId, DateTime? oldModifiedOn)
        {
            bool flag = false;
            if ((dataRow[BaseBusinessLogic.FieldModifiedUserId] == DBNull.Value) || (dataRow[BaseBusinessLogic.FieldModifiedOn] == DBNull.Value))
            {
                return false;
            }
            DateTime time = DateTime.Parse(dataRow[BaseBusinessLogic.FieldModifiedOn].ToString());
            return ((!dataRow[BaseBusinessLogic.FieldModifiedUserId].ToString().Equals(oldModifiedUserId) && (oldModifiedOn == time)) || flag);
        }

        private static bool IsModifed(DataTable dataTable, string oldModifiedUserId, DateTime? oldModifiedOn)
        {
            bool flag = false;
            foreach (DataRow row in dataTable.Rows)
            {
                flag = IsModifed(row, oldModifiedUserId, oldModifiedOn);
            }
            return flag;
        }

        public static bool IsModifed(IDbHelper dbHelper, string tableName, object id, string oldModifiedUserId, DateTime? oldModifiedOn)
        {
            return IsModifed(dbHelper, tableName, BaseBusinessLogic.FieldId, id, oldModifiedUserId, oldModifiedOn);
        }

        public static bool IsModifed(IDbHelper dbHelper, string tableName, string fieldName, object fieldValue, string oldModifiedUserId, DateTime? oldModifiedOn)
        {
            string commandText = " SELECT " + BaseBusinessLogic.FieldId + "," + BaseBusinessLogic.FieldCreateUserId + "," + BaseBusinessLogic.FieldCreateOn + "," + BaseBusinessLogic.FieldModifiedUserId + "," + BaseBusinessLogic.FieldModifiedOn + " FROM " + tableName + " WHERE " + fieldName + " = " + dbHelper.GetParameter(fieldName);
            return IsModifed(dbHelper.Fill(commandText, new IDbDataParameter[] { dbHelper.MakeParameter(fieldName, fieldValue) }), oldModifiedUserId, oldModifiedOn);
        }

        public static int LockNoWait(IDbHelper dbHelper, string tableName, string[] ids)
        {
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                num += LockNoWait(dbHelper, tableName, BaseBusinessLogic.FieldId, ids[i]);
            }
            return num;
        }

        public static int LockNoWait(IDbHelper dbHelper, string tableName, string id)
        {
            return LockNoWait(dbHelper, tableName, BaseBusinessLogic.FieldId, id);
        }

        /// <summary>
        /// 采用NOWAIT方式来进行检索(Oracle特有)
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="tableName"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>若数据被锁定返回-1</returns>
        public static int LockNoWait(IDbHelper dbHelper, string tableName, string name, object value)
        {
            string commandText = " SELECT " + BaseBusinessLogic.FieldId + " FROM " + tableName;
            if (name == null)
            {
                commandText = commandText + " WHERE (" + name + " IS NULL ) ";
            }
            else
            {
                string str2 = commandText;
                commandText = str2 + " WHERE (" + name + " = " + dbHelper.GetParameter(name) + ")";
            }
            commandText = commandText + " FOR UPDATE NOWAIT ";
            try
            {
                DataSet dataSet = new DataSet();
                dbHelper.Fill(dataSet, commandText, "ForUpdateNoWait", new IDbDataParameter[] { dbHelper.MakeParameter(name, value) });
                return dataSet.Tables["ForUpdateNoWait"].Rows.Count;
            }
            catch
            {
                return -1;
            }
        }

        public static int LockNoWait(IDbHelper dbHelper, string tableName, string[] names, object[] values)
        {
            string commandText = (" SELECT " + BaseBusinessLogic.FieldId + " FROM " + tableName + " WHERE " + GetWhereString(dbHelper, ref names, values, BaseBusinessLogic.SQLLogicConditional)) + " FOR UPDATE NOWAIT ";
            try
            {
                DataSet dataSet = new DataSet();
                dbHelper.Fill(dataSet, commandText, "ForUpdateNoWait", dbHelper.MakeParameters(names, values));
                return dataSet.Tables["ForUpdateNoWait"].Rows.Count;
            }
            catch
            {
                return -1;
            }
        }

        public static int LockNoWait(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2)
        {
            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = name1;
            names[1] = name2;
            values[0] = value1;
            values[1] = value2;
            return LockNoWait(dbHelper, tableName, names, values);
        }

        public static int SetProperty(IDbHelper dbHelper, string tableName, string targetField, object targetValue)
        {
            SQLBuilder builder = new SQLBuilder(dbHelper);
            builder.BeginUpdate(tableName);
            builder.SetValue(targetField, targetValue, null);
            return builder.EndUpdate();
        }

        public static int SetProperty(IDbHelper dbHelper, string tableName, string name, object value, string targetField, object targetValue)
        {
            SQLBuilder builder = new SQLBuilder(dbHelper);
            builder.BeginUpdate(tableName);
            builder.SetValue(targetField, targetValue, null);
            builder.SetWhere(name, value);
            return builder.EndUpdate();
        }

        public static int SetProperty(IDbHelper dbHelper, string tableName, string name, object value, string[] targetFields, object[] targetValues)
        {
            SQLBuilder builder = new SQLBuilder(dbHelper);
            builder.BeginUpdate(tableName);
            for (int i = 0; i < targetFields.Length; i++)
            {
                builder.SetValue(targetFields[i], targetValues[i], null);
            }
            builder.SetWhere(name, value);
            return builder.EndUpdate();
        }

        public static int SetProperty(IDbHelper dbHelper, string tableName, string name, object[] value, string[] targetFields, object[] targetValues)
        {
            SQLBuilder builder = new SQLBuilder(dbHelper);
            builder.BeginUpdate(tableName);
            for (int i = 0; i < targetFields.Length; i++)
            {
                builder.SetValue(targetFields[i], targetValues[i], null);
            }
            builder.SetWhere(name, value);
            return builder.EndUpdate();
        }

        public static int SetProperty(IDbHelper dbHelper, string tableName, string name, object[] values, string targetField, object targetValue)
        {
            int num = 0;
            if (values == null)
            {
                return SetProperty(dbHelper, tableName, name, string.Empty, targetField, targetValue);
            }
            for (int i = 0; i < values.Length; i++)
            {
                num += SetProperty(dbHelper, tableName, name, values[i], targetField, targetValue);
            }
            return num;
        }

        public static int SetProperty(IDbHelper dbHelper, string tableName, string[] names, object[] values, string targetField, object targetValue)
        {
            SQLBuilder builder = new SQLBuilder(dbHelper);
            builder.BeginUpdate(tableName);
            builder.SetValue(targetField, targetValue, null);
            builder.SetWhere(names, values);
            return builder.EndUpdate();
        }

        public static int SetProperty(IDbHelper dbHelper, string tableName, string name1, object value1, string name2, object value2, string targetField, object targetValue)
        {
            SQLBuilder builder = new SQLBuilder(dbHelper);
            builder.BeginUpdate(tableName);
            builder.SetValue(targetField, targetValue, null);
            builder.SetWhere(name1, value1);
            builder.SetWhere(name2, value2);
            return builder.EndUpdate();
        }

        public static string SqlSafe(string value)
        {
            value = value.Replace("'", "''");
            return value;
        }

        public static int Truncate(IDbHelper dbHelper, string tableName)
        {
            string commandText = " TRUNCATE TABLE " + tableName;
            if (dbHelper.CurrentDbType == CurrentDbType.DB2)
            {
                commandText = " ALTER TABLE " + tableName + " ACTIVATE NOT LOGGED INITIALLY WITH EMPTY TABLE ";
            }
            return dbHelper.ExecuteNonQuery(commandText);
        }

        public static int UpdateRecord(IDbHelper dbHelper, string tableName, string name, string value, string targetField, object targetValue)
        {
            SQLBuilder builder = new SQLBuilder(dbHelper);
            builder.BeginUpdate(tableName);
            builder.SetValue(targetField, targetValue, null);
            builder.SetWhere(name, value);
            return builder.EndUpdate();
        }
    }
}

