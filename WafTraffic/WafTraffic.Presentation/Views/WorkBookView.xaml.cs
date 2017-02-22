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
using DotNet.Business;
using System.Data;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// WorkBookView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IWorkBookView))]
    public partial class WorkBookView  : UserControl, IWorkBookView
    {
        //private readonly Lazy<WorkBookViewModel> viewModel;
        public WorkBookView()
        {
            InitializeComponent();

            //viewModel = new Lazy<WorkBookViewModel>(() => ViewHelper.GetViewModel<WorkBookViewModel>(this));
            //Loaded += FirstTimeLoadedHandler;

            
        }

        //private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        //{
        //    DataTable dt = DotNetService.Instance.UserService.GetDTByDepartment(CurrentLoginService.Instance.CurrentUserInfo, "100000012", true); // "100000012" --政工科
        //    if (dt.Rows.Count > viewModel.Value.WorkContentCount)
        //    {

        //    }
        //}
    }
}
    