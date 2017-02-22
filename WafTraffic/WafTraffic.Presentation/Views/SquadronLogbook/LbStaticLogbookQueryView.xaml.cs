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
    /// LbStaticLogbookQueryView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(ILbStaticLogbookQueryView))]
    public partial class LbStaticLogbookQueryView  : UserControl, ILbStaticLogbookQueryView
    {
        private readonly Lazy<LbStaticLogbookQueryViewModel> viewModel;
    
        public LbStaticLogbookQueryView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbStaticLogbookQueryViewModel>(() => ViewHelper.GetViewModel<LbStaticLogbookQueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dpStartDate.Text = DateTime.Today.ToLongDateString();
                this.dpEndDate.Text = DateTime.Today.ToLongDateString();

                this.gridStaticLogbookList.Columns[4].Visibility = viewModel.Value.BrowsePermissionVisibility;
                this.gridStaticLogbookList.Columns[5].Visibility = viewModel.Value.ModifyPermissionVisibility;
                this.gridStaticLogbookList.Columns[6].Visibility = viewModel.Value.DeletePermissionVisibility;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void gridStaticLogbookList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            try
            {
                int startIndex = (args.PageIndex - 1) * args.PageSize;
                if (startIndex > gridStaticLogbookList.Total)
                {
                    startIndex = (gridStaticLogbookList.Total / args.PageSize) * args.PageSize;
                    gridStaticLogbookList.PageIndex = (gridStaticLogbookList.Total % args.PageSize) == 0 ? (gridStaticLogbookList.Total / args.PageSize) : (gridStaticLogbookList.Total / args.PageSize) + 1;
                    args.PageIndex = gridStaticLogbookList.PageIndex;
                }

                IQueryable<ZdtzStaticTable> gridTables = viewModel.Value.LbStaticLogbooks;

                gridStaticLogbookList.Total = gridTables.Count();
                gridStaticLogbookList.ItemsSource = gridTables.OrderBy(p=>p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
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
                gridStaticLogbookList.RaisePageChanged();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }   
        }
    }
}
    