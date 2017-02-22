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
    /// HealthArchiveListView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IHealthArchiveListView))]
    public partial class HealthArchiveListView : UserControl, IHealthArchiveListView
    {
        private readonly Lazy<HealthArchiveListViewModel> viewModel;
        public HealthArchiveListView()
        {
            InitializeComponent();
            viewModel = new Lazy<HealthArchiveListViewModel>(() => ViewHelper.GetViewModel<HealthArchiveListViewModel>(this));

            if (CurrentLoginService.Instance.IsAuthorized("yctraffic.HealthArchive.ListAll")) //大队长
            {
                tbDepartment.Visibility = System.Windows.Visibility.Visible;
                cbxDepartment.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                tbDepartment.Visibility = System.Windows.Visibility.Collapsed;
                cbxDepartment.Visibility = System.Windows.Visibility.Collapsed;
            }
        }


        private void gridHealthArchiveList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridHealthArchiveList.Total)
            {
                startIndex = (gridHealthArchiveList.Total / args.PageSize) * args.PageSize;
                gridHealthArchiveList.PageIndex = (gridHealthArchiveList.Total % args.PageSize) == 0 ? (gridHealthArchiveList.Total / args.PageSize) : (gridHealthArchiveList.Total / args.PageSize) + 1;
                args.PageIndex = gridHealthArchiveList.PageIndex;
            }

            IQueryable<HealthArchiveTable> gridSource = viewModel.Value.HealthArchives;

            gridHealthArchiveList.Total = gridSource.Count();

            gridHealthArchiveList.ItemsSource = gridSource.OrderByDescending(p => p.CheckTime).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);           
        }

        public void PagingReload()
        {
            gridHealthArchiveList.PageIndex = 1;
            gridHealthArchiveList.RaisePageChanged();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (cbxDepartment.SelectedValue != null)
            {
                viewModel.Value.SelectDepartCode = cbxDepartment.SelectedValue.ToString();
            }
            viewModel.Value.SelectYear = Convert.ToInt32(tbSelectYear.Value);
        }
    }
}
    