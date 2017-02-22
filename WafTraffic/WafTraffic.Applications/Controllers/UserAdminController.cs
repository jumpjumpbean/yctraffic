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
using DotNet.Utilities;
using System.Windows.Forms;
using WafTraffic.Applications.Utils;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class UserAdminController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private UserAdminViewModel userAdminViewModel;
        private UserEditViewModel userEditViewModel;

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand modifyCommand;
        private readonly DelegateCommand deleteCommand;
        private readonly DelegateCommand saveCommand;
        private readonly DelegateCommand retreatCommand;

        [ImportingConstructor]
        public UserAdminController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            this.container = container;
            this.messageService = messageService;
            this.shellService = shellService;
            this.entityService = entityService;

            mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
            userAdminViewModel = container.GetExportedValue<UserAdminViewModel>();
            userEditViewModel = container.GetExportedValue<UserEditViewModel>();

            this.newCommand = new DelegateCommand(() => NewOper(), null);
            this.modifyCommand = new DelegateCommand(() => ModifyOper(), null);
            this.deleteCommand = new DelegateCommand(() => DeleteOper(), null);
            this.saveCommand = new DelegateCommand(() => Save(), null);
            this.retreatCommand = new DelegateCommand(() => RetreatOper(), null);
        }

        public void Initialize()
        {
            userAdminViewModel.NewCommand = this.newCommand;
            userAdminViewModel.ModifyCommand = this.modifyCommand;
            userAdminViewModel.DeleteCommand = this.deleteCommand;

            userEditViewModel.SaveCommand = this.saveCommand;
            userEditViewModel.RetreatCommand = this.retreatCommand;
        }

        public bool NewOper()
        {
            bool newer = true;
            userEditViewModel.User = new BaseUserEntity();
            mainFrameViewModel.ContentView = userEditViewModel.View;
          
            return newer;
        }

        public bool ModifyOper()
        {
            bool newer = true;
            userEditViewModel.User = userAdminViewModel.SelectedUser;
            mainFrameViewModel.ContentView = userEditViewModel.View;
            return newer;
        }

        public bool RetreatOper()
        {
            bool newer = true;
            if (userAdminViewModel.SelectedUser != null && userAdminViewModel.SelectedUser.Id != 0)
            {
                //entityService.Entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, userAdminViewModel.SelectedUser);
                userAdminViewModel.SelectedUser = null;
            }
            mainFrameViewModel.ContentView = userAdminViewModel.View;
            return newer;
        }

        public bool DeleteOper()
        {
            bool newer = true;
            try
            {
                int i = DotNetService.Instance.UserService.Delete(CurrentLoginService.Instance.CurrentUserInfo, Convert.ToString(userAdminViewModel.SelectedUser.Id));
                if (i > 0)
                {
                    //删除成功也要更新一下数据源
                    List<BaseUserEntity> users = new List<BaseUserEntity>();
                    DataTable dt = DotNetService.Instance.UserService.GetDT(CurrentLoginService.Instance.CurrentUserInfo);
                    foreach (DataRow dr in dt.Rows)
                    {
                        users.Add(new BaseUserEntity(dr));
                    }
                    userAdminViewModel.Users = users;

                    //刷新DataGrid
                    userAdminViewModel.GridRefresh();

                    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "删除成功."));
                }
                else
                {
                    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "删除失败."));
                }
            }
            catch (System.Exception ex)
            {
                newer = false;
                //errlog处理
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool Save()
        {
            bool saved = false;
            string strMessage = string.Empty;
            string strCode = string.Empty;
            string oldFile = string.Empty;
            FtpHelper ftp = null;

            if (!ValueCheck())
            {
                return saved;
            }

            try
            {
                ftp = new FtpHelper();
                if (userEditViewModel.User.CompanyId != null)
                {
                    userEditViewModel.User.CompanyName = userEditViewModel.CompanyList.Find(entity => entity.Id == userEditViewModel.User.CompanyId).FullName;
                }

                if (userEditViewModel.User.DepartmentId != null)
                {
                    userEditViewModel.User.DepartmentName = userEditViewModel.DepartmentList.Find(entity => entity.Id == userEditViewModel.User.DepartmentId).FullName;
                }

                if (userEditViewModel.User.Id <= 0 || userEditViewModel.User.Id == null)
                {

                    if (!string.IsNullOrEmpty(userEditViewModel.SignPath))
                    {
                        userEditViewModel.User.AnswerQuestion = ftp.UploadFile(FtpFileType.Picture, userEditViewModel.SignPath);
                        if (string.IsNullOrEmpty(userEditViewModel.User.AnswerQuestion))
                        {
                            throw new ValidationException();
                        }
                    }
                    string strId = DotNetService.Instance.UserService.AddUser(CurrentLoginService.Instance.CurrentUserInfo, userEditViewModel.User, out strCode, out strMessage);
                    if (strCode == StatusCode.OK.ToString())
                    {
                        userEditViewModel.User.Id = Convert.ToInt32(strId);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(userEditViewModel.SignPath))
                    {
                        oldFile = userEditViewModel.User.AnswerQuestion;
                        userEditViewModel.User.AnswerQuestion = ftp.UpdateFile(FtpFileType.Picture, userEditViewModel.SignPath, oldFile);
                        if (string.IsNullOrEmpty(userEditViewModel.User.AnswerQuestion))
                        {
                            throw new ValidationException();
                        }
                    }

                    int affected = DotNetService.Instance.UserService.UpdateUser(CurrentLoginService.Instance.CurrentUserInfo, userEditViewModel.User, out strCode, out strMessage);
                }
                if (strCode == StatusCode.OKAdd.ToString() || strCode == StatusCode.OKUpdate.ToString())
                {//新增或修改成功
                    List<BaseUserEntity>  users = new List<BaseUserEntity>();
                    DataTable dt = DotNetService.Instance.UserService.GetDT(CurrentLoginService.Instance.CurrentUserInfo);
                    foreach (DataRow dr in dt.Rows)
                    {
                        users.Add(new BaseUserEntity(dr));
                    }
                    userAdminViewModel.Users = users;
                }

                messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, strMessage));

            }
            catch (System.Exception ex)
            {
                saved = false;
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //errlog处理
                CurrentLoginService.Instance.LogException(ex);
            }
            finally
            {
                if (ftp != null)
                {
                    ftp.Disconnect();
                    ftp = null;
                }
            }

            return saved;
        }

        private bool ValueCheck()
        {
            bool result = false;
            try
            {
                if (string.IsNullOrEmpty(userEditViewModel.User.UserName))
                {
                    MessageBox.Show("用户名不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }
                if (string.IsNullOrEmpty(userEditViewModel.User.RealName))
                {
                    MessageBox.Show("姓名不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }
                if (string.IsNullOrEmpty(userEditViewModel.User.UserPassword))
                {
                    MessageBox.Show("密码不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }
                if (userEditViewModel.User.CompanyId == null || userEditViewModel.User.CompanyId == 0)
                {
                    MessageBox.Show("单位不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }
                if (userEditViewModel.User.DepartmentId == null || userEditViewModel.User.DepartmentId == 0)
                {
                    MessageBox.Show("部门不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }
                if (userEditViewModel.User.RoleId == null || userEditViewModel.User.RoleId == 0)
                {
                    MessageBox.Show("默认角色不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }
                if(AuthService.Instance.IsLeader(userEditViewModel.User.DepartmentId)
                    && string.IsNullOrEmpty(userEditViewModel.User.AnswerQuestion)
                    && string.IsNullOrEmpty(userEditViewModel.SignPath))
                {
                    MessageBox.Show("大队领导电子签名不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }
                result = true;
            }
            catch (System.Exception ex)
            {
                result = false;
                CurrentLoginService.Instance.LogException(ex);
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
    }
}
    