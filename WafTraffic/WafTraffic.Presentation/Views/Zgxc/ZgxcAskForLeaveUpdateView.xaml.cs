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
    /// ZgxcAskForLeaveUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZgxcAskForLeaveUpdateView))]
    public partial class ZgxcAskForLeaveUpdateView  : UserControl, IZgxcAskForLeaveUpdateView
    {
        private readonly Lazy<ZgxcAskForLeaveUpdateViewModel> viewModel;

        public ZgxcAskForLeaveUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZgxcAskForLeaveUpdateViewModel>(() => ViewHelper.GetViewModel<ZgxcAskForLeaveUpdateViewModel>(this));

        }



    }
}
    