using System;
using System.ComponentModel;

namespace WafTraffic.Applications.Services
{
    public interface IShellService : INotifyPropertyChanged
    {
        object ShellView { get; }

        string DocumentName { get; set; }


        bool IsReportingEnabled { get; set; }

        Lazy<object> LazyReportingView { get; set; }
    }
}
