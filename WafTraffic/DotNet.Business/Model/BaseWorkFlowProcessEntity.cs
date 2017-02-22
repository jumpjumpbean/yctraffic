namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseWorkFlowProcessEntity
    {
        private string category;
        private string code;
        private string contents;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string description;
        private int? enabled;
        [NonSerialized]
        public static string FieldCategoryCode = "CategoryCode";
        [NonSerialized]
        public static string FieldCode = "Code";
        [NonSerialized]
        public static string FieldContents = "Contents";
        [NonSerialized]
        public static string FieldCreateBy = "CreateBy";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDeletionStateCode = "DeletionStateCode";
        [NonSerialized]
        public static string FieldDescription = "Description";
        [NonSerialized]
        public static string FieldEnabled = "Enabled";
        [NonSerialized]
        public static string FieldFullName = "FullName";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldOrganizeId = "OrganizeId";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        private string fullName;
        private int? id;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private int? organizeId;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_WorkFlowProcess";

        public BaseWorkFlowProcessEntity()
        {
            this.id = 0;
            this.organizeId = 0;
            this.sortCode = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseWorkFlowProcessEntity(DataRow dataRow)
        {
            this.id = 0;
            this.organizeId = 0;
            this.sortCode = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseWorkFlowProcessEntity(DataTable dataTable)
        {
            this.id = 0;
            this.organizeId = 0;
            this.sortCode = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseWorkFlowProcessEntity(IDataReader dataReader)
        {
            this.id = 0;
            this.organizeId = 0;
            this.sortCode = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseWorkFlowProcessEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.OrganizeId = BaseBusinessLogic.ConvertToInt(dataRow[FieldOrganizeId]);
            this.CategoryCode = BaseBusinessLogic.ConvertToString(dataRow[FieldCategoryCode]);
            this.Code = BaseBusinessLogic.ConvertToString(dataRow[FieldCode]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataRow[FieldFullName]);
            this.Contents = BaseBusinessLogic.ConvertToString(dataRow[FieldContents]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldSortCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            return this;
        }

        public BaseWorkFlowProcessEntity GetFrom(DataTable dataTable)
        {
            if ((dataTable == null) || (dataTable.Rows.Count == 0))
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                this.GetFrom(row);
                break;
            }
            return this;
        }

        public BaseWorkFlowProcessEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.OrganizeId = BaseBusinessLogic.ConvertToInt(dataReader[FieldOrganizeId]);
            this.CategoryCode = BaseBusinessLogic.ConvertToString(dataReader[FieldCategoryCode]);
            this.Code = BaseBusinessLogic.ConvertToString(dataReader[FieldCode]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataReader[FieldFullName]);
            this.Contents = BaseBusinessLogic.ConvertToString(dataReader[FieldContents]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldSortCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedBy]);
            return this;
        }

        public string CategoryCode
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        public string Contents
        {
            get
            {
                return this.contents;
            }
            set
            {
                this.contents = value;
            }
        }

        public string CreateBy
        {
            get
            {
                return this.createBy;
            }
            set
            {
                this.createBy = value;
            }
        }

        public DateTime? CreateOn
        {
            get
            {
                return this.createOn;
            }
            set
            {
                this.createOn = value;
            }
        }

        public string CreateUserId
        {
            get
            {
                return this.createUserId;
            }
            set
            {
                this.createUserId = value;
            }
        }

        public int? DeletionStateCode
        {
            get
            {
                return this.deletionStateCode;
            }
            set
            {
                this.deletionStateCode = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public int? Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
            }
        }

        public string FullName
        {
            get
            {
                return this.fullName;
            }
            set
            {
                this.fullName = value;
            }
        }

        public int? Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string ModifiedBy
        {
            get
            {
                return this.modifiedBy;
            }
            set
            {
                this.modifiedBy = value;
            }
        }

        public DateTime? ModifiedOn
        {
            get
            {
                return this.modifiedOn;
            }
            set
            {
                this.modifiedOn = value;
            }
        }

        public string ModifiedUserId
        {
            get
            {
                return this.modifiedUserId;
            }
            set
            {
                this.modifiedUserId = value;
            }
        }

        public int? OrganizeId
        {
            get
            {
                return this.organizeId;
            }
            set
            {
                this.organizeId = value;
            }
        }

        public int? SortCode
        {
            get
            {
                return this.sortCode;
            }
            set
            {
                this.sortCode = value;
            }
        }
    }
}

