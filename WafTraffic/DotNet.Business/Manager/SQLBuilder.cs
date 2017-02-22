namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Data;

    public class SQLBuilder
    {
        private string CommandText;
        private IDbHelper DbHelper;
        private bool Identity;
        private string InsertField;
        private string InsertValue;
        private Dictionary<string, object> parameters;
        private string PrimaryKey;
        private bool ReturnId;
        private string SelectSql;
        private DbOperation sqlOperation;
        private string TableName;
        private string UpdateSql;
        private string WhereSql;
        public string InsertReturnId;

        private SQLBuilder()
        {
            this.ReturnId = true;
            this.PrimaryKey = "Id";
            this.sqlOperation = DbOperation.Update;
            this.CommandText = string.Empty;
            this.TableName = string.Empty;
            this.InsertValue = string.Empty;
            this.InsertField = string.Empty;
            this.UpdateSql = string.Empty;
            this.SelectSql = string.Empty;
            this.WhereSql = string.Empty;
            this.parameters = new Dictionary<string, object>();
            this.parameters = new Dictionary<string, object>();
        }

        public SQLBuilder(IDbHelper dbHelper) : this()
        {
            this.DbHelper = dbHelper;
        }

        public SQLBuilder(IDbHelper dbHelper, bool identity) : this(dbHelper)
        {
            this.Identity = identity;
        }

        public SQLBuilder(IDbHelper dbHelper, bool identity, bool returnId) : this(dbHelper)
        {
            this.Identity = identity;
            this.ReturnId = returnId;
        }

        private void AddParameter(string targetFiled, object targetValue)
        {
            this.parameters.Add(targetFiled, targetValue);
        }

        private void Begin(string tableName, DbOperation dbOperation)
        {
            this.PrepareCommand();
            this.TableName = tableName;
            this.sqlOperation = dbOperation;
        }

        public void BeginDelete(string tableName)
        {
            this.Begin(tableName, DbOperation.Delete);
        }

        public void BeginInsert(string tableName)
        {
            this.Begin(tableName, DbOperation.Insert);
        }

        public void BeginInsert(string tableName, bool identity)
        {
            this.Identity = identity;
            this.Begin(tableName, DbOperation.Insert);
        }

        public void BeginInsert(string tableName, string primaryKey)
        {
            this.PrimaryKey = primaryKey;
            this.Begin(tableName, DbOperation.Insert);
        }

        public void BeginSelect(string tableName)
        {
            this.Begin(tableName, DbOperation.Select);
        }

        public void BeginUpdate(string tableName)
        {
            this.Begin(tableName, DbOperation.Update);
        }

        public int EndDelete()
        {
            return this.Execute();
        }

        public int EndInsert()
        {
            return this.Execute();
        }
        //public string EndInsert()
        //{
        //    if (this.ReturnId)
        //    {
        //        return this.Execute().ToString();
        //    }
        //    else
        //    {
        //        this.Execute();
        //        return insertReturnId;
        //     }
        //}

        public int EndSelect()
        {
            return this.Execute();
        }

        public int EndUpdate()
        {
            return this.Execute();
        }

        private int Execute()
        {
            if (this.sqlOperation == DbOperation.Select)
            {
                this.CommandText = " SELECT * FROM " + this.TableName + this.WhereSql;
            }
            else if (this.sqlOperation == DbOperation.Insert)
            {
                this.InsertField = this.InsertField.Substring(0, this.InsertField.Length - 2);
                this.InsertValue = this.InsertValue.Substring(0, this.InsertValue.Length - 2);
                this.CommandText = " INSERT INTO " + this.TableName + "(" + this.InsertField + ") VALUES(" + this.InsertValue + ") ";
                if ((this.Identity && (this.DbHelper.CurrentDbType == CurrentDbType.SqlServer)) && this.ReturnId)
                {
                    this.CommandText = this.CommandText + " SELECT SCOPE_IDENTITY(); ";
                }
            }
            else if (this.sqlOperation == DbOperation.Update)
            {
                this.UpdateSql = this.UpdateSql.Substring(0, this.UpdateSql.Length - 2);
                this.CommandText = " UPDATE " + this.TableName + " SET " + this.UpdateSql + this.WhereSql;
            }
            else if (this.sqlOperation == DbOperation.Delete)
            {
                this.CommandText = " DELETE FROM " + this.TableName + this.WhereSql;
            }

            List<IDbDataParameter> list = new List<IDbDataParameter>();
            foreach (string str in this.parameters.Keys)
            {
                list.Add(this.DbHelper.MakeParameter(str, this.parameters[str]));
            }

            int num = 0;
            if ((this.Identity && this.ReturnId && (this.sqlOperation == DbOperation.Insert)) && (this.DbHelper.CurrentDbType == CurrentDbType.SqlServer))
            {
                num = int.Parse(this.DbHelper.ExecuteScalar(this.CommandText, list.ToArray()).ToString());
                InsertReturnId = num.ToString(); 
            }
            else
            {
                num = this.DbHelper.ExecuteNonQuery(this.CommandText, list.ToArray());
            }

            if ((this.Identity && (this.DbHelper.CurrentDbType == CurrentDbType.Access)) && this.ReturnId)
            {
                this.CommandText = " SELECT @@identity AS ID FROM " + this.TableName + "; ";
                num = int.Parse(this.DbHelper.ExecuteScalar(this.CommandText).ToString());
            }

            bool autoOpenClose = this.DbHelper.AutoOpenClose;
            this.parameters.Clear();
            this.DbHelper.GetDbCommand().Parameters.Clear();
            return num;
        }

        private void PrepareCommand()
        {
            this.sqlOperation = DbOperation.Update;
            this.CommandText = string.Empty;
            this.TableName = string.Empty;
            this.InsertValue = string.Empty;
            this.InsertField = string.Empty;
            this.UpdateSql = string.Empty;
            this.WhereSql = string.Empty;
            if (!this.DbHelper.AutoOpenClose)
            {
                this.DbHelper.GetDbCommand().Parameters.Clear();
            }
        }

        public void SetDBNow(string targetFiled)
        {
            if (this.sqlOperation == DbOperation.Insert)
            {
                this.InsertField = this.InsertField + targetFiled + ", ";
                this.InsertValue = this.InsertValue + this.DbHelper.GetDBNow() + ", ";
            }
            if (this.sqlOperation == DbOperation.Update)
            {
                string updateSql = this.UpdateSql;
                this.UpdateSql = updateSql + targetFiled + " = " + this.DbHelper.GetDBNow() + ", ";
            }
        }

        public void SetFormula(string targetFiled, string formula)
        {
            string relation = " = ";
            this.SetFormula(targetFiled, formula, relation);
        }

        public void SetFormula(string targetFiled, string formula, string relation)
        {
            if (this.sqlOperation == DbOperation.Insert)
            {
                this.InsertField = this.InsertField + targetFiled + ", ";
                this.InsertValue = this.InsertValue + formula + ", ";
            }
            if (this.sqlOperation == DbOperation.Update)
            {
                string updateSql = this.UpdateSql;
                this.UpdateSql = updateSql + targetFiled + relation + formula + ", ";
            }
        }

        public void SetNull(string targetFiled)
        {
            this.SetValue(targetFiled, null, null);
        }

        //public void SetValue(string targetFiled, object targetValue, string targetFiledName = null) //C# 4.0 才支持缺省参数
        public void SetValue(string targetFiled, object targetValue, string targetFiledName) 
        {
            if (targetFiledName == null)
            {
                targetFiledName = targetFiled;
            }
            switch (this.sqlOperation)
            {
                case DbOperation.Insert:
                    if (this.DbHelper.CurrentDbType != CurrentDbType.SqlServer)
                    {
                        this.InsertField = this.InsertField + targetFiled + ", ";
                        break;
                    }
                    if (!this.Identity || (targetFiled != this.PrimaryKey))
                    {
                        this.InsertField = this.InsertField + targetFiled + ", ";
                    }
                    break;

                case DbOperation.Update:
                    if (targetValue != null)
                    {
                        if (targetValue.ToString().Length > 0)
                        {
                            string updateSql = this.UpdateSql;
                            this.UpdateSql = updateSql + targetFiled + " = " + this.DbHelper.GetParameter(targetFiledName) + ", ";
                            this.AddParameter(targetFiledName, targetValue);
                            return;
                        }
                        this.UpdateSql = this.UpdateSql + targetFiled + " = '', ";
                        return;
                    }
                    this.UpdateSql = this.UpdateSql + targetFiled + " = Null, ";
                    return;

                default:
                    return;
            }
            if (targetValue == null)
            {
                if (this.DbHelper.CurrentDbType == CurrentDbType.SqlServer)
                {
                    if (!this.Identity || (targetFiled != this.PrimaryKey))
                    {
                        this.InsertValue = this.InsertValue + " Null, ";
                    }
                }
                else
                {
                    this.InsertValue = this.InsertValue + " Null, ";
                }
            }
            else if ((!this.Identity || (targetFiled != this.PrimaryKey)) || (this.DbHelper.CurrentDbType != CurrentDbType.SqlServer))
            {
                this.InsertValue = this.InsertValue + this.DbHelper.GetParameter(targetFiledName) + ", ";
                this.AddParameter(targetFiledName, targetValue);
            }
        }

        public void SetValue(string targetFiled, object targetValue) 
        {
            SetValue(targetFiled,targetValue, null);
        }

        public string SetWhere(string[] targetFileds, object[] targetValues)
        {
            string relation = " AND ";
            return this.SetWhere(targetFileds, targetValues, relation);
        }

        public string SetWhere(string targetFiled, object[] targetValues)
        {
            if (this.WhereSql.Length == 0)
            {
                this.WhereSql = " WHERE ";
            }
            string whereSql = this.WhereSql;
            this.WhereSql = whereSql + targetFiled + " IN (" + BaseBusinessLogic.ObjectsToList(targetValues) + ")";
            return this.WhereSql;
        }

        public string SetWhere(string targetFiled, object targetValue)
        {
            string relation = " AND ";
            return this.SetWhere(targetFiled, targetValue, null, relation);
        }

        //public string SetWhere(string[] targetFileds, object[] targetValues, string relation = " AND ") //C# 4.0 才支持缺省参数
        public string SetWhere(string[] targetFileds, object[] targetValues, string relation)
        {
            for (int i = 0; i < targetFileds.Length; i++)
            {
                this.SetWhere(targetFileds[i], targetValues[i], targetFileds[i], relation);
            }
            return this.WhereSql;
        }

        //public string SetWhere(string targetFiled, object targetValue, string targetFiledName = null, string relation = " AND ")//C# 4.0 才支持缺省参数
        public string SetWhere(string targetFiled, object targetValue, string targetFiledName, string relation)
        {
            if (string.IsNullOrEmpty(targetFiledName))
            {
                targetFiledName = targetFiled;
            }
            if (this.WhereSql.Length == 0)
            {
                this.WhereSql = " WHERE ";
            }
            else
            {
                this.WhereSql = this.WhereSql + relation;
            }
            if ((targetValue == null) || ((targetValue is string) && string.IsNullOrEmpty((string) targetValue)))
            {
                this.WhereSql = this.WhereSql + targetFiled + " IS NULL ";
            }
            else
            {
                this.WhereSql = this.WhereSql + targetFiled + " = " + this.DbHelper.GetParameter(targetFiledName);
                this.AddParameter(targetFiledName, targetValue);
            }
            return this.WhereSql;
        }

        /// <summary>
        /// 设置主键语句
        /// </summary>
        /// <param name="keyFiled"></param>
        /// <param name="keyValue"></param>
        public void SetKeyValue(string keyFiled, object keyValue)
        {
            if (!this.Identity)
            {
                this.SetValue(keyFiled, keyValue, null);
            }
            else if (!this.ReturnId && ((this.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (this.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (this.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    this.SetFormula(BaseLogEntity.FieldId, "SEQ_" + this.TableName.ToUpper() + ".NEXTVAL ");
                }
                if (this.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    this.SetFormula(BaseLogEntity.FieldId, "NEXT VALUE FOR SEQ_" + this.TableName.ToUpper());
                }
            }
            else if (this.Identity && ((this.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (this.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                InsertReturnId = new BaseSequenceManager(this.DbHelper, this.Identity).GetSequence(this.TableName);
                this.SetValue(BaseLogEntity.FieldId, InsertReturnId);
            }
        }
    }
}

