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
using System.Data;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// ZhzxOfficeSupplyStockListView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxOfficeSupplyStockListView))]
    public partial class ZhzxOfficeSupplyStockListView : UserControl, IZhzxOfficeSupplyStockListView
    {

        private readonly Lazy<ZhzxOfficeSupplyStockListViewModel> viewModel;
        public ZhzxOfficeSupplyStockListView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxOfficeSupplyStockListViewModel>(() => ViewHelper.GetViewModel<ZhzxOfficeSupplyStockListViewModel>(this));
        }

        private void gridZhzxOfficeSupplyStockList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridZhzxOfficeSupplyStockList.Total)
            {
                startIndex = (gridZhzxOfficeSupplyStockList.Total / args.PageSize) * args.PageSize;
                gridZhzxOfficeSupplyStockList.PageIndex = (gridZhzxOfficeSupplyStockList.Total % args.PageSize) == 0 ? (gridZhzxOfficeSupplyStockList.Total / args.PageSize) : (gridZhzxOfficeSupplyStockList.Total / args.PageSize) + 1;
                args.PageIndex = gridZhzxOfficeSupplyStockList.PageIndex;
            }

            IQueryable<ZhzxOfficeSupplyStock> gridSource = viewModel.Value.ZhzxOfficeSupplyStock;

            gridZhzxOfficeSupplyStockList.Total = gridSource.Count();
            gridZhzxOfficeSupplyStockList.ItemsSource = gridSource.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize); ;

        }

        public void PagingReload()
        {
            gridZhzxOfficeSupplyStockList.PageIndex = 1;
            gridZhzxOfficeSupplyStockList.RaisePageChanged();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (tbStartDate.Value == null)
            {
                viewModel.Value.StartDate = DateTime.Parse("1990-01-01");
            }
            else
            {
                viewModel.Value.StartDate = DateTime.Parse(Convert.ToDateTime(tbStartDate.Value).ToShortDateString());  // a bit ugly, conver 2014-11-19 15:51:59 to 2014-11-19 00:00:00
            }

            if (tbEndDate.Value == null)
            {
                viewModel.Value.EndDate = DateTime.Parse("2199-12-31");
            }
            else
            {
                viewModel.Value.EndDate = DateTime.Parse(Convert.ToDateTime(tbEndDate.Value).AddDays(1).ToShortDateString());
            }

            if (!string.IsNullOrEmpty(tb_ItemName.Text))
            {
                viewModel.Value.KeyWord = tb_ItemName.Text;
            }
            else
            {
                viewModel.Value.KeyWord = null;
            }
        }

    }
}
    