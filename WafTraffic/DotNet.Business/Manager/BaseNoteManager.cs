namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    public class BaseNoteManager : BaseManager
    {
        public BaseNoteManager()
        {
            base.CurrentTableName = BaseNoteEntity.TableName;
        }

        public BaseNoteManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseNoteManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseNoteManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public string Add(BaseNoteEntity noteEntity)
        {
            return this.AddEntity(noteEntity);
        }

        public string AddEntity(BaseNoteEntity noteEntity)
        {
            string targetValue = Guid.NewGuid().ToString();
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginInsert(BaseNoteEntity.TableName);
            sqlBuilder.SetValue(BaseNoteEntity.FieldId, targetValue, null);
            this.SetEntity(sqlBuilder, noteEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseNoteEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetDBNow(BaseNoteEntity.FieldCreateOn);
            }
            sqlBuilder.SetDBNow(BaseNoteEntity.FieldModifiedOn);
            if (sqlBuilder.EndInsert() <= 0)
            {
                return string.Empty;
            }
            return targetValue;
        }

        public DataTable GetDTByUser(string userId, bool deletionStateCode)
        {
            string commandText = string.Concat(new object[] { " SELECT *  FROM ", BaseNoteEntity.TableName, " WHERE (", BaseNoteEntity.FieldCreateUserId, " = ", base.DbHelper.GetParameter(BaseNoteEntity.FieldCreateUserId), " AND ", BaseNoteEntity.FieldDeletionStateCode, " = ", deletionStateCode ? 1 : 0, ") ORDER BY ", BaseNoteEntity.FieldSortCode });
            string[] targetFileds = new string[1];
            object[] targetValues = new object[1];
            targetFileds[0] = BaseNoteEntity.FieldCreateUserId;
            targetValues[0] = userId;
            DataTable dataTable = new DataTable(BaseNoteEntity.TableName);
            base.DbHelper.Fill(dataTable, commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        public DataTable GetDTByUser(string userId, bool deletionStateCode, string searchValue)
        {
            if (searchValue.Length == 0)
            {
                return this.GetDTByUser(userId, deletionStateCode);
            }
            searchValue = BaseBusinessLogic.GetSearchString(searchValue);
            DataTable dataTable = new DataTable(BaseNoteEntity.TableName);
            string commandText = string.Concat(new object[] { 
                " SELECT *  FROM ", BaseNoteEntity.TableName, " WHERE ((", BaseNoteEntity.FieldTitle, " LIKE ", base.DbHelper.GetParameter(BaseNoteEntity.FieldTitle), " ) OR (", BaseNoteEntity.FieldContent, " LIKE ", base.DbHelper.GetParameter(BaseNoteEntity.FieldContent), " ) OR (", BaseNoteEntity.FieldCategoryFullName, " LIKE ", base.DbHelper.GetParameter(BaseNoteEntity.FieldCategoryFullName), " ) OR (CONVERT (NVARCHAR(10), ", BaseNoteEntity.FieldCreateOn, 
                ", 20) LIKE ", base.DbHelper.GetParameter(BaseNoteEntity.FieldCreateOn), " )) AND (", BaseNoteEntity.FieldCreateUserId, " = ", base.DbHelper.GetParameter(BaseNoteEntity.FieldCreateUserId), " AND ", BaseNoteEntity.FieldDeletionStateCode, " = ", deletionStateCode ? 1 : 0, ") ORDER BY ", BaseNoteEntity.FieldSortCode
             });
            string[] targetFileds = new string[5];
            object[] targetValues = new object[5];
            targetFileds[0] = BaseNoteEntity.FieldTitle;
            targetValues[0] = searchValue;
            targetFileds[1] = BaseNoteEntity.FieldContent;
            targetValues[1] = searchValue;
            targetFileds[2] = BaseNoteEntity.FieldCategoryFullName;
            targetValues[2] = searchValue;
            targetFileds[3] = BaseNoteEntity.FieldCreateOn;
            targetValues[3] = searchValue;
            targetFileds[4] = BaseNoteEntity.FieldCreateUserId;
            targetValues[4] = userId;
            base.DbHelper.Fill(dataTable, commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            return dataTable;
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseNoteEntity noteEntity)
        {
            sqlBuilder.SetValue(BaseNoteEntity.FieldTitle, noteEntity.Title, null);
            sqlBuilder.SetValue(BaseNoteEntity.FieldCategoryId, noteEntity.CategoryId, null);
            sqlBuilder.SetValue(BaseNoteEntity.FieldCategoryFullName, noteEntity.CategoryFullName, null);
            sqlBuilder.SetValue(BaseNoteEntity.FieldColor, noteEntity.Color, null);
            sqlBuilder.SetValue(BaseNoteEntity.FieldContent, noteEntity.Content, null);
            sqlBuilder.SetValue(BaseNoteEntity.FieldEnabled, noteEntity.Enabled, null);
            sqlBuilder.SetValue(BaseNoteEntity.FieldImportant, noteEntity.Important, null);
        }

        public int Update(BaseNoteEntity noteEntity)
        {
            return this.UpdateEntity(noteEntity);
        }

        public int UpdateEntity(BaseNoteEntity noteEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BaseNoteEntity.TableName);
            this.SetEntity(sqlBuilder, noteEntity);
            sqlBuilder.SetValue(BaseNoteEntity.FieldModifiedUserId, base.UserInfo.Id, null);
            sqlBuilder.SetDBNow(BaseNoteEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseNoteEntity.FieldId, noteEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

