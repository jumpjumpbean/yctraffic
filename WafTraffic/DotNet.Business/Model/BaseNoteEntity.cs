namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseNoteEntity
    {
        private string categoryFullName;
        private string categoryId;
        private string color;
        private string content;
        private string createOn;
        private string createUserId;
        private bool deletionStateCode;
        private bool enabled;
        [NonSerialized]
        public static string FieldCategoryFullName = "CategoryFullName";
        [NonSerialized]
        public static string FieldCategoryId = "CategoryId";
        [NonSerialized]
        public static string FieldColor = "Color";
        [NonSerialized]
        public static string FieldContent = "Content";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDeletionStateCode = "DeletionStateCode";
        [NonSerialized]
        public static string FieldEnabled = "Enabled";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldImportant = "Important";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldTitle = "Title";
        private string id;
        private bool important;
        private string modifiedOn;
        private string modifiedUserId;
        private string sortCode;
        [NonSerialized]
        public static string TableName = "Base_Note";
        private string title;

        public BaseNoteEntity()
        {
            this.id = string.Empty;
            this.title = string.Empty;
            this.categoryId = string.Empty;
            this.categoryFullName = string.Empty;
            this.color = string.Empty;
            this.content = string.Empty;
            this.enabled = true;
            this.sortCode = string.Empty;
            this.createUserId = string.Empty;
            this.createOn = string.Empty;
            this.modifiedUserId = string.Empty;
            this.modifiedOn = string.Empty;
        }

        public BaseNoteEntity(DataRow dataRow)
        {
            this.id = string.Empty;
            this.title = string.Empty;
            this.categoryId = string.Empty;
            this.categoryFullName = string.Empty;
            this.color = string.Empty;
            this.content = string.Empty;
            this.enabled = true;
            this.sortCode = string.Empty;
            this.createUserId = string.Empty;
            this.createOn = string.Empty;
            this.modifiedUserId = string.Empty;
            this.modifiedOn = string.Empty;
            this.GetFrom(dataRow);
        }

        public void ClearProperty()
        {
            this.Id = string.Empty;
            this.Title = string.Empty;
            this.CategoryId = string.Empty;
            this.CategoryFullName = string.Empty;
            this.Color = string.Empty;
            this.Content = string.Empty;
            this.Enabled = true;
            this.Important = false;
            this.DeletionStateCode = false;
            this.SortCode = string.Empty;
            this.CreateUserId = string.Empty;
            this.CreateOn = string.Empty;
            this.ModifiedUserId = string.Empty;
            this.ModifiedOn = string.Empty;
        }

        public BaseNoteEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.Title = BaseBusinessLogic.ConvertToString(dataRow[FieldTitle]);
            this.CategoryId = BaseBusinessLogic.ConvertToString(dataRow[FieldCategoryId]);
            this.CategoryFullName = BaseBusinessLogic.ConvertToString(dataRow[FieldCategoryFullName]);
            this.Color = BaseBusinessLogic.ConvertToString(dataRow[FieldColor]);
            this.Content = BaseBusinessLogic.ConvertToString(dataRow[FieldContent]);
            this.Enabled = BaseBusinessLogic.ConvertIntToBoolean(dataRow[FieldEnabled]);
            this.Important = BaseBusinessLogic.ConvertIntToBoolean(dataRow[FieldImportant]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertIntToBoolean(dataRow[FieldDeletionStateCode]);
            this.SortCode = BaseBusinessLogic.ConvertToString(dataRow[FieldSortCode]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateOn = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedOn]);
            return this;
        }

        public BaseNoteEntity GetFrom(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                this.GetFrom(row);
                break;
            }
            return this;
        }

        public string CategoryFullName
        {
            get
            {
                return this.categoryFullName;
            }
            set
            {
                this.categoryFullName = value;
            }
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

        public string Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }

        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
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

        public bool DeletionStateCode
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

        public bool Important
        {
            get
            {
                return this.important;
            }
            set
            {
                this.important = value;
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

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }
    }
}

