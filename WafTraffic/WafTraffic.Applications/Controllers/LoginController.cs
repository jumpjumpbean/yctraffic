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
using DotNet.Utilities;
using System.Net;
using DotNet.Business;
using System.Windows.Forms;
using WafTraffic.Applications.Utils;

namespace WafTraffic.Applications.Controllers
{
    /// <summary>
    /// Responsible for the person management and the master / detail views.
    /// </summary>
    [Export]
    internal class LoginController : Controller
    {
        private readonly CompositionContainer container;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IShellService shellService;
        private readonly IEntityService entityService;
        private ShellViewModel shellViewModel;
        private readonly LoginViewModel loginViewModel;

        private readonly DelegateCommand loginCommand;

         [ImportingConstructor]
        public LoginController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService,
            IEntityService entityService, LoginViewModel loginViewModel)
        {
            this.container = container;
            this.messageService = messageService;
            this.shellService = shellService;
            this.entityService = entityService;
            this.shellViewModel = container.GetExportedValue<ShellViewModel>();

            this.loginViewModel = loginViewModel;

            this.loginCommand = new DelegateCommand(() => Login(), CanLogin);
        }

         public void Initialize()
         {
             //AddWeakEventListener(personViewModel, PersonViewModelPropertyChanged);
             loginViewModel.LoginCommand = this.loginCommand;
         }

         public bool CanLogin()
         {
             return true;
         }

         public bool Login()
         {
             bool loginSuccess = false;
             if (!CanLogin())
             {
                 throw new InvalidOperationException("没有登录权限.");
             }
             try
             {
                 BaseUserInfo tmpUser = new BaseUserInfo();

                 string returnStatusCode = string.Empty;
                 string returnStatusMessage = string.Empty;

                 string AddressIP = string.Empty;
                 foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                 {
                     if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                     {
                         AddressIP = _IPAddress.ToString();
                         break;
                     }
                 }
                 tmpUser.UserName = loginViewModel.UserName.Trim();
                 tmpUser.IPAddress = AddressIP;

                 // 统一的登录服务
                 DotNetService dotNetService = new DotNetService();
                 BaseUserInfo userInfo = dotNetService.LogOnService.UserLogOn(tmpUser, loginViewModel.UserName.Trim(), loginViewModel.Password.Trim(), false, out returnStatusCode, out returnStatusMessage);
                 // 检查身份
                 if (returnStatusCode.Equals(StatusCode.OK.ToString()))
                 {
                     if (VersionUpdateUtil.NeedUpdate()) //检查软件版本更新
                     {
                         DialogResult dlgResult = MessageBox.Show("发现新版本，点击确定后，系统会自动下载新版本！\n下载大概需要几分钟时间，请耐心等待！",
                                 "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                         if (dlgResult == DialogResult.OK)
                         {

                             bool tmpResult = VersionUpdateUtil.DownloadSoftware();
                             if (!tmpResult)
                             {
                                 MessageBox.Show("新版本下载失败，请联系管理员！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                 shellViewModel.Close();
                             }
                             else
                             {
                                 MessageBox.Show("新版本下载成功，请卸载旧版本后，再安装新版本！",
                                     "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                 shellViewModel.Close();
                             }
                         }
                         else
                         {
                             shellViewModel.Close();
                         }
                     }


                     loginSuccess = true;
                     CurrentLoginService.Instance.CurrentUserInfo = userInfo;
                     CurrentLoginService.Instance.InitDTPermission();
                     CurrentLoginService.Instance.InitModuleDT();
                     shellService.DocumentName = "登录成功，到主页";
                     //shellViewModel.ContentView = container.GetExportedValue<UserListViewModel>().View;
                     container.GetExportedValue<MainFrameController>().Initialize();
                     shellViewModel.SoftwareVersion = BaseSystemInfo.Version;
                     shellViewModel.ContentView = container.GetExportedValue<MainFrameViewModel>().View;
                     container.GetExportedValue<AlarmNotifyController>().RunAlarmNotifyTimer();
                     
                 }
                 else
                 {
                     MessageBox.Show("用户名或密码不正确！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     //throw new UpdateException("用户名或密码不正确！");
                 }
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
                 loginSuccess = false;
                 //errlog处理
                 CurrentLoginService.Instance.LogException(ex);
                 //throw ex;
                 messageService.ShowError(ex.Message);
             }
             
             return loginSuccess;
         }
    }
}
