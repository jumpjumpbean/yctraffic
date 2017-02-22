namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseStaffEntity
    {
        private string age;
        private string bankCode;
        private string birthday;
        private string carCode;
        private string code;
        private int? companyId;
        private string competency;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private string degree;
        private int? deletionStateCode;
        private int? departmentId;
        private string description;
        private string dimissionCause;
        private string dimissionDate;
        private string dimissionWhither;
        private string dutyId;
        private string education;
        private string email;
        private string emergencyContact;
        private int? enabled;
        [NonSerialized]
        public static string FieldAge = "Age";
        [NonSerialized]
        public static string FieldBankCode = "BankCode";
        [NonSerialized]
        public static string FieldBirthday = "Birthday";
        [NonSerialized]
        public static string FieldCarCode = "CarCode";
        [NonSerialized]
        public static string FieldCode = "Code";
        [NonSerialized]
        public static string FieldCompanyId = "CompanyId";
        [NonSerialized]
        public static string FieldCompetency = "Competency";
        [NonSerialized]
        public static string FieldCreateBy = "CreateBy";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDegree = "Degree";
        [NonSerialized]
        public static string FieldDeletionStateCode = "DeletionStateCode";
        [NonSerialized]
        public static string FieldDepartmentId = "DepartmentId";
        [NonSerialized]
        public static string FieldDescription = "Description";
        [NonSerialized]
        public static string FieldDimissionCause = "DimissionCause";
        [NonSerialized]
        public static string FieldDimissionDate = "DimissionDate";
        [NonSerialized]
        public static string FieldDimissionWhither = "DimissionWhither";
        [NonSerialized]
        public static string FieldDutyId = "DutyId";
        [NonSerialized]
        public static string FieldEducation = "Education";
        [NonSerialized]
        public static string FieldEmail = "Email";
        [NonSerialized]
        public static string FieldEmergencyContact = "EmergencyContact";
        [NonSerialized]
        public static string FieldEnabled = "Enabled";
        [NonSerialized]
        public static string FieldGender = "Gender";
        [NonSerialized]
        public static string FieldHomeAddress = "HomeAddress";
        [NonSerialized]
        public static string FieldHomeFax = "HomeFax";
        [NonSerialized]
        public static string FieldHomePhone = "HomePhone";
        [NonSerialized]
        public static string FieldHomeZipCode = "HomeZipCode";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldIdCard = "IdCard";
        [NonSerialized]
        public static string FieldIdentificationCode = "IdentificationCode";
        [NonSerialized]
        public static string FieldIsDimission = "IsDimission";
        [NonSerialized]
        public static string FieldJoinInDate = "JoinInDate";
        [NonSerialized]
        public static string FieldMajor = "Major";
        [NonSerialized]
        public static string FieldMobile = "Mobile";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldNation = "Nation";
        [NonSerialized]
        public static string FieldNationality = "Nationality";
        [NonSerialized]
        public static string FieldNativePlace = "NativePlace";
        [NonSerialized]
        public static string FieldOfficeAddress = "OfficeAddress";
        [NonSerialized]
        public static string FieldOfficeFax = "OfficeFax";
        [NonSerialized]
        public static string FieldOfficePhone = "OfficePhone";
        [NonSerialized]
        public static string FieldOfficeZipCode = "OfficeZipCode";
        [NonSerialized]
        public static string FieldOICQ = "OICQ";
        [NonSerialized]
        public static string FieldParty = "Party";
        [NonSerialized]
        public static string FieldRealName = "RealName";
        [NonSerialized]
        public static string FieldSchool = "School";
        [NonSerialized]
        public static string FieldShortNumber = "ShortNumber";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldTelephone = "Telephone";
        [NonSerialized]
        public static string FieldTitleDate = "TitleDate";
        [NonSerialized]
        public static string FieldTitleId = "TitleId";
        [NonSerialized]
        public static string FieldTitleLevel = "TitleLevel";
        [NonSerialized]
        public static string FieldUserId = "UserId";
        [NonSerialized]
        public static string FieldUserName = "UserName";
        [NonSerialized]
        public static string FieldWorkgroupId = "WorkgroupId";
        [NonSerialized]
        public static string FieldWorkingDate = "WorkingDate";
        [NonSerialized]
        public static string FieldWorkingProperty = "WorkingProperty";
        private string gender;
        private string homeAddress;
        private string homeFax;
        private string homePhone;
        private string homeZipCode;
        private int? id;
        private string idCard;
        private string identificationCode;
        private int? isDimission;
        private string joinInDate;
        private string major;
        private string mobile;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string nation;
        private string nationality;
        private string nativePlace;
        private string officeAddress;
        private string officeFax;
        private string officePhone;
        private string officeZipCode;
        private string oICQ;
        private string party;
        private string realName;
        private string school;
        private string shortNumber;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_Staff";
        private string telephone;
        private string titleDate;
        private string titleId;
        private string titleLevel;
        private int? userId;
        private string userName;
        private int? workgroupId;
        private string workingDate;
        private string workingProperty;

        public BaseStaffEntity()
        {
            this.id = null;
            this.userId = null;
            this.companyId = null;
            this.departmentId = null;
            this.workgroupId = null;
            this.isDimission = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseStaffEntity(DataRow dataRow)
        {
            this.id = null;
            this.userId = null;
            this.companyId = null;
            this.departmentId = null;
            this.workgroupId = null;
            this.isDimission = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseStaffEntity(DataTable dataTable)
        {
            this.id = null;
            this.userId = null;
            this.companyId = null;
            this.departmentId = null;
            this.workgroupId = null;
            this.isDimission = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseStaffEntity(IDataReader dataReader)
        {
            this.id = null;
            this.userId = null;
            this.companyId = null;
            this.departmentId = null;
            this.workgroupId = null;
            this.isDimission = 0;
            this.enabled = 0;
            this.deletionStateCode = 0;
            this.sortCode = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseStaffEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataRow[FieldId]);
            this.UserId = BaseBusinessLogic.ConvertToInt(dataRow[FieldUserId]);
            this.UserName = BaseBusinessLogic.ConvertToString(dataRow[FieldUserName]);
            this.RealName = BaseBusinessLogic.ConvertToString(dataRow[FieldRealName]);
            this.Code = BaseBusinessLogic.ConvertToString(dataRow[FieldCode]);
            this.Gender = BaseBusinessLogic.ConvertToString(dataRow[FieldGender]);
            this.CompanyId = BaseBusinessLogic.ConvertToInt(dataRow[FieldCompanyId]);
            this.DepartmentId = BaseBusinessLogic.ConvertToInt(dataRow[FieldDepartmentId]);
            this.WorkgroupId = BaseBusinessLogic.ConvertToInt(dataRow[FieldWorkgroupId]);
            this.DutyId = BaseBusinessLogic.ConvertToString(dataRow[FieldDutyId]);
            this.IdentificationCode = BaseBusinessLogic.ConvertToString(dataRow[FieldIdentificationCode]);
            this.IdCard = BaseBusinessLogic.ConvertToString(dataRow[FieldIdCard]);
            this.BankCode = BaseBusinessLogic.ConvertToString(dataRow[FieldBankCode]);
            this.Email = BaseBusinessLogic.ConvertToString(dataRow[FieldEmail]);
            this.Mobile = BaseBusinessLogic.ConvertToString(dataRow[FieldMobile]);
            this.ShortNumber = BaseBusinessLogic.ConvertToString(dataRow[FieldShortNumber]);
            this.Telephone = BaseBusinessLogic.ConvertToString(dataRow[FieldTelephone]);
            this.OICQ = BaseBusinessLogic.ConvertToString(dataRow[FieldOICQ]);
            this.OfficePhone = BaseBusinessLogic.ConvertToString(dataRow[FieldOfficePhone]);
            this.OfficeZipCode = BaseBusinessLogic.ConvertToString(dataRow[FieldOfficeZipCode]);
            this.OfficeAddress = BaseBusinessLogic.ConvertToString(dataRow[FieldOfficeAddress]);
            this.OfficeFax = BaseBusinessLogic.ConvertToString(dataRow[FieldOfficeFax]);
            this.HomePhone = BaseBusinessLogic.ConvertToString(dataRow[FieldHomePhone]);
            this.Age = BaseBusinessLogic.ConvertToString(dataRow[FieldAge]);
            this.Birthday = BaseBusinessLogic.ConvertToString(dataRow[FieldBirthday]);
            this.Education = BaseBusinessLogic.ConvertToString(dataRow[FieldEducation]);
            this.School = BaseBusinessLogic.ConvertToString(dataRow[FieldSchool]);
            this.Degree = BaseBusinessLogic.ConvertToString(dataRow[FieldDegree]);
            this.TitleId = BaseBusinessLogic.ConvertToString(dataRow[FieldTitleId]);
            this.TitleDate = BaseBusinessLogic.ConvertToString(dataRow[FieldTitleDate]);
            this.TitleLevel = BaseBusinessLogic.ConvertToString(dataRow[FieldTitleLevel]);
            this.WorkingDate = BaseBusinessLogic.ConvertToString(dataRow[FieldWorkingDate]);
            this.JoinInDate = BaseBusinessLogic.ConvertToString(dataRow[FieldJoinInDate]);
            this.HomeZipCode = BaseBusinessLogic.ConvertToString(dataRow[FieldHomeZipCode]);
            this.HomeAddress = BaseBusinessLogic.ConvertToString(dataRow[FieldHomeAddress]);
            this.HomeFax = BaseBusinessLogic.ConvertToString(dataRow[FieldHomeFax]);
            this.CarCode = BaseBusinessLogic.ConvertToString(dataRow[FieldCarCode]);
            this.EmergencyContact = BaseBusinessLogic.ConvertToString(dataRow[FieldEmergencyContact]);
            this.NativePlace = BaseBusinessLogic.ConvertToString(dataRow[FieldNativePlace]);
            this.Party = BaseBusinessLogic.ConvertToString(dataRow[FieldParty]);
            this.Nation = BaseBusinessLogic.ConvertToString(dataRow[FieldNation]);
            this.Nationality = BaseBusinessLogic.ConvertToString(dataRow[FieldNationality]);
            this.Major = BaseBusinessLogic.ConvertToString(dataRow[FieldMajor]);
            this.WorkingProperty = BaseBusinessLogic.ConvertToString(dataRow[FieldWorkingProperty]);
            this.Competency = BaseBusinessLogic.ConvertToString(dataRow[FieldCompetency]);
            this.IsDimission = BaseBusinessLogic.ConvertToInt(dataRow[FieldIsDimission]);
            this.DimissionDate = BaseBusinessLogic.ConvertToString(dataRow[FieldDimissionDate]);
            this.DimissionCause = BaseBusinessLogic.ConvertToString(dataRow[FieldDimissionCause]);
            this.DimissionWhither = BaseBusinessLogic.ConvertToString(dataRow[FieldDimissionWhither]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
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

        public BaseStaffEntity GetFrom(DataTable dataTable)
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

        public BaseStaffEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToInt(dataReader[FieldId]);
            this.UserId = BaseBusinessLogic.ConvertToInt(dataReader[FieldUserId]);
            this.UserName = BaseBusinessLogic.ConvertToString(dataReader[FieldUserName]);
            this.RealName = BaseBusinessLogic.ConvertToString(dataReader[FieldRealName]);
            this.Code = BaseBusinessLogic.ConvertToString(dataReader[FieldCode]);
            this.Gender = BaseBusinessLogic.ConvertToString(dataReader[FieldGender]);
            this.CompanyId = BaseBusinessLogic.ConvertToInt(dataReader[FieldCompanyId]);
            this.DepartmentId = BaseBusinessLogic.ConvertToInt(dataReader[FieldDepartmentId]);
            this.WorkgroupId = BaseBusinessLogic.ConvertToInt(dataReader[FieldWorkgroupId]);
            this.DutyId = BaseBusinessLogic.ConvertToString(dataReader[FieldDutyId]);
            this.IdentificationCode = BaseBusinessLogic.ConvertToString(dataReader[FieldIdentificationCode]);
            this.IdCard = BaseBusinessLogic.ConvertToString(dataReader[FieldIdCard]);
            this.BankCode = BaseBusinessLogic.ConvertToString(dataReader[FieldBankCode]);
            this.Email = BaseBusinessLogic.ConvertToString(dataReader[FieldEmail]);
            this.Mobile = BaseBusinessLogic.ConvertToString(dataReader[FieldMobile]);
            this.ShortNumber = BaseBusinessLogic.ConvertToString(dataReader[FieldShortNumber]);
            this.Telephone = BaseBusinessLogic.ConvertToString(dataReader[FieldTelephone]);
            this.OICQ = BaseBusinessLogic.ConvertToString(dataReader[FieldOICQ]);
            this.OfficePhone = BaseBusinessLogic.ConvertToString(dataReader[FieldOfficePhone]);
            this.OfficeZipCode = BaseBusinessLogic.ConvertToString(dataReader[FieldOfficeZipCode]);
            this.OfficeAddress = BaseBusinessLogic.ConvertToString(dataReader[FieldOfficeAddress]);
            this.OfficeFax = BaseBusinessLogic.ConvertToString(dataReader[FieldOfficeFax]);
            this.HomePhone = BaseBusinessLogic.ConvertToString(dataReader[FieldHomePhone]);
            this.Age = BaseBusinessLogic.ConvertToString(dataReader[FieldAge]);
            this.CarCode = BaseBusinessLogic.ConvertToString(dataReader[FieldCarCode]);
            this.Birthday = BaseBusinessLogic.ConvertToString(dataReader[FieldBirthday]);
            this.Education = BaseBusinessLogic.ConvertToString(dataReader[FieldEducation]);
            this.School = BaseBusinessLogic.ConvertToString(dataReader[FieldSchool]);
            this.Degree = BaseBusinessLogic.ConvertToString(dataReader[FieldDegree]);
            this.TitleId = BaseBusinessLogic.ConvertToString(dataReader[FieldTitleId]);
            this.TitleDate = BaseBusinessLogic.ConvertToString(dataReader[FieldTitleDate]);
            this.TitleLevel = BaseBusinessLogic.ConvertToString(dataReader[FieldTitleLevel]);
            this.WorkingDate = BaseBusinessLogic.ConvertToString(dataReader[FieldWorkingDate]);
            this.JoinInDate = BaseBusinessLogic.ConvertToString(dataReader[FieldJoinInDate]);
            this.HomeZipCode = BaseBusinessLogic.ConvertToString(dataReader[FieldHomeZipCode]);
            this.HomeAddress = BaseBusinessLogic.ConvertToString(dataReader[FieldHomeAddress]);
            this.HomeFax = BaseBusinessLogic.ConvertToString(dataReader[FieldHomeFax]);
            this.NativePlace = BaseBusinessLogic.ConvertToString(dataReader[FieldNativePlace]);
            this.Party = BaseBusinessLogic.ConvertToString(dataReader[FieldParty]);
            this.Nation = BaseBusinessLogic.ConvertToString(dataReader[FieldNation]);
            this.Nationality = BaseBusinessLogic.ConvertToString(dataReader[FieldNationality]);
            this.Major = BaseBusinessLogic.ConvertToString(dataReader[FieldMajor]);
            this.WorkingProperty = BaseBusinessLogic.ConvertToString(dataReader[FieldWorkingProperty]);
            this.Competency = BaseBusinessLogic.ConvertToString(dataReader[FieldCompetency]);
            this.IsDimission = BaseBusinessLogic.ConvertToInt(dataReader[FieldIsDimission]);
            this.DimissionDate = BaseBusinessLogic.ConvertToString(dataReader[FieldDimissionDate]);
            this.DimissionCause = BaseBusinessLogic.ConvertToString(dataReader[FieldDimissionCause]);
            this.DimissionWhither = BaseBusinessLogic.ConvertToString(dataReader[FieldDimissionWhither]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
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

        public string Age
        {
            get
            {
                return this.age;
            }
            set
            {
                this.age = value;
            }
        }

        public string BankCode
        {
            get
            {
                return this.bankCode;
            }
            set
            {
                this.bankCode = value;
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

        public string CarCode
        {
            get
            {
                return this.carCode;
            }
            set
            {
                this.carCode = value;
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

        public string Competency
        {
            get
            {
                return this.competency;
            }
            set
            {
                this.competency = value;
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

        public string Degree
        {
            get
            {
                return this.degree;
            }
            set
            {
                this.degree = value;
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

        public string DimissionCause
        {
            get
            {
                return this.dimissionCause;
            }
            set
            {
                this.dimissionCause = value;
            }
        }

        public string DimissionDate
        {
            get
            {
                return this.dimissionDate;
            }
            set
            {
                this.dimissionDate = value;
            }
        }

        public string DimissionWhither
        {
            get
            {
                return this.dimissionWhither;
            }
            set
            {
                this.dimissionWhither = value;
            }
        }

        public string DutyId
        {
            get
            {
                return this.dutyId;
            }
            set
            {
                this.dutyId = value;
            }
        }

        public string Education
        {
            get
            {
                return this.education;
            }
            set
            {
                this.education = value;
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

        public string EmergencyContact
        {
            get
            {
                return this.emergencyContact;
            }
            set
            {
                this.emergencyContact = value;
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

        public string HomeAddress
        {
            get
            {
                return this.homeAddress;
            }
            set
            {
                this.homeAddress = value;
            }
        }

        public string HomeFax
        {
            get
            {
                return this.homeFax;
            }
            set
            {
                this.homeFax = value;
            }
        }

        public string HomePhone
        {
            get
            {
                return this.homePhone;
            }
            set
            {
                this.homePhone = value;
            }
        }

        public string HomeZipCode
        {
            get
            {
                return this.homeZipCode;
            }
            set
            {
                this.homeZipCode = value;
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

        public string IdCard
        {
            get
            {
                return this.idCard;
            }
            set
            {
                this.idCard = value;
            }
        }

        public string IdentificationCode
        {
            get
            {
                return this.identificationCode;
            }
            set
            {
                this.identificationCode = value;
            }
        }

        public int? IsDimission
        {
            get
            {
                return this.isDimission;
            }
            set
            {
                this.isDimission = value;
            }
        }

        public string JoinInDate
        {
            get
            {
                return this.joinInDate;
            }
            set
            {
                this.joinInDate = value;
            }
        }

        public string Major
        {
            get
            {
                return this.major;
            }
            set
            {
                this.major = value;
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

        public string Nation
        {
            get
            {
                return this.nation;
            }
            set
            {
                this.nation = value;
            }
        }

        public string Nationality
        {
            get
            {
                return this.nationality;
            }
            set
            {
                this.nationality = value;
            }
        }

        public string NativePlace
        {
            get
            {
                return this.nativePlace;
            }
            set
            {
                this.nativePlace = value;
            }
        }

        public string OfficeAddress
        {
            get
            {
                return this.officeAddress;
            }
            set
            {
                this.officeAddress = value;
            }
        }

        public string OfficeFax
        {
            get
            {
                return this.officeFax;
            }
            set
            {
                this.officeFax = value;
            }
        }

        public string OfficePhone
        {
            get
            {
                return this.officePhone;
            }
            set
            {
                this.officePhone = value;
            }
        }

        public string OfficeZipCode
        {
            get
            {
                return this.officeZipCode;
            }
            set
            {
                this.officeZipCode = value;
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

        public string Party
        {
            get
            {
                return this.party;
            }
            set
            {
                this.party = value;
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

        public string School
        {
            get
            {
                return this.school;
            }
            set
            {
                this.school = value;
            }
        }

        public string ShortNumber
        {
            get
            {
                return this.shortNumber;
            }
            set
            {
                this.shortNumber = value;
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

        public string TitleDate
        {
            get
            {
                return this.titleDate;
            }
            set
            {
                this.titleDate = value;
            }
        }

        public string TitleId
        {
            get
            {
                return this.titleId;
            }
            set
            {
                this.titleId = value;
            }
        }

        public string TitleLevel
        {
            get
            {
                return this.titleLevel;
            }
            set
            {
                this.titleLevel = value;
            }
        }

        public int? UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
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

        public string WorkingDate
        {
            get
            {
                return this.workingDate;
            }
            set
            {
                this.workingDate = value;
            }
        }

        public string WorkingProperty
        {
            get
            {
                return this.workingProperty;
            }
            set
            {
                this.workingProperty = value;
            }
        }
    }
}

