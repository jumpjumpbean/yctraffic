namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;

    public class TemplateManager : BaseNewsManager
    {
        public static string TemplateTable = "WorkFlow_BillTemplate";

        public TemplateManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.WorkFlowDbType, BaseSystemInfo.WorkFlowDbConnection);
            }
            base.CurrentTableName = TemplateTable;
        }

        public TemplateManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public TemplateManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public TemplateManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public TemplateManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public TemplateManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public override int SetDeleted(object id)
        {
            BaseWorkFlowCurrentManager manager = new BaseWorkFlowCurrentManager(base.UserInfo);
            string[] names = new string[] { BaseWorkFlowCurrentEntity.FieldCategoryCode, BaseWorkFlowCurrentEntity.FieldDeletionStateCode, BaseWorkFlowCurrentEntity.FieldEnabled };
            object[] values = new object[] { id, 0, 0 };
            if (!manager.Exists(names, values))
            {
                return base.SetDeleted(id);
            }
            return 0;
        }
    }
}

