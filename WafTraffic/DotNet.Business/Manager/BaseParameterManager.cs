namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    public class BaseParameterManager : BaseManager
    {
        public BaseParameterManager()
        {
            if (base.dbHelper == null)
            {
                base.dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, BaseSystemInfo.UserCenterDbConnection);
            }
            base.CurrentTableName = BaseParameterEntity.TableName;
        }

        public BaseParameterManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseParameterManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseParameterManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this()
        {
            base.DbHelper = dbHelper;
            base.UserInfo = userInfo;
        }

        public string Add(BaseParameterEntity parameterEntity)
        {
            string str = string.Empty;
            string[] names = new string[3];
            object[] values = new object[3];
            names[0] = BaseParameterEntity.FieldCategoryId;
            names[1] = BaseParameterEntity.FieldParameterId;
            names[2] = BaseParameterEntity.FieldParameterCode;
            values[0] = parameterEntity.CategoryId;
            values[1] = parameterEntity.ParameterId;
            values[2] = parameterEntity.ParameterCode;
            if (this.Exists(names, values))
            {
                base.ReturnStatusCode = StatusCode.ErrorCodeExist.ToString();
                return str;
            }
            str = this.AddEntity(parameterEntity);
            base.ReturnStatusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(string categoryId, string parameterId, string parameterCode, string parameterContent, bool worked, bool enabled)
        {
            BaseParameterEntity parameterEntity = new BaseParameterEntity {
                CategoryId = categoryId,
                ParameterId = parameterId,
                ParameterCode = parameterCode,
                ParameterContent = parameterContent,
                Worked = worked,
                Enabled = enabled
            };
            return this.Add(parameterEntity);
        }

        public string AddEntity(BaseParameterEntity parameterEntity)
        {
            string sequence = new BaseSequenceManager(base.DbHelper).GetSequence(BaseParameterEntity.TableName);
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginInsert(BaseParameterEntity.TableName);
            builder.SetValue(BaseParameterEntity.FieldId, sequence, null);
            builder.SetValue(BaseParameterEntity.FieldCategoryId, parameterEntity.CategoryId, null);
            builder.SetValue(BaseParameterEntity.FieldParameterId, parameterEntity.ParameterId, null);
            builder.SetValue(BaseParameterEntity.FieldParameterCode, parameterEntity.ParameterCode, null);
            builder.SetValue(BaseParameterEntity.FieldWorked, parameterEntity.Worked ? 1 : 0, null);
            builder.SetValue(BaseParameterEntity.FieldParameterContent, parameterEntity.ParameterContent, null);
            builder.SetValue(BaseParameterEntity.FieldEnabled, parameterEntity.Enabled ? 1 : 0, null);
            builder.SetValue(BaseParameterEntity.FieldDescription, parameterEntity.Description, null);
            if (base.UserInfo != null)
            {
                builder.SetValue(BaseParameterEntity.FieldCreateUserId, base.UserInfo.Id, null);
                builder.SetValue(BaseParameterEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            builder.SetDBNow(BaseParameterEntity.FieldCreateOn);
            if (builder.EndInsert() <= 0)
            {
                return string.Empty;
            }
            return sequence;
        }

        public int DeleteByParameter(string categoryId, string parameterId)
        {
            string[] names = new string[] { BaseParameterEntity.FieldCategoryId, BaseParameterEntity.FieldParameterId };
            object[] values = new object[] { categoryId, parameterId };
            return this.Delete(names, values);
        }

        public int DeleteByParameterCode(string categoryId, string parameterId, string parameterCode)
        {
            string[] names = new string[] { BaseParameterEntity.FieldCategoryId, BaseParameterEntity.FieldParameterId, BaseParameterEntity.FieldParameterCode };
            object[] values = new object[] { categoryId, parameterId, parameterCode };
            return this.Delete(names, values);
        }

        public DataTable GetDTByParameter(string categoryId, string parameterId)
        {
            string[] names = new string[] { BaseParameterEntity.FieldCategoryId, BaseParameterEntity.FieldParameterId };
            object[] values = new object[] { categoryId, parameterId };
            return this.GetDT(names, values);
        }

        public DataTable GetDTParameterCode(string categoryId, string parameterId, string parameterCode)
        {
            string[] names = new string[] { BaseParameterEntity.FieldCategoryId, BaseParameterEntity.FieldParameterId, BaseParameterEntity.FieldParameterCode };
            object[] values = new object[] { categoryId, parameterId, parameterCode };
            return this.GetDT(names, values);
        }

        public string GetParameter(string categoryId, string parameterId, string parameterCode)
        {
            string[] names = new string[] { BaseParameterEntity.FieldCategoryId, BaseParameterEntity.FieldParameterId, BaseParameterEntity.FieldParameterCode, BaseParameterEntity.FieldDeletionStateCode };
            object[] values = new object[] { categoryId, parameterId, parameterCode, 0 };
            return this.GetProperty(names, values, BaseParameterEntity.FieldParameterContent).ToString();
        }

        public int SetParameter(string categoryId, string parameterId, string parameterCode, string parameterContent)
        {
            int num = 0;
            string[] names = new string[] { BaseParameterEntity.FieldCategoryId, BaseParameterEntity.FieldParameterId, BaseParameterEntity.FieldParameterCode, BaseParameterEntity.FieldDeletionStateCode };
            object[] values = new object[] { categoryId, parameterId, parameterCode, 0 };
            if ((parameterContent == null) || (parameterContent.Length == 0))
            {
                return this.Delete(names, values);
            }
            num = this.SetProperty(names, values, BaseParameterEntity.FieldParameterContent, parameterContent);
            if (num == 0)
            {
                BaseParameterEntity parameterEntity = new BaseParameterEntity {
                    CategoryId = categoryId,
                    ParameterId = parameterId,
                    ParameterCode = parameterCode,
                    ParameterContent = parameterContent,
                    Enabled = true,
                    DeletionStateCode = 0
                };
                this.AddEntity(parameterEntity);
                num = 1;
            }
            return num;
        }

        public int Update(BaseParameterEntity parameterEntity)
        {
            int num = 0;
            if (this.Exists(BaseParameterEntity.FieldParameterCode, parameterEntity.ParameterCode, parameterEntity.Id))
            {
                base.ReturnStatusCode = StatusCode.ErrorCodeExist.ToString();
                return num;
            }
            num = this.UpdateEntity(parameterEntity);
            if (num == 1)
            {
                base.ReturnStatusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            base.ReturnStatusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        protected int UpdateEntity(BaseParameterEntity parameterEntity)
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(BaseParameterEntity.TableName);
            builder.SetValue(BaseParameterEntity.FieldCategoryId, parameterEntity.CategoryId, null);
            builder.SetValue(BaseParameterEntity.FieldParameterCode, parameterEntity.ParameterCode, null);
            builder.SetValue(BaseParameterEntity.FieldParameterId, parameterEntity.ParameterId, null);
            builder.SetValue(BaseParameterEntity.FieldParameterContent, parameterEntity.ParameterContent, null);
            builder.SetValue(BaseParameterEntity.FieldWorked, parameterEntity.Worked ? 1 : 0, null);
            builder.SetValue(BaseParameterEntity.FieldEnabled, parameterEntity.Enabled ? 1 : 0, null);
            builder.SetValue(BaseParameterEntity.FieldModifiedUserId, base.UserInfo.Id, null);
            builder.SetValue(BaseParameterEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            builder.SetDBNow(BaseParameterEntity.FieldModifiedOn);
            builder.SetWhere(BaseParameterEntity.FieldId, parameterEntity.Id);
            return builder.EndUpdate();
        }
    }
}

