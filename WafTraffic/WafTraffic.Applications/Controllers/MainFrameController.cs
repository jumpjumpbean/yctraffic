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
    internal class MainFrameController : Controller
    {
        private readonly CompositionContainer container;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly HotLineController hotLineController;

        private MainFrameViewModel mainFrameViewModel;
        private ShellViewModel shellViewModel;


        [ImportingConstructor]
        public MainFrameController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService)
        {
            this.container = container;
            this.messageService = messageService;
            //userController = container.GetExportedValue<UserController>();
            hotLineController = container.GetExportedValue<HotLineController>();

            this.mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
            shellViewModel = container.GetExportedValue<ShellViewModel>();

        }

        public void Initialize()
        {
            AddWeakEventListener(mainFrameViewModel, MainFrameViewModelPropertyChanged);
            //userController.Initialize();
            hotLineController.Initialize();

            //mainFrameViewModel.ContentView = container.GetExportedValue<UserViewModel>().View; //显示主页
            if (mainFrameViewModel.Modules != null && mainFrameViewModel.Modules.Count() > 0)
            {
                mainFrameViewModel.SelectModule = mainFrameViewModel.Modules.First<BaseModuleEntity>(); //登录后默认选择第一项   
            }

            shellViewModel.CanAlarmShow = System.Windows.Visibility.Visible;   // 控制status bar上梯形图标的显示
        }

        private void MainFrameViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectModule")
            {
                //mainFrameViewModel.ContentView = null;  //切换到空白页
                mainFrameViewModel.SubModules = CurrentLoginService.Instance.GetSecModuleList(mainFrameViewModel.SelectModule.Id.ToString());
                if (mainFrameViewModel.SubModules != null && mainFrameViewModel.SubModules.Count()>0 )
                {
                    mainFrameViewModel.SelectSubModule = mainFrameViewModel.SubModules.First<BaseModuleEntity>(); //初始选择第一个子菜单
                }
                
                //UpdateCommands();
            }
            else if (e.PropertyName == "SelectSubModule" && mainFrameViewModel.SelectSubModule != null)
            {
                if (mainFrameViewModel.SelectSubModule.Code == "100.100.100")
                {
                    container.GetExportedValue<HotLineController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<HotLineListViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.100.101")
                {
                    container.GetExportedValue<PersonArchiveController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<PersonArchiveListViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.100.102")
                {
                    container.GetExportedValue<HealthArchiveController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<HealthArchiveListViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == YcConstantTable.MaterialDeclare_CODE)
                {
                    container.GetExportedValue<MaterialDeclareController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<MaterialDeclareViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.100.103")
                {
                    container.GetExportedValue<MonthRegisterController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<MonthRegisterListViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.100.105")
                {
                    container.GetExportedValue<ZgxcPublicityLogbookController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZgxcPublicityLogbookQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.100.106")
                {
                    container.GetExportedValue<ZgxcAssistantCheckinController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZgxcAssistantCheckinQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.100.107")
                {
                    container.GetExportedValue<ZgxcPersonnelChangeController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZgxcPersonnelChangeQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.100.108")
                {
                    container.GetExportedValue<ZgxcAskForLeaveController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZgxcAskForLeaveQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.101.100")
                {
                    container.GetExportedValue<FrequentMainController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<FrequentMainViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.101.101")
                {
                    container.GetExportedValue<LogbookMainController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<LogbookMainViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.101.102")
                {
                    container.GetExportedValue<LbConfigController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<LbConfigQueryViewModel>().View;
                }   
                else if (mainFrameViewModel.SelectSubModule.Code == "100.990.300")
                {
                    container.GetExportedValue<UserAdminController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<UserAdminViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.110.100")
                {
                    //string mapURL = DotNet.Utilities.UserConfigHelper.GetValue("MapURL");
                    //System.Diagnostics.Process.Start(mapURL); 

                    //container.GetExportedValue<YuChangMapController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<YuChengMapViewModel>().View;

                }  
                else if (mainFrameViewModel.SelectSubModule.Code == YcConstantTable.AlarmNotify_CODE)
                {
                    container.GetExportedValue<AlarmNotifyController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<AlarmNotifyViewModel>().View;         
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.108.100")
                {
                    container.GetExportedValue<ZhzxTrafficViolationController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZhzxTrafficViolationViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.106.100")
                {
                    container.GetExportedValue<KjkRequestController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<KjkRequestQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.108.101")
                {
                    container.GetExportedValue<ZhzxRequestController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZhzxRequestQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.108.102")
                {
                    container.GetExportedValue<ZhzxFilterController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZhzxFilterQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.108.103")
                {
                    container.GetExportedValue<ZhzxElectronMonitorController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZhzxElectronMonitorListViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.108.104")
                {
                    container.GetExportedValue<ZhzxOfficeSupplyStockController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZhzxOfficeSupplyStockListViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.108.105")
                {
                    container.GetExportedValue<ZhzxFixedAssetsRegisterController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZhzxFixedAssetsRegisterListViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.108.106")
                {
                    container.GetExportedValue<ZhzxTotalViolationController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZhzxTotalViolationListViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.108.107")
                {
                    container.GetExportedValue<ZhzxTrafficViolationController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZhzxFakePlateNumberViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.102.100")
                {
                    container.GetExportedValue<GggsPublishNoticeController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<GggsPublishNoticeListViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.103.100")
                {
                    container.GetExportedValue<ZxkOrderController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<ZxkOrderQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.104.100")
                {
                    container.GetExportedValue<FzkPunishController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<FzkPunishQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.104.101")
                {
                    container.GetExportedValue<FzkPetitionController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<FzkPetitionQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.104.102")
                {
                    container.GetExportedValue<FzkChangeMeasureController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<FzkChangeMeasureQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.104.103")
                {
                    container.GetExportedValue<FzkConsultationController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<FzkConsultationQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.104.104")
                {
                    container.GetExportedValue<FzkReleaseCarController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<FzkReleaseCarQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.104.105")
                {
                    container.GetExportedValue<FzkLawQualityLogbookController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<FzkLawQualityLogbookQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.105.100")
                {
                    container.GetExportedValue<SgkAccidentController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<SgkAccidentQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.105.101")
                {
                    container.GetExportedValue<SgkReleaseCarController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<SgkReleaseCarQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.107.100")
                {
                    container.GetExportedValue<QbyqInfoAnalysisController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<QbyqInfoAnalysisQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.109.100")
                {
                    container.GetExportedValue<CgsVehicleAdminController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<CgsVehicleAdminQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.109.101")
                {
                    container.GetExportedValue<CgsYellowMarkCarController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<CgsYellowMarkCarQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.109.102")
                {
                    container.GetExportedValue<CgsKeyVehicleLogbookController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<CgsKeyVehicleLogbookQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.109.103")
                {
                    container.GetExportedValue<CgsKeyDriverLogbookController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<CgsKeyDriverLogbookQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.109.104")
                {
                    container.GetExportedValue<CgsKeyCompanyLogbookController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<CgsKeyCompanyLogbookQueryViewModel>().View;
                }
                else if (mainFrameViewModel.SelectSubModule.Code == "100.111.100")
                {
                    container.GetExportedValue<SskRequestController>().Initialize();
                    mainFrameViewModel.ContentView = container.GetExportedValue<SskRequestQueryViewModel>().View;
                }
                else
                {
                    mainFrameViewModel.ContentView = null;
                }
            }
            else if (e.PropertyName == "ContentView")
            {
                shellViewModel.CloseMyImage();
            }
        }

    }
}
