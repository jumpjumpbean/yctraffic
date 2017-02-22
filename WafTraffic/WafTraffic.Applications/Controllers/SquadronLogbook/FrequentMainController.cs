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
    internal class FrequentMainController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private FrequentMainViewModel mFrequentMainViewModel;

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand modifyCommand;
        private readonly DelegateCommand deleteCommand;

        private readonly DelegateCommand saveCommand;
        private List<BaseOrganizeEntity> departmentList;

        [ImportingConstructor]
        public FrequentMainController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            this.container = container;
            this.messageService = messageService;
            this.shellService = shellService;
            this.entityService = entityService;

            mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
            mFrequentMainViewModel = container.GetExportedValue<FrequentMainViewModel>();

            this.newCommand = new DelegateCommand(() => NewOper(), null);
            this.modifyCommand = new DelegateCommand(() => ModifyOper(), null);
            this.deleteCommand = new DelegateCommand(() => DeleteOper(), null);

            this.saveCommand = new DelegateCommand(() => Save(), null);
        }

        public void Initialize()
        {
            AddWeakEventListener(mFrequentMainViewModel, FrequentCreateViewModel_PropertyChanged);
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

        void FrequentCreateViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedFrequent")
            {
                string selectCode = string.Empty;
                if (mFrequentMainViewModel.SelectedFrequent != null && mFrequentMainViewModel.SelectedFrequent.Code != null)
                {
                    selectCode = mFrequentMainViewModel.SelectedFrequent.Code;
                }

                if (this.departmentList == null)
                {
                    InitDepartmentList();
                }

                if (selectCode == "2000.1000")
                {
                    /*
                    container.GetExportedValue<LbCyPunishController>().Initialize();
                    container.GetExportedValue<LbCyPunishQueryViewModel>().DepartmentList = this.departmentList;
                    container.GetExportedValue<LbCyPunishQueryViewModel>().IsFromAll = false;
                    mFrequentMainViewModel.ContentView = container.GetExportedValue<LbCyPunishQueryViewModel>().View;
                     */
                }
                else if (selectCode == "2000.1001")
                {
                    container.GetExportedValue<LbCyPatrolController>().Initialize();
                    container.GetExportedValue<LbCyPatrolQueryViewModel>().DepartmentList = this.departmentList;
                    container.GetExportedValue<LbCyPatrolQueryViewModel>().IsFromAll = false;
                    mFrequentMainViewModel.ContentView = container.GetExportedValue<LbCyPatrolQueryViewModel>().View;
                }
                else if (selectCode == "2000.1002")
                {
                    container.GetExportedValue<LbCyDangerDealQueryViewModel>().IsAfterDangerDeal = false;
                    container.GetExportedValue<LbCyDangerDealController>().Initialize();
                    container.GetExportedValue<LbCyDangerDealQueryViewModel>().DepartmentList = this.departmentList;
                    container.GetExportedValue<LbCyDangerDealQueryViewModel>().IsFromAll = false;
                    mFrequentMainViewModel.ContentView = container.GetExportedValue<LbCyDangerDealQueryViewModel>().View;
                }
                else if (selectCode == "2000.1003")
                {
                    container.GetExportedValue<LbCyDangerDealQueryViewModel>().IsAfterDangerDeal = true;
                    container.GetExportedValue<LbCyDangerDealController>().Initialize();
                    container.GetExportedValue<LbCyDangerDealQueryViewModel>().DepartmentList = this.departmentList;
                    container.GetExportedValue<LbCyDangerDealQueryViewModel>().IsFromAll = false;
                    mFrequentMainViewModel.ContentView = container.GetExportedValue<LbCyDangerDealQueryViewModel>().View;
                }
                //列表页
                //mLogbookCreateViewModel.GridRefresh();
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
            catch (ValidationException e)
            {
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidEntities, e.Message));
            }
            catch (UpdateException e)
            {
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidFields, e.InnerException.Message));
            }

            return saved;
        }

    }
}
