namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;

    public class BaseDbSortLogic
    {
        public const string CommandSetBottom = "SetBottom";
        public const string CommandSetDown = "SetDown";
        public const string CommandSetTop = "SetTop";
        public const string CommandSetUp = "SetUp";
        public const string CommandSwap = "Swap";

        public static string GetDownId(IDbHelper dbHelper, string tableName, string id)
        {
            return GetDownId(dbHelper, tableName, string.Empty, id);
        }

        public static string GetDownId(IDbHelper dbHelper, string tableName, string categoryId, string id)
        {
            string str = string.Empty;
            string commandText = string.Empty;
            if (categoryId.Length > 0)
            {
                str = BaseBusinessLogic.FieldCategoryId + " = '" + categoryId + "' And ";
            }
            commandText = "SELECT TOP 1 " + BaseBusinessLogic.FieldId + " FROM " + tableName + " WHERE ( " + str + BaseBusinessLogic.FieldSortCode + " < (SELECT TOP 1 " + BaseBusinessLogic.FieldSortCode + " FROM " + tableName + " WHERE ( " + str + BaseBusinessLogic.FieldId + " = '" + id + "' )  ORDER BY " + BaseBusinessLogic.FieldSortCode + " )) ORDER BY " + BaseBusinessLogic.FieldSortCode + " DESC ";
            return dbHelper.ExecuteScalar(commandText).ToString();
        }

        public static string GetUpId(IDbHelper dbHelper, string tableName, string id)
        {
            return GetUpId(dbHelper, tableName, string.Empty, id);
        }

        public static string GetUpId(IDbHelper dbHelper, string tableName, string categoryId, string id)
        {
            string str = string.Empty;
            string commandText = string.Empty;
            if (categoryId.Length > 0)
            {
                str = BaseBusinessLogic.FieldCategoryId + " = '" + categoryId + "' AND ";
            }
            commandText = " SELECT TOP 1 " + BaseBusinessLogic.FieldId + " FROM " + tableName + " WHERE ( " + str + BaseBusinessLogic.FieldSortCode + " > (SELECT TOP 1 " + BaseBusinessLogic.FieldSortCode + " FROM " + tableName + " WHERE ( " + str + BaseBusinessLogic.FieldId + " = '" + id + "' )  ORDER BY " + BaseBusinessLogic.FieldSortCode + " ))  ORDER BY " + BaseBusinessLogic.FieldSortCode + " ASC ";
            return dbHelper.ExecuteScalar(commandText).ToString();
        }

        public static int SetBottom(IDbHelper dbHelper, string tableName, string id)
        {
            string sequence = new BaseSequenceManager(dbHelper).GetSequence(tableName);
            return DbLogic.SetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldSortCode, sequence);
        }

        public static int SetBottom(IDbHelper dbHelper, string tableName, string id, string sequenceName)
        {
            if (string.IsNullOrEmpty(sequenceName))
            {
                sequenceName = tableName;
            }
            string sequence = new BaseSequenceManager(dbHelper).GetSequence(sequenceName);
            return DbLogic.SetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldSortCode, sequence);
        }

        public static int SetDown(IDbHelper dbHelper, string tableName, string id)
        {
            return SetDown(dbHelper, tableName, string.Empty, id);
        }

        public static int SetDown(IDbHelper dbHelper, string tableName, string categoryId, string id)
        {
            string str = string.Empty;
            string targetValue = string.Empty;
            string str3 = string.Empty;
            int num = 0;
            str = GetDownId(dbHelper, tableName, categoryId, id);
            if (str.Length == 0)
            {
                return num;
            }
            targetValue = DbLogic.GetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldSortCode);
            str3 = DbLogic.GetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, str, BaseBusinessLogic.FieldSortCode);
            DbLogic.SetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, str, BaseBusinessLogic.FieldSortCode, targetValue);
            return DbLogic.SetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldSortCode, str3);
        }

        public static int SetTop(IDbHelper dbHelper, string tableName, string id)
        {
            string reduction = new BaseSequenceManager(dbHelper).GetReduction(tableName);
            return DbLogic.SetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldSortCode, reduction);
        }

        public static int SetTop(IDbHelper dbHelper, string tableName, string id, string sequenceName)
        {
            if (string.IsNullOrEmpty(sequenceName))
            {
                sequenceName = tableName;
            }
            string reduction = new BaseSequenceManager(dbHelper).GetReduction(sequenceName);
            return DbLogic.SetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldSortCode, reduction);
        }

        public static int SetUp(IDbHelper dbHelper, string tableName, string id)
        {
            return SetUp(dbHelper, tableName, string.Empty, id);
        }

        public static int SetUp(IDbHelper dbHelper, string tableName, string categoryId, string id)
        {
            string str = GetUpId(dbHelper, tableName, categoryId, id);
            string targetValue = string.Empty;
            string str3 = string.Empty;
            int num = 0;
            if (str.Length == 0)
            {
                return num;
            }
            targetValue = DbLogic.GetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldSortCode);
            str3 = DbLogic.GetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, str, BaseBusinessLogic.FieldSortCode);
            return (DbLogic.SetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, str, BaseBusinessLogic.FieldSortCode, targetValue) + DbLogic.SetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldSortCode, str3));
        }

        public static int Swap(IDbHelper dbHelper, string tableName, string id, string targetId)
        {
            int num = 0;
            string targetValue = DbLogic.GetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldSortCode);
            string str2 = DbLogic.GetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, targetId, BaseBusinessLogic.FieldSortCode);
            num += DbLogic.SetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, id, BaseBusinessLogic.FieldSortCode, str2);
            return (num + DbLogic.SetProperty(dbHelper, tableName, BaseBusinessLogic.FieldId, targetId, BaseBusinessLogic.FieldSortCode, targetValue));
        }
    }
}

