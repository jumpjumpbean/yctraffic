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
    /// LbCyPatrolQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ILbCyPatrolQueryView))]
    public partial class LbCyPatrolQueryView  : UserControl, ILbCyPatrolQueryView
    {
        private readonly Lazy<LbCyPatrolQueryViewModel> viewModel;
    
        public LbCyPatrolQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbCyPatrolQueryViewModel>(() => ViewHelper.GetViewModel<LbCyPatrolQueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            this.dpStartDate.Text = DateTime.Today.ToLongDateString();
            this.dpEndDate.Text = DateTime.Today.ToLongDateString();

            this.gridPatrolList.Columns[5].Visibility = viewModel.Value.BrowsePermissionVisibility;
            this.gridPatrolList.Columns[6].Visibility = viewModel.Value.ModifyPermissionVisibility;
            this.gridPatrolList.Columns[7].Visibility = viewModel.Value.DeletePermissionVisibility;
        }

        private void gridPatrolList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridPatrolList.Total)
            {
                startIndex = (gridPatrolList.Total / args.PageSize) * args.PageSize;
                gridPatrolList.PageIndex = (gridPatrolList.Total % args.PageSize) == 0 ? (gridPatrolList.Total / args.PageSize) : (gridPatrolList.Total / args.PageSize) + 1;
                args.PageIndex = gridPatrolList.PageIndex;
            }

            IQueryable<ZdtzCyPatrol> gridTables = viewModel.Value.LbPatrols;

            gridPatrolList.Total = gridTables.Count();
            gridPatrolList.ItemsSource = gridTables.OrderByDescending(p=>p.PatrolDate).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
        }

        public void PagingReload()
        {
            gridPatrolList.RaisePageChanged();
        }
    }
}
    