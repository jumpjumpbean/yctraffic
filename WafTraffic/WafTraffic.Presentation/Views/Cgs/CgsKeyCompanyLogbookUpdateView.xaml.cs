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
    /// CgsKeyCompanyLogbookUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ICgsKeyCompanyLogbookUpdateView))]
    public partial class CgsKeyCompanyLogbookUpdateView  : UserControl, ICgsKeyCompanyLogbookUpdateView
    {
        private readonly Lazy<CgsKeyCompanyLogbookUpdateViewModel> viewModel;

        public CgsKeyCompanyLogbookUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<CgsKeyCompanyLogbookUpdateViewModel>(() => ViewHelper.GetViewModel<CgsKeyCompanyLogbookUpdateViewModel>(this));

        }



    }
}
    