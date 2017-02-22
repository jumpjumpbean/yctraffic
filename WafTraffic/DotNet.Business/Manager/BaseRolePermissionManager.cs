namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;

    public class BaseRolePermissionManager : BaseManager, IBaseManager
    {
        public BaseRolePermissionManager()
        {
            base.CurrentTableName = BasePermissionEntity.TableName;
        }

        public BaseRolePermissionManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseRolePermissionManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseRolePermissionManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseRolePermissionManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseRolePermissionManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string[] GetPermissionItemIds(string roleId)
        {
            string[] names = new string[4];
            string[] values = new string[4];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseRoleEntity.TableName;
            names[1] = BasePermissionEntity.FieldDeletionStateCode;
            values[1] = "0";
            names[2] = BasePermissionEntity.FieldEnabled;
            values[2] = "1";
            names[3] = BasePermissionEntity.FieldResourceId;
            values[3] = roleId;
            return this.GetIds(names, values, BasePermissionEntity.FieldPermissionItemId);
        }

        public string[] GetRoleIds(string permissionItemId)
        {
            string[] names = new string[4];
            string[] values = new string[4];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseRoleEntity.TableName;
            names[1] = BasePermissionEntity.FieldDeletionStateCode;
            values[1] = "0";
            names[2] = BasePermissionEntity.FieldEnabled;
            values[2] = "1";
            names[3] = BasePermissionEntity.FieldPermissionItemId;
            values[3] = permissionItemId;
            return this.GetIds(names, values, BasePermissionEntity.FieldResourceId);
        }

        public string Grant(string roleId, string permissionItemId)
        {
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            return this.Grant(permissionManager, roleId, permissionItemId);
        }

        public int Grant(string[] roleIds, string[] permissionItemIds)
        {
            int num = 0;
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < roleIds.Length; i++)
            {
                for (int j = 0; j < permissionItemIds.Length; j++)
                {
                    this.Grant(permissionManager, roleIds[i], permissionItemIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int Grant(string roleId, string[] permissionItemIds)
        {
            int num = 0;
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < permissionItemIds.Length; i++)
            {
                this.Grant(permissionManager, roleId, permissionItemIds[i]);
                num++;
            }
            return num;
        }

        public int Grant(string[] roleIds, string permissionItemId)
        {
            int num = 0;
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < roleIds.Length; i++)
            {
                this.Grant(permissionManager, roleIds[i], permissionItemId);
                num++;
            }
            return num;
        }

        private string Grant(BasePermissionManager permissionManager, string roleId, string permissionItemId)
        {
            BasePermissionEntity resourcePermissionEntity = new BasePermissionEntity {
                ResourceCategory = BaseRoleEntity.TableName,
                ResourceId = roleId,
                PermissionId = new int?(int.Parse(permissionItemId)),
                Enabled = 1
            };
            return permissionManager.Add(resourcePermissionEntity);
        }

        public int Revoke(string[] roleIds, string[] permissionItemIds)
        {
            int num = 0;
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < roleIds.Length; i++)
            {
                for (int j = 0; j < permissionItemIds.Length; j++)
                {
                    num += this.Revoke(permissionManager, roleIds[i], permissionItemIds[j]);
                }
            }
            return num;
        }

        public int Revoke(string roleId, string permissionItemId)
        {
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            return this.Revoke(permissionManager, roleId, permissionItemId);
        }

        public int Revoke(string roleId, string[] permissionItemIds)
        {
            int num = 0;
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < permissionItemIds.Length; i++)
            {
                num += this.Revoke(permissionManager, roleId, permissionItemIds[i]);
            }
            return num;
        }

        public int Revoke(string[] roleIds, string permissionItemId)
        {
            int num = 0;
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < roleIds.Length; i++)
            {
                num += this.Revoke(permissionManager, roleIds[i], permissionItemId);
            }
            return num;
        }

        private int Revoke(BasePermissionManager permissionManager, string roleId, string permissionItemId)
        {
            string[] names = new string[3];
            string[] values = new string[3];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseRoleEntity.TableName;
            names[1] = BasePermissionEntity.FieldResourceId;
            values[1] = roleId;
            names[2] = BasePermissionEntity.FieldPermissionItemId;
            values[2] = permissionItemId;
            return permissionManager.Delete(names, values);
        }

        public int RevokeAll(string roleId)
        {
            BasePermissionManager manager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            string[] names = new string[2];
            string[] values = new string[2];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseRoleEntity.TableName;
            names[1] = BasePermissionEntity.FieldResourceId;
            values[1] = roleId;
            return manager.Delete(names, values);
        }
    }
}

