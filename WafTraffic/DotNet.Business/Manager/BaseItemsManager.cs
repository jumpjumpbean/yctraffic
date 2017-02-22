namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseItemsManager : BaseManager, IBaseManager
    {
        public BaseItemsManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseItemsEntity.TableName;
        }

        public BaseItemsManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseItemsManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseItemsManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseItemsManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseItemsManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseItemsEntity baseItemsEntity)
        {
            return this.AddEntity(baseItemsEntity);
        }

        public string Add(BaseItemsEntity itemsEntity, out string statusCode)
        {
            string str = string.Empty;
            if (this.Exists(BaseItemsEntity.FieldParentId, itemsEntity.ParentId, BaseItemsEntity.FieldCode, itemsEntity.Code))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return str;
            }
            if (this.Exists(BaseItemsEntity.FieldParentId, itemsEntity.ParentId, BaseItemsEntity.FieldFullName, itemsEntity.FullName))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return str;
            }
            str = this.AddEntity(itemsEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseItemsEntity baseItemsEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseItemsEntity);
        }

        public string AddEntity(BaseItemsEntity baseItemsEntity)
        {
            string s = string.Empty;
            if (baseItemsEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseItemsEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseItemsEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseItemsEntity.FieldId, baseItemsEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseItemsEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseItemsEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (string.IsNullOrEmpty(s))
                {
                    s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                }
                baseItemsEntity.Id = new int?(int.Parse(s));
                sqlBuilder.SetValue(BaseItemsEntity.FieldId, baseItemsEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseItemsEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseItemsEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseItemsEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseItemsEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseItemsEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseItemsEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseItemsEntity.FieldModifiedOn);
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
            BaseItemsEntity baseItemsEntity = new BaseItemsEntity();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BaseItemsEntity.FieldId, DataRowVersion.Original].ToString();
                    if ((id.Length > 0) && (baseItemsEntity.AllowDelete == 1))
                    {
                        num += this.Delete(id);
                    }
                }
                if ((row.RowState == DataRowState.Modified) && (row[BaseItemsEntity.FieldId, DataRowVersion.Original].ToString().Length > 0))
                {
                    baseItemsEntity.GetFrom(row);
                    if (baseItemsEntity.AllowEdit == 1)
                    {
                        num += this.UpdateEntity(baseItemsEntity);
                    }
                    else
                    {
                        num += this.SetProperty(baseItemsEntity.Id, BaseItemsEntity.FieldSortCode, baseItemsEntity.SortCode);
                    }
                }
                if (row.RowState == DataRowState.Added)
                {
                    this.GetFrom(row);
                    num += (this.AddEntity(baseItemsEntity).Length > 0) ? 1 : 0;
                }
                if (row.RowState != DataRowState.Unchanged)
                {
                    DataRowState rowState = row.RowState;
                }
            }
            return num;
        }

        public void CreateTable(string tableName, out string statusCode)
        {
            statusCode = StatusCode.Error.ToString();
            string commandText = string.Empty;
            if (base.DbHelper.CurrentDbType == CurrentDbType.SqlServer)
            {
                commandText = "\r\nCREATE TABLE [dbo].[#tableName#](\r\n\t[Id] [int] IDENTITY(1,1) NOT NULL,\r\n\t[ParentId] [int] NULL,\r\n\t[ItemCode] [nvarchar](40) NULL,\r\n\t[ItemName] [nvarchar](100)  NULL,\r\n\t[ItemValue] [nvarchar](100)  NULL,\r\n\t[Enabled] [int] NOT NULL CONSTRAINT [DF_#tableName#_Enabled]  DEFAULT ((1)),\r\n\t[AllowEdit] [int] NOT NULL CONSTRAINT [DF_#tableName#_AllowEdit]  DEFAULT ((1)),\r\n\t[AllowDelete] [int] NOT NULL CONSTRAINT [DF_#tableName#_AllowDelete]  DEFAULT ((1)),\r\n\t[IsPublic] [int] NOT NULL CONSTRAINT [DF_#tableName#_IsPublic]  DEFAULT ((1)),\r\n\t[DeletionStateCode] [int] NOT NULL CONSTRAINT [DF_#tableName#_DeleteMark]  DEFAULT ((0)),\r\n\t[SortCode] [int] NULL,\r\n\t[Description] [nvarchar](200)  NULL,\r\n\t[CreateOn] [smalldatetime] NOT NULL CONSTRAINT [DF_#tableName#_CreateOn]  DEFAULT (GETDATE()),\r\n\t[CreateUserId] [nvarchar](20)  NULL,\r\n\t[CreateBy] [nvarchar](20)  NULL,\r\n\t[ModifiedOn] [smalldatetime] NOT NULL CONSTRAINT [DF_#tableName#_ModifiedOn]  DEFAULT (GETDATE()),\r\n\t[ModifiedUserId] [nvarchar](20)  NULL,\r\n\t[ModifiedBy] [nvarchar](20)  NULL,\r\n CONSTRAINT [PK_#tableName#] PRIMARY KEY CLUSTERED \r\n(\r\n\t[Id] ASC\r\n)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]";
                commandText = commandText.Replace("#tableName#", tableName);
                base.DbHelper.ExecuteNonQuery(commandText);
                statusCode = StatusCode.OK.ToString();
            }
            else if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
            {
                commandText = "\r\ncreate table #tableName# \r\n(\r\n   Id                 INT                  not null,\r\n   ParentId           INT,\r\n   ItemCode           NVARCHAR2(200),\r\n   ItemName           NVARCHAR2(200)       not null,\r\n   ItemValue          NVARCHAR2(200),\r\n   AllowEdit          INT                  default 1 not null,\r\n   AllowDelete        INT                  default 1 not null,\r\n   IsPublic           INT                  default 1 not null,\r\n   Enabled            INT                  default 1 not null,\r\n   DeletionStateCode  INT                  default 0,\r\n   SortCode           INT,\r\n   Description        NVARCHAR2(800),\r\n   CreateOn           DATE,\r\n   CreateUserId       NVARCHAR2(50),\r\n   CreateBy           NVARCHAR2(50),\r\n   ModifiedOn         DATE,\r\n   ModifiedUserId     NVARCHAR2(50),\r\n   ModifiedBy         NVARCHAR2(50),\r\n   constraint PK_#tableName# primary key (Id)\r\n)";
                commandText = commandText.Replace("#tableName#", tableName);
                base.DbHelper.ExecuteNonQuery(commandText);
                commandText = "CREATE SEQUENCE SEQ_" + tableName + " MINVALUE 1 MAXVALUE 9999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20";
                base.DbHelper.ExecuteNonQuery(commandText);
                statusCode = StatusCode.OK.ToString();
            }
        }

        public int Delete(int id)
        {
            return this.Delete(BaseItemsEntity.FieldId, id);
        }

        public BaseItemsEntity GetEntity(int id)
        {
            return this.GetEntity(id.ToString());
        }

        public BaseItemsEntity GetEntity(string id)
        {
            return new BaseItemsEntity(this.GetDT(BaseItemsEntity.FieldId, id));
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseItemsEntity baseItemsEntity)
        {
            sqlBuilder.SetValue(BaseItemsEntity.FieldParentId, baseItemsEntity.ParentId, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldCode, baseItemsEntity.Code, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldFullName, baseItemsEntity.FullName, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldTargetTable, baseItemsEntity.TargetTable, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldIsTree, baseItemsEntity.IsTree, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldUseItemCode, baseItemsEntity.UseItemCode, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldUseItemName, baseItemsEntity.UseItemName, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldUseItemValue, baseItemsEntity.UseItemValue, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldAllowEdit, baseItemsEntity.AllowEdit, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldAllowDelete, baseItemsEntity.AllowDelete, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldDeletionStateCode, baseItemsEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldDescription, baseItemsEntity.Description, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldEnabled, baseItemsEntity.Enabled, null);
            sqlBuilder.SetValue(BaseItemsEntity.FieldSortCode, baseItemsEntity.SortCode, null);
        }

        public int Update(BaseItemsEntity baseItemsEntity)
        {
            return this.UpdateEntity(baseItemsEntity);
        }

        public int Update(BaseItemsEntity itemsEntity, out string statusCode)
        {
            int num = 0;
            if (this.Exists(BaseItemsEntity.FieldParentId, itemsEntity.ParentId, BaseItemsEntity.FieldCode, itemsEntity.Code, itemsEntity.Id))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return num;
            }
            if (this.Exists(BaseItemsEntity.FieldParentId, itemsEntity.ParentId, BaseItemsEntity.FieldFullName, itemsEntity.FullName, itemsEntity.Id))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return num;
            }
            num = this.UpdateEntity(itemsEntity);
            if (num == 1)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseItemsEntity baseItemsEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseItemsEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseItemsEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseItemsEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseItemsEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseItemsEntity.FieldId, baseItemsEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

