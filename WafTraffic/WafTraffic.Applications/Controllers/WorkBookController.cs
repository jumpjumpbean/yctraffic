using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.Foundation;
using WafTraffic.Applications.Properties;
using WafTraffic.Applications.Services;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using DotNet.Business;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class WorkBookController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private WorkBookViewModel workBookViewModel;

        private readonly DelegateCommand queryCommand;
        
        [ImportingConstructor]
        public WorkBookController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            this.container = container;
            this.messageService = messageService;
            this.shellService = shellService;
            this.entityService = entityService;

            mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
            workBookViewModel = container.GetExportedValue<WorkBookViewModel>();

            this.queryCommand = new DelegateCommand(() => QueryOper(), null);
        }

        public void Initialize()
        {
            workBookViewModel.SelectYear = DateTime.Now.Year;
            workBookViewModel.SelectMonth = DateTime.Now.Month;

            workBookViewModel.QueryCommand = this.queryCommand;
        }

        public bool QueryOper()
        {
            bool query = true;
            try
            {
                //string workContents = entityService.GetWorkContents(workBookViewModel.SelectYear, workBookViewModel.SelectMonth, CurrentLoginService.Instance.CurrentUserInfo.DepartmentCode + "%");
                string workContents = entityService.GetWorkContents(workBookViewModel.SelectYear, workBookViewModel.SelectMonth, "10.10%");
                workBookViewModel.WorkContents = workContents;
                if (string.IsNullOrEmpty(workContents))
                {
                    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "台账未填写完成."));
                }
            }
            catch (System.Exception ex)
            {
                query = false;
                //errlog处理
                CurrentLoginService.Instance.LogException(ex);
            }
            return query;
        }
    }
}
    