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
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// HealthArchiveView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IMaterialDeclareBuildView))]
    public partial class MaterialDeclareBuildView : UserControl, IMaterialDeclareBuildView
    {
        private readonly Lazy<MaterialDeclareBuildViewModel> viewModel;

        public MaterialDeclareBuildView()
        {
            InitializeComponent();
            viewModel = new Lazy<MaterialDeclareBuildViewModel>(() => ViewHelper.GetViewModel<MaterialDeclareBuildViewModel>(this));

            //Loaded += FirstTimeLoadedHandler;
        }

    //    private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
    //    {
    //        Loaded -= FirstTimeLoadedHandler;
    //        if (!CurrentLoginService.Instance.IsAuthorized("yctraffic.MaterialDeclare.Score"))
    //        {
    //            tbScore.Visibility = System.Windows.Visibility.Visible;
    //            cbxScore.IsEnabled = false;
    //        }
    //    }
    }
}
