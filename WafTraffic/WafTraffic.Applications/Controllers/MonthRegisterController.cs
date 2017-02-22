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
using WafTraffic.Domain.Common;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class MonthRegisterController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private MonthRegisterListViewModel monthRegisterListViewModel;
        private MonthRegisterApproveViewModel monthRegisterApproveViewModel;
        private MonthRegisterViewModel monthRegisterViewModel;
        private MonthRegisterGatherViewModel monthRegisterGatherViewModel;

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand gatherCommand;
        private readonly DelegateCommand modifyCommand;
        private readonly DelegateCommand deleteCommand;
        private readonly DelegateCommand approveCommand;
        private readonly DelegateCommand submitCommand;
        private readonly DelegateCommand browseCommand;
        private readonly DelegateCommand queryCommand;

        private readonly DelegateCommand saveCommand;        
        private readonly DelegateCommand retreatCommand;
        private readonly DelegateCommand queryGatherCommand;

        [ImportingConstructor]
        public MonthRegisterController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            this.container = container;
            this.messageService = messageService;
            this.shellService = shellService;
            this.entityService = entityService;

            mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
            monthRegisterListViewModel = container.GetExportedValue<MonthRegisterListViewModel>();
            monthRegisterApproveViewModel = container.GetExportedValue<MonthRegisterApproveViewModel>();
            monthRegisterViewModel = container.GetExportedValue<MonthRegisterViewModel>();
            monthRegisterGatherViewModel = container.GetExportedValue<MonthRegisterGatherViewModel>();

            this.newCommand = new DelegateCommand(() => NewOper(), null);
            this.gatherCommand = new DelegateCommand(() => GatherOper(), null);

            this.modifyCommand = new DelegateCommand(() => ModifyOper(), null);
            this.deleteCommand = new DelegateCommand(() => DeleteOper(), null);
            this.browseCommand = new DelegateCommand(() => BrowseOper(), null);
            this.approveCommand = new DelegateCommand(() => ApproveOper(), null);
            this.queryCommand = new DelegateCommand(() => QueryOper(), null);

            this.submitCommand = new DelegateCommand(() => SubmitOper(), null);
            this.saveCommand = new DelegateCommand(() => Save(), null);
            this.retreatCommand = new DelegateCommand(() => RetreatOper(), null);
            this.queryGatherCommand = new DelegateCommand(() => QueryGatherOper(), null);

        }

        public void Initialize()
        {
            monthRegisterListViewModel.NewCommand = this.newCommand;
            monthRegisterListViewModel.GatherCommand = this.gatherCommand;
            monthRegisterListViewModel.QueryCommand = this.queryCommand;

            monthRegisterListViewModel.ModifyCommand = this.modifyCommand;
            monthRegisterListViewModel.DeleteCommand = this.deleteCommand;
            monthRegisterListViewModel.BrowseCommand = this.browseCommand;
            monthRegisterListViewModel.ApproveCommand = this.approveCommand;

            monthRegisterViewModel.SubmitCommand = this.submitCommand;
            monthRegisterViewModel.SaveCommand = this.saveCommand;
            monthRegisterViewModel.RetreatCommand = this.retreatCommand;

            monthRegisterApproveViewModel.SaveCommand = this.saveCommand;
            monthRegisterApproveViewModel.RetreatCommand = this.retreatCommand;

            monthRegisterGatherViewModel.QueryGatherCommand = this.queryGatherCommand;
            monthRegisterGatherViewModel.RetreatCommand = this.retreatCommand;

        }

        public bool QueryGatherOper()
        {
            bool deal = true;
            monthRegisterApproveViewModel.Operation = "";
            monthRegisterViewModel.Operation = "";
            try
            {
                monthRegisterGatherViewModel.GatherApproveList = entityService.GetGatherApproves(monthRegisterGatherViewModel.SelectDepartId, monthRegisterGatherViewModel.MinMonth, monthRegisterGatherViewModel.MaxMonth);
                monthRegisterGatherViewModel.GatherChartSource = entityService.GetGatherChartSource(monthRegisterGatherViewModel.SelectDepartId, monthRegisterGatherViewModel.MinMonth, monthRegisterGatherViewModel.MaxMonth);
            }
            catch
            {
                monthRegisterGatherViewModel.GatherApproveList = new List<MohthRgisterGatherTable>();
            }
            monthRegisterGatherViewModel.MonthRegisterSeries = monthRegisterGatherViewModel.GetMonthRegisterSeries();
            monthRegisterGatherViewModel.MonthRegisterReports = monthRegisterGatherViewModel.GetMonthRegisterReports();

            monthRegisterGatherViewModel.ReprotRefresh();

            return deal;
        }

        public bool RetreatOper()
        {
            bool deal = true;
            monthRegisterApproveViewModel.Operation = "";
            monthRegisterViewModel.Operation = "";
            
            //if (monthRegisterListViewModel.SelectedMonthRegister != null && monthRegisterListViewModel.SelectedMonthRegister.Id != 0)
            //{
            //    var modifyEntities =

            //    from p in entityService.Entities.MonthRegisterTables

            //    where p.Id == monthRegisterListViewModel.SelectedMonthRegister.Id     // return only entities, not relationships

            //    select p;
            //    if (modifyEntities.ToList().Count > 0)
            //    {
            //        entityService.Entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, modifyEntities);
            //        monthRegisterListViewModel.SelectedMonthRegister = modifyEntities.ToList()[0];
            //    }
            //    //entityService.Entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, monthRegisterListViewModel.SelectedMonthRegister);
            //}
            mainFrameViewModel.ContentView = monthRegisterListViewModel.View;

            return deal;
        }

        public bool BrowseOper()
        {
            bool deal = true;
            monthRegisterApproveViewModel.Operation = "Browse";
            monthRegisterViewModel.Operation = "";

            monthRegisterApproveViewModel.MonthRegister = monthRegisterListViewModel.SelectedMonthRegister;
            mainFrameViewModel.ContentView = monthRegisterApproveViewModel.View;

            return deal;
        }

        public bool ApproveOper()
        {
            bool newer = true;
            monthRegisterApproveViewModel.Operation = "Approve";
            monthRegisterViewModel.Operation = "";

            monthRegisterApproveViewModel.MonthRegister = monthRegisterListViewModel.SelectedMonthRegister;
            mainFrameViewModel.ContentView = monthRegisterApproveViewModel.View;
            return newer;
        }

        public bool NewOper()
        {
            bool newer = true;
            monthRegisterViewModel.Operation = "New";
            monthRegisterApproveViewModel.Operation = "";

            monthRegisterViewModel.MonthRegister = new MonthRegisterTable();
            mainFrameViewModel.ContentView = monthRegisterViewModel.View;
            return newer;
        }

        public bool GatherOper()
        {
            bool newer = true;
            monthRegisterViewModel.Operation = "";
            monthRegisterApproveViewModel.Operation = "";

            //只是转页
            mainFrameViewModel.ContentView = monthRegisterGatherViewModel.View;
            return newer;
        }

        public bool ModifyOper()
        {
            bool newer = true;
            monthRegisterViewModel.Operation = "Modify";
            monthRegisterApproveViewModel.Operation = "";

            monthRegisterViewModel.MonthRegister = monthRegisterListViewModel.SelectedMonthRegister;
            mainFrameViewModel.ContentView = monthRegisterViewModel.View;
            return newer;
        }

        public bool QueryOper()
        {
            bool query = true;
            //monthRegisterViewModel.Operation = "";
            //monthRegisterApproveViewModel.Operation = "";

            //monthRegisterListViewModel.MonthRegisters = entityService.EnumMonthRegisters.Where<MonthRegisterTable>(
            //    entity => (entity.WhichMonth >= monthRegisterListViewModel.MinMonth && entity.WhichMonth <= monthRegisterListViewModel.MaxMonth)
            //    );

            ////列表页
            //monthRegisterListViewModel.GridRefresh();

            return query;
        }

        public bool SubmitOper()
        {
            bool newer = true;
            monthRegisterViewModel.Operation = "Submit";
            monthRegisterApproveViewModel.Operation = "";
            if (monthRegisterViewModel.MonthRegister != null && monthRegisterViewModel.MonthRegister.Id > 0)
            {
                if (Save())
                {
                    mainFrameViewModel.ContentView = monthRegisterListViewModel.View;
                }
            }
            else
            {
                monthRegisterViewModel.Operation = "New";
                messageService.ShowError(shellService.ShellView,  string.Format(CultureInfo.CurrentCulture, "操作无效，请先保存后提交！"));
            }

            return newer;
        }

        public bool DeleteOper()
        {
            bool deler = true;
            monthRegisterViewModel.Operation = "Delete";
            monthRegisterApproveViewModel.Operation = "";

            if (monthRegisterListViewModel.SelectedMonthRegister != null)
            {
                monthRegisterListViewModel.SelectedMonthRegister.IsDeleted = true;
                //entityService.MonthRegisters.Remove(monthRegisterListViewModel.SelectedMonthRegister);
                entityService.Entities.SaveChanges();
                //刷新DataGrid
                monthRegisterListViewModel.GridRefresh();

                messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "删除成功."));
            }
            else
            {
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "未找到"));
            }

            return deler;
        }

        public bool Save()
        {
            bool saved = false;
            if (monthRegisterViewModel.Operation == "New" || monthRegisterViewModel.Operation == "Modify" || monthRegisterViewModel.Operation == "Submit")
            {
                if ( string.IsNullOrEmpty( monthRegisterViewModel.MonthRegister.PoliceNumber ))
                {
                    messageService.ShowMessage("警号是必填项");
                    return false;
                }

                if (monthRegisterViewModel.MonthRegister.WhichMonth == null || monthRegisterViewModel.MonthRegister.WhichMonth == DateTime.MinValue)
                {
                    messageService.ShowMessage("月份是必填项");
                    return false;
                }
            }

            if (monthRegisterApproveViewModel.Operation == "Approve")
            {
                if ( string.IsNullOrEmpty(monthRegisterApproveViewModel.MonthRegister.ApproveResult ) )
                {
                    messageService.ShowMessage("审批结果是必选项");
                    return false;
                }
            }

            try
            {
                if (monthRegisterViewModel.Operation == "New")
                {
                    monthRegisterViewModel.MonthRegister.UserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    monthRegisterViewModel.MonthRegister.UserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    monthRegisterViewModel.MonthRegister.DepartmentId = CurrentLoginService.Instance.CurrentUserInfo.DepartmentId;
                    OrganizeService origanizeService = new OrganizeService();
                    monthRegisterViewModel.MonthRegister.DepartmentCode = origanizeService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo,
                                                                                                        CurrentLoginService.Instance.CurrentUserInfo.DepartmentId.ToString()).Code;
                    monthRegisterViewModel.MonthRegister.DepartmentName = CurrentLoginService.Instance.CurrentUserInfo.DepartmentName;

                    //monthRegisterViewModel.MonthRegister.RoleId = CurrentLoginService.Instance.CurrentUserInfo.RoleId;
                    //monthRegisterViewModel.MonthRegister.RoleName = CurrentLoginService.Instance.CurrentUserInfo.RoleName;
                    //monthRegisterViewModel.MonthRegister.WorkGroupId = CurrentLoginService.Instance.CurrentUserInfo.WorkgroupId;
                    //monthRegisterViewModel.MonthRegister.WorkGroupCode = CurrentLoginService.Instance.CurrentUserInfo.WorkgroupCode;
                    //monthRegisterViewModel.MonthRegister.WorkGroupName = CurrentLoginService.Instance.CurrentUserInfo.WorkgroupName;

                    monthRegisterViewModel.MonthRegister.StatusId = Convert.ToInt32(MonthRegisterStatus.Create);
                    monthRegisterViewModel.MonthRegister.StatusName = "新建";

                    monthRegisterViewModel.MonthRegister.IsDeleted = false;
                    monthRegisterViewModel.MonthRegister.CreateUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    monthRegisterViewModel.MonthRegister.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    monthRegisterViewModel.MonthRegister.CreateTime = System.DateTime.Now;

                    monthRegisterViewModel.MonthRegister.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    monthRegisterViewModel.MonthRegister.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    monthRegisterViewModel.MonthRegister.UpdateTime = System.DateTime.Now;

                    entityService.Entities.MonthRegisterTables.AddObject(monthRegisterViewModel.MonthRegister);

                    entityService.Entities.SaveChanges(); //insert
                    monthRegisterViewModel.Operation = "Modify";
                    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));

                }
                else if (monthRegisterViewModel.Operation == "Modify")
                {
                    if (monthRegisterViewModel.MonthRegister.StatusId >= Convert.ToInt32(MonthRegisterStatus.Handon))
                    {
                        messageService.ShowMessage("已经提交，不可再修改！");
                        return false;
                    }
                    monthRegisterViewModel.MonthRegister.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    monthRegisterViewModel.MonthRegister.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    monthRegisterViewModel.MonthRegister.UpdateTime = System.DateTime.Now;
                    entityService.Entities.SaveChanges(); //update

                    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                    
                }
                else if (monthRegisterViewModel.Operation == "Submit")
                {
                    if (monthRegisterViewModel.MonthRegister.StatusId >= Convert.ToInt32(MonthRegisterStatus.Handon))
                    {
                        //throw new UpdateException("已经提交，操作无效！");
                        messageService.ShowMessage("已经提交，操作无效！");
                        return false;
                    }
                    monthRegisterViewModel.MonthRegister.StatusId = Convert.ToInt32(MonthRegisterStatus.Handon);
                    monthRegisterViewModel.MonthRegister.StatusName = "提交";

                    monthRegisterViewModel.MonthRegister.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    monthRegisterViewModel.MonthRegister.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    monthRegisterViewModel.MonthRegister.UpdateTime = System.DateTime.Now;
                    entityService.Entities.SaveChanges(); //update

                    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                    //返回列表页
                    mainFrameViewModel.ContentView = monthRegisterListViewModel.View;
                }
                else if (monthRegisterApproveViewModel.Operation == "Approve")
                {
                    if (monthRegisterApproveViewModel.MonthRegister.StatusId < Convert.ToInt32(MonthRegisterStatus.Handon))
                    {
                        //throw new UpdateException("未提交，操作无效！");
                        messageService.ShowMessage("未提交，操作无效！");
                        return false;
                    }
                    else if (monthRegisterApproveViewModel.MonthRegister.StatusId >= Convert.ToInt32(MonthRegisterStatus.Approve))
                    {
                        //throw new UpdateException("已经审批过了，不可重复操作！");
                        messageService.ShowMessage("已经审批过了，不可重复操作！");
                        return false;
                    }
                    monthRegisterApproveViewModel.MonthRegister.StatusId = Convert.ToInt32(MonthRegisterStatus.Approve);
                    monthRegisterApproveViewModel.MonthRegister.StatusName = "审批";

                    monthRegisterApproveViewModel.MonthRegister.ApproveUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    monthRegisterApproveViewModel.MonthRegister.ApproveUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    monthRegisterApproveViewModel.MonthRegister.ApproveTime = System.DateTime.Now;

                    monthRegisterApproveViewModel.MonthRegister.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    monthRegisterApproveViewModel.MonthRegister.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    monthRegisterApproveViewModel.MonthRegister.UpdateTime = System.DateTime.Now;
                    entityService.Entities.SaveChanges(); //update
                    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "审批成功."));
                    //返回列表页
                    mainFrameViewModel.ContentView = monthRegisterListViewModel.View;
                }
                else
                {
                    return false;
                }

                saved = true;
                             
            }
            catch (ValidationException e)
            {
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidEntities, e.Message));
            }
            catch (UpdateException e)
            {
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidFields, e.Message));
            }
            catch (System.Exception ex)
            {
                saved = false;
                //errlog处理
                CurrentLoginService.Instance.LogException(ex);
            }

            return saved;
        }

        public void Close()
        {
            try
            {
                if (monthRegisterGatherViewModel != null)
                {
                    monthRegisterGatherViewModel.Close();
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }
    }
}
    