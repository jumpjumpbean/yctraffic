using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Reporting.Applications.Views;

namespace WafTraffic.Reporting.Applications.ViewModels
{
    [Export]
    public class ReportViewModel : ViewModel<IReportView>
    {
        private object report;
        private ICommand createWorkBookReportCommand;


        [ImportingConstructor]
        public ReportViewModel(IReportView view)
            : base(view)
        {
        }


        public object Report
        {
            get { return report; }
            set
            {
                if (report != value)
                {
                    report = value;
                    RaisePropertyChanged("Report");
                }
            }
        }

        public ICommand CreateWorkBookReportCommand
        {
            get { return createWorkBookReportCommand; }
            set
            {
                if (createWorkBookReportCommand != value)
                {
                    createWorkBookReportCommand = value;
                    RaisePropertyChanged("CreateWorkBookReportCommand");
                }
            }
        }
       
    }
}

