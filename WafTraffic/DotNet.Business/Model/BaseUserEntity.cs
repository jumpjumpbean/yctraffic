namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseUserEntity
    {
        private DateTime? allowEndTime;
        private DateTime? allowStartTime;
        private string answerQuestion;
        private string auditStatus;
        private string birthday;
        private DateTime? changePasswordDate;
        private string code;
        private string communicationPassword;
        private int? companyId;
        private string companyName;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private int? departmentId;
        private string departmentName;
        private string description;
        private string duty;
        private string email;
        private int? enabled;
        [NonSerialized]
        public static string FieldAllowEndTime = "AllowEndTime";
        [NonSerialized]
        public static string FieldAllowStartTime = "AllowStartTime";
        [NonSerialized]
        public static string FieldAnswerQuestion = "AnswerQuestion";
        [NonSerialized]
        public static string FieldAuditStatus = "AuditStatus";
        [NonSerialized]
        public static string FieldBirthday = "Birthday";
        [NonSerialized]
        public static string FieldChangePasswordDate = "ChangePasswordDate";
        [NonSerialized]
        public static string FieldCode = "Code";
        [NonSerialized]
        public static string FieldCommunicationPassword = "CommunicationPassword";
        [NonSerialized]
        public static string FieldCompanyId = "CompanyId";
        [NonSerialized]
        public static string FieldCompanyName = "CompanyName";
        [NonSerialized]
        public static string FieldCreateBy = "CreateBy";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDeletionStateCode = "DeletionStateCode";
        [NonSerialized]
        public static string FieldDepartmentId = "DepartmentId";
        [NonSerialized]
        public static string FieldDepartmentName = "DepartmentName";
        [NonSerialized]
        public static string FieldDescription = "Description";
        [NonSerialized]
        public static string FieldDuty = "Duty";
        [NonSerialized]
        public static string FieldEmail = "Email";
        [NonSerialized]
        public static string FieldEnabled = "Enabled";
        [NonSerialized]
        public static string FieldFirstVisit = "FirstVisit";
        [NonSerialized]
        public static string FieldGender = "Gender";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldIPAddress = "IPAddress";
        [NonSerialized]
        public static string FieldIsStaff = "IsStaff";
        [NonSerialized]
        public static string FieldIsVisible = "IsVisible";
        [NonSerialized]
        public static string FieldLang = "Lang";
        [NonSerialized]
        public static string FieldLastVisit = "LastVisit";
        [NonSerialized]
        public static string FieldLockEndDate = "LockEndDate";
        [NonSerialized]
        public static string FieldLockStartDate = "LockStartDate";
        [NonSerialized]
        public static string FieldLogOnCount = "LogOnCount";
        [NonSerialized]
        public static string FieldMACAddress = "MACAddress";
        [NonSerialized]
        public static string FieldMobile = "Mobile";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldOICQ = "OICQ";
        [NonSerialized]
        public static string FieldOpenId = "OpenId";
        [NonSerialized]
        public static string FieldPreviousVisit = "PreviousVisit";
        [NonSerialized]
        public static string FieldPublicKey = "PublicKey";
        [NonSerialized]
        public static string FieldQuestion = "Question";
        [NonSerialized]
        public static string FieldRealName = "RealName";
        [NonSerialized]
        public static string FieldRoleId = "RoleId";
        [NonSerialized]
        public static string FieldSecurityLevel = "SecurityLevel";
        [NonSerialized]
        public static string FieldSignedPassword = "SignedPassword";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldTelephone = "Telephone";
        [NonSerialized]
        public static string FieldTheme = "Theme";
        [NonSerialized]
        public static string FieldTitle = "Title";
        [NonSerialized]
        public static string FieldUserAddressId = "UserAddressId";
        [NonSerialized]
        public static string FieldUserFrom = "UserFrom";
        [NonSerialized]
        public static string FieldUserName = "UserName";
        [NonSerialized]
        public static string FieldUserOnLine = "UserOnLine";
        [NonSerialized]
        public static string FieldUserPassword = "UserPassword";
        [NonSerialized]
        public static string FieldWorkCategory = "WorkCategory";
        [NonSerialized]
        public static string FieldWorkgroupId = "WorkgroupId";
        [NonSerialized]
        public static string FieldWorkgroupName = "WorkgroupName";
        private DateTime? firstVisit;
        private string gender;
        private int? id;
        private string iPAddress;
        private int? isStaff;
        private int? isVisible;
        private string lang;
        private DateTime? lastVisit;
        private DateTime? lockEndDate;
        private DateTime? lockStartDate;
        private int? logOnCount;
        private string mACAddress;
        private string mobile;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string oICQ;
        private string openId;
        private DateTime? previousVisit;
        private string publicKey;
        private string question;
        private string realName;
        private int? roleId;
        private int? securityLevel;
        private string signedPassword;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_User";
        private string telephone;
        private string theme;
        private string title;
        private string userAddressId;
        private string userFrom;
        private string userName;
        private int? userOnLine;
        private string userPassword;
        private string workCategory;
        private int? workgroupId;
        private string workgroupName;

        public BaseUserEntity()
        {
            this.id = null;
            this.code = string.Empty;
            this.userName = string.Empty;
            this.realName = string.Empty;
            this.roleId = null;
            this.securityLevel = null;
            this.userFrom = string.Empty;
            this.workCategory = string.Empty;
            this.companyId = null;
            this.companyName = string.Empty;
            this.departmentId = null;
            this.departmentName = string.Empty;
            this.workgroupId = null;
            this.workgroupName = string.Empty;
            this.gender = string.Empty;
            this.mobile = string.Empty;
            this.telephone = string.Empty;
            this.birthday = string.Empty;
            this.changePasswordDate = null;
            this.allowStartTime = null;
            this.allowEndTime = null;
            this.lockStartDate = null;
            this.lockEndDate = null;
            this.firstVisit = null;
            this.previousVisit = null;
            this.lastVisit = null;
            this.logOnCount = 0;
            this.isStaff = 0;
            this.isVisible = 1;
            this.userOnLine = 0;
            this.openId = BaseBusinessLogic.NewGuid();
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseUserEntity(DataRow dataRow)
        {
            this.id = null;
            this.code = string.Empty;
            this.userName = string.Empty;
            this.realName = string.Empty;
            this.roleId = null;
            this.securityLevel = null;
            this.userFrom = string.Empty;
            this.workCategory = string.Empty;
            this.companyId = null;
            this.companyName = string.Empty;
            this.departmentId = null;
            this.departmentName = string.Empty;
            this.workgroupId = null;
            this.workgroupName = string.Empty;
            this.gender = string.Empty;
            this.mobile = string.Empty;
            this.telephone = string.Empty;
            this.birthday = string.Empty;
            this.changePasswordDate = null;
            this.allowStartTime = null;
            this.allowEndTime = null;
            this.lockStartDate = null;
            this.lockEndDate = null;
            this.firstVisit = null;
            this.previousVisit = null;
            this.lastVisit = null;
            this.logOnCount = 0;
            this.isStaff = 0;
            this.isVisible = 1;
            this.userOnLine = 0;
            this.openId = BaseBusinessLogic.NewGuid();
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseUserEntity(DataTable dataTable)
        {
            this.id = null;
            this.code = string.Empty;
            this.userName = string.Empty;
            this.realName = string.Empty;
            this.roleId = null;
            this.securityLevel = null;
            this.userFrom = string.Empty;
            this.workCategory = string.Empty;
            this.companyId = null;
            this.companyName = string.Empty;
            this.departmentId = null;
            this.departmentName = string.Empty;
            this.workgroupId = null;
            this.workgroupName = string.Empty;
            this.gender = string.Empty;
            this.mobile = string.Empty;
            this.telephone = string.Empty;
            this.birthday = string.Empty;
            this.changePasswordDate = null;
            this.allowStartTime = null;
            this.allowEndTime = null;
            this.lockStartDate = null;
            this.lockEndDate = null;
            this.firstVisit = null;
            this.previousVisit = null;
            this.lastVisit = null;
            this.logOnCount = 0;
            this.isStaff = 0;
            this.isVisible = 1;
            this.userOnLine = 0;
            this.openId = BaseBusinessLogic.NewGuid();
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseUserEntity(IDataReader dataReader)
        {
            this.id = null;
            this.code = string.Empty;
            this.userName = string.Empty;
            this.realName = string.Empty;
            this.roleId = null;
            this.securityLevel = null;
            this.userFrom = string.Empty;
            this.workCategory = string.Empty;
            this.companyId = null;
            this.companyName = string.Empty;
            this.departmentId = null;
            this.departmentName = string.Empty;
            this.workgroupId = null;
            this.workgroupName = string.Empty;
            this.gender = string.Empty;
            this.mobile = string.Empty;
            this.telephone = string.Empty;
            this.birthday = string.Empty;
            this.changePasswordDate = null;
            this.allowStartTime = null;
            this.allowEndTime = null;
            this.lockStartDate = null;
            this.lockEndDate = null;
            this.firstVisit = null;
            this.previousVisit = null;
            this.lastVisit = null;
            this.logOnCount = 0;
            this.isStaff = 0;
            this.isVisible = 1;
            this.userOnLine = 0;
            this.openId = BaseBusinessLogic.NewGuid();
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseUserEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataRow[FieldCode]);
            this.UserName = BaseBusinessLogic.ConvertToString(dataRow[FieldUserName]);
            this.RealName = BaseBusinessLogic.ConvertToString(dataRow[FieldRealName]);
            this.RoleId = BaseBusinessLogic.ConvertToInt(dataRow[FieldRoleId]);
            this.SecurityLevel = BaseBusinessLogic.ConvertToInt(dataRow[FieldSecurityLevel]);
            this.UserFrom = BaseBusinessLogic.ConvertToString(dataRow[FieldUserFrom]);
            this.WorkCategory = BaseBusinessLogic.ConvertToString(dataRow[FieldWorkCategory]);
            this.CompanyId = BaseBusinessLogic.ConvertToInt(dataRow[FieldCompanyId]);
            this.CompanyName = BaseBusinessLogic.ConvertToString(dataRow[FieldCompanyName]);
            this.DepartmentId = BaseBusinessLogic.ConvertToInt(dataRow[FieldDepartmentId]);
            this.DepartmentName = BaseBusinessLogic.ConvertToString(dataRow[FieldDepartmentName]);
            this.WorkgroupId = BaseBusinessLogic.ConvertToInt(dataRow[FieldWorkgroupId]);
            this.WorkgroupName = BaseBusinessLogic.ConvertToString(dataRow[FieldWorkgroupName]);
            this.Gender = BaseBusinessLogic.ConvertToString(dataRow[FieldGender]);
            this.Mobile = BaseBusinessLogic.ConvertToString(dataRow[FieldMobile]);
            this.Telephone = BaseBusinessLogic.ConvertToString(dataRow[FieldTelephone]);
            this.Birthday = BaseBusinessLogic.ConvertToString(dataRow[FieldBirthday]);
            this.Duty = BaseBusinessLogic.ConvertToString(dataRow[FieldDuty]);
            this.Title = BaseBusinessLogic.ConvertToString(dataRow[FieldTitle]);
            this.UserPassword = BaseBusinessLogic.ConvertToString(dataRow[FieldUserPassword]);
            this.ChangePasswordDate = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldChangePasswordDate]);
            this.CommunicationPassword = BaseBusinessLogic.ConvertToString(dataRow[FieldCommunicationPassword]);
            this.SignedPassword = BaseBusinessLogic.ConvertToString(dataRow[FieldSignedPassword]);
            this.PublicKey = BaseBusinessLogic.ConvertToString(dataRow[FieldPublicKey]);
            this.OICQ = BaseBusinessLogic.ConvertToString(dataRow[FieldOICQ]);
            this.Email = BaseBusinessLogic.ConvertToString(dataRow[FieldEmail]);
            this.Lang = BaseBusinessLogic.ConvertToString(dataRow[FieldLang]);
            this.Theme = BaseBusinessLogic.ConvertToString(dataRow[FieldTheme]);
            this.AllowStartTime = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldAllowStartTime]);
            this.AllowEndTime = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldAllowEndTime]);
            this.LockStartDate = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldLockStartDate]);
            this.LockEndDate = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldLockEndDate]);
            this.FirstVisit = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldFirstVisit]);
            this.PreviousVisit = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldPreviousVisit]);
            this.LastVisit = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldLastVisit]);
            this.LogOnCount = BaseBusinessLogic.ConvertToInt(dataRow[FieldLogOnCount]);
            this.IsStaff = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsStaff]);
            this.AuditStatus = BaseBusinessLogic.ConvertToString(dataRow[FieldAuditStatus]);
            this.IsVisible = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsVisible]);
            this.UserOnLine = BaseBusinessLogic.ConvertToInt(dataRow[FieldUserOnLine]);
            this.IPAddress = BaseBusinessLogic.ConvertToString(dataRow[FieldIPAddress]);
            this.MACAddress = BaseBusinessLogic.ConvertToString(dataRow[FieldMACAddress]);
            this.OpenId = BaseBusinessLogic.ConvertToString(dataRow[FieldOpenId]);
            this.Question = BaseBusinessLogic.ConvertToString(dataRow[FieldQuestion]);
            this.AnswerQuestion = BaseBusinessLogic.ConvertToString(dataRow[FieldAnswerQuestion]);
            this.UserAddressId = BaseBusinessLogic.ConvertToString(dataRow[FieldUserAddressId]);
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

        public BaseUserEntity GetFrom(DataTable dataTable)
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

        public BaseUserEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.Code = BaseBusinessLogic.ConvertToString(dataReader[FieldCode]);
            this.UserName = BaseBusinessLogic.ConvertToString(dataReader[FieldUserName]);
            this.RealName = BaseBusinessLogic.ConvertToString(dataReader[FieldRealName]);
            this.RoleId = BaseBusinessLogic.ConvertToInt(dataReader[FieldRoleId]);
            this.SecurityLevel = BaseBusinessLogic.ConvertToInt(dataReader[FieldSecurityLevel]);
            this.UserFrom = BaseBusinessLogic.ConvertToString(dataReader[FieldUserFrom]);
            this.WorkCategory = BaseBusinessLogic.ConvertToString(dataReader[FieldWorkCategory]);
            this.CompanyId = BaseBusinessLogic.ConvertToInt(dataReader[FieldCompanyId]);
            this.CompanyName = BaseBusinessLogic.ConvertToString(dataReader[FieldCompanyName]);
            this.DepartmentId = BaseBusinessLogic.ConvertToInt(dataReader[FieldDepartmentId]);
            this.DepartmentName = BaseBusinessLogic.ConvertToString(dataReader[FieldDepartmentName]);
            this.WorkgroupId = BaseBusinessLogic.ConvertToInt(dataReader[FieldWorkgroupId]);
            this.WorkgroupName = BaseBusinessLogic.ConvertToString(dataReader[FieldWorkgroupName]);
            this.Gender = BaseBusinessLogic.ConvertToString(dataReader[FieldGender]);
            this.Mobile = BaseBusinessLogic.ConvertToString(dataReader[FieldMobile]);
            this.Telephone = BaseBusinessLogic.ConvertToString(dataReader[FieldTelephone]);
            this.Birthday = BaseBusinessLogic.ConvertToString(dataReader[FieldBirthday]);
            this.Duty = BaseBusinessLogic.ConvertToString(dataReader[FieldDuty]);
            this.Title = BaseBusinessLogic.ConvertToString(dataReader[FieldTitle]);
            this.UserPassword = BaseBusinessLogic.ConvertToString(dataReader[FieldUserPassword]);
            this.ChangePasswordDate = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldChangePasswordDate]);
            this.CommunicationPassword = BaseBusinessLogic.ConvertToString(dataReader[FieldCommunicationPassword]);
            this.SignedPassword = BaseBusinessLogic.ConvertToString(dataReader[FieldSignedPassword]);
            this.PublicKey = BaseBusinessLogic.ConvertToString(dataReader[FieldPublicKey]);
            this.OICQ = BaseBusinessLogic.ConvertToString(dataReader[FieldOICQ]);
            this.Email = BaseBusinessLogic.ConvertToString(dataReader[FieldEmail]);
            this.Lang = BaseBusinessLogic.ConvertToString(dataReader[FieldLang]);
            this.Theme = BaseBusinessLogic.ConvertToString(dataReader[FieldTheme]);
            this.AllowStartTime = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldAllowStartTime]);
            this.AllowEndTime = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldAllowEndTime]);
            this.LockStartDate = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldLockStartDate]);
            this.LockEndDate = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldLockEndDate]);
            this.FirstVisit = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldFirstVisit]);
            this.PreviousVisit = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldPreviousVisit]);
            this.LastVisit = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldLastVisit]);
            this.LogOnCount = BaseBusinessLogic.ConvertToInt(dataReader[FieldLogOnCount]);
            this.IsStaff = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsStaff]);
            this.AuditStatus = BaseBusinessLogic.ConvertToString(dataReader[FieldAuditStatus]);
            this.IsVisible = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsVisible]);
            this.UserOnLine = BaseBusinessLogic.ConvertToInt(dataReader[FieldUserOnLine]);
            this.IPAddress = BaseBusinessLogic.ConvertToString(dataReader[FieldIPAddress]);
            this.MACAddress = BaseBusinessLogic.ConvertToString(dataReader[FieldMACAddress]);
            this.OpenId = BaseBusinessLogic.ConvertToString(dataReader[FieldOpenId]);
            this.Question = BaseBusinessLogic.ConvertToString(dataReader[FieldQuestion]);
            this.AnswerQuestion = BaseBusinessLogic.ConvertToString(dataReader[FieldAnswerQuestion]);
            this.UserAddressId = BaseBusinessLogic.ConvertToString(dataReader[FieldUserAddressId]);
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

        public DateTime? AllowEndTime
        {
            get
            {
                return this.allowEndTime;
            }
            set
            {
                this.allowEndTime = value;
            }
        }

        public DateTime? AllowStartTime
        {
            get
            {
                return this.allowStartTime;
            }
            set
            {
                this.allowStartTime = value;
            }
        }

        public string AnswerQuestion
        {
            get
            {
                return this.answerQuestion;
            }
            set
            {
                this.answerQuestion = value;
            }
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

        public string Birthday
        {
            get
            {
                return this.birthday;
            }
            set
            {
                this.birthday = value;
            }
        }

        public DateTime? ChangePasswordDate
        {
            get
            {
                return this.changePasswordDate;
            }
            set
            {
                this.changePasswordDate = value;
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

        public string CommunicationPassword
        {
            get
            {
                return this.communicationPassword;
            }
            set
            {
                this.communicationPassword = value;
            }
        }

        public int? CompanyId
        {
            get
            {
                return this.companyId;
            }
            set
            {
                this.companyId = value;
            }
        }

        public string CompanyName
        {
            get
            {
                return this.companyName;
            }
            set
            {
                this.companyName = value;
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

        public int? DepartmentId
        {
            get
            {
                return this.departmentId;
            }
            set
            {
                this.departmentId = value;
            }
        }

        public string DepartmentName
        {
            get
            {
                return this.departmentName;
            }
            set
            {
                this.departmentName = value;
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

        public string Duty
        {
            get
            {
                return this.duty;
            }
            set
            {
                this.duty = value;
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
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

        public DateTime? FirstVisit
        {
            get
            {
                return this.firstVisit;
            }
            set
            {
                this.firstVisit = value;
            }
        }

        public string Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                this.gender = value;
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

        public string IPAddress
        {
            get
            {
                return this.iPAddress;
            }
            set
            {
                this.iPAddress = value;
            }
        }

        public int? IsStaff
        {
            get
            {
                return this.isStaff;
            }
            set
            {
                this.isStaff = value;
            }
        }

        public int? IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                this.isVisible = value;
            }
        }

        public string Lang
        {
            get
            {
                return this.lang;
            }
            set
            {
                this.lang = value;
            }
        }

        public DateTime? LastVisit
        {
            get
            {
                return this.lastVisit;
            }
            set
            {
                this.lastVisit = value;
            }
        }

        public DateTime? LockEndDate
        {
            get
            {
                return this.lockEndDate;
            }
            set
            {
                this.lockEndDate = value;
            }
        }

        public DateTime? LockStartDate
        {
            get
            {
                return this.lockStartDate;
            }
            set
            {
                this.lockStartDate = value;
            }
        }

        public int? LogOnCount
        {
            get
            {
                return this.logOnCount;
            }
            set
            {
                this.logOnCount = value;
            }
        }

        public string MACAddress
        {
            get
            {
                return this.mACAddress;
            }
            set
            {
                this.mACAddress = value;
            }
        }

        public string Mobile
        {
            get
            {
                return this.mobile;
            }
            set
            {
                this.mobile = value;
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

        public string OICQ
        {
            get
            {
                return this.oICQ;
            }
            set
            {
                this.oICQ = value;
            }
        }

        public string OpenId
        {
            get
            {
                return this.openId;
            }
            set
            {
                this.openId = value;
            }
        }

        public DateTime? PreviousVisit
        {
            get
            {
                return this.previousVisit;
            }
            set
            {
                this.previousVisit = value;
            }
        }

        public string PublicKey
        {
            get
            {
                return this.publicKey;
            }
            set
            {
                this.publicKey = value;
            }
        }

        public string Question
        {
            get
            {
                return this.question;
            }
            set
            {
                this.question = value;
            }
        }

        public string RealName
        {
            get
            {
                return this.realName;
            }
            set
            {
                this.realName = value;
            }
        }

        public int? RoleId
        {
            get
            {
                return this.roleId;
            }
            set
            {
                this.roleId = value;
            }
        }

        public int? SecurityLevel
        {
            get
            {
                return this.securityLevel;
            }
            set
            {
                this.securityLevel = value;
            }
        }

        public string SignedPassword
        {
            get
            {
                return this.signedPassword;
            }
            set
            {
                this.signedPassword = value;
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

        public string Telephone
        {
            get
            {
                return this.telephone;
            }
            set
            {
                this.telephone = value;
            }
        }

        public string Theme
        {
            get
            {
                return this.theme;
            }
            set
            {
                this.theme = value;
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

        public string UserAddressId
        {
            get
            {
                return this.userAddressId;
            }
            set
            {
                this.userAddressId = value;
            }
        }

        public string UserFrom
        {
            get
            {
                return this.userFrom;
            }
            set
            {
                this.userFrom = value;
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }

        public int? UserOnLine
        {
            get
            {
                return this.userOnLine;
            }
            set
            {
                this.userOnLine = value;
            }
        }

        public string UserPassword
        {
            get
            {
                return this.userPassword;
            }
            set
            {
                this.userPassword = value;
            }
        }

        public string WorkCategory
        {
            get
            {
                return this.workCategory;
            }
            set
            {
                this.workCategory = value;
            }
        }

        public int? WorkgroupId
        {
            get
            {
                return this.workgroupId;
            }
            set
            {
                this.workgroupId = value;
            }
        }

        public string WorkgroupName
        {
            get
            {
                return this.workgroupName;
            }
            set
            {
                this.workgroupName = value;
            }
        }


        // add by Nick
        public override string ToString()
        {
            return this.RealName;
        }
    }
}

