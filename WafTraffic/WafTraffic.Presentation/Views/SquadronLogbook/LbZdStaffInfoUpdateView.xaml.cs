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
    /// LbZdStaffInfoView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ILbZdStaffInfoUpdateView))]
    public partial class LbZdStaffInfoUpdateView  : UserControl, ILbZdStaffInfoUpdateView
    {
        private readonly Lazy<LbZdStaffInfoUpdateViewModel> viewModel;

        public LbZdStaffInfoUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbZdStaffInfoUpdateViewModel>(() => ViewHelper.GetViewModel<LbZdStaffInfoUpdateViewModel>(this));

            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            this.dpRecordTime.IsEnabled = viewModel.Value.IsNewOrModify;
            this.btnBack.Visibility = viewModel.Value.BrowseVisibility;
            this.btnPrint.Visibility = viewModel.Value.BrowseVisibility;
            this.btnSave.Visibility = viewModel.Value.NewOrModifyVisibility;
            this.btnCancel.Visibility = viewModel.Value.NewOrModifyVisibility;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();

            double top = printArea.Margin.Top;
            top += 60;                  //top + 60 , 以防打印顶头
            printArea.Margin = new Thickness(0, top, 0, 0);

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    dialog.PrintVisual(printArea, "中队台账");
                }
                catch (SystemException)
                {
                    MessageBox.Show("打印出错");
                }
            }

            top -= 60;
            printArea.Margin = new Thickness(0, top, 0, 0);
        }
    }
}
    