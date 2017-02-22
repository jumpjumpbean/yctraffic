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
    /// ZgxcPersonnelChangeUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZgxcPersonnelChangeUpdateView))]
    public partial class ZgxcPersonnelChangeUpdateView  : UserControl, IZgxcPersonnelChangeUpdateView
    {
        private readonly Lazy<ZgxcPersonnelChangeUpdateViewModel> viewModel;

        public ZgxcPersonnelChangeUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZgxcPersonnelChangeUpdateViewModel>(() => ViewHelper.GetViewModel<ZgxcPersonnelChangeUpdateViewModel>(this));

        }



    }
}
    