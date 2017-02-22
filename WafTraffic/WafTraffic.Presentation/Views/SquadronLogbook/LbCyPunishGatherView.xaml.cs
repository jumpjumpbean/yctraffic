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
    [Export(typeof(ILbCyPunishGatherView))]
    public partial class LbCyPunishGatherView : UserControl, ILbCyPunishGatherView
    {
        private readonly Lazy<LbCyPunishGatherViewModel> viewModel;
        public LbCyPunishGatherView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbCyPunishGatherViewModel>(() => ViewHelper.GetViewModel<LbCyPunishGatherViewModel>(this));

            Unloaded += new RoutedEventHandler(LbCyPunishGatherView_Unloaded);

        }

        void LbCyPunishGatherView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void GatherReportReload()
        {
            this._gatherReportViewer.ProcessingMode = ProcessingMode.Local;
            LocalReport localReport = this._gatherReportViewer.LocalReport;
            this._gatherReportViewer.LocalReport.ReportPath = "LbCyPunishGatherReport.rdlc";

            ReportDataSource ds = new ReportDataSource();
            ds.Name = "DataSet1";//DataSet名_表名            
            ds.Value = viewModel.Value.PunishGatherList;
            this._gatherReportViewer.LocalReport.DataSources.Clear();
            this._gatherReportViewer.LocalReport.DataSources.Add(ds);

            _gatherReportViewer.RefreshReport();
        }



        public void Close()
        {
            _gatherReportViewer.LocalReport.ReleaseSandboxAppDomain();
        }

        //private void btnUploadQueryGather_Click(object sender, RoutedEventArgs e)
        //{

        //    if (tbUploadStartDate.Value == null)
        //    {
        //        viewModel.Value.UploadStartDate = DateTime.Parse("1990-01-01");
        //    }
        //    else
        //    {
        //        viewModel.Value.UploadStartDate = Convert.ToDateTime(tbUploadStartDate.Value);
        //    }

        //    if (tbUploadEndDate.Value == null)
        //    {
        //        viewModel.Value.UploadEndDate = DateTime.Parse("2099-12-31");
        //    }
        //    else
        //    {
        //        viewModel.Value.UploadEndDate = Convert.ToDateTime(tbUploadEndDate.Value);
        //    }
        //}

        //private void btnApproveQueryGather_Click(object sender, RoutedEventArgs e)
        //{

        //    if (tbApproveStartDate.Value == null)
        //    {
        //        viewModel.Value.ApproveStartDate = DateTime.Parse("1990-01-01");
        //    }
        //    else
        //    {
        //        viewModel.Value.ApproveStartDate = Convert.ToDateTime(tbApproveStartDate.Value);
        //    }

        //    if (tbApproveEndDate.Value == null)
        //    {
        //        viewModel.Value.ApproveEndDate = DateTime.Parse("2099-12-31");
        //    }
        //    else
        //    {
        //        viewModel.Value.ApproveEndDate = Convert.ToDateTime(tbApproveEndDate.Value);
        //    }
        //}
    }
}
