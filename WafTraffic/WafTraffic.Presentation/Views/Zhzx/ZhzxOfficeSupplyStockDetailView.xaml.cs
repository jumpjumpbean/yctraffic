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
    /// ZhzxOfficeSupplyStockDetailView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxOfficeSupplyStockDetailView))]
    public partial class ZhzxOfficeSupplyStockDetailView : UserControl, IZhzxOfficeSupplyStockDetailView
    {
        private readonly Lazy<ZhzxOfficeSupplyStockDetailViewModel> viewModel;

        public ZhzxOfficeSupplyStockDetailView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxOfficeSupplyStockDetailViewModel>(() => ViewHelper.GetViewModel<ZhzxOfficeSupplyStockDetailViewModel>(this));

        }
    }
}
