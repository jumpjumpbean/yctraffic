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
    internal class ZhzxFixedAssetsRegisterController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private ZhzxFixedAssetsRegisterListViewModel zhzxFixedAssetsRegisterListViewModel;   //公告公示模块主界面
        private ZhzxFixedAssetsRegisterDetailViewModel zhzxFixedAssetsRegisterDetailViewModel;  

        private ZhzxFixedAssetsRegister tmpZhzxFixedAssetsRegister = new ZhzxFixedAssetsRegister();

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand modifyCommand;
        private readonly DelegateCommand deleteCommand;
        private readonly DelegateCommand browseCommand;
        private readonly DelegateCommand saveCommand;
        private readonly DelegateCommand retreatCommand;
        private readonly DelegateCommand queryCommand;

        [ImportingConstructor]
        public ZhzxFixedAssetsRegisterController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.container = container;
                this.messageService = messageService;
                this.shellService = shellService;
                this.entityService = entityService;

                mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                zhzxFixedAssetsRegisterListViewModel = container.GetExportedValue<ZhzxFixedAssetsRegisterListViewModel>();
                zhzxFixedAssetsRegisterDetailViewModel = container.GetExportedValue<ZhzxFixedAssetsRegisterDetailViewModel>(); 

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
                zhzxFixedAssetsRegisterListViewModel.NewCommand = this.newCommand;
                zhzxFixedAssetsRegisterListViewModel.ModifyCommand = this.modifyCommand;
                zhzxFixedAssetsRegisterListViewModel.DeleteCommand = this.deleteCommand;
                zhzxFixedAssetsRegisterListViewModel.BrowseCommand = this.browseCommand;
                zhzxFixedAssetsRegisterListViewModel.QueryCommand = this.queryCommand;

                zhzxFixedAssetsRegisterDetailViewModel.SaveCommand = this.saveCommand;
                zhzxFixedAssetsRegisterDetailViewModel.RetreatCommand = this.retreatCommand;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool NewOper()
        {
            bool newer = true;
            zhzxFixedAssetsRegisterDetailViewModel.Operation = "New";

            try
            {
                zhzxFixedAssetsRegisterDetailViewModel.CanBaseInfoEdit = false;
                zhzxFixedAssetsRegisterDetailViewModel.CanSave = System.Windows.Visibility.Visible;
                zhzxFixedAssetsRegisterDetailViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;
                zhzxFixedAssetsRegisterDetailViewModel.CanAboveSaveVisibal = System.Windows.Visibility.Visible;

                zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister = new ZhzxFixedAssetsRegister();

                zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister.RegisteTime = System.DateTime.Now;

                mainFrameViewModel.ContentView = zhzxFixedAssetsRegisterDetailViewModel.View;
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
            zhzxFixedAssetsRegisterDetailViewModel.Operation = "Modify";

            try
            {
                zhzxFixedAssetsRegisterDetailViewModel.CanBaseInfoEdit = false;
                zhzxFixedAssetsRegisterDetailViewModel.CanSave = System.Windows.Visibility.Visible;
                zhzxFixedAssetsRegisterDetailViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;
                zhzxFixedAssetsRegisterDetailViewModel.CanAboveSaveVisibal = System.Windows.Visibility.Visible;

                zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister = zhzxFixedAssetsRegisterListViewModel.SelectedZhzxFixedAssetsRegister;

                mainFrameViewModel.ContentView = zhzxFixedAssetsRegisterDetailViewModel.View;
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
            zhzxFixedAssetsRegisterDetailViewModel.Operation = "Delete";

            try
            {
                if (zhzxFixedAssetsRegisterListViewModel.SelectedZhzxFixedAssetsRegister != null &&
                    entityService.Entities.ZhzxFixedAssetsRegisters.Select(entity => entity.Id == zhzxFixedAssetsRegisterListViewModel.SelectedZhzxFixedAssetsRegister.Id).Count() > 0)
                {
                    DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        zhzxFixedAssetsRegisterListViewModel.SelectedZhzxFixedAssetsRegister.IsDeleted = true;  // set IsDeleted to 1, means this record was deleted by logical, but not real deleted.

                        entityService.Entities.SaveChanges();
                        //刷新DataGrid
                        zhzxFixedAssetsRegisterListViewModel.GridRefresh();

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
                zhzxFixedAssetsRegisterDetailViewModel.Operation = "Browse";

                zhzxFixedAssetsRegisterDetailViewModel.CanBaseInfoEdit = true;
                zhzxFixedAssetsRegisterDetailViewModel.CanSave = System.Windows.Visibility.Collapsed;
                zhzxFixedAssetsRegisterDetailViewModel.CanCreatorVisibal = System.Windows.Visibility.Visible;
                zhzxFixedAssetsRegisterDetailViewModel.CanAboveSaveVisibal = System.Windows.Visibility.Collapsed;

                zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister = zhzxFixedAssetsRegisterListViewModel.SelectedZhzxFixedAssetsRegister;

                mainFrameViewModel.ContentView = zhzxFixedAssetsRegisterDetailViewModel.View;
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
                zhzxFixedAssetsRegisterDetailViewModel.Operation = "Query";
                zhzxFixedAssetsRegisterListViewModel.ZhzxFixedAssetsRegister = entityService.QueryableZhzxFixedAssetsRegister.Where<ZhzxFixedAssetsRegister>
                    (
                        entity =>
                            (
                             (entity.CreateTime.Value >= zhzxFixedAssetsRegisterListViewModel.StartDate)
                                &&
                             (entity.CreateTime.Value <= zhzxFixedAssetsRegisterListViewModel.EndDate)
                                &&
                             ((string.IsNullOrEmpty(zhzxFixedAssetsRegisterListViewModel.KeyWord)) ? true : (entity.AssetName.Contains(zhzxFixedAssetsRegisterListViewModel.KeyWord)))
                                &&
                            (entity.IsDeleted == false))
                    );

                //列表页
                zhzxFixedAssetsRegisterListViewModel.GridRefresh();
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
                zhzxFixedAssetsRegisterDetailViewModel.Operation = "Retreat";

                mainFrameViewModel.ContentView = zhzxFixedAssetsRegisterListViewModel.View;
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


                if (zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister.Id > 0)   //update
                {
                    zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister.UpdateTime = System.DateTime.Now;

                    zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister.UpdateName =
                        CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister.UpdaterId =
                        Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);

                }
                else  //insert
                {
                    zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister.CreateName =
                        CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister.CreateId =
                        Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);

                    zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister.CreateTime = System.DateTime.Now;

                    entityService.Entities.ZhzxFixedAssetsRegisters.AddObject(zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister);
                }

                entityService.Entities.SaveChanges();

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mainFrameViewModel.ContentView = zhzxFixedAssetsRegisterListViewModel.View;
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
            if (zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister.RegisteTime == null)
            {
                messageService.ShowMessage("登记时间是必填项");
                return false;
            }

            if (string.IsNullOrEmpty(zhzxFixedAssetsRegisterDetailViewModel.ZhzxFixedAssetsRegister.AssetName))
            {
                messageService.ShowMessage("资产名称是必填项");
                return false;
            }

            return true;
        }
    }
}
    