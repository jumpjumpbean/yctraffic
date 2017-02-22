using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WafTraffic.Domain
{
    public class ZhzxTrafficViolationExt
    {
        private string mComposedPicturePath;
        private string mSourcePicturePath1;
        private string mSourcePicturePath2;
        private string mSourcePicturePath3;
        private ZhzxTrafficViolation mZhzxTrafficViolation;

        public ZhzxTrafficViolation ZhzxTrafficViolationEntity
        {
            get { return mZhzxTrafficViolation; }
            set
            {
                if (mZhzxTrafficViolation != value)
                {
                    mZhzxTrafficViolation = value;
                }
            }
        }

        public string ComposedPicturePath
        {
            get { return mComposedPicturePath; }
            set
            {
                if (mComposedPicturePath != value)
                {
                    mComposedPicturePath = value;
                }
            }
        }

        public string SourcePicturePath1
        {
            get { return mSourcePicturePath1; }
            set
            {
                if (mSourcePicturePath1 != value)
                {
                    mSourcePicturePath1 = value;
                }
            }
        }

        public string SourcePicturePath2
        {
            get { return mSourcePicturePath2; }
            set
            {
                if (mSourcePicturePath2 != value)
                {
                    mSourcePicturePath2 = value;
                }
            }
        }

        public string SourcePicturePath3
        {
            get { return mSourcePicturePath3; }
            set
            {
                if (mSourcePicturePath3 != value)
                {
                    mSourcePicturePath3 = value;
                }
            }
        }
    }

    public class ZhzxViolationGatherTable
    {
        public string Name { get; set; }
        public int WorkflowStatus { get; set; }
        public string ViolationType { get; set; }
        public int ViolationCnt { get; set; }
    }

    public class ZhzxViolationGatherElementTable                //上传人员统计信息
    {
        public string UploadName { get; set; }
        public string ApprovalName { get; set; }
        public int UpldRjct_Redlight { get; set; }
        public int UpldRjct_WrongRoad { get; set; }
        public int UpldRjct_Reverse { get; set; }
        public int UpldRjct_YellowLine { get; set; }
        public int UpldRjct_WhiteLine { get; set; }
        public int UpldRjct_ChangeRoad { get; set; }
        public int UpldRjct_NoBelt { get; set; }
        public int UpldRjct_Count { get; set; }

        public int Upload_Redlight { get; set; }
        public int Upload_WrongRoad { get; set; }
        public int Upload_Reverse { get; set; }
        public int Upload_YellowLine { get; set; }
        public int Upload_WhiteLine { get; set; }
        public int Upload_ChangeRoad { get; set; }
        public int Upload_NoBelt { get; set; }
        public int Upload_Count { get; set; }

        public int Filter_Redlight { get; set; }
        public int Filter_WrongRoad { get; set; }
        public int Filter_Reverse { get; set; }
        public int Filter_YellowLine { get; set; }
        public int Filter_WhiteLine { get; set; }
        public int Filter_ChangeRoad { get; set; }
        public int Filter_NoBelt { get; set; }
        public int Filter_Count { get; set; }

        public int AprvRjct_Redlight { get; set; }
        public int AprvRjct_WrongRoad { get; set; }
        public int AprvRjct_Reverse { get; set; }
        public int AprvRjct_YellowLine { get; set; }
        public int AprvRjct_WhiteLine { get; set; }
        public int AprvRjct_ChangeRoad { get; set; }
        public int AprvRjct_NoBelt { get; set; }
        public int AprvRjct_Count { get; set; }

        public int Approved_Redlight { get; set; }
        public int Approved_WrongRoad { get; set; }
        public int Approved_Reverse { get; set; }
        public int Approved_YellowLine { get; set; }
        public int Approved_WhiteLine { get; set; }
        public int Approved_ChangeRoad { get; set; }
        public int Approved_NoBelt { get; set; }
        public int Approved_Count { get; set; }
    }

}
