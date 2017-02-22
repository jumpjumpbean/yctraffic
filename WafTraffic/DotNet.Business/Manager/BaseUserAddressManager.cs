namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;

    public class BaseUserAddressManager : BaseManager, IBaseManager
    {
        public BaseUserAddressManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseUserAddressEntity.TableName;
        }

        public BaseUserAddressManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseUserAddressManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseUserAddressManager(string tableName)
        {
            base.CurrentTableName = tableName;
        }

        public BaseUserAddressManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseUserAddressManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseUserAddressEntity baseUserAddressEntity)
        {
            return this.AddEntity(baseUserAddressEntity);
        }

        public string Add(BaseUserAddressEntity baseUserAddressEntity, bool identity, bool returnId)
        {
            base.Identity = identity;
            base.ReturnId = returnId;
            return this.AddEntity(baseUserAddressEntity);
        }

        public string AddEntity(BaseUserAddressEntity baseUserAddressEntity)
        {
            string s = string.Empty;
            s = baseUserAddressEntity.Id;
            if (baseUserAddressEntity.SortCode == 0)
            {
                s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                baseUserAddressEntity.SortCode = new int?(int.Parse(s));
            }
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(BaseUserAddressEntity.TableName, BaseUserAddressEntity.FieldId);
            if (baseUserAddressEntity.Id != null)
            {
                base.Identity = false;
            }
            if (!base.Identity)
            {
                sqlBuilder.SetValue(BaseUserAddressEntity.FieldId, baseUserAddressEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseUserAddressEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseUserAddressEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (string.IsNullOrEmpty(baseUserAddressEntity.Id))
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        s = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseUserAddressEntity.Id = s;
                }
                sqlBuilder.SetValue(BaseUserAddressEntity.FieldId, baseUserAddressEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseUserAddressEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserAddressEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserAddressEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserAddressEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserAddressEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserAddressEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserAddressEntity.FieldModifiedOn);
            if ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) && base.Identity)
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return s;
        }

        public int Delete(string id)
        {
            return this.Delete(BaseUserAddressEntity.FieldId, id);
        }

        public BaseUserAddressEntity GetEntity(string id)
        {
            return new BaseUserAddressEntity(this.GetDT(BaseUserAddressEntity.FieldId, id));
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseUserAddressEntity baseUserAddressEntity)
        {
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldUserId, baseUserAddressEntity.UserId, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldRealName, baseUserAddressEntity.RealName, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldProvinceId, baseUserAddressEntity.ProvinceId, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldProvince, baseUserAddressEntity.Province, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldCityId, baseUserAddressEntity.CityId, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldCity, baseUserAddressEntity.City, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldAreaId, baseUserAddressEntity.AreaId, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldArea, baseUserAddressEntity.Area, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldAddress, baseUserAddressEntity.Address, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldPostCode, baseUserAddressEntity.PostCode, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldPhone, baseUserAddressEntity.Phone, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldFax, baseUserAddressEntity.Fax, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldMobile, baseUserAddressEntity.Mobile, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldEmail, baseUserAddressEntity.Email, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldDeliverCategory, baseUserAddressEntity.DeliverCategory, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldSortCode, baseUserAddressEntity.SortCode, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldDeletionStateCode, baseUserAddressEntity.DeletionStateCode, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldEnabled, baseUserAddressEntity.Enabled, null);
            sqlBuilder.SetValue(BaseUserAddressEntity.FieldDescription, baseUserAddressEntity.Description, null);
        }

        public int Update(BaseUserAddressEntity baseUserAddressEntity)
        {
            return this.UpdateEntity(baseUserAddressEntity);
        }

        public int UpdateEntity(BaseUserAddressEntity baseUserAddressEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(BaseUserAddressEntity.TableName);
            this.SetEntity(sqlBuilder, baseUserAddressEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseUserAddressEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseUserAddressEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseUserAddressEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseUserAddressEntity.FieldId, baseUserAddressEntity.Id);
            return sqlBuilder.EndUpdate();
        }
    }
}

