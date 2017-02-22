namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseNewsEntity
    {
        private string auditStatus;
        private string categoryCode;
        private string code;
        private string contents;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string description;
        private int? enabled;
        [NonSerialized]
        public static string FieldAuditStatus = "AuditStatus";
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
        public static string FieldFilePath = "FilePath";
        [NonSerialized]
        public static string FieldFileSize = "FileSize";
        [NonSerialized]
        public static string FieldFolderId = "FolderId";
        [NonSerialized]
        public static string FieldHomePage = "HomePage";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldImageUrl = "ImageUrl";
        [NonSerialized]
        public static string FieldIntroduction = "Introduction";
        [NonSerialized]
        public static string FieldKeywords = "Keywords";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldReadCount = "ReadCount";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldSource = "Source";
        [NonSerialized]
        public static string FieldSubPage = "SubPage";
        [NonSerialized]
        public static string FieldTitle = "Title";
        private string filePath;
        private int? fileSize;
        private string folderId;
        private int? homePage;
        private string id;
        private string imageUrl;
        private string introduction;
        private string keywords;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private int? readCount;
        private int? sortCode;
        private string source;
        private int? subPage;
        [NonSerialized]
        public static string TableName = "Base_News";
        private string title;

        public BaseNewsEntity()
        {
            this.fileSize = 0;
            this.homePage = 0;
            this.subPage = 0;
            this.readCount = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseNewsEntity(DataRow dataRow)
        {
            this.fileSize = 0;
            this.homePage = 0;
            this.subPage = 0;
            this.readCount = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseNewsEntity(DataTable dataTable)
        {
            this.fileSize = 0;
            this.homePage = 0;
            this.subPage = 0;
            this.readCount = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseNewsEntity(IDataReader dataReader)
        {
            this.fileSize = 0;
            this.homePage = 0;
            this.subPage = 0;
            this.readCount = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseNewsEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.FolderId = BaseBusinessLogic.ConvertToString(dataRow[FieldFolderId]);
            this.CategoryCode = BaseBusinessLogic.ConvertToString(dataRow[FieldCategoryCode]);
            this.Code = BaseBusinessLogic.ConvertToString(dataRow[FieldCode]);
            this.Title = BaseBusinessLogic.ConvertToString(dataRow[FieldTitle]);
            this.FilePath = BaseBusinessLogic.ConvertToString(dataRow[FieldFilePath]);
            this.Introduction = BaseBusinessLogic.ConvertToString(dataRow[FieldIntroduction]);
            this.Contents = BaseBusinessLogic.ConvertToString(dataRow[FieldContents]);
            this.Source = BaseBusinessLogic.ConvertToString(dataRow[FieldSource]);
            this.Keywords = BaseBusinessLogic.ConvertToString(dataRow[FieldKeywords]);
            this.FileSize = BaseBusinessLogic.ConvertToInt(dataRow[FieldFileSize]);
            this.ImageUrl = BaseBusinessLogic.ConvertToString(dataRow[FieldImageUrl]);
            this.SubPage = BaseBusinessLogic.ConvertToInt(dataRow[FieldSubPage]);
            this.HomePage = BaseBusinessLogic.ConvertToInt(dataRow[FieldHomePage]);
            this.AuditStatus = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditStatus]);
            this.ReadCount = BaseBusinessLogic.ConvertToInt(dataRow[FieldReadCount]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldSortCode]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            return this;
        }

        public BaseNewsEntity GetFrom(DataTable dataTable)
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

        public BaseNewsEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataReader[FieldId]);
            this.FolderId = BaseBusinessLogic.ConvertToString(dataReader[FieldFolderId]);
            this.CategoryCode = BaseBusinessLogic.ConvertToString(dataReader[FieldCategoryCode]);
            this.Code = BaseBusinessLogic.ConvertToString(dataReader[FieldCode]);
            this.Title = BaseBusinessLogic.ConvertToString(dataReader[FieldTitle]);
            this.FilePath = BaseBusinessLogic.ConvertToString(dataReader[FieldFilePath]);
            this.Introduction = BaseBusinessLogic.ConvertToString(dataReader[FieldIntroduction]);
            this.Contents = BaseBusinessLogic.ConvertToString(dataReader[FieldContents]);
            this.Source = BaseBusinessLogic.ConvertToString(dataReader[FieldSource]);
            this.Keywords = BaseBusinessLogic.ConvertToString(dataReader[FieldKeywords]);
            this.FileSize = BaseBusinessLogic.ConvertToInt(dataReader[FieldFileSize]);
            this.ImageUrl = BaseBusinessLogic.ConvertToString(dataReader[FieldImageUrl]);
            this.SubPage = BaseBusinessLogic.ConvertToInt(dataReader[FieldSubPage]);
            this.HomePage = BaseBusinessLogic.ConvertToInt(dataReader[FieldHomePage]);
            this.AuditStatus = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditStatus]);
            this.ReadCount = BaseBusinessLogic.ConvertToInt(dataReader[FieldReadCount]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[FieldDescription]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldSortCode]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldCreateOn]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateBy]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateUserId]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldModifiedOn]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedBy]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedUserId]);
            return this;
        }

        public string AuditStatus
        {
            get
            {
                return this.auditStatus;
            }
            set
            {
                this.auditStatus = value;
            }
        }

        public string CategoryCode
        {
            get
            {
                return this.categoryCode;
            }
            set
            {
                this.categoryCode = value;
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

        public string FilePath
        {
            get
            {
                return this.filePath;
            }
            set
            {
                this.filePath = value;
            }
        }

        public int? FileSize
        {
            get
            {
                return this.fileSize;
            }
            set
            {
                this.fileSize = value;
            }
        }

        public string FolderId
        {
            get
            {
                return this.folderId;
            }
            set
            {
                this.folderId = value;
            }
        }

        public int? HomePage
        {
            get
            {
                return this.homePage;
            }
            set
            {
                this.homePage = value;
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

        public string ImageUrl
        {
            get
            {
                return this.imageUrl;
            }
            set
            {
                this.imageUrl = value;
            }
        }

        public string Introduction
        {
            get
            {
                return this.introduction;
            }
            set
            {
                this.introduction = value;
            }
        }

        public string Keywords
        {
            get
            {
                return this.keywords;
            }
            set
            {
                this.keywords = value;
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

        public int? ReadCount
        {
            get
            {
                return this.readCount;
            }
            set
            {
                this.readCount = value;
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

        public string Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
            }
        }

        public int? SubPage
        {
            get
            {
                return this.subPage;
            }
            set
            {
                this.subPage = value;
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

