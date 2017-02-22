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
    [Export(typeof(IFzkReleaseCarPrintView))]
    public partial class FzkReleaseCarPrintView : UserControl, IFzkReleaseCarPrintView
    {
        private readonly Lazy<FzkReleaseCarPrintViewModel> viewModel;
        public FzkReleaseCarPrintView()
        {
            InitializeComponent();
            viewModel = new Lazy<FzkReleaseCarPrintViewModel>(() => ViewHelper.GetViewModel<FzkReleaseCarPrintViewModel>(this));

            Unloaded += new RoutedEventHandler(FzkReleaseCarPrintView_Unloaded);

        }

        void FzkReleaseCarPrintView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void GatherReportReload()
        {
            this._gatherReportViewer.ProcessingMode = ProcessingMode.Local;
            LocalReport localReport = this._gatherReportViewer.LocalReport;
            this._gatherReportViewer.LocalReport.ReportPath = "FzkReleaseCarPrint.rdlc";

            ReportDataSource ds = new ReportDataSource();
            ds.Name = "DataSet1";//DataSet名_表名            

            System.Collections.Generic.List<FzkReleaseCar> dataSet = new System.Collections.Generic.List<FzkReleaseCar>();
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
