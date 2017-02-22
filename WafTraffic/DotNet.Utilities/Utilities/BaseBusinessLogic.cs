//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotNet.Utilities
{

    /// 
    /// 修改纪录
    /// 
    ///		2009.09.08 版本：4.4	JiRiGaLa 改进 GetPermissionScope(string[] organizeIds)。
    ///		2008.08.29 版本：4.3	JiRiGaLa 改进 DataTableToString 的 null值处理技术。
    ///		2007.11.08 版本：4.2	JiRiGaLa 改进 DataTableToStringList 为 FieldToList。
    ///		2007.11.05 版本：4.1	JiRiGaLa 改进 GetDS、GetDT 功能，整体思路又上一个台阶，基类的又一次飞跃。
    ///		2007.11.05 版本：4.0	JiRiGaLa 改进 支持不是“Id”为字段的主键表。
    ///		2007.11.01 版本：3.9	JiRiGaLa 改进 BUOperatorInfo 去掉这个变量，可以提高性能，提高速度，基类的又一次飞跃。
    ///		2007.09.13 版本：3.8	JiRiGaLa 改进 BUBaseBusinessLogic.SQLLogicConditional 错误。
    ///		2007.08.14 版本：3.7	JiRiGaLa 改进 WebService 模式下 DataSet 传输数据的速度问题。
    ///		2007.07.20 版本：3.6	JiRiGaLa 改进 DataSet 修改为 DataTable 应该能提高一些速度吧。
    ///		2007.05.20 版本：3.6	JiRiGaLa 改进 GetList() 方法整理，这次又应该是一次升华，质的飞跃很不容易啊，身心都有提高了。
    ///		2007.05.20 版本：3.4	JiRiGaLa 改进 Exists() 方法整理。
    ///		2007.05.13 版本：3.3	JiRiGaLa 改进 GetProperty()，SetProperty()，Delete() 方法整理。
    ///		2007.05.10 版本：3.2	JiRiGaLa 改进 GetList() 方法整理。
    ///		2007.04.10 版本：3.1	JiRiGaLa 添加 GetNextId，GetPreviousId 方法整理。
    ///		2007.03.03 版本：3.0	JiRiGaLa 进行了一次彻底的优化工作，方法的位置及功能整理。
    ///		2007.03.01 版本：2.0	JiRiGaLa 重新调整主键的规范化。
    ///		2006.02.05 版本：1.1	JiRiGaLa 重新调整主键的规范化。
    ///		2005.12.30 版本：1.0	JiRiGaLa 数据库连接方式都进行改进
    ///		2005.09.04 版本：1.0	JiRiGaLa 执行数据库脚本
    ///		2005.08.19 版本：1.0	整理一下编排	
    ///		2005.07.10 版本：1.0	修改了程序，格式以及理念都有些提高，应该是一次大突破
    ///		2004.11.12 版本：1.0	添加了最新的GetParent、GetChildren、GetParentChildren 方法
    ///		2004.07.21 版本：1.0	UpdateRecord、Delete、SetProperty、GetProperty、ExecuteNonQuery、GetRecord 方法进行改进。
    ///								还删除一些重复的主键，提取了最优化的方法，有时候写的主键真的是垃圾，可能自己也没有注意时就写出了垃圾。
    ///								GetRepeat、GetDayOfWeek、ExecuteProcedure、GetFromProcedure 方法进行改进，基本上把所有的方法都重新写了一遍。
    ///	


    /// <summary>
    ///	BaseBusinessLogic
    /// 通用业务逻辑处理类
    /// 版本：4.4
    /// </summary>
    public class BaseBusinessLogic
    {
        public static string FieldId = "Id";                        // 主键字段
        public static string FieldParentId = "ParentId";            // 上级字段
        public static string FieldCode = "Code";                    // 编号字段
        public static string FieldFullName = "FullName";            // 名称字段
        public static string FieldCategoryId = "CategoryId";        // 类别字段
        public static string FieldEnabled = "Enabled";              // 有效字段
        public static string FieldDeletionStateCode = "DeletionStateCode";        // 删除标志
        public static string FieldSortCode = "SortCode";            // 排序码
        public static string FieldCreateUserId = "CreateUserId";    // 创建者主键
        public static string FieldCreateOn = "CreateOn";        // 创建时间
        public static string FieldModifiedUserId = "ModifiedUserId";    // 最后修改者主键
        public static string FieldModifiedOn = "ModifiedOn";        // 最后修改时间

        public static string SQLLogicConditional = " AND ";         // 查询逻辑

        public static string SelectedColumn = "Selected";           // 选择列
        
        /// <summary>
        /// 当发布评论时
        /// </summary>
        /// <param name="categoryId">类别主键</param>
        /// <param name="id">选择的主键</param>
        /// <returns>是否成功</returns>
        public delegate bool OnCommnetEventHandler(string categoryId, string id);

        /// <summary>
        /// 检查转移
        /// </summary>
        /// <param name="selectedId">选择的主键</param>
        /// <returns>是否成功</returns>
        public delegate bool CheckMoveEventHandler(string selectedId);

        /// <summary>
        /// 选择主键发生变化
        /// </summary>
        /// <param name="selectedId">选择的主键</param>
        public delegate void SelectedIndexChangedEventHandler(string selectedId);

        #region public static string SqlSafe(string value) 检查参数的安全性
        /// <summary>
        /// 检查参数的安全性
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns>安全的参数</returns>
        public static string SqlSafe(string value)
        {
            value = value.Replace("'", "''");
            // value = value.Replace("%", "'%");
            return value;
        }
        #endregion

        #region public static string NewGuid() 获得 Guid
        /// <summary>
        /// 获得 Guid
        /// </summary>
        /// <returns>主键</returns>
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion

        public static string GetAuditStatus(AuditStatus auditStatus)
        {
            return GetAuditStatus(auditStatus.ToString());
        }

        public static string GetAuditStatus(string auditStatus)
        {
            string returnValue = auditStatus;
            if (AuditStatus.Draft.ToString().Equals(auditStatus))
            {
                returnValue = "草稿";
            }
            else if (AuditStatus.StartAudit.ToString().Equals(auditStatus))
            {
                returnValue = "提交";
            }
            else if (AuditStatus.WaitForAudit.ToString().Equals(auditStatus))
            {
                returnValue = "待审";
            }
            else if (AuditStatus.AuditPass.ToString().Equals(auditStatus))
            {
                returnValue = "通过";
            }
            else if (AuditStatus.AuditReject.ToString().Equals(auditStatus))
            {
                returnValue = "退回";
            }
            else if (AuditStatus.Transmit.ToString().Equals(auditStatus))
            {
                returnValue = "转发";
            }
            else if (AuditStatus.AuditComplete.ToString().Equals(auditStatus))
            {
                returnValue = "完成";
            }
            else if (AuditStatus.AuditQuash.ToString().Equals(auditStatus))
            {
                returnValue = "已废弃";
            }
            return returnValue;
        }

        /// <summary>
        /// 按数据类型获取数据库访问实现类
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns>数据库访问实现类</returns>
        public static string GetDbHelperClass(CurrentDbType dbType)
        {
            string returnValue = "DotNet.Utilities.SqlHelper";
            switch (dbType)
            {
                case CurrentDbType.SqlServer:
                    returnValue = "DotNet.Utilities.SqlHelper";
                    break;
                case CurrentDbType.Oracle:
                    returnValue = "DotNet.Utilities.MSOracleHelper";
                    break;
                case CurrentDbType.MySql:
                    returnValue = "DotNet.Utilities.MySqlHelper";
                    break;
                case CurrentDbType.DB2:
                    returnValue = "DotNet.Utilities.DB2Helper";
                    break;
                case CurrentDbType.SQLite:
                    returnValue = "DotNet.Utilities.SqLiteHelper";
                    break;
            }
            return returnValue;
        }

        /// <summary>
        /// 判断是否关键字
        /// </summary>
        /// <param name="field">字段</param>
        /// <returns>是关键字</returns>
        public static bool IsKeywords(string field)
        {
            bool returnValue = false;
            // 首字母进行强制大写改进
            field = field.Substring(0, 1).ToUpper() + field.Substring(1);

            string[] keywords = new string[] { "Id", "SortCode", "DeletionStateCode", "Enabled", "CreateOn", "CreateUserId", "CreateBy", "ModifiedOn", "ModifiedUserId", "ModifiedBy" };

            for (int y = 0; y < keywords.Length; y++)
            {
                // 防止大小写问题发生
                if (keywords[y].ToUpper().Equals(field.ToUpper()))
                {
                    // 进行大小写转换
                    field = keywords[y];
                    returnValue = true;
                    break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 过滤表中的字段
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="columns">过滤字段</param>
        /// <returns>过滤后的结果</returns>
        public static DataTable SetColumnsFilter(DataTable dataTable, string[] columns)
        {
            for (int i = dataTable.Columns.Count - 1; i > 0; i--)
            {
                // 不是关键字的才过滤，全过滤了，没法用了
                if (!IsKeywords(dataTable.Columns[i].ColumnName))
                {
                    // 都统一转为大写，比较省事一些，有必要时再改
                    if (!BaseBusinessLogic.Exists(columns, dataTable.Columns[i].ColumnName))
                    {
                        dataTable.Columns.RemoveAt(i);
                    }
                }
            }
            return dataTable;
        }

        //
        // 从内存判断是否有相应的权限
        //

        public static bool Exists(DataTable dataTable, string fieldName, string fieldValue)
        {
            bool returnValue = false;
            if (dataTable == null)
            {
                return returnValue;
            }
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow[fieldName].ToString().Equals(fieldValue))
                {
                    returnValue = true;
                    break;
                }
            }
            return returnValue;
        }

        #region public static bool IsAuthorized(DataTable dataTable, string permissionItemCode) 是否有相应的权限
        /// <summary>
        /// 是否有相应的权限
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="permissionItemCode">权限编号</param>
        /// <returns>是否有权限</returns>
        public static bool IsAuthorized(DataTable dataTable, string permissionItemCode)
        {
            return Exists(dataTable, FieldCode, permissionItemCode);
        }
        #endregion

        /// <summary>
        /// 获取权限范围的设置
        /// </summary>
        /// <param name="organizeIds">有权限的组织机构</param>
        /// <returns>权限范围</returns>
        public static PermissionScope GetPermissionScope(string[] organizeIds)
        {
            PermissionScope returnValue = PermissionScope.None;
            foreach (PermissionScope permissionScope in (PermissionScope[])Enum.GetValues(typeof(PermissionScope)))
            {
                if (BaseBusinessLogic.Exists(organizeIds, permissionScope.ToString()))
                {
                    returnValue = permissionScope;
                    break;
                }
            }
            return returnValue;
        }

        //
        // WebService 传递参数的专用方法
        //

        #region public static byte[] GetBinaryFormatData(DataTable dataTable) 服务器上面取数据,填充数据权限,转换为二进制格式.
        /// <summary>
        /// 服务器上面取数据,填充数据权限,转换为二进制格式.
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>二进制格式</returns>
        public static byte[] GetBinaryFormatData(DataTable dataTable)
        {
            byte[] ArrayResult = null;
            dataTable.RemotingFormat = SerializationFormat.Binary;
            MemoryStream memoryStream = new MemoryStream();
            IFormatter IFormatter = new BinaryFormatter();
            IFormatter.Serialize(memoryStream, dataTable);
            ArrayResult = memoryStream.ToArray();
            memoryStream.Close();
            memoryStream.Dispose();
            return ArrayResult;
        }
        #endregion

        #region public static DataTable RetrieveDataSet(byte[] ArrayResult) 客户端接收到byte[]格式的数据,对其进行反序列化,得到数据权限,进行客户端操作.
        /// <summary>
        /// 客户端接收到byte[]格式的数据,对其进行反序列化,得到数据权限,进行客户端操作.
        /// </summary>
        /// <param name="ArrayResult">二进制格式</param>
        /// <returns>数据表</returns>
        public static DataTable RetrieveDataTable(byte[] arrayResult)
        {
            DataTable dataTable = null;
            MemoryStream memoryStream = new MemoryStream(arrayResult);
            IFormatter IFormatter = new BinaryFormatter();
            Object targetObject = IFormatter.Deserialize(memoryStream);
            memoryStream.Close();
            memoryStream.Dispose();
            dataTable = (DataTable)targetObject;
            return dataTable;
        }
        #endregion

        //
        // 不是针对数据库的常用方法
        //

        #region public static string RepeatString(string targetString, int repeatCount) 重复字符串
        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="repeatCount">重复次数</param>
        /// <returns>结果字符串</returns>
        public static string RepeatString(string targetString, int repeatCount)
        {
            string returnValue = string.Empty;
            for (int i = 0; i < repeatCount; i++)
            {
                returnValue += targetString;
            }
            return returnValue;
        }
        #endregion

        #region public static string GetDateTime(DataRow dataRow, string name) 获取日期时间
        /// <summary>
        /// 获取日期时间
        /// </summary>
        /// <param name="dataRow">数据行</param>
        /// <param name="name">字段名</param>
        /// <returns>日期时间</returns>
        public static string GetDateTime(DataRow dataRow, string name)
        {
            string returnValue = string.Empty;
            if (!dataRow.IsNull(name))
            {
                DateTime DateTime = DateTime.Parse(dataRow[name].ToString());
                returnValue = DateTime.ToString(BaseSystemInfo.DateFormat);
            }
            return returnValue;
        }
        #endregion

        #region public static string GetDayOfWeek(string dayOfWeek) 获得数字星期几
        /// <summary>
        /// 获得数字星期几
        /// </summary>
        /// <param name="dayOfWeek">英文星期</param>
        /// <returns>数字星期几</returns>
        public static string GetDayOfWeek(string dayOfWeek)
        {
            return GetDayOfWeek(dayOfWeek, false);
        }
        #endregion

        #region public static int GetDayOfWeek(string dayOfWeek, bool chinese) 获得星期几
        /// <summary>
        /// 获得星期几
        /// </summary>
        /// <param name="dayOfWeek">英文星期</param>
        /// <param name="chinese">需要中文</param>
        /// <returns>数字星期几</returns>
        public static string GetDayOfWeek(string dayOfWeek, bool chinese)
        {
            string week = "0";
            switch (dayOfWeek)
            {
                case "Sunday":
                    week = "0";
                    break;
                case "Monday":
                    week = "1";
                    break;
                case "Tuesday":
                    week = "2";
                    break;
                case "Wednesday":
                    week = "3";
                    break;
                case "Thursday":
                    week = "4";
                    break;
                case "Friday":
                    week = "5";
                    break;
                case "Saturday":
                    week = "6";
                    break;
                default:
                    break;
            }
            if (chinese)
            {
                switch (week)
                {
                    case "0":
                        week = "星期日";
                        break;
                    case "1":
                        week = "星期一";
                        break;
                    case "2":
                        week = "星期二";
                        break;
                    case "3":
                        week = "星期三";
                        break;
                    case "4":
                        week = "星期四";
                        break;
                    case "5":
                        week = "星期五";
                        break;
                    case "6":
                        week = "星期六";
                        break;
                    default:
                        break;
                }
            }
            return week;
        }
        #endregion

        #region public static string FieldToList(DataTable dataTable) 表格字段转换为字符串列表
        /// <summary>
        /// 表格字段转换为字符串列表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>字段值字符串</returns>
        public static string FieldToList(DataTable dataTable)
        {
            return FieldToList(dataTable, FieldId);
        }
        #endregion

        #region public static string FieldToList(DataTable dataTable, string name) 表格字段转换为字符串列表
        /// <summary>
        /// 表格字段转换为字符串列表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="name">列名</param>
        /// <returns>字段值字符串</returns>
        public static string FieldToList(DataTable dataTable, string name)
        {
            int rowCount = 0;
            string returnValue = "'";
            foreach (DataRow dataRow in dataTable.Rows)
            {
                rowCount++;
                returnValue += dataRow[name].ToString() + "', '";
            }
            if (rowCount == 0)
            {
                returnValue = "''";
            }
            else
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 3);
            }
            return returnValue;
        }
        #endregion

        public static string ArrayToList(string[] ids)
        {
            return ArrayToList(ids, string.Empty);
        }

        public static string ArrayToList(string[] ids, string separativeSign)
        {
            int rowCount = 0;
            string returnValue = string.Empty;
            foreach (string id in ids)
            {
                rowCount++;
                returnValue += separativeSign + id + separativeSign + ",";
            }
            if (rowCount == 0)
            {
                returnValue = "";
            }
            else
            {
                returnValue = returnValue.TrimEnd(',');
            }
            return returnValue;
        }

        #region public static string[] FieldToArray(DataTable dataTable, string name) 表格字段转换为字符串数组
        /// <summary>
        /// 表格字段转换为字符串数组
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="name">列名</param>
        /// <returns>字段值数组</returns>
        public static string[] FieldToArray(DataTable dataTable, string name)
        {
            string[] returnValue = new string[0];
            int rowCount = 0;
            string stringList = string.Empty;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(dataRow[name].ToString()))
                {
                    rowCount++;
                    stringList += dataRow[name].ToString() + ",";
                }
            }
            if (rowCount > 0)
            {
                stringList = stringList.TrimEnd(',');
                returnValue = stringList.Split(',');
            }
            return returnValue;
        }
        #endregion

        #region public static string ObjectsToList(object ids) 字段值数组转换为字符串列表
        /// <summary>
        /// 字段值数组转换为字符串列表
        /// </summary>
        /// <param name="ids">字段值</param>
        /// <returns>字段值字符串</returns>
        public static string ObjectsToList(Object[] ids)
        {
            string returnValue = string.Empty;
            string stringList = "'";
            for (int i = 0; i < ids.Length; i++)
            {
                stringList += ids[i] + "', '";
            }
            if (ids.Length == 0)
            {
                returnValue = " NULL ";
            }
            else
            {
                returnValue = stringList.Substring(0, stringList.Length - 3);
            }
            return returnValue;
        }
        #endregion

        #region public static DataTable SetFilter(DataTable dataTable, string fieldName, string fieldValue, bool equals = false) 对数据表进行过滤
        /// <summary>
        /// 对数据表进行过滤
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="equals">相等</param>
        /// <returns>数据权限</returns>
        //public static DataTable SetFilter(DataTable dataTable, string fieldName, string fieldValue, bool equals = false)
        public static DataTable SetFilter(DataTable dataTable, string fieldName, string fieldValue, bool equals)
        {
            foreach (DataRow dataRow in dataTable.Rows)
            {
                // 要求把相等的删除掉
                if (equals)
                {
                    if (string.IsNullOrEmpty(fieldValue))
                    {
                        if (string.IsNullOrEmpty(dataRow[fieldName].ToString()))
                        {
                            dataRow.Delete();
                        }
                    }
                    else
                    {
                        if (dataRow[fieldName].ToString().Equals(fieldValue))
                        {
                            dataRow.Delete();
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(fieldValue))
                    {
                        if (!string.IsNullOrEmpty(dataRow[fieldName].ToString()))
                        {
                            dataRow.Delete();
                        }
                    }
                    else
                    {
                        if (!dataRow[fieldName].ToString().Equals(fieldValue))
                        {
                            dataRow.Delete();
                        }
                    }
                }
            }
            dataTable.AcceptChanges();
            return dataTable;
        }

        //用来模拟默认参数
        public static DataTable SetFilter(DataTable dataTable, string fieldName, string fieldValue)
        {
            bool equals = false;
            return SetFilter(dataTable, fieldName, fieldValue, equals);
        }
        #endregion

        #region public static string GetProperty(DataTable dataTable, string id, string targetField) 读取一个属性
        /// <summary>
        /// 读取一个属性
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="id">主键</param>
        /// <param name="targetField">目标字段</param>
        /// <returns>目标值</returns>
        public static string GetProperty(DataTable dataTable, string id, string targetField)
        {
            return GetProperty(dataTable, FieldId, id, targetField);
        }
        #endregion

        #region public static string GetProperty(DataTable dataTable, string fieldName, string fieldValue, string targetField) 读取一个属性
        /// <summary>
        /// 读取一个属性
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="fieldName">字段</param>
        /// <param name="fieldValue">值</param>
        /// <param name="targetField">目标字段</param>
        /// <returns>目标值</returns>
        public static string GetProperty(DataTable dataTable, string fieldName, string fieldValue, string targetField)
        {
            string returnValue = string.Empty;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow[fieldName].ToString().Equals(fieldValue))
                {
                    returnValue = dataRow[targetField].ToString();
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region public static int SetProperty(DataTable dataTable, string id, string targetField, object targetValue) 设置一个属性
        /// <summary>
        /// 设置一个属性
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="id">主键</param>
        /// <param name="targetField">更新字段</param>
        /// <param name="targetValue">目标值</param>
        /// <returns>影响行数</returns>
        public static int SetProperty(DataTable dataTable, string id, string targetField, object targetValue)
        {
            return SetProperty(dataTable, FieldId, id, targetField, targetValue);
        }
        #endregion

        #region public static int SetProperty(DataTable dataTable, string fieldName, string fieldValue, string targetField, object targetValue) 设置一个属性
        /// <summary>
        /// 设置一个属性
        /// </summary>        
        /// <param name="dataTable">数据表</param>
        /// <param name="fieldName">字段</param>
        /// <param name="fieldValue">值</param>
        /// <param name="targetField">更新字段</param>
        /// <param name="targetValue">目标值</param>
        /// <returns>影响行数</returns>
        public static int SetProperty(DataTable dataTable, string fieldName, string fieldValue, string targetField, object targetValue)
        {
            int returnValue = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow.RowState != DataRowState.Deleted)
                {
                    if (dataRow[fieldName].ToString().Equals(fieldValue))
                    {
                        dataRow[targetField] = targetValue;
                        returnValue++;
                        break;
                    }
                }
            }
            return returnValue;
        }
        #endregion

        #region public static int Delete(DataTable dataTable, string id) 删除记录
        /// <summary>
        /// 删除一条记录
        /// </summary>        
        /// <param name="dataTable">数据表名</param>
        /// <param name="id">主键</param>
        /// <returns>影响行数</returns>
        public static int Delete(DataTable dataTable, string id)
        {
            return Delete(dataTable, FieldId, id);
        }
        #endregion

        #region public static int Delete(DataTable dataTable, string fieldName, string fieldValue) 删除记录
        /// <summary>
        /// 删除一条记录
        /// </summary>        
        /// <param name="dataTable">数据表名</param>
        /// <param name="fieldName">字段</param>
        /// <param name="fieldValue">值</param>
        /// <returns>影响行数</returns>
        public static int Delete(DataTable dataTable, string fieldName, string fieldValue)
        {
            int returnValue = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow[fieldName].ToString().Equals(fieldValue))
                {
                    dataRow.Delete();
                    returnValue++;
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region public static DataRow GetDataRow(DataTable dataTable, string id) 从数据权限读取一行数据
        /// <summary>
        /// 从数据权限读取一行数据
        /// </summary>        
        /// <param name="dataTable">数据表</param>
        /// <param name="id">主键</param>
        /// <returns>数据行</returns>
        public static DataRow GetDataRow(DataTable dataTable, string id)
        {
            return GetDataRow(dataTable, FieldId, id);
        }
        #endregion

        #region public static DataRow GetDataRow(DataTable dataTable, string fieldName, string fieldValue) 从数据权限读取一行数据
        /// <summary>
        /// 从数据权限读取一行数据
        /// </summary>        
        /// <param name="dataTable">数据表</param>
        /// <param name="fieldName">字段</param>
        /// <param name="fieldValue">值</param>
        /// <returns>数据行</returns>
        public static DataRow GetDataRow(DataTable dataTable, string fieldName, string fieldValue)
        {
            DataRow returnValue = null;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow.RowState != DataRowState.Deleted)
                {
                    if (dataRow[fieldName].ToString().Equals(fieldValue))
                    {
                        returnValue = dataRow;
                        break;
                    }
                }
            }
            return returnValue;
        }
        #endregion

        #region private static int SetClassValue(object sourceObject, string field, object targetObject) 设置对象的属性
        /// <summary>
        /// 设置对象的属性
        /// </summary>
        /// <param name="sourceObject">目标对象</param>
        /// <param name="field">属性名称</param>
        /// <param name="targetValue">目标值</param>
        /// <returns>影响的属性个数</returns>
        private static int SetClassValue(object sourceObject, string field, object targetValue)
        {
            int returnValue = 0;
            Type currentType = sourceObject.GetType();
            FieldInfo[] fieldInfo = currentType.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo currentFieldInfo;
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                if (field.Equals(fieldInfo[i].Name))
                {
                    currentFieldInfo = currentType.GetField(field);
                    currentFieldInfo.SetValue(sourceObject, targetValue);
                    // FieldInfo[i].SetValue(TargetObject, value);
                    returnValue++;
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region public static object CopyObjectValue(object sourceObject, object targetObject) 复制类对象的对应的值
        /// <summary>
        /// 复制类对象的对应的值
        /// </summary>
        /// <param name="sourceObject">当前对象</param>
        /// <param name="targetObject">目标对象</param>
        /// <returns>对象</returns>
        public static object CopyObjectValue(object sourceObject, object targetObject)
        {
            int returnValue = 0;
            string name = string.Empty;
            Type type = sourceObject.GetType();
            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo currentFieldInfo;
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                name = fieldInfo[i].Name;
                currentFieldInfo = fieldInfo[i];
                returnValue = SetClassValue(targetObject, name, currentFieldInfo.GetValue(sourceObject));
            }
            return targetObject;
        }
        #endregion

        #region public static object CopyObjectProperties(object sourceObject, object targetObject)
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="sourceObject">类</param>
        /// <param name="targetObject">目标类</param>
        /// <returns>类</returns>
        public static object CopyObjectProperties(object sourceObject, object targetObject)
        {
            Type typeSource = sourceObject.GetType();
            Type typeTarget = targetObject.GetType();
            PropertyInfo[] PropertyInfoSource = typeSource.GetProperties();
            PropertyInfo[] PropertyInfoTarget = typeTarget.GetProperties();
            for (int i = 0; i < PropertyInfoTarget.Length; i++)
            {
                for (int j = 0; j < PropertyInfoSource.Length; j++)
                {
                    if (PropertyInfoTarget[i].Name.Equals(PropertyInfoSource[j].Name))
                    {
                        if (PropertyInfoTarget[i].CanWrite)
                        {
                            object pValue = PropertyInfoSource[j].GetValue(sourceObject, null);
                            PropertyInfoTarget[i].SetValue(targetObject, pValue, null);
                        }
                        break;
                    }
                }
            }
            return targetObject;
        }
        #endregion

        #region public static string GetSearchString(string searchValue) 获取查询字符串
        /// <summary>
        /// 获取查询字符串
        /// </summary>
        /// <param name="search">查询字符</param>
        /// <returns>字符串</returns>
        public static string GetSearchString(string searchValue)
        {
            searchValue = searchValue.Trim();
            searchValue = SqlSafe(searchValue);
            if (searchValue.Length > 0)
            {
                searchValue = searchValue.Replace('[', '_');
                searchValue = searchValue.Replace(']', '_');
            }
            if (searchValue == "%")
            {
                searchValue = "[%]";
            }
            if ((searchValue.Length > 0) && (searchValue.IndexOf('%') < 0) && (searchValue.IndexOf('_') < 0))
            {
                searchValue = "%" + searchValue + "%";
            }
            return searchValue;
        }
        #endregion


        /// <summary>
        /// 判断是否包含的方法
        /// </summary>
        /// <param name="ids">数组</param>
        /// <param name="targetString">目标值</param>
        /// <returns>包含</returns>
        public static bool Exists(string[] ids, string targetString)
        {
            bool returnValue = false;
            if (ids != null && !string.IsNullOrEmpty(targetString))
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i].Equals(targetString))
                    {
                        returnValue = true;
                        break;
                    }
                }
            }
            return returnValue;
        }

        public static string[] Concat(string[] ids, string id)
        {
            return Concat(ids, new string[] { id });
        }

        
        /// <summary>
        /// 合并数组
        /// </summary>
        /// <param name="ids">数组</param>
        /// <returns>数组</returns>
        public static string[] Concat(params string[][] ids)
        {
            // 进行合并
            Hashtable hashValues = new Hashtable();
            if (ids != null)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i] != null)
                    {
                        for (int j = 0; j < ids[i].Length; j++)
                        {
                            if (ids[i][j] != null)
                            {
                                if (!hashValues.ContainsKey(ids[i][j]))
                                {
                                    hashValues.Add(ids[i][j], ids[i][j]);
                                }
                            }
                        }
                    }
                }
            }
            // 返回合并结果
            string[] returnValues = new string[hashValues.Count];
            IDictionaryEnumerator enumerator = hashValues.GetEnumerator();
            int key = 0;
            while (enumerator.MoveNext())
            {
                returnValues[key] = (string)(enumerator.Key.ToString());
                key++;
            }
            return returnValues;
        }

        /// <summary>
        /// 从目标数组中去除某个值
        /// </summary>
        /// <param name="ids">数组</param>
        /// <param name="id">目标值</param>
        /// <returns>数组</returns>
        public static string[] Remove(string[] ids, string id)
        {
            // 进行合并
            Hashtable hashValues = new Hashtable();
            if (ids != null)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i] != null && (!ids[i].Equals(id)))
                    {
                        if (!hashValues.ContainsKey(ids[i]))
                        {
                            hashValues.Add(ids[i], ids[i]);
                        }
                    }
                }
            }
            // 返回合并结果
            string[] returnValues = new string[hashValues.Count];
            IDictionaryEnumerator enumerator = hashValues.GetEnumerator();
            int key = 0;
            while (enumerator.MoveNext())
            {
                returnValues[key] = (string)(enumerator.Key.ToString());
                key++;
            }
            return returnValues;
        }

        /// 
        ///  类型转换
        ///

        public static string ConvertToString(Object targetValue)
        {
            return targetValue != DBNull.Value ? Convert.ToString(targetValue) : null;
        }

        public static int? ConvertToInt(Object targetValue)
        {
            int? returnValue = null; 
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            int result = 0;
            int.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static Int32? ConvertToInt32(Object targetValue)
        {
            Int32? returnValue = null; 
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            Int32 result = 0;
            Int32.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static Int64? ConvertToInt64(Object targetValue)
        {
            Int64? returnValue = null; 
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            Int64 result = 0;
            Int64.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static long? ConvertToLong(Object targetValue)
        {
            long? returnValue = null; 
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            long result = 0;
            long.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }        

        public static Boolean ConvertIntToBoolean(Object targetValue)
        {
            return targetValue != DBNull.Value ? (targetValue.ToString().Equals("1") || targetValue.ToString().Equals(true.ToString())) : false;
        }

        public static Boolean ConvertToBoolean(Object targetValue)
        {
            return targetValue != DBNull.Value ? (targetValue.ToString().Equals(true.ToString())) : false;
        }

        public static Double? ConvertToDouble(Object targetValue)
        {
            Double? returnValue = null;
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            Double result = 0;
            Double.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static decimal? ConvertToDecimal(Object targetValue)
        {
            decimal? returnValue = null;
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            decimal result = 0;
            decimal.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static DateTime? ConvertToDateTime(Object targetValue)
        {
            DateTime? returnValue = null;
            if (targetValue != DBNull.Value)
            {
                returnValue = Convert.ToDateTime(targetValue.ToString());
            }
            return returnValue;
        }

        public static string ConvertToDateToString(Object targetValue)
        {
            string returnValue = string.Empty;
            returnValue = targetValue != DBNull.Value ? DateTime.Parse(targetValue.ToString()).ToString(BaseSystemInfo.DateFormat) : null;
            return returnValue;
        }

        public static byte[] ConvertToByte(Object targetValue)
        {
            return targetValue != DBNull.Value ? (byte[])targetValue : null;
        }

        //
        // 调试信息
        //

        // 写入开始调试信息
        public static int StartDebug(MethodBase methodBase)
        {
            // 输出访问日志
            // 写入调试信息
            #if (DEBUG)
                Console.WriteLine(DateTime.Now.ToString(BaseSystemInfo.DateTimeFormat) + " :Begin: " + methodBase.ReflectedType.Name + "." + methodBase.Name);
                Trace.WriteLine(DateTime.Now.ToString(BaseSystemInfo.DateTimeFormat) + " :Begin: " + methodBase.ReflectedType.Name + "." + methodBase.Name);
            #endif

            return Environment.TickCount;
        }

        public static int StartDebug(BaseUserInfo userInfo, MethodBase methodBase)
        {
            // 写入访问日志
            // userInfo.OperationId = MethodBase.Name;
            // userInfo.Description = MethodBase.ReflectedType.Name;
            // 输出访问日志
            #if (DEBUG)
                if (userInfo != null)
                {
                    Console.WriteLine("User: " + userInfo.RealName + " IP: " + userInfo.IPAddress);
                    Console.WriteLine(DateTime.Now.ToString(BaseSystemInfo.DateTimeFormat) + " :Begin: " + methodBase.ReflectedType.Name + "." + methodBase.Name);
                    Trace.WriteLine(DateTime.Now.ToString(BaseSystemInfo.DateTimeFormat) + " :Begin: " + methodBase.ReflectedType.Name + "." + methodBase.Name);
                }
            #endif

            return Environment.TickCount;
        }

        // 写入结束调试信息
        public static int EndDebug(MethodBase methodBase, int milliStart, ConsoleColor consoleColor)
        {
            int milliEnd = Environment.TickCount;
            
            #if (DEBUG)
            Console.Write(DateTime.Now.ToString("MM-dd HH:mm:ss") + " : " 
                    + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() 
                    + " : ");
                Console.ForegroundColor = consoleColor;
                Console.Write(methodBase.ReflectedType.Name + "." + methodBase.Name);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(string.Empty);
                Console.WriteLine(string.Empty);

                Trace.WriteLine(DateTime.Now.ToString(BaseSystemInfo.DateTimeFormat) 
                    + " : " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() 
                    + " : " + methodBase.ReflectedType.Name + "." + methodBase.Name);
            #endif

            return milliEnd - milliStart;
        }

        // 写入结束调试信息
        public static int EndDebug(MethodBase methodBase, int milliStart)
        {
            return EndDebug(methodBase, milliStart, ConsoleColor.White);
        }

        // 写入调试信息
        public static void WriteDebug(BaseUserInfo userInfo, MethodBase methodBase)
        {
            #if (DEBUG)
                Console.WriteLine(DateTime.Now.ToString(BaseSystemInfo.DateTimeFormat) + " " + userInfo.IPAddress + methodBase.ReflectedType.Name + "." + methodBase.Name);
                Trace.WriteLine(DateTime.Now.ToString(BaseSystemInfo.DateTimeFormat) + " " + userInfo.IPAddress + methodBase.ReflectedType.Name + "." + methodBase.Name);
            #endif
        }

        #region public static string GetFriendlyFileSize(double fileSize) 有善的文件大小现实方式
        /// <summary>
        /// 有善的文件大小现实方式
        /// </summary>
        /// <param name="fileSize">文件大小</param>
        /// <returns>现实方式</returns>
        public static string GetFriendlyFileSize(double fileSize)
        {
            string returnValue = string.Empty;
            if (fileSize < 1024)
            {
                returnValue = fileSize.ToString("F1") + "Byte";
            }
            else
            {
                fileSize = fileSize / 1024;
                if (fileSize < 1024)
                {
                    returnValue = fileSize.ToString("F1") + "KB";
                }
                else
                {
                    fileSize = fileSize / 1024;
                    if (fileSize < 1024)
                    {
                        returnValue = fileSize.ToString("F1") + "M";
                    }
                    else
                    {
                        fileSize = fileSize / 1024;
                        returnValue = fileSize.ToString("F1") + "GB";
                    }
                }
            }
            return returnValue;
        }
        #endregion

        #region public static int GetLanguageResource(object targetObject) 从当前指定的语言包读取信息
        /// <summary>
        /// 从当前指定的语言包读取信息
        /// </summary>
        /// <returns>设置多语言的属性个数</returns>
        public static int GetLanguageResource(object targetObject)
        {
            int returnValue = 0;
            string name = string.Empty;
            Type type = targetObject.GetType();
            // Type type = typeof(TargetObject);
            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo currentFieldInfo;
            string messages = string.Empty;
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                name = fieldInfo[i].Name;
                currentFieldInfo = fieldInfo[i];
                messages = ResourceManagerWrapper.Instance.Get(name);
                if (messages.Length > 0)
                {
                    currentFieldInfo.SetValue(targetObject, messages);
                    returnValue++;
                }
            }
            return returnValue;
        }
        #endregion
    }
}