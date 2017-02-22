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
    /// CgsYellowMarkCarUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ICgsYellowMarkCarUpdateView))]
    public partial class CgsYellowMarkCarUpdateView  : UserControl, ICgsYellowMarkCarUpdateView
    {
        private readonly Lazy<CgsYellowMarkCarUpdateViewModel> viewModel;

        public CgsYellowMarkCarUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<CgsYellowMarkCarUpdateViewModel>(() => ViewHelper.GetViewModel<CgsYellowMarkCarUpdateViewModel>(this));

        }



    }
}
    