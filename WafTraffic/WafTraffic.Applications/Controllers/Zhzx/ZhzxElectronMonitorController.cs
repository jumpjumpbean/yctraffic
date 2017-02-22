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
    internal class ZhzxElectronMonitorController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private ZhzxElectronMonitorListViewModel zhzxElectronMonitorListViewModel;   
        private ZhzxElectronMonitorDetailViewModel zhzxElectronMonitorDetailViewModel;  

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand modifyCommand;
        private readonly DelegateCommand deleteCommand;
        private readonly DelegateCommand browseCommand;
        private readonly DelegateCommand saveCommand;
        private readonly DelegateCommand retreatCommand;
        private readonly DelegateCommand queryCommand;

        [ImportingConstructor]
        public ZhzxElectronMonitorController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.container = container;
                this.messageService = messageService;
                this.shellService = shellService;
                this.entityService = entityService;

                mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                zhzxElectronMonitorListViewModel = container.GetExportedValue<ZhzxElectronMonitorListViewModel>();
                zhzxElectronMonitorDetailViewModel = container.GetExportedValue<ZhzxElectronMonitorDetailViewModel>(); 

                this.newCommand = new DelegateCommand(() => NewOper(), null);
                this.modifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.deleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.saveCommand = new DelegateCommand(() => Save(), null);
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
                zhzxElectronMonitorListViewModel.NewCommand = this.newCommand;
                zhzxElectronMonitorListViewModel.ModifyCommand = this.modifyCommand;
                zhzxElectronMonitorListViewModel.DeleteCommand = this.deleteCommand;
                zhzxElectronMonitorListViewModel.BrowseCommand = this.browseCommand;
                zhzxElectronMonitorListViewModel.QueryCommand = this.queryCommand;

                zhzxElectronMonitorDetailViewModel.SaveCommand = this.saveCommand;
                zhzxElectronMonitorDetailViewModel.RetreatCommand = this.retreatCommand;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool NewOper()
        {
            bool newer = true;

            try
            {
                zhzxElectronMonitorDetailViewModel.CanBaseInfoEdit = false;
                zhzxElectronMonitorDetailViewModel.CanSave = System.Windows.Visibility.Visible;
                zhzxElectronMonitorDetailViewModel.CanUpdaterVisibal = System.Windows.Visibility.Collapsed;

                zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor = new ZhzxElectronMonitor();

                mainFrameViewModel.ContentView = zhzxElectronMonitorDetailViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool ModifyOper()
        {
            bool newer = true;

            try
            {
                zhzxElectronMonitorDetailViewModel.CanBaseInfoEdit = false;
                zhzxElectronMonitorDetailViewModel.CanSave = System.Windows.Visibility.Visible;
                zhzxElectronMonitorDetailViewModel.CanUpdaterVisibal = System.Windows.Visibility.Collapsed;

                zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor = zhzxElectronMonitorListViewModel.SelectedZhzxElectronMonitor;

                mainFrameViewModel.ContentView = zhzxElectronMonitorDetailViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool DeleteOper()
        {
            bool deler = true;

            try
            {
                if (zhzxElectronMonitorListViewModel.SelectedZhzxElectronMonitor != null &&
                    entityService.Entities.ZhzxElectronMonitors.Select(entity => entity.Id == zhzxElectronMonitorListViewModel.SelectedZhzxElectronMonitor.Id).Count() > 0)
                {
                    DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        zhzxElectronMonitorListViewModel.SelectedZhzxElectronMonitor.IsDeleted = true;  // set IsDeleted to 1, means this record was deleted by logical, but not real deleted.

                        entityService.Entities.SaveChanges();
                        //刷新DataGrid
                        zhzxElectronMonitorListViewModel.GridRefresh();

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
                zhzxElectronMonitorDetailViewModel.CanBaseInfoEdit = true;
                zhzxElectronMonitorDetailViewModel.CanSave = System.Windows.Visibility.Collapsed;
                zhzxElectronMonitorDetailViewModel.CanUpdaterVisibal = System.Windows.Visibility.Visible;

                zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor = zhzxElectronMonitorListViewModel.SelectedZhzxElectronMonitor;

                mainFrameViewModel.ContentView = zhzxElectronMonitorDetailViewModel.View;
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
                zhzxElectronMonitorListViewModel.ZhzxElectronMonitor = entityService.QueryableZhzxElectronMonitor.Where<ZhzxElectronMonitor>
                    (
                        entity =>
                            (
                             ((string.IsNullOrEmpty(zhzxElectronMonitorListViewModel.CheckpointKeyWord)) ? true : (entity.CheckpointName.Contains(zhzxElectronMonitorListViewModel.CheckpointKeyWord)))
                                &&
                             ((string.IsNullOrEmpty(zhzxElectronMonitorListViewModel.StatusKeyWord)) ? true : (entity.Status.Contains(zhzxElectronMonitorListViewModel.StatusKeyWord)))
                                &&
                            (entity.IsDeleted == false))
                    );

                //列表页
                zhzxElectronMonitorListViewModel.GridRefresh();
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
                mainFrameViewModel.ContentView = zhzxElectronMonitorListViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool Save()
        {
            bool saver = true;

            if (!ValueCheck())
            {
                return false;
            }

            try
            {
                if (zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor.Id > 0)   //update
                {
                    zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor.UpdateTime = System.DateTime.Now;

                    zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor.UpdateName =
                        CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor.UpdaterId =
                        Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);

                }
                else  //insert
                {
                    entityService.Entities.ZhzxElectronMonitors.AddObject(zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor);

                    zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor.UpdateTime = System.DateTime.Now;

                    zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor.UpdateName =
                        CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor.UpdaterId =
                        Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                }

                entityService.Entities.SaveChanges();

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mainFrameViewModel.ContentView = zhzxElectronMonitorListViewModel.View;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return saver;
        }

        private bool ValueCheck()
        {
            if (string.IsNullOrEmpty(zhzxElectronMonitorDetailViewModel.ZhzxElectronMonitor.CheckpointName))
            {
                messageService.ShowMessage("卡口名称是必填项");
                return false;
            }

            return true;
        }
    }
}
    