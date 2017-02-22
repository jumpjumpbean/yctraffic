namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseFolderManager : BaseManager
    {
        public BaseFolderManager()
        {
            base.CurrentTableName = BaseFolderEntity.TableName;
        }

        public BaseFolderManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseFolderManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseFolderManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public string Add(BaseFolderEntity folderEntity, out string statusCode)
        {
            string str = string.Empty;
            if (this.Exists(BaseFolderEntity.FieldParentId, folderEntity.ParentId, BaseFolderEntity.FieldFolderName, folderEntity.FolderName))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return str;
            }
            str = this.AddEntity(folderEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string AddEntity(BaseFolderEntity folderEntity)
        {
            string sequence = new BaseSequenceManager(base.DbHelper).GetSequence(BaseFolderEntity.TableName);
            if (string.IsNullOrEmpty(folderEntity.Id))
            {
                folderEntity.Id = BaseBusinessLogic.NewGuid();
            }
            folderEntity.SortCode = sequence;
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginInsert(BaseFolderEntity.TableName);
            sqlBuilder.SetValue(BaseFolderEntity.FieldId, folderEntity.Id, null);
            this.SetEntity(sqlBuilder, folderEntity);
            sqlBuilder.SetValue(BaseFolderEntity.FieldCreateUserId, base.UserInfo.Id, null);
            sqlBuilder.SetValue(BaseFolderEntity.FieldCreateBy, base.UserInfo.RealName, null);
            sqlBuilder.SetDBNow(BaseFolderEntity.FieldCreateOn);
            sqlBuilder.SetValue(BaseFolderEntity.FieldModifiedUserId, base.UserInfo.Id, null);
            sqlBuilder.SetValue(BaseFolderEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            sqlBuilder.SetDBNow(BaseFolderEntity.FieldModifiedOn);
            if (sqlBuilder.EndInsert() <= 0)
            {
                return string.Empty;
            }
            return sequence;
        }

        public override int BatchSave(DataTable dataTable)
        {
            int num = 0;
            BaseFolderEntity folderEntity = new BaseFolderEntity();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BaseFolderEntity.FieldId, DataRowVersion.Original].ToString();
                    if (id.Length > 0)
                    {
                        num += this.DeleteEntity(id);
                    }
                }
                if ((row.RowState == DataRowState.Modified) && (row[BaseFolderEntity.FieldId, DataRowVersion.Original].ToString().Length > 0))
                {
                    folderEntity.GetFrom(row);
                    num += this.UpdateEntity(folderEntity);
                }
                if (row.RowState == DataRowState.Added)
                {
                    this.GetFrom(row);
                    num += (this.AddEntity(folderEntity).Length > 0) ? 1 : 0;
                }
                if (row.RowState != DataRowState.Unchanged)
                {
                    DataRowState rowState = row.RowState;
                }
            }
            return num;
        }

        public BaseFolderEntity GetEntity(string id)
        {
            return new BaseFolderEntity(this.GetDTById(id));
        }

        public int MoveTo(string id, string parentId)
        {
            return this.SetProperty(id, BaseOrganizeEntity.FieldParentId, parentId);
        }

        public DataTable Search(string searchValue)
        {
            string commandText = string.Empty;
            commandText = " SELECT *  FROM " + BaseFolderEntity.TableName + " WHERE " + BaseFolderEntity.FieldFolderName + " LIKE " + base.DbHelper.GetParameter(BaseFolderEntity.FieldFolderName) + " OR " + BaseFolderEntity.FieldDescription + " LIKE " + base.DbHelper.GetParameter(BaseFolderEntity.FieldDescription);
            DataTable dataTable = new DataTable(BaseFolderEntity.TableName);
            searchValue = searchValue.Trim();
            if (searchValue.IndexOf("%") < 0)
            {
                searchValue = "%" + searchValue + "%";
            }
            List<IDbDataParameter> list = new List<IDbDataParameter> {
                base.DbHelper.MakeParameter(BaseFolderEntity.FieldFolderName, searchValue),
                base.DbHelper.MakeParameter(BaseFolderEntity.FieldDescription, searchValue)
            };
            base.DbHelper.Fill(dataTable, commandText, list.ToArray());
            return dataTable;
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseFolderEntity folderEntity)
        {
            sqlBuilder.SetValue(BaseFolderEntity.FieldParentId, folderEntity.ParentId, null);
            sqlBuilder.SetValue(BaseFolderEntity.FieldFolderName, folderEntity.FolderName, null);
            sqlBuilder.SetValue(BaseFolderEntity.FieldSortCode, folderEntity.SortCode, null);
            sqlBuilder.SetValue(BaseFolderEntity.FieldDescription, folderEntity.Description, null);
            sqlBuilder.SetValue(BaseFolderEntity.FieldEnabled, folderEntity.Enabled ? 1 : 0, null);
        }

        public int Update(BaseFolderEntity folderEntity, out string statusCode)
        {
            int num = 0;
            if (this.Exists(BaseFolderEntity.FieldParentId, folderEntity.ParentId, BaseFolderEntity.FieldFolderName, folderEntity.FolderName, folderEntity.Id))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return num;
            }
            num = this.UpdateEntity(folderEntity);
            if (num == 1)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseFolderEntity folderEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BaseFolderEntity.TableName);
            this.SetEntity(sqlBuilder, folderEntity);
            sqlBuilder.SetValue(BaseFolderEntity.FieldModifiedUserId, base.UserInfo.Id, null);
            sqlBuilder.SetValue(BaseFolderEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            sqlBuilder.SetDBNow(BaseFolderEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseFolderEntity.FieldId, folderEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

