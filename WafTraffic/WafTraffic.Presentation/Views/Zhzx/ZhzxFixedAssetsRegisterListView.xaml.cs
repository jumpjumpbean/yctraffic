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
    /// ZhzxFixedAssetsRegisterListView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxFixedAssetsRegisterListView))]
    public partial class ZhzxFixedAssetsRegisterListView : UserControl, IZhzxFixedAssetsRegisterListView
    {

        private readonly Lazy<ZhzxFixedAssetsRegisterListViewModel> viewModel;
        public ZhzxFixedAssetsRegisterListView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxFixedAssetsRegisterListViewModel>(() => ViewHelper.GetViewModel<ZhzxFixedAssetsRegisterListViewModel>(this));
        }

        private void gridZhzxFixedAssetsRegisterList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridZhzxFixedAssetsRegisterList.Total)
            {
                startIndex = (gridZhzxFixedAssetsRegisterList.Total / args.PageSize) * args.PageSize;
                gridZhzxFixedAssetsRegisterList.PageIndex = (gridZhzxFixedAssetsRegisterList.Total % args.PageSize) == 0 ? (gridZhzxFixedAssetsRegisterList.Total / args.PageSize) : (gridZhzxFixedAssetsRegisterList.Total / args.PageSize) + 1;
                args.PageIndex = gridZhzxFixedAssetsRegisterList.PageIndex;
            }

            IQueryable<ZhzxFixedAssetsRegister> gridSource = viewModel.Value.ZhzxFixedAssetsRegister;

            gridZhzxFixedAssetsRegisterList.Total = gridSource.Count();
            gridZhzxFixedAssetsRegisterList.ItemsSource = gridSource.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize); ;

        }

        public void PagingReload()
        {
            gridZhzxFixedAssetsRegisterList.PageIndex = 1;
            gridZhzxFixedAssetsRegisterList.RaisePageChanged();
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
    