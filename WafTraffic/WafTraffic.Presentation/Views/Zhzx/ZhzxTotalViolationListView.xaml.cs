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
    /// ZhzxTotalViolationListView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxTotalViolationListView))]
    public partial class ZhzxTotalViolationListView : UserControl, IZhzxTotalViolationListView
    {

        private readonly Lazy<ZhzxTotalViolationListViewModel> viewModel;
        public ZhzxTotalViolationListView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxTotalViolationListViewModel>(() => ViewHelper.GetViewModel<ZhzxTotalViolationListViewModel>(this));
        }

        private void gridZhzxTotalViolationList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridZhzxTotalViolationList.Total)
            {
                startIndex = (gridZhzxTotalViolationList.Total / args.PageSize) * args.PageSize;
                gridZhzxTotalViolationList.PageIndex = (gridZhzxTotalViolationList.Total % args.PageSize) == 0 ? (gridZhzxTotalViolationList.Total / args.PageSize) : (gridZhzxTotalViolationList.Total / args.PageSize) + 1;
                args.PageIndex = gridZhzxTotalViolationList.PageIndex;
            }

            IQueryable<ZhzxTotalViolation> gridSource = viewModel.Value.ZhzxTotalViolation;

            gridZhzxTotalViolationList.Total = gridSource.Count();
            gridZhzxTotalViolationList.ItemsSource = gridSource.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize); ;

        }

        public void PagingReload()
        {
            gridZhzxTotalViolationList.PageIndex = 1;
            gridZhzxTotalViolationList.RaisePageChanged();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_LicensePlateNumber.Text))
            {
                viewModel.Value.PlateNumber = tb_LicensePlateNumber.Text;
            }
            else
            {
                viewModel.Value.PlateNumber = null;
            }


            if (!string.IsNullOrEmpty(tb_CheckpointName.Text))
            {
                viewModel.Value.Checkpoint = tb_CheckpointName.Text;
            }
            else
            {
                viewModel.Value.Checkpoint = null;
            }
        }

    }
}
    