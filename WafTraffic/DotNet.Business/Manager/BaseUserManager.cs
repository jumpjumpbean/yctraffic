//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

/// 修改纪录
/// 
///		2012.04.26 版本：1.0 sun 实例化时就获取UserCenterDbConnection，保证连接的正确性。

namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;

    /// <summary>
    /// BaseUserManager
    /// </summary>
    public class BaseUserManager : BaseManager, IBaseManager
    {
        public BaseUserManager()
        {
            if (base.dbHelper == null)
            {
                if (String.IsNullOrEmpty(BaseSystemInfo.UserCenterDbConnection)) BaseConfiguration.GetSetting();
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseUserEntity.TableName;
        }

        public BaseUserManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseUserManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseUserManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseUserManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseUserManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        /// <summary>
        /// 激活用户账号
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public BaseUserInfo AccountActivation(string openId, out string statusCode)
        {
            BaseUserInfo info = null;
            statusCode = StatusCode.UserNotFound.ToString();
            if (!string.IsNullOrEmpty(openId))
            {
                BaseUserManager manager = new BaseUserManager(base.DbHelper);
                DataTable dataTable = manager.GetDT(BaseUserEntity.FieldOpenId, openId, BaseUserEntity.FieldDeletionStateCode, 0);
                if (dataTable.Rows.Count != 1)
                {
                    return info;
                }
                BaseUserEntity entity = new BaseUserEntity(dataTable);
                if (entity.Enabled == 0)
                {
                    statusCode = StatusCode.UserLocked.ToString();
                    return info;
                }
                if (entity.Enabled == 1)
                {
                    statusCode = StatusCode.UserIsActivate.ToString();
                    return info;
                }
                if (entity.Enabled == -1)
                {
                    statusCode = StatusCode.OK.ToString();
                    manager.SetProperty(BaseUserEntity.FieldId, entity.Id, BaseUserEntity.FieldEnabled, 1);
                    return info;
                }
            }
            return info;
        }

        public string Add(BaseUserEntity baseUserEntity)
        {
            return this.AddEntity(baseUserEntity);
        }

        public string Add(BaseUserEntity userEntity, out string statusCode)
        {
            string str = string.Empty;
            this.BeforeAdd(userEntity, out statusCode);
            if (statusCode == StatusCode.OK.ToString())
            {
                str = this.AddEntity(userEntity);
                this.AfterAdd(userEntity, out statusCode);
            }
            return str;
        }

        public string Add(BaseUserEntity baseUserEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseUserEntity);
        }

        public string AddEntity(BaseUserEntity baseUserEntity)
        {
            string s = string.Empty;
            if (baseUserEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseUserEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseUserEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseUserEntity.FieldId, baseUserEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseUserEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseUserEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!baseUserEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseUserEntity.Id = new int?(int.Parse(s));
                }
                sqlBuilder.SetValue(BaseUserEntity.FieldId, baseUserEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseUserEntity);

            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public void AfterAdd(BaseUserEntity userEntity, out string statusCode)
        {
            statusCode = StatusCode.OKAdd.ToString();
        }

        public override int BatchSave(DataTable dataTable)
        {
            int num = 0;
            BaseUserEntity baseUserEntity = new BaseUserEntity();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    string id = row[BaseUserEntity.FieldId, DataRowVersion.Original].ToString();
                    if (id.Length > 0)
                    {
                        num += this.Delete(id);
                    }
                }
                if ((row.RowState == DataRowState.Modified) && !string.IsNullOrEmpty(row[BaseUserEntity.FieldId, DataRowVersion.Original].ToString()))
                {
                    baseUserEntity.GetFrom(row);
                    num += this.UpdateEntity(baseUserEntity);
                }
                if (row.RowState == DataRowState.Added)
                {
                    baseUserEntity.GetFrom(row);
                    num += (this.AddEntity(baseUserEntity).Length > 0) ? 1 : 0;
                }
                if (row.RowState != DataRowState.Unchanged)
                {
                    DataRowState rowState = row.RowState;
                }
            }
            return num;
        }

        public virtual int BatchSetCommunicationPassword(string[] userIds, string password, out string statusCode)
        {
            int num = 0;
            if (BaseSystemInfo.CheckPasswordStrength && (password.Length == 0))
            {
                statusCode = StatusCode.PasswordCanNotBeNull.ToString();
                return num;
            }
            if (BaseSystemInfo.ServerEncryptPassword)
            {
                password = this.EncryptUserPassword(password);
            }
            for (int i = 0; i < userIds.Length; i++)
            {
                num = DbLogic.SetProperty(base.DbHelper, BaseUserEntity.TableName, BaseUserEntity.FieldId, (object[]) userIds, BaseUserEntity.FieldCommunicationPassword, password);
            }
            if (num > 0)
            {
                statusCode = StatusCode.SetPasswordOK.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        /// <summary>
        /// 批量设置密码
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual int BatchSetPassword(string[] userIds, string password)
        {
            int num = 0;
            if (BaseSystemInfo.ServerEncryptPassword)
            {
                password = this.EncryptUserPassword(password);
            }
            string[] targetFields = new string[] { BaseUserEntity.FieldUserPassword, BaseUserEntity.FieldChangePasswordDate };
            object[] objArray2 = new object[2];
            objArray2[0] = password;
            object[] targetValues = objArray2;
            num += DbLogic.SetProperty(base.DbHelper, BaseUserEntity.TableName, BaseUserEntity.FieldId, (object[]) userIds, targetFields, targetValues);
            if (num > 0)
            {
                base.ReturnStatusCode = StatusCode.SetPasswordOK.ToString();
                return num;
            }
            base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public void BeforeAdd(BaseUserEntity userEntity, out string statusCode)
        {
            if (this.Exists(BaseUserEntity.FieldUserName, userEntity.UserName, BaseUserEntity.FieldDeletionStateCode, "0") || DbLogic.Exists(base.DbHelper, BaseStaffEntity.TableName, BaseStaffEntity.FieldUserName, userEntity.UserName, BaseStaffEntity.FieldDeletionStateCode, "0"))
            {
                statusCode = StatusCode.ErrorUserExist.ToString();
            }
            else if (!string.IsNullOrEmpty(userEntity.Code) && this.Exists(BaseUserEntity.FieldCode, userEntity.Code, BaseUserEntity.FieldDeletionStateCode, "0"))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
            }
            else if (!string.IsNullOrEmpty(userEntity.Code) && DbLogic.Exists(base.DbHelper, BaseStaffEntity.TableName, BaseStaffEntity.FieldCode, userEntity.Code))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
            }
            else
            {
                statusCode = StatusCode.OK.ToString();
            }
        }

        public virtual int ChangeCommunicationPassword(string oldPassword, string newPassword, out string statusCode)
        {
            int num = 0;
            if (BaseSystemInfo.CheckPasswordStrength && string.IsNullOrEmpty(newPassword))
            {
                statusCode = StatusCode.PasswordCanNotBeNull.ToString();
                return num;
            }
            if (BaseSystemInfo.ServerEncryptPassword)
            {
                oldPassword = this.EncryptUserPassword(oldPassword);
                newPassword = this.EncryptUserPassword(newPassword);
            }
            BaseUserEntity entity = new BaseUserEntity();
            entity.GetFrom(this.GetDTById(base.UserInfo.Id));
            if (entity.CommunicationPassword == null)
            {
                entity.CommunicationPassword = string.Empty;
            }
            if (!entity.CommunicationPassword.Equals(oldPassword))
            {
                statusCode = StatusCode.OldPasswordError.ToString();
                return num;
            }
            num = this.SetProperty(base.UserInfo.Id, BaseUserEntity.FieldCommunicationPassword, newPassword);
            if (num == 1)
            {
                statusCode = StatusCode.ChangePasswordOK.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        /// <summary>
        /// 设置用户为在线状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int ChangeOnLine(string id)
        {
            int num = 0;
            if (((base.UserInfo != null) && !string.IsNullOrEmpty(base.UserInfo.Id)) && (!string.IsNullOrEmpty(base.UserInfo.OpenId) && !base.UserInfo.Id.Equals(id)))
            {
                num += this.OnExit(base.UserInfo.Id);
            }
            return (num + this.OnLine(id, 1));
        }

        public virtual int ChangePassword(string oldPassword, string newPassword, out string statusCode)
        {
            int num = 0;
            if (BaseSystemInfo.CheckPasswordStrength && string.IsNullOrEmpty(newPassword))
            {
                statusCode = StatusCode.PasswordCanNotBeNull.ToString();
                return num;
            }
            if (BaseSystemInfo.ServerEncryptPassword)
            {
                oldPassword = this.EncryptUserPassword(oldPassword);
                newPassword = this.EncryptUserPassword(newPassword);
            }
            BaseUserEntity entity = new BaseUserEntity();
            entity.GetFrom(this.GetDTById(base.UserInfo.Id));
            if (entity.UserPassword == null)
            {
                entity.UserPassword = string.Empty;
            }
            if (!entity.UserPassword.Equals(oldPassword))
            {
                statusCode = StatusCode.OldPasswordError.ToString();
                return num;
            }
            string[] targetFields = new string[] { BaseUserEntity.FieldUserPassword, BaseUserEntity.FieldChangePasswordDate };
            object[] targetValues = new object[] { newPassword, DateTime.Now };
            num = this.SetProperty(base.UserInfo.Id, targetFields, targetValues);
            if (num == 1)
            {
                statusCode = StatusCode.ChangePasswordOK.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public virtual int ChangeSignedPassword(string oldPassword, string newPassword, out string statusCode)
        {
            int num = 0;
            if (BaseSystemInfo.CheckPasswordStrength && string.IsNullOrEmpty(newPassword))
            {
                statusCode = StatusCode.PasswordCanNotBeNull.ToString();
                return num;
            }
            if (BaseSystemInfo.ServerEncryptPassword)
            {
                oldPassword = this.EncryptUserPassword(oldPassword);
                newPassword = this.EncryptUserPassword(newPassword);
            }
            BaseUserEntity entity = new BaseUserEntity();
            entity.GetFrom(this.GetDTById(base.UserInfo.Id));
            if (entity.SignedPassword == null)
            {
                entity.SignedPassword = string.Empty;
            }
            if (!entity.SignedPassword.Equals(oldPassword))
            {
                statusCode = StatusCode.OldPasswordError.ToString();
                return num;
            }
            num = this.SetProperty(base.UserInfo.Id, BaseUserEntity.FieldSignedPassword, newPassword);
            if (num == 1)
            {
                statusCode = StatusCode.ChangePasswordOK.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        private bool CheckIPAddress(string ipAddress, string userId)
        {
            bool flag = false;
            string[] names = new string[] { BaseParameterEntity.FieldParameterId, BaseParameterEntity.FieldCategoryId, BaseParameterEntity.FieldEnabled };
            object[] values = new object[] { userId, "IPAddress", 1 };
            DataTable table = DbLogic.GetDT(base.DbHelper, BaseParameterEntity.TableName, names, values);
            if (table.Rows.Count > 0)
            {
                string str = string.Empty;
                string sourceIp = string.Empty;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    str = table.Rows[i][BaseParameterEntity.FieldParameterCode].ToString();
                    sourceIp = table.Rows[i][BaseParameterEntity.FieldParameterContent].ToString();
                    string str3 = str;
                    if (str3 != null)
                    {
                        if (!(str3 == "Single"))
                        {
                            if (str3 == "Range")
                            {
                                goto Label_0100;
                            }
                            if (str3 == "Mask")
                            {
                                goto Label_010C;
                            }
                        }
                        else
                        {
                            flag = this.CheckSingleIPAddress(ipAddress, sourceIp);
                        }
                    }
                    goto Label_0116;
                Label_0100:
                    flag = this.CheckIPAddressWithRange(ipAddress, sourceIp);
                    goto Label_0116;
                Label_010C:
                    flag = this.CheckIPAddressWithMask(ipAddress, sourceIp);
                Label_0116:
                    if (flag)
                    {
                        return flag;
                    }
                }
            }
            return flag;
        }

        private bool CheckIPAddress(string[] ipAddress, string userId)
        {
            for (int i = 0; i < ipAddress.Length; i++)
            {
                if (this.CheckIPAddress(ipAddress[i], userId))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckSingleIPAddress(string ipAddress, string sourceIp)
        {
            return ipAddress.Equals(sourceIp);
        }

        private bool CheckIPAddressWithMask(string ipAddress, string ipWithMask)
        {
            string[] strArray = ipAddress.Split(new char[] { '.' });
            string[] strArray2 = ipWithMask.Split(new char[] { '.' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (!strArray2[i].Equals("*") && !strArray[i].Equals(strArray2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckIPAddressWithRange(string ipAddress, string ipRange)
        {
            string str = ipRange.Split(new char[] { '-' })[0];
            string str2 = ipRange.Split(new char[] { '-' })[1];
            if (((this.CompareIp(ipAddress, str) != 2) || (this.CompareIp(ipAddress, str2) != 0)) && ((this.CompareIp(ipAddress, str) != 1) && (this.CompareIp(ipAddress, str2) != 1)))
            {
                return false;
            }
            return true;
        }

        public int CompareIp(string ip1, string ip2)
        {
            string[] strArray = ip1.Split(new char[] { '.' });
            string[] strArray2 = ip2.Split(new char[] { '.' });
            for (int i = 0; i < strArray.Length; i++)
            {
                int num2 = int.Parse(strArray[i]);
                int num3 = int.Parse(strArray2[i]);
                if (num2 > num3)
                {
                    return 2;
                }
                if (num2 < num3)
                {
                    return 0;
                }
            }
            return 1;
        }

        private bool CheckMacAddress(string macAddress, string userId)
        {
            bool flag = false;
            string[] names = new string[] { BaseParameterEntity.FieldParameterId, BaseParameterEntity.FieldCategoryId, BaseParameterEntity.FieldEnabled };
            object[] values = new object[] { userId, "MacAddress", 1 };
            DataTable table = DbLogic.GetDT(base.DbHelper, BaseParameterEntity.TableName, names, values);
            if (table.Rows.Count > 0)
            {
                string str = string.Empty;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    table.Rows[i][BaseParameterEntity.FieldParameterCode].ToString();
                    str = table.Rows[i][BaseParameterEntity.FieldParameterContent].ToString();
                    flag = macAddress.ToLower().Equals(str.ToLower());
                    if (flag)
                    {
                        return flag;
                    }
                }
            }
            return flag;
        }

        private bool CheckMacAddress(string[] macAddress, string userId)
        {
            for (int i = 0; i < macAddress.Length; i++)
            {
                if (this.CheckMacAddress(macAddress[i], userId))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 根据上次活动时间更新用户的在线状态
        /// </summary>
        /// <returns></returns>
        public int CheckOnLine()
        {
            int num = 0;
            if (BaseSystemInfo.UpdateVisit)
            {
                string commandText = string.Empty;
                switch (base.DbHelper.CurrentDbType)
                {
                    case CurrentDbType.Oracle:
                        commandText = " UPDATE " + BaseUserEntity.TableName + "     SET " + BaseUserEntity.FieldUserOnLine + " = 0    WHERE (" + BaseUserEntity.FieldLastVisit + " IS NULL)       OR ((" + BaseUserEntity.FieldUserOnLine + " > 0) AND (" + BaseUserEntity.FieldLastVisit + " IS NOT NULL) AND ((SYSDATE - " + BaseUserEntity.FieldLastVisit + ") * 24 * 60 * 60 > " + BaseSystemInfo.OnLineTime0ut.ToString() + "))";
                        return (num + base.DbHelper.ExecuteNonQuery(commandText));

                    case CurrentDbType.SqlServer:
                        commandText = " UPDATE " + BaseUserEntity.TableName + "     SET " + BaseUserEntity.FieldUserOnLine + " = 0    WHERE (" + BaseUserEntity.FieldLastVisit + " IS NULL)       OR ((" + BaseUserEntity.FieldUserOnLine + " > 0) AND (" + BaseUserEntity.FieldLastVisit + " IS NOT NULL) AND (DATEADD (s, " + BaseSystemInfo.OnLineTime0ut.ToString() + ", " + BaseUserEntity.FieldLastVisit + ") < " + base.DbHelper.GetDBNow() + "))";
                        return (num + base.DbHelper.ExecuteNonQuery(commandText));

                    case CurrentDbType.Access:
                        return num;

                    case CurrentDbType.DB2:
                        commandText = " UPDATE " + BaseUserEntity.TableName + "     SET " + BaseUserEntity.FieldUserOnLine + " = 0    WHERE (" + BaseUserEntity.FieldLastVisit + " IS NULL)       OR ((" + BaseUserEntity.FieldUserOnLine + " > 0) AND (" + BaseUserEntity.FieldLastVisit + " IS NOT NULL) AND (" + BaseUserEntity.FieldLastVisit + " + " + BaseSystemInfo.OnLineTime0ut.ToString() + " SECONDS < " + base.DbHelper.GetDBNow() + "))";
                        return (num + base.DbHelper.ExecuteNonQuery(commandText));

                    case CurrentDbType.MySql:
                        commandText = " UPDATE " + BaseUserEntity.TableName + "     SET " + BaseUserEntity.FieldUserOnLine + " = 0    WHERE (" + BaseUserEntity.FieldLastVisit + " IS NULL)       OR ((" + BaseUserEntity.FieldUserOnLine + " > 0) AND (" + BaseUserEntity.FieldLastVisit + " IS NOT NULL) AND (DATE_ADD(" + BaseUserEntity.FieldLastVisit + ", Interval " + BaseSystemInfo.OnLineTime0ut.ToString() + " SECOND) < " + base.DbHelper.GetDBNow() + "))";
                        return (num + base.DbHelper.ExecuteNonQuery(commandText));
                }
            }
            return num;
        }

        /// <summary>
        /// 检查是否超出同时在线用户数的限制
        /// </summary>
        /// <returns></returns>
        private bool CheckOnLineLimit()
        {
            bool flag = false;
            this.CheckOnLine();
            string commandText = string.Empty;
            commandText = " SELECT COUNT(*)    FROM " + BaseUserEntity.TableName + "  WHERE " + BaseUserEntity.FieldUserOnLine + " > 0 ";
            object obj2 = base.DbHelper.ExecuteScalar(commandText);
            if ((obj2 != null) && (BaseSystemInfo.OnLineLimit <= int.Parse(obj2.ToString())))
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 清理不存在的User在Staff中的关联
        /// </summary>
        /// <returns></returns>
        public int CheckUserStaff()
        {
            string commandText = " UPDATE Base_Staff SET UserId = null WHERE UserId NOT IN ( SELECT Id FROM BASE_USER WHERE DeletionStateCode = 0 ) ";
            return base.DbHelper.ExecuteNonQuery(commandText);
        }

        public virtual bool CommunicationPassword(string communicationPassword)
        {
            bool flag = false;
            if (!BaseSystemInfo.CheckPasswordStrength || !string.IsNullOrEmpty(communicationPassword))
            {
                if (BaseSystemInfo.ServerEncryptPassword)
                {
                    communicationPassword = this.EncryptUserPassword(communicationPassword);
                }
                BaseUserEntity entity = new BaseUserEntity();
                entity.GetFrom(this.GetDTById(base.UserInfo.Id));
                if (entity.CommunicationPassword == null)
                {
                    entity.CommunicationPassword = string.Empty;
                }
                if (entity.CommunicationPassword.Equals(communicationPassword))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public BaseUserInfo ConvertToUserInfo(BaseUserEntity userEntity)
        {
            BaseUserInfo userInfo = new BaseUserInfo();
            return this.ConvertToUserInfo(userEntity, userInfo);
        }

        public BaseUserInfo ConvertToUserInfo(BaseUserEntity userEntity, BaseUserInfo userInfo)
        {
            userInfo.OpenId = userEntity.OpenId;
            userInfo.Id = userEntity.Id.ToString();
            userInfo.Code = userEntity.Code;
            userInfo.UserName = userEntity.UserName;
            userInfo.RealName = userEntity.RealName;
            userInfo.RoleId = userEntity.RoleId;
            userInfo.CompanyId = userEntity.CompanyId;
            userInfo.CompanyName = userEntity.CompanyName;
            userInfo.DepartmentId = userEntity.DepartmentId;
            userInfo.DepartmentName = userEntity.DepartmentName;
            userInfo.WorkgroupId = userEntity.WorkgroupId;
            userInfo.WorkgroupName = userEntity.WorkgroupName;
            if (!userEntity.SecurityLevel.HasValue)
            {
                userEntity.SecurityLevel = 0;
            }
            userInfo.SecurityLevel = userEntity.SecurityLevel.Value;
            if (userEntity.RoleId.HasValue)
            {
                BaseRoleEntity entity = new BaseRoleManager(base.DbHelper, base.UserInfo).GetEntity(userEntity.RoleId);
                if (entity.Id > 0)
                {
                    userInfo.RoleName = entity.RealName;
                }
            }
            return userInfo;
        }

        /// <summary>
        /// 建立数字签名
        /// </summary>
        /// <param name="password"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public virtual string CreateDigitalSignature(string password, out string statusCode)
        {
            string str = string.Empty;
            statusCode = string.Empty;
            password = this.EncryptUserPassword(password);
            this.SetProperty(base.UserInfo.Id, BaseUserEntity.FieldSignedPassword, password);
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            string targetValue = Convert.ToBase64String(provider.ExportCspBlob(false));
            string str3 = Convert.ToBase64String(provider.ExportCspBlob(true));
            this.SetProperty(base.UserInfo.Id, BaseUserEntity.FieldPublicKey, targetValue);
            str = str3;
            statusCode = StatusCode.OK.ToString();
            return str;
        }

        public int Delete(int id)
        {
            return this.Delete(BaseUserEntity.FieldId, id);
        }

        public virtual string EncryptUserPassword(string password)
        {
            return SecretUtil.md5(password, 0x20);
        }

        /// <summary>
        /// 获取本部门及其下级部门的用户列表
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        public DataTable GetChildrenUsers(string organizeId)
        {
            string[] organizeIds = null;
            BaseOrganizeManager manager = new BaseOrganizeManager(base.DbHelper, base.UserInfo);
            switch (base.DbHelper.CurrentDbType)
            {
                case CurrentDbType.Oracle:
                case CurrentDbType.SqlServer: //2005以上
                    organizeIds = manager.GetChildrensId(BaseOrganizeEntity.FieldId, organizeId, BaseOrganizeEntity.FieldParentId);
                    break;

                case CurrentDbType.Access:
                {
                    string codeById = this.GetCodeById(organizeId);
                    organizeIds = manager.GetChildrensIdByCode(BaseOrganizeEntity.FieldCode, codeById);
                    break;
                }
            }
            return this.GetDTByOrganizes(organizeIds);
        }

        public string GetCount()
        {
            string commandText = " SELECT COUNT(Id) AS UserCount    FROM " + base.CurrentTableName + "  WHERE Enabled = 1";
            return base.DbHelper.ExecuteScalar(commandText).ToString();
        }

        public override DataTable GetDT()
        {
            string commandText = " SELECT " + BaseUserEntity.TableName + ".*         , ( SELECT " + BaseRoleEntity.FieldRealName + "  FROM " + BaseRoleEntity.TableName + " WHERE " + BaseRoleEntity.FieldId + " = " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldRoleId + ") AS RoleName    FROM " + BaseUserEntity.TableName + " WHERE " + BaseUserEntity.FieldDeletionStateCode + "= 0  AND " + BaseUserEntity.FieldIsVisible + "= 1  ORDER BY " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public DataTable GetDTByDepartment(string departmentId)
        {
            string str2 = " SELECT " + BaseUserEntity.TableName + ".*  FROM " + BaseUserEntity.TableName;
            string str3 = str2 + " WHERE (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDeletionStateCode + " = 0 ";
            string commandText = str3 + " AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldEnabled + " = 1 ) ";
            if (!string.IsNullOrEmpty(departmentId))
            {
                string str4 = commandText;
                string str5 = str4 + " AND ((" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDepartmentId + " = '" + departmentId + "') ";
                commandText = str5 + " OR " + BaseUserEntity.FieldId + " IN ( SELECT " + BaseUserOrganizeEntity.FieldUserId + "   FROM " + BaseUserOrganizeEntity.TableName + "  WHERE (" + BaseUserOrganizeEntity.TableName + "." + BaseUserOrganizeEntity.FieldDeletionStateCode + " = 0 )        AND (" + BaseUserOrganizeEntity.TableName + "." + BaseUserOrganizeEntity.FieldDepartmentId + " = '" + departmentId + "'))) ";
            }
            string str6 = commandText;
            commandText = str6 + " ORDER BY " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public DataTable GetDTByIds(string[] userIds)
        {
            string commandText = ((" SELECT " + BaseUserEntity.TableName + ".*         , ( SELECT " + BaseRoleEntity.FieldRealName + "  FROM " + BaseRoleEntity.TableName + " WHERE " + BaseRoleEntity.FieldId + " = " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldRoleId + ") AS RoleName    FROM " + BaseUserEntity.TableName) + " WHERE Id IN (" + BaseBusinessLogic.ObjectsToList(userIds) + ")") + " ORDER BY " + BaseUserEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public DataTable GetDTByOrganizes(string[] organizeIds)
        {
            string str = BaseBusinessLogic.ObjectsToList(organizeIds);
            string commandText = " SELECT *  FROM " + BaseUserEntity.TableName + " WHERE (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDeletionStateCode + " = 0 )        AND (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldWorkgroupId + " IN ( " + str + ")        OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDepartmentId + " IN (" + str + ")        OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldCompanyId + " IN (" + str + "))  OR " + BaseUserEntity.FieldId + " IN ( SELECT " + BaseUserOrganizeEntity.FieldUserId + "   FROM " + BaseUserOrganizeEntity.TableName + "  WHERE (" + BaseUserOrganizeEntity.TableName + "." + BaseUserOrganizeEntity.FieldDeletionStateCode + " = 0 )        AND (" + BaseUserOrganizeEntity.TableName + "." + BaseUserOrganizeEntity.FieldWorkgroupId + " IN ( " + str + ")        OR " + BaseUserOrganizeEntity.TableName + "." + BaseUserOrganizeEntity.FieldDepartmentId + " IN (" + str + ")        OR " + BaseUserOrganizeEntity.TableName + "." + BaseUserOrganizeEntity.FieldCompanyId + " IN (" + str + ")))  ORDER BY " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public DataTable GetDTByRole(string roleId)
        {
            string commandText = " SELECT * FROM " + BaseUserEntity.TableName + " WHERE " + BaseUserEntity.FieldEnabled + "=1        AND " + BaseUserEntity.FieldDeletionStateCode + "= 0        AND (" + BaseUserEntity.FieldRoleId + "='" + roleId + "'            OR " + BaseUserEntity.FieldId + " IN            (SELECT + " + BaseUserRoleEntity.FieldUserId + "              FROM " + BaseUserRoleEntity.TableName + "             WHERE " + BaseUserRoleEntity.FieldRoleId + " = '" + roleId + "'               AND " + BaseUserRoleEntity.FieldEnabled + " = 1                AND " + BaseUserRoleEntity.FieldDeletionStateCode + " = 0))  ORDER BY  " + BaseUserEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public BaseUserEntity GetEntity(int? id)
        {
            return new BaseUserEntity(this.GetDT(BaseUserEntity.FieldId, id));
        }

        public BaseUserEntity GetEntity(string id)
        {
            return new BaseUserEntity(this.GetDT(BaseUserEntity.FieldId, id));
        }

        public BaseUserEntity GetEntityByCode(string userCode)
        {
            BaseUserEntity entity = null;
            string[] names = new string[] { BaseUserEntity.FieldCode, BaseUserEntity.FieldDeletionStateCode, BaseUserEntity.FieldEnabled };
            object[] values = new object[] { userCode, 0, 1 };
            DataTable dT = this.GetDT(names, values);
            if (dT.Rows.Count > 0)
            {
                entity = new BaseUserEntity(dT);
            }
            return entity;
        }

        public string GetLogOnCount(int days)
        {
            string commandText = " SELECT COUNT(Id) AS UserCount    FROM " + base.CurrentTableName + "  WHERE Enabled = 1 AND (DATEADD(d, " + days.ToString() + ", " + BaseUserEntity.FieldLastVisit + ") > " + base.DbHelper.GetDBNow() + ")";
            return base.DbHelper.ExecuteScalar(commandText).ToString();
        }

        public string GetOnLineCount()
        {
            string commandText = " SELECT COUNT(Id) AS UserCount    FROM " + base.CurrentTableName + "  WHERE Enabled = 1 AND UserOnLine = 1";
            return base.DbHelper.ExecuteScalar(commandText).ToString();
        }

        public DataTable GetOnLineStateDT()
        {
            string commandText = string.Empty;
            commandText = " SELECT " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + ", " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldUserOnLine + " FROM " + BaseUserEntity.TableName + " WHERE " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldEnabled + " = 1  AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldIsVisible + " = 1  AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDeletionStateCode + " = 0  AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldLastVisit + " IS NOT NULL ";
            if (base.DbHelper.CurrentDbType == CurrentDbType.SqlServer)
            {
                string str2 = commandText;
                string[] strArray2 = new string[] { str2, " AND (DATEADD (s, ", (BaseSystemInfo.OnLineTime0ut + 5).ToString(), ", ", BaseUserEntity.FieldLastVisit, ") > ", base.DbHelper.GetDBNow(), ")" };
                commandText = string.Concat(strArray2);
            }
            return base.DbHelper.Fill(commandText);
        }

        public string GetPublicKey()
        {
            return this.GetProperty(base.UserInfo.Id, BaseUserEntity.FieldPublicKey);
        }

        public string GetPublicKey(string userId)
        {
            return this.GetProperty(userId, BaseUserEntity.FieldPublicKey);
        }

        public string GetRegistrationCount(int days)
        {
            string commandText = " SELECT COUNT(Id) AS UserCount    FROM " + base.CurrentTableName + "  WHERE Enabled = 1 AND (DATEADD(d, " + days.ToString() + ", " + BaseUserEntity.FieldCreateOn + ") > " + base.DbHelper.GetDBNow() + ")";
            return base.DbHelper.ExecuteScalar(commandText).ToString();
        }

        public string[] GetUserIds(string organizeId)
        {
            string[] strArray = null;
            string[] strArray2 = null;
            string[] strArray3 = null;
            if (!string.IsNullOrEmpty(organizeId))
            {
                BaseStaffManager manager = new BaseStaffManager(base.DbHelper);
                strArray = manager.GetIds(BaseStaffEntity.FieldCompanyId, organizeId, BaseStaffEntity.FieldUserId);
                strArray2 = manager.GetIds(BaseStaffEntity.FieldDepartmentId, organizeId, BaseStaffEntity.FieldUserId);
                strArray3 = manager.GetIds(BaseStaffEntity.FieldWorkgroupId, organizeId, BaseStaffEntity.FieldUserId);
            }
            return BaseBusinessLogic.Concat(new string[][] { strArray, strArray2, strArray3 });
        }

        public string[] GetUserIds(string[] organizeIds, string[] roleIds)
        {
            string[] strArray = null;
            string[] strArray2 = null;
            string[] strArray3 = null;
            if (organizeIds != null)
            {
                BaseStaffManager manager = new BaseStaffManager(base.DbHelper);
                strArray = manager.GetIds(BaseStaffEntity.FieldCompanyId, (object[]) organizeIds, BaseStaffEntity.FieldUserId);
                strArray2 = manager.GetIds(BaseStaffEntity.FieldDepartmentId, (object[]) organizeIds, BaseStaffEntity.FieldUserId);
                strArray3 = manager.GetIds(BaseStaffEntity.FieldWorkgroupId, (object[]) organizeIds, BaseStaffEntity.FieldUserId);
            }
            string[] strArray4 = null;
            if (roleIds != null)
            {
                strArray4 = new BaseUserRoleManager(base.DbHelper).GetIds(BaseUserRoleEntity.FieldRoleId, (object[]) roleIds, BaseUserRoleEntity.FieldUserId);
            }
            return BaseBusinessLogic.Concat(new string[][] { strArray, strArray2, strArray3, strArray4 });
        }

        public string[] GetUserIds(string[] userIds, string[] organizeIds, string[] roleIds)
        {
            return BaseBusinessLogic.Concat(new string[][] { userIds, this.GetUserIds(organizeIds, roleIds) });
        }

        public BaseUserInfo Impersonation(string id, out string statusCode)
        {
            BaseUserInfo userInfo = null;
            DataTable dTById = this.GetDTById(id);
            BaseUserEntity userEntity = new BaseUserEntity();
            userEntity.GetFrom(dTById);
            if ((!base.UserInfo.Id.Equals(id) && BaseSystemInfo.CheckOnLine) && (userEntity.UserOnLine > 0))
            {
                statusCode = StatusCode.ErrorOnLine.ToString();
                return userInfo;
            }
            userInfo = this.ConvertToUserInfo(userEntity);
            if (userEntity.IsStaff.Equals("1"))
            {
                BaseStaffEntity staffEntity = new BaseStaffEntity();
                BaseStaffManager manager = new BaseStaffManager(base.DbHelper, base.UserInfo);
                DataTable dataTable = manager.GetDTById(id);
                staffEntity.GetFrom(dataTable);
                userInfo = manager.ConvertToUserInfo(staffEntity, userInfo);
            }
            statusCode = StatusCode.OK.ToString();
            this.ChangeOnLine(id);
            return userInfo;
        }

        public bool IsAdministrator(string userId)
        {
            if (userId.Equals("Administrator"))
            {
                return true;
            }
            BaseUserEntity entity = this.GetEntity(userId);
            if ((entity.Code != null) && entity.Code.Equals("Administrator"))
            {
                return true;
            }
            if ((entity.UserName != null) && entity.UserName.Equals("Administrator"))
            {
                return true;
            }
            BaseRoleManager manager = new BaseRoleManager(base.DbHelper, base.UserInfo);
            BaseRoleEntity entity2 = null;
            if (entity.RoleId.HasValue)
            {
                entity2 = manager.GetEntity(entity.RoleId);
                if ((entity2.Code != null) && entity2.Code.Equals(DefaultRole.Administrators.ToString()))
                {
                    return true;
                }
            }
            string[] allRoleIds = new BaseUserRoleManager(base.DbHelper, base.UserInfo).GetAllRoleIds(userId);
            for (int i = 0; i < allRoleIds.Length; i++)
            {
                if (allRoleIds[i].Equals(DefaultRole.Administrators.ToString()))
                {
                    return true;
                }
                entity2 = manager.GetEntity(allRoleIds[i]);
                if ((entity2.Code != null) && entity2.Code.Equals(DefaultRole.Administrators.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        
        //public BaseUserInfo LogOn(string userName, string password, bool createNewOpenId = false, string ipAddress = null, string macAddress = null, bool checkUserPassword = true)//C# 4.0 才支持缺省参数
        public BaseUserInfo LogOn(string userName, string password, bool createNewOpenId, string ipAddress, string macAddress, bool checkUserPassword)
        {
            BaseUserInfo info = null;
            string realName = string.Empty;
            if (base.UserInfo != null)
            {
                realName = base.UserInfo.RealName;
            }
            if ((ipAddress == null) && (base.UserInfo != null))
            {
                ipAddress = base.UserInfo.IPAddress;
            }
            //检查同时在线限制
            if ((BaseSystemInfo.OnLineLimit > 0) && this.CheckOnLineLimit())
            {
                base.ReturnStatusCode = StatusCode.ErrorOnLineLimit.ToString();
                BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userName, ipAddress, AppMessage.MSG0089 + BaseSystemInfo.OnLineLimit.ToString());
                return info;
            }

            if (BaseSystemInfo.CheckPasswordStrength)
            {
                base.ReturnStatusCode = StatusCode.ErrorLogOn.ToString();
            }
            else
            {
                base.ReturnStatusCode = StatusCode.UserNotFound.ToString();
            }

            string[] names = new string[] { BaseUserEntity.FieldDeletionStateCode, BaseUserEntity.FieldUserName };
            object[] values = new object[] { 0, userName };
            DataTable dT = this.GetDT(names, values);
            BaseUserEntity userEntity = null;
            if (dT.Rows.Count > 1)
            {
                base.ReturnStatusCode = StatusCode.UserDuplicate.ToString();
            }
            else if (dT.Rows.Count == 1)
            {
                if (checkUserPassword && BaseSystemInfo.ServerEncryptPassword) //判断密码是否加密保存
                {
                    password = this.EncryptUserPassword(password); 
                }
                foreach (DataRow row in dT.Rows)
                {
                    userEntity = new BaseUserEntity(row);
                    if (!string.IsNullOrEmpty(userEntity.AuditStatus) && userEntity.AuditStatus.EndsWith(AuditStatus.WaitForAudit.ToString()))  //用户审核状态
                    {
                        base.ReturnStatusCode = AuditStatus.WaitForAudit.ToString();
                        BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userName, ipAddress, AppMessage.MSG0078);
                        return info;
                    }
                    if (userEntity.Enabled == 0)
                    {
                        base.ReturnStatusCode = StatusCode.LogOnDeny.ToString();
                        BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userName, ipAddress, AppMessage.MSG0079);
                        return info;
                    }
                    if (userEntity.Enabled == -1)  //用户未被激活
                    {
                        base.ReturnStatusCode = StatusCode.UserNotActive.ToString();
                        BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userName, ipAddress, AppMessage.MSG0080);
                        return info;
                    }
                    if (userEntity.AllowStartTime.HasValue) //用户登录时间限制
                    {
                        userEntity.AllowStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, userEntity.AllowStartTime.Value.Hour, userEntity.AllowStartTime.Value.Minute, userEntity.AllowStartTime.Value.Second);
                        DateTime now = DateTime.Now;
                        DateTime? allowStartTime = userEntity.AllowStartTime;
                        if (allowStartTime.HasValue ? (now < allowStartTime.GetValueOrDefault()) : false)
                        {
                            base.ReturnStatusCode = StatusCode.UserLocked.ToString();
                            BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userName, ipAddress, AppMessage.MSG0081 + userEntity.AllowStartTime.Value.ToString("HH:mm"));
                            return info;
                        }
                    }
                    if (userEntity.AllowEndTime.HasValue) //用户登录时间限制
                    {
                        userEntity.AllowEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, userEntity.AllowEndTime.Value.Hour, userEntity.AllowEndTime.Value.Minute, userEntity.AllowEndTime.Value.Second);
                        DateTime time15 = DateTime.Now;
                        DateTime? allowEndTime = userEntity.AllowEndTime;
                        if (allowEndTime.HasValue ? (time15 > allowEndTime.GetValueOrDefault()) : false)
                        {
                            base.ReturnStatusCode = StatusCode.UserLocked.ToString();
                            BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userName, ipAddress, AppMessage.MSG0082 + userEntity.AllowEndTime.Value.ToString("HH:mm"));
                            return info;
                        }
                    }
                    if (userEntity.LockStartDate.HasValue) //锁定日期限制
                    {
                        DateTime time17 = DateTime.Now;
                        DateTime? lockStartDate = userEntity.LockStartDate;
                        if (lockStartDate.HasValue ? (time17 > lockStartDate.GetValueOrDefault()) : false)
                        {
                            if (userEntity.LockEndDate.HasValue)
                            {
                                DateTime time18 = DateTime.Now;
                                DateTime? lockEndDate = userEntity.LockEndDate;
                                if (!(lockEndDate.HasValue ? (time18 < lockEndDate.GetValueOrDefault()) : false))
                                {
                                    goto Label_0624;
                                }
                            }
                            base.ReturnStatusCode = StatusCode.UserLocked.ToString();
                            BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userName, ipAddress, AppMessage.MSG0083 + userEntity.LockStartDate.Value.ToString("yyyy-MM-dd"));
                            return info;
                        }
                    }
                Label_0624:
                    if (userEntity.LockEndDate.HasValue)
                    {
                        DateTime time20 = DateTime.Now;
                        DateTime? nullable22 = userEntity.LockEndDate;
                        if (nullable22.HasValue ? (time20 < nullable22.GetValueOrDefault()) : false)
                        {
                            base.ReturnStatusCode = StatusCode.UserLocked.ToString();
                            BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userName, ipAddress, AppMessage.MSG0084 + userEntity.LockEndDate.Value.ToString("yyyy-MM-dd"));
                            return info;
                        }
                    }
                    //检查IP地址限制
                    if (BaseSystemInfo.CheckIPAddress && !this.IsAdministrator(userEntity.Id.ToString()))
                    {
                        string[] strArray2 = new string[2];
                        string[] strArray3 = new string[2];
                        strArray2[0] = BaseParameterEntity.FieldParameterId;
                        strArray2[1] = BaseParameterEntity.FieldCategoryId;
                        strArray3[0] = userEntity.Id.ToString();
                        strArray3[1] = "IPAddress";
                        BaseParameterManager manager = new BaseParameterManager(base.DbHelper);
                        if ((manager.Exists(strArray2, strArray3) && !string.IsNullOrEmpty(ipAddress)) && !this.CheckIPAddress(ipAddress, userEntity.Id.ToString()))
                        {
                            base.ReturnStatusCode = StatusCode.ErrorIPAddress.ToString();
                            BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, ipAddress, ipAddress, AppMessage.MSG0085);
                            return info;
                        }
                        strArray3[1] = "MacAddress";
                        if ((manager.Exists(strArray2, strArray3) && !string.IsNullOrEmpty(macAddress)) && !this.CheckMacAddress(macAddress, userEntity.Id.ToString()))
                        {
                            base.ReturnStatusCode = StatusCode.ErrorMacAddress.ToString();
                            BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, macAddress, ipAddress, AppMessage.MSG0086);
                            return info;
                        }
                    }
                    //处理重复登录
                    if ((((base.UserInfo != null) && !base.UserInfo.Id.Equals(userEntity.Id.ToString())) && BaseSystemInfo.CheckOnLine) && (userEntity.UserOnLine > 0))
                    {
                        base.ReturnStatusCode = StatusCode.ErrorOnLine.ToString();
                        BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userName, ipAddress, AppMessage.MSG0087);
                        return info;
                    }
                    if (!string.IsNullOrEmpty(userEntity.UserPassword) || !string.IsNullOrEmpty(password))
                    {
                        bool flag = true;
                        if (string.IsNullOrEmpty(userEntity.UserPassword))
                        {
                            if (!string.IsNullOrEmpty(password))
                            {
                                flag = false;
                            }
                        }
                        else if (string.IsNullOrEmpty(password))
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = userEntity.UserPassword.Equals(password);
                        }
                        if (!flag)
                        {
                            BaseLogManager.Instance.Add(base.DbHelper, userEntity.Id.ToString(), userEntity.RealName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userEntity.RealName, ipAddress, AppMessage.MSG0088);
                            if (BaseSystemInfo.CheckPasswordStrength)
                            {
                                base.ReturnStatusCode = StatusCode.ErrorLogOn.ToString();
                                return info;
                            }
                            base.ReturnStatusCode = StatusCode.PasswordError.ToString();
                            return info;
                        }
                    }
                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        this.SetProperty(userEntity.Id, BaseUserEntity.FieldIPAddress, ipAddress);
                    }
                    if (!string.IsNullOrEmpty(macAddress))
                    {
                        this.SetProperty(userEntity.Id, BaseUserEntity.FieldMACAddress, macAddress);
                    }

                    //用户验证完成
                    base.ReturnStatusCode = StatusCode.OK.ToString();
                    this.ChangeOnLine(userEntity.Id.ToString());
                    info = this.ConvertToUserInfo(userEntity);
                    //bool flag1 = userEntity.IsStaff == 1;  //未填充StaffID
                    info.IPAddress = ipAddress;
                    info.MACAddress = macAddress;
                    info.Password = password;
                    info.IsAdministrator = this.IsAdministrator(info.Id);
                    break;
                }
            }
            if (base.ReturnStatusCode == StatusCode.OK.ToString())
            {
                BaseLogManager.Instance.Add(base.DbHelper, userEntity.Id.ToString(), userEntity.RealName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userEntity.RealName, ipAddress, AppMessage.BaseUserManager_LogOnSuccess);
                if (string.IsNullOrEmpty(info.OpenId))
                {
                    createNewOpenId = true;
                }
                if (createNewOpenId)
                {
                    info.OpenId = this.UpdateVisitDate(userEntity.Id.ToString(), createNewOpenId);
                    return info;
                }
                this.UpdateVisitDate(userEntity.Id.ToString(), false);
                return info;
            }
            BaseLogManager.Instance.Add(base.DbHelper, userName, realName, "LogOn", AppMessage.BaseUserManager, "LogOn", AppMessage.BaseUserManager_LogOn, userName, ipAddress, AppMessage.MSG0090);
            return info;
        }

        //用来模拟默认参数
        public BaseUserInfo LogOn(string userName, string password)
        {
            bool createNewOpenId = false;
            string ipAddress = null;
            string macAddress = null;
            bool checkUserPassword = true;
            return LogOn(userName, password, createNewOpenId, ipAddress, macAddress, checkUserPassword);
        }

        //public BaseUserInfo LogOnByOpenId(string openId, string ipAddress = null, string macAddress = null) //C# 4.0 才支持缺省参数
        public BaseUserInfo LogOnByOpenId(string openId, string ipAddress, string macAddress)
        {
            BaseUserInfo info = null;
            base.ReturnStatusCode = StatusCode.UserNotFound.ToString();
            if (!string.IsNullOrEmpty(openId))
            {
                DataTable dT = new BaseUserManager(base.DbHelper).GetDT(BaseUserEntity.FieldOpenId, openId);
                if (dT.Rows.Count == 1)
                {
                    BaseUserEntity entity = new BaseUserEntity(dT);
                    info = this.LogOn(entity.UserName, entity.UserPassword, false, ipAddress, macAddress, false);
                }
            }
            return info;
        }
        //用来模拟默认参数
        public BaseUserInfo LogOnByOpenId(string openId)
        {
            string ipAddress = null;
            string macAddress = null;
            return LogOnByOpenId(openId, ipAddress, macAddress);
        }

        //public BaseUserInfo LogOnByUserName(string userName, string ipAddress = null, string macAddress = null)//C# 4.0 才支持缺省参数
        public BaseUserInfo LogOnByUserName(string userName, string ipAddress, string macAddress)
        {
            BaseUserInfo info = null;
            base.ReturnStatusCode = StatusCode.UserNotFound.ToString();
            if (!string.IsNullOrEmpty(userName))
            {
                DataTable dT = new BaseUserManager(base.DbHelper).GetDT(BaseUserEntity.FieldUserName, userName);
                if (dT.Rows.Count == 1)
                {
                    BaseUserEntity entity = new BaseUserEntity(dT);
                    info = this.LogOn(entity.UserName, entity.UserPassword, true, ipAddress, macAddress, true);
                }
            }
            return info;
        }

        //用来模拟默认参数
        public BaseUserInfo LogOnByUserName(string userName)
        {
            string ipAddress = null;
            string macAddress = null;
            return LogOnByUserName(userName, ipAddress, macAddress);
        }

        /// <summary>
        /// 用户下线，修改用户访问记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int OnExit(string userId)
        {
            int num = 0;
            if (!BaseSystemInfo.UpdateVisit)
            {
                return num;
            }
            string commandText = string.Empty;
            commandText = " UPDATE " + BaseUserEntity.TableName + " SET " + BaseUserEntity.FieldPreviousVisit + " = " + BaseUserEntity.FieldLastVisit + " , " + BaseUserEntity.FieldUserOnLine + " = 0 , " + BaseUserEntity.FieldLastVisit + " = " + base.DbHelper.GetDBNow() + "  WHERE (" + BaseUserEntity.FieldId + " = '" + userId + "')";
            return (num + base.DbHelper.ExecuteNonQuery(commandText));
        }

        //public int OnLine(string userId, int onLineState = 1) //C# 4.0 才支持缺省参数
        /// <summary>
        /// 设置用户在线状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="onLineState"></param>
        /// <returns></returns>
        public int OnLine(string userId, int onLineState)
        {
            int num = 0;
            if (!BaseSystemInfo.UpdateVisit)
            {
                return num;
            }
            string commandText = string.Empty;
            commandText = " UPDATE " + BaseUserEntity.TableName + "    SET " + BaseUserEntity.FieldUserOnLine + " = " + onLineState.ToString() + " , " + BaseUserEntity.FieldLastVisit + " = " + base.DbHelper.GetDBNow() + "  WHERE (" + BaseUserEntity.FieldId + " = '" + userId + "')";
            return (num + base.DbHelper.ExecuteNonQuery(commandText));
        }
        //用来模拟默认参数
        public int OnLine(string userId)
        {
            int onLineState = 1;
            return OnLine(userId, onLineState);
        }

        public int Reset()
        {
            int num = 0;
            num += this.ResetData();
            return (num + this.ResetVisitInfo());
        }

        private int ResetData()
        {
            int num = 0;
            string commandText = " DELETE FROM " + BaseUserEntity.TableName + " WHERE Id NOT IN (SELECT Id FROM " + BaseStaffEntity.TableName + ") ";
            num += base.DbHelper.ExecuteNonQuery(commandText);
            commandText = " UPDATE " + BaseUserEntity.TableName + " SET SortCode = " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldSortCode + " FROM " + BaseStaffEntity.TableName + " WHERE " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + " = " + BaseStaffEntity.TableName + "." + BaseStaffEntity.FieldId;
            return (num + base.DbHelper.ExecuteNonQuery(commandText));
        }

        private int ResetVisitInfo()
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(BaseUserEntity.TableName);
            builder.SetNull(BaseUserEntity.FieldFirstVisit);
            builder.SetNull(BaseUserEntity.FieldPreviousVisit);
            builder.SetNull(BaseUserEntity.FieldLastVisit);
            builder.SetValue(BaseUserEntity.FieldLogOnCount, 0, null);
            return builder.EndUpdate();
        }

        public int ResetVisitInfo(string[] ids)
        {
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i].Length > 0)
                {
                    num += this.ResetVisitInfo(ids[i]);
                }
            }
            return num;
        }

        private int ResetVisitInfo(string id)
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(BaseUserEntity.TableName);
            builder.SetNull(BaseUserEntity.FieldFirstVisit);
            builder.SetNull(BaseUserEntity.FieldPreviousVisit);
            builder.SetNull(BaseUserEntity.FieldLastVisit);
            builder.SetValue(BaseUserEntity.FieldLogOnCount, 0, null);
            builder.SetWhere(BaseUserEntity.FieldId, id);
            return builder.EndUpdate();
        }

        //public DataTable Search(string search = null, string[] roleIds = null, bool? enabled = true, string auditStates = null) //C# 4.0 才支持缺省参数
        public DataTable Search(string search, string[] roleIds, bool? enabled, string auditStates)
        {
            return this.Search(string.Empty, search, roleIds, enabled, auditStates);
        }

        public DataTable Search(string permissionScopeItemCode, string search, string[] roleIds, bool? enabled, string auditStates)
        {
            search = BaseBusinessLogic.GetSearchString(search);
            string commandText = " SELECT " + BaseUserEntity.TableName + ".* ," + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldRealName + " AS RoleName  FROM " + BaseUserEntity.TableName + "      LEFT OUTER JOIN " + BaseRoleEntity.TableName + "      ON " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldRoleId + " = " + BaseRoleEntity.TableName + "." + BaseRoleEntity.FieldId + " WHERE " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDeletionStateCode + " = 0  AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldIsVisible + " = 1 ";
            if (!string.IsNullOrEmpty(search))
            {
                string str4 = commandText;
                commandText = str4 + " AND (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldUserName + " LIKE '" + search + "' OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldCode + " LIKE '" + search + "' OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldRealName + " LIKE '" + search + "' OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDepartmentName + " LIKE '" + search + "' OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDescription + " LIKE '" + search + "')";
            }
            if (!string.IsNullOrEmpty(auditStates))
            {
                string str5 = commandText;
                commandText = str5 + " AND (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldAuditStatus + " = '" + auditStates + "')";
            }
            if (enabled.HasValue)
            {
                object obj2 = commandText;
                commandText = string.Concat(new object[] { obj2, " AND (", BaseUserEntity.TableName, ".", BaseUserEntity.FieldEnabled, " = ", enabled.Value ? 1 : 0, ")" });
            }
            if ((roleIds != null) && (roleIds.Length > 0))
            {
                string str2 = BaseBusinessLogic.ArrayToList(roleIds, "'");
                string str6 = commandText;
                string str7 = str6 + " AND (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldRoleId + " IN (" + str2 + ") ";
                commandText = str7 + "      OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + " IN (SELECT " + BaseUserRoleEntity.FieldUserId + " FROM " + BaseUserRoleEntity.TableName + " WHERE " + BaseUserRoleEntity.FieldRoleId + " IN (" + str2 + ")))";
            }
            if (!base.UserInfo.IsAdministrator && BaseSystemInfo.UsePermissionScope)
            {
                string id = new BasePermissionItemManager(base.DbHelper, base.UserInfo).GetId(BasePermissionItemEntity.FieldCode, permissionScopeItemCode);
                if (!string.IsNullOrEmpty(id))
                {
                    string[] organizeIds = new BaseUserScopeManager(base.DbHelper, base.UserInfo).GetOrganizeIds(base.UserInfo.Id, id);
                    int num = 0;
                    if (BaseBusinessLogic.Exists(organizeIds, num.ToString()))
                    {
                        string str8 = commandText;
                        commandText = str8 + " AND (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + " = NULL ) ";
                    }
                    int num2 = -7;
                    if (BaseBusinessLogic.Exists(organizeIds, num2.ToString()))
                    {
                        string[] userIds = new BasePermissionScopeManager(base.DbHelper, base.UserInfo).GetUserIds(base.UserInfo.Id, permissionScopeItemCode);
                        string str9 = commandText;
                        commandText = str9 + " AND (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + " IN (" + BaseBusinessLogic.ObjectsToList(userIds) + ")) ";
                    }
                    int num3 = -6;
                    if (BaseBusinessLogic.Exists(organizeIds, num3.ToString()))
                    {
                        string str10 = commandText;
                        commandText = str10 + " AND (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldId + " = " + base.UserInfo.Id + ") ";
                    }
                    int num4 = -5;
                    if (BaseBusinessLogic.Exists(organizeIds, num4.ToString()))
                    {
                        object obj3 = commandText;
                        commandText = string.Concat(new object[] { obj3, " AND (", BaseUserEntity.TableName, ".", BaseUserEntity.FieldWorkgroupId, " = ", base.UserInfo.WorkgroupId, ") " });
                    }
                    int num5 = -4;
                    if (BaseBusinessLogic.Exists(organizeIds, num5.ToString()))
                    {
                        object obj4 = commandText;
                        commandText = string.Concat(new object[] { obj4, " AND (", BaseUserEntity.TableName, ".", BaseUserEntity.FieldDepartmentId, " = ", base.UserInfo.DepartmentId, ") " });
                    }
                    int num6 = -2;
                    if (BaseBusinessLogic.Exists(organizeIds, num6.ToString()))
                    {
                        object obj5 = commandText;
                        commandText = string.Concat(new object[] { obj5, " AND (", BaseUserEntity.TableName, ".", BaseUserEntity.FieldCompanyId, " = ", base.UserInfo.CompanyId, ") " });
                    }
                    //BaseBusinessLogic.Exists(organizeIds, -1.ToString());
                    BaseBusinessLogic.Exists(organizeIds, "-1");
                }
            }
            string str11 = commandText;
            commandText = str11 + " ORDER BY " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText);
        }

        public DataTable SearchByDepartment(string departmentId, string searchValue)
        {
            string str2 = " SELECT " + BaseUserEntity.TableName + ".*  FROM " + BaseUserEntity.TableName;
            string str3 = str2 + " WHERE (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDeletionStateCode + " = 0 ";
            string commandText = str3 + " AND " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldEnabled + " = 1 ) ";
            if (!string.IsNullOrEmpty(departmentId))
            {
                string[] ids = new BaseOrganizeManager(base.DbHelper, base.UserInfo).GetChildrensId(BaseOrganizeEntity.FieldId, departmentId, BaseOrganizeEntity.FieldParentId);
                if ((ids != null) && (ids.Length > 0))
                {
                    string str4 = commandText;
                    commandText = str4 + " AND (" + BaseUserEntity.TableName + "." + BaseUserEntity.FieldCompanyId + " IN (" + BaseBusinessLogic.ArrayToList(ids) + ") OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldDepartmentId + " IN (" + BaseBusinessLogic.ArrayToList(ids) + ") OR " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldWorkgroupId + " IN (" + BaseBusinessLogic.ArrayToList(ids) + "))";
                }
            }
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            searchValue = searchValue.Trim();
            if (!string.IsNullOrEmpty(searchValue))
            {
                string str5 = commandText;
                string str6 = str5 + " AND (" + BaseUserEntity.FieldUserName + " LIKE " + base.DbHelper.GetParameter(BaseUserEntity.FieldUserName);
                string str7 = str6 + " OR " + BaseUserEntity.FieldCode + " LIKE " + base.DbHelper.GetParameter(BaseUserEntity.FieldCode);
                string str8 = str7 + " OR " + BaseUserEntity.FieldRealName + " LIKE " + base.DbHelper.GetParameter(BaseUserEntity.FieldRealName);
                commandText = str8 + " OR " + BaseUserEntity.FieldDepartmentName + " LIKE " + base.DbHelper.GetParameter(BaseUserEntity.FieldDepartmentName) + ")";
                if (searchValue.IndexOf("%") < 0)
                {
                    searchValue = "%" + searchValue + "%";
                }
                list.Add(base.DbHelper.MakeParameter(BaseUserEntity.FieldUserName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseUserEntity.FieldCode, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseUserEntity.FieldRealName, searchValue));
                list.Add(base.DbHelper.MakeParameter(BaseUserEntity.FieldDepartmentName, searchValue));
            }
            string str9 = commandText;
            commandText = str9 + " ORDER BY " + BaseUserEntity.TableName + "." + BaseUserEntity.FieldSortCode;
            return base.DbHelper.Fill(commandText, list.ToArray());
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseUserEntity baseUserEntity)
        {
            sqlBuilder.SetValue(BaseUserEntity.FieldCode, baseUserEntity.Code, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldUserName, baseUserEntity.UserName, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldRealName, baseUserEntity.RealName, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldRoleId, baseUserEntity.RoleId, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldSecurityLevel, baseUserEntity.SecurityLevel, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldUserFrom, baseUserEntity.UserFrom, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldWorkCategory, baseUserEntity.WorkCategory, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldCompanyId, baseUserEntity.CompanyId, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldCompanyName, baseUserEntity.CompanyName, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldDepartmentId, baseUserEntity.DepartmentId, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldDepartmentName, baseUserEntity.DepartmentName, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldWorkgroupId, baseUserEntity.WorkgroupId, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldWorkgroupName, baseUserEntity.WorkgroupName, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldGender, baseUserEntity.Gender, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldMobile, baseUserEntity.Mobile, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldTelephone, baseUserEntity.Telephone, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldBirthday, baseUserEntity.Birthday, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldDuty, baseUserEntity.Duty, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldTitle, baseUserEntity.Title, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldUserPassword, baseUserEntity.UserPassword, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldCommunicationPassword, baseUserEntity.CommunicationPassword, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldOICQ, baseUserEntity.OICQ, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldEmail, baseUserEntity.Email, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldLang, baseUserEntity.Lang, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldTheme, baseUserEntity.Theme, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldAllowStartTime, baseUserEntity.AllowStartTime, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldAllowEndTime, baseUserEntity.AllowEndTime, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldLockStartDate, baseUserEntity.LockStartDate, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldLockEndDate, baseUserEntity.LockEndDate, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldFirstVisit, baseUserEntity.FirstVisit, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldPreviousVisit, baseUserEntity.PreviousVisit, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldLastVisit, baseUserEntity.LastVisit, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldLogOnCount, baseUserEntity.LogOnCount, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldIsStaff, baseUserEntity.IsStaff, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldAuditStatus, baseUserEntity.AuditStatus, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldIsVisible, baseUserEntity.IsVisible, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldUserOnLine, baseUserEntity.UserOnLine, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldIPAddress, baseUserEntity.IPAddress, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldMACAddress, baseUserEntity.MACAddress, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldOpenId, baseUserEntity.OpenId, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldQuestion, baseUserEntity.Question, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldAnswerQuestion, baseUserEntity.AnswerQuestion, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldUserAddressId, baseUserEntity.UserAddressId, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldDeletionStateCode, baseUserEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldEnabled, baseUserEntity.Enabled, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldSortCode, baseUserEntity.SortCode, null);
            sqlBuilder.SetValue(BaseUserEntity.FieldDescription, baseUserEntity.Description, null);
        }

        public virtual int SetPassword(string userId, string password)
        {
            int num = 0;
            if (BaseSystemInfo.ServerEncryptPassword)
            {
                password = this.EncryptUserPassword(password);
            }
            string[] targetFields = new string[] { BaseUserEntity.FieldUserPassword, BaseUserEntity.FieldChangePasswordDate };
            object[] objArray2 = new object[2];
            objArray2[0] = password;
            object[] targetValues = objArray2;
            num = this.SetProperty(userId, targetFields, targetValues);
            if (num == 1)
            {
                base.ReturnStatusCode = StatusCode.SetPasswordOK.ToString();
                return num;
            }
            base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public virtual bool SignedPassword(string signedPassword)
        {
            bool flag = false;
            if (!BaseSystemInfo.CheckPasswordStrength || !string.IsNullOrEmpty(signedPassword))
            {
                if (BaseSystemInfo.ServerEncryptPassword)
                {
                    signedPassword = this.EncryptUserPassword(signedPassword);
                }
                BaseUserEntity entity = new BaseUserEntity();
                entity.GetFrom(this.GetDTById(base.UserInfo.Id));
                if ((entity.CommunicationPassword == null) && (signedPassword.Length == 0))
                {
                    return flag;
                }
                if (entity.SignedPassword.Equals(signedPassword))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public int Update(BaseUserEntity baseUserEntity)
        {
            return this.UpdateEntity(baseUserEntity);
        }

        public int Update(BaseUserEntity userEntity, out string statusCode)
        {
            int num = 0;
            if (this.Exists(BaseUserEntity.FieldUserName, userEntity.UserName, BaseUserEntity.FieldDeletionStateCode, "0", userEntity.Id))
            {
                statusCode = StatusCode.ErrorUserExist.ToString();
                return num;
            }
            if ((!string.IsNullOrEmpty(userEntity.Code) && (userEntity.Code.Length > 0)) && this.Exists(BaseUserEntity.FieldCode, userEntity.Code, BaseUserEntity.FieldDeletionStateCode, "0", userEntity.Id))
            {
                statusCode = StatusCode.ErrorCodeExist.ToString();
                return num;
            }
            num = this.UpdateEntity(userEntity);
            if (num == 0)
            {
                statusCode = StatusCode.ErrorDeleted.ToString();
                return num;
            }
            statusCode = StatusCode.OKUpdate.ToString();
            return num;
        }

        public int UpdateEntity(BaseUserEntity baseUserEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseUserEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseUserEntity.FieldId, baseUserEntity.Id);
            return sqlBuilder.EndUpdate();
        }

        //private string UpdateVisitDate(string userId, bool createOpenId = false) //C# 4.0 才支持缺省参数
        private string UpdateVisitDate(string userId, bool createOpenId)
        {
            string str = string.Empty;
            string commandText = string.Empty;
            if (BaseSystemInfo.UpdateVisit)
            {
                commandText = " UPDATE " + BaseUserEntity.TableName + "    SET " + BaseUserEntity.FieldFirstVisit + " = " + base.DbHelper.GetDBNow() + "  WHERE (" + BaseUserEntity.FieldId + " = '" + userId + "') AND " + BaseUserEntity.FieldFirstVisit + " IS NULL";
                base.DbHelper.ExecuteNonQuery(commandText);
                string str3 = " UPDATE " + BaseUserEntity.TableName + " SET " + BaseUserEntity.FieldPreviousVisit + " = " + BaseUserEntity.FieldLastVisit + " , " + BaseUserEntity.FieldUserOnLine + " = 1 , " + BaseUserEntity.FieldLastVisit + " = " + base.DbHelper.GetDBNow() + " , " + BaseUserEntity.FieldLogOnCount + " = " + BaseUserEntity.FieldLogOnCount + " + 1 ";
                commandText = str3 + "  WHERE (" + BaseUserEntity.FieldId + " = '" + userId + "')";
                base.DbHelper.ExecuteNonQuery(commandText);
            }
            if (createOpenId)
            {
                str = BaseBusinessLogic.NewGuid();
                commandText = " UPDATE " + BaseUserEntity.TableName + "    SET " + BaseUserEntity.FieldOpenId + " = '" + str + "'  WHERE (" + BaseUserEntity.FieldId + " = '" + userId + "')";
                base.DbHelper.ExecuteNonQuery(commandText);
            }
            return str;
        }

        //用来模拟默认参数
        private string UpdateVisitDate(string userId)
        {
            bool createOpenId = false;
            return UpdateVisitDate(userId, createOpenId);
        }
        public bool UserIsLogOn(BaseUserInfo userInfo)
        {
            string[] names = new string[] { BaseUserEntity.FieldId, BaseUserEntity.FieldDeletionStateCode, BaseUserEntity.FieldEnabled, BaseUserEntity.FieldOpenId };
            object[] values = new object[] { userInfo.Id, 0, 1, userInfo.OpenId };
            string id = this.GetId(names, values);
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            if (!userInfo.Id.Equals(id))
            {
                return false;
            }
            return true;
        }
    }
}

