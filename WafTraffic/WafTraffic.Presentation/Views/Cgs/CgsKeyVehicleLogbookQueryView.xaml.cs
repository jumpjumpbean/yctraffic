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
    /// CgsKeyVehicleLogbookQueryView.xaml �Ľ����߼�
    /// </summary>
    [Export(typeof(ICgsKeyVehicleLogbookQueryView))]
    public partial class CgsKeyVehicleLogbookQueryView : UserControl, ICgsKeyVehicleLogbookQueryView
    {
        private readonly Lazy<CgsKeyVehicleLogbookQueryViewModel> viewModel;
    
        public CgsKeyVehicleLogbookQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<CgsKeyVehicleLogbookQueryViewModel>(() => ViewHelper.GetViewModel<CgsKeyVehicleLogbookQueryViewModel>(this));
        }

        private void gridYellowMarkCarList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            try
            {
                int startIndex = (args.PageIndex - 1) * args.PageSize;
                if (startIndex > gridYellowMarkCarList.Total)
                {
                    startIndex = (gridYellowMarkCarList.Total / args.PageSize) * args.PageSize;
                    gridYellowMarkCarList.PageIndex = (gridYellowMarkCarList.Total % args.PageSize) == 0 ? (gridYellowMarkCarList.Total / args.PageSize) : (gridYellowMarkCarList.Total / args.PageSize) + 1;
                    args.PageIndex = gridYellowMarkCarList.PageIndex;
                }

                IQueryable<CgsKeyVehicleLogbook> gridTables = viewModel.Value.CgsKeyVehicleLogbooks;

                gridYellowMarkCarList.Total = gridTables.Count();
                gridYellowMarkCarList.ItemsSource = gridTables.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
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
                gridYellowMarkCarList.RaisePageChanged();
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
    