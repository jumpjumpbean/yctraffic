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
    /// ZhzxFilterQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxFilterQueryView))]
    public partial class ZhzxFilterQueryView  : UserControl, IZhzxFilterQueryView
    {
        private readonly Lazy<ZhzxFilterQueryViewModel> viewModel;

        public ZhzxFilterQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxFilterQueryViewModel>(() => ViewHelper.GetViewModel<ZhzxFilterQueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {

        }

        private void gridFilterList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridFilterList.Total)
            {
                startIndex = (gridFilterList.Total / args.PageSize) * args.PageSize;
                gridFilterList.PageIndex = (gridFilterList.Total % args.PageSize) == 0 ? (gridFilterList.Total / args.PageSize) : (gridFilterList.Total / args.PageSize) + 1;
                args.PageIndex = gridFilterList.PageIndex;
            }

            IQueryable<ZhzxRedNameList> gridTables = viewModel.Value.Filters;

            gridFilterList.Total = gridTables.Count();

            var itemsSource = gridTables.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
            viewModel.Value.StoreWins_Refresh(itemsSource);
            gridFilterList.ItemsSource = itemsSource;
        }

        public void PagingReload()
        {
            gridFilterList.RaisePageChanged();
        }
    }
}
    