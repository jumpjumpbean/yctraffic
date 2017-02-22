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

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// HotLineListView.xaml 的交互逻辑
    /// </summary>
     [Export(typeof(IHotLineListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class HotLineListView  : UserControl, IHotLineListView
    {
         private readonly Lazy<HotLineListViewModel> viewModel;

        public HotLineListView()
        {
            InitializeComponent();
            viewModel = new Lazy<HotLineListViewModel>(() => ViewHelper.GetViewModel<HotLineListViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            Loaded -= FirstTimeLoadedHandler;
            if (!CurrentLoginService.Instance.IsAuthorized("yctraffic.hotline.Browse"))
            {
                gridHotLineList.Columns[5].Visibility = System.Windows.Visibility.Hidden;
            }
            if (!CurrentLoginService.Instance.IsAuthorized("yctraffic.hotline.Modify"))
            {
                gridHotLineList.Columns[6].Visibility = System.Windows.Visibility.Hidden;
            }
            if (!CurrentLoginService.Instance.IsAuthorized("yctraffic.hotline.Delete"))
            {
                gridHotLineList.Columns[7].Visibility = System.Windows.Visibility.Hidden;
            }
            if (!CurrentLoginService.Instance.IsAuthorized("yctraffic.hotline.Deal"))
            {
                gridHotLineList.Columns[8].Visibility = System.Windows.Visibility.Hidden;
            }
            if (!CurrentLoginService.Instance.IsAuthorized("yctraffic.hotline.Check"))
            {
                gridHotLineList.Columns[9].Visibility = System.Windows.Visibility.Hidden;
            }

            if (!CurrentLoginService.Instance.IsAuthorized("yctraffic.hotline.Add"))
            {
                btnAdd.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void gridHotLineList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridHotLineList.Total)
            {
                startIndex = (gridHotLineList.Total / args.PageSize) * args.PageSize;
                gridHotLineList.PageIndex = (gridHotLineList.Total % args.PageSize) == 0 ? (gridHotLineList.Total / args.PageSize) : (gridHotLineList.Total / args.PageSize) + 1;
                args.PageIndex = gridHotLineList.PageIndex;
            }

            IQueryable<MayorHotlineTaskTable> gridTables = viewModel.Value.HotLineTasks;

            gridHotLineList.Total = gridTables.Count();

            gridHotLineList.ItemsSource = gridTables.OrderByDescending(p => p.CreateDate).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
        }

        public void PagingReload()
        {
            gridHotLineList.PageIndex = 1;
            gridHotLineList.RaisePageChanged();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            //gridHotLineList.PageIndex = 1;
            //gridHotLineList.RaisePageChanged();
        }
                 
        //private void gridHotLineList_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    e.Row.
        //    //e.Row.Header = e.Row.GetIndex() + 1; 
        //    //gridHotLineList.Columns[0].GetCellContent(e.Row).Visibility = System.Windows.Visibility.Hidden;

        //}

        //private void gridHotLineList_Loaded(object sender, RoutedEventArgs e)
        //{
        //   ((DataGrid) e.Source).Items[0].
        //}

    }
}
