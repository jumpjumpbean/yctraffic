namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;
    using System.Data;

    [Serializable]
    public class BaseSequenceEntity
    {
        private string createBy;
        private DateTime? createOn;
        private string createUserId;
        private string description;
        [NonSerialized]
        public static string FieldCreateBy = "CreateBy";
        [NonSerialized]
        public static string FieldCreateOn = "CreateOn";
        [NonSerialized]
        public static string FieldCreateUserId = "CreateUserId";
        [NonSerialized]
        public static string FieldDescription = "Description";
        [NonSerialized]
        public static string FieldFullName = "FullName";
        [NonSerialized]
        public static string FieldId = "Id";
        [NonSerialized]
        public static string FieldModifiedBy = "ModifiedBy";
        [NonSerialized]
        public static string FieldModifiedOn = "ModifiedOn";
        [NonSerialized]
        public static string FieldModifiedUserId = "ModifiedUserId";
        [NonSerialized]
        public static string FieldPrefix = "Prefix";
        [NonSerialized]
        public static string FieldReduction = "Reduction";
        [NonSerialized]
        public static string FieldSeparator = "Separator";
        [NonSerialized]
        public static string FieldSequence = "Sequence";
        [NonSerialized]
        public static string FieldStep = "Step";
        private string fullName;
        private string id;
        private string modifiedBy;
        private DateTime? modifiedOn;
        private string modifiedUserId;
        private string prefix;
        private int? reduction;
        private string separator;
        private int? sequence;
        private int? step;
        [NonSerialized]
        public static string TableName = "Base_Sequence";

        //日期前缀
        [NonSerialized]
        public static string FieldUseDatePrefix = "UseDatePrefix";    //是否使用日期前缀
        private bool useDatePrefix = false;

        public bool UseDatePrefix
        {
            get { return useDatePrefix; }
            set { useDatePrefix = value; }
        }

        [NonSerialized]
        public static string FieldSequenceLength = "SequenceLength";  //数字序列位数
        private int? sequenceLength = 8;

        public int? SequenceLength
        {
            get { return sequenceLength; }
            set { sequenceLength = value; }
        }

        public BaseSequenceEntity()
        {
            this.sequence = 0;
            this.reduction = 0;
            this.step = 0;
            this.createOn = null;
            this.modifiedOn = null;
        }

        public BaseSequenceEntity(DataRow dataRow)
        {
            this.sequence = 0;
            this.reduction = 0;
            this.step = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataRow);
        }

        public BaseSequenceEntity(DataTable dataTable)
        {
            this.sequence = 0;
            this.reduction = 0;
            this.step = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataTable);
        }

        public BaseSequenceEntity(IDataReader dataReader)
        {
            this.sequence = 0;
            this.reduction = 0;
            this.step = 0;
            this.createOn = null;
            this.modifiedOn = null;
            this.GetFrom(dataReader);
        }

        public BaseSequenceEntity GetFrom(DataRow dataRow)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataRow[FieldId]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataRow[FieldFullName]);
            this.Prefix = BaseBusinessLogic.ConvertToString(dataRow[FieldPrefix]);
            this.Separator = BaseBusinessLogic.ConvertToString(dataRow[FieldSeparator]);
            this.Sequence = BaseBusinessLogic.ConvertToInt(dataRow[FieldSequence]);
            this.Reduction = BaseBusinessLogic.ConvertToInt(dataRow[FieldReduction]);
            this.Step = BaseBusinessLogic.ConvertToInt(dataRow[FieldStep]);
            this.Description = BaseBusinessLogic.ConvertToString(dataRow[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataRow[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataRow[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataRow[FieldModifiedBy]);
            this.useDatePrefix = (BaseBusinessLogic.ConvertToInt(dataRow[FieldUseDatePrefix]) == 1) ? true : false;
            this.sequenceLength = BaseBusinessLogic.ConvertToInt(dataRow[FieldSequenceLength]);
            return this;
        }

        public BaseSequenceEntity GetFrom(DataTable dataTable)
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

        public BaseSequenceEntity GetFrom(IDataReader dataReader)
        {
            this.Id = BaseBusinessLogic.ConvertToString(dataReader[FieldId]);
            this.FullName = BaseBusinessLogic.ConvertToString(dataReader[FieldFullName]);
            this.Prefix = BaseBusinessLogic.ConvertToString(dataReader[FieldPrefix]);
            this.Separator = BaseBusinessLogic.ConvertToString(dataReader[FieldSeparator]);
            this.Sequence = BaseBusinessLogic.ConvertToInt(dataReader[FieldSequence]);
            this.Reduction = BaseBusinessLogic.ConvertToInt(dataReader[FieldReduction]);
            this.Step = BaseBusinessLogic.ConvertToInt(dataReader[FieldStep]);
            this.Description = BaseBusinessLogic.ConvertToString(dataReader[FieldDescription]);
            this.CreateOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldCreateOn]);
            this.CreateUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateUserId]);
            this.CreateBy = BaseBusinessLogic.ConvertToString(dataReader[FieldCreateBy]);
            this.ModifiedOn = BaseBusinessLogic.ConvertToDateTime(dataReader[FieldModifiedOn]);
            this.ModifiedUserId = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedUserId]);
            this.ModifiedBy = BaseBusinessLogic.ConvertToString(dataReader[FieldModifiedBy]);
            this.useDatePrefix = (BaseBusinessLogic.ConvertToInt(dataReader[FieldUseDatePrefix]) == 1) ? true : false;
            this.sequenceLength = BaseBusinessLogic.ConvertToInt(dataReader[FieldSequenceLength]);
            return this;
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

        public string Prefix
        {
            get
            {
                return this.prefix;
            }
            set
            {
                this.prefix = value;
            }
        }

        public int? Reduction
        {
            get
            {
                return this.reduction;
            }
            set
            {
                this.reduction = value;
            }
        }

        public string Separator
        {
            get
            {
                return this.separator;
            }
            set
            {
                this.separator = value;
            }
        }

        public int? Sequence
        {
            get
            {
                return this.sequence;
            }
            set
            {
                this.sequence = value;
            }
        }

        public int? Step
        {
            get
            {
                return this.step;
            }
            set
            {
                this.step = value;
            }
        }
    }
}

