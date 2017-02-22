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
    [Export(typeof(IFzkPetitionQueryView))]
    public partial class FzkPetitionQueryView : UserControl, IFzkPetitionQueryView
    {
        private readonly Lazy<FzkPetitionQueryViewModel> viewModel;

        public FzkPetitionQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<FzkPetitionQueryViewModel>(() => ViewHelper.GetViewModel<FzkPetitionQueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dpStartDate.Text = DateTime.Today.ToLongDateString();
                this.dpEndDate.Text = DateTime.Today.ToLongDateString();

                this.gridPetitionCaseList.Columns[5].Visibility = viewModel.Value.BrowsePermissionVisibility;
                this.gridPetitionCaseList.Columns[6].Visibility = viewModel.Value.ModifyPermissionVisibility;
                this.gridPetitionCaseList.Columns[7].Visibility = viewModel.Value.DeletePermissionVisibility;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

        }

        private void gridPetitionCaseList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            try
            {
                int startIndex = (args.PageIndex - 1) * args.PageSize;
                if (startIndex > gridPetitionCaseList.Total)
                {
                    startIndex = (gridPetitionCaseList.Total / args.PageSize) * args.PageSize;
                    gridPetitionCaseList.PageIndex = (gridPetitionCaseList.Total % args.PageSize) == 0 ? (gridPetitionCaseList.Total / args.PageSize) : (gridPetitionCaseList.Total / args.PageSize) + 1;
                    args.PageIndex = gridPetitionCaseList.PageIndex;
                }

                IQueryable<FzkPetition> gridTables = viewModel.Value.PetitionCases;

                gridPetitionCaseList.Total = gridTables.Count();
                gridPetitionCaseList.ItemsSource = gridTables.OrderByDescending(p => p.PetitionTime).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
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
                gridPetitionCaseList.RaisePageChanged();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }
    }
}
    