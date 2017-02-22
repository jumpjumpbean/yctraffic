namespace DotNet.Business
{
    using System;

    public class WorkFlowInfo
    {
        private string callBack;
        private string categoryFullName;
        private string categoryId;
        private string objectFullName;
        private string objectId;
        private string workFlowCode;

        public string CallBack
        {
            get
            {
                return this.callBack;
            }
            set
            {
                this.callBack = value;
            }
        }

        public string CategoryCode
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

        public string CategoryFullName
        {
            get
            {
                return this.categoryFullName;
            }
            set
            {
                this.categoryFullName = value;
            }
        }

        public string ObjectFullName
        {
            get
            {
                return this.objectFullName;
            }
            set
            {
                this.objectFullName = value;
            }
        }

        public string ObjectId
        {
            get
            {
                return this.objectId;
            }
            set
            {
                this.objectId = value;
            }
        }

        public string WorkFlowCode
        {
            get
            {
                return this.workFlowCode;
            }
            set
            {
                this.workFlowCode = value;
            }
        }
    }
}

