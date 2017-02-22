using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Waf.Applications;
using WafTraffic.Applications.Controllers;
using WafTraffic.Applications.Services;
using WafTraffic.Reporting.Applications.DataModels;
using WafTraffic.Reporting.Applications.Reports;
using WafTraffic.Reporting.Applications.ViewModels;

namespace WafTraffic.Reporting.Applications.Controllers
{
    [Export(typeof(IModuleController)), Export]
    internal class ModuleController : IModuleController
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly IEntityService entityService;
        private readonly Lazy<ReportViewModel> reportViewModel;
        private readonly DelegateCommand createWorkBookReportCommand;
       

        [ImportingConstructor]
        public ModuleController(CompositionContainer container, IShellService shellService, IEntityService entityService, Lazy<ReportViewModel> reportViewModel)
        {
            this.container = container;
            this.shellService = shellService;
            this.entityService = entityService;
            this.reportViewModel = reportViewModel;
            //this.reportViewModel = new Lazy<ReportViewModel>(() => container.GetExportedValue<ReportViewModel>());
            this.createWorkBookReportCommand = new DelegateCommand(CreateWorkBookReport);
        }


        private ReportViewModel ReportViewModel { get { return reportViewModel.Value; } }


        public void Initialize()
        {
            shellService.IsReportingEnabled = true;
            shellService.LazyReportingView = new Lazy<object>(InitializeReportView);
        }

        public void Run()
        {
        }

        public void Shutdown()
        {
        }

        private object InitializeReportView()
        {
            ReportViewModel.CreateWorkBookReportCommand = createWorkBookReportCommand;
            return ReportViewModel.View;
        }

        private void CreateWorkBookReport()
        {
            IWorkBookReport workBookReport = container.GetExportedValue<IWorkBookReport>();
            workBookReport.ReportData = new WorkBookReportDataModel(entityService.EnumWorkBookReport);
            ReportViewModel.Report = workBookReport.Report;
        }

    }
}
