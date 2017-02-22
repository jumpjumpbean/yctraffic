using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using WafTraffic.Applications.Properties;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Services;
using System.ComponentModel.Composition.Hosting;

namespace WafTraffic.Applications.Controllers
{
    /// <summary>
    /// Responsible for the module lifecycle.
    /// 
    /// [Export(typeof(IModuleController)), Export]
    /// </summary>
    [Export(typeof(IModuleController))]
    internal class ModuleController : Controller, IModuleController
    {
        private readonly IMessageService messageService;
        private readonly IEntityController entityController;
        private readonly IShellService shellService;
        private readonly ShellViewModel shellViewModel;
        private readonly DelegateCommand exitCommand;
        private readonly CompositionContainer container;

        //private readonly UserController userController;
        //private readonly UserListViewModel userListViewModel;
        private readonly LoginViewModel loginViewModel;
        private readonly LoginController loginController;
        //private MonthRegisterController monthRegisterController;
        private LbStaticLogbookController lbStaticLogbookController;
        //private ZhzxTrafficViolationController zhzxTrafficViolationController;
        private SgkReleaseCarController sgkReleaseCarController;
        private FzkReleaseCarController fzkReleaseCarController;


        [ImportingConstructor]
        public ModuleController(CompositionContainer container, IMessageService messageService, IPresentationService presentationService, 
            IEntityController entityController, LoginController loginController, ShellService shellService)
        {
            presentationService.InitializeCultures();

            this.container = container;
            this.messageService = messageService;
            this.shellService = shellService;

            this.entityController = container.GetExportedValue<IEntityController>();
            this.loginController = container.GetExportedValue<LoginController>();
           // this.userController = container.GetExportedValue<UserController>();
            

            this.shellViewModel = container.GetExportedValue<ShellViewModel>();
            this.loginViewModel = container.GetExportedValue<LoginViewModel>();
            
            

            shellService.ShellView = shellViewModel.View;

            //this.shellViewModel.Closing += ShellViewModelClosing;
            this.exitCommand = new DelegateCommand(Close);
        }


        public void Initialize()
        {
            //shellViewModel.ExitCommand = exitCommand;
            entityController.Initialize();
            loginController.Initialize();
            //userController.Initialize(); // don't Init userController, load after if call UserListView 
        }

        public void Run()
        {
            shellViewModel.CanAlarmShow = System.Windows.Visibility.Collapsed;
            shellViewModel.CanAlarmNotifyShow = System.Windows.Visibility.Collapsed;
            shellViewModel.ContentView = loginViewModel.View;
            shellViewModel.Show();
        }

        public void Shutdown()
        {
            entityController.Shutdown();

            try
            {

                sgkReleaseCarController = container.GetExportedValue<SgkReleaseCarController>();
                fzkReleaseCarController = container.GetExportedValue<FzkReleaseCarController>();
                this.lbStaticLogbookController = container.GetExportedValue<LbStaticLogbookController>();

                sgkReleaseCarController.Close();
                fzkReleaseCarController.Close();
                lbStaticLogbookController.Close();

                Settings.Default.Save();
            }
            catch (Exception)
            {
                // When more application instances are closed at the same time then an exception occurs.
            }
        }

        private void ShellViewModelClosing(object sender, CancelEventArgs e)
        {
            if (entityController.HasChanges)
            {
                if (entityController.CanSave())
                {
                    bool? result = messageService.ShowQuestion(shellService.ShellView, Resources.SaveChangesQuestion);
                    if (result == true)
                    {
                        if (!entityController.Save())
                        {
                            e.Cancel = true;
                        }
                    }
                    else if (result == null)
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = !messageService.ShowYesNoQuestion(shellService.ShellView, Resources.LoseChangesQuestion);
                }
            }
        }

        private void Close()
        {
            shellViewModel.Close();
        }
    }
}
