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

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// MonthRegisterListView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IMonthRegisterListView))]
    public partial class MonthRegisterListView : UserControl, IMonthRegisterListView
    {
        private readonly Lazy<MonthRegisterListViewModel> viewModel;
        public MonthRegisterListView()
        {
            InitializeComponent();
            viewModel = new Lazy<MonthRegisterListViewModel>(() => ViewHelper.GetViewModel<MonthRegisterListViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            if (!CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.Approve"))
            {
                gridMonthRegisterList.Columns[1].Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                gridMonthRegisterList.Columns[1].Visibility = System.Windows.Visibility.Visible;
            }
        }


        private void gridMonthRegisterList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridMonthRegisterList.Total)
            {
                startIndex = (gridMonthRegisterList.Total / args.PageSize) * args.PageSize;
                gridMonthRegisterList.PageIndex = (gridMonthRegisterList.Total % args.PageSize) == 0 ? (gridMonthRegisterList.Total / args.PageSize) : (gridMonthRegisterList.Total / args.PageSize) + 1;
                args.PageIndex = gridMonthRegisterList.PageIndex;
            }

            IQueryable<MonthRegisterTable> gridSource = viewModel.Value.MonthRegisters;

            gridMonthRegisterList.Total = gridSource.Count();

            gridMonthRegisterList.ItemsSource = gridSource.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);  
        }

        public void PagingReload()
        {
            gridMonthRegisterList.PageIndex = 1;
            gridMonthRegisterList.RaisePageChanged();
        }

        //private void btnQuery_Click(object sender, RoutedEventArgs e)
        //{
        //    gridMonthRegisterList.PageIndex = 1;
        //    gridMonthRegisterList.RaisePageChanged();

        //    //if (cbxDepartment.SelectedValue != null)
        //    //{
        //    //    viewModel.Value.SelectDepartCode = cbxDepartment.SelectedValue.ToString();
        //    //}
        //    //viewModel.Value.SelectYear = Convert.ToInt32(tbSelectYear.Value);
        //}
    }
}
    
