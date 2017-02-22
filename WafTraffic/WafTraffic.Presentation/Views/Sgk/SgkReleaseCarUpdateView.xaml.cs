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
using Microsoft.Win32;
using System.IO;
using DotNet.Business;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// SgkReleaseCarUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ISgkReleaseCarUpdateView))]
    public partial class SgkReleaseCarUpdateView  : UserControl, ISgkReleaseCarUpdateView
    {
        private readonly Lazy<SgkReleaseCarUpdateViewModel> viewModel;

        public SgkReleaseCarUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<SgkReleaseCarUpdateViewModel>(() => ViewHelper.GetViewModel<SgkReleaseCarUpdateViewModel>(this));

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

        public void Show_Loading()
        {
            this._loading.Visibility = System.Windows.Visibility.Visible;
        }

        public void Shutdown_Loading()
        {
            this._loading.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnSgkChargeSign_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.WhosSign = 1;
        }

        private void btnRescueChargeSign_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.WhosSign = 2;
        }

        private void btnFDDSign_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.WhosSign = 3;
        }
    }
}
    