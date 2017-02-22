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
    /// LbConfigUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ILbConfigUpdateView))]
    public partial class LbConfigUpdateView : UserControl, ILbConfigUpdateView
    {
        private readonly Lazy<LbConfigUpdateViewModel> viewModel;

        public LbConfigUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbConfigUpdateViewModel>(() => ViewHelper.GetViewModel<LbConfigUpdateViewModel>(this));

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
    