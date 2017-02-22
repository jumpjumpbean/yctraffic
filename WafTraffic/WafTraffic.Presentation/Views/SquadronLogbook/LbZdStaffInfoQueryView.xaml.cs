using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows;
using WafTraffic.Domain;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using DotNet.Business;
using System.Data;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// LbZdStaffInfoQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ILbZdStaffInfoQueryView))]
    public partial class LbZdStaffInfoQueryView  : UserControl, ILbZdStaffInfoQueryView
    {
        private readonly Lazy<LbZdStaffInfoQueryViewModel> viewModel;

        public LbZdStaffInfoQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbZdStaffInfoQueryViewModel>(() => ViewHelper.GetViewModel<LbZdStaffInfoQueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            this.dpStartDate.Text = DateTime.Today.ToLongDateString();
            this.dpEndDate.Text = DateTime.Today.ToLongDateString();

            this.gridStaffInfoList.Columns[6].Visibility = viewModel.Value.BrowsePermissionVisibility;
            this.gridStaffInfoList.Columns[7].Visibility = viewModel.Value.ModifyPermissionVisibility;
            this.gridStaffInfoList.Columns[8].Visibility = viewModel.Value.DeletePermissionVisibility;
        }

        private void gridStaffInfoList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridStaffInfoList.Total)
            {
                startIndex = (gridStaffInfoList.Total / args.PageSize) * args.PageSize;
                gridStaffInfoList.PageIndex = (gridStaffInfoList.Total % args.PageSize) == 0 ? (gridStaffInfoList.Total / args.PageSize) : (gridStaffInfoList.Total / args.PageSize) + 1;
                args.PageIndex = gridStaffInfoList.PageIndex;
            }

            IQueryable<ZdtzZdStaffInfo> gridTables = viewModel.Value.LbStaffinfos;

            gridStaffInfoList.Total = gridTables.Count();
            gridStaffInfoList.ItemsSource = gridTables.OrderBy(p=>p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
        }

        public void PagingReload()
        {
            gridStaffInfoList.RaisePageChanged();
        }
    }
}
    