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

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// LbCyDangerDealQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ILbCyDangerDealQueryView))]
    public partial class LbCyDangerDealQueryView  : UserControl, ILbCyDangerDealQueryView
    {
        private readonly Lazy<LbCyDangerDealQueryViewModel> viewModel;
    
        public LbCyDangerDealQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbCyDangerDealQueryViewModel>(() => ViewHelper.GetViewModel<LbCyDangerDealQueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            this.dpStartDate.Text = DateTime.Today.ToLongDateString();
            this.dpEndDate.Text = DateTime.Today.ToLongDateString();
            this.gridDangerDealList.Columns[7].Visibility = viewModel.Value.BrowsePermissionVisibility;
            this.gridDangerDealList.Columns[8].Visibility = viewModel.Value.ModifyPermissionVisibility;
            this.gridDangerDealList.Columns[9].Visibility = viewModel.Value.DeletePermissionVisibility;
            this.gridDangerDealList.Columns[10].Visibility = viewModel.Value.DealPermissionVisibility;
            this.gridDangerDealList.Columns[11].Visibility = viewModel.Value.VerifyPermissionVisibility;
        }

        private void gridDangerDealList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridDangerDealList.Total)
            {
                startIndex = (gridDangerDealList.Total / args.PageSize) * args.PageSize;
                gridDangerDealList.PageIndex = (gridDangerDealList.Total % args.PageSize) == 0 ? (gridDangerDealList.Total / args.PageSize) : (gridDangerDealList.Total / args.PageSize) + 1;
                args.PageIndex = gridDangerDealList.PageIndex;
            }

            IQueryable<ZdtzCyDangerDeal> gridTables = viewModel.Value.LbDangerDeals;

            gridDangerDealList.Total = gridTables.Count();
            gridDangerDealList.ItemsSource = gridTables.OrderByDescending(p => p.HappenDate).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
        }

        public void PagingReload()
        {
            gridDangerDealList.RaisePageChanged();
        }
    }
}
    