namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseOrganizeEntity
    {
        private string address;
        private string assistantManager;
        private string bank;
        private string bankAccount;
        private string category;
        private string code;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string description;
        private int? enabled;
        private string fax;
        [NonSerialized]
        public static string FieldAddress = "Address";
        [NonSerialized]
        public static string FieldAssistantManager = "AssistantManager";
        [NonSerialized]
        public static string FieldBank = "Bank";
        [NonSerialized]
        public static string FieldBankAccount = "BankAccount";
        [NonSerialized]
        public static string FieldCategory = "Category";
        [NonSerialized]
        public static string FieldCode = "Code";
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
        public static string FieldFax = "Fax";
        [NonSerialized]
        public static string FieldFullName = "FullName";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldInnerPhone = "InnerPhone";
        [NonSerialized]
        public static string FieldIsInnerOrganize = "IsInnerOrganize";
        [NonSerialized]
        public static string FieldLayer = "Layer";
        [NonSerialized]
        public static string FieldManager = "Manager";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldOuterPhone = "OuterPhone";
        [NonSerialized]
        public static string FieldParentId = "ParentId";
        [NonSerialized]
        public static string FieldPostalcode = "Postalcode";
        [NonSerialized]
        public static string FieldShortName = "ShortName";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldWeb = "Web";
        private string fullName;
        private int? id;
        private string innerPhone;
        private int? isInnerOrganize;
        private int? layer;
        private string manager;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string outerPhone;
        private int? parentId;
        private string postalcode;
        private string shortName;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_Organize";
        private string web;

        public BaseOrganizeEntity()
        {
            this.id = null;
            this.parentId = null;
            this.layer = 0;
            this.isInnerOrganize = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseOrganizeEntity(DataRow dataRow)
        {
            this.id = null;
            this.parentId = null;
            this.layer = 0;
            this.isInnerOrganize = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseOrganizeEntity(DataTable dataTable)
        {
            this.id = null;
            this.parentId = null;
            this.layer = 0;
            this.isInnerOrganize = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseOrganizeEntity(IDataReader dataReader)
        {
            this.id = null;
            this.parentId = null;
            this.layer = 0;
            this.isInnerOrganize = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseOrganizeEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToInt(dataRow[FieldParentId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataRow[FieldCode]);
            this.ShortName = BaseBusinessLogic.ConvertToString(dataRow[FieldShortName]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataRow[FieldFullName]);
            this.Category = BaseBusinessLogic.ConvertToString(dataRow[FieldCategory]);
            this.Layer = BaseBusinessLogic.ConvertToInt(dataRow[FieldLayer]);
            this.OuterPhone = BaseBusinessLogic.ConvertToString(dataRow[FieldOuterPhone]);
            this.InnerPhone = BaseBusinessLogic.ConvertToString(dataRow[FieldInnerPhone]);
            this.Fax = BaseBusinessLogic.ConvertToString(dataRow[FieldFax]);
            this.Postalcode = BaseBusinessLogic.ConvertToString(dataRow[FieldPostalcode]);
            this.Address = BaseBusinessLogic.ConvertToString(dataRow[FieldAddress]);
            this.Web = BaseBusinessLogic.ConvertToString(dataRow[FieldWeb]);
            this.Manager = BaseBusinessLogic.ConvertToString(dataRow[FieldManager]);
            this.AssistantManager = BaseBusinessLogic.ConvertToString(dataRow[FieldAssistantManager]);
            this.IsInnerOrganize = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsInnerOrganize]);
            this.Bank = BaseBusinessLogic.ConvertToString(dataRow[FieldBank]);
            this.BankAccount = BaseBusinessLogic.ConvertToString(dataRow[FieldBankAccount]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldSortCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            return this;
        }

        public BaseOrganizeEntity GetFrom(DataTable dataTable)
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

        public BaseOrganizeEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.ParentId = BaseBusinessLogic.ConvertToInt(dataReader[FieldParentId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataReader[FieldCode]);
            this.ShortName = BaseBusinessLogic.ConvertToString(dataReader[FieldShortName]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataReader[FieldFullName]);
            this.Category = BaseBusinessLogic.ConvertToString(dataReader[FieldCategory]);
            this.Layer = BaseBusinessLogic.ConvertToInt(dataReader[FieldLayer]);
            this.OuterPhone = BaseBusinessLogic.ConvertToString(dataReader[FieldOuterPhone]);
            this.InnerPhone = BaseBusinessLogic.ConvertToString(dataReader[FieldInnerPhone]);
            this.Fax = BaseBusinessLogic.ConvertToString(dataReader[FieldFax]);
            this.Postalcode = BaseBusinessLogic.ConvertToString(dataReader[FieldPostalcode]);
            this.Address = BaseBusinessLogic.ConvertToString(dataReader[FieldAddress]);
            this.Web = BaseBusinessLogic.ConvertToString(dataReader[FieldWeb]);
            this.Manager = BaseBusinessLogic.ConvertToString(dataReader[FieldManager]);
            this.AssistantManager = BaseBusinessLogic.ConvertToString(dataReader[FieldAssistantManager]);
            this.IsInnerOrganize = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsInnerOrganize]);
            this.Bank = BaseBusinessLogic.ConvertToString(dataReader[FieldBank]);
            this.BankAccount = BaseBusinessLogic.ConvertToString(dataReader[FieldBankAccount]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldSortCode]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedBy]);
            return this;
        }

        public string Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
            }
        }

        public string AssistantManager
        {
            get
            {
                return this.assistantManager;
            }
            set
            {
                this.assistantManager = value;
            }
        }

        public string Bank
        {
            get
            {
                return this.bank;
            }
            set
            {
                this.bank = value;
            }
        }

        public string BankAccount
        {
            get
            {
                return this.bankAccount;
            }
            set
            {
                this.bankAccount = value;
            }
        }

        public string Category
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

        public string Fax
        {
            get
            {
                return this.fax;
            }
            set
            {
                this.fax = value;
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

        public string InnerPhone
        {
            get
            {
                return this.innerPhone;
            }
            set
            {
                this.innerPhone = value;
            }
        }

        public int? IsInnerOrganize
        {
            get
            {
                return this.isInnerOrganize;
            }
            set
            {
                this.isInnerOrganize = value;
            }
        }

        public int? Layer
        {
            get
            {
                return this.layer;
            }
            set
            {
                this.layer = value;
            }
        }

        public string Manager
        {
            get
            {
                return this.manager;
            }
            set
            {
                this.manager = value;
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

        public string OuterPhone
        {
            get
            {
                return this.outerPhone;
            }
            set
            {
                this.outerPhone = value;
            }
        }

        public int? ParentId
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

        public string Postalcode
        {
            get
            {
                return this.postalcode;
            }
            set
            {
                this.postalcode = value;
            }
        }

        public string ShortName
        {
            get
            {
                return this.shortName;
            }
            set
            {
                this.shortName = value;
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

        public string Web
        {
            get
            {
                return this.web;
            }
            set
            {
                this.web = value;
            }
        }

        //ydf add
        public override string ToString()
        {
            return this.FullName;
        }
    }
}

