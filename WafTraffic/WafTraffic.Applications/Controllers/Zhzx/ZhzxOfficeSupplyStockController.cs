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
    internal class ZhzxOfficeSupplyStockController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private ZhzxOfficeSupplyStockListViewModel zhzxOfficeSupplyStockListViewModel;   //公告公示模块主界面
        private ZhzxOfficeSupplyStockDetailViewModel zhzxOfficeSupplyStockDetailViewModel;  

        private ZhzxOfficeSupplyStock tmpZhzxOfficeSupplyStock = new ZhzxOfficeSupplyStock();

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand modifyCommand;
        private readonly DelegateCommand deleteCommand;
        private readonly DelegateCommand browseCommand;
        private readonly DelegateCommand saveCommand;
        private readonly DelegateCommand retreatCommand;
        private readonly DelegateCommand queryCommand;

        [ImportingConstructor]
        public ZhzxOfficeSupplyStockController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.container = container;
                this.messageService = messageService;
                this.shellService = shellService;
                this.entityService = entityService;

                mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                zhzxOfficeSupplyStockListViewModel = container.GetExportedValue<ZhzxOfficeSupplyStockListViewModel>();
                zhzxOfficeSupplyStockDetailViewModel = container.GetExportedValue<ZhzxOfficeSupplyStockDetailViewModel>(); 

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
                zhzxOfficeSupplyStockListViewModel.NewCommand = this.newCommand;
                zhzxOfficeSupplyStockListViewModel.ModifyCommand = this.modifyCommand;
                zhzxOfficeSupplyStockListViewModel.DeleteCommand = this.deleteCommand;
                zhzxOfficeSupplyStockListViewModel.BrowseCommand = this.browseCommand;
                zhzxOfficeSupplyStockListViewModel.QueryCommand = this.queryCommand;

                zhzxOfficeSupplyStockDetailViewModel.SaveCommand = this.saveCommand;
                zhzxOfficeSupplyStockDetailViewModel.RetreatCommand = this.retreatCommand;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool NewOper()
        {
            bool newer = true;
            zhzxOfficeSupplyStockDetailViewModel.Operation = "New";

            try
            {
                zhzxOfficeSupplyStockDetailViewModel.CanBaseInfoEdit = false;
                zhzxOfficeSupplyStockDetailViewModel.CanSave = System.Windows.Visibility.Visible;
                zhzxOfficeSupplyStockDetailViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;
                zhzxOfficeSupplyStockDetailViewModel.CanAboveSaveVisibal = System.Windows.Visibility.Visible;

                zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock = new ZhzxOfficeSupplyStock();

                zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock.RecordTime = System.DateTime.Now;

                mainFrameViewModel.ContentView = zhzxOfficeSupplyStockDetailViewModel.View;
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
            zhzxOfficeSupplyStockDetailViewModel.Operation = "Modify";

            try
            {
                zhzxOfficeSupplyStockDetailViewModel.CanBaseInfoEdit = false;
                zhzxOfficeSupplyStockDetailViewModel.CanSave = System.Windows.Visibility.Visible;
                zhzxOfficeSupplyStockDetailViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;
                zhzxOfficeSupplyStockDetailViewModel.CanAboveSaveVisibal = System.Windows.Visibility.Visible;

                zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock = zhzxOfficeSupplyStockListViewModel.SelectedZhzxOfficeSupplyStock;

                mainFrameViewModel.ContentView = zhzxOfficeSupplyStockDetailViewModel.View;
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
            zhzxOfficeSupplyStockDetailViewModel.Operation = "Delete";

            try
            {
                if (zhzxOfficeSupplyStockListViewModel.SelectedZhzxOfficeSupplyStock != null &&
                    entityService.Entities.ZhzxOfficeSupplyStocks.Select(entity => entity.Id == zhzxOfficeSupplyStockListViewModel.SelectedZhzxOfficeSupplyStock.Id).Count() > 0)
                {
                    DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        zhzxOfficeSupplyStockListViewModel.SelectedZhzxOfficeSupplyStock.IsDeleted = true;  // set IsDeleted to 1, means this record was deleted by logical, but not real deleted.

                        entityService.Entities.SaveChanges();
                        //刷新DataGrid
                        zhzxOfficeSupplyStockListViewModel.GridRefresh();

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
                zhzxOfficeSupplyStockDetailViewModel.Operation = "Browse";

                zhzxOfficeSupplyStockDetailViewModel.CanBaseInfoEdit = true;
                zhzxOfficeSupplyStockDetailViewModel.CanSave = System.Windows.Visibility.Collapsed;
                zhzxOfficeSupplyStockDetailViewModel.CanCreatorVisibal = System.Windows.Visibility.Visible;
                zhzxOfficeSupplyStockDetailViewModel.CanAboveSaveVisibal = System.Windows.Visibility.Collapsed;

                zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock = zhzxOfficeSupplyStockListViewModel.SelectedZhzxOfficeSupplyStock;

                mainFrameViewModel.ContentView = zhzxOfficeSupplyStockDetailViewModel.View;
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
                zhzxOfficeSupplyStockDetailViewModel.Operation = "Query";
                zhzxOfficeSupplyStockListViewModel.ZhzxOfficeSupplyStock = entityService.QueryableZhzxOfficeSupplyStock.Where<ZhzxOfficeSupplyStock>
                    (
                        entity =>
                            (
                             (entity.CreateTime.Value >= zhzxOfficeSupplyStockListViewModel.StartDate)
                                &&
                             (entity.CreateTime.Value <= zhzxOfficeSupplyStockListViewModel.EndDate)
                                &&
                             ((string.IsNullOrEmpty(zhzxOfficeSupplyStockListViewModel.KeyWord)) ? true : (entity.ItemName.Contains(zhzxOfficeSupplyStockListViewModel.KeyWord)))
                                &&
                            (entity.IsDeleted == false))
                    );

                //列表页
                zhzxOfficeSupplyStockListViewModel.GridRefresh();
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
                zhzxOfficeSupplyStockDetailViewModel.Operation = "Retreat";

                mainFrameViewModel.ContentView = zhzxOfficeSupplyStockListViewModel.View;
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


                if (zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock.Id > 0)   //update
                {
                    zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock.UpdateTime = System.DateTime.Now;

                    zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock.UpdateName =
                        CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock.UpdaterId =
                        Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);

                }
                else  //insert
                {
                    zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock.CreateName =
                        CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock.CreateId =
                        Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);

                    zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock.CreateTime = System.DateTime.Now;

                    entityService.Entities.ZhzxOfficeSupplyStocks.AddObject(zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock);
                }

                entityService.Entities.SaveChanges();

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mainFrameViewModel.ContentView = zhzxOfficeSupplyStockListViewModel.View;
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
            if (zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock.RecordTime == null)
            {
                messageService.ShowMessage("统计时间是必填项");
                return false;
            }


            if (string.IsNullOrEmpty(zhzxOfficeSupplyStockDetailViewModel.ZhzxOfficeSupplyStock.ItemName))
            {
                messageService.ShowMessage("物品名称是必填项");
                return false;
            }




            return true;
        }
    }
}
    