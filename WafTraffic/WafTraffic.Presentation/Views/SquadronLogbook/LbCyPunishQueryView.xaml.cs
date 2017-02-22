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
    /// LbCyPunishQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ILbCyPunishQueryView))]
    public partial class LbCyPunishQueryView  : UserControl, ILbCyPunishQueryView
    {
        private readonly Lazy<LbCyPunishQueryViewModel> viewModel;
    
        public LbCyPunishQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbCyPunishQueryViewModel>(() => ViewHelper.GetViewModel<LbCyPunishQueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            this.dpStartDate.Text = DateTime.Today.ToLongDateString();
            this.dpEndDate.Text = DateTime.Today.ToLongDateString();

            this.gridPunishList.Columns[5].Visibility = viewModel.Value.BrowsePermissionVisibility;
            this.gridPunishList.Columns[6].Visibility = viewModel.Value.ModifyPermissionVisibility;
            this.gridPunishList.Columns[7].Visibility = viewModel.Value.DeletePermissionVisibility;
        }

        private void gridPunishList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridPunishList.Total)
            {
                startIndex = (gridPunishList.Total / args.PageSize) * args.PageSize;
                gridPunishList.PageIndex = (gridPunishList.Total % args.PageSize) == 0 ? (gridPunishList.Total / args.PageSize) : (gridPunishList.Total / args.PageSize) + 1;
                args.PageIndex = gridPunishList.PageIndex;
            }

            IQueryable<ZdtzCyPunish> gridTables = viewModel.Value.LbPunishs;

            gridPunishList.Total = gridTables.Count();
            gridPunishList.ItemsSource = gridTables.OrderByDescending(p=>p.PatrolDate).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
        }

        public void PagingReload()
        {
            gridPunishList.RaisePageChanged();
        }
    }
}
    