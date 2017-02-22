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
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;
using GMap.NET;
using WafTraffic.Presentation.MapSources;
using WafTraffic.Presentation.CustomMarkers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using WafTraffic.Domain.Common;
using System.Windows.Media;
using System.Windows.Input;
using DotNet.Business;
using System.Windows.Navigation;
using System.Diagnostics;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// YuChengMapView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IYuChengMapView))]
    public partial class YuChengMapView  : UserControl, IYuChengMapView
    {
        private readonly Lazy<YuChengMapViewModel> viewModel;
        public YuChengMapView()
        {
            InitializeComponent();
            viewModel = new Lazy<YuChengMapViewModel>(() => ViewHelper.GetViewModel<YuChengMapViewModel>(this));
        }


        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {

            Process.Start(e.Uri.OriginalString);

            e.Handled = true;
        }
    }
}
    