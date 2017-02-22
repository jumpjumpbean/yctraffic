namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class BaseStaffManager : BaseManager, IBaseManager
    {
        public BaseStaffManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseStaffEntity.TableName;
        }

        public BaseStaffManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseStaffManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseStaffManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseStaffManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseStaffManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseStaffEntity staffEntity)
        {
            return this.AddEntity(staffEntity);
        }

        public string Add(BaseStaffEntity staffEntity, out string statusCode)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(staffEntity.UserName) && this.Exists(BaseStaffEntity.FieldUserName, staffEntity.UserName, BaseStaffEntity.FieldDeletionStateCode, 0))
            {
                statusCode = StatusCode.ErrorUserExist.ToString();
                return str;
            }
            if (!string.IsNullOrEmpty(staffEntity.Code) && this.Exists(BaseStaffEntity.FieldCode, staffEntity.Code, BaseStaffEntity.FieldDeletionStateCode, 0))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return str;
            }
            str = this.AddEntity(staffEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseStaffEntity staffEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(staffEntity);
        }

        public string Add(string departmentId, string userName, string code, string fullName, bool isVirtual, bool isDimission, bool enabled, string description)
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            string sequence = new BaseSequenceManager(base.DbHelper).GetSequence(BaseStaffEntity.TableName);
            builder.BeginInsert(BaseStaffEntity.TableName);
            builder.SetValue(BaseStaffEntity.FieldId, sequence, null);
            builder.SetValue(BaseStaffEntity.FieldCode, code, null);
            builder.SetValue(BaseStaffEntity.FieldUserName, userName, null);
            builder.SetValue(BaseStaffEntity.FieldRealName, fullName, null);
            builder.SetValue(BaseStaffEntity.FieldDepartmentId, departmentId, null);
            builder.SetValue(BaseStaffEntity.FieldSortCode, sequence, null);
            builder.SetValue(BaseStaffEntity.FieldEnabled, enabled ? 1 : 0, null);
            if (base.UserInfo != null)
            {
                builder.SetValue(BaseStaffEntity.FieldCreateUserId, base.UserInfo.Id, null);
                builder.SetValue(BaseStaffEntity.FieldCreateOn, base.UserInfo.RealName, null);
            }
            builder.SetDBNow(BaseStaffEntity.FieldCreateOn);
            return ((builder.EndInsert() > 0) ? sequence : string.Empty);
        }

        public string AddEntity(BaseStaffEntity staffEntity)
        {
            string s = string.Empty;
            if (staffEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                staffEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(BaseStaffEntity.TableName, BaseStaffEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseStaffEntity.FieldId, staffEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseStaffEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseStaffEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!staffEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    staffEntity.Id = new int?(int.Parse(s));
                }
                sqlBuilder.SetValue(BaseStaffEntity.FieldId, staffEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, staffEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseStaffEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseStaffEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseStaffEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseStaffEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseStaffEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseStaffEntity.FieldModifiedOn);
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

        public override int BatchSave(DataTable dataTable)
        {
            int num = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BaseStaffEntity.FieldId, DataRowVersion.Original].ToString();
                    if (id.Length > 0)
                    {
                        num += this.DeleteEntity(id);
                    }
                }
                if ((row.RowState == DataRowState.Modified) && (row[BaseStaffEntity.FieldId, DataRowVersion.Original].ToString().Length > 0))
                {
                    BaseStaffEntity staffEntity = new BaseStaffEntity(row);
                    num += this.UpdateEntity(staffEntity);
                }
                if (row.RowState == DataRowState.Added)
                {
                    BaseStaffEntity entity2 = new BaseStaffEntity(row);
                    num += (this.AddEntity(entity2).Length > 0) ? 1 : 0;
                }
                if (row.RowState != DataRowState.Unchanged)
                {
                    DataRowState rowState = row.RowState;
                }
            }
            base.ReturnStatusCode = StatusCode.OK.ToString();
            return num;
        }

        public BaseUserInfo ConvertToUserInfo(BaseStaffEntity staffEntity, BaseUserInfo userInfo)
        {
            userInfo.StaffId = staffEntity.Id.ToString();
            userInfo.Code = staffEntity.Code;
            if (string.IsNullOrEmpty(userInfo.UserName))
            {
                userInfo.UserName = staffEntity.UserName;
            }
            if (string.IsNullOrEmpty(userInfo.RealName))
            {
                userInfo.RealName = staffEntity.RealName;
            }
            userInfo.CompanyId = staffEntity.CompanyId;
            userInfo.DepartmentId = staffEntity.DepartmentId;
            userInfo.WorkgroupId = staffEntity.WorkgroupId;
            return userInfo;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseStaffEntity.FieldId, id);
        }

        public int Delete(string id)
        {
            int num = 0;
            BaseStaffEntity entity = this.GetEntity(id);
            num += DbLogic.Delete(base.DbHelper, BaseUserRoleEntity.TableName, BaseUserRoleEntity.FieldUserId, entity.UserId);
            BaseUserManager manager = new BaseUserManager(base.DbHelper, base.UserInfo);
            num += manager.DeleteEntity(entity.UserId);
            return (num + DbLogic.Delete(base.DbHelper, BaseStaffEntity.TableName, BaseStaffEntity.FieldId, id));
        }

        public int DeleteUser(string staffId)
        {
            int num = 0;
            string str = this.GetEntity(staffId).UserId.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                BaseUserManager manager = new BaseUserManager(base.DbHelper, base.UserInfo);
                num += manager.SetDeleted(str);
            }
            return (num + this.SetProperty(staffId, BaseStaffEntity.FieldUserId, null));
        }

        public DataTable GetAddressDT()
        {
            return this.GetAddressDT(string.Empty, string.Empty);
        }

        public DataTable GetAddressDT(string organizeId)
        {
            return this.GetAddressDT(organizeId, string.Empty);
        }

        public DataTable GetAddressDT(string organizeId, string searchValue)
        {
            if (organizeId == null)
            {
                organizeId = string.Empty;
            }
            searchValue = BaseBusinessLogic.GetSearchString(searchValue);
            string commandText = " SELECT " + BaseStaffEntity.TableName + ".* ," + BaseOrganizeEntity.TableName + "A." + BaseOrganizeEntity.FieldCode + " AS CompanyCode ," + BaseOrganizeEntity.TableName + "A." + BaseOrganizeEntity.FieldFullName + " AS CompanyName ," + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldCode + " AS DepartmentCode ," + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldFullName + " AS DepartmentName ,Base_ItemDetails_Duty." + BaseItemDetailsEntity.FieldItemName + " AS DutyName ,OT.RoleName  FROM " + BaseStaffEntity.TableName + "      LEFT OUTER JOIN " + BaseOrganizeEntity.TableName + " " + BaseOrganizeEntity.TableName + "A ON " + BaseOrganizeEntity.TableName + "A." + BaseOrganizeEntity.FieldId + " = " + BaseStaffEntity.FieldCompanyId + "      LEFT OUTER JOIN " + BaseOrganizeEntity.TableName + " " + BaseOrganizeEntity.TableName + "B ON " + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldId + " = " + BaseStaffEntity.FieldDepartmentId + "      LEFT OUTER JOIN Base_ItemDetails_Duty ON Base_ItemDetails_Duty." + BaseItemDetailsEntity.FieldId + " = " + BaseStaffEntity.FieldDutyId + "      LEFT OUTER JOIN (SELECT " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + "," + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldRealName + " AS RoleName  FROM " + BaseUserEntity.TableName + "," + BaseRoleEntity.TableName + " WHERE " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldRoleId + " = " + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldId + ") OT                   ON " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldId + " = OT.Id ";
            if (string.IsNullOrEmpty(organizeId))
            {
                string str2 = commandText;
                commandText = str2 + " WHERE ((" + BaseOrganizeEntity.TableName + "A." + BaseOrganizeEntity.FieldIsInnerOrganize + " = 1) OR (" + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldIsInnerOrganize + " =1)) ";
            }
            else
            {
                string str3 = commandText;
                commandText = str3 + " WHERE (" + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldCompanyId + " = '" + organizeId + "' OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldDepartmentId + " = '" + organizeId + "') ";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                string str4 = commandText;
                string str5 = str4 + " AND (" + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldUserName + " LIKE '" + searchValue + "'";
                string str6 = str5 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldRealName + " LIKE '" + searchValue + "'";
                string str7 = str6 + " OR " + BaseOrganizeEntity.TableName + "A." + BaseOrganizeEntity.FieldFullName + " LIKE '" + searchValue + "'";
                string str8 = str7 + " OR " + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldFullName + " LIKE '" + searchValue + "'";
                string str9 = str8 + " OR Base_ItemDetails_Duty." + BaseItemDetailsEntity.FieldItemName + " LIKE '" + searchValue + "'";
                string str10 = str9 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldOfficePhone + " LIKE '" + searchValue + "'";
                string str11 = str10 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldMobile + " LIKE '" + searchValue + "'";
                string str12 = str11 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldShortNumber + " LIKE '" + searchValue + "'";
                string str13 = str12 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldOICQ + " LIKE '" + searchValue + "'";
                string str14 = str13 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldEmail + " LIKE '" + searchValue + "'";
                commandText = (str14 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldDescription + " LIKE '" + searchValue + "'") + " OR OT.RoleName LIKE '" + searchValue + "')";
            }
            string str15 = commandText;
            commandText = str15 + " ORDER BY " + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldSortCode + " ," + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public DataTable GetAddressPageDT(out int recordCount, string organizeId, string searchValue, int pageSize, int currentPage)
        {
            if (organizeId == null)
            {
                organizeId = string.Empty;
            }
            searchValue = BaseBusinessLogic.GetSearchString(searchValue);
            string sqlQuery = " SELECT " + BaseStaffEntity.TableName + ".* ," + BaseOrganizeEntity.TableName + "A." + BaseOrganizeEntity.FieldCode + " AS CompanyCode ," + BaseOrganizeEntity.TableName + "A." + BaseOrganizeEntity.FieldFullName + " AS CompanyName ," + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldCode + " AS DepartmentCode ," + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldFullName + " AS DepartmentName ,Base_ItemDetails_Duty." + BaseItemDetailsEntity.FieldItemName + " AS DutyName ,OT.RoleName  FROM " + BaseStaffEntity.TableName + "      LEFT OUTER JOIN " + BaseOrganizeEntity.TableName + " " + BaseOrganizeEntity.TableName + "A ON " + BaseOrganizeEntity.TableName + "A." + BaseOrganizeEntity.FieldId + " = " + BaseStaffEntity.FieldCompanyId + "      LEFT OUTER JOIN " + BaseOrganizeEntity.TableName + " " + BaseOrganizeEntity.TableName + "B ON " + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldId + " = " + BaseStaffEntity.FieldDepartmentId + "      LEFT OUTER JOIN Base_ItemDetails_Duty ON Base_ItemDetails_Duty." + BaseItemDetailsEntity.FieldId + " = " + BaseStaffEntity.FieldDutyId + "      LEFT OUTER JOIN (SELECT " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + "," + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldRealName + " AS RoleName  FROM " + BaseUserEntity.TableName + "," + BaseRoleEntity.TableName + " WHERE " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldRoleId + " = " + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldId + ") OT                   ON " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldId + " = OT.Id ";
            if (string.IsNullOrEmpty(organizeId))
            {
                string str3 = sqlQuery;
                sqlQuery = str3 + " WHERE ((" + BaseOrganizeEntity.TableName + "A." + BaseOrganizeEntity.FieldIsInnerOrganize + " = 1) OR (" + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldIsInnerOrganize + " =1)) ";
            }
            else
            {
                string str4 = sqlQuery;
                sqlQuery = str4 + " WHERE (" + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldCompanyId + " = '" + organizeId + "' OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldDepartmentId + " = '" + organizeId + "') ";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                string str5 = sqlQuery;
                string str6 = str5 + " AND (" + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldUserName + " LIKE '" + searchValue + "'";
                string str7 = str6 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldRealName + " LIKE '" + searchValue + "'";
                string str8 = str7 + " OR " + BaseOrganizeEntity.TableName + "A." + BaseOrganizeEntity.FieldFullName + " LIKE '" + searchValue + "'";
                string str9 = str8 + " OR " + BaseOrganizeEntity.TableName + "B." + BaseOrganizeEntity.FieldFullName + " LIKE '" + searchValue + "'";
                string str10 = str9 + " OR Base_ItemDetails_Duty." + BaseItemDetailsEntity.FieldItemName + " LIKE '" + searchValue + "'";
                string str11 = str10 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldOfficePhone + " LIKE '" + searchValue + "'";
                string str12 = str11 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldMobile + " LIKE '" + searchValue + "'";
                string str13 = str12 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldShortNumber + " LIKE '" + searchValue + "'";
                string str14 = str13 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldOICQ + " LIKE '" + searchValue + "'";
                string str15 = str14 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldEmail + " LIKE '" + searchValue + "'";
                sqlQuery = (str15 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldDescription + " LIKE '" + searchValue + "'") + " OR OT.RoleName LIKE '" + searchValue + "')";
            }
            string orderby = string.Empty;
            string str16 = BaseSystemInfo.BusinessDbType.ToString().ToLower();
            if (str16 != null)
            {
                if (!(str16 == "sqlserver"))
                {
                    if (str16 == "db2")
                    {
                        orderby = BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldSortCode;
                        goto Label_07A3;
                    }
                }
                else
                {
                    orderby = BaseStaffEntity.FieldSortCode;
                    goto Label_07A3;
                }
            }
            orderby = BaseStaffEntity.FieldSortCode;
        Label_07A3:
            return this.GetDTByPage(out recordCount, currentPage, pageSize, sqlQuery, orderby);
        }

        public string GetCategoryCount(IDbHelper dbHelper, string categoryId, string organizeCode, string categoryField)
        {
            string commandText = string.Empty;
            string str2 = string.Empty;
            string[] targetFileds = new string[3];
            object[] targetValues = new object[3];
            commandText = " SELECT COUNT(Id) AS STAFFCOUNT FROM " + BaseStaffEntity.TableName + " WHERE (" + categoryField + " = ?) AND (ENABLED = 1) AND (ISDIMISSION <> 1) AND (ISSTAFF = 1) AND (DepartmentId IN (SELECT Id FROM " + BaseOrganizeEntity.TableName + " WHERE (LEFT(CODE, LEN(?)) = ?))) ";
            targetFileds[0] = categoryField;
            targetFileds[1] = BaseOrganizeEntity.FieldCode;
            targetFileds[2] = organizeCode;
            targetValues[0] = categoryId;
            targetValues[1] = organizeCode;
            targetValues[2] = organizeCode;
            object obj2 = dbHelper.ExecuteScalar(commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            if (obj2 != null)
            {
                str2 = obj2.ToString();
            }
            return str2;
        }

        public DataTable GetChildrenStaffs(string organizeId)
        {
            BaseOrganizeManager manager = new BaseOrganizeManager(base.DbHelper, base.UserInfo);
            string[] organizeIds = null;
            switch (base.DbHelper.CurrentDbType)
            {
                case CurrentDbType.Oracle:
                    organizeIds = manager.GetChildrensId(BaseOrganizeEntity.FieldId, organizeId, BaseOrganizeEntity.FieldParentId);
                    break;

                case CurrentDbType.SqlServer:
                case CurrentDbType.Access:
                {
                    string codeById = this.GetCodeById(organizeId);
                    organizeIds = manager.GetChildrensIdByCode(BaseOrganizeEntity.FieldCode, codeById);
                    break;
                }
            }
            return this.GetDTByOrganizes(organizeIds);
        }

        public override DataTable GetDT()
        {
            string commandText = " SELECT " + BaseStaffEntity.TableName + ".*  , " + BaseUserEntity.TableName + ".UserOnLine ,(SELECT " + BaseOrganizeEntity.FieldCode + " FROM " + BaseOrganizeEntity.TableName + " WHERE Id = " + BaseStaffEntity.TableName + ".CompanyId) AS CompanyCode ,(SELECT " + BaseOrganizeEntity.FieldFullName + " FROM " + BaseOrganizeEntity.TableName + " WHERE Id = " + BaseStaffEntity.TableName + ".CompanyId) AS CompanyFullname  ,(SELECT " + BaseOrganizeEntity.FieldCode + " From " + BaseOrganizeEntity.TableName + " WHERE Id = " + BaseStaffEntity.TableName + ".DepartmentId) AS DepartmentCode ,(SELECT " + BaseOrganizeEntity.FieldFullName + " FROM " + BaseOrganizeEntity.TableName + " WHERE Id = " + BaseStaffEntity.TableName + ".DepartmentId) AS DepartmentName  ,(SELECT " + BaseOrganizeEntity.FieldCode + " From " + BaseOrganizeEntity.TableName + " WHERE Id = " + BaseStaffEntity.TableName + ".WorkgroupId) AS WorkgroupCode ,(SELECT " + BaseOrganizeEntity.FieldFullName + " FROM " + BaseOrganizeEntity.TableName + " WHERE Id = " + BaseStaffEntity.TableName + ".WorkgroupId) AS WorkgroupName  ,(SELECT " + BaseItemDetailsEntity.FieldItemName + " FROM Base_ItemDetails_Duty WHERE Id = " + BaseStaffEntity.TableName + ".DutyId) AS DutyName  ,(SELECT " + BaseItemDetailsEntity.FieldItemName + " FROM Base_ItemDetails_Title WHERE Id = " + BaseStaffEntity.TableName + ".TitleId) AS TitleName  ,(SELECT " + BaseRoleEntity.FieldRealName + " FROM " + BaseRoleEntity.TableName + " WHERE Id = RoleId) AS RoleName  FROM (" + BaseStaffEntity.TableName + " LEFT OUTER JOIN " + BaseUserEntity.TableName + " ON " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldId + " = " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + ")   LEFT OUTER JOIN " + BaseOrganizeEntity.TableName + "  ON " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldDepartmentId + " = " + BaseOrganizeEntity.TableName + "." + BaseOrganizeEntity.FieldId + " ORDER BY " + BaseOrganizeEntity.TableName + "." + BaseOrganizeEntity.FieldSortCode + " , " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public DataTable GetDT(string fieldName, string fieldValue)
        {
            string commandText = " SELECT A.*  ,(SELECT Code FROM " + BaseOrganizeEntity.TableName + " WHERE " + BaseOrganizeEntity.TableName + ".ID = A.CompanyId) AS CompanyCode ,(SELECT FullName FROM " + BaseOrganizeEntity.TableName + " WHERE " + BaseOrganizeEntity.TableName + ".ID = A.CompanyId) AS CompanyName  ,(SELECT Code FROM " + BaseOrganizeEntity.TableName + " WHERE " + BaseOrganizeEntity.TableName + ".ID = A.DepartmentId) AS DepartmentCode ,(SELECT FullName FROM " + BaseOrganizeEntity.TableName + " WHERE " + BaseOrganizeEntity.TableName + ".ID = A.DepartmentId) AS DepartmentName  ,(SELECT " + BaseOrganizeEntity.FieldCode + " From " + BaseOrganizeEntity.TableName + " WHERE Id = A.WorkgroupId) AS WorkgroupCode ,(SELECT " + BaseOrganizeEntity.FieldFullName + " FROM " + BaseOrganizeEntity.TableName + " WHERE Id = A.WorkgroupId) AS WorkgroupName  ,(SELECT ItemName FROM Base_ItemDetails_Duty WHERE Base_ItemDetails_Duty.Id = A.DutyId) AS DutyName  ,(SELECT ItemName FROM Base_ItemDetails_Title WHERE Base_ItemDetails_Title.Id = A.TitleId) AS TitleName  FROM " + BaseStaffEntity.TableName + " A  WHERE " + fieldName + " = " + base.DbHelper.GetParameter(fieldName) + " ORDER BY A.SortCode";
            return base.DbHelper.Fill(commandText, new IDbDataParameter[] { base.DbHelper.MakeParameter(fieldName, fieldValue) });
        }

        public DataTable GetDTByDepartment(string departmentId)
        {
            string commandText = " SELECT " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldUserOnLine + ", " + BaseStaffEntity.TableName + ".*  ,(SELECT " + BaseOrganizeEntity.FieldCode + " FROM " + BaseOrganizeEntity.TableName + " WHERE Id = " + BaseStaffEntity.TableName + ".CompanyId) AS CompanyCode ,(SELECT " + BaseOrganizeEntity.FieldFullName + " FROM " + BaseOrganizeEntity.TableName + " WHERE Id = " + BaseStaffEntity.TableName + ".CompanyId) AS CompanyFullname  ,(SELECT " + BaseOrganizeEntity.FieldCode + " From " + BaseOrganizeEntity.TableName + " WHERE Id = " + BaseStaffEntity.TableName + ".DepartmentId) AS DepartmentCode ,(SELECT " + BaseOrganizeEntity.FieldFullName + " FROM " + BaseOrganizeEntity.TableName + " WHERE Id = " + BaseStaffEntity.TableName + ".DepartmentId) AS DepartmentName  ,(SELECT " + BaseItemDetailsEntity.FieldItemName + " FROM Base_ItemDetails_Duty WHERE Id = " + BaseStaffEntity.TableName + ".DutyId) AS DutyName  ,(SELECT " + BaseItemDetailsEntity.FieldItemName + " FROM Base_ItemDetails_Title WHERE Id = " + BaseStaffEntity.TableName + ".TitleId) AS TitleName  ,(SELECT " + BaseRoleEntity.FieldRealName + " FROM " + BaseRoleEntity.TableName + " WHERE Id = RoleId) AS RoleName  FROM " + BaseStaffEntity.TableName + " LEFT OUTER JOIN " + BaseUserEntity.TableName + "       ON " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldId + " = " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId;
            if (!string.IsNullOrEmpty(departmentId))
            {
                string str2 = commandText;
                commandText = str2 + " WHERE " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldDepartmentId + " = '" + departmentId + "' ";
            }
            string str3 = commandText;
            commandText = str3 + " ORDER BY " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public DataTable GetDTByOrganize(string organizeId)
        {
            string[] organizeIds = new string[] { organizeId };
            return this.GetDTByOrganizes(organizeIds);
        }

        public DataTable GetDTByOrganizes(string[] organizeIds)
        {
            string str = BaseBusinessLogic.ObjectsToList(organizeIds);
            string commandText = " SELECT " + BaseStaffEntity.TableName + ".* ," + BaseUserEntity.TableName + ".UserOnLine  FROM " + BaseStaffEntity.TableName + " LEFT OUTER JOIN " + BaseUserEntity.TableName + "       ON " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldUserId + " = " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + " WHERE (" + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldDeletionStateCode + " = 0 )        AND (" + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldWorkgroupId + " IN ( " + str + ")        OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldDepartmentId + " IN (" + str + ")        OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldCompanyId + " IN (" + str + "))  ORDER BY " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public BaseStaffEntity GetEntity(int id)
        {
            return this.GetEntity(id.ToString());
        }

        public BaseStaffEntity GetEntity(string id)
        {
            return new BaseStaffEntity(this.GetDT(BaseStaffEntity.FieldId, id));
        }

        public DataTable GetParentChildrenStaffs(string organizeId)
        {
            string[] organizeIds = null;
            BaseOrganizeManager manager = new BaseOrganizeManager(base.DbHelper, base.UserInfo);
            string codeById = manager.GetCodeById(organizeId);
            organizeIds = manager.GetChildrensIdByCode(BaseOrganizeEntity.FieldCode, codeById);
            return this.GetDTByOrganizes(organizeIds);
        }

        public string GetStaffCount(string organizeCode)
        {
            string commandText = string.Empty;
            string str2 = string.Empty;
            string[] targetFileds = new string[1];
            object[] targetValues = new object[1];
            commandText = " SELECT COUNT(Id) AS STAFFCOUNT FROM " + BaseStaffEntity.TableName + " WHERE (ENABLED = 1) AND (ISDIMISSION <> 1) AND (ISSTAFF = 1) AND (DepartmentId IN (SELECT Id FROM " + BaseOrganizeEntity.TableName + " WHERE (LEFT(CODE, LEN(?)) = ?))) ";
            targetFileds[0] = BaseStaffEntity.FieldCompanyId;
            targetValues[0] = organizeCode;
            object obj2 = base.DbHelper.ExecuteScalar(commandText, base.DbHelper.MakeParameters(targetFileds, targetValues));
            if (obj2 != null)
            {
                str2 = obj2.ToString();
            }
            return str2;
        }

        public override int ResetSortCode()
        {
            int num = 0;
            DataTable dT = this.GetDT();
            string id = string.Empty;
            string targetValue = string.Empty;
            foreach (DataRow row in dT.Rows)
            {
                id = row[BaseStaffEntity.FieldId].ToString();
                targetValue = new BaseSequenceManager(base.DbHelper).GetSequence(BaseStaffEntity.TableName);
                num += this.SetProperty(id, BaseStaffEntity.FieldSortCode, targetValue);
            }
            return num;
        }

        public DataTable Search(string organizeId, string searchValue, bool deletionStateCode)
        {
            searchValue = BaseBusinessLogic.GetSearchString(searchValue);
            string commandText = string.Concat(new object[] { 
                " SELECT ", BaseStaffEntity.TableName, ".* ,", BaseOrganizeEntity.TableName, ".", BaseOrganizeEntity.FieldFullName, " AS DepartmentName ,Base_ItemDetails_Duty.", BaseItemDetailsEntity.FieldItemName, " AS DutyName ,OT.RoleName  FROM ", BaseStaffEntity.TableName, "      LEFT OUTER JOIN ", BaseOrganizeEntity.TableName, " ON ", BaseOrganizeEntity.TableName, ".", BaseOrganizeEntity.FieldId, 
                " = ", BaseStaffEntity.FieldDepartmentId, "      LEFT OUTER JOIN Base_ItemDetails_Duty ON  Base_ItemDetails_Duty.", BaseItemDetailsEntity.FieldId, " = ", BaseStaffEntity.FieldDutyId, "      LEFT OUTER JOIN (SELECT ", BaseUserEntity.TableName, ".", BaseUserEntity.FieldId, ",", BaseRoleEntity.TableName, ".", BaseRoleEntity.FieldRealName, " AS RoleName  FROM ", BaseUserEntity.TableName, 
                ",", BaseRoleEntity.TableName, " WHERE ", BaseUserEntity.TableName, ".", BaseUserEntity.FieldRoleId, " = ", BaseRoleEntity.TableName, ".", BaseRoleEntity.FieldId, ") OT                   ON ", BaseStaffEntity.TableName, ".", BaseStaffEntity.FieldId, " = OT.ID  WHERE (", BaseStaffEntity.TableName, 
                ".", BaseStaffEntity.FieldDeletionStateCode, " = ", deletionStateCode ? 1 : 0, ")"
             });
            if (!string.IsNullOrEmpty(organizeId))
            {
                string str2 = commandText;
                commandText = str2 + " AND (" + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldDepartmentId + " = '" + organizeId + "')";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                string str3 = commandText;
                string str4 = str3 + " AND (" + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldUserName + " LIKE '" + searchValue + "'";
                string str5 = str4 + " OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldRealName + " LIKE '" + searchValue + "'";
                string str6 = str5 + " OR " + BaseOrganizeEntity.TableName + "." + BaseOrganizeEntity.FieldFullName + " LIKE '" + searchValue + "'";
                commandText = (str6 + " OR Base_ItemDetails_Duty." + BaseItemDetailsEntity.FieldItemName + " LIKE '" + searchValue + "'") + " OR OT.RoleName LIKE '" + searchValue + "')";
            }
            string str7 = commandText;
            commandText = str7 + " ORDER BY " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public DataTable Search(string userName, string enabled, string role)
        {
            return this.SearchByOrganizeIds(null, userName, enabled, role);
        }

        public DataTable SearchByOrganizeIds(string[] organizeIds, string userName, string enabled, string role)
        {
            string commandText = " SELECT " + BaseUserEntity.TableName + ".* ," + BaseUserEntity.TableName + "." + BaseUserEntity.FieldUserOnLine + " ,(SELECT " + BaseRoleEntity.FieldRealName + " FROM " + BaseRoleEntity.TableName + " WHERE " + BaseRoleEntity.FieldId + " = " + BaseUserEntity.FieldRoleId + ") AS RoleName  FROM " + BaseUserEntity.TableName + " LEFT OUTER JOIN " + BaseStaffEntity.TableName + " ON " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + " = " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldId + " WHERE 1=1 ";
            if (organizeIds != null)
            {
                string str2 = BaseBusinessLogic.ObjectsToList(organizeIds);
                string str3 = commandText;
                string str4 = str3 + " AND (" + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldWorkgroupId + " IN (" + str2 + ") ";
                string str5 = str4 + "     OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldDepartmentId + " IN (" + str2 + ") ";
                commandText = str5 + "     OR " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldCompanyId + " IN (" + str2 + ")) ";
            }
            if (!string.IsNullOrEmpty(userName))
            {
                if (userName.IndexOf('%') < 0)
                {
                    userName = "%" + userName + "%";
                }
                userName = base.DbHelper.SqlSafe(userName);
            }
            if (!string.IsNullOrEmpty(userName))
            {
                string str6 = commandText;
                commandText = str6 + " AND UPPER(" + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldUserName + ") LIKE UPPER('" + userName + "') ";
            }
            if (!string.IsNullOrEmpty(enabled))
            {
                string str7 = commandText;
                commandText = str7 + " AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldEnabled + " = '" + enabled + "'";
            }
            if (!string.IsNullOrEmpty(role))
            {
                string str8 = commandText;
                commandText = str8 + " AND " + BaseUserEntity.FieldRoleId + " = '" + role + "'";
            }
            string str9 = commandText;
            commandText = str9 + " ORDER BY " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public int SetDeleted(string id)
        {
            string property = this.GetProperty(id, BaseStaffEntity.FieldUserId);
            if (!string.IsNullOrEmpty(property))
            {
                new BaseUserManager(base.DbHelper, base.UserInfo).SetDeleted(property);
            }
            return this.SetProperty(BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldDeletionStateCode, "1");
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseStaffEntity staffEntity)
        {
            sqlBuilder.SetValue(BaseStaffEntity.FieldUserId, staffEntity.UserId, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldUserName, staffEntity.UserName, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldRealName, staffEntity.RealName, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldCode, staffEntity.Code, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldGender, staffEntity.Gender, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldCompanyId, staffEntity.CompanyId, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldDepartmentId, staffEntity.DepartmentId, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldWorkgroupId, staffEntity.WorkgroupId, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldDutyId, staffEntity.DutyId, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldIdentificationCode, staffEntity.IdentificationCode, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldIdCard, staffEntity.IdCard, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldBankCode, staffEntity.BankCode, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldEmail, staffEntity.Email, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldMobile, staffEntity.Mobile, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldShortNumber, staffEntity.ShortNumber, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldTelephone, staffEntity.Telephone, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldOICQ, staffEntity.OICQ, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldOfficePhone, staffEntity.OfficePhone, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldOfficeZipCode, staffEntity.OfficeZipCode, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldOfficeAddress, staffEntity.OfficeAddress, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldOfficeFax, staffEntity.OfficeFax, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldHomePhone, staffEntity.HomePhone, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldAge, staffEntity.Age, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldBirthday, staffEntity.Birthday, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldEducation, staffEntity.Education, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldSchool, staffEntity.School, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldDegree, staffEntity.Degree, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldTitleId, staffEntity.TitleId, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldTitleDate, staffEntity.TitleDate, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldTitleLevel, staffEntity.TitleLevel, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldWorkingDate, staffEntity.WorkingDate, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldJoinInDate, staffEntity.JoinInDate, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldHomeZipCode, staffEntity.HomeZipCode, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldHomeAddress, staffEntity.HomeAddress, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldHomeFax, staffEntity.HomeFax, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldCarCode, staffEntity.CarCode, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldEmergencyContact, staffEntity.EmergencyContact, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldNativePlace, staffEntity.NativePlace, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldParty, staffEntity.Party, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldNation, staffEntity.Nation, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldNationality, staffEntity.Nationality, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldMajor, staffEntity.Major, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldWorkingProperty, staffEntity.WorkingProperty, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldCompetency, staffEntity.Competency, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldIsDimission, staffEntity.IsDimission, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldDimissionDate, staffEntity.DimissionDate, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldDimissionCause, staffEntity.DimissionCause, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldDimissionWhither, staffEntity.DimissionWhither, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldEnabled, staffEntity.Enabled, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldDeletionStateCode, staffEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldSortCode, staffEntity.SortCode, null);
            sqlBuilder.SetValue(BaseStaffEntity.FieldDescription, staffEntity.Description, null);
        }

        public int Update(BaseStaffEntity staffEntity)
        {
            return this.UpdateEntity(staffEntity);
        }

        public int Update(BaseStaffEntity staffEntity, out string statusCode)
        {
            int num = 0;
            if (!string.IsNullOrEmpty(staffEntity.Code) && this.Exists(BaseStaffEntity.FieldCode, staffEntity.Code, BaseStaffEntity.FieldDeletionStateCode, 0, staffEntity.Id))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return num;
            }
            if (!string.IsNullOrEmpty(staffEntity.UserName) && this.Exists(BaseStaffEntity.FieldUserName, staffEntity.UserName, BaseStaffEntity.FieldDeletionStateCode, 0, staffEntity.Id))
            {
                statusCode = StatusCode.ErrorUserExist.ToString();
                return num;
            }
            num = this.UpdateEntity(staffEntity);
            this.UpdateUser(staffEntity.Id.ToString());
            if (num > 0)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateAddress(BaseStaffEntity staffEntity, out string statusCode)
        {
            int num = 0;
            num = this.UpdateEntity(staffEntity);
            if (num == 1)
            {
                this.UpdateUser(staffEntity.Id.ToString());
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseStaffEntity staffEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BaseStaffEntity.TableName);
            this.SetEntity(sqlBuilder, staffEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseStaffEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseStaffEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseStaffEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseStaffEntity.FieldId, staffEntity.Id);
            return sqlBuilder.EndUpdate();
        }

        public int UpdateUser(string staffId)
        {
            BaseStaffEntity entity = new BaseStaffEntity(this.GetDT(BaseStaffEntity.FieldId, staffId));
            if (entity.UserId > 0)
            {
                BaseUserEntity entity2 = new BaseUserManager(base.DbHelper, base.UserInfo).GetEntity(entity.UserId);
                entity2.UserName = entity.UserName;
                entity2.RealName = entity.RealName;
                entity2.Code = entity.Code;
                entity2.Email = entity.Email;
                entity2.Enabled = entity.Enabled;
                entity2.Gender = entity.Gender;
                entity2.Birthday = entity.Birthday;
                entity2.Mobile = entity.Mobile;
            }
            return 0;
        }
    }
}

