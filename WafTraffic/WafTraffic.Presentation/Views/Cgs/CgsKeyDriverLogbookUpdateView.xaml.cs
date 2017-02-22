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
    /// CgsKeyDriverLogbookUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ICgsKeyDriverLogbookUpdateView))]
    public partial class CgsKeyDriverLogbookUpdateView  : UserControl, ICgsKeyDriverLogbookUpdateView
    {
        private readonly Lazy<CgsKeyDriverLogbookUpdateViewModel> viewModel;

        public CgsKeyDriverLogbookUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<CgsKeyDriverLogbookUpdateViewModel>(() => ViewHelper.GetViewModel<CgsKeyDriverLogbookUpdateViewModel>(this));

        }



    }
}
    