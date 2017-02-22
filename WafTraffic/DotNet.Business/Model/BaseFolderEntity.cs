namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseFolderEntity
    {
        private string createBy;
        private string createOn;
        private string createUserId;
        private string description;
        private bool enabled;
        [NonSerialized]
        public static string FieldCreateBy = "CreateBy";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDescription = "Description";
        [NonSerialized]
        public static string FieldEnabled = "Enabled";
        [NonSerialized]
        public static string FieldFolderName = "FolderName";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldParentId = "ParentId";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldStateCode = "StateCode";
        private string folderName;
        private string id;
        private string modifiedBy;
        private string modifiedOn;
        private string modifiedUserId;
        private string parentId;
        private string sortCode;
        [NonSerialized]
        public static string TableName = "Base_Folder";

        public BaseFolderEntity()
        {
            this.id = string.Empty;
            this.parentId = string.Empty;
            this.folderName = string.Empty;
            this.enabled = true;
            this.description = string.Empty;
            this.sortCode = string.Empty;
            this.createUserId = string.Empty;
            this.createBy = string.Empty;
            this.createOn = string.Empty;
            this.modifiedUserId = string.Empty;
            this.modifiedBy = string.Empty;
            this.modifiedOn = string.Empty;
        }

        public BaseFolderEntity(DataRow dataRow)
        {
            this.id = string.Empty;
            this.parentId = string.Empty;
            this.folderName = string.Empty;
            this.enabled = true;
            this.description = string.Empty;
            this.sortCode = string.Empty;
            this.createUserId = string.Empty;
            this.createBy = string.Empty;
            this.createOn = string.Empty;
            this.modifiedUserId = string.Empty;
            this.modifiedBy = string.Empty;
            this.modifiedOn = string.Empty;
            this.GetFrom(dataRow);
        }

        public BaseFolderEntity(DataTable dataTable)
        {
            this.id = string.Empty;
            this.parentId = string.Empty;
            this.folderName = string.Empty;
            this.enabled = true;
            this.description = string.Empty;
            this.sortCode = string.Empty;
            this.createUserId = string.Empty;
            this.createBy = string.Empty;
            this.createOn = string.Empty;
            this.modifiedUserId = string.Empty;
            this.modifiedBy = string.Empty;
            this.modifiedOn = string.Empty;
            this.GetFrom(dataTable);
        }

        public BaseFolderEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToString(dataRow[FieldParentId]);
            this.FolderName = BaseBusinessLogic.ConvertToString(dataRow[FieldFolderName]);
            this.Enabled = BaseBusinessLogic.ConvertIntToBoolean(dataRow[FieldEnabled]);
            this.SortCode = BaseBusinessLogic.ConvertToString(dataRow[FieldSortCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateOn = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateOn]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedOn]);
            return this;
        }

        public BaseFolderEntity GetFrom(DataTable dataTable)
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

        public string FolderName
        {
            get
            {
                return this.folderName;
            }
            set
            {
                this.folderName = value;
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

        public string ParentId
        {
            get
            {
                return this.parentId;
            }
            set
            {
                this.parentId = value;
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
    }
}

