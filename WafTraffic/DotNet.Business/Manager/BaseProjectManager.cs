namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    public class BaseProjectManager : BaseManager
    {
        public BaseProjectManager()
        {
            base.CurrentTableName = BaseProjectEntity.TableName;
        }

        public BaseProjectManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseProjectManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseProjectManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public string FieldToList(DataTable dataTable)
        {
            return "";
        }

        public DataTable GetList()
        {
            string commandText = " SELECT  Id, Code, FullName, CategoryId, Description, Enabled, OrganizeId, ManagerId, CategoryId,  (SELECT FullName FROM " + BaseOrganizeEntity.TableName + " WHERE (ID = T_Project.OrganizeId)) AS OrganizeFullName , (SELECT FullName FROM " + BaseStaffEntity.TableName + " WHERE (ID = T_Project.ManagerId)) AS ManagerFullName , (SELECT FullName FROM T_ItemDetails WHERE (ID = T_Project.CategoryId)) AS CategoryFullName  FROM " + BaseProjectEntity.TableName;
            return base.DbHelper.Fill(commandText);
        }

        public string IDToCode(string id)
        {
            return this.GetProperty(id, BaseProjectEntity.FieldCode);
        }
    }
}

