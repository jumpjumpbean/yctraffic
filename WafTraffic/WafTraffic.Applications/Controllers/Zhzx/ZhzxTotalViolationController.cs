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
using System.Windows.Forms;
using WafTraffic.Applications.Utils;
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class ZhzxTotalViolationController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private ZhzxTotalViolationListViewModel zhzxTotalViolationListViewModel;   
        private ZhzxTotalViolationDetailViewModel zhzxTotalViolationDetailViewModel;  

        private readonly DelegateCommand deleteCommand;
        private readonly DelegateCommand browseCommand;
        private readonly DelegateCommand retreatCommand;
        private readonly DelegateCommand queryCommand;

        [ImportingConstructor]
        public ZhzxTotalViolationController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.container = container;
                this.messageService = messageService;
                this.shellService = shellService;
                this.entityService = entityService;

                mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                zhzxTotalViolationListViewModel = container.GetExportedValue<ZhzxTotalViolationListViewModel>();
                zhzxTotalViolationDetailViewModel = container.GetExportedValue<ZhzxTotalViolationDetailViewModel>(); 


                this.deleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.retreatCommand = new DelegateCommand(() => RetreatOper(), null);
                this.browseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.queryCommand = new DelegateCommand(() => QueryOper(), null);
            }
            catch(System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public void Initialize()
        {
            try
            {
                zhzxTotalViolationListViewModel.DeleteCommand = this.deleteCommand;
                zhzxTotalViolationListViewModel.BrowseCommand = this.browseCommand;
                zhzxTotalViolationListViewModel.QueryCommand = this.queryCommand;

                zhzxTotalViolationDetailViewModel.RetreatCommand = this.retreatCommand;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool DeleteOper()
        {
            bool deler = true;

            try
            {
                if (zhzxTotalViolationListViewModel.SelectedZhzxTotalViolation != null &&
                    entityService.Entities.ZhzxTotalViolations.Select(entity => entity.Id == zhzxTotalViolationListViewModel.SelectedZhzxTotalViolation.Id).Count() > 0)
                {
                    DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        zhzxTotalViolationListViewModel.SelectedZhzxTotalViolation.IsDeleted = true;  // set IsDeleted to 1, means this record was deleted by logical, but not real deleted.

                        entityService.Entities.SaveChanges();
                        //刷新DataGrid
                        zhzxTotalViolationListViewModel.GridRefresh();

                        messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "删除成功."));
                    }
                }
                else
                {
                    messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "未找到"));
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deler;
        }

        public bool BrowseOper()
        {
            bool deal = true;
            try
            {
                zhzxTotalViolationDetailViewModel.ZhzxTotalViolation = zhzxTotalViolationListViewModel.SelectedZhzxTotalViolation;

                mainFrameViewModel.ContentView = zhzxTotalViolationDetailViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool QueryOper()
        {
            bool deal = true;
            try
            {
                zhzxTotalViolationListViewModel.ZhzxTotalViolation = entityService.QueryableZhzxTotalViolation.Where<ZhzxTotalViolation>
                    (
                        entity =>
                            (
                             ((string.IsNullOrEmpty(zhzxTotalViolationListViewModel.PlateNumber)) ? true : (entity.LicensePlateNumber.Contains(zhzxTotalViolationListViewModel.PlateNumber)))
                                &&
                             ((string.IsNullOrEmpty(zhzxTotalViolationListViewModel.Checkpoint)) ? true : (entity.CheckpointName.Contains(zhzxTotalViolationListViewModel.Checkpoint)))
                                &&
                            (entity.IsDeleted == false))
                    );

                //列表页
                zhzxTotalViolationListViewModel.GridRefresh();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool RetreatOper()
        {
            bool newer = true;

            try
            {
                mainFrameViewModel.ContentView = zhzxTotalViolationListViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }
    }
}
    