namespace DotNet.Business
{
    using System;

    [Serializable]
    public class BaseProjectEntity
    {
        private string categoryId = string.Empty;
        private string code = string.Empty;
        private string createOn = string.Empty;
        private string createUserId = string.Empty;
        private string description = string.Empty;
        private bool enabled;
        [NonSerialized]
        public static string FieldCategoryId = "CategoryId";
        [NonSerialized]
        public static string FieldCode = "Code";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDescription = "Description";
        [NonSerialized]
        public static string FieldEnabled = "Enabled";
        [NonSerialized]
        public static string FieldFullName = "FullName";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldManagerID = "ManagerID";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldOrganizeId = "OrganizeId";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        private string fullName = string.Empty;
        private string id = string.Empty;
        private string managerId = string.Empty;
        private string modifiedOn = string.Empty;
        private string modifiedUserId = string.Empty;
        private string organizeId = string.Empty;
        private string sortCode = string.Empty;
        [NonSerialized]
        public static string TableName = "Base_Project";

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

        public string ID
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

        public string ManagerId
        {
            get
            {
                return this.managerId;
            }
            set
            {
                this.managerId = value;
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

        public string OrganizeId
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

