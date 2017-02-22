using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;
using WafTraffic.Domain;

namespace WafTraffic.Reporting.Applications.DataModels
{
    public class WorkBookReportDataModel : DataModel
    {
        private readonly IEnumerable<MonthRegisterTable> monthRegister;


        public WorkBookReportDataModel(IEnumerable<MonthRegisterTable> monthRegister)
        {
            this.monthRegister = monthRegister;
        }


        public IEnumerable<MonthRegisterTable> MonthRegisters { get { return monthRegister; } }

        public int MonthRegisterCount { get { return monthRegister.Count(); } }
    }
}
