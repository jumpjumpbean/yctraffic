namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseRoleManager : BaseManager, IBaseManager
    {
        public BaseRoleManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseRoleEntity.TableName;
        }

        public BaseRoleManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseRoleManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseRoleManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseRoleManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseRoleManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseRoleEntity baseRoleEntity)
        {
            return this.AddEntity(baseRoleEntity);
        }

        public string Add(BaseRoleEntity roleEntity, out string statusCode)
        {
            string str = string.Empty;
            string[] names = new string[] { BaseRoleEntity.FieldSystemId, BaseRoleEntity.FieldRealName, BaseRoleEntity.FieldDeletionStateCode };
            object[] values = new object[] { roleEntity.SystemId, roleEntity.RealName, 0 };
            if (this.Exists(names, values))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return str;
            }
            str = this.AddEntity(roleEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseRoleEntity baseRoleEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseRoleEntity);
        }

        public string AddEntity(BaseRoleEntity baseRoleEntity)
        {
            string s = string.Empty;
            if (baseRoleEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseRoleEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(BaseRoleEntity.TableName, BaseRoleEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseRoleEntity.FieldId, baseRoleEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseRoleEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseRoleEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!baseRoleEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseRoleEntity.Id = new int?(int.Parse(s));
                }
                sqlBuilder.SetValue(BaseRoleEntity.FieldId, baseRoleEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseRoleEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseRoleEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseRoleEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseRoleEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseRoleEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseRoleEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseRoleEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public int BatchDelete(string[] ids)
        {
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                num += this.Delete(ids[i]);
            }
            return num;
        }

        public int BatchMoveTo(string[] ids, string targetOrganizeId)
        {
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                num += this.MoveTo(ids[i], targetOrganizeId);
            }
            return num;
        }

        public int BatchSave(List<BaseRoleEntity> roleEntites)
        {
            int num = 0;
            foreach (BaseRoleEntity entity in roleEntites)
            {
                num += this.UpdateEntity(entity);
            }
            return num;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseRoleEntity.FieldId, id);
        }

        public int Delete(string id)
        {
            int num = 0;
            num += DbLogic.Delete(base.DbHelper, BaseUserRoleEntity.TableName, BaseUserRoleEntity.FieldRoleId, id);
            return (num + DbLogic.Delete(base.DbHelper, BaseRoleEntity.TableName, id));
        }

        public DataTable GetDTBySystem(string systemId)
        {
            string commandText = " SELECT " + BaseRoleEntity.TableName + ".*, (SELECT COUNT(*) FROM " + BaseUserRoleEntity.TableName + " WHERE (Enabled = 1) AND (RoleId = " + BaseRoleEntity.TableName + ".Id)) AS StaffCount  FROM " + BaseRoleEntity.TableName + " WHERE " + BaseRoleEntity.FieldSystemId + " = '" + systemId + "' ORDER BY " + BaseRoleEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public BaseRoleEntity GetEntity(int? id)
        {
            return this.GetEntity(id.ToString());
        }

        public BaseRoleEntity GetEntity(string id)
        {
            return new BaseRoleEntity(this.GetDT(BaseRoleEntity.FieldId, id));
        }

        public int MoveTo(string id, string targetSystemId)
        {
            return this.SetProperty(id, BaseRoleEntity.FieldSystemId, targetSystemId);
        }

        public int ResetSortCode(string systemId)
        {
            int num = 0;
            DataTable dTBySystem = this.GetDTBySystem(systemId);
            string id = string.Empty;
            string[] batchSequence = new BaseSequenceManager(base.DbHelper).GetBatchSequence(BaseRoleEntity.TableName, dTBySystem.Rows.Count);
            int index = 0;
            foreach (DataRow row in dTBySystem.Rows)
            {
                id = row[BaseRoleEntity.FieldId].ToString();
                num += this.SetProperty(id, BaseRoleEntity.FieldSortCode, batchSequence[index]);
                index++;
            }
            return num;
        }

        public DataTable Search(string systemId, string searchValue)
        {
            searchValue = BaseBusinessLogic.GetSearchString(searchValue);
            if (string.IsNullOrEmpty(searchValue))
            {
                if (string.IsNullOrEmpty(systemId))
                {
                    return this.GetDT();
                }
                return this.GetDTBySystem(systemId);
            }
            string commandText = " SELECT " + BaseRoleEntity.TableName + ".*, (SELECT COUNT(*) FROM " + BaseUserRoleEntity.TableName + " WHERE (Enabled = 1) AND (RoleId = " + BaseRoleEntity.TableName + ".Id)) AS StaffCount  FROM " + BaseRoleEntity.TableName + " WHERE (" + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldRealName + " LIKE '" + searchValue + "' OR " + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldDescription + " LIKE '" + searchValue + "')";
            if (!string.IsNullOrEmpty(systemId))
            {
                string str2 = commandText;
                commandText = str2 + " AND (" + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldSystemId + " = '" + systemId + "')";
            }
            commandText = commandText + " ORDER BY " + BaseRoleEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseRoleEntity baseRoleEntity)
        {
            sqlBuilder.SetValue(BaseRoleEntity.FieldSystemId, baseRoleEntity.SystemId, null);
            sqlBuilder.SetValue(BaseRoleEntity.FieldOrganizeId, baseRoleEntity.OrganizeId, null);
            sqlBuilder.SetValue(BaseRoleEntity.FieldCode, baseRoleEntity.Code, null);
            sqlBuilder.SetValue(BaseRoleEntity.FieldRealName, baseRoleEntity.RealName, null);
            sqlBuilder.SetValue(BaseRoleEntity.FieldCategory, baseRoleEntity.Category, null);
            sqlBuilder.SetValue(BaseRoleEntity.FieldAllowEdit, baseRoleEntity.AllowEdit, null);
            sqlBuilder.SetValue(BaseRoleEntity.FieldAllowDelete, baseRoleEntity.AllowDelete, null);
            sqlBuilder.SetValue(BaseRoleEntity.FieldSortCode, baseRoleEntity.SortCode, null);
            sqlBuilder.SetValue(BaseRoleEntity.FieldDeletionStateCode, baseRoleEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseRoleEntity.FieldEnabled, baseRoleEntity.Enabled, null);
            sqlBuilder.SetValue(BaseRoleEntity.FieldDescription, baseRoleEntity.Description, null);
        }

        public int Update(BaseRoleEntity baseRoleEntity)
        {
            return this.UpdateEntity(baseRoleEntity);
        }

        public int Update(BaseRoleEntity roleEntity, out string statusCode)
        {
            int num = 0;
            if (DbLogic.IsModifed(base.DbHelper, BaseRoleEntity.TableName, roleEntity.Id, roleEntity.ModifiedUserId, roleEntity.ModifiedOn))
            {
                statusCode = StatusCode.ErrorChanged.ToString();
                return num;
            }
            string[] names = new string[] { BaseRoleEntity.FieldSystemId, BaseRoleEntity.FieldRealName, BaseRoleEntity.FieldDeletionStateCode };
            object[] values = new object[] { roleEntity.SystemId, roleEntity.RealName, 0 };
            if (this.Exists(names, values, roleEntity.Id))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return num;
            }
            num = this.UpdateEntity(roleEntity);
            if (num == 1)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseRoleEntity baseRoleEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BaseRoleEntity.TableName);
            this.SetEntity(sqlBuilder, baseRoleEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseRoleEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseRoleEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseRoleEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseRoleEntity.FieldId, baseRoleEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

