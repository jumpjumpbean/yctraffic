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
    /// ZgxcAssistantCheckinQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IZgxcAssistantCheckinQueryView))]
    public partial class ZgxcAssistantCheckinQueryView  : UserControl, IZgxcAssistantCheckinQueryView
    {
        private readonly Lazy<ZgxcAssistantCheckinQueryViewModel> viewModel;
    
        public ZgxcAssistantCheckinQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZgxcAssistantCheckinQueryViewModel>(() => ViewHelper.GetViewModel<ZgxcAssistantCheckinQueryViewModel>(this));
        }

        private void gridAssistantCheckinList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            try
            {
                int startIndex = (args.PageIndex - 1) * args.PageSize;
                if (startIndex > gridAssistantCheckinList.Total)
                {
                    startIndex = (gridAssistantCheckinList.Total / args.PageSize) * args.PageSize;
                    gridAssistantCheckinList.PageIndex = (gridAssistantCheckinList.Total % args.PageSize) == 0 ? (gridAssistantCheckinList.Total / args.PageSize) : (gridAssistantCheckinList.Total / args.PageSize) + 1;
                    args.PageIndex = gridAssistantCheckinList.PageIndex;
                }

                IQueryable<ZgxcAssistantCheckin> gridTables = viewModel.Value.ZgxcAssistantCheckins;

                gridAssistantCheckinList.Total = gridTables.Count();
                gridAssistantCheckinList.ItemsSource = gridTables.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
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
                gridAssistantCheckinList.RaisePageChanged();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }   
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (cbxDepartment.SelectedValue != null)
            {
                viewModel.Value.SelectDepartId = Convert.ToInt32(cbxDepartment.SelectedValue);
            }

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
    