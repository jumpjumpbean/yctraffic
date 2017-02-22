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
    /// ZhzxRequestQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ISskRequestQueryView))]
    public partial class SskRequestQueryView : UserControl, ISskRequestQueryView
    {
        private readonly Lazy<SskRequestQueryViewModel> viewModel;

        public SskRequestQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<SskRequestQueryViewModel>(() => ViewHelper.GetViewModel<SskRequestQueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            this.dpStartDate.Text = DateTime.Today.ToLongDateString();
            this.dpEndDate.Text = DateTime.Today.ToLongDateString();
        }

        private void gridRequestList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridRequestList.Total)
            {
                startIndex = (gridRequestList.Total / args.PageSize) * args.PageSize;
                gridRequestList.PageIndex = (gridRequestList.Total % args.PageSize) == 0 ? (gridRequestList.Total / args.PageSize) : (gridRequestList.Total / args.PageSize) + 1;
                args.PageIndex = gridRequestList.PageIndex;
            }

            IQueryable<SskEquipmentRequest> gridTables = viewModel.Value.Requests;

            gridRequestList.Total = gridTables.Count();

            var itemsSource = gridTables.OrderByDescending(p => p.CreateTime).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
            //viewModel.Value.StoreWins_Refresh(itemsSource);
            gridRequestList.ItemsSource = itemsSource;
        }

        public void PagingReload()
        {
            gridRequestList.RaisePageChanged();
        }
    }
}
    