//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

/// 修改纪录
/// 
///		2012.06.29 版本：2.0 sunmiao 修改
///		
/// 版本：2.0
/// 

namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;

    public class BaseUserRoleManager : BaseManager, IBaseManager
    {
        public BaseUserRoleManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseUserRoleEntity.TableName;
        }

        public BaseUserRoleManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseUserRoleManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseUserRoleManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseUserRoleManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseUserRoleManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseUserRoleEntity baseUserRoleEntity)
        {
            return this.AddEntity(baseUserRoleEntity);
        }

        public string Add(BaseUserRoleEntity baseUserRoleEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseUserRoleEntity);
        }

        public string AddEntity(BaseUserRoleEntity baseUserRoleEntity)
        {
            string sequence = string.Empty;
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(BaseUserRoleEntity.TableName, BaseUserRoleEntity.FieldId);
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseUserRoleEntity.FieldId, baseUserRoleEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseUserRoleEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseUserRoleEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (!baseUserRoleEntity.Id.HasValue)
                {
                    if (string.IsNullOrEmpty(sequence))
                    {
                        sequence = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseUserRoleEntity.Id = new int?(int.Parse(sequence));
                }
                sqlBuilder.SetValue(BaseUserRoleEntity.FieldId, baseUserRoleEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseUserRoleEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserRoleEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserRoleEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserRoleEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserRoleEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserRoleEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserRoleEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return sequence;
        }

        public int AddToRole(string[] userIds, string roleId)
        {
            int num = 0;
            for (int i = 0; i < userIds.Length; i++)
            {
                this.AddToRole(userIds[i], roleId);
                num++;
            }
            return num;
        }

        public int AddToRole(string[] userIds, string[] roleIds)
        {
            int num = 0;
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < roleIds.Length; j++)
                {
                    this.AddToRole(userIds[i], roleIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int AddToRole(string userId, string[] roleIds)
        {
            int num = 0;
            for (int i = 0; i < roleIds.Length; i++)
            {
                this.AddToRole(userId, roleIds[i]);
                num++;
            }
            return num;
        }

        public string AddToRole(string userId, string roleId)
        {
            BaseUserRoleEntity baseUserRoleEntity = new BaseUserRoleEntity {
                UserId = new int?(int.Parse(userId)),
                RoleId = new int?(int.Parse(roleId)),
                Enabled = 1,
                DeletionStateCode = 0
            };
            return this.Add(baseUserRoleEntity);
        }

        public int ClearRoleUser(string roleId)
        {
            BaseUserManager manager = new BaseUserManager(base.DbHelper, base.UserInfo);
            return (manager.SetProperty(BaseUserEntity.FieldRoleId, roleId, BaseUserEntity.FieldRoleId, null) + this.Delete(BaseUserRoleEntity.FieldRoleId, roleId));
        }

        public int ClearUserRole(string userId)
        {
            int num = 0;
            BaseUserManager manager = new BaseUserManager(base.DbHelper, base.UserInfo);
            num += manager.SetProperty(BaseUserEntity.FieldId, userId, BaseUserEntity.FieldRoleId, null);
            return (num + this.Delete(BaseUserRoleEntity.FieldUserId, userId));
        }

        public int Delete(int id)
        {
            return this.Delete(BaseUserRoleEntity.FieldId, id);
        }

        public string[] GetAllRoleIds(string userId)
        {
            string commandText = " SELECT RoleId FROM Base_User WHERE (Id = " + userId + ") AND (DeletionStateCode = 0) AND (Enabled = 1)  UNION SELECT RoleId FROM Base_UserRole WHERE (UserId = " + userId + ") AND (RoleId IN (SELECT Id FROM Base_Role WHERE (DeletionStateCode = 0))) AND (DeletionStateCode = 0) ";
            return BaseBusinessLogic.FieldToArray(base.DbHelper.Fill(commandText), BaseUserRoleEntity.FieldRoleId);
        }

        public BaseUserRoleEntity GetEntity(int id)
        {
            return new BaseUserRoleEntity(this.GetDT(BaseUserRoleEntity.FieldId, id));
        }

        public string[] GetRoleIds(string userId)
        {
            string commandText = " SELECT RoleId     FROM Base_UserRole   WHERE (UserId = " + userId + ")     AND (RoleId IN (SELECT Id FROM Base_Role WHERE (DeletionStateCode = 0))) AND (DeletionStateCode = 0) ";
            return BaseBusinessLogic.FieldToArray(base.DbHelper.Fill(commandText), BaseUserRoleEntity.FieldRoleId);
        }

        public string GetRoleName(string id)
        {
            string property = this.GetProperty(id, BaseUserRoleEntity.FieldRoleId);
            return DbLogic.GetProperty(base.DbHelper, BaseRoleEntity.TableName, BaseRoleEntity.FieldId, property, BaseRoleEntity.FieldRealName);
        }

        public string GetUserFullName(string id)
        {
            string property = this.GetProperty(id, BaseUserRoleEntity.FieldUserId);
            return DbLogic.GetProperty(base.DbHelper, BaseStaffEntity.TableName, BaseStaffEntity.FieldId, property, BaseStaffEntity.FieldRealName);
        }

        public string[] GetUserIds(string roleId)
        {
            string commandText = " SELECT Id AS USERID FROM Base_User WHERE (RoleId = " + roleId + ") AND (DeletionStateCode = 0) AND (Enabled = 1)  UNION SELECT UserId FROM Base_UserRole WHERE (RoleId = " + roleId + ") AND (UserId IN (SELECT Id FROM Base_User WHERE (DeletionStateCode = 0))) AND (DeletionStateCode = 0) ";
            return BaseBusinessLogic.FieldToArray(base.DbHelper.Fill(commandText), BaseUserRoleEntity.FieldUserId);
        }

        public string[] GetUserIds(string[] roleIds)
        {
            string[] strArray = null;
            if ((roleIds != null) && (roleIds.Length > 0))
            {
                string commandText = " SELECT Id AS USERID FROM Base_User WHERE (RoleId IN ( " + BaseBusinessLogic.ArrayToList(roleIds) + ")) AND (DeletionStateCode = 0) AND (Enabled = 1)  UNION SELECT UserId FROM Base_UserRole WHERE (RoleId IN (" + BaseBusinessLogic.ArrayToList(roleIds) + ")) AND (UserId IN (SELECT Id FROM Base_User WHERE (DeletionStateCode = 0))) AND (DeletionStateCode = 0) ";
                strArray = BaseBusinessLogic.FieldToArray(base.DbHelper.Fill(commandText), BaseUserRoleEntity.FieldUserId);
            }
            return strArray;
        }

        public int RemoveFormRole(string[] userIds, string[] roleIds)
        {
            int num = 0;
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < roleIds.Length; j++)
                {
                    num += this.RemoveFormRole(userIds[i], roleIds[j]);
                }
            }
            return num;
        }

        public int RemoveFormRole(string[] userIds, string roleId)
        {
            int num = 0;
            //BaseUserManager manager = new BaseUserManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                num += this.RemoveFormRole(userIds[i], roleId);
                //manager.SetProperty(BaseUserEntity.FieldId, userIds[i], BaseUserEntity.FieldRoleId, roleId, BaseUserEntity.FieldRoleId, null);
            }
            return num;
        }

        public int RemoveFormRole(string userId, string[] roleIds)
        {
            int num = 0;
            for (int i = 0; i < roleIds.Length; i++)
            {
                num += this.RemoveFormRole(userId, roleIds[i]);
            }
            return num;
        }

        public int RemoveFormRole(string userId, string roleId)
        {
            BaseUserManager manager = new BaseUserManager(base.DbHelper, base.UserInfo);
            string role = manager.GetProperty(userId, BaseUserEntity.FieldRoleId);
            if (role == roleId) //清理User中的缺省角色
                manager.SetProperty(BaseUserEntity.FieldId, userId, BaseUserEntity.FieldRoleId, roleId, BaseUserEntity.FieldRoleId, null);

            string[] names = new string[2];
            object[] values = new object[2];
            names[0] = BaseUserRoleEntity.FieldUserId;
            values[0] = userId;
            names[1] = BaseUserRoleEntity.FieldRoleId;
            values[1] = roleId;
            return this.Delete(names, values);
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseUserRoleEntity baseUserRoleEntity)
        {
            sqlBuilder.SetValue(BaseUserRoleEntity.FieldUserId, baseUserRoleEntity.UserId, null);
            sqlBuilder.SetValue(BaseUserRoleEntity.FieldRoleId, baseUserRoleEntity.RoleId, null);
            sqlBuilder.SetValue(BaseUserRoleEntity.FieldEnabled, baseUserRoleEntity.Enabled, null);
            sqlBuilder.SetValue(BaseUserRoleEntity.FieldDescription, baseUserRoleEntity.Description, null);
            sqlBuilder.SetValue(BaseUserRoleEntity.FieldDeletionStateCode, baseUserRoleEntity.DeletionStateCode, null);
        }

        public int Update(BaseUserRoleEntity baseUserRoleEntity)
        {
            return this.UpdateEntity(baseUserRoleEntity);
        }

        public int UpdateEntity(BaseUserRoleEntity baseUserRoleEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BaseUserRoleEntity.TableName);
            this.SetEntity(sqlBuilder, baseUserRoleEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserRoleEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserRoleEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserRoleEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseUserRoleEntity.FieldId, baseUserRoleEntity.Id);
            return sqlBuilder.EndUpdate();
        }

        public bool UserInRole(string userId, string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode))
            {
                return false;
            }
            string str = new BaseRoleManager(base.DbHelper, base.UserInfo).GetId(BaseRoleEntity.FieldDeletionStateCode, 0, BaseRoleEntity.FieldCode, roleCode);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return BaseBusinessLogic.Exists(this.GetAllRoleIds(userId), str);
        }
    }
}

