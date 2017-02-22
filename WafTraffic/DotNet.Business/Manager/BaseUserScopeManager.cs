namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    public class BaseUserScopeManager : BaseManager, IBaseManager
    {
        public BaseUserScopeManager()
        {
            base.CurrentTableName = BasePermissionScopeEntity.TableName;
        }

        public BaseUserScopeManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseUserScopeManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseUserScopeManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public int ClearUserPermissionScope(string userId, string permissionItemCode)
        {
            string[] names = new string[3];
            string[] values = new string[3];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[2] = this.GetIdByCode(permissionItemCode);
            BasePermissionScopeManager manager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return manager.Delete(names, values);
        }

        public string GetIdByCode(string permissionItemCode)
        {
            BasePermissionItemManager manager = new BasePermissionItemManager(base.DbHelper);
            return manager.GetIdByAdd(permissionItemCode);
        }

        public string[] GetModuleIds(string userId, string permissionItemCode)
        {
            string[] names = new string[4];
            string[] values = new string[4];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BaseModuleEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[3] = this.GetIdByCode(permissionItemCode);
            return BaseBusinessLogic.FieldToArray(this.GetDT(names, values), BasePermissionScopeEntity.FieldTargetId);
        }

        public string[] GetOrganizeIds(string userId, string permissionItemCode)
        {
            string[] names = new string[4];
            string[] values = new string[4];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BaseOrganizeEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[3] = this.GetIdByCode(permissionItemCode);
            return BaseBusinessLogic.FieldToArray(this.GetDT(names, values), BasePermissionScopeEntity.FieldTargetId);
        }

        public string[] GetPermissionItemIds(string userId, string permissionItemCode)
        {
            string[] names = new string[4];
            string[] values = new string[4];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BasePermissionItemEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[3] = this.GetIdByCode(permissionItemCode);
            return BaseBusinessLogic.FieldToArray(this.GetDT(names, values), BasePermissionScopeEntity.FieldTargetId);
        }

        public string[] GetRoleIds(string userId, string permissionItemCode)
        {
            string[] names = new string[4];
            string[] values = new string[4];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BaseRoleEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[3] = this.GetIdByCode(permissionItemCode);
            return BaseBusinessLogic.FieldToArray(this.GetDT(names, values), BasePermissionScopeEntity.FieldTargetId);
        }

        public string[] GetUserIds(string userId, string permissionItemCode)
        {
            string[] names = new string[4];
            string[] values = new string[4];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BaseUserEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[3] = this.GetIdByCode(permissionItemCode);
            return BaseBusinessLogic.FieldToArray(this.GetDT(names, values), BasePermissionScopeEntity.FieldTargetId);
        }

        public string GrantModule(string userId, string permissionItemCode, string grantModuleId)
        {
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return this.GrantModule(permissionScopeManager, userId, permissionItemCode, grantModuleId);
        }

        private string GrantModule(BasePermissionScopeManager permissionScopeManager, string userId, string permissionItemCode, string grantModuleId)
        {
            BasePermissionScopeEntity baseResourcePermissionScopeEntity = new BasePermissionScopeEntity();
            string idByCode = this.GetIdByCode(permissionItemCode);
            if (string.IsNullOrEmpty(idByCode))
            {
                return string.Empty;
            }
            baseResourcePermissionScopeEntity.PermissionId = new int?(int.Parse(idByCode));
            baseResourcePermissionScopeEntity.ResourceCategory = BaseUserEntity.TableName;
            baseResourcePermissionScopeEntity.ResourceId = userId;
            baseResourcePermissionScopeEntity.TargetCategory = BaseModuleEntity.TableName;
            baseResourcePermissionScopeEntity.TargetId = grantModuleId;
            baseResourcePermissionScopeEntity.Enabled = 1;
            baseResourcePermissionScopeEntity.DeletionStateCode = 0;
            return permissionScopeManager.Add(baseResourcePermissionScopeEntity);
        }

        public int GrantModules(string userId, string permissionItemCode, string[] grantModuleIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < grantModuleIds.Length; i++)
            {
                this.GrantModule(permissionScopeManager, userId, permissionItemCode, grantModuleIds[i]);
                num++;
            }
            return num;
        }

        public int GrantModules(string[] userIds, string permissionItemCode, string[] grantModuleIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < grantModuleIds.Length; j++)
                {
                    this.GrantModule(permissionScopeManager, userIds[i], permissionItemCode, grantModuleIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int GrantModules(string[] userIds, string permissionItemCode, string grantModuleId)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.GrantModule(permissionScopeManager, userIds[i], permissionItemCode, grantModuleId);
                num++;
            }
            return num;
        }

        public string GrantOrganize(string userId, string permissionItemCode, string grantOrganizeId)
        {
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return this.GrantOrganize(permissionScopeManager, userId, permissionItemCode, grantOrganizeId);
        }

        private string GrantOrganize(BasePermissionScopeManager permissionScopeManager, string userId, string permissionItemCode, string grantOrganizeId)
        {
            string str = string.Empty;
            string[] names = new string[5];
            string[] values = new string[5];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BaseOrganizeEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            values[3] = grantOrganizeId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = this.GetIdByCode(permissionItemCode);
            BasePermissionScopeEntity baseResourcePermissionScopeEntity = new BasePermissionScopeEntity();
            DataTable dT = new DataTable();
            if (!this.Exists(names, values))
            {
                baseResourcePermissionScopeEntity.PermissionId = new int?(int.Parse(this.GetIdByCode(permissionItemCode)));
                baseResourcePermissionScopeEntity.ResourceCategory = BaseUserEntity.TableName;
                baseResourcePermissionScopeEntity.ResourceId = userId;
                baseResourcePermissionScopeEntity.TargetCategory = BaseOrganizeEntity.TableName;
                baseResourcePermissionScopeEntity.TargetId = grantOrganizeId;
                baseResourcePermissionScopeEntity.Enabled = 1;
                baseResourcePermissionScopeEntity.DeletionStateCode = 0;
                str = permissionScopeManager.Add(baseResourcePermissionScopeEntity);
                int num2 = 0;
                if (grantOrganizeId != num2.ToString())
                {
                    values[3] = 0.ToString();
                    if (this.Exists(names, values))
                    {
                        dT = permissionScopeManager.GetDT(names, values);
                        if ((dT != null) && (dT.Rows.Count > 0))
                        {
                            permissionScopeManager.DeleteEntity(dT.Rows[0]["Id"].ToString());
                        }
                    }
                    return str;
                }
                string[] strArray3 = new string[4];
                string[] strArray4 = new string[4];
                strArray3[0] = names[0];
                strArray4[0] = values[0];
                strArray3[1] = names[1];
                strArray4[1] = values[1];
                strArray3[2] = names[2];
                strArray4[2] = values[2];
                strArray3[3] = names[4];
                strArray4[3] = values[4];
                dT = permissionScopeManager.GetDT(strArray3, strArray4);
                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    int num4 = 0;
                    if (dT.Rows[i]["TargetId"].ToString() != num4.ToString())
                    {
                        permissionScopeManager.DeleteEntity(dT.Rows[0]["Id"].ToString());
                    }
                }
            }
            return str;
        }

        public int GrantOrganizes(string[] userIds, string permissionItemCode, string grantOrganizeId)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.GrantOrganize(permissionScopeManager, userIds[i], permissionItemCode, grantOrganizeId);
                num++;
            }
            return num;
        }

        public int GrantOrganizes(string[] userIds, string permissionItemCode, string[] grantOrganizeIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < grantOrganizeIds.Length; j++)
                {
                    this.GrantOrganize(permissionScopeManager, userIds[i], permissionItemCode, grantOrganizeIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int GrantOrganizes(string userId, string permissionItemCode, string[] grantOrganizeIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < grantOrganizeIds.Length; i++)
            {
                this.GrantOrganize(permissionScopeManager, userId, permissionItemCode, grantOrganizeIds[i]);
                num++;
            }
            return num;
        }

        public string GrantPermissionItem(string userId, string permissionItemCode, string grantPermissionId)
        {
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return this.GrantPermissionItem(permissionScopeManager, userId, permissionItemCode, grantPermissionId);
        }

        private string GrantPermissionItem(BasePermissionScopeManager permissionScopeManager, string userId, string permissionItemCode, string grantPermissionId)
        {
            BasePermissionScopeEntity baseResourcePermissionScopeEntity = new BasePermissionScopeEntity {
                PermissionId = new int?(int.Parse(this.GetIdByCode(permissionItemCode))),
                ResourceCategory = BaseUserEntity.TableName,
                ResourceId = userId,
                TargetCategory = BasePermissionItemEntity.TableName,
                TargetId = grantPermissionId,
                Enabled = 1,
                DeletionStateCode = 0
            };
            return permissionScopeManager.Add(baseResourcePermissionScopeEntity);
        }

        public int GrantPermissionItemes(string userId, string permissionItemCode, string[] grantPermissionIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < grantPermissionIds.Length; i++)
            {
                this.GrantPermissionItem(permissionScopeManager, userId, permissionItemCode, grantPermissionIds[i]);
                num++;
            }
            return num;
        }

        public int GrantPermissionItemes(string[] userIds, string permissionItemCode, string grantPermissionId)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.GrantPermissionItem(permissionScopeManager, userIds[i], permissionItemCode, grantPermissionId);
                num++;
            }
            return num;
        }

        public int GrantPermissionItems(string[] userIds, string permissionItemCode, string[] grantPermissionIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < grantPermissionIds.Length; j++)
                {
                    this.GrantPermissionItem(permissionScopeManager, userIds[i], permissionItemCode, grantPermissionIds[j]);
                    num++;
                }
            }
            return num;
        }

        public string GrantRole(string userId, string permissionItemCode, string grantRoleId)
        {
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return this.GrantRole(permissionScopeManager, userId, permissionItemCode, grantRoleId);
        }

        private string GrantRole(BasePermissionScopeManager permissionScopeManager, string userId, string permissionItemCode, string grantRoleId)
        {
            BasePermissionScopeEntity baseResourcePermissionScopeEntity = new BasePermissionScopeEntity {
                PermissionId = new int?(int.Parse(this.GetIdByCode(permissionItemCode))),
                ResourceCategory = BaseUserEntity.TableName,
                ResourceId = userId,
                TargetCategory = BaseRoleEntity.TableName,
                TargetId = grantRoleId,
                Enabled = 1,
                DeletionStateCode = 0
            };
            return permissionScopeManager.Add(baseResourcePermissionScopeEntity);
        }

        public int GrantRoles(string[] userIds, string permissionItemCode, string grantRoleId)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.GrantRole(permissionScopeManager, userIds[i], permissionItemCode, grantRoleId);
                num++;
            }
            return num;
        }

        public int GrantRoles(string[] userIds, string permissionItemCode, string[] grantRoleIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < grantRoleIds.Length; j++)
                {
                    this.GrantRole(permissionScopeManager, userIds[i], permissionItemCode, grantRoleIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int GrantRoles(string userId, string permissionItemCode, string[] grantRoleIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < grantRoleIds.Length; i++)
            {
                this.GrantRole(permissionScopeManager, userId, permissionItemCode, grantRoleIds[i]);
                num++;
            }
            return num;
        }

        public string GrantUser(string userId, string permissionItemCode, string grantUserId)
        {
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return this.GrantUser(permissionScopeManager, userId, permissionItemCode, grantUserId);
        }

        private string GrantUser(BasePermissionScopeManager permissionScopeManager, string userId, string permissionItemCode, string grantUserId)
        {
            string str = string.Empty;
            string[] names = new string[5];
            string[] values = new string[5];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BaseUserEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            values[3] = grantUserId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = this.GetIdByCode(permissionItemCode);
            if (!this.Exists(names, values))
            {
                BasePermissionScopeEntity baseResourcePermissionScopeEntity = new BasePermissionScopeEntity {
                    PermissionId = new int?(int.Parse(this.GetIdByCode(permissionItemCode))),
                    ResourceCategory = BaseUserEntity.TableName,
                    ResourceId = userId,
                    TargetCategory = BaseUserEntity.TableName,
                    TargetId = grantUserId,
                    Enabled = 1,
                    DeletionStateCode = 0
                };
                return permissionScopeManager.Add(baseResourcePermissionScopeEntity);
            }
            return str;
        }

        public int GrantUsers(string[] userIds, string permissionItemCode, string[] grantUserIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < grantUserIds.Length; j++)
                {
                    this.GrantUser(permissionScopeManager, userIds[i], permissionItemCode, grantUserIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int GrantUsers(string[] userIds, string permissionItemCode, string grantUserId)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.GrantUser(permissionScopeManager, userIds[i], permissionItemCode, grantUserId);
                num++;
            }
            return num;
        }

        public int GrantUsers(string userId, string permissionItemCode, string[] grantUserIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < grantUserIds.Length; i++)
            {
                this.GrantUser(permissionScopeManager, userId, permissionItemCode, grantUserIds[i]);
                num++;
            }
            return num;
        }

        public int RevokeAll(string userId)
        {
            string[] names = new string[2];
            string[] values = new string[2];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            BasePermissionScopeManager manager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return manager.Delete(names, values);
        }

        public int RevokeModule(string userId, string permissionItemCode, string revokeModuleId)
        {
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return this.RevokeModule(permissionScopeManager, userId, permissionItemCode, revokeModuleId);
        }

        private int RevokeModule(BasePermissionScopeManager permissionScopeManager, string userId, string permissionItemCode, string revokeModuleId)
        {
            string[] names = new string[5];
            string[] values = new string[5];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BaseModuleEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            values[3] = revokeModuleId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = this.GetIdByCode(permissionItemCode);
            return permissionScopeManager.Delete(names, values);
        }

        public int RevokeModules(string[] userIds, string permissionItemCode, string[] revokeModuleIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < revokeModuleIds.Length; j++)
                {
                    this.RevokeModule(permissionScopeManager, userIds[i], permissionItemCode, revokeModuleIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int RevokeModules(string userId, string permissionItemCode, string[] revokeModuleIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < revokeModuleIds.Length; i++)
            {
                this.RevokeModule(permissionScopeManager, userId, permissionItemCode, revokeModuleIds[i]);
                num++;
            }
            return num;
        }

        public int RevokeModules(string[] userIds, string permissionItemCode, string revokeModuleId)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.RevokeModule(permissionScopeManager, userIds[i], permissionItemCode, revokeModuleId);
                num++;
            }
            return num;
        }

        public int RevokeOrganize(string userId, string permissionItemCode, string revokeOrganizeId)
        {
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return this.RevokeOrganize(permissionScopeManager, userId, permissionItemCode, revokeOrganizeId);
        }

        private int RevokeOrganize(BasePermissionScopeManager permissionScopeManager, string userId, string permissionItemCode, string revokeOrganizeId)
        {
            string[] names = new string[5];
            string[] values = new string[5];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BaseOrganizeEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            values[3] = revokeOrganizeId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = this.GetIdByCode(permissionItemCode);
            return permissionScopeManager.Delete(names, values);
        }

        public int RevokeOrganizes(string[] userIds, string permissionItemCode, string revokeOrganizeId)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.RevokeOrganize(permissionScopeManager, userIds[i], permissionItemCode, revokeOrganizeId);
                num++;
            }
            return num;
        }

        public int RevokeOrganizes(string[] userIds, string permissionItemCode, string[] revokeOrganizeIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < revokeOrganizeIds.Length; j++)
                {
                    this.RevokeOrganize(permissionScopeManager, userIds[i], permissionItemCode, revokeOrganizeIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int RevokeOrganizes(string userId, string permissionItemCode, string[] revokeOrganizeIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < revokeOrganizeIds.Length; i++)
            {
                this.RevokeOrganize(permissionScopeManager, userId, permissionItemCode, revokeOrganizeIds[i]);
                num++;
            }
            return num;
        }

        public int RevokePermissionItem(string userId, string permissionItemCode, string revokePermissionId)
        {
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return this.RevokePermissionItem(permissionScopeManager, userId, permissionItemCode, revokePermissionId);
        }

        private int RevokePermissionItem(BasePermissionScopeManager permissionScopeManager, string userId, string permissionItemCode, string revokePermissionId)
        {
            string[] names = new string[5];
            string[] values = new string[5];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BasePermissionItemEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            values[3] = revokePermissionId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = this.GetIdByCode(permissionItemCode);
            return permissionScopeManager.Delete(names, values);
        }

        public int RevokePermissionItems(string userId, string permissionItemCode, string[] revokePermissionIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < revokePermissionIds.Length; i++)
            {
                this.RevokePermissionItem(permissionScopeManager, userId, permissionItemCode, revokePermissionIds[i]);
                num++;
            }
            return num;
        }

        public int RevokePermissionItems(string[] userIds, string permissionItemCode, string revokePermissionId)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.RevokePermissionItem(permissionScopeManager, userIds[i], permissionItemCode, revokePermissionId);
                num++;
            }
            return num;
        }

        public int RevokePermissionItems(string[] userIds, string permissionItemCode, string[] revokePermissionIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < revokePermissionIds.Length; j++)
                {
                    this.RevokePermissionItem(permissionScopeManager, userIds[i], permissionItemCode, revokePermissionIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int RevokeRole(string userId, string permissionItemCode, string revokeRoleId)
        {
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return this.RevokeRole(permissionScopeManager, userId, permissionItemCode, revokeRoleId);
        }

        private int RevokeRole(BasePermissionScopeManager permissionScopeManager, string userId, string permissionItemCode, string revokeRoleId)
        {
            string[] names = new string[5];
            string[] values = new string[5];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BaseRoleEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            values[3] = revokeRoleId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = this.GetIdByCode(permissionItemCode);
            return permissionScopeManager.Delete(names, values);
        }

        public int RevokeRoles(string[] userIds, string permissionItemCode, string revokeRoleId)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.RevokeRole(permissionScopeManager, userIds[i], permissionItemCode, revokeRoleId);
                num++;
            }
            return num;
        }

        public int RevokeRoles(string userId, string permissionItemCode, string[] revokeRoleIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < revokeRoleIds.Length; i++)
            {
                this.RevokeRole(permissionScopeManager, userId, permissionItemCode, revokeRoleIds[i]);
                num++;
            }
            return num;
        }

        public int RevokeRoles(string[] userIds, string permissionItemCode, string[] revokeRoleIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < revokeRoleIds.Length; j++)
                {
                    this.RevokeRole(permissionScopeManager, userIds[i], permissionItemCode, revokeRoleIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int RevokeUser(string userId, string permissionItemCode, string revokeUserId)
        {
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            return this.RevokeUser(permissionScopeManager, userId, permissionItemCode, revokeUserId);
        }

        private int RevokeUser(BasePermissionScopeManager permissionScopeManager, string userId, string permissionItemCode, string revokeUserId)
        {
            string[] names = new string[5];
            string[] values = new string[5];
            names[0] = BasePermissionScopeEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionScopeEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionScopeEntity.FieldTargetCategory;
            values[2] = BaseUserEntity.TableName;
            names[3] = BasePermissionScopeEntity.FieldTargetId;
            values[3] = revokeUserId;
            names[4] = BasePermissionScopeEntity.FieldPermissionItemId;
            values[4] = this.GetIdByCode(permissionItemCode);
            return permissionScopeManager.Delete(names, values);
        }

        public int RevokeUsers(string[] userIds, string permissionItemCode, string revokeUserId)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.RevokeUser(permissionScopeManager, userIds[i], permissionItemCode, revokeUserId);
                num++;
            }
            return num;
        }

        public int RevokeUsers(string userId, string permissionItemCode, string[] revokeUserIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < revokeUserIds.Length; i++)
            {
                this.RevokeUser(permissionScopeManager, userId, permissionItemCode, revokeUserIds[i]);
                num++;
            }
            return num;
        }

        public int RevokeUsers(string[] userIds, string permissionItemCode, string[] revokeUserIds)
        {
            int num = 0;
            BasePermissionScopeManager permissionScopeManager = new BasePermissionScopeManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < revokeUserIds.Length; j++)
                {
                    this.RevokeUser(permissionScopeManager, userIds[i], permissionItemCode, revokeUserIds[j]);
                    num++;
                }
            }
            return num;
        }
    }
}

