using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Collections.ObjectModel;
using WafTraffic.Domain;
using Microsoft.Reporting.WinForms;
using System.Windows;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// MonthRegisterGatherView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IZhzxTrafficViolationGatherView))]
    public partial class ZhzxTrafficViolationGatherView : UserControl, IZhzxTrafficViolationGatherView
    {
        private readonly Lazy<ZhzxTrafficViolationGatherViewModel> viewModel;
        public ZhzxTrafficViolationGatherView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxTrafficViolationGatherViewModel>(() => ViewHelper.GetViewModel<ZhzxTrafficViolationGatherViewModel>(this));

            Unloaded += new RoutedEventHandler(ZhzxTrafficViolationGatherView_Unloaded);
            //_uploadReportViewer.Load += ReportViewer_Load;
            //_approveReportViewer.Load += ReportViewer_Load;

        }

        void ZhzxTrafficViolationGatherView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private bool _isReportViewerLoaded;

        //private void ReportViewer_Load(object sender, EventArgs e)
        //{
        //    if (!_isReportViewerLoaded)
        //    {
        //        _reportViewer.ProcessingMode = ProcessingMode.Local;
        //        LocalReport localReport = _reportViewer.LocalReport;
        //        this._reportViewer.LocalReport.ReportPath = "TrafficViolationReport.rdlc";

        //        ReportDataSource ds = new ReportDataSource();
        //        ds.Name = "DataSet1";//DataSet名_表名
        //        ds.Value = viewModel.Value.ViolationGatherDataSource;
        //        this._reportViewer.LocalReport.DataSources.Add(ds);

        //        _reportViewer.RefreshReport();

        //        _isReportViewerLoaded = true;
        //    }
        //}

        public void UploadReportReload()
        {
            this._uploadReportViewer.ProcessingMode = ProcessingMode.Local;
            LocalReport localReport = this._uploadReportViewer.LocalReport;
            this._uploadReportViewer.LocalReport.ReportPath = "TrafficViolationUploadReport.rdlc";

            ReportDataSource ds = new ReportDataSource();
            ds.Name = "DataSet1";//DataSet名_表名            
            ds.Value = viewModel.Value.ViolationUploadGatherDataSource;
            this._uploadReportViewer.LocalReport.DataSources.Clear();
            this._uploadReportViewer.LocalReport.DataSources.Add(ds);

            _uploadReportViewer.RefreshReport();
        }

        public void ApproveReportReload()
        {
            this._approveReportViewer.ProcessingMode = ProcessingMode.Local;
            LocalReport localReport = this._approveReportViewer.LocalReport;
            this._approveReportViewer.LocalReport.ReportPath = "TrafficViolationApproveReport.rdlc";

            ReportDataSource ds = new ReportDataSource();
            ds.Name = "DataSet1";//DataSet名_表名            
            ds.Value = viewModel.Value.ViolationApproveGatherDataSource;
            this._approveReportViewer.LocalReport.DataSources.Clear();
            this._approveReportViewer.LocalReport.DataSources.Add(ds);

            _approveReportViewer.RefreshReport();
        }

        public void Close()
        {
            _uploadReportViewer.LocalReport.ReleaseSandboxAppDomain();
            _approveReportViewer.LocalReport.ReleaseSandboxAppDomain();
        }

        private void btnUploadQueryGather_Click(object sender, RoutedEventArgs e)
        {

            if (tbUploadStartDate.Value == null)
            {
                viewModel.Value.UploadStartDate = DateTime.Parse("1990-01-01");
            }
            else
            {
                viewModel.Value.UploadStartDate = Convert.ToDateTime(tbUploadStartDate.Value);
            }

            if (tbUploadEndDate.Value == null)
            {
                viewModel.Value.UploadEndDate = DateTime.Parse("2099-12-31");
            }
            else
            {
                viewModel.Value.UploadEndDate = Convert.ToDateTime(tbUploadEndDate.Value);
            }
        }

        private void btnApproveQueryGather_Click(object sender, RoutedEventArgs e)
        {

            if (tbApproveStartDate.Value == null)
            {
                viewModel.Value.ApproveStartDate = DateTime.Parse("1990-01-01");
            }
            else
            {
                viewModel.Value.ApproveStartDate = Convert.ToDateTime(tbApproveStartDate.Value);
            }

            if (tbApproveEndDate.Value == null)
            {
                viewModel.Value.ApproveEndDate = DateTime.Parse("2099-12-31");
            }
            else
            {
                viewModel.Value.ApproveEndDate = Convert.ToDateTime(tbApproveEndDate.Value);
            }
        }
    }
}
