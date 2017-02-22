using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using WafTraffic.Applications.Common;
using System.Windows;
using DotNet.Business;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// ZhzxRequestUpdateView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IKjkRequestUpdateView))]
    public partial class KjkRequestUpdateView : UserControl, IKjkRequestUpdateView
    {
        private readonly Lazy<KjkRequestUpdateViewModel> viewModel;

        public KjkRequestUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<KjkRequestUpdateViewModel>(() => ViewHelper.GetViewModel<KjkRequestUpdateViewModel>(this));

            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {

            try
            {
                viewModel.Value.ShowSignImgCommand.Execute(null);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
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
                    dialog.PrintVisual(printArea, "办公用品审批");
                }
                catch (SystemException)
                {
                    MessageBox.Show("打印出错");
                }
            }

            top -= 60;
            printArea.Margin = new Thickness(0, top, 0, 0);
        }

        public void Show_Loading()
        {
            this._loading.Visibility = System.Windows.Visibility.Visible;
        }

        public void Shutdown_Loading()
        {
            this._loading.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
    