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
using Microsoft.Win32;
using System.IO;
using DotNet.Business;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// ZhzxFakePlateNumberView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZhzxFakePlateNumberView))]
    public partial class ZhzxFakePlateNumberView : UserControl, IZhzxFakePlateNumberView
    {
        private readonly Lazy<ZhzxFakePlateNumberViewModel> viewModel;

        public ZhzxFakePlateNumberView()
        {
            InitializeComponent();

            viewModel = new Lazy<ZhzxFakePlateNumberViewModel>(() => ViewHelper.GetViewModel<ZhzxFakePlateNumberViewModel>(this));
        }

        private void gridFakePlateNumberList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridFakePlateNumberList.Total)
            {
                startIndex = (gridFakePlateNumberList.Total / args.PageSize) * args.PageSize;
                gridFakePlateNumberList.PageIndex = (gridFakePlateNumberList.Total % args.PageSize) == 0 ? (gridFakePlateNumberList.Total / args.PageSize) : (gridFakePlateNumberList.Total / args.PageSize) + 1;
                args.PageIndex = gridFakePlateNumberList.PageIndex;
            }

            IQueryable<ZhzxTrafficViolation> gridSource = viewModel.Value.FakePlateNumber;

            gridFakePlateNumberList.Total = gridSource.Count();

            var itemsSource = gridSource.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
            gridFakePlateNumberList.ItemsSource = itemsSource;

        }

        public void PagingReload()
        {
            gridFakePlateNumberList.RaisePageChanged();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {

            if (tbStartTime.Value == null)
            {
                viewModel.Value.StartTime = DateTime.Parse("1990-01-01");
            }
            else
            {
                viewModel.Value.StartTime = Convert.ToDateTime(tbStartTime.Value);
            }

            if (tbEndTime.Value == null)
            {
                viewModel.Value.EndTime = DateTime.Parse("2199-12-31");
            }
            else
            {
                viewModel.Value.EndTime = Convert.ToDateTime(tbEndTime.Value);
            }
            viewModel.Value.Checkpoint = tbCheckpoint.Text;
            viewModel.Value.LicensePlate = tbLicensePlate.Text;
            viewModel.Value.SelectWorkflowStatusId = (int)cbxStatus.SelectedValue;
        }
    }
}
    