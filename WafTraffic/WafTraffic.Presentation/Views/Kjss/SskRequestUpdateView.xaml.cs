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
    /// ZhzxRequestUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ISskRequestUpdateView))]
    public partial class SskRequestUpdateView : UserControl, ISskRequestUpdateView
    {
        private readonly Lazy<SskRequestUpdateViewModel> viewModel;

        public SskRequestUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<SskRequestUpdateViewModel>(() => ViewHelper.GetViewModel<SskRequestUpdateViewModel>(this));

            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            /*
            this.btnBack.Visibility = viewModel.Value.BrowseVisibility;
            this.btnSave.Visibility = viewModel.Value.NewOrModifyVisibility;
            this.btnCancel.Visibility = viewModel.Value.NewOrModifyVisibility;
            */
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
    }
}
    