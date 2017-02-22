namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseNewsManager : BaseManager, IBaseManager
    {
        public BaseNewsManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseNewsEntity.TableName;
        }

        public BaseNewsManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseNewsManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseNewsManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseNewsManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseNewsManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseNewsEntity baseNewsEntity)
        {
            return this.AddEntity(baseNewsEntity);
        }

        public string Add(BaseNewsEntity fileEntity, out string statusCode)
        {
            statusCode = string.Empty;
            string str = string.Empty;
            string[] names = new string[] { BaseNewsEntity.FieldFolderId, BaseNewsEntity.FieldTitle, BaseNewsEntity.FieldDeletionStateCode };
            object[] values = new object[] { fileEntity.FolderId, fileEntity.Title, 0 };
            if (this.Exists(names, values))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return str;
            }
            str = this.AddEntity(fileEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseNewsEntity baseNewsEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseNewsEntity);
        }

        public string AddEntity(BaseNewsEntity newsEntity)
        {
            string s = string.Empty;
            s = newsEntity.Id;
            if (newsEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                newsEntity.SortCode = new int?(int.Parse(s));
            }
            if (newsEntity.Id != null)
            {
                base.Identity = false;
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseNewsEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseNewsEntity.FieldId, newsEntity.Id, null);
            }
            else if (!base.ReturnId && (base.DbHelper.CurrentDbType == CurrentDbType.Oracle))
            {
                sqlBuilder.SetFormula(BaseNewsEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
            }
            else if (base.Identity && (base.DbHelper.CurrentDbType == CurrentDbType.Oracle))
            {
                if (string.IsNullOrEmpty(newsEntity.Id))
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    newsEntity.Id = s;
                }
                sqlBuilder.SetValue(BaseNewsEntity.FieldId, newsEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, newsEntity);
            if (base.UserInfo != null)
            {
                newsEntity.CreateUserId = base.UserInfo.Id;
                newsEntity.CreateBy = base.UserInfo.RealName;
                sqlBuilder.SetValue(BaseNewsEntity.FieldCreateUserId, newsEntity.CreateUserId, null);
                sqlBuilder.SetValue(BaseNewsEntity.FieldCreateBy, newsEntity.CreateBy, null);
            }
            sqlBuilder.SetDBNow(BaseNewsEntity.FieldCreateOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public int Delete(string id)
        {
            return this.Delete(BaseNewsEntity.FieldId, id);
        }

        public int DeleteByFolder(string folderId)
        {
            return this.Delete(BaseNewsEntity.FieldFolderId, folderId);
        }

        public DataTable GetDTByFolder(string folderid)
        {
            string commandText = string.Empty;
            commandText = " SELECT " + BaseNewsEntity.FieldId + "        ," + BaseNewsEntity.FieldFolderId + "        ," + BaseNewsEntity.FieldCode + "        ," + BaseNewsEntity.FieldTitle + "        ," + BaseNewsEntity.FieldHomePage + "        ," + BaseNewsEntity.FieldSubPage + "        ," + BaseNewsEntity.FieldFilePath + "        ," + BaseNewsEntity.FieldFileSize + "        ," + BaseNewsEntity.FieldReadCount + "        ," + BaseNewsEntity.FieldCategoryCode + "        ," + BaseNewsEntity.FieldDescription + "        ," + BaseNewsEntity.FieldAuditStatus + "        ," + BaseNewsEntity.FieldEnabled + "        ," + BaseNewsEntity.FieldDeletionStateCode + "        ," + BaseNewsEntity.FieldSortCode + "        ," + BaseNewsEntity.FieldCreateUserId + "        ," + BaseNewsEntity.FieldCreateBy + "        ," + BaseNewsEntity.FieldCreateOn + "        ," + BaseNewsEntity.FieldModifiedUserId + "        ," + BaseNewsEntity.FieldModifiedBy + "        ," + BaseNewsEntity.FieldModifiedOn + "       , (SELECT " + BaseFolderEntity.FieldFolderName + " FROM " + BaseFolderEntity.TableName + " WHERE " + BaseFolderEntity.FieldId + " = " + BaseNewsEntity.FieldFolderId + ") AS FolderFullName  FROM " + base.CurrentTableName + " WHERE " + BaseNewsEntity.FieldFolderId + " = " + base.DbHelper.GetParameter(BaseNewsEntity.FieldFolderId) + " ORDER BY " + BaseNewsEntity.FieldSortCode + " DESC ";
            return base.DbHelper.Fill(commandText, new IDbDataParameter[] { base.DbHelper.MakeParameter(BaseNewsEntity.FieldFolderId, folderid) });
        }

        public BaseNewsEntity GetEntity(string id)
        {
            return new BaseNewsEntity(this.GetDT(BaseNewsEntity.FieldId, id));
        }

        public BaseNewsEntity GetNews(string id)
        {
            this.UpdateReadCount(id);
            return this.GetEntity(id);
        }

        public int MoveTo(string id, string folderId)
        {
            string property = this.GetProperty(id, BaseNewsEntity.FieldTitle);
            this.Delete(BaseNewsEntity.FieldFolderId, folderId, BaseNewsEntity.FieldTitle, property);
            return this.SetProperty(id, BaseNewsEntity.FieldFolderId, folderId);
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
                " SELECT ", BaseNewsEntity.FieldId, "        ,", BaseNewsEntity.FieldFolderId, "        ,", BaseNewsEntity.FieldCategoryCode, "        ,", BaseNewsEntity.FieldCode, "        ,", BaseNewsEntity.FieldTitle, "        ,", BaseNewsEntity.FieldIntroduction, "        ,", BaseNewsEntity.FieldHomePage, "        ,", BaseNewsEntity.FieldSubPage, 
                "        ,", BaseNewsEntity.FieldFilePath, "        ,", BaseNewsEntity.FieldFileSize, "        ,", BaseNewsEntity.FieldReadCount, "        ,", BaseNewsEntity.FieldDescription, "        ,", BaseNewsEntity.FieldAuditStatus, "        ,", BaseNewsEntity.FieldEnabled, "        ,", BaseNewsEntity.FieldDeletionStateCode, "        ,", BaseNewsEntity.FieldSortCode, 
                "        ,", BaseNewsEntity.FieldCreateUserId, "        ,", BaseNewsEntity.FieldCreateBy, "        ,", BaseNewsEntity.FieldCreateOn, "        ,", BaseNewsEntity.FieldModifiedUserId, "        ,", BaseNewsEntity.FieldModifiedBy, "        ,", BaseNewsEntity.FieldModifiedOn, " FROM ", base.CurrentTableName, " WHERE ", BaseNewsEntity.FieldDeletionStateCode, 
                " = ", num
             });
            if (enabled.HasValue)
            {
                int num2 = enabled.Value ? 1 : 0;
                object obj2 = commandText;
                commandText = string.Concat(new object[] { obj2, " AND ", BaseNewsEntity.FieldEnabled, " = ", num2 });
            }
            if (!string.IsNullOrEmpty(userId))
            {
                string str2 = commandText;
                commandText = str2 + " AND " + BaseNewsEntity.FieldCreateUserId + " = " + userId;
            }
            if (!string.IsNullOrEmpty(categoryCode))
            {
                string str3 = commandText;
                commandText = str3 + " AND " + BaseNewsEntity.FieldCategoryCode + " = '" + categoryCode + "'";
            }
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            searchValue = searchValue.Trim();
            if (!string.IsNullOrEmpty(searchValue))
            {
                string str4 = commandText;
                string str5 = str4 + " AND (" + BaseNewsEntity.FieldTitle + " LIKE " + base.DbHelper.GetParameter(BaseNewsEntity.FieldTitle);
                string str6 = str5 + " OR " + BaseNewsEntity.FieldCategoryCode + " LIKE " + base.DbHelper.GetParameter(BaseNewsEntity.FieldCategoryCode);
                string str7 = str6 + " OR " + BaseNewsEntity.FieldCreateBy + " LIKE " + base.DbHelper.GetParameter(BaseNewsEntity.FieldCreateBy);
                string str8 = str7 + " OR " + BaseNewsEntity.FieldContents + " LIKE " + base.DbHelper.GetParameter(BaseNewsEntity.FieldContents);
                commandText = str8 + " OR " + BaseNewsEntity.FieldDescription + " LIKE " + base.DbHelper.GetParameter(BaseNewsEntity.FieldDescription) + ")";
                if (searchValue.IndexOf("%") < 0)
                {
                    searchValue = "%" + searchValue + "%";
                }
                list.Add(base.DbHelper.MakeParameter(BaseNewsEntity.FieldTitle, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseNewsEntity.FieldCategoryCode, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseNewsEntity.FieldCreateBy, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseNewsEntity.FieldContents, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseNewsEntity.FieldDescription, searchValue));
            }
            commandText = commandText + " ORDER BY " + BaseNewsEntity.FieldSortCode + " DESC ";
            return base.DbHelper.Fill(commandText, list.ToArray());
        }

        public override int SetDeleted(object[] ids)
        {
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                if (this.GetEntity(ids[i].ToString()).AuditStatus.Equals(AuditStatus.Draft.ToString()))
                {
                    num += this.SetDeleted(ids[i]);
                }
            }
            return num;
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseNewsEntity baseNewsEntity)
        {
            if (baseNewsEntity.Contents == null)
            {
                baseNewsEntity.FileSize = 0;
            }
            else
            {
                baseNewsEntity.FileSize = new int?(baseNewsEntity.Contents.Length);
            }
            sqlBuilder.SetValue(BaseNewsEntity.FieldFolderId, baseNewsEntity.FolderId, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldCategoryCode, baseNewsEntity.CategoryCode, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldCode, baseNewsEntity.Code, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldTitle, baseNewsEntity.Title, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldFilePath, baseNewsEntity.FilePath, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldIntroduction, baseNewsEntity.Introduction, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldContents, baseNewsEntity.Contents, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldSource, baseNewsEntity.Source, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldKeywords, baseNewsEntity.Keywords, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldFileSize, baseNewsEntity.FileSize, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldImageUrl, baseNewsEntity.ImageUrl, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldHomePage, baseNewsEntity.HomePage, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldSubPage, baseNewsEntity.SubPage, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldAuditStatus, baseNewsEntity.AuditStatus, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldReadCount, baseNewsEntity.ReadCount, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldDeletionStateCode, baseNewsEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldDescription, baseNewsEntity.Description, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldEnabled, baseNewsEntity.Enabled, null);
            sqlBuilder.SetValue(BaseNewsEntity.FieldSortCode, baseNewsEntity.SortCode, null);
        }

        public int SetTableColumns()
        {
            int num = 0;
            string currentTableName = base.CurrentTableName;
            string tableName = "新闻表";
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginDelete(BaseTableColumnsEntity.TableName);
            builder.SetWhere(BaseTableColumnsEntity.FieldTableCode, BaseNewsEntity.TableName);
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

        public string ShowNews(string id)
        {
            this.UpdateReadCount(id);
            string str = null;
            string commandText = " SELECT " + BaseNewsEntity.FieldContents + "   FROM " + base.CurrentTableName + "  WHERE " + BaseNewsEntity.FieldId + " = " + base.DbHelper.GetParameter(BaseNewsEntity.FieldId);
            IDataReader reader = null;
            try
            {
                reader = base.DbHelper.ExecuteReader(commandText, new IDbDataParameter[] { base.DbHelper.MakeParameter(BaseNewsEntity.FieldId, id) });
                if (reader.Read())
                {
                    str = reader[BaseNewsEntity.FieldContents].ToString();
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
            return str;
        }

        public int Update(BaseNewsEntity baseNewsEntity)
        {
            return this.UpdateEntity(baseNewsEntity);
        }

        public int Update(BaseNewsEntity fileEntity, out string statusCode)
        {
            statusCode = string.Empty;
            int num = this.UpdateEntity(fileEntity);
            if (num > 0)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseNewsEntity baseNewsEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseNewsEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseNewsEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseNewsEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseNewsEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseNewsEntity.FieldId, baseNewsEntity.Id);
            return sqlBuilder.EndUpdate();
        }

        public int UpdateFile(string id, string fileName, string contents)
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(base.CurrentTableName);
            builder.SetValue(BaseNewsEntity.FieldTitle, fileName, null);
            builder.SetValue(BaseNewsEntity.FieldContents, contents, null);
            builder.SetValue(BaseNewsEntity.FieldFileSize, contents.Length, null);
            builder.SetValue(BaseNewsEntity.FieldModifiedUserId, base.UserInfo.Id, null);
            builder.SetValue(BaseNewsEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            builder.SetDBNow(BaseNewsEntity.FieldModifiedOn);
            builder.SetWhere(BaseNewsEntity.FieldId, id);
            return builder.EndUpdate();
        }

        private int UpdateReadCount(string id)
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(base.CurrentTableName);
            builder.SetFormula(BaseNewsEntity.FieldReadCount, BaseNewsEntity.FieldReadCount + " + 1 ");
            builder.SetWhere(BaseNewsEntity.FieldId, id);
            return builder.EndUpdate();
        }

        public string Upload(string folderId, string title, string contents)
        {
            string str = this.GetId(BaseNewsEntity.FieldFolderId, folderId, BaseNewsEntity.FieldTitle, title);
            if (!string.IsNullOrEmpty(str))
            {
                this.UpdateFile(str, title, contents);
                return str;
            }
            BaseNewsEntity newsEntity = new BaseNewsEntity {
                FolderId = folderId,
                Title = title,
                Contents = contents
            };
            return this.AddEntity(newsEntity);
        }
    }
}

