//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

// 修改纪录
//
// 2012-08-28 版本：1.1 sunmiao 只有Identity为true，才用数据库内置的自增机制
// 2012-06-01 版本：1.0 sunmiao 可在数据表中设置产生序列码
//	

namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// BaseSequenceManager 序号ID
    /// </summary>
    public class BaseSequenceManager : BaseManager, IBaseManager
    {
        public string DefaultPrefix; //缺省前缀
        public int DefaultReduction; //缺省递减序列
        public string DefaultSeparator;
        public int DefaultSequence; //缺省递增序列
        public int DefaultSequenceLength;
        public int DefaultStep;
        public bool FillZeroPrefix;
        public int SequenceLength;
        private static readonly object SequenceLock = new object();
        public bool UsePrefix; //使用前缀
        private const string dateFormat = "yyMMdd";

        public BaseSequenceManager()
        {
            this.FillZeroPrefix = true;
            this.DefaultSequence = 0x989680;
            this.DefaultReduction = 0x98967f;
            this.DefaultPrefix = "";
            this.DefaultSeparator = "";
            this.DefaultStep = 1;
            this.DefaultSequenceLength = 8;
            this.SequenceLength = 8;
            this.UsePrefix = true;
            base.CurrentTableName = BaseSequenceEntity.TableName;
            base.PrimaryKey = "Id";
        }

        public BaseSequenceManager(BaseUserInfo userInfo) : this()
        {
            base.UserInfo = userInfo;
        }

        public BaseSequenceManager(IDbHelper dbHelper) : this()
        {
            base.DbHelper = dbHelper;
        }

        public BaseSequenceManager(string tableName)
        {
            this.FillZeroPrefix = true;
            this.DefaultSequence = 0x989680;
            this.DefaultReduction = 0x98967f;
            this.DefaultPrefix = "";
            this.DefaultSeparator = "";
            this.DefaultStep = 1;
            this.DefaultSequenceLength = 8;
            this.SequenceLength = 8;
            this.UsePrefix = true;
            base.CurrentTableName = tableName;
        }

        public BaseSequenceManager(IDbHelper dbHelper, BaseUserInfo userInfo) : this(dbHelper)
        {
            base.UserInfo = userInfo;
        }

        public BaseSequenceManager(IDbHelper dbHelper, bool identity) : this()
        {
            base.DbHelper = dbHelper;
            base.Identity = identity;
        }

        public BaseSequenceManager(IDbHelper dbHelper, BaseUserInfo userInfo, string tableName) : this(dbHelper, userInfo)
        {
            base.CurrentTableName = tableName;
        }

        public string Add(BaseSequenceEntity baseSequenceEntity)
        {
            return this.AddEntity(baseSequenceEntity);
        }

        public string Add(BaseSequenceEntity sequenceEntity, out string statusCode)
        {
            string str = string.Empty;
            if (this.Exists(BaseSequenceEntity.FieldFullName, sequenceEntity.FullName))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return str;
            }
            str = this.AddEntity(sequenceEntity);
            statusCode = StatusCode.OKAdd.ToString();
            return str;
        }

        public string Add(BaseSequenceEntity baseSequenceEntity, bool identity)
        {
            base.Identity = identity;
            return this.AddEntity(baseSequenceEntity);
        }

        public string AddEntity(BaseSequenceEntity baseSequenceEntity)
        {
            string sequence = string.Empty;
            if (baseSequenceEntity.Id != null)
            {
                sequence = baseSequenceEntity.Id.ToString();
            }
            base.Identity = false;
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper, base.Identity, base.ReturnId);
            sqlBuilder.BeginInsert(base.CurrentTableName, BaseSequenceEntity.FieldId);
            if (!base.Identity)
            {
                if (string.IsNullOrEmpty(baseSequenceEntity.Id))
                {
                    sequence = BaseBusinessLogic.NewGuid();
                    baseSequenceEntity.Id = sequence;
                }
                sqlBuilder.SetValue(BaseSequenceEntity.FieldId, baseSequenceEntity.Id, null);
            }
            else if (!base.ReturnId && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle)
                {
                    sqlBuilder.SetFormula(BaseSequenceEntity.FieldId, "SEQ_" + base.CurrentTableName.ToUpper() + ".NEXTVAL ");
                }
                if (base.DbHelper.CurrentDbType == CurrentDbType.DB2)
                {
                    sqlBuilder.SetFormula(BaseSequenceEntity.FieldId, "NEXT VALUE FOR SEQ_" + base.CurrentTableName.ToUpper());
                }
            }
            else if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.Oracle) || (base.DbHelper.CurrentDbType == CurrentDbType.DB2)))
            {
                if (string.IsNullOrEmpty(baseSequenceEntity.Id))
                {
                    if (string.IsNullOrEmpty(sequence))
                    {
                        sequence = new BaseSequenceManager(base.DbHelper, base.Identity).GetSequence(base.CurrentTableName);
                    }
                    baseSequenceEntity.Id = sequence;
                }
                sqlBuilder.SetValue(BaseSequenceEntity.FieldId, baseSequenceEntity.Id, null);
            }
            this.SetEntity(sqlBuilder, baseSequenceEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseSequenceEntity.FieldCreateUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseSequenceEntity.FieldCreateBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseSequenceEntity.FieldCreateOn);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseSequenceEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseSequenceEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseSequenceEntity.FieldModifiedOn);
            if (base.Identity && ((base.DbHelper.CurrentDbType == CurrentDbType.SqlServer) || (base.DbHelper.CurrentDbType == CurrentDbType.Access)))
            {
                return sqlBuilder.EndInsert().ToString();
            }
            sqlBuilder.EndInsert();
            return sequence;
        }

        public int Delete(string id)
        {
            return this.Delete(BaseSequenceEntity.FieldId, id);
        }

        public string[] GetBatchSequence(string fullName, int sequenceCount)
        {
            return this.GetBatchSequence(fullName, sequenceCount, this.DefaultSequence);
        }

        public string[] GetBatchSequence(string fullName, int sequenceCount, int defaultSequence)
        {
            string[] strArray = new string[sequenceCount];
            lock (SequenceLock)
            {
                int num;
                int num2;
                this.DefaultSequence = defaultSequence;
                switch (base.DbHelper.CurrentDbType)
                {
                    case CurrentDbType.Oracle:
                        num2 = 0;
                        goto Label_0090;

                    case CurrentDbType.SqlServer:
                    case CurrentDbType.Access:
                    case CurrentDbType.MySql:
                    {
                        BaseSequenceEntity entityByAdd = this.GetEntityByAdd(fullName);
                        this.UpdateSequence(fullName, sequenceCount);
                        return this.GetSequence(entityByAdd, sequenceCount);
                    }
                    case CurrentDbType.DB2:
                        num = 0;
                        goto Label_0078;

                    default:
                        return strArray;
                }
            Label_006A:
                strArray[num] = this.GetDB2Sequence(fullName);
                num++;
            Label_0078:
                if (num < sequenceCount)
                {
                    goto Label_006A;
                }
                return strArray;
            Label_0082:
                strArray[num2] = this.GetOracleSequence(fullName);
                num2++;
            Label_0090:
                if (num2 < sequenceCount)
                {
                    goto Label_0082;
                }
            }
            return strArray;
        }

        private string GetDB2Sequence(string fullName)
        {
            return base.DbHelper.ExecuteScalar("SELECT NEXTVAL FOR SEQ_" + fullName.ToUpper() + " FROM sysibm.sysdummy1").ToString();
        }

        private string GetOracleSequence(string fullName)
        {
            return base.DbHelper.ExecuteScalar("SELECT SEQ_" + fullName.ToUpper() + ".NEXTVAL FROM DUAL ").ToString();
        }

        public BaseSequenceEntity GetEntity(string id)
        {
            return new BaseSequenceEntity(this.GetDT(BaseSequenceEntity.FieldId, id));
        }

        /// <summary>
        /// 返回序列实体（无锁定机制，若实体不存在自动新增）
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private BaseSequenceEntity GetEntityByAdd(string fullName)
        {
            BaseSequenceEntity baseSequenceEntity = null;
            baseSequenceEntity = this.GetEntityByName(fullName);
            if (baseSequenceEntity == null)
            {
                baseSequenceEntity = new BaseSequenceEntity {
                    Id = BaseBusinessLogic.NewGuid(),
                    FullName = fullName,
                    Sequence = new int?(this.DefaultSequence),
                    Reduction = new int?(this.DefaultReduction),
                    Step = new int?(this.DefaultStep),
                    Prefix = this.DefaultPrefix,
                    Separator = this.DefaultSeparator
                };
                this.Add(baseSequenceEntity, false);
            }
            return baseSequenceEntity;
        }

        private BaseSequenceEntity GetEntityByName(string fullName)
        {
            BaseSequenceEntity entity = null;
            DataTable dT = this.GetDT(BaseSequenceEntity.FieldFullName, fullName);
            if (dT.Rows.Count > 0)
            {
                entity = new BaseSequenceEntity(dT);
            }
            return entity;
        }

        //public string GetOldSequence(string fullName)
        //{
        //    return this.GetOldSequence(fullName, this.DefaultSequence, this.DefaultSequenceLength, this.FillZeroPrefix);
        //}

        //public string GetOldSequence(string fullName, int defaultSequence)
        //{
        //    return this.GetOldSequence(fullName, defaultSequence, this.DefaultSequenceLength, this.FillZeroPrefix);
        //}

        //public string GetOldSequence(string fullName, int defaultSequence, int sequenceLength)
        //{
        //    return this.GetOldSequence(fullName, defaultSequence, sequenceLength, false);
        //}

        //public string GetOldSequence(string fullName, int defaultSequence, int sequenceLength, bool fillZeroPrefix)
        //{
        //    lock (SequenceLock)
        //    {
        //        this.SequenceLength = sequenceLength;
        //        this.FillZeroPrefix = fillZeroPrefix;
        //        this.DefaultReduction = defaultSequence;
        //        this.DefaultSequence = defaultSequence + 1;
        //        BaseSequenceEntity entityByAdd = this.GetEntityByAdd(fullName);
        //        return this.GetSequence(entityByAdd);
        //    }
        //}


        private string GetReduction(BaseSequenceEntity sequenceEntity)
        {
            string str = sequenceEntity.Reduction.ToString();
            if (this.FillZeroPrefix)
            {
                //str = BaseBusinessLogic.RepeatString("0", this.SequenceLength - sequenceEntity.Reduction.ToString().Length) + sequenceEntity.Reduction.ToString();
                str = sequenceEntity.Sequence.ToString().PadLeft(this.SequenceLength, '0');
            }
            if (this.UsePrefix)
            {
                str = sequenceEntity.Prefix + sequenceEntity.Separator + str;
            }
            return str;
        }

        /// <summary>
        /// 获得递减序列
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public string GetReduction(string fullName)
        {
            return this.GetReduction(fullName, this.DefaultSequence);
        }

        public string GetReduction(string fullName, int defaultSequence)
        {
            BaseSequenceEntity sequenceEntity = null;
            lock (SequenceLock)
            {
                this.DefaultReduction = defaultSequence;
                this.DefaultSequence = defaultSequence + 1;
                switch (base.DbHelper.CurrentDbType)
                {
                    case CurrentDbType.Oracle:
                        if (!base.DbHelper.InTransaction)
                        {
                            goto Label_00CC;
                        }
                        sequenceEntity = this.GetSequenceByLock(fullName, defaultSequence);
                        if (base.ReturnStatusCode == StatusCode.LockOK.ToString())
                        {
                            if (this.UpdateReduction(fullName) <= 0)
                            {
                                break;
                            }
                            base.ReturnStatusCode = StatusCode.LockOK.ToString();
                        }
                        goto Label_018D;

                    case CurrentDbType.SqlServer:
                    case CurrentDbType.Access:
                    case CurrentDbType.MySql:
                        sequenceEntity = this.GetEntityByAdd(fullName);
                        this.UpdateReduction(fullName);
                        goto Label_018D;

                    default:
                        goto Label_018D;
                }
                base.ReturnStatusCode = StatusCode.CanNotLock.ToString();
                goto Label_018D;
            Label_00CC:
                try
                {
                    base.DbHelper.BeginTransaction();
                    base.ReturnStatusCode = StatusCode.CanNotLock.ToString();
                    sequenceEntity = this.GetSequenceByLock(fullName, defaultSequence);
                    if (base.ReturnStatusCode == StatusCode.LockOK.ToString())
                    {
                        base.ReturnStatusCode = StatusCode.CanNotLock.ToString();
                        if (this.UpdateReduction(fullName) > 0)
                        {
                            base.DbHelper.CommitTransaction();
                            base.ReturnStatusCode = StatusCode.LockOK.ToString();
                        }
                        else
                        {
                            base.DbHelper.RollbackTransaction();
                        }
                    }
                    else
                    {
                        base.DbHelper.RollbackTransaction();
                    }
                }
                catch
                {
                    base.DbHelper.RollbackTransaction();
                    base.ReturnStatusCode = StatusCode.CanNotLock.ToString();
                }
            Label_018D:;
            }
            return this.GetReduction(sequenceEntity);
        }

        /// <summary>
        /// 返回构造好的序列值
        /// </summary>
        /// <param name="sequenceEntity"></param>
        /// <returns></returns>
        private string GetSequence(BaseSequenceEntity sequenceEntity)
        {
            this.SequenceLength = sequenceEntity.SequenceLength.Value;

            if (sequenceEntity.UseDatePrefix) //使用日期前缀
            {
                if (DateTime.Today != sequenceEntity.ModifiedOn.Value.Date)  //每日需重新初始化序列
                {
                    ResetSequence(sequenceEntity.FullName);
                    sequenceEntity = GetSequenceAndUpdate(sequenceEntity.FullName, DefaultSequence, SequenceLength, FillZeroPrefix);
                }
            }

            string str = sequenceEntity.Sequence.ToString();
            if (this.FillZeroPrefix)
            {
                //str = BaseBusinessLogic.RepeatString("0", this.SequenceLength - sequenceEntity.Sequence.ToString().Length) + sequenceEntity.Sequence.ToString();
                str = sequenceEntity.Sequence.ToString().PadLeft(sequenceEntity.SequenceLength.Value, '0');
            }
            if (this.UsePrefix)
            {
                if (sequenceEntity.UseDatePrefix) //使用日期格式前缀
                {

                    str = sequenceEntity.Prefix + sequenceEntity.Separator + sequenceEntity.ModifiedOn.Value.ToString(dateFormat) + str;
                }
                else
                {
                    str = sequenceEntity.Prefix + sequenceEntity.Separator + str;
                }

                //str = sequenceEntity.Prefix + sequenceEntity.Separator + str;
            }
            return str;
        }

        /// <summary>
        /// 返回递增序列值
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public string GetSequence(string fullName)
        {
            if (base.DbHelper.CurrentDbType == CurrentDbType.Oracle && this.Identity) //只有Identity为true，才用数据库内置的自增机制
            {
                return this.GetOracleSequence(fullName);
            }
            if (base.DbHelper.CurrentDbType == CurrentDbType.DB2 && this.Identity)
            {
                return this.GetDB2Sequence(fullName);
            }
            return this.GetSequence(fullName, this.DefaultSequence, this.DefaultSequenceLength, this.FillZeroPrefix);
        }

        private string[] GetSequence(BaseSequenceEntity sequenceEntity, int sequenceCount)
        {
            string[] strArray = new string[sequenceCount];
            for (int i = 0; i < sequenceCount; i++)
            {
                strArray[i] = this.GetSequence(sequenceEntity);
                sequenceEntity.Sequence += sequenceEntity.Step;
            }
            return strArray;
        }

        public string GetSequence(string fullName, int defaultSequence)
        {
            return this.GetSequence(fullName, defaultSequence, this.DefaultSequenceLength, this.FillZeroPrefix);
        }

        public string GetSequence(string fullName, int defaultSequence, int sequenceLength)
        {
            return this.GetSequence(fullName, defaultSequence, sequenceLength, false);
        }

        public string GetSequence(string fullName, int defaultSequence, int sequenceLength, bool fillZeroPrefix)
        {
            return this.GetSequence(GetSequenceAndUpdate(fullName, defaultSequence, sequenceLength, fillZeroPrefix));
        }

        public BaseSequenceEntity GetSequenceAndUpdate(string fullName, int defaultSequence, int sequenceLength, bool fillZeroPrefix)
        {
            this.DefaultSequence = defaultSequence;
            this.SequenceLength = sequenceLength;
            this.FillZeroPrefix = fillZeroPrefix;
            this.DefaultReduction = defaultSequence - 1;
            BaseSequenceEntity sequenceEntity = null;
            lock (SequenceLock)
            {
                IDbTransaction transaction;
                switch (base.DbHelper.CurrentDbType)
                {
                    case CurrentDbType.Oracle:
                        if (!base.DbHelper.InTransaction)
                        {
                            goto Label_00DE;
                        }
                        sequenceEntity = this.GetSequenceByLock(fullName, defaultSequence);
                        if (base.ReturnStatusCode == StatusCode.LockOK.ToString())
                        {
                            if (this.UpdateSequence(fullName) <= 0)
                            {
                                break;
                            }
                            base.ReturnStatusCode = StatusCode.LockOK.ToString();
                        }
                        goto Label_0192;

                    case CurrentDbType.SqlServer:
                    case CurrentDbType.Access:
                    case CurrentDbType.MySql:
                        sequenceEntity = this.GetEntityByAdd(fullName);
                        this.UpdateSequence(fullName);
                        goto Label_0192;

                    default:
                        goto Label_0192;
                }
                base.ReturnStatusCode = StatusCode.CanNotLock.ToString();
                goto Label_0192;
            Label_00DE:
                transaction = base.DbHelper.BeginTransaction();
                try
                {
                    base.ReturnStatusCode = StatusCode.CanNotLock.ToString();
                    sequenceEntity = this.GetSequenceByLock(fullName, defaultSequence);
                    if (base.ReturnStatusCode == StatusCode.LockOK.ToString())
                    {
                        base.ReturnStatusCode = StatusCode.CanNotLock.ToString();
                        if (this.UpdateSequence(fullName) > 0)
                        {
                            transaction.Commit();
                            base.ReturnStatusCode = StatusCode.LockOK.ToString();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    transaction.Rollback();
                    base.ReturnStatusCode = StatusCode.CanNotLock.ToString();
                }
            Label_0192: ;
            }
            return sequenceEntity;
        }

        /// <summary>
        /// 获得当前序列实体（使用锁定机制）
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="defaultSequence"></param>
        /// <returns></returns>
        protected BaseSequenceEntity GetSequenceByLock(string fullName, int defaultSequence)
        {
            BaseSequenceEntity baseSequenceEntity = new BaseSequenceEntity();
            baseSequenceEntity = this.GetEntityByName(fullName);
            //若存在此序列记录，返回序列实体
            if (baseSequenceEntity != null)
            {
                base.ReturnStatusCode = StatusCode.CanNotLock.ToString();
                for (int j = 0; j < BaseSystemInfo.LockNoWaitCount; j++)
                {
                    if (DbLogic.LockNoWait(base.DbHelper, BaseSequenceEntity.TableName, BaseSequenceEntity.FieldFullName, fullName) > 0)
                    {
                        baseSequenceEntity = this.GetEntityByName(fullName);
                        base.ReturnStatusCode = StatusCode.LockOK.ToString();
                        return baseSequenceEntity;
                    }
                    Thread.Sleep(BaseRandom.GetRandom(1, BaseSystemInfo.LockNoWaitTickMilliSeconds));
                }
                return baseSequenceEntity;
            }
            //若不存在此序列继续，在序列表中新增此序列记录
            base.ReturnStatusCode = StatusCode.CanNotLock.ToString();
            for (int i = 0; i < BaseSystemInfo.LockNoWaitCount; i++)
            {
                if (DbLogic.LockNoWait(base.DbHelper, BaseSequenceEntity.TableName, BaseSequenceEntity.FieldFullName, BaseSequenceEntity.TableName) > 0)
                {
                    baseSequenceEntity.FullName = fullName;
                    baseSequenceEntity.Reduction = new int?(defaultSequence - 1);
                    baseSequenceEntity.Sequence = new int?(defaultSequence);
                    baseSequenceEntity.Step = new int?(this.DefaultStep);
                    this.AddEntity(baseSequenceEntity);
                    base.ReturnStatusCode = StatusCode.LockOK.ToString();
                    break;
                }
                Thread.Sleep(BaseRandom.GetRandom(1, BaseSystemInfo.LockNoWaitTickMilliSeconds));
            }
            if (base.ReturnStatusCode == StatusCode.LockOK.ToString())
            {
                baseSequenceEntity = this.GetEntityByName(fullName);
            }
            return baseSequenceEntity;
        }

        public int Reset(string[] ids)
        {
            int num = 0;
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i].Length > 0)
                {
                    builder.BeginUpdate(BaseSequenceEntity.TableName);
                    builder.SetValue(BaseSequenceEntity.FieldSequence, this.DefaultSequence, null);
                    builder.SetValue(BaseSequenceEntity.FieldReduction, this.DefaultReduction, null);
                    builder.SetWhere(BaseSequenceEntity.FieldId, ids[i]);
                    num += builder.EndUpdate();
                }
            }
            return num;
        }

        private void SetEntity(SQLBuilder sqlBuilder, BaseSequenceEntity baseSequenceEntity)
        {
            sqlBuilder.SetValue(BaseSequenceEntity.FieldFullName, baseSequenceEntity.FullName, null);
            sqlBuilder.SetValue(BaseSequenceEntity.FieldPrefix, baseSequenceEntity.Prefix, null);
            sqlBuilder.SetValue(BaseSequenceEntity.FieldSeparator, baseSequenceEntity.Separator, null);
            sqlBuilder.SetValue(BaseSequenceEntity.FieldSequence, baseSequenceEntity.Sequence, null);
            sqlBuilder.SetValue(BaseSequenceEntity.FieldReduction, baseSequenceEntity.Reduction, null);
            sqlBuilder.SetValue(BaseSequenceEntity.FieldStep, baseSequenceEntity.Step, null);
            sqlBuilder.SetValue(BaseSequenceEntity.FieldDescription, baseSequenceEntity.Description, null);
            sqlBuilder.SetValue(BaseSequenceEntity.FieldUseDatePrefix, baseSequenceEntity.UseDatePrefix?1:0, null);
            sqlBuilder.SetValue(BaseSequenceEntity.FieldSequenceLength, baseSequenceEntity.SequenceLength, null);
        }

        public int Update(BaseSequenceEntity baseSequenceEntity)
        {
            return this.UpdateEntity(baseSequenceEntity);
        }

        public int Update(BaseSequenceEntity sequenceEntity, out string statusCode)
        {
            int num = 0;
            if (this.Exists(BaseSequenceEntity.FieldFullName, sequenceEntity.FullName, sequenceEntity.Id))
            {
                statusCode = StatusCode.ErrorNameExist.ToString();
                return num;
            }
            num = this.UpdateEntity(sequenceEntity);
            if (num == 1)
            {
                statusCode = StatusCode.OKUpdate.ToString();
                return num;
            }
            statusCode = StatusCode.ErrorDeleted.ToString();
            return num;
        }

        public int UpdateEntity(BaseSequenceEntity baseSequenceEntity)
        {
            SQLBuilder sqlBuilder = new SQLBuilder(base.DbHelper);
            sqlBuilder.BeginUpdate(base.CurrentTableName);
            this.SetEntity(sqlBuilder, baseSequenceEntity);
            if (base.UserInfo != null)
            {
                sqlBuilder.SetValue(BaseSequenceEntity.FieldModifiedUserId, base.UserInfo.Id, null);
                sqlBuilder.SetValue(BaseSequenceEntity.FieldModifiedBy, base.UserInfo.RealName, null);
            }
            sqlBuilder.SetDBNow(BaseSequenceEntity.FieldModifiedOn);
            sqlBuilder.SetWhere(BaseSequenceEntity.FieldId, baseSequenceEntity.Id);
            return sqlBuilder.EndUpdate();
        }

        protected int UpdateReduction(string fullName)
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(BaseSequenceEntity.TableName);
            builder.SetFormula(BaseSequenceEntity.FieldReduction, BaseSequenceEntity.FieldReduction + " - " + BaseSequenceEntity.FieldStep);
            builder.SetWhere(BaseSequenceEntity.FieldFullName, fullName);
            return builder.EndUpdate();
        }

        protected int UpdateSequence(string fullName)
        {
            return this.UpdateSequence(fullName, 1);
        }

        protected int UpdateSequence(string fullName, int sequenceCount)
        {
            SQLBuilder builder = new SQLBuilder(base.DbHelper);
            builder.BeginUpdate(BaseSequenceEntity.TableName);
            builder.SetFormula(BaseSequenceEntity.FieldSequence, BaseSequenceEntity.FieldSequence + " + " + sequenceCount.ToString() + " * " + BaseSequenceEntity.FieldStep);
            builder.SetWhere(BaseSequenceEntity.FieldFullName, fullName);
            builder.SetDBNow(BaseSequenceEntity.FieldModifiedOn);
            return builder.EndUpdate();
        }

        /// <summary>
        /// 重置序列
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private int ResetSequence(string fullName)
        {
            return this.SetProperty(BaseSequenceEntity.FieldFullName, fullName, BaseSequenceEntity.FieldSequence, 1);
        }
    }
}

