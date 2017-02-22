using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Domain;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// LbCyPunishUpdateView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ILbCyPunishUpdateView))]
    public partial class LbCyPunishUpdateView  : UserControl, ILbCyPunishUpdateView
    {
        private readonly Lazy<LbCyPunishUpdateViewModel> viewModel;
        public LbCyPunishUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbCyPunishUpdateViewModel>(() => ViewHelper.GetViewModel<LbCyPunishUpdateViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
        }
        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            if (viewModel.Value.IsBrowse == true)
            {
                btnSave.Visibility = System.Windows.Visibility.Hidden;
                btnCancel.Visibility = System.Windows.Visibility.Hidden;

                btnBack.Visibility = System.Windows.Visibility.Visible;
                btnPrint.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                btnSave.Visibility = System.Windows.Visibility.Visible;
                btnCancel.Visibility = System.Windows.Visibility.Visible;

                btnBack.Visibility = System.Windows.Visibility.Hidden;
                btnPrint.Visibility = System.Windows.Visibility.Hidden;
            }
            this.dpPatrolDate.IsEnabled = viewModel.Value.IsNewOrModify;
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
    