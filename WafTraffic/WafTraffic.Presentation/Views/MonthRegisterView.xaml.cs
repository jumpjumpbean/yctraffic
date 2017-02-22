using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Windows;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// MonthRegisterView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IMonthRegisterView))]
    public partial class MonthRegisterView : UserControl, IMonthRegisterView
    {
        //private readonly Lazy<MonthRegisterViewModel> viewModel;

        public MonthRegisterView()
        {
            InitializeComponent();
            //viewModel = new Lazy<MonthRegisterViewModel>(() => ViewHelper.GetViewModel<MonthRegisterViewModel>(this));
            //Loaded += FirstTimeLoadedHandler;
        }

        //private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        //{
        //    if (viewModel.Value.Operation == "Browse")
        //    {
        //        btnSave.Visibility = System.Windows.Visibility.Hidden;
        //        btnSubmit.Visibility = System.Windows.Visibility.Hidden;
        //    }
        //    else if (viewModel.Value.MonthRegister == null && viewModel.Value.MonthRegister.Id == null && viewModel.Value.MonthRegister.Id <= 0)
        //    {
        //        btnSave.Visibility = System.Windows.Visibility.Visible;
        //        btnSubmit.Visibility = System.Windows.Visibility.Hidden;
        //    }
        //    else
        //    {
        //        btnSave.Visibility = System.Windows.Visibility.Visible;
        //        btnSubmit.Visibility = System.Windows.Visibility.Visible;
        //    }
        //}
    }
}
    