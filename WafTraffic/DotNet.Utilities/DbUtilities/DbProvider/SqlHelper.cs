//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DotNet.Utilities
{
    /// <summary>
    /// SqlHelper
	/// 有关数据库连接的方法。
	/// 
	/// 修改纪录
    /// 
    ///		2008.08.26 版本：1.2 JiRiGaLa 修改Open时的错误反馈。
    ///		2008.06.01 版本：1.1 JiRiGaLa 数据库连接获得方式进行改进，构造函数获得调通。
    ///		2008.05.07 版本：1.0 JiRiGaLa 创建。
    /// 
    /// 版本：1.2
	/// 
	/// <author>
    ///		<name>JiRiGaLa</name>
    ///		<date>2008.08.26</date>
	/// </author> 
	/// </summary>
    public class SqlHelper : BaseDbHelper, IDbHelper, IDbHelperExpand
	{
        public override DbProviderFactory GetInstance()
        {
            return SqlClientFactory.Instance;
        }

        #region public override CurrentDbType CurrentDbType 当前数据库类型
        /// <summary>
        /// 当前数据库类型
        /// </summary>
        public override CurrentDbType CurrentDbType
        {
            get
            {
                return CurrentDbType.SqlServer;
            }
        }
        #endregion 

        #region public SqlHelper() 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public SqlHelper()
		{
            FileName = "SqlHelper.txt"; // sql查询句日志
		}
		#endregion

        #region public SqlHelper(string connectionString) 构造函数,设置数据库连接
        /// <summary>
        /// 构造函数,设置数据库连接
        /// </summary>
        /// <param name="connectionString">数据连接</param>
        public SqlHelper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        #endregion

        #region public string GetDBNow() 获得数据库当前日期时间
        /// <summary>
        /// 获得数据库当前日期时间
        /// </summary>
        /// <returns>日期时间</returns>
        public string GetDBNow()
        {
            return " Getdate() ";
        }
        #endregion

        #region public string GetDBDateTime() 获得数据库日期时间
        /// <summary>
        /// 获得数据库日期时间
        /// </summary>
        /// <returns>日期时间</returns>
        public string GetDBDateTime()
        {
            string commandText = " SELECT " + this.GetDBNow();
            this.Open();
            string dateTime = this.ExecuteScalar(CommandType.Text, commandText, new DbParameter[0]).ToString();
            this.Close();
            return dateTime;
        }
        #endregion

        #region string PlusSign(params string[] values) 获得Sql字符串相加符号
        /// <summary>
        ///  获得Sql字符串相加符号
        /// </summary>
        /// <param name="values">参数值</param>
        /// <returns>字符加</returns>
        public string PlusSign(params string[] values)
        {
            string returnValue = string.Empty;
            for (int i = 0; i < values.Length; i++)
            {
                returnValue += values[i] + " + ";
            }
            if (!String.IsNullOrEmpty(returnValue))
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 3);
            }
            else
            {
                returnValue = " + ";
            }
            return returnValue;
        }
        #endregion

        #region public string GetParameter(string parameter) 获得参数Sql表达式
        /// <summary>
        /// 获得参数Sql表达式
        /// </summary>
        /// <param name="parameter">参数名称</param>
        /// <returns>字符串</returns>
        public string GetParameter(string parameter)
        {
            return " @" + parameter + " ";
        }
        #endregion

        #region public IDbDataParameter MakeInParam(string targetFiled, object targetValue) 获取参数
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetValue">值</param>
        /// <returns>参数</returns>
        public IDbDataParameter MakeInParam(string targetFiled, object targetValue)
        {
            return new SqlParameter("@" + targetFiled, targetValue);
        }
        #endregion

        #region public IDbDataParameter MakeParameter(string targetFiled, object targetValue) 获取参数
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetValue">值</param>
        /// <returns>参数集</returns>
        public IDbDataParameter MakeParameter(string targetFiled, object targetValue)
        {
            IDbDataParameter dbParameter = null;
            if (targetFiled != null && targetValue != null)
            {
                dbParameter = this.MakeInParam(targetFiled, targetValue);
            }
            return dbParameter;
        }
        #endregion

        #region public IDbDataParameter[] MakeParameters(string[] targetFileds, Object[] targetValues) 获取参数
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetValue">值</param>
        /// <returns>参数集</returns>
        public IDbDataParameter[] MakeParameters(string[] targetFileds, Object[] targetValues)
        {
            // 这里需要用泛型列表，因为有不合法的数组的时候
            List<IDbDataParameter> dbParameters = new List<IDbDataParameter>();
            if (targetFileds != null && targetValues != null)
            {
                for (int i = 0; i < targetFileds.Length; i++)
                {
                    if (targetFileds[i] != null && targetValues[i] != null && (!(targetValues[i] is Array)))
                    {
                        dbParameters.Add(this.MakeInParam(targetFileds[i], targetValues[i]));
                    }
                }
            }
            return dbParameters.ToArray();
        }
        #endregion

        #region public IDbDataParameter MakeOutParam(string paramName, DbType dbType, int size) 获取输出参数
        /// <summary>
        /// 获取输出参数
        /// </summary>
        /// <param name="paramName">参数</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">长度</param>
        /// <returns></returns>
        public IDbDataParameter MakeOutParam(string paramName, DbType dbType, int size)
        {
            return MakeParam(paramName, null, dbType, size, ParameterDirection.Output);
        }
        #endregion 

        #region public IDbDataParameter MakeInParam(string paramName, DbType dbType, int Size, object value) 获取输入参数
        /// <summary>
        /// 获取输入参数
        /// </summary>
        /// <param name="paramName">参数</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="Size">长度</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public IDbDataParameter MakeInParam(string paramName, DbType dbType, int Size, object value)
        {
            return MakeParam(paramName, value, dbType, Size, ParameterDirection.Input);
        }
        #endregion 

        #region public IDbDataParameter MakeParam(string parameterName, object parameterValue, DbType dbType, Int32 parameterSize, ParameterDirection parameterDirection) 获取参数
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="parameterValue">值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="parameterSize">长度</param>
        /// <param name="parameterDirection">参数类型</param>
        /// <returns>参数</returns>
        public IDbDataParameter MakeParam(string parameterName, object parameterValue, DbType dbType, Int32 parameterSize, ParameterDirection parameterDirection)
        {
            SqlParameter parameter;

            if (parameterSize > 0)
            {
                parameter = new SqlParameter(parameterName, (SqlDbType)dbType, parameterSize);
            }
            else
            {
                parameter = new SqlParameter(parameterName, (SqlDbType)dbType);
            }

            parameter.Direction = parameterDirection;
            if (!(parameterDirection == ParameterDirection.Output && parameterValue == null))
            {
                parameter.Value = parameterValue;
            }

            return parameter;
        }
        #endregion 

        #region public void SqlBulkCopyData(DataTable dataTable) 利用Net SqlBulkCopy 批量导入数据库,速度超快
        /// <summary>
        /// 利用Net SqlBulkCopy 批量导入数据库,速度超快
        /// </summary>
        /// <param name="dataTable">源内存数据表</param>
        public void SqlBulkCopyData(DataTable dataTable)
        {
            // SQL 数据连接
            SqlConnection conn = null;
            // 打开数据库
            this.Open();

            // 获取连接
            conn = (SqlConnection)GetDbConnection();

            using (SqlTransaction tran = conn.BeginTransaction())
            {
                // 批量保存数据，只能用于Sql
                SqlBulkCopy sqlbulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran);
                // 设置源表名称
                sqlbulkCopy.DestinationTableName = dataTable.TableName;
                // 设置超时限制
                sqlbulkCopy.BulkCopyTimeout = 1000;

                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    sqlbulkCopy.ColumnMappings.Add(dtColumn.ColumnName, dtColumn.ColumnName);
                }
                try
                {
                    // 写入
                    sqlbulkCopy.WriteToServer(dataTable);
                    // 提交事务
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    sqlbulkCopy.Close();
                }
                finally
                {
                    sqlbulkCopy.Close();
                    this.Close();
                }

            }
        }
        #endregion
    }
}