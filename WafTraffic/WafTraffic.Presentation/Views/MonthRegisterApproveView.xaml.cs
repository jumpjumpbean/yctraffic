using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Windows;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// MonthRegisterApplyView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IMonthRegisterApproveView))]
    public partial class MonthRegisterApproveView : UserControl, IMonthRegisterApproveView
    {
        private readonly Lazy<MonthRegisterApproveViewModel> viewModel;

        public MonthRegisterApproveView()
        {
            InitializeComponent();
            viewModel = new Lazy<MonthRegisterApproveViewModel>(() => ViewHelper.GetViewModel<MonthRegisterApproveViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            if (viewModel.Value.Operation == "Browse")
            {
                btnSave.Visibility = System.Windows.Visibility.Hidden;
                cbxApproveResult.IsEnabled = false;
            }
            else
            {
                btnSave.Visibility = System.Windows.Visibility.Visible;
                cbxApproveResult.IsEnabled = true;
            }
        }
    }

   
}
