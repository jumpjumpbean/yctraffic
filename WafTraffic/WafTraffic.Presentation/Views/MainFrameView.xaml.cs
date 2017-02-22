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
using DotNet.Utilities;
using WafTraffic.Presentation.Services;
using DotNet.Business;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// MainFrameView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IMainFrameView))]
    public partial class MainFrameView : UserControl, IMainFrameView
    {
        private readonly Lazy<MainFrameViewModel> viewModel;
        public MainFrameView()
        {
            InitializeComponent();
            viewModel = new Lazy<MainFrameViewModel>(() => ViewHelper.GetViewModel<MainFrameViewModel>(this));
        }

        private MainFrameViewModel ViewModel { get { return viewModel.Value; } }

        public void FocusFirstItem()
        {
            lbModules.Focus();
            if (lbModules.Items.Count > 0)
            {
                lbModules.SelectedItem = lbModules.Items[0];
            }

        }

    }
}
