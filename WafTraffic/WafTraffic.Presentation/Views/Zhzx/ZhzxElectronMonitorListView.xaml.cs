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
    /// ZhzxElectronMonitorListView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxElectronMonitorListView))]
    public partial class ZhzxElectronMonitorListView : UserControl, IZhzxElectronMonitorListView
    {

        private readonly Lazy<ZhzxElectronMonitorListViewModel> viewModel;
        public ZhzxElectronMonitorListView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxElectronMonitorListViewModel>(() => ViewHelper.GetViewModel<ZhzxElectronMonitorListViewModel>(this));
        }

        private void gridZhzxElectronMonitorList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridZhzxElectronMonitorList.Total)
            {
                startIndex = (gridZhzxElectronMonitorList.Total / args.PageSize) * args.PageSize;
                gridZhzxElectronMonitorList.PageIndex = (gridZhzxElectronMonitorList.Total % args.PageSize) == 0 ? (gridZhzxElectronMonitorList.Total / args.PageSize) : (gridZhzxElectronMonitorList.Total / args.PageSize) + 1;
                args.PageIndex = gridZhzxElectronMonitorList.PageIndex;
            }

            IQueryable<ZhzxElectronMonitor> gridSource = viewModel.Value.ZhzxElectronMonitor;

            gridZhzxElectronMonitorList.Total = gridSource.Count();
            gridZhzxElectronMonitorList.ItemsSource = gridSource.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize); ;

        }

        public void PagingReload()
        {
            gridZhzxElectronMonitorList.PageIndex = 1;
            gridZhzxElectronMonitorList.RaisePageChanged();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_CheckpointName.Text))
            {
                viewModel.Value.CheckpointKeyWord = tb_CheckpointName.Text;
            }
            else
            {
                viewModel.Value.CheckpointKeyWord = null;
            }

            if (!string.IsNullOrEmpty(tb_Status.Text))
            {
                viewModel.Value.StatusKeyWord = tb_Status.Text;
            }
            else
            {
                viewModel.Value.StatusKeyWord = null;
            }
        }

    }
}
    