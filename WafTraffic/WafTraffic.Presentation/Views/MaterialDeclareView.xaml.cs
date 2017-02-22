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
    /// MaterialDeclareView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IMaterialDeclareView))]
    public partial class MaterialDeclareView  : UserControl, IMaterialDeclareView
    {

        private readonly Lazy<MaterialDeclareViewModel> viewModel;
        public MaterialDeclareView()
        {
            InitializeComponent();
            viewModel = new Lazy<MaterialDeclareViewModel>(() => ViewHelper.GetViewModel<MaterialDeclareViewModel>(this));
        }

        private void gridMaterialDeclareList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridMaterialDeclareList.Total)
            {
                startIndex = (gridMaterialDeclareList.Total / args.PageSize) * args.PageSize;
                gridMaterialDeclareList.PageIndex = (gridMaterialDeclareList.Total % args.PageSize) == 0 ? (gridMaterialDeclareList.Total / args.PageSize) : (gridMaterialDeclareList.Total / args.PageSize) + 1;
                args.PageIndex = gridMaterialDeclareList.PageIndex;
            }

            IQueryable<MaterialDeclareTable> gridSource = viewModel.Value.MaterialDeclare;

            gridMaterialDeclareList.Total = gridSource.Count();
            gridMaterialDeclareList.ItemsSource = gridSource.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize); ;

        }

        public void PagingReload()
        {
            gridMaterialDeclareList.PageIndex = 1;
            gridMaterialDeclareList.RaisePageChanged();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (cbxDepartment.SelectedValue != null)
            {
                viewModel.Value.SelectDepartCode = cbxDepartment.SelectedValue.ToString();
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
    