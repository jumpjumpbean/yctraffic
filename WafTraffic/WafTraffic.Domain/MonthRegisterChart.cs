using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace WafTraffic.Domain
{
    public class MonthRegisterChart
    {
        public string Category { get; set; }

        public int Number { get; set; }
    }

    public class ChartSeries
    {
        public string UserName { get; set; }

        public ObservableCollection<MonthRegisterChart> ChartItems  { get; set; }
    }

    public class ChartReport
    {
        public string UserName { get; set; }

        public string Category { get; set; }

        public int Number { get; set; }
    }
}
