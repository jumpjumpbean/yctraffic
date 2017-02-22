using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WafTraffic.Domain
{
    public class MaterialDeclareGatherTable
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int ScorePerDept { get; set; }
        public int CountPerDept { get; set; }
    }
}
