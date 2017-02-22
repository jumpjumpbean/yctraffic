using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Windows;
using WafTraffic.Domain;
using DotNet.Business;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// LogbookCreateView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ILogbookMainView))]
    public partial class LogbookMainView  : UserControl, ILogbookMainView
    {
        private readonly Lazy<LogbookMainViewModel> viewModel;

        public LogbookMainView()
        {
            InitializeComponent();
            viewModel = new Lazy<LogbookMainViewModel>(() => ViewHelper.GetViewModel<LogbookMainViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
           try
           {
               tvSquadronLogbook.ItemsSource = viewModel.Value.RootNodes; //绑定树
           }
           catch (System.Exception ex)
           {
               CurrentLoginService.Instance.LogException(ex);
           }
            
        }

        private void tvSquadronLogbook_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                viewModel.Value.SelectedLogbook = (ZdtzConfigTable)e.NewValue;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }
    }
}
