using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Waf;
using WafTraffic.Domain;
using DotNet.Utilities;
using WafTraffic.Presentation.Services;
using DotNet.Business;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// FrequentUsedLindConfigView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IFrequentUsedLinkConfigView))]
    public partial class FrequentUsedLinkConfigView : UserControl, IFrequentUsedLinkConfigView
    {
        private readonly Lazy<FrequentUsedLinkConfigViewModel> viewModel;
        public FrequentUsedLinkConfigView()
        {
            InitializeComponent();
            viewModel = new Lazy<FrequentUsedLinkConfigViewModel>(() => ViewHelper.GetViewModel<FrequentUsedLinkConfigViewModel>(this));


        }







    }
}
    