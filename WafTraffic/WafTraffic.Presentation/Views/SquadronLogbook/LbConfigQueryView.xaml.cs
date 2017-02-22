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
    /// LbConfigQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ILbConfigQueryView))]
    public partial class LbConfigQueryView : UserControl, ILbConfigQueryView
    {
        private readonly Lazy<LbConfigQueryViewModel> viewModel;

        public LbConfigQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbConfigQueryViewModel>(() => ViewHelper.GetViewModel<LbConfigQueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {

        }

        private void gridConfigList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridConfigList.Total)
            {
                startIndex = (gridConfigList.Total / args.PageSize) * args.PageSize;
                gridConfigList.PageIndex = (gridConfigList.Total % args.PageSize) == 0 ? (gridConfigList.Total / args.PageSize) : (gridConfigList.Total / args.PageSize) + 1;
                args.PageIndex = gridConfigList.PageIndex;
            }

            IQueryable<ZdtzConfigTable> gridTables = viewModel.Value.LbConfigs;

            gridConfigList.Total = gridTables.Count();
            gridConfigList.ItemsSource = gridTables.OrderBy(p => p.Code).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
        }

        public void PagingReload()
        {
            gridConfigList.RaisePageChanged();
        }
    }
}
    