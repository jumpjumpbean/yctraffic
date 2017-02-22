namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class BaseCommentManager : BaseManager, IBaseManager
    {
        public BaseCommentManager()
        {
            base.CurrentTableName = BaseCommentEntity.TableName;
            base.PrimaryKey = "Id";
            base.Identity = false;
        }

        public BaseCommentManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseCommentManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseCommentManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseCommentManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseCommentManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseCommentEntity baseCommentEntity)
        {
            return this.AddEntity(baseCommentEntity);
        }

        public string Add(BaseCommentEntity baseCommentEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseCommentEntity);
        }

        public string Add(string categoryCode, string objectId, string contents, string ipAddress)
        {
            BaseCommentEntity baseCommentEntity = new BaseCommentEntity {
                CreateUserId = base.UserInfo.Id,
                CategoryCode = categoryCode,
                ObjectId = objectId,
                Contents = contents,
                DeletionStateCode = 0,
                Enabled = 0,
                IPAddress = ipAddress,
                CreateBy = base.UserInfo.RealName
            };
            return this.Add(baseCommentEntity, false, false);
        }

        public string AddEntity(BaseCommentEntity entity)
        {
            string s = string.Empty;
            if (entity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                entity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseCommentEntity.FieldId);
            if (!string.IsNullOrEmpty(entity.Id))
            {
                sqlBuilder.SetValue(BaseCommentEntity.FieldId, entity.Id, null);
            }
            this.SetEntity(sqlBuilder, entity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseCommentEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseCommentEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseCommentEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseCommentEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseCommentEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseCommentEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public int ChageWorked(IDbHelper dbHelper, string id, bool worked)
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(BaseCommentEntity.TableName);
            builder.SetWhere(BaseCommentEntity.FieldId, id);
            return builder.EndUpdate();
        }

        public int ChangeWorked(string id)
        {
            return 0;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseCommentEntity.FieldId, id);
        }

        public DataTable GetDTByCategory(string categoryCode, int topLimit)
        {
            return this.GetDT(BaseCommentEntity.FieldCategoryCode, categoryCode, BaseCommentEntity.FieldCreateOn, topLimit);
        }

        public DataTable GetDTByCategory(string categoryCode, string objectId)
        {
            string[] names = new string[] { BaseCommentEntity.FieldCategoryCode, BaseCommentEntity.FieldObjectId, BaseCommentEntity.FieldDeletionStateCode };
            object[] values = new object[] { categoryCode, objectId, 0 };
            return this.GetDT(names, values, BaseCommentEntity.FieldCreateOn);
        }

        public BaseCommentEntity GetEntity(int id)
        {
            return new BaseCommentEntity(this.GetDT(BaseCommentEntity.FieldId, id));
        }

        public DataTable GetPreviousNextID(string id)
        {
            string commandText = "   SELECT Id      FROM " + BaseCommentEntity.TableName + "    WHERE (" + BaseCommentEntity.FieldCreateUserId + " = ?)  ORDER BY " + BaseCommentEntity.FieldSortCode;
            string[] targetFileds = new string[1];
            object[] targetValues = new object[1];
            targetFileds[0] = BaseCommentEntity.FieldCreateUserId;
            targetValues[0] = base.UserInfo.Id;
            DataTable dataTable = new DataTable(BaseCommentEntity.TableName);
            base.DbHelper.Fill(dataTable, commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public DataTable Search(string searchValue)
        {
            return this.Search(string.Empty, string.Empty, searchValue);
        }

        public DataTable Search(string year, string month, string searchValue)
        {
            DataTable dataTable = new DataTable(base.CurrentTableName);
            string commandText = " SELECT *  FROM " + base.CurrentTableName + " WHERE (" + BaseCommentEntity.FieldDeletionStateCode + " = 0) ";
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            if (!string.IsNullOrEmpty(year) && !year.Equals("All"))
            {
                string str2 = commandText;
                commandText = str2 + " AND YEAR(" + BaseCommentEntity.FieldCreateOn + ") = " + base.DbHelper.GetParameter("CurrentYear");
                list.Add(base.DbHelper.MakeParameter("CurrentYear", year));
            }
            if (!string.IsNullOrEmpty(month) && !month.Equals("All"))
            {
                string str3 = commandText;
                commandText = str3 + " AND MONTH(" + BaseCommentEntity.FieldCreateOn + ") = " + base.DbHelper.GetParameter("CurrentMonth");
                list.Add(base.DbHelper.MakeParameter("CurrentMonth", month));
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                string str4 = commandText;
                string str5 = str4 + " AND (" + BaseCommentEntity.FieldTitle + " LIKE " + base.DbHelper.GetParameter(BaseCommentEntity.FieldTitle);
                string str6 = str5 + " OR " + BaseCommentEntity.FieldCreateBy + " LIKE " + base.DbHelper.GetParameter(BaseCommentEntity.FieldCreateBy);
                commandText = str6 + " OR " + BaseCommentEntity.FieldContents + " LIKE " + base.DbHelper.GetParameter(BaseCommentEntity.FieldContents) + ")";
                searchValue = BaseBusinessLogic.GetSearchString(searchValue);
                list.Add(base.DbHelper.MakeParameter(BaseCommentEntity.FieldTitle, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseCommentEntity.FieldCreateBy, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseCommentEntity.FieldContents, searchValue));
            }
            commandText = commandText + " ORDER BY " + BaseCommentEntity.FieldSortCode + " DESC ";
            base.DbHelper.Fill(dataTable, commandText, list.ToArray());
            return dataTable;
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseCommentEntity baseCommentEntity)
        {
            sqlBuilder.SetValue(BaseCommentEntity.FieldCategoryCode, baseCommentEntity.CategoryCode, null);
            sqlBuilder.SetValue(BaseCommentEntity.FieldObjectId, baseCommentEntity.ObjectId, null);
            sqlBuilder.SetValue(BaseCommentEntity.FieldTitle, baseCommentEntity.Title, null);
            sqlBuilder.SetValue(BaseCommentEntity.FieldContents, baseCommentEntity.Contents, null);
            sqlBuilder.SetValue(BaseCommentEntity.FieldTargetURL, baseCommentEntity.TargetURL, null);
            sqlBuilder.SetValue(BaseCommentEntity.FieldIPAddress, baseCommentEntity.IPAddress, null);
            sqlBuilder.SetValue(BaseCommentEntity.FieldDeletionStateCode, baseCommentEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseCommentEntity.FieldEnabled, baseCommentEntity.Enabled, null);
            sqlBuilder.SetValue(BaseCommentEntity.FieldSortCode, baseCommentEntity.SortCode, null);
        }

        public int SetTableColumns()
        {
            int num = 0;
            string currentTableName = base.CurrentTableName;
            string tableName = "评论表";
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginDelete(BaseTableColumnsEntity.TableName);
            builder.SetWhere(BaseTableColumnsEntity.FieldTableCode, BaseCommentEntity.TableName);
            num += builder.EndDelete();
            num += this.SetTableColumns(currentTableName, tableName, "Id", "主键");
            num += this.SetTableColumns(currentTableName, tableName, "ParentId", "父亲节点主键");
            num += this.SetTableColumns(currentTableName, tableName, "CategoryId", "分类主键");
            num += this.SetTableColumns(currentTableName, tableName, "ObjectId", "唯一识别主键");
            num += this.SetTableColumns(currentTableName, tableName, "Title", "主题");
            num += this.SetTableColumns(currentTableName, tableName, "Content", "内容");
            num += this.SetTableColumns(currentTableName, tableName, "TargetURL", "消息的指向网页页面");
            num += this.SetTableColumns(currentTableName, tableName, "IPAddress", "IP地址");
            num += this.SetTableColumns(currentTableName, tableName, "DeletionStateCode", "删除标志");
            num += this.SetTableColumns(currentTableName, tableName, "Enabled", "有效");
            num += this.SetTableColumns(currentTableName, tableName, "Description", "描述");
            num += this.SetTableColumns(currentTableName, tableName, "SortCode", "排序码");
            num += this.SetTableColumns(currentTableName, tableName, "CreateOn", "创建日期");
            num += this.SetTableColumns(currentTableName, tableName, "CreateUserId", "创建用户主键");
            num += this.SetTableColumns(currentTableName, tableName, "CreateBy", "创建用户");
            num += this.SetTableColumns(currentTableName, tableName, "ModifiedOn", "修改日期");
            num += this.SetTableColumns(currentTableName, tableName, "ModifiedUserId", "修改用户主键");
            return (num + this.SetTableColumns(currentTableName, tableName, "ModifiedBy", "修改用户"));
        }

        public int SetWorked(string[] ids, int worked)
        {
            try
            {
                base.DbHelper.BeginTransaction();
                for (int i = 0; i < ids.Length; i++)
                {
                    string text2 = ids[i];
                }
                base.DbHelper.CommitTransaction();
            }
            catch (Exception exception)
            {
                base.DbHelper.RollbackTransaction();
                BaseExceptionManager.LogException(base.DbHelper, base.UserInfo, exception);
                throw exception;
            }
            return 0;
        }

        public int Update(BaseCommentEntity baseCommentEntity)
        {
            return this.UpdateEntity(baseCommentEntity);
        }

        public int Update(string id, string categoryCode, string title, string contents, bool worked, string priorityId, bool important)
        {
            BaseCommentEntity baseCommentEntity = new BaseCommentEntity {
                Id = id,
                ModifiedUserId = base.UserInfo.Id,
                CategoryCode = categoryCode,
                Title = title,
                Contents = contents
            };
            return this.Update(baseCommentEntity);
        }

        public int UpdateEntity(BaseCommentEntity baseCommentEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseCommentEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseCommentEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseCommentEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseCommentEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseCommentEntity.FieldId, baseCommentEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

