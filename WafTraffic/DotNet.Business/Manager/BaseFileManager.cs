namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseFileManager : BaseManager, IBaseManager
    {
        public BaseFileManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseFileEntity.TableName;
        }

        public BaseFileManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseFileManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseFileManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseFileManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseFileManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseFileEntity fileEntity)
        {
            return this.AddEntity(fileEntity);
        }

        public string Add(BaseFileEntity fileEntity, out string statusCode)
        {
            statusCode = string.Empty;
            string str = string.Empty;
            string[] names = new string[] { BaseFileEntity.FieldFolderId, BaseFileEntity.FieldFileName, BaseFileEntity.FieldDeletionStateCode };
            object[] values = new object[] { fileEntity.FolderId, fileEntity.FileName, 0 };
            if (this.Exists(names, values))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return str;
            }
            str = this.AddEntity(fileEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseFileEntity fileEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(fileEntity);
        }

        public string Add(string folderId, string fileName, string file, string description, string category, bool enabled, out string statusCode)
        {
            return this.Add(folderId, fileName, file, null, description, category, enabled, out statusCode);
        }

        public string Add(string folderId, string fileName, byte[] file, string description, string category, bool enabled, out string statusCode)
        {
            return this.Add(folderId, fileName, null, file, description, category, enabled, out statusCode);
        }

        private string Add(string folderId, string fileName, string file, byte[] byteFile, string description, string categoryCode, bool enabled, out string statusCode)
        {
            statusCode = string.Empty;
            BaseFileEntity fileEntity = new BaseFileEntity {
                FolderId = folderId,
                FileName = fileName,
                Contents = byteFile,
                Description = description,
                CategoryCode = categoryCode,
                Enabled = new int?(enabled ? 1 : 0)
            };
            string str = string.Empty;
            if (this.Exists(BaseFileEntity.FieldFolderId, fileEntity.FolderId, BaseFileEntity.FieldFileName, fileEntity.FileName))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return str;
            }
            str = this.AddEntity(fileEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string AddEntity(BaseFileEntity fileEntity)
        {
            string s = string.Empty;
            s = fileEntity.Id;
            if (fileEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                fileEntity.SortCode = new int?(int.Parse(s));
            }
            if (fileEntity.Id != null)
            {
                base.Identity = false;
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseFileEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseFileEntity.FieldId, fileEntity.Id, null);
            }
            else if (!base.ReturnId && (base.DbHelper.CurrentDbType == CurrentDbType.Oracle))
            {
                sqlBuilder.SetFormula(BaseFileEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
            }
            else if (base.Identity && (base.DbHelper.CurrentDbType == CurrentDbType.Oracle))
            {
                if (string.IsNullOrEmpty(fileEntity.Id))
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    fileEntity.Id = s;
                }
                sqlBuilder.SetValue(BaseFileEntity.FieldId, fileEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, fileEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseFileEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseFileEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseFileEntity.FieldCreateOn);
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
            BaseFileEntity fileEntity = new BaseFileEntity();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BaseFileEntity.FieldId, DataRowVersion.Original].ToString();
                    if (id.Length > 0)
                    {
                        num += this.Delete(id);
                    }
                }
                if ((row.RowState == DataRowState.Modified) && (row[BaseFileEntity.FieldId, DataRowVersion.Original].ToString().Length > 0))
                {
                    fileEntity.GetFrom(row);
                    num += this.UpdateEntity(fileEntity);
                }
                if (row.RowState == DataRowState.Added)
                {
                    fileEntity.GetFrom(row);
                    num += (this.AddEntity(fileEntity).Length > 0) ? 1 : 0;
                }
                if (row.RowState != DataRowState.Unchanged)
                {
                    DataRowState rowState = row.RowState;
                }
            }
            return num;
        }

        public int Delete(string id)
        {
            return this.Delete(BaseFileEntity.FieldId, id);
        }

        public int DeleteByFolder(string folderId)
        {
            return this.Delete(BaseFileEntity.FieldFolderId, folderId);
        }

        public byte[] Download(string id)
        {
            this.UpdateReadCount(id);
            byte[] buffer = null;
            string commandText = " SELECT " + BaseFileEntity.FieldContents + "   FROM " + BaseFileEntity.TableName + "  WHERE " + BaseFileEntity.FieldId + " = " + base.DbHelper.GetParameter(BaseFileEntity.FieldId);
            IDataReader reader = null;
            try
            {
                reader = base.DbHelper.ExecuteReader(commandText, new IDbDataParameter[] { base.DbHelper.MakeParameter(BaseFileEntity.FieldId, id) });
                if (reader.Read())
                {
                    buffer = (byte[]) reader[BaseFileEntity.FieldContents];
                }
            }
            catch (Exception exception)
            {
                BaseExceptionManager.LogException(base.DbHelper, base.UserInfo, exception);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return buffer;
        }

        public DataTable GetDTByFolder(string id)
        {
            string commandText = string.Empty;
            commandText = " SELECT " + BaseFileEntity.FieldId + "        ," + BaseFileEntity.FieldFolderId + "        ," + BaseFileEntity.FieldFileName + "        ," + BaseFileEntity.FieldFilePath + "        ," + BaseFileEntity.FieldFileSize + "        ," + BaseFileEntity.FieldReadCount + "        ," + BaseFileEntity.FieldCategoryCode + "        ," + BaseFileEntity.FieldDescription + "        ," + BaseFileEntity.FieldEnabled + "        ," + BaseFileEntity.FieldSortCode + "        ," + BaseFileEntity.FieldCreateUserId + "        ," + BaseFileEntity.FieldCreateBy + "        ," + BaseFileEntity.FieldCreateOn + "        ," + BaseFileEntity.FieldModifiedUserId + "        ," + BaseFileEntity.FieldModifiedBy + "        ," + BaseFileEntity.FieldModifiedOn + "       , (SELECT " + BaseFolderEntity.FieldFolderName + " FROM " + BaseFolderEntity.TableName + " WHERE " + BaseFolderEntity.FieldId + " = " + BaseFileEntity.FieldFolderId + ") AS FolderFullName  FROM " + BaseFileEntity.TableName + " WHERE " + BaseFileEntity.FieldFolderId + " = " + base.DbHelper.GetParameter(BaseFileEntity.FieldFolderId);
            DataTable dataTable = new DataTable(BaseFileEntity.TableName);
            base.DbHelper.Fill(dataTable, commandText, new IDbDataParameter[] { base.DbHelper.MakeParameter(BaseFileEntity.FieldFolderId, id) });
            if (id.Length == 0)
            {
                DataRow row = dataTable.NewRow();
                row[BaseFileEntity.FieldEnabled] = 1;
                dataTable.Rows.Add(row);
                dataTable.AcceptChanges();
            }
            this.GetFrom(dataTable);
            return dataTable;
        }

        public BaseFileEntity GetEntity(string id)
        {
            string commandText = string.Empty;
            commandText = " SELECT " + BaseFileEntity.FieldId + "        ," + BaseFileEntity.FieldFolderId + "        ," + BaseFileEntity.FieldCategoryCode + "        ," + BaseFileEntity.FieldCode + "        ," + BaseFileEntity.FieldFileName + "        ," + BaseFileEntity.FieldSource + "        ," + BaseFileEntity.FieldIntroduction + "        ," + BaseFileEntity.FieldHomePage + "        ," + BaseFileEntity.FieldSubPage + "        ," + BaseFileEntity.FieldFilePath + "        ," + BaseFileEntity.FieldImageUrl + "        ," + BaseFileEntity.FieldKeywords + "        ," + BaseFileEntity.FieldFileSize + "        ," + BaseFileEntity.FieldReadCount + "        ," + BaseFileEntity.FieldDescription + "        ," + BaseFileEntity.FieldAuditStatus + "        ," + BaseFileEntity.FieldEnabled + "        ," + BaseFileEntity.FieldDeletionStateCode + "        ," + BaseFileEntity.FieldSortCode + "        ," + BaseFileEntity.FieldCreateUserId + "        ," + BaseFileEntity.FieldCreateBy + "        ," + BaseFileEntity.FieldCreateOn + "        ," + BaseFileEntity.FieldModifiedUserId + "        ," + BaseFileEntity.FieldModifiedBy + "        ," + BaseFileEntity.FieldModifiedOn + " FROM " + base.CurrentTableName + " WHERE " + BaseFileEntity.FieldId + " = " + base.DbHelper.GetParameter(BaseFileEntity.FieldId);
            DataTable dataTable = new DataTable(BaseFileEntity.TableName);
            base.DbHelper.Fill(dataTable, commandText, new IDbDataParameter[] { base.DbHelper.MakeParameter(BaseFileEntity.FieldId, id) });
            return new BaseFileEntity(dataTable);
        }

        public int GetFileCount()
        {
            string commandText = " SELECT COUNT(*) FROM Base_File ";
            return int.Parse(base.DbHelper.ExecuteScalar(commandText).ToString());
        }

        public int GetFileCount(bool enabled)
        {
            string commandText = " SELECT COUNT(*) FROM Base_File WHERE Enabled = ";
            commandText = commandText + (enabled ? "1" : "0");
            return int.Parse(base.DbHelper.ExecuteScalar(commandText).ToString());
        }

        public int GetFileCount(string userId)
        {
            string commandText = " SELECT COUNT(*) FROM Base_File WHERE CreateUserId='" + userId + "'";
            return int.Parse(base.DbHelper.ExecuteScalar(commandText).ToString());
        }

        public int GetFileCount(string folderId, string userId)
        {
            string commandText = " SELECT SUM(FileSize * ReadCount) FROM Base_File  WHERE CreateUserId='" + userId + "' AND " + BaseFolderEntity.FieldParentId + " = '" + folderId + "'";
            return int.Parse(base.DbHelper.ExecuteScalar(commandText).ToString());
        }

        public int GetFlowmeter()
        {
            string commandText = "SELECT SUM(FileSize * ReadCount) FROM Base_File ";
            return int.Parse(base.DbHelper.ExecuteScalar(commandText).ToString());
        }

        public int GetFlowmeter(string userId)
        {
            string commandText = "SELECT SUM(FileSize * ReadCount) FROM Base_File  WHERE CreateUserId='" + userId + "'";
            return int.Parse(base.DbHelper.ExecuteScalar(commandText).ToString());
        }

        public double GetSumFileSize()
        {
            string commandText = " SELECT SUM(FileSize) FROM Base_File ";
            return double.Parse(base.DbHelper.ExecuteScalar(commandText).ToString());
        }

        public double GetSumFileSize(bool enabled)
        {
            string commandText = " SELECT SUM(FileSize) FROM Base_File WHERE Enabled = ";
            commandText = commandText + (enabled ? "1" : "0");
            return double.Parse(base.DbHelper.ExecuteScalar(commandText).ToString());
        }

        public double GetSumFileSize(string userId)
        {
            string commandText = " SELECT SUM(FileSize) FROM Base_File WHERE CreateUserId='" + userId + "'";
            return double.Parse(base.DbHelper.ExecuteScalar(commandText).ToString());
        }

        public int MoveTo(string id, string folderId)
        {
            string property = this.GetProperty(id, BaseFileEntity.FieldFileName);
            this.Delete(BaseFileEntity.FieldFolderId, folderId, BaseFileEntity.FieldFileName, property);
            return this.SetProperty(id, BaseFileEntity.FieldFolderId, folderId);
        }

        public DataTable Search(string searchValue)
        {
            return this.Search(string.Empty, searchValue);
        }

        public DataTable Search(string userId, string searchValue)
        {
            return this.Search(userId, string.Empty, searchValue);
        }

        public DataTable Search(string userId, string categoryCode, string searchValue)
        {
            return this.Search(userId, categoryCode, searchValue, null, null);
        }

        public DataTable Search(string userId, string categoryCode, string searchValue, bool? enabled, bool? deletionStateCode)
        {
            int num = 0;
            if (deletionStateCode.HasValue)
            {
                num = deletionStateCode.Value ? 1 : 0;
            }
            string commandText = string.Empty;
            commandText = string.Concat(new object[] { 
                " SELECT ", BaseFileEntity.FieldId, "        ,", BaseFileEntity.FieldFolderId, "        ,", BaseFileEntity.FieldCategoryCode, "        ,", BaseFileEntity.FieldCode, "        ,", BaseFileEntity.FieldFileName, "        ,", BaseFileEntity.FieldIntroduction, "        ,", BaseFileEntity.FieldHomePage, "        ,", BaseFileEntity.FieldSubPage, 
                "        ,", BaseFileEntity.FieldFilePath, "        ,", BaseFileEntity.FieldFileSize, "        ,", BaseFileEntity.FieldReadCount, "        ,", BaseFileEntity.FieldDescription, "        ,", BaseFileEntity.FieldCategoryCode, "        ,", BaseFileEntity.FieldAuditStatus, "        ,", BaseFileEntity.FieldEnabled, "        ,", BaseFileEntity.FieldDeletionStateCode, 
                "        ,", BaseFileEntity.FieldSortCode, "        ,", BaseFileEntity.FieldCreateUserId, "        ,", BaseFileEntity.FieldCreateBy, "        ,", BaseFileEntity.FieldCreateOn, "        ,", BaseFileEntity.FieldModifiedUserId, "        ,", BaseFileEntity.FieldModifiedBy, "        ,", BaseFileEntity.FieldModifiedOn, " FROM ", base.CurrentTableName, 
                " WHERE ", BaseFileEntity.FieldDeletionStateCode, " = ", num
             });
            if (enabled.HasValue)
            {
                int num2 = enabled.Value ? 1 : 0;
                object obj2 = commandText;
                commandText = string.Concat(new object[] { obj2, " AND ", BaseFileEntity.FieldEnabled, " = ", num2 });
            }
            if (!string.IsNullOrEmpty(userId))
            {
                string str2 = commandText;
                commandText = str2 + " AND " + BaseFileEntity.FieldCreateUserId + " = " + userId;
            }
            if (!string.IsNullOrEmpty(categoryCode))
            {
                string str3 = commandText;
                commandText = str3 + " AND " + BaseFileEntity.FieldCategoryCode + " = '" + categoryCode + "'";
            }
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            searchValue = searchValue.Trim();
            if (!string.IsNullOrEmpty(searchValue))
            {
                string str4 = commandText;
                string str5 = str4 + " AND (" + BaseFileEntity.FieldFileName + " LIKE " + base.DbHelper.GetParameter(BaseFileEntity.FieldFileName);
                string str6 = str5 + " OR " + BaseFileEntity.FieldCategoryCode + " LIKE " + base.DbHelper.GetParameter(BaseFileEntity.FieldCategoryCode);
                string str7 = str6 + " OR " + BaseFileEntity.FieldCreateBy + " LIKE " + base.DbHelper.GetParameter(BaseFileEntity.FieldCreateBy);
                string str8 = str7 + " OR " + BaseFileEntity.FieldContents + " LIKE " + base.DbHelper.GetParameter(BaseFileEntity.FieldContents);
                commandText = str8 + " OR " + BaseFileEntity.FieldDescription + " LIKE " + base.DbHelper.GetParameter(BaseFileEntity.FieldDescription) + ")";
                if (searchValue.IndexOf("%") < 0)
                {
                    searchValue = "%" + searchValue + "%";
                }
                list.Add(base.DbHelper.MakeParameter(BaseFileEntity.FieldFileName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseFileEntity.FieldCategoryCode, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseFileEntity.FieldCreateBy, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseFileEntity.FieldContents, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseFileEntity.FieldDescription, searchValue));
            }
            commandText = commandText + " ORDER BY " + BaseFileEntity.FieldSortCode + " DESC ";
            return base.DbHelper.Fill(commandText, list.ToArray());
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseFileEntity fileEntity)
        {
            sqlBuilder.SetValue(BaseFileEntity.FieldFolderId, fileEntity.FolderId, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldCategoryCode, fileEntity.CategoryCode, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldCode, fileEntity.Code, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldFileName, fileEntity.FileName, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldFilePath, fileEntity.FilePath, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldIntroduction, fileEntity.Introduction, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldContents, fileEntity.Contents, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldSource, fileEntity.Source, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldKeywords, fileEntity.Keywords, null);
            if (fileEntity.Contents != null)
            {
                sqlBuilder.SetValue(BaseFileEntity.FieldFileSize, fileEntity.Contents.Length, null);
            }
            else
            {
                sqlBuilder.SetValue(BaseFileEntity.FieldFileSize, fileEntity.FileSize, null);
            }
            sqlBuilder.SetValue(BaseFileEntity.FieldImageUrl, fileEntity.ImageUrl, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldHomePage, fileEntity.HomePage, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldSubPage, fileEntity.SubPage, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldAuditStatus, fileEntity.AuditStatus, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldReadCount, fileEntity.ReadCount, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldDeletionStateCode, fileEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldDescription, fileEntity.Description, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldEnabled, fileEntity.Enabled, null);
            sqlBuilder.SetValue(BaseFileEntity.FieldSortCode, fileEntity.SortCode, null);
        }

        public int SetTableColumns()
        {
            int num = 0;
            string currentTableName = base.CurrentTableName;
            string tableName = "新闻表";
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginDelete(BaseTableColumnsEntity.TableName);
            builder.SetWhere(BaseTableColumnsEntity.FieldTableCode, BaseFileEntity.TableName);
            num += builder.EndDelete();
            num += this.SetTableColumns(currentTableName, tableName, "Id", "代码");
            num += this.SetTableColumns(currentTableName, tableName, "FolderId", "文件夹节点代码");
            num += this.SetTableColumns(currentTableName, tableName, "CategoryCode", "文件类别码");
            num += this.SetTableColumns(currentTableName, tableName, "Title", "文件名");
            num += this.SetTableColumns(currentTableName, tableName, "FilePath", "文件路径");
            num += this.SetTableColumns(currentTableName, tableName, "Introduction", "内容简介");
            num += this.SetTableColumns(currentTableName, tableName, "Contents", "文件内容");
            num += this.SetTableColumns(currentTableName, tableName, "Source", "新闻来源");
            num += this.SetTableColumns(currentTableName, tableName, "Keywords", "关键字");
            num += this.SetTableColumns(currentTableName, tableName, "FileSize", "文件大小");
            num += this.SetTableColumns(currentTableName, tableName, "ImageUrl", "图片文件位置(图片新闻)");
            num += this.SetTableColumns(currentTableName, tableName, "HomePage", "置首页");
            num += this.SetTableColumns(currentTableName, tableName, "AuditStatus", "审核状态");
            num += this.SetTableColumns(currentTableName, tableName, "ReadCount", "被阅读次数");
            num += this.SetTableColumns(currentTableName, tableName, "DeletionStateCode", "删除标志");
            num += this.SetTableColumns(currentTableName, tableName, "Description", "描述");
            num += this.SetTableColumns(currentTableName, tableName, "Enabled", "有效");
            num += this.SetTableColumns(currentTableName, tableName, "SortCode", "排序码");
            num += this.SetTableColumns(currentTableName, tableName, "CreateOn", "创建日期");
            num += this.SetTableColumns(currentTableName, tableName, "CreateBy", "创建用户");
            num += this.SetTableColumns(currentTableName, tableName, "CreateUserId", "创建用户主键");
            num += this.SetTableColumns(currentTableName, tableName, "ModifiedOn", "修改日期");
            num += this.SetTableColumns(currentTableName, tableName, "ModifiedBy", "修改用户");
            return (num + this.SetTableColumns(currentTableName, tableName, "ModifiedUserId", "修改用户主键"));
        }

        public int Update(BaseFileEntity fileEntity)
        {
            return this.UpdateEntity(fileEntity);
        }

        public int Update(BaseFileEntity fileEntity, out string statusCode)
        {
            int num = 0;
            if (this.Exists(BaseFileEntity.FieldFolderId, fileEntity.FolderId, BaseFileEntity.FieldFileName, fileEntity.FileName, fileEntity.Id))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return num;
            }
            num = this.UpdateEntity(fileEntity);
            if (num == 1)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int Update(string id, string folderId, string fileName, string description, bool enabled, out string statusCode)
        {
            statusCode = string.Empty;
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(BaseFileEntity.TableName);
            builder.SetValue(BaseFileEntity.FieldFolderId, folderId, null);
            builder.SetValue(BaseFileEntity.FieldFileName, fileName, null);
            builder.SetValue(BaseFileEntity.FieldEnabled, enabled, null);
            builder.SetValue(BaseFileEntity.FieldDescription, description, null);
            builder.SetValue(BaseFileEntity.FieldModifiedUserId, base.UserInfo.Id, null);
            builder.SetValue(BaseFileEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            builder.SetDBNow(BaseFileEntity.FieldModifiedOn);
            builder.SetWhere(BaseFileEntity.FieldId, id);
            int num = builder.EndUpdate();
            if (num > 0)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseFileEntity fileEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, fileEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseFileEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseFileEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseFileEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseFileEntity.FieldId, fileEntity.Id);
            return sqlBuilder.EndUpdate();
        }

        public int UpdateFile(string id, string fileName, byte[] file)
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(BaseFileEntity.TableName);
            builder.SetValue(BaseFileEntity.FieldFileName, fileName, null);
            if (file != null)
            {
                builder.SetValue(BaseFileEntity.FieldContents, file, null);
                builder.SetValue(BaseFileEntity.FieldFileSize, file.Length, null);
            }
            builder.SetValue(BaseFileEntity.FieldModifiedUserId, base.UserInfo.Id, null);
            builder.SetValue(BaseFileEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            builder.SetDBNow(BaseFileEntity.FieldModifiedOn);
            builder.SetWhere(BaseFileEntity.FieldId, id);
            return builder.EndUpdate();
        }

        public int UpdateFile(string id, string fileName, byte[] file, out string statusCode)
        {
            statusCode = string.Empty;
            int num = this.UpdateFile(id, fileName, file);
            if (num > 0)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        private int UpdateReadCount(string id)
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(BaseFileEntity.TableName);
            builder.SetFormula(BaseFileEntity.FieldReadCount, BaseFileEntity.FieldReadCount + " + 1 ");
            builder.SetWhere(BaseFileEntity.FieldId, id);
            return builder.EndUpdate();
        }

        public string Upload(string folderId, string fileName, byte[] file, bool enabled)
        {
            string str = this.GetId(BaseFileEntity.FieldFolderId, folderId, BaseFileEntity.FieldFileName, fileName);
            if (!string.IsNullOrEmpty(str))
            {
                this.UpdateFile(str, fileName, file);
                return str;
            }
            BaseFileEntity fileEntity = new BaseFileEntity {
                FolderId = folderId,
                FileName = fileName,
                Contents = file,
                Enabled = new int?(enabled ? 1 : 0)
            };
            return this.AddEntity(fileEntity);
        }
    }
}

