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
    /// ZhzxTrafficViolationView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IZhzxTrafficViolationView))]
    public partial class ZhzxTrafficViolationView  : UserControl, IZhzxTrafficViolationView
    {
        private readonly Lazy<ZhzxTrafficViolationViewModel> viewModel;

        public ZhzxTrafficViolationView()
        {
            InitializeComponent();

            viewModel = new Lazy<ZhzxTrafficViolationViewModel>(() => ViewHelper.GetViewModel<ZhzxTrafficViolationViewModel>(this));
        }

        private void gridTrafficViolationList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridTrafficViolationList.Total)
            {
                startIndex = (gridTrafficViolationList.Total / args.PageSize) * args.PageSize;
                gridTrafficViolationList.PageIndex = (gridTrafficViolationList.Total % args.PageSize) == 0 ? (gridTrafficViolationList.Total / args.PageSize) : (gridTrafficViolationList.Total / args.PageSize) + 1;
                args.PageIndex = gridTrafficViolationList.PageIndex;
            }

            IQueryable<ZhzxTrafficViolation> gridSource = viewModel.Value.TrafficViolation;

            gridTrafficViolationList.Total = gridSource.Count();

            var itemsSource = gridSource.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
            gridTrafficViolationList.ItemsSource = itemsSource;

        }

        public void PagingReload()
        {
            gridTrafficViolationList.RaisePageChanged();
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
            viewModel.Value.SelectViolationTypePhrase = (string)cbxViolationType.SelectedValue;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Title = "选择Excel";
            open.Filter = "Excel文档(*.xls, *.xlsx)|*.xls;*.xlsx";
            open.FilterIndex = 1;
            if ((bool)open.ShowDialog().GetValueOrDefault())//打开
            {
                try
                {
                    viewModel.Value.SourceDataPath = open.FileName;
                }
                catch (System.Exception ex)
                {
                    CurrentLoginService.Instance.LogException(ex);
                }
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Interop.HwndSource source = PresentationSource.FromVisual(this) as System.Windows.Interop.HwndSource;
            System.Windows.Forms.IWin32Window win = new OldWindow(source.Handle);
            System.Windows.Forms.DialogResult result = dlg.ShowDialog(win);

            try
            {
                viewModel.Value.TargetFolderPath = dlg.SelectedPath;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }
    }

    public class OldWindow : System.Windows.Forms.IWin32Window
    {
        IntPtr _handle;
        public OldWindow(IntPtr handle)
        {
            _handle = handle;
        }
        #region IWin32Window Members
        IntPtr System.Windows.Forms.IWin32Window.Handle
        {
            get { return _handle; }
        }
        #endregion
    } 
}
    