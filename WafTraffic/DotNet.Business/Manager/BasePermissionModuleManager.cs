namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;

    public class BasePermissionModuleManager : BaseManager
    {
        public BasePermissionModuleManager()
        {
            base.CurrentTableName = BasePermissionEntity.TableName;
        }

        public BasePermissionModuleManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BasePermissionModuleManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public int Add(string[] moduleIds, string permissionItemId)
        {
            int num = 0;
            for (int i = 0; i < moduleIds.Length; i++)
            {
                this.Add(moduleIds[i], permissionItemId);
                num++;
            }
            return num;
        }

        public int Add(string[] moduleIds, string[] permissionItemIds)
        {
            int num = 0;
            for (int i = 0; i < moduleIds.Length; i++)
            {
                int index = 0;
                while (i < permissionItemIds.Length)
                {
                    this.Add(moduleIds[i], permissionItemIds[index]);
                    num++;
                    i++;
                }
            }
            return num;
        }

        public int Add(string moduleId, string permissionItemId)
        {
            int num = 0;
            string[] names = new string[4];
            string[] values = new string[4];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseModuleEntity.TableName;
            names[1] = BasePermissionEntity.FieldResourceId;
            values[1] = moduleId;
            names[2] = BasePermissionEntity.FieldPermissionItemId;
            values[2] = permissionItemId;
            names[3] = BasePermissionEntity.FieldDeletionStateCode;
            values[3] = "0";
            if (!this.Exists(names, values))
            {
                BasePermissionEntity resourcePermissionEntity = new BasePermissionEntity {
                    ResourceId = moduleId,
                    ResourceCategory = BaseModuleEntity.TableName,
                    Enabled = 1,
                    DeletionStateCode = 0,
                    PermissionId = new int?(int.Parse(permissionItemId))
                };
                new BasePermissionManager(base.DbHelper, base.UserInfo).AddEntity(resourcePermissionEntity);
                num++;
            }
            return num;
        }

        public int Add(string moduleId, string[] permissionItemIds)
        {
            int num = 0;
            for (int i = 0; i < permissionItemIds.Length; i++)
            {
                this.Add(moduleId, permissionItemIds[i]);
                num++;
            }
            return num;
        }

        public int Delete(string[] moduleIds, string[] permissionItemIds)
        {
            int num = 0;
            for (int i = 0; i < moduleIds.Length; i++)
            {
                int index = 0;
                while (i < permissionItemIds.Length)
                {
                    num += this.Delete(moduleIds[i], permissionItemIds[index]);
                    i++;
                }
            }
            return num;
        }

        public int Delete(string[] moduleIds, string permissionItemId)
        {
            int num = 0;
            for (int i = 0; i < moduleIds.Length; i++)
            {
                num += this.Delete(moduleIds[i], permissionItemId);
            }
            return num;
        }

        public int Delete(string moduleId, string permissionItemId)
        {
            string[] moduleIds = new string[3];
            string[] permissionItemIds = new string[3];
            moduleIds[0] = BasePermissionEntity.FieldResourceCategory;
            permissionItemIds[0] = BaseModuleEntity.TableName;
            moduleIds[1] = BasePermissionEntity.FieldResourceId;
            permissionItemIds[1] = moduleId;
            moduleIds[2] = BasePermissionEntity.FieldPermissionItemId;
            permissionItemIds[2] = permissionItemId;
            return this.Delete(moduleIds, permissionItemIds);
        }

        public int Delete(string moduleId, string[] permissionItemIds)
        {
            int num = 0;
            for (int i = 0; i < permissionItemIds.Length; i++)
            {
                num += this.Delete(moduleId, permissionItemIds[i]);
            }
            return num;
        }

        public string[] GetModuleIds(string permissionItemId)
        {
            string[] names = new string[3];
            string[] values = new string[3];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseModuleEntity.TableName;
            names[1] = BasePermissionEntity.FieldPermissionItemId;
            values[1] = permissionItemId;
            names[2] = BasePermissionEntity.FieldDeletionStateCode;
            values[2] = "0";
            return BaseBusinessLogic.FieldToArray(this.GetDT(names, values), BasePermissionEntity.FieldResourceId);
        }

        public string[] GetPermissionIds(string moduleId)
        {
            string[] names = new string[3];
            string[] values = new string[3];
            names[0] = BasePermissionEntity.FieldResourceCategory;
            values[0] = BaseModuleEntity.TableName;
            names[1] = BasePermissionEntity.FieldResourceId;
            values[1] = moduleId;
            names[2] = BasePermissionEntity.FieldDeletionStateCode;
            values[2] = "0";
            return BaseBusinessLogic.FieldToArray(this.GetDT(names, values), BasePermissionEntity.FieldPermissionItemId);
        }
    }
}

