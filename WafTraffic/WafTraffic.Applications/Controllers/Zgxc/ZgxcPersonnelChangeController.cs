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
    internal class ZgxcPersonnelChangeController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mainFrameViewModel;
        private ZgxcPersonnelChangeUpdateViewModel logbookUpdateViewModel;
        private ZgxcPersonnelChangeQueryViewModel logbookQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand approveCommand;
        private readonly DelegateCommand signCommand;
        private readonly DelegateCommand archiveCommand;

        short optionCode;   // browse --0  New --1 Modify --2 Approve -- 3 Sign  -- 4 Archive -- 5

        [ImportingConstructor]
        public ZgxcPersonnelChangeController(CompositionContainer container, 
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
                logbookUpdateViewModel = container.GetExportedValue<ZgxcPersonnelChangeUpdateViewModel>();
                logbookQueryViewModel = container.GetExportedValue<ZgxcPersonnelChangeQueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.approveCommand = new DelegateCommand(() => ApproveOper(), null);
                this.signCommand = new DelegateCommand(() => SignOper(), null);
                this.archiveCommand = new DelegateCommand(() => ArchiveOper(), null);
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
                logbookQueryViewModel.ApproveCommand = this.approveCommand;
                logbookQueryViewModel.SignCommand = this.signCommand;
                logbookQueryViewModel.ArchiveCommand = this.archiveCommand;
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
                optionCode = 1;
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanSignVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanArchiveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsNameReadOnly = false;
                logbookUpdateViewModel.CanPersonStatusEdit = true;
                logbookUpdateViewModel.IsCommentsReadOnly = true;

                logbookUpdateViewModel.PersonnelChangeEntity = new ZgxcPersonnelChange();
                logbookUpdateViewModel.PersonnelChangeEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                logbookUpdateViewModel.PersonnelChangeEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(logbookUpdateViewModel.PersonnelChangeEntity.OwnDepartmentId);
                logbookUpdateViewModel.PersonnelChangeEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                logbookUpdateViewModel.PersonnelChangeEntity.CreateName = AuthService.Instance.GetUserNameById(logbookUpdateViewModel.PersonnelChangeEntity.CreateId);
                logbookUpdateViewModel.PersonnelChangeEntity.CreateTime = System.DateTime.Now;

                logbookUpdateViewModel.PersonnelChangeEntity.RecordStatus = "待审核";

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
                optionCode = 2;

                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanSignVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanArchiveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsNameReadOnly = false;
                logbookUpdateViewModel.CanPersonStatusEdit = true;
                logbookUpdateViewModel.IsCommentsReadOnly = true;


                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.ZgxcPersonnelChange.Approve"))
                {
                    logbookUpdateViewModel.IsNameReadOnly = true;
                    logbookUpdateViewModel.CanPersonStatusEdit = false;
                    logbookUpdateViewModel.IsCommentsReadOnly = false;
                }

                logbookUpdateViewModel.PersonnelChangeEntity = logbookQueryViewModel.SelectedConsultationLogbook;
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
            try
            {
                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

                    logbookQueryViewModel.SelectedConsultationLogbook.IsDeleted = true;
                    logbookQueryViewModel.SelectedConsultationLogbook.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookQueryViewModel.SelectedConsultationLogbook.UpdateTime = System.DateTime.Now;
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
                optionCode = 0;

                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanSignVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanArchiveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsNameReadOnly = true;
                logbookUpdateViewModel.CanPersonStatusEdit = false;
                logbookUpdateViewModel.IsCommentsReadOnly = false;

                logbookUpdateViewModel.PersonnelChangeEntity = logbookQueryViewModel.SelectedConsultationLogbook;
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
                logbookQueryViewModel.ZgxcPersonnelChanges = mEntityService.QueryZgxcPersonnelChanges.Where<ZgxcPersonnelChange>
                (
                    entity =>
                        (entity.CreateTime.Value >= logbookQueryViewModel.StartDate)
                            &&
                        (entity.CreateTime.Value <= logbookQueryViewModel.EndDate)
                            &&
                        (string.IsNullOrEmpty(logbookQueryViewModel.KeyWord) ? true : (entity.PersonName.Contains(logbookQueryViewModel.KeyWord)))
                            &&
                        (entity.IsDeleted == false)
                );

                if (logbookQueryViewModel.ZgxcPersonnelChanges == null || logbookQueryViewModel.ZgxcPersonnelChanges.Count() == 0)
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

        public bool ApproveOper()
        {
            bool dealer = true;

            try
            {
                optionCode = 3;

                logbookUpdateViewModel.IsNameReadOnly = true;
                logbookUpdateViewModel.CanPersonStatusEdit = false;
                logbookUpdateViewModel.IsCommentsReadOnly = false;

                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanSignVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanArchiveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsNameReadOnly = false;

                logbookUpdateViewModel.PersonnelChangeEntity = logbookQueryViewModel.SelectedConsultationLogbook;
                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return dealer;
        }

        public bool SignOper()
        {
            bool dealer = true;

            try
            {
                optionCode = 4;

                logbookUpdateViewModel.IsNameReadOnly = true;
                logbookUpdateViewModel.CanPersonStatusEdit = false;
                logbookUpdateViewModel.IsCommentsReadOnly = false;

                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanSignVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanArchiveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsNameReadOnly = false;

                logbookUpdateViewModel.PersonnelChangeEntity = logbookQueryViewModel.SelectedConsultationLogbook;
                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return dealer;
        }

        public bool ArchiveOper()
        {
            bool dealer = true;

            try
            {
                optionCode = 5;

                logbookUpdateViewModel.IsNameReadOnly = true;
                logbookUpdateViewModel.CanPersonStatusEdit = false;
                logbookUpdateViewModel.IsCommentsReadOnly = false;

                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanSignVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanArchiveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsNameReadOnly = false;

                logbookUpdateViewModel.PersonnelChangeEntity = logbookQueryViewModel.SelectedConsultationLogbook;
                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return dealer;
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
                if (logbookUpdateViewModel.PersonnelChangeEntity.Id > 0)
                {
                    logbookUpdateViewModel.PersonnelChangeEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.PersonnelChangeEntity.UpdateTime = System.DateTime.Now;
                }
                else
                {
                    logbookUpdateViewModel.PersonnelChangeEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    logbookUpdateViewModel.PersonnelChangeEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.PersonnelChangeEntity.CreateTime = System.DateTime.Now;
                    logbookUpdateViewModel.PersonnelChangeEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.PersonnelChangeEntity.UpdateTime = System.DateTime.Now;
                    logbookUpdateViewModel.PersonnelChangeEntity.IsDeleted = false;

                    mEntityService.Entities.ZgxcPersonnelChanges.AddObject(logbookUpdateViewModel.PersonnelChangeEntity);
                }

                BaseOrganizeEntity org = logbookUpdateViewModel.DepartmentList.Find(
                    instance => (instance.Id == logbookUpdateViewModel.PersonnelChangeEntity.PersonDepartmentId)
                );

                if (org != null)
                {
                    logbookUpdateViewModel.PersonnelChangeEntity.PersonDepartmentName = org.FullName;
                }

                if (optionCode == 1)
                {
                    logbookUpdateViewModel.PersonnelChangeEntity.RecordStatus = "待审核";
                }
                else if (optionCode == 3)
                {   
                    logbookUpdateViewModel.PersonnelChangeEntity.RecordStatus = "已审核";
                }
                else if (optionCode == 4)
                {
                    logbookUpdateViewModel.PersonnelChangeEntity.RecordStatus = "已签收";
                }
                else if (optionCode == 5)
                {
                    logbookUpdateViewModel.PersonnelChangeEntity.RecordStatus = "已归档";
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

            if (ValidateUtil.IsBlank(logbookUpdateViewModel.PersonnelChangeEntity.PersonName)
                || (logbookUpdateViewModel.PersonnelChangeEntity.PersonDepartmentId == 0)
                || ValidateUtil.IsBlank(logbookUpdateViewModel.PersonnelChangeEntity.PersonStatus))
            {
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
                return result;
            }

            return result;
        }
    }
}
    