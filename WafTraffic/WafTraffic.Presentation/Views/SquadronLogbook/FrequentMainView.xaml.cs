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

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// FrequentCreateView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IFrequentMainView))]
    public partial class FrequentMainView  : UserControl, IFrequentMainView
    {
        private readonly Lazy<FrequentMainViewModel> viewModel;

        public FrequentMainView()
        {
            InitializeComponent();
            viewModel = new Lazy<FrequentMainViewModel>(() => ViewHelper.GetViewModel<FrequentMainViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {

            tvSquadronFrequent.ItemsSource = viewModel.Value.RootNodes; //绑定树
        }

        private void tvSquadronFrequent_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            viewModel.Value.SelectedFrequent = (ZdtzConfigTable)e.NewValue;
        }
    }
}
