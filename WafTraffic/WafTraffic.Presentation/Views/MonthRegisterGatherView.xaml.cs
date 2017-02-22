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
    [Export(typeof(IMonthRegisterGatherView))]
    public partial class MonthRegisterGatherView  : UserControl, IMonthRegisterGatherView
    {
        private readonly Lazy<MonthRegisterGatherViewModel> viewModel;
        public MonthRegisterGatherView()
        {
            InitializeComponent();
            viewModel = new Lazy<MonthRegisterGatherViewModel>(() => ViewHelper.GetViewModel<MonthRegisterGatherViewModel>(this));

            Unloaded += new RoutedEventHandler(MonthRegisterGatherView_Unloaded);
            _reportViewer.Load += ReportViewer_Load;
            
        }

        void MonthRegisterGatherView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool _isReportViewerLoaded;

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                _reportViewer.ProcessingMode = ProcessingMode.Local;
                LocalReport localReport = _reportViewer.LocalReport;
                this._reportViewer.LocalReport.ReportPath = "MonthRegisterChart.rdlc";

                ReportDataSource ds = new ReportDataSource();
                ds.Name = "DataSet1";//DataSet名_表名
                ds.Value = viewModel.Value.MonthRegisterReports;
                this._reportViewer.LocalReport.DataSources.Add(ds);

                
                _reportViewer.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }

        public void ReportReload()
        {
            ReportDataSource ds = new ReportDataSource();
            ds.Name = "DataSet1";//DataSet名_表名            
            ds.Value = viewModel.Value.GatherChartSource;
            this._reportViewer.LocalReport.DataSources.Clear();
            this._reportViewer.LocalReport.DataSources.Add(ds);

            _reportViewer.RefreshReport();
        }

        public void Close()
        {
            _reportViewer.LocalReport.ReleaseSandboxAppDomain();
        }
    }
}
    