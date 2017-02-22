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
using System.Windows.Threading;
using System.Globalization;
using DotNet.Business;
using WafTraffic.Domain.Common;
using System.Windows.Media;
using System.Windows.Forms;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class AlarmNotifyController : Controller
    {
        private readonly CompositionContainer container;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private ShellViewModel shellViewModel;
        private AlarmNotifyViewModel alarmNotifyViewModel;
        private FrequentUsedLinkConfigViewModel frequentUsedLindConfigViewModel;

        private DispatcherTimer alarmNotifyTimer;

        private readonly DelegateCommand alarmHandlerCommand;
        private readonly DelegateCommand showAlarmCommand;
        private readonly DelegateCommand alarmNotifyRefreshCommand;
        private readonly DelegateCommand settingCommand;
        private readonly DelegateCommand saveCommand;
        private readonly DelegateCommand retreatCommand;

        [ImportingConstructor]
        public AlarmNotifyController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            this.container = container;
            this.messageService = messageService;
            this.entityService = entityService;

            this.mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
            shellViewModel = container.GetExportedValue<ShellViewModel>();
            alarmNotifyViewModel = container.GetExportedValue<AlarmNotifyViewModel>();
            frequentUsedLindConfigViewModel = container.GetExportedValue<FrequentUsedLinkConfigViewModel>();

            this.alarmHandlerCommand = new DelegateCommand(() => AlarmNotifyHandler(), null);
            this.alarmNotifyRefreshCommand = new DelegateCommand(() => ShowAlarmNotifyView(), null);
            this.showAlarmCommand = new DelegateCommand(() => ShowAlarmNotifyView(), null);
            this.settingCommand = new DelegateCommand(()=> SettingOper(), null);
            this.saveCommand = new DelegateCommand(()=> SaveOper(), null);
            this.retreatCommand = new DelegateCommand(() => RetreatOper(), null);


            if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
            {
                alarmNotifyViewModel.CanSettingShow = System.Windows.Visibility.Visible;
            }
            else
            {
                alarmNotifyViewModel.CanSettingShow = System.Windows.Visibility.Collapsed;
            }

            alarmNotifyTimer = new DispatcherTimer();
        }

        public void Initialize()
        {
            alarmNotifyViewModel.AlarmNotifyHandlerCommand = this.alarmHandlerCommand;
            alarmNotifyViewModel.AlarmNotifyRefreshCommand = this.alarmNotifyRefreshCommand;
            alarmNotifyViewModel.SettingCommand = this.settingCommand;
            shellViewModel.ShowAlarmCommand = this.showAlarmCommand;
            frequentUsedLindConfigViewModel.SaveCommand = this.saveCommand;
            frequentUsedLindConfigViewModel.RetreatCommand = this.retreatCommand;

            AlarmTimerExpireHandler();
            alarmNotifyViewModel.AlarmCount = alarmNotifyViewModel.AlarmNotifyList.Count<AlarmNotifyTable>().ToString();

        }


        public bool AlarmNotifyHandler()
        {
            bool dealer = true;
            if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "MaterialDeclareAlarmCheck")
            {
                this.MaterialDeclareAlarmCheck();
            }
            //else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "NeedMonthRegister")
            //{
            //    this.NeedMonthRegister();
            //}
            else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "NoDealHotLine")
            {
                this.NoDealHotLine();
            }
            //else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "OverTimeHotLine")
            //{
            //    this.OverTimeHotLine();
            //}
            else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "EquipmentDueDate")
            {
                //this.EquipmentDueDate();
            }
            else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "OverTimeZhzxRequest")
            {
                this.OverTimeZhzxRequestHandler();
            }
            else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "ZhzxEquipReq_NeedDdzDeals")
            {
                this.ZhzxEquipReq_NeedDdzDeal();
            }
            else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "KjkEquipReq_NeedDdzDeals")
            {
                this.KjkEquipReq_NeedDdzDeal();
            }
            else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "OverTimeZdtzCyDangerOverTime")
            {
                this.OverTimeZdtzCyDangerDealHandler();
            }
            else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "QbyqInfoCheck")
            {
                this.NonReadQbyqInfoHandler();
            }
            else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "ZgxcAskForLeave")
            {
                this.WaitingForApprovalLeavesHandler();
            }
            else if (alarmNotifyViewModel.SelectedAlarmNotify.AlarmTag == "FzkReleaseCarFdz")
            {
                this.FzkReleaseCarFdzHandler();
            }


            return dealer;
        }

        public bool MaterialDeclareAlarmCheck()
        {
            bool dealer = true;

            MaterialDeclareViewModel materialDeclareViewModel = container.GetExportedValue<MaterialDeclareViewModel>();

            materialDeclareViewModel.MaterialDeclare = container.GetExportedValue<EntityService>().EnumNoApproveMaterialDeclares;

            materialDeclareViewModel.GridRefresh();

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.ZGXC_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.MaterialDeclare_CODE);

            return dealer;
        }

        public void RunAlarmNotifyTimer()
        {
            alarmNotifyTimer.Interval = TimeSpan.FromMinutes(5);   
            alarmNotifyTimer.Tick += new EventHandler(AlarmNotifyTimer_Elapsed);
            
            alarmNotifyTimer.Start();
        }

        void AlarmNotifyTimer_Elapsed(object sender, EventArgs e)
        {
            this.AlarmTimerExpireHandler();
        }

        public void AlarmTimerExpireHandler()
        {
            alarmNotifyViewModel.AlarmNotifyList = GetAlarmNotify();

            if (alarmNotifyViewModel.AlarmNotifyList.Count() > 0)
            {
                alarmNotifyViewModel.NoAlarm = System.Windows.Visibility.Collapsed;
                shellViewModel.BackgroundColor = new SolidColorBrush(Colors.Red);
                shellViewModel.CanAlarmNotifyShow = System.Windows.Visibility.Visible;
                shellViewModel.LetAlarmButtonSingDance();
            }
            else
            {
                alarmNotifyViewModel.NoAlarm = System.Windows.Visibility.Visible;
                shellViewModel.BackgroundColor = new SolidColorBrush(Colors.Transparent);
                shellViewModel.CanAlarmNotifyShow = System.Windows.Visibility.Collapsed;

            }
        
        }

        //public bool NeedMonthRegister()
        //{
        //    bool dealer = true;
        //    MonthRegisterListViewModel monthRegisterViewModel = container.GetExportedValue<MonthRegisterListViewModel>();
        //    if (alarmNotifyViewModel.AlarmMonthRegisters != null)
        //    {
        //        monthRegisterViewModel.MonthRegisters.ToList().AddRange(alarmNotifyViewModel.AlarmMonthRegisters);
        //    }

        //    monthRegisterViewModel.GridRefresh();

        //    mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.ZGXC_ID);

        //    mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.MonthRegister_CODE);
            
        //    return dealer;
        //}

        public bool ZhzxEquipReq_NeedDdzDeal()
        {
            bool dealer = true;

            if (alarmNotifyViewModel.ZhzxEquipReq_NeedDdzDeals != null)
            {
                container.GetExportedValue<ZhzxRequestQueryViewModel>().Requests = alarmNotifyViewModel.ZhzxEquipReq_NeedDdzDeals;
            }

            container.GetExportedValue<ZhzxRequestQueryViewModel>().GridRefresh();

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.ZHZX_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.ZHZX_REQUEST_CODE);

            return dealer;
        }

        public bool KjkEquipReq_NeedDdzDeal()
        {
            bool dealer = true;

            if (alarmNotifyViewModel.KjkEquipReq_NeedDdzDeals != null)
            {
                container.GetExportedValue<KjkRequestQueryViewModel>().Requests = alarmNotifyViewModel.KjkEquipReq_NeedDdzDeals;
            }

            container.GetExportedValue<KjkRequestQueryViewModel>().GridRefresh();

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.KJSSK_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.EquipmentDueDate_CODE);

            return dealer;
        }

        public bool NoDealHotLine()
        {
            bool dealer = true;
            HotLineListViewModel hotLineListViewModel = container.GetExportedValue<HotLineListViewModel>();
            if(alarmNotifyViewModel.AlarmHotLineNoDeal != null)
            {
                hotLineListViewModel.HotLineTasks = alarmNotifyViewModel.AlarmHotLineNoDeal.AsQueryable() ;
            }

            hotLineListViewModel.GridRefresh();

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.ZGXC_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.HotLine_CODE);

            return dealer;
        }

        //public bool OverTimeHotLine()
        //{
        //    bool dealer = true;
        //    HotLineListViewModel hotLineListViewModel = container.GetExportedValue<HotLineListViewModel>();
        //    if (alarmNotifyViewModel.AlarmHotLineOverTime != null)
        //    {
        //        hotLineListViewModel.HotLineTasks = alarmNotifyViewModel.AlarmHotLineOverTime.AsQueryable<MayorHotlineTaskTable>();
        //    }

        //    hotLineListViewModel.GridRefresh();

        //    mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.ZGXC_ID);

        //    mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.HotLine_CODE);

        //    return dealer;
        //}

        public bool OverTimeZhzxRequestHandler()
        {
            bool dealer = true;

            container.GetExportedValue<ZhzxRequestQueryViewModel>().Requests = alarmNotifyViewModel.ZhzxRequestOverTime;

            container.GetExportedValue<ZhzxRequestQueryViewModel>().GridRefresh();

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.ZHZX_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.ZHZX_REQUEST_CODE);

            return dealer;
        }

        public bool OverTimeZdtzCyDangerDealHandler()
        {
            bool dealer = true;
            LbCyDangerDealQueryViewModel dangerDealQueryViewModel = container.GetExportedValue<LbCyDangerDealQueryViewModel>();
            dangerDealQueryViewModel.LbDangerDeals = alarmNotifyViewModel.ZdtzCyDangerDealOverTime;

            dangerDealQueryViewModel.GridRefresh();

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.ZDTZ_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.ZDTZ_CY_CODE);

            return dealer;
        }

        /*
        public bool EquipmentDueDate()
        {
            bool dealer = true;

            EquipmentCheckListViewModel equipmentCheckListViewModel = container.GetExportedValue<EquipmentCheckListViewModel>();
            if (CurrentLoginService.Instance.IsAuthorized("yctraffic.EquipmentCheck.Supervise"))
            {
                equipmentCheckListViewModel.EquipmentChecks = alarmNotifyViewModel.EquipmentDueDate;

               
            }
            else
            {
                var tmpList = alarmNotifyViewModel.EquipmentDueDate.Where(entity => entity.DepartmentId == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                equipmentCheckListViewModel.EquipmentChecks = tmpList;
            }
            equipmentCheckListViewModel.GridRefresh();

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.KJSSK_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.EquipmentDueDate_CODE);

            return dealer;
        }
        */

        public bool NonReadQbyqInfoHandler()
        {
            bool dealer = true;
            QbyqInfoAnalysisQueryViewModel qbyqInfoAnalysisQueryViewModel = container.GetExportedValue<QbyqInfoAnalysisQueryViewModel>();

            if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_DDZ)
            {
                qbyqInfoAnalysisQueryViewModel.PunishCases = entityService.NonReadQbyqInfo_DDZ;
            }
            else if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZW)
            {
                qbyqInfoAnalysisQueryViewModel.PunishCases = entityService.NonReadQbyqInfo_ZW;
            }

            qbyqInfoAnalysisQueryViewModel.GridRefresh();

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.QBYQ_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.QbyqCase_CODE);

            return dealer;
        }

        public bool WaitingForApprovalLeavesHandler()
        {
            bool dealer = true;
            ZgxcAskForLeaveQueryViewModel zgxcAskForLeaveQueryViewModel = container.GetExportedValue<ZgxcAskForLeaveQueryViewModel>();

            zgxcAskForLeaveQueryViewModel.ZgxcAskForLeaves = alarmNotifyViewModel.ZgxcWaitingForApprovalLeaves;

            zgxcAskForLeaveQueryViewModel.GridRefresh();

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.ZGXC_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.ZgxcAskForLeave_CODE);

            return dealer;
        }

        public bool FzkReleaseCarFdzHandler()
        {
            bool dealer = true;
            FzkReleaseCarQueryViewModel fzkReleaseCarQueryViewModel = container.GetExportedValue<FzkReleaseCarQueryViewModel>();

            fzkReleaseCarQueryViewModel.FzkReleaseCars = alarmNotifyViewModel.FzkReleaseCarFdzDeals;

            fzkReleaseCarQueryViewModel.GridRefresh();

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.FZK_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.FzkReleaseCar_CODE);

            return dealer;
        }

        public bool ShowAlarmNotifyView()
        {
            bool dealer = true;

            mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First(module => module.Id == YcConstantTable.DBSX_ID);

            mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First(module => module.Code == YcConstantTable.AlarmNotify_CODE);

            return dealer;
        }

        public List<AlarmNotifyTable> GetAlarmNotify()
        {
            List<AlarmNotifyTable> alarmNotify = new List<AlarmNotifyTable>();

            //材料申报提醒 ....
            if (System.DateTime.Now.AddDays(3).Month != System.DateTime.Now.Month
                && CurrentLoginService.Instance.IsAuthorized("yctraffic.MaterialDeclare.Score")
                && !CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator
                ) //每月最后三天提醒
            {
                if (alarmNotifyViewModel.MaterialDeclareNoApproveCount > 0)
                {
                    AlarmNotifyTable alarm = new AlarmNotifyTable();
                    alarm.AlarmMessage = "本月还有材料申报没有录入评分, 请查看？";
                    alarm.AlarmTag = "MaterialDeclareAlarmCheck";     // 任意字符串，用于AlarmNotifyHandler找到各自的入口方法

                    alarmNotify.Add(alarm);
                }
            }

            //市长热线临界时间的提醒 
            if (alarmNotifyViewModel.AlarmHotLineNoDeal.Count > 0)
            {
                AlarmNotifyTable alarm = new AlarmNotifyTable();
                alarm.AlarmMessage = "您的科室有快到期未处理的市长热线, 请查看？";
                alarm.AlarmTag = "NoDealHotLine";
                alarmNotify.Add(alarm);
            }

            // 隐患排查台账提醒
            if (alarmNotifyViewModel.ZdtzCyDangerDealOverTime.Count() > 0
                && (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZXK_KZ
                || CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZXK_YG))
            {
                AlarmNotifyTable alarm = new AlarmNotifyTable();
                alarm.AlarmMessage = "当前有将到期或过期未处理的隐患排查项, 请查看？";
                alarm.AlarmTag = "OverTimeZdtzCyDangerOverTime";
                alarmNotify.Add(alarm);
            }

            // 指挥中心设备申请提醒
            if (alarmNotifyViewModel.ZhzxRequestOverTime.Count() > 0 
                && (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZHZX_SCRY
                || CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZHZX_SHRY
                || CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZHZX_ZR))
            {
                AlarmNotifyTable alarm = new AlarmNotifyTable();
                alarm.AlarmMessage = "当前有将到期或过期未处理的办公设备申请, 请查看？";
                alarm.AlarmTag = "OverTimeZhzxRequest";
                alarmNotify.Add(alarm);
            }

            //情报舆情未读提醒 ....
            if (
                (CurrentLoginService.Instance.CurrentUserInfo.RoleName == "大队长" && entityService.NonReadQbyqInfo_DDZ.Count() > 0) ||
                (CurrentLoginService.Instance.CurrentUserInfo.RoleName == "政委" && entityService.NonReadQbyqInfo_ZW.Count() > 0)
                ) //大队长或政委有未读情报舆情
            {

                AlarmNotifyTable alarm = new AlarmNotifyTable();
                alarm.AlarmMessage = "你有未读的情报舆情, 请查看？";
                alarm.AlarmTag = "QbyqInfoCheck";     // 任意字符串，用于AlarmNotifyHandler找到各自的入口方法

                alarmNotify.Add(alarm);
            }

            if (alarmNotifyViewModel.ZgxcWaitingForApprovalLeaves.Count() > 0)
            {
                AlarmNotifyTable alarm = new AlarmNotifyTable();
                alarm.AlarmMessage = "你有未审批的请假申请, 请查看？";
                alarm.AlarmTag = "ZgxcAskForLeave";     // 任意字符串，用于AlarmNotifyHandler找到各自的入口方法

                alarmNotify.Add(alarm);
            }

            if (alarmNotifyViewModel.ZhzxEquipReq_NeedDdzDeals.Count() > 0 && CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_DDZ)
            {
                AlarmNotifyTable alarm = new AlarmNotifyTable();
                alarm.AlarmMessage = "你有未处理的办公用品审批, 请查看？";
                alarm.AlarmTag = "ZhzxEquipReq_NeedDdzDeals";     // 任意字符串，用于AlarmNotifyHandler找到各自的入口方法

                alarmNotify.Add(alarm);
            }

            if (alarmNotifyViewModel.KjkEquipReq_NeedDdzDeals.Count() > 0 && CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_DDZ)
            {
                AlarmNotifyTable alarm = new AlarmNotifyTable();
                alarm.AlarmMessage = "你有未处理的电脑相关用品审批, 请查看？";
                alarm.AlarmTag = "KjkEquipReq_NeedDdzDeals";     // 任意字符串，用于AlarmNotifyHandler找到各自的入口方法

                alarmNotify.Add(alarm);
            }

            if (CurrentLoginService.Instance.IsAuthorized("yctraffic.FzkReleaseCar.Approve"))
            {
                if (alarmNotifyViewModel.FzkReleaseCarFdzDeals.Count() > 0)
                {
                    AlarmNotifyTable alarm = new AlarmNotifyTable();
                    alarm.AlarmMessage = "你有未处理的法制科放车审批, 请查看？";
                    alarm.AlarmTag = "FzkReleaseCarFdz";     // 任意字符串，用于AlarmNotifyHandler找到各自的入口方法

                    alarmNotify.Add(alarm);
                }
            }


            return alarmNotify;
        }

        void SettingOper()
        {
            if (entityService.Entities.FrequentUsedLinks.Count() != 0)
            {
                frequentUsedLindConfigViewModel.FrequentlyUsedLink = entityService.QueryableFrequentUsedLinks.ToList()[0];
            }
            else
            {
                frequentUsedLindConfigViewModel.FrequentlyUsedLink = new FrequentUsedLink();
            }

            mainFrameViewModel.ContentView = frequentUsedLindConfigViewModel.View;
        }

        void SaveOper()
        {
            if (!ValueCheck())
            {
                return;
            }

            if (frequentUsedLindConfigViewModel.FrequentlyUsedLink.Id > 0)      //update
            {
                entityService.Entities.SaveChanges();
            }
            else      // insert
            {
                entityService.Entities.FrequentUsedLinks.AddObject(frequentUsedLindConfigViewModel.FrequentlyUsedLink);
                entityService.Entities.SaveChanges();
                alarmNotifyViewModel.FreqtUsdLnk = entityService.QueryableFrequentUsedLinks.ToList()[0];
            }

            mainFrameViewModel.ContentView = alarmNotifyViewModel.View;

            alarmNotifyViewModel.LoadFrequentUsedLinks();
        }

        void RetreatOper()
        {
            mainFrameViewModel.ContentView = alarmNotifyViewModel.View;
        }


        private bool ValueCheck()
        {
            if (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.InlineText1) &&
                string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri1) 
                )
            {
                MessageBox.Show("网站名称1不为空时，网址1为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.InlineText2) &&
                string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri2)
                )
            {
                MessageBox.Show("网站名称2不为空时，网址2为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.InlineText3) &&
                string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri3)
                )
            {
                MessageBox.Show("网站名称3不为空时，网址3为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.InlineText4) &&
                string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri4)
                )
            {
                MessageBox.Show("网站名称4不为空时，网址4为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.InlineText5) &&
                string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri5)
                )
            {
                MessageBox.Show("网站名称5不为空时，网址5为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.InlineText6) &&
                string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri6)
                )
            {
                MessageBox.Show("网站名称6不为空时，网址6为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.InlineText7) &&
                string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri7)
                )
            {
                MessageBox.Show("网站名称7不为空时，网址7为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.InlineText8) &&
                string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri8)
                )
            {
                MessageBox.Show("网站名称8不为空时，网址8为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.InlineText9) &&
                string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri9)
                )
            {
                MessageBox.Show("网站名称9不为空时，网址9为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.InlineText10) &&
                string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri10)
                )
            {
                MessageBox.Show("网站名称10不为空时，网址10为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                if ((!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri1) &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri1.Substring(0, 7) != "http://" &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri1.Substring(0, 8) != "https://") ||


                    (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri2) &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri2.Substring(0, 7) != "http://" &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri2.Substring(0, 8) != "https://") ||

                    (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri3) &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri3.Substring(0, 7) != "http://" &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri3.Substring(0, 8) != "https://") ||

                    (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri4) &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri4.Substring(0, 7) != "http://" &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri4.Substring(0, 8) != "https://") ||

                    (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri5) &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri5.Substring(0, 7) != "http://" &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri5.Substring(0, 8) != "https://") ||

                    (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri6) &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri6.Substring(0, 7) != "http://" &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri6.Substring(0, 8) != "https://") ||

                    (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri7) &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri7.Substring(0, 7) != "http://" &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri7.Substring(0, 8) != "https://") ||

                    (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri8) &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri8.Substring(0, 7) != "http://" &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri8.Substring(0, 8) != "https://") ||

                    (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri9) &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri9.Substring(0, 7) != "http://" &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri9.Substring(0, 8) != "https://") ||

                    (!string.IsNullOrEmpty(frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri10) &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri10.Substring(0, 7) != "http://" &&
                    frequentUsedLindConfigViewModel.FrequentlyUsedLink.NavigateUri10.Substring(0, 8) != "https://")
                    )
                {
                    MessageBox.Show("网址格式应以http://或https://开头", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("网址格式应以http://或https://开头", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;  
            }

            return true;
        }

    }
}
