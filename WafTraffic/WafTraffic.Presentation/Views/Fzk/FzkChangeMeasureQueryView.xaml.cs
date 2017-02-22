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
    /// FzkChangeMeasureQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IFzkChangeMeasureQueryView))]
    public partial class FzkChangeMeasureQueryView  : UserControl, IFzkChangeMeasureQueryView
    {
        private readonly Lazy<FzkChangeMeasureQueryViewModel> viewModel;
    
        public FzkChangeMeasureQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<FzkChangeMeasureQueryViewModel>(() => ViewHelper.GetViewModel<FzkChangeMeasureQueryViewModel>(this));
        }

        private void gridPublicityLogbookList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            try
            {
                int startIndex = (args.PageIndex - 1) * args.PageSize;
                if (startIndex > gridPublicityLogbookList.Total)
                {
                    startIndex = (gridPublicityLogbookList.Total / args.PageSize) * args.PageSize;
                    gridPublicityLogbookList.PageIndex = (gridPublicityLogbookList.Total % args.PageSize) == 0 ? (gridPublicityLogbookList.Total / args.PageSize) : (gridPublicityLogbookList.Total / args.PageSize) + 1;
                    args.PageIndex = gridPublicityLogbookList.PageIndex;
                }

                IQueryable<FzkChangeMeasure> gridTables = viewModel.Value.FzkChangeMeasures;

                gridPublicityLogbookList.Total = gridTables.Count();
                gridPublicityLogbookList.ItemsSource = gridTables.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
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
                gridPublicityLogbookList.RaisePageChanged();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }   
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_VehicleNo.Text))
            {
                viewModel.Value.KeyWord_Vehicle = tb_VehicleNo.Text;
            }
            else
            {
                viewModel.Value.KeyWord_Vehicle = null;
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

            if (!string.IsNullOrEmpty(tb_Name.Text))
            {
                viewModel.Value.KeyWord_Name = tb_Name.Text;
            }
            else
            {
                viewModel.Value.KeyWord_Name = null;
            }
        }
    }
}
    