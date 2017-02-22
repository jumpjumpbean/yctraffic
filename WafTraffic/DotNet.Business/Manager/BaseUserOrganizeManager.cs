namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseUserOrganizeManager : BaseManager, IBaseManager
    {
        public BaseUserOrganizeManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseUserOrganizeEntity.TableName;
        }

        public BaseUserOrganizeManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseUserOrganizeManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseUserOrganizeManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseUserOrganizeManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseUserOrganizeManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseUserOrganizeEntity baseUserOrganizeEntity)
        {
            return this.AddEntity(baseUserOrganizeEntity);
        }

        public string Add(BaseUserOrganizeEntity userOrganizeEntity, out string statusCode)
        {
            string str = string.Empty;
            string[] names = new string[] { BaseUserOrganizeEntity.FieldDeletionStateCode, BaseUserOrganizeEntity.FieldEnabled, BaseUserOrganizeEntity.FieldCompanyId, BaseUserOrganizeEntity.FieldDepartmentId, BaseUserOrganizeEntity.FieldWorkgroupId, BaseUserOrganizeEntity.FieldRoleId };
            object[] values = new object[] { 0, 1, userOrganizeEntity.UserId, userOrganizeEntity.CompanyId, userOrganizeEntity.DepartmentId, userOrganizeEntity.WorkgroupId, userOrganizeEntity.RoleId };
            if (this.Exists(names, values))
            {
                statusCode = StatusCode.Exist.ToString();
                return str;
            }
            str = this.AddEntity(userOrganizeEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseUserOrganizeEntity baseUserOrganizeEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseUserOrganizeEntity);
        }

        public string AddEntity(BaseUserOrganizeEntity baseUserOrganizeEntity)
        {
            string sequence = string.Empty;
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseUserOrganizeEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldId, baseUserOrganizeEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseUserOrganizeEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseUserOrganizeEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!baseUserOrganizeEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(sequence))
                    {
                        sequence = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseUserOrganizeEntity.Id = new int?(int.Parse(sequence));
                }
                sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldId, baseUserOrganizeEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseUserOrganizeEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserOrganizeEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserOrganizeEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return sequence;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseUserOrganizeEntity.FieldId, id);
        }

        public BaseUserOrganizeEntity GetEntity(int id)
        {
            return new BaseUserOrganizeEntity(this.GetDT(BaseUserOrganizeEntity.FieldId, id));
        }

        public DataTable GetUserOrganizeDT(string userId)
        {
            string commandText = " SELECT Base_UserOrganize.*      , Base_Role.Code AS RoleCode      , Base_Role.RealName AS RoleName      , Base_Organize1.FullName AS CompanyName      , Base_Organize2.FullName AS DepartmentName      , Base_Organize3.FullName AS WorkgroupName  FROM Base_UserOrganize LEFT OUTER JOIN      Base_Organize Base_Organize1 ON Base_UserOrganize.CompanyId = Base_Organize1.Id LEFT OUTER JOIN      Base_Organize Base_Organize2 ON Base_UserOrganize.DepartmentId = Base_Organize2.Id LEFT OUTER JOIN      Base_Organize Base_Organize3 ON Base_UserOrganize.WorkgroupId = Base_Organize3.Id LEFT OUTER JOIN      Base_Role ON Base_UserOrganize.RoleId = Base_Role.Id  WHERE UserId = '" + userId + "'";
            return base.DbHelper.Fill(commandText);
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseUserOrganizeEntity baseUserOrganizeEntity)
        {
            sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldUserId, baseUserOrganizeEntity.UserId, null);
            sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldCompanyId, baseUserOrganizeEntity.CompanyId, null);
            sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldDepartmentId, baseUserOrganizeEntity.DepartmentId, null);
            sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldWorkgroupId, baseUserOrganizeEntity.WorkgroupId, null);
            sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldRoleId, baseUserOrganizeEntity.RoleId, null);
            sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldEnabled, baseUserOrganizeEntity.Enabled, null);
            sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldDescription, baseUserOrganizeEntity.Description, null);
            sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldDeletionStateCode, baseUserOrganizeEntity.DeletionStateCode, null);
        }

        public int Update(BaseUserOrganizeEntity baseUserOrganizeEntity)
        {
            return this.UpdateEntity(baseUserOrganizeEntity);
        }

        public int UpdateEntity(BaseUserOrganizeEntity baseUserOrganizeEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseUserOrganizeEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserOrganizeEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserOrganizeEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseUserOrganizeEntity.FieldId, baseUserOrganizeEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

