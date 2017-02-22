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
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class LogbookMainController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private LogbookMainViewModel mLogbookCreateViewModel;

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand modifyCommand;
        private readonly DelegateCommand deleteCommand;

        private readonly DelegateCommand saveCommand;
        private List<BaseOrganizeEntity> departmentList;

        [ImportingConstructor]
        public LogbookMainController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.container = container;
                this.messageService = messageService;
                this.shellService = shellService;
                this.entityService = entityService;

                mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                mLogbookCreateViewModel = container.GetExportedValue<LogbookMainViewModel>();

                this.newCommand = new DelegateCommand(() => NewOper(), null);
                this.modifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.deleteCommand = new DelegateCommand(() => DeleteOper(), null);

                this.saveCommand = new DelegateCommand(() => Save(), null);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public void Initialize()
        {
            AddWeakEventListener(mLogbookCreateViewModel, LogbookCreateViewModel_PropertyChanged);
            InitDepartmentList();
        }

        private void InitDepartmentList()
        {
            try
            {
                departmentList = new List<BaseOrganizeEntity>();
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    BaseOrganizeManager origanizeService = new BaseOrganizeManager();
                    DataTable departmentDT = origanizeService.GetDepartmentDT("");
                    BaseOrganizeEntity entity;
                    foreach (DataRow dr in departmentDT.Rows)
                    {
                        entity = new BaseOrganizeEntity(dr);
                        departmentList.Add(entity);
                    }
                }
                else
                {
                    foreach (BaseOrganizeEntity entity in AuthService.Instance.FddZwChargeDepts)
                    {
                        departmentList.Add(entity);
                    }
                }

                //ALL
                BaseOrganizeEntity currentEntity = new BaseOrganizeEntity();
                currentEntity.Code = "";
                currentEntity.Id = 0;
                currentEntity.FullName = "全部";

                departmentList.Insert(0, currentEntity);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        void LogbookCreateViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedLogbook")
            {
                try
                {
                    string selectCode = string.Empty;
                    int? nodeLevel = 0;
                    if (mLogbookCreateViewModel.SelectedLogbook != null && mLogbookCreateViewModel.SelectedLogbook.Code != null)
                    {
                        selectCode = mLogbookCreateViewModel.SelectedLogbook.Code;
                        nodeLevel = mLogbookCreateViewModel.SelectedLogbook.NodeLevel;
                    }

                    if (nodeLevel != YcConstants.INT_LBCONFIG_NODE_LEVEL_2)
                    {
                        return;
                    }

                    if (this.departmentList == null)
                    {
                        InitDepartmentList();
                    }

                    string parentCode = string.Empty;
                    if (selectCode.Contains("."))
                    {
                        parentCode = selectCode.Substring(0, selectCode.IndexOf("."));
                    }

                    if (selectCode == "2000.1000")
                    {
                        /*
                        container.GetExportedValue<LbCyPunishController>().Initialize();
                        container.GetExportedValue<LbCyPunishQueryViewModel>().DepartmentList = this.departmentList;
                        container.GetExportedValue<LbCyPunishQueryViewModel>().IsFromAll = true;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbCyPunishQueryViewModel>().View;
                         */
                    }
                    else if (selectCode == "2000.1001")
                    {
                        container.GetExportedValue<LbCyPatrolController>().Initialize();
                        container.GetExportedValue<LbCyPatrolQueryViewModel>().DepartmentList = this.departmentList;
                        container.GetExportedValue<LbCyPatrolQueryViewModel>().IsFromAll = true;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbCyPatrolQueryViewModel>().View;
                    }
                    else if (selectCode == "2000.1002")
                    {
                        container.GetExportedValue<LbCyDangerDealController>().Initialize();
                        container.GetExportedValue<LbCyDangerDealQueryViewModel>().DepartmentList = this.departmentList;
                        container.GetExportedValue<LbCyDangerDealQueryViewModel>().IsFromAll = true;
                        container.GetExportedValue<LbCyDangerDealQueryViewModel>().IsAfterDangerDeal = false;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbCyDangerDealQueryViewModel>().View;
                    }
                    else if (selectCode == "2000.1003")
                    {
                        container.GetExportedValue<LbCyDangerDealController>().Initialize();
                        container.GetExportedValue<LbCyDangerDealQueryViewModel>().DepartmentList = this.departmentList;
                        container.GetExportedValue<LbCyDangerDealQueryViewModel>().IsFromAll = true;
                        container.GetExportedValue<LbCyDangerDealQueryViewModel>().IsAfterDangerDeal = true;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbCyDangerDealQueryViewModel>().View;
                    }
                    else
                    {
                        container.GetExportedValue<LbStaticLogbookController>().Initialize();
                        container.GetExportedValue<LbStaticLogbookQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbStaticLogbookQueryViewModel>().View;
                    }

                    /*
                    if (selectCode == "1000.1000")
                    {
                        container.GetExportedValue<LbZdStaffInfoController>().Initialize();
                        container.GetExportedValue<LbZdStaffInfoQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbZdStaffInfoQueryViewModel>().View;
                    }
                    if (selectCode == "1000.1001")
                    {
                        container.GetExportedValue<LbZdStaffController>().Initialize();
                        container.GetExportedValue<LbZdStaffQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbZdStaffQueryViewModel>().View;
                    }
                    if (selectCode == "1000.1002")
                    {
                        container.GetExportedValue<LbZdRosterController>().Initialize();
                        container.GetExportedValue<LbZdRosterQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbZdRosterQueryViewModel>().View;
                    }
                    if (selectCode == "1000.1003")
                    {
                        container.GetExportedValue<LbZdCarController>().Initialize();
                        container.GetExportedValue<LbZdCarQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbZdCarQueryViewModel>().View;
                    }
                    if (selectCode == "1000.1004")
                    {
                        container.GetExportedValue<LbZdGroupController>().Initialize();
                        container.GetExportedValue<LbZdGroupQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbZdGroupQueryViewModel>().View;
                    }
                    else if (selectCode == "1000.1005")
                    {
                        container.GetExportedValue<LbZdRotaController>().Initialize();
                        container.GetExportedValue<LbZdRotaQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbZdRotaQueryViewModel>().View;
                    }
                    else if (selectCode == "1000.1006")
                    {
                        container.GetExportedValue<LbZdPatrolRangeController>().Initialize();
                        container.GetExportedValue<LbZdPatrolRangeQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbZdPatrolRangeQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1000")
                    {
                        container.GetExportedValue<LbCyDangerDealController>().Initialize();
                        container.GetExportedValue<LbCyDangerDealQueryViewModel>().DepartmentList = this.departmentList;
                        container.GetExportedValue<LbCyDangerDealQueryViewModel>().IsFromAll = true;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbCyDangerDealQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1001")
                    {
                        container.GetExportedValue<LbYwSectionAssignController>().Initialize();
                        container.GetExportedValue<LbYwSectionAssignQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwSectionAssignQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1002")
                    {
                        container.GetExportedValue<LbYwSpecialRectificationController>().Initialize();
                        container.GetExportedValue<LbYwSpecialRectificationQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwSpecialRectificationQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1003")
                    {
                        container.GetExportedValue<LbYwTrafficAnalysisController>().Initialize();
                        container.GetExportedValue<LbYwTrafficAnalysisQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwTrafficAnalysisQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1004")
                    {
                        container.GetExportedValue<LbYwTrafficSignalController>().Initialize();
                        container.GetExportedValue<LbYwTrafficSignalQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwTrafficSignalQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1005")
                    {
                        container.GetExportedValue<LbYwKeyVehicleController>().Initialize();
                        container.GetExportedValue<LbYwKeyVehicleQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwKeyVehicleQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1006")
                    {
                        container.GetExportedValue<LbYwSchoolCarController>().Initialize();
                        container.GetExportedValue<LbYwSchoolCarQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwSchoolCarQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1007")
                    {
                        container.GetExportedValue<LbYwPropagandaController>().Initialize();
                        container.GetExportedValue<LbYwPropagandaQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwPropagandaQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1008")
                    {
                        container.GetExportedValue<LbYwPatrolRecordController>().Initialize();
                        container.GetExportedValue<LbYwPatrolRecordQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwPatrolRecordQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1010")
                    {
                        container.GetExportedValue<LbYwSendPoliceController>().Initialize();
                        container.GetExportedValue<LbYwSendPoliceQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwSendPoliceQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1011")
                    {
                        container.GetExportedValue<LbYwDriverController>().Initialize();
                        container.GetExportedValue<LbYwDriverQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwDriverQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1012")
                    {
                        container.GetExportedValue<LbYwInterviewController>().Initialize();
                        container.GetExportedValue<LbYwInterviewQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwInterviewQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1013")
                    {
                        container.GetExportedValue<LbYwOrderController>().Initialize();
                        container.GetExportedValue<LbYwOrderQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwOrderQueryViewModel>().View;
                    }
                    else if (selectCode == "1001.1014")
                    {
                        container.GetExportedValue<LbYwDUIRectificationController>().Initialize();
                        container.GetExportedValue<LbYwDUIRectificationQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbYwDUIRectificationQueryViewModel>().View;
                    }
                    else if (parentCode == "1002" || parentCode == "1003" || parentCode == "1004" || parentCode == "1005" || parentCode == "1006" || parentCode == "1007"
                        || parentCode == "1008" || selectCode == "1001.1009" || selectCode == "1002.1000" || selectCode == "1002.1001" || selectCode == "1002.1003"
                        || selectCode == "1002.1007" || selectCode == "1003.1003" || selectCode == "1003.1004" || selectCode == "1003.1005"
                        || selectCode == "1003.1006" || selectCode == "1003.1007")
                    {
                        container.GetExportedValue<LbStaticLogbookController>().Initialize();
                        container.GetExportedValue<LbStaticLogbookQueryViewModel>().DepartmentList = this.departmentList;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbStaticLogbookQueryViewModel>().View;
                    } 
                    else if (selectCode == "2000.1000")
                    {
                        container.GetExportedValue<LbCyPunishController>().Initialize();
                        container.GetExportedValue<LbCyPunishQueryViewModel>().DepartmentList = this.departmentList;
                        container.GetExportedValue<LbCyPunishQueryViewModel>().IsFromAll = true;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbCyPunishQueryViewModel>().View;
                    }
                    else if (selectCode == "2000.1001")
                    {
                        container.GetExportedValue<LbCyPatrolController>().Initialize();
                        container.GetExportedValue<LbCyPatrolQueryViewModel>().DepartmentList = this.departmentList;
                        container.GetExportedValue<LbCyPatrolQueryViewModel>().IsFromAll = true;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbCyPatrolQueryViewModel>().View;
                    }
                    else if (selectCode == "2000.1002")
                    {
                        container.GetExportedValue<LbCyDangerDealController>().Initialize();
                        container.GetExportedValue<LbCyDangerDealQueryViewModel>().DepartmentList = this.departmentList;
                        container.GetExportedValue<LbCyDangerDealQueryViewModel>().IsFromAll = true;
                        mLogbookCreateViewModel.ContentView = container.GetExportedValue<LbCyDangerDealQueryViewModel>().View;
                    }
                    */
                    //列表页
                    //mLogbookCreateViewModel.GridRefresh();
                }
                catch (System.Exception ex)
                {
                    messageService.ShowError(ex.Message);
                    CurrentLoginService.Instance.LogException(ex);
                }

            }
        }

        public bool NewOper()
        {
            bool newer = true;

            return newer;
        }

        public bool ModifyOper()
        {
            bool newer = true;

            return newer;
        }

        public bool DeleteOper()
        {            
            bool newer = true;


            return newer;
        }

        public bool Save()
        {
            bool saved = false;

            try
            {

            }
            catch (ValidationException ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
            catch (UpdateException ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return saved;
        }

    }
}
