using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WafTraffic.Domain
{
    public class MohthRgisterGatherTable
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Excel { get; set; }
        public int Well { get; set; }
        public int Good { get; set; }
        public int Normal { get; set; }
        public int Bad { get; set; }
    }

    public class MohthRgisterChartTable
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? WhichMonth { get; set; }
        public string ApproveResult { get; set; }
        public string ShortMonth { get; set; }
        public int ResultCode { get; set; }
    }
}
