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

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// LbStaticLogbookDetailsView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ILbStaticLogbookDetailsView))]
    public partial class LbStaticLogbookDetailsView : UserControl, ILbStaticLogbookDetailsView
    {
        private readonly Lazy<LbStaticLogbookDetailsViewModel> viewModel;

        public LbStaticLogbookDetailsView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbStaticLogbookDetailsViewModel>(() => ViewHelper.GetViewModel<LbStaticLogbookDetailsViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            //this.tbUploadFile.Text = string.Empty;
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
    