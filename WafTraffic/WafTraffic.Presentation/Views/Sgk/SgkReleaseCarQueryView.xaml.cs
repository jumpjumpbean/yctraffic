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
using DotNet.Business;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// SgkReleaseCarQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ISgkReleaseCarQueryView))]
    public partial class SgkReleaseCarQueryView : UserControl, ISgkReleaseCarQueryView
    {
        private readonly Lazy<SgkReleaseCarQueryViewModel> viewModel;
    
        public SgkReleaseCarQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<SgkReleaseCarQueryViewModel>(() => ViewHelper.GetViewModel<SgkReleaseCarQueryViewModel>(this));
        }

        private void gridAskForLeaveList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            try
            {
                int startIndex = (args.PageIndex - 1) * args.PageSize;
                if (startIndex > gridAskForLeaveList.Total)
                {
                    startIndex = (gridAskForLeaveList.Total / args.PageSize) * args.PageSize;
                    gridAskForLeaveList.PageIndex = (gridAskForLeaveList.Total % args.PageSize) == 0 ? (gridAskForLeaveList.Total / args.PageSize) : (gridAskForLeaveList.Total / args.PageSize) + 1;
                    args.PageIndex = gridAskForLeaveList.PageIndex;
                }

                IQueryable<SgkReleaseCar> gridTables = viewModel.Value.SgkReleaseCars;

                gridAskForLeaveList.Total = gridTables.Count();
                gridAskForLeaveList.ItemsSource = gridTables.OrderByDescending(p => p.CreateTime).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

        }

        public void PagingReload()
        {
            try
            {
                gridAskForLeaveList.RaisePageChanged();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }   
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
    