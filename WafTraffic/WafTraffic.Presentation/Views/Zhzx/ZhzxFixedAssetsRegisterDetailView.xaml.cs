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
    /// ZhzxFixedAssetsRegisterDetailView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxFixedAssetsRegisterDetailView))]
    public partial class ZhzxFixedAssetsRegisterDetailView : UserControl, IZhzxFixedAssetsRegisterDetailView
    {
        private readonly Lazy<ZhzxFixedAssetsRegisterDetailViewModel> viewModel;

        public ZhzxFixedAssetsRegisterDetailView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxFixedAssetsRegisterDetailViewModel>(() => ViewHelper.GetViewModel<ZhzxFixedAssetsRegisterDetailViewModel>(this));

        }
    }
}
