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
    /// ZhzxTotalViolationDetailView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxTotalViolationDetailView))]
    public partial class ZhzxTotalViolationDetailView : UserControl, IZhzxTotalViolationDetailView
    {
        private readonly Lazy<ZhzxTotalViolationDetailViewModel> viewModel;

        public ZhzxTotalViolationDetailView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxTotalViolationDetailViewModel>(() => ViewHelper.GetViewModel<ZhzxTotalViolationDetailViewModel>(this));

        }
    }
}
