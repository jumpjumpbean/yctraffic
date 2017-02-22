namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseParameterEntity
    {
        private string categoryId = string.Empty;
        private string createOn = string.Empty;
        private string createUserId = string.Empty;
        private int? deletionStateCode = 0;
        private string description = string.Empty;
        private bool enabled = true;
        [NonSerialized]
        public static string FieldCategoryId = "CategoryId";
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
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldParameterCode = "ParameterCode";
        [NonSerialized]
        public static string FieldParameterContent = "ParameterContent";
        [NonSerialized]
        public static string FieldParameterId = "ParameterId";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldWorked = "Worked";
        private string id = string.Empty;
        private string modifiedOn = string.Empty;
        private string modifiedUserId = string.Empty;
        private string parameterCode = string.Empty;
        private string parameterContent = string.Empty;
        private string parameterId = string.Empty;
        private string sortCode = string.Empty;
        [NonSerialized]
        public static string TableName = "Base_Parameter";
        private bool worked;

        public void ClearProperty(BaseParameterEntity ParameterEntity)
        {
            ParameterEntity.Id = string.Empty;
            ParameterEntity.CategoryId = string.Empty;
            ParameterEntity.ParameterId = string.Empty;
            ParameterEntity.ParameterCode = string.Empty;
            ParameterEntity.ParameterContent = string.Empty;
            ParameterEntity.Worked = false;
            ParameterEntity.Enabled = false;
            ParameterEntity.SortCode = string.Empty;
            ParameterEntity.Description = string.Empty;
            ParameterEntity.CreateUserId = string.Empty;
            ParameterEntity.CreateOn = string.Empty;
            ParameterEntity.ModifiedUserId = string.Empty;
            ParameterEntity.ModifiedOn = string.Empty;
        }

        public BaseParameterEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.CategoryId = BaseBusinessLogic.ConvertToString(dataRow[FieldCategoryId]);
            this.ParameterId = BaseBusinessLogic.ConvertToString(dataRow[FieldParameterId]);
            this.ParameterCode = BaseBusinessLogic.ConvertToString(dataRow[FieldParameterCode]);
            this.ParameterContent = BaseBusinessLogic.ConvertToString(dataRow[FieldParameterContent]);
            this.Worked = BaseBusinessLogic.ConvertIntToBoolean(dataRow[FieldWorked]);
            this.Enabled = BaseBusinessLogic.ConvertIntToBoolean(dataRow[FieldEnabled]);
            this.SortCode = BaseBusinessLogic.ConvertToString(dataRow[FieldSortCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateOn = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedOn]);
            return this;
        }

        public BaseParameterEntity GetFrom(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                this.GetFrom(row);
                break;
            }
            return this;
        }

        public string CategoryId
        {
            get
            {
                return this.categoryId;
            }
            set
            {
                this.categoryId = value;
            }
        }

        public string CreateOn
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

        public bool Enabled
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

        public string Id
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

        public string ModifiedOn
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

        public string ParameterCode
        {
            get
            {
                return this.parameterCode;
            }
            set
            {
                this.parameterCode = value;
            }
        }

        public string ParameterContent
        {
            get
            {
                return this.parameterContent;
            }
            set
            {
                this.parameterContent = value;
            }
        }

        public string ParameterId
        {
            get
            {
                return this.parameterId;
            }
            set
            {
                this.parameterId = value;
            }
        }

        public string SortCode
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

        public bool Worked
        {
            get
            {
                return this.worked;
            }
            set
            {
                this.worked = value;
            }
        }
    }
}

