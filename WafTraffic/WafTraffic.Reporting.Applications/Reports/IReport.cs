using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WafTraffic.Reporting.Applications.Reports
{
    public interface IReport
    {
        object Report { get; }

        object ReportData { get; set; }
    }
}
