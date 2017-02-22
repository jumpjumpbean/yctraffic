namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseUserAddressEntity
    {
        private string address;
        private string area;
        private string areaId;
        private string city;
        private string cityId;
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private int? deletionStateCode;
        private string deliverCategory;
        private string description;
        private string email;
        private int? enabled;
        private string fax;
        [NonSerialized]
        public static string FieldAddress = "Address";
        [NonSerialized]
        public static string FieldArea = "Area";
        [NonSerialized]
        public static string FieldAreaId = "AreaId";
        [NonSerialized]
        public static string FieldCity = "City";
        [NonSerialized]
        public static string FieldCityId = "CityId";
        [NonSerialized]
        public static string FieldCreateBy = "CreateBy";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDeletionStateCode = "DeletionStateCode";
        [NonSerialized]
        public static string FieldDeliverCategory = "DeliverCategory";
        [NonSerialized]
        public static string FieldDescription = "Description";
        [NonSerialized]
        public static string FieldEmail = "Email";
        [NonSerialized]
        public static string FieldEnabled = "Enabled";
        [NonSerialized]
        public static string FieldFax = "Fax";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldMobile = "Mobile";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldPhone = "Phone";
        [NonSerialized]
        public static string FieldPostCode = "PostCode";
        [NonSerialized]
        public static string FieldProvince = "Province";
        [NonSerialized]
        public static string FieldProvinceId = "ProvinceId";
        [NonSerialized]
        public static string FieldRealName = "RealName";
        [NonSerialized]
        public static string FieldSortCode = "SortCode";
        [NonSerialized]
        public static string FieldUserId = "UserId";
        private string id;
        private string mobile;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string phone;
        private string postCode;
        private string province;
        private string provinceId;
        private string realName;
        private int? sortCode;
        [NonSerialized]
        public static string TableName = "Base_UserAddress";
        private string userId;

        public BaseUserAddressEntity()
        {
            this.sortCode = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseUserAddressEntity(DataRow dataRow)
        {
            this.sortCode = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseUserAddressEntity(DataTable dataTable)
        {
            this.sortCode = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseUserAddressEntity(IDataReader dataReader)
        {
            this.sortCode = 0;
            this.deletionStateCode = 0;
            this.enabled = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseUserAddressEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.UserId = BaseBusinessLogic.ConvertToString(dataRow[FieldUserId]);
            this.RealName = BaseBusinessLogic.ConvertToString(dataRow[FieldRealName]);
            this.ProvinceId = BaseBusinessLogic.ConvertToString(dataRow[FieldProvinceId]);
            this.Province = BaseBusinessLogic.ConvertToString(dataRow[FieldProvince]);
            this.CityId = BaseBusinessLogic.ConvertToString(dataRow[FieldCityId]);
            this.City = BaseBusinessLogic.ConvertToString(dataRow[FieldCity]);
            this.AreaId = BaseBusinessLogic.ConvertToString(dataRow[FieldAreaId]);
            this.Area = BaseBusinessLogic.ConvertToString(dataRow[FieldArea]);
            this.Address = BaseBusinessLogic.ConvertToString(dataRow[FieldAddress]);
            this.PostCode = BaseBusinessLogic.ConvertToString(dataRow[FieldPostCode]);
            this.Phone = BaseBusinessLogic.ConvertToString(dataRow[FieldPhone]);
            this.Fax = BaseBusinessLogic.ConvertToString(dataRow[FieldFax]);
            this.Mobile = BaseBusinessLogic.ConvertToString(dataRow[FieldMobile]);
            this.Email = BaseBusinessLogic.ConvertToString(dataRow[FieldEmail]);
            this.DeliverCategory = BaseBusinessLogic.ConvertToString(dataRow[FieldDeliverCategory]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldSortCode]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataRow[FieldDeletionStateCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataRow[FieldEnabled]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            return this;
        }

        public BaseUserAddressEntity GetFrom(DataTable dataTable)
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

        public BaseUserAddressEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataReader[FieldId]);
            this.UserId = BaseBusinessLogic.ConvertToString(dataReader[FieldUserId]);
            this.RealName = BaseBusinessLogic.ConvertToString(dataReader[FieldRealName]);
            this.ProvinceId = BaseBusinessLogic.ConvertToString(dataReader[FieldProvinceId]);
            this.Province = BaseBusinessLogic.ConvertToString(dataReader[FieldProvince]);
            this.CityId = BaseBusinessLogic.ConvertToString(dataReader[FieldCityId]);
            this.City = BaseBusinessLogic.ConvertToString(dataReader[FieldCity]);
            this.AreaId = BaseBusinessLogic.ConvertToString(dataReader[FieldAreaId]);
            this.Area = BaseBusinessLogic.ConvertToString(dataReader[FieldArea]);
            this.Address = BaseBusinessLogic.ConvertToString(dataReader[FieldAddress]);
            this.PostCode = BaseBusinessLogic.ConvertToString(dataReader[FieldPostCode]);
            this.Phone = BaseBusinessLogic.ConvertToString(dataReader[FieldPhone]);
            this.Fax = BaseBusinessLogic.ConvertToString(dataReader[FieldFax]);
            this.Mobile = BaseBusinessLogic.ConvertToString(dataReader[FieldMobile]);
            this.Email = BaseBusinessLogic.ConvertToString(dataReader[FieldEmail]);
            this.DeliverCategory = BaseBusinessLogic.ConvertToString(dataReader[FieldDeliverCategory]);
            this.SortCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldSortCode]);
            this.DeletionStateCode = BaseBusinessLogic.ConvertToInt(dataReader[FieldDeletionStateCode]);
            this.Enabled = BaseBusinessLogic.ConvertToInt(dataReader[FieldEnabled]);
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

        public string Area
        {
            get
            {
                return this.area;
            }
            set
            {
                this.area = value;
            }
        }

        public string AreaId
        {
            get
            {
                return this.areaId;
            }
            set
            {
                this.areaId = value;
            }
        }

        public string City
        {
            get
            {
                return this.city;
            }
            set
            {
                this.city = value;
            }
        }

        public string CityId
        {
            get
            {
                return this.cityId;
            }
            set
            {
                this.cityId = value;
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

        public string DeliverCategory
        {
            get
            {
                return this.deliverCategory;
            }
            set
            {
                this.deliverCategory = value;
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

        public string Phone
        {
            get
            {
                return this.phone;
            }
            set
            {
                this.phone = value;
            }
        }

        public string PostCode
        {
            get
            {
                return this.postCode;
            }
            set
            {
                this.postCode = value;
            }
        }

        public string Province
        {
            get
            {
                return this.province;
            }
            set
            {
                this.province = value;
            }
        }

        public string ProvinceId
        {
            get
            {
                return this.provinceId;
            }
            set
            {
                this.provinceId = value;
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

        public string UserId
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
    }
}

