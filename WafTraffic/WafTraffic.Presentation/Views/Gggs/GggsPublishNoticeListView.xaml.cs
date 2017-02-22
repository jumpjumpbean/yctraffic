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
    /// GggsPublishNoticeListView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IGggsPublishNoticeListView))]
    public partial class GggsPublishNoticeListView : UserControl, IGggsPublishNoticeListView
    {

        private readonly Lazy<GggsPublishNoticeListViewModel> viewModel;
        public GggsPublishNoticeListView()
        {
            InitializeComponent();
            viewModel = new Lazy<GggsPublishNoticeListViewModel>(() => ViewHelper.GetViewModel<GggsPublishNoticeListViewModel>(this));
        }

        private void gridGggsPublishNoticeList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridGggsPublishNoticeList.Total)
            {
                startIndex = (gridGggsPublishNoticeList.Total / args.PageSize) * args.PageSize;
                gridGggsPublishNoticeList.PageIndex = (gridGggsPublishNoticeList.Total % args.PageSize) == 0 ? (gridGggsPublishNoticeList.Total / args.PageSize) : (gridGggsPublishNoticeList.Total / args.PageSize) + 1;
                args.PageIndex = gridGggsPublishNoticeList.PageIndex;
            }

            IQueryable<GggsPublishNotice> gridSource = viewModel.Value.GggsPublishNotice;

            gridGggsPublishNoticeList.Total = gridSource.Count();
            gridGggsPublishNoticeList.ItemsSource = gridSource.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize); ;

        }

        public void PagingReload()
        {
            gridGggsPublishNoticeList.PageIndex = 1;
            gridGggsPublishNoticeList.RaisePageChanged();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            //if (cbxDepartment.SelectedValue != null)
            //{
            //    viewModel.Value.SelectDepartCode = cbxDepartment.SelectedValue.ToString();
            //}

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

            if (!string.IsNullOrEmpty(tb_KeyWord.Text))
            {
                viewModel.Value.KeyWord = tb_KeyWord.Text;
            }
            else
            {
                viewModel.Value.KeyWord = null;
            }
        }

    }
}
    