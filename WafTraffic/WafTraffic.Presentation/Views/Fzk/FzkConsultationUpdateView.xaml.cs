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
    /// FzkConsultationUpdateView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IFzkConsultationUpdateView))]
    public partial class FzkConsultationUpdateView  : UserControl, IFzkConsultationUpdateView
    {
        private readonly Lazy<FzkConsultationUpdateViewModel> viewModel;

        public FzkConsultationUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<FzkConsultationUpdateViewModel>(() => ViewHelper.GetViewModel<FzkConsultationUpdateViewModel>(this));

        }



    }
}
    