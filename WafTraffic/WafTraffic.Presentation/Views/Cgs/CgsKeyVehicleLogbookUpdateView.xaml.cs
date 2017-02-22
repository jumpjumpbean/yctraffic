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
    /// CgsKeyVehicleLogbookUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ICgsKeyVehicleLogbookUpdateView))]
    public partial class CgsKeyVehicleLogbookUpdateView  : UserControl, ICgsKeyVehicleLogbookUpdateView
    {
        private readonly Lazy<CgsKeyVehicleLogbookUpdateViewModel> viewModel;

        public CgsKeyVehicleLogbookUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<CgsKeyVehicleLogbookUpdateViewModel>(() => ViewHelper.GetViewModel<CgsKeyVehicleLogbookUpdateViewModel>(this));

        }



    }
}
    