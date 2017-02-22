namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseOrganizeManager : BaseManager, IBaseManager
    {
        private DataTable DTOrganize;
        private string head;
        private DataTable organizeTable;

        public BaseOrganizeManager()
        {
            this.head = "|";
            this.organizeTable = new DataTable(BaseOrganizeEntity.TableName);
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseOrganizeEntity.TableName;
        }

        public BaseOrganizeManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseOrganizeManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseOrganizeManager(string tableName)
        {
            this.head = "|";
            this.organizeTable = new DataTable(BaseOrganizeEntity.TableName);
            base.CurrentTableName = tableName;
        }

        public BaseOrganizeManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseOrganizeManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseOrganizeEntity baseOrganizeEntity)
        {
            return this.AddEntity(baseOrganizeEntity);
        }

        public string Add(BaseOrganizeEntity organizeEntity, out string statusCode)
        {
            string str = string.Empty;
            string[] names = new string[] { BaseOrganizeEntity.FieldParentId, BaseOrganizeEntity.FieldFullName, BaseOrganizeEntity.FieldDeletionStateCode };
            object[] values = new object[] { organizeEntity.ParentId, organizeEntity.FullName, 0 };
            if (this.Exists(names, values))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return str;
            }
            names = new string[] { BaseOrganizeEntity.FieldCode, BaseOrganizeEntity.FieldDeletionStateCode };
            values = new object[] { organizeEntity.Code, 0 };
            if ((organizeEntity.Code.Length > 0) && this.Exists(names, values))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return str;
            }
            str = this.AddEntity(organizeEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseOrganizeEntity baseOrganizeEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseOrganizeEntity);
        }

        public string AddByDetail(string parentId, string code, string fullName, string category, string outerPhone, string innerPhone, string fax, bool enabled, out string statusCode)
        {
            BaseOrganizeEntity organizeEntity = new BaseOrganizeEntity {
                ParentId = new int?(int.Parse(parentId)),
                Code = code,
                FullName = fullName,
                Category = category,
                OuterPhone = outerPhone,
                InnerPhone = innerPhone,
                Fax = fax,
                Enabled = new int?(enabled ? 1 : 0)
            };
            return this.Add(organizeEntity, out statusCode);
        }

        public string AddEntity(BaseOrganizeEntity baseOrganizeEntity)
        {
            string s = string.Empty;
            if (baseOrganizeEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseOrganizeEntity.SortCode = new int?(int.Parse(s));
            }

            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(BaseOrganizeEntity.TableName, BaseOrganizeEntity.FieldId);

            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseOrganizeEntity.FieldId, baseOrganizeEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseOrganizeEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseOrganizeEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!baseOrganizeEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseOrganizeEntity.Id = new int?(int.Parse(s));
                }
                sqlBuilder.SetValue(BaseOrganizeEntity.FieldId, baseOrganizeEntity.Id, null);
            }

            this.SetEntity(sqlBuilder, baseOrganizeEntity);

            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseOrganizeEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseOrganizeEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseOrganizeEntity.FieldCreateOn);

            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseOrganizeEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseOrganizeEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseOrganizeEntity.FieldModifiedOn);

            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public override int BatchSave(DataTable dataTable)
        {
            int num = 0;
            BaseOrganizeEntity baseOrganizeEntity = new BaseOrganizeEntity();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BaseOrganizeEntity.FieldId, DataRowVersion.Original].ToString();
                    if (id.Length > 0)
                    {
                        num += this.DeleteEntity(id);
                    }
                }
                if ((row.RowState == DataRowState.Modified) && (row[BaseOrganizeEntity.FieldId, DataRowVersion.Original].ToString().Length > 0))
                {
                    baseOrganizeEntity.GetFrom(row);
                    num += this.UpdateEntity(baseOrganizeEntity);
                }
                if (row.RowState == DataRowState.Added)
                {
                    baseOrganizeEntity.GetFrom(row);
                    num += (this.AddEntity(baseOrganizeEntity).Length > 0) ? 1 : 0;
                }
                if (row.RowState != DataRowState.Unchanged)
                {
                    DataRowState rowState = row.RowState;
                }
            }
            return num;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseOrganizeEntity.FieldId, id);
        }

        public DataTable GetCompanyDT()
        {
            return DbLogic.GetDT(base.DbHelper, BaseOrganizeEntity.TableName, BaseOrganizeEntity.FieldCategory, "Company", BaseOrganizeEntity.FieldSortCode);
        }

        public DataTable GetDepartmentDT(string organizeId)
        {
            if (string.IsNullOrEmpty(organizeId))
            {
                return DbLogic.GetDT(base.DbHelper, BaseOrganizeEntity.TableName, BaseOrganizeEntity.FieldCategory, "Department", BaseOrganizeEntity.FieldSortCode);
            }
            return DbLogic.GetDT(base.DbHelper, BaseOrganizeEntity.TableName, BaseOrganizeEntity.FieldParentId, organizeId, BaseOrganizeEntity.FieldCategory, "Department", BaseOrganizeEntity.FieldSortCode);
        }

        public BaseOrganizeEntity GetEntity(int? id)
        {
            return this.GetEntity(id.ToString());
        }

        public BaseOrganizeEntity GetEntity(string id)
        {
            return new BaseOrganizeEntity(this.GetDT(BaseOrganizeEntity.FieldId, id));
        }

        public DataTable GetInnerOrganize()
        {
            return this.GetDT(BaseOrganizeEntity.FieldIsInnerOrganize, "1", BaseOrganizeEntity.FieldDeletionStateCode, "0", BaseOrganizeEntity.FieldSortCode);
        }

        //public DataTable GetOrganizeTree(DataTable dtOrganize = null) //C# 4.0 才支持缺省参数
        /// <summary>
        /// 获取组织结构树(只能获取内部组织) 后期可扩展，添加记录名称和父节点
        /// </summary>
        /// <param name="dtOrganize"></param>
        /// <returns></returns>
        public DataTable GetOrganizeTree(DataTable dtOrganize)
        {
            if (dtOrganize != null)
            {
                this.DTOrganize = dtOrganize;
            }
            if (this.organizeTable.Columns.Count == 0)
            {
                this.organizeTable.Columns.Add(new DataColumn(BaseOrganizeEntity.FieldId, Type.GetType("System.Int32")));
                this.organizeTable.Columns.Add(new DataColumn(BaseOrganizeEntity.FieldFullName, Type.GetType("System.String")));
            }
            if (this.DTOrganize == null)
            {
                string[] names = new string[] { BaseOrganizeEntity.FieldIsInnerOrganize, BaseOrganizeEntity.FieldEnabled, BaseOrganizeEntity.FieldDeletionStateCode };
                object[] values = new object[] { 1, 1, 0 };
                this.DTOrganize = this.GetDT(names, values, BaseOrganizeEntity.FieldSortCode);
            }
            for (int i = 0; i < this.DTOrganize.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(this.DTOrganize.Rows[i][BaseOrganizeEntity.FieldParentId].ToString()))
                {
                    DataRow row = this.organizeTable.NewRow();
                    row[BaseOrganizeEntity.FieldId] = this.DTOrganize.Rows[i][BaseOrganizeEntity.FieldId].ToString();
                    row[BaseOrganizeEntity.FieldFullName] = this.DTOrganize.Rows[i][BaseOrganizeEntity.FieldFullName].ToString();
                    this.organizeTable.Rows.Add(row);
                    this.GetSubOrganize(this.DTOrganize.Rows[i][BaseOrganizeEntity.FieldId].ToString());
                }
            }
            return this.organizeTable;
        }

        public DataTable GetOrganizeTree()
        {
            return GetOrganizeTree(null);
        }

        public void GetSubOrganize(string parentId)
        {
            this.head = this.head + "--";
            for (int i = 0; i < this.DTOrganize.Rows.Count; i++)
            {
                if (this.DTOrganize.Rows[i][BaseOrganizeEntity.FieldParentId].ToString().EndsWith(parentId))
                {
                    DataRow row = this.organizeTable.NewRow();
                    row[BaseOrganizeEntity.FieldId] = this.DTOrganize.Rows[i][BaseOrganizeEntity.FieldId].ToString();
                    row[BaseOrganizeEntity.FieldFullName] = this.head + this.DTOrganize.Rows[i][BaseOrganizeEntity.FieldFullName].ToString();
                    this.organizeTable.Rows.Add(row);
                    this.GetSubOrganize(this.DTOrganize.Rows[i][BaseOrganizeEntity.FieldId].ToString());
                }
            }
            this.head = this.head.Substring(0, this.head.Length - 2);
        }

        public int MoveTo(string id, string parentId)
        {
            return this.SetProperty(id, BaseOrganizeEntity.FieldParentId, parentId);
        }

        public DataTable Search(string organizeId, string searchValue)
        {
            searchValue = BaseBusinessLogic.GetSearchString(searchValue);
            string commandText = " SELECT * FROM " + BaseOrganizeEntity.TableName + " WHERE " + BaseOrganizeEntity.FieldCode + " LIKE '" + searchValue + "'       OR " + BaseOrganizeEntity.FieldFullName + " LIKE '" + searchValue + "'       OR " + BaseOrganizeEntity.FieldDescription + " LIKE '" + searchValue + "' ORDER BY " + BaseOrganizeEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseOrganizeEntity baseOrganizeEntity)
        {
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldParentId, baseOrganizeEntity.ParentId, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldCode, baseOrganizeEntity.Code, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldShortName, baseOrganizeEntity.ShortName, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldFullName, baseOrganizeEntity.FullName, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldCategory, baseOrganizeEntity.Category, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldLayer, baseOrganizeEntity.Layer, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldOuterPhone, baseOrganizeEntity.OuterPhone, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldInnerPhone, baseOrganizeEntity.InnerPhone, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldFax, baseOrganizeEntity.Fax, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldPostalcode, baseOrganizeEntity.Postalcode, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldAddress, baseOrganizeEntity.Address, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldWeb, baseOrganizeEntity.Web, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldManager, baseOrganizeEntity.Manager, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldAssistantManager, baseOrganizeEntity.AssistantManager, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldIsInnerOrganize, baseOrganizeEntity.IsInnerOrganize, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldBank, baseOrganizeEntity.Bank, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldBankAccount, baseOrganizeEntity.BankAccount, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldDeletionStateCode, baseOrganizeEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldEnabled, baseOrganizeEntity.Enabled, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldSortCode, baseOrganizeEntity.SortCode, null);
            sqlBuilder.SetValue(BaseOrganizeEntity.FieldDescription, baseOrganizeEntity.Description, null);
        }

        public int Update(BaseOrganizeEntity baseOrganizeEntity)
        {
            return this.UpdateEntity(baseOrganizeEntity);
        }

        public int Update(BaseOrganizeEntity organizeEntity, out string statusCode)
        {
            int num = 0;
            string[] names = new string[] { BaseOrganizeEntity.FieldParentId, BaseOrganizeEntity.FieldFullName, BaseOrganizeEntity.FieldDeletionStateCode };
            object[] values = new object[] { organizeEntity.ParentId, organizeEntity.FullName, 0 };
            if (this.Exists(names, values, organizeEntity.Id))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return num;
            }
            names = new string[] { BaseOrganizeEntity.FieldCode, BaseOrganizeEntity.FieldDeletionStateCode };
            values = new object[] { organizeEntity.Code, 0 };
            if ((organizeEntity.Code.Length > 0) && this.Exists(names, values, organizeEntity.Id))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return num;
            }
            num = this.UpdateEntity(organizeEntity);
            if (num == 1)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseOrganizeEntity baseOrganizeEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BaseOrganizeEntity.TableName);
            this.SetEntity(sqlBuilder, baseOrganizeEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseOrganizeEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseOrganizeEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseOrganizeEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseOrganizeEntity.FieldId, baseOrganizeEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

