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
using System.Windows.Forms;
using DotNet.Business;
using System.IO;
using WafTraffic.Applications.Common;
using DotNet.Utilities;
using System.Data.Common;
using WafTraffic.Applications.Utils;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class CgsKeyDriverLogbookController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mainFrameViewModel;
        private CgsKeyDriverLogbookUpdateViewModel logbookUpdateViewModel;
        private CgsKeyDriverLogbookQueryViewModel logbookQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mBrowseCommand;


        [ImportingConstructor]
        public CgsKeyDriverLogbookController(CompositionContainer container, 
            System.Waf.Applications.Services.IMessageService messageService, 
            IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.mContainer = container;
                this.mMessageService = messageService;
                this.mShellService = shellService;
                this.mEntityService = entityService;

                mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                logbookUpdateViewModel = container.GetExportedValue<CgsKeyDriverLogbookUpdateViewModel>();
                logbookQueryViewModel = container.GetExportedValue<CgsKeyDriverLogbookQueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public void Initialize()
        {
            try
            {
                logbookUpdateViewModel.SaveCommand = this.mSaveCommand;
                logbookUpdateViewModel.CancelCommand = this.mCancelCommand;

                logbookQueryViewModel.NewCommand = this.mNewCommand;
                logbookQueryViewModel.ModifyCommand = this.mModifyCommand;
                logbookQueryViewModel.DeleteCommand = this.mDeleteCommand;
                logbookQueryViewModel.QueryCommand = this.mQueryCommand;
                logbookQueryViewModel.BrowseCommand = this.mBrowseCommand;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool NewOper()
        {
            bool newer = true;
            try
            {
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;

                logbookUpdateViewModel.IsTitleReadOnly = false;
                logbookUpdateViewModel.CanGovCarEnable = true;

                logbookUpdateViewModel.KeyDriverLogbookEntity = new CgsKeyDriverLogbook();
                //logbookUpdateViewModel.KeyDriverLogbookEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                //logbookUpdateViewModel.KeyDriverLogbookEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(logbookUpdateViewModel.KeyDriverLogbookEntity.OwnDepartmentId);
                logbookUpdateViewModel.KeyDriverLogbookEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                logbookUpdateViewModel.KeyDriverLogbookEntity.CreateName = AuthService.Instance.GetUserNameById(logbookUpdateViewModel.KeyDriverLogbookEntity.CreateId);
                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
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

            try
            {
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;

                logbookUpdateViewModel.IsTitleReadOnly = false;
                logbookUpdateViewModel.CanGovCarEnable = true;

                logbookUpdateViewModel.KeyDriverLogbookEntity = logbookQueryViewModel.SelectedYellowMarkCar;
                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool DeleteOper()
        {
            bool deleter = false;
            FtpHelper ftp = null;
            try
            {
                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    ftp = new FtpHelper();

                    logbookQueryViewModel.SelectedYellowMarkCar.IsDeleted = true;
                    logbookQueryViewModel.SelectedYellowMarkCar.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookQueryViewModel.SelectedYellowMarkCar.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    logbookQueryViewModel.GridRefresh();
                }
                deleter = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误，删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            return deleter;
        }

        public bool CancelOper()
        {
            bool newer = true;

            try
            {
                mainFrameViewModel.ContentView = logbookQueryViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool BrowseOper()
        {
            bool newer = true;

            try
            {
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsTitleReadOnly = true;
                logbookUpdateViewModel.CanGovCarEnable = false;

                logbookUpdateViewModel.KeyDriverLogbookEntity = logbookQueryViewModel.SelectedYellowMarkCar;
                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool QueryOper()
        {
            bool newer = true;

            try
            {
                logbookQueryViewModel.CgsKeyDriverLogbooks = mEntityService.QueryCgsKeyDriverLogbooks.Where<CgsKeyDriverLogbook>
                (
                    entity =>
                        (entity.CreateTime.Value >= logbookQueryViewModel.StartDate)
                            &&
                        (entity.CreateTime.Value <= logbookQueryViewModel.EndDate)
                            &&
                        (string.IsNullOrEmpty(logbookQueryViewModel.KeyWord) ? true : (entity.DriverName.Contains(logbookQueryViewModel.KeyWord)))
                            &&
                        (entity.IsDeleted == false)
                );

                if (logbookQueryViewModel.CgsKeyDriverLogbooks == null || logbookQueryViewModel.CgsKeyDriverLogbooks.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                logbookQueryViewModel.GridRefresh();
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
                if (logbookUpdateViewModel.KeyDriverLogbookEntity.Id > 0)
                {
                    logbookUpdateViewModel.KeyDriverLogbookEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.KeyDriverLogbookEntity.UpdateTime = System.DateTime.Now;
                }
                else
                {
                    //logbookUpdateViewModel.KeyDriverLogbookEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    logbookUpdateViewModel.KeyDriverLogbookEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.KeyDriverLogbookEntity.CreateTime = System.DateTime.Now;
                    logbookUpdateViewModel.KeyDriverLogbookEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.KeyDriverLogbookEntity.UpdateTime = System.DateTime.Now;
                    logbookUpdateViewModel.KeyDriverLogbookEntity.IsDeleted = false;

                    mEntityService.Entities.CgsKeyDriverLogbooks.AddObject(logbookUpdateViewModel.KeyDriverLogbookEntity);
                }
                mEntityService.Entities.SaveChanges();

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            mainFrameViewModel.ContentView = logbookQueryViewModel.View;

            return saver;
        }


        private bool ValueCheck()
        {
            bool result = true;

            if (ValidateUtil.IsBlank(logbookUpdateViewModel.KeyDriverLogbookEntity.DriverName)
                || ValidateUtil.IsBlank(logbookUpdateViewModel.KeyDriverLogbookEntity.LicenseNo)
                || ValidateUtil.IsBlank(logbookUpdateViewModel.KeyDriverLogbookEntity.PermitDriveType)
                || ValidateUtil.IsBlank(logbookUpdateViewModel.KeyDriverLogbookEntity.Status))
            {
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
                return result;
            }

            return result;
        }
    }
}
    