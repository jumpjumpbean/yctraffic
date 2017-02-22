using System;
using System.ComponentModel.Composition;
using System.Waf.Applications;

namespace WafTraffic.Applications.Services
{
    [Export(typeof(IShellService)), Export]
    internal class ShellService : DataModel, IShellService
    {
        private object shellView;
        private string documentName;

        private bool isReportingEnabled;
        private Lazy<object> lazyReportingView;


        public object ShellView
        {
            get { return shellView; }
            set
            {
                if (shellView != value)
                {
                    shellView = value;
                    RaisePropertyChanged("ShellView");
                }
            }
        }
        

        public string DocumentName
        {
            get { return documentName; }
            set
            {
                if (documentName != value)
                {
                    documentName = value;
                    RaisePropertyChanged("DocumentName");
                }
            }
        }

        public bool IsReportingEnabled
        {
            get { return isReportingEnabled; }
            set
            {
                if (isReportingEnabled != value)
                {
                    isReportingEnabled = value;
                    RaisePropertyChanged("IsReportingEnabled");
                }
            }
        }

        public Lazy<object> LazyReportingView
        {
            get { return lazyReportingView; }
            set
            {
                if (lazyReportingView != value)
                {
                    lazyReportingView = value;
                    RaisePropertyChanged("LazyReportingView");
                }
            }
        }
        
    }
}
