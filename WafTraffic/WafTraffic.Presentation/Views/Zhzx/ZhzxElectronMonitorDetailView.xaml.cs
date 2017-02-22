using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Waf;
using WafTraffic.Domain;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// ZhzxElectronMonitorDetailView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxElectronMonitorDetailView))]
    public partial class ZhzxElectronMonitorDetailView : UserControl, IZhzxElectronMonitorDetailView
    {
        private readonly Lazy<ZhzxElectronMonitorDetailViewModel> viewModel;

        public ZhzxElectronMonitorDetailView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxElectronMonitorDetailViewModel>(() => ViewHelper.GetViewModel<ZhzxElectronMonitorDetailViewModel>(this));

        }
    }
}
