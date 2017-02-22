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
    [Export(typeof(ISgkReleaseCarPrintView))]
    public partial class SgkReleaseCarPrintView : UserControl, ISgkReleaseCarPrintView
    {
        private readonly Lazy<SgkReleaseCarPrintViewModel> viewModel;
        public SgkReleaseCarPrintView()
        {
            InitializeComponent();
            viewModel = new Lazy<SgkReleaseCarPrintViewModel>(() => ViewHelper.GetViewModel<SgkReleaseCarPrintViewModel>(this));

            Unloaded += new RoutedEventHandler(SgkReleaseCarPrintView_Unloaded);

        }

        void SgkReleaseCarPrintView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void GatherReportReload()
        {
            this._gatherReportViewer.ProcessingMode = ProcessingMode.Local;
            LocalReport localReport = this._gatherReportViewer.LocalReport;
            this._gatherReportViewer.LocalReport.ReportPath = "SgkReleaseCarPrint.rdlc";

            ReportDataSource ds = new ReportDataSource();
            ds.Name = "DataSet1";//DataSet名_表名            

            System.Collections.Generic.List<SgkReleaseCar> dataSet = new System.Collections.Generic.List<SgkReleaseCar>();
            dataSet.Add(viewModel.Value.ReleaseCarEntity);

            ds.Value = dataSet;
            
            this._gatherReportViewer.LocalReport.DataSources.Clear();
            this._gatherReportViewer.LocalReport.DataSources.Add(ds);

            _gatherReportViewer.RefreshReport();
        }



        public void Close()
        {
            _gatherReportViewer.LocalReport.ReleaseSandboxAppDomain();
        }


    }
}
