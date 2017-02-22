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

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// LbZdStaffQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ICgsVehicleAdminQueryView))]
    public partial class CgsVehicleAdminQueryView : UserControl, ICgsVehicleAdminQueryView
    {
        private readonly Lazy<CgsVehicleAdminQueryViewModel> viewModel;

        public CgsVehicleAdminQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<CgsVehicleAdminQueryViewModel>(() => ViewHelper.GetViewModel<CgsVehicleAdminQueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dpStartDate.Text = DateTime.Today.ToLongDateString();
                this.dpEndDate.Text = DateTime.Today.ToLongDateString();

                this.gridPunishCaseList.Columns[5].Visibility = viewModel.Value.BrowsePermissionVisibility;
                this.gridPunishCaseList.Columns[6].Visibility = viewModel.Value.ModifyPermissionVisibility;
                this.gridPunishCaseList.Columns[7].Visibility = viewModel.Value.DeletePermissionVisibility;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

        }

        private void gridPunishCaseList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            try
            {
                int startIndex = (args.PageIndex - 1) * args.PageSize;
                if (startIndex > gridPunishCaseList.Total)
                {
                    startIndex = (gridPunishCaseList.Total / args.PageSize) * args.PageSize;
                    gridPunishCaseList.PageIndex = (gridPunishCaseList.Total % args.PageSize) == 0 ? (gridPunishCaseList.Total / args.PageSize) : (gridPunishCaseList.Total / args.PageSize) + 1;
                    args.PageIndex = gridPunishCaseList.PageIndex;
                }

                IQueryable<CgsVehicleAdminCase> gridTables = viewModel.Value.PunishCases;

                gridPunishCaseList.Total = gridTables.Count();
                gridPunishCaseList.ItemsSource = gridTables.OrderByDescending(p => p.CaseTime).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

        }

        public void PagingReload()
        {
            try
            {
                gridPunishCaseList.RaisePageChanged();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }
    }
}
    