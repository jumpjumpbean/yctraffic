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
    /// ZhzxFilterUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxFilterUpdateView))]
    public partial class ZhzxFilterUpdateView : UserControl, IZhzxFilterUpdateView
    {
        private readonly Lazy<ZhzxFilterUpdateViewModel> viewModel;

        public ZhzxFilterUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxFilterUpdateViewModel>(() => ViewHelper.GetViewModel<ZhzxFilterUpdateViewModel>(this));

            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            this.btnBack.Visibility = viewModel.Value.BrowseVisibility;
            this.btnSave.Visibility = viewModel.Value.NewOrModifyVisibility;
            this.btnCancel.Visibility = viewModel.Value.NewOrModifyVisibility;
        }
    }
}
    