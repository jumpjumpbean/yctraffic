namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;

    public class BaseUserPermissionManager : BaseManager, IBaseManager
    {
        public BaseUserPermissionManager()
        {
            base.CurrentTableName = BasePermissionEntity.TableName;
        }

        public BaseUserPermissionManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseUserPermissionManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseUserPermissionManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public bool CheckPermission(string userId, string permissionItemCode)
        {
            BasePermissionItemManager manager = new BasePermissionItemManager(base.DbHelper);
            if (string.IsNullOrEmpty(manager.GetIdByCode(permissionItemCode)))
            {
                return false;
            }
            string[] names = new string[3];
            string[] values = new string[3];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionEntity.FieldEnabled;
            values[2] = "1";
            return DbLogic.Exists(base.DbHelper, base.CurrentTableName, names, values);
        }

        public string[] GetPermissionItemIds(string userId)
        {
            string[] names = new string[4];
            string[] values = new string[4];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionEntity.FieldDeletionStateCode;
            values[1] = "0";
            names[2] = BasePermissionEntity.FieldEnabled;
            values[2] = "1";
            names[3] = BasePermissionEntity.FieldResourceId;
            values[3] = userId;
            return this.GetIds(names, values, BasePermissionEntity.FieldPermissionItemId);
        }

        public string[] GetUserIds(string permissionItemId)
        {
            string[] names = new string[4];
            string[] values = new string[4];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionEntity.FieldDeletionStateCode;
            values[1] = "0";
            names[2] = BasePermissionEntity.FieldEnabled;
            values[2] = "1";
            names[3] = BasePermissionEntity.FieldPermissionItemId;
            values[3] = permissionItemId;
            return this.GetIds(names, values, BasePermissionEntity.FieldResourceId);
        }

        public string Grant(string userId, string permissionItemId)
        {
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            return this.Grant(permissionManager, string.Empty, userId, permissionItemId);
        }

        public int Grant(string[] userIds, string[] permissionItemIds)
        {
            int num = 0;
            string[] batchSequence = new BaseSequenceManager(base.DbHelper).GetBatchSequence(BasePermissionEntity.TableName, userIds.Length * permissionItemIds.Length);
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < permissionItemIds.Length; j++)
                {
                    this.Grant(permissionManager, batchSequence[(i * permissionItemIds.Length) + j], userIds[i], permissionItemIds[j]);
                    num++;
                }
            }
            return num;
        }

        public int Grant(string userId, string[] permissionItemIds)
        {
            int num = 0;
            string[] batchSequence = new BaseSequenceManager(base.DbHelper).GetBatchSequence(BasePermissionEntity.TableName, permissionItemIds.Length);
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < permissionItemIds.Length; i++)
            {
                this.Grant(permissionManager, batchSequence[i], userId, permissionItemIds[i]);
                num++;
            }
            return num;
        }

        public int Grant(string[] userIds, string permissionItemId)
        {
            int num = 0;
            string[] batchSequence = new BaseSequenceManager(base.DbHelper).GetBatchSequence(BasePermissionEntity.TableName, userIds.Length);
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                this.Grant(permissionManager, batchSequence[i], userIds[i], permissionItemId);
                num++;
            }
            return num;
        }

        private string Grant(BasePermissionManager permissionManager, string id, string userId, string permissionItemId)
        {
            BasePermissionEntity resourcePermissionEntity = new BasePermissionEntity {
                ResourceCategory = BaseUserEntity.TableName,
                ResourceId = userId,
                PermissionId = new int?(int.Parse(permissionItemId)),
                Enabled = 1
            };
            return permissionManager.Add(resourcePermissionEntity);
        }

        public int Revoke(string[] userIds, string[] permissionItemIds)
        {
            int num = 0;
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                for (int j = 0; j < permissionItemIds.Length; j++)
                {
                    num += this.Revoke(permissionManager, userIds[i], permissionItemIds[j]);
                }
            }
            return num;
        }

        public int Revoke(string userId, string permissionItemId)
        {
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            return this.Revoke(permissionManager, userId, permissionItemId);
        }

        public int Revoke(string userId, string[] permissionItemIds)
        {
            int num = 0;
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < permissionItemIds.Length; i++)
            {
                num += this.Revoke(permissionManager, userId, permissionItemIds[i]);
            }
            return num;
        }

        public int Revoke(string[] userIds, string permissionItemId)
        {
            int num = 0;
            BasePermissionManager permissionManager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            for (int i = 0; i < userIds.Length; i++)
            {
                num += this.Revoke(permissionManager, userIds[i], permissionItemId);
            }
            return num;
        }

        private int Revoke(BasePermissionManager permissionManager, string userId, string permissionItemId)
        {
            string[] names = new string[3];
            string[] values = new string[3];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionEntity.FieldResourceId;
            values[1] = userId;
            names[2] = BasePermissionEntity.FieldPermissionItemId;
            values[2] = permissionItemId;
            return permissionManager.Delete(names, values);
        }

        public int RevokeAll(string userId)
        {
            BasePermissionManager manager = new BasePermissionManager(base.DbHelper, base.UserInfo);
            string[] names = new string[2];
            string[] values = new string[2];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseUserEntity.TableName;
            names[1] = BasePermissionEntity.FieldResourceId;
            values[1] = userId;
            return manager.Delete(names, values);
        }
    }
}

