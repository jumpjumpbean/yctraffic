using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WafTraffic.Reporting.Applications.Reports;
using System.ComponentModel.Composition;

namespace WafTraffic.Reporting.Presentation.Reports
{
    /// <summary>
    /// WorkBookReport.xaml 的交互逻辑
    /// </summary>

    [Export(typeof(IWorkBookReport)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class WorkBookReport : UserControl, IWorkBookReport
    {
        private object reportData;
        
        public WorkBookReport()
        {
            InitializeComponent();

            // Disconnect the flowDocument from the UserControl. The UserControl is only used for DesignTime support.
            Content = null;
        }

        public object Report { get { return flowDocument; } }

        public object ReportData
        {
            get { return reportData; }
            set
            {
                reportData = value;
                flowDocument.DataContext = value;
            }
        }
    }
}
