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
using System.Windows.Forms;
using WafTraffic.Applications.Common;
using DotNet.Utilities;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class LbZdStaffInfoController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private LogbookMainViewModel mLogbookMainViewModel;
        private LbZdStaffInfoUpdateViewModel mStaffInfoViewModel;
        private LbZdStaffInfoQueryViewModel mStaffInfoQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;

        [ImportingConstructor]
        public LbZdStaffInfoController(CompositionContainer container, 
            System.Waf.Applications.Services.IMessageService messageService, 
            IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.mContainer = container;
                this.mMessageService = messageService;
                this.mShellService = shellService;
                this.mEntityService = entityService;

                mLogbookMainViewModel = container.GetExportedValue<LogbookMainViewModel>();
                mStaffInfoViewModel = container.GetExportedValue<LbZdStaffInfoUpdateViewModel>();
                mStaffInfoQueryViewModel = container.GetExportedValue<LbZdStaffInfoQueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
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
                mStaffInfoViewModel.SaveCommand = this.mSaveCommand;
                mStaffInfoViewModel.CancelCommand = this.mCancelCommand;

                mStaffInfoQueryViewModel.NewCommand = this.mNewCommand;
                mStaffInfoQueryViewModel.ModifyCommand = this.mModifyCommand;
                mStaffInfoQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mStaffInfoQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mStaffInfoQueryViewModel.QueryCommand = this.mQueryCommand;
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
                mStaffInfoViewModel.IsBrowse = false;
                mStaffInfoViewModel.StaffInfoEntity = new ZdtzZdStaffInfo();
                mLogbookMainViewModel.ContentView = mStaffInfoViewModel.View;
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
                mStaffInfoViewModel.IsBrowse = false;
                mStaffInfoViewModel.StaffInfoEntity = mStaffInfoQueryViewModel.SelectedStaffInfo;
                mLogbookMainViewModel.ContentView = mStaffInfoViewModel.View;
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
                mStaffInfoViewModel.IsBrowse = true;
                mStaffInfoViewModel.StaffInfoEntity = mStaffInfoQueryViewModel.SelectedStaffInfo;
                mLogbookMainViewModel.ContentView = mStaffInfoViewModel.View;
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
                    mStaffInfoQueryViewModel.SelectedStaffInfo.IsDeleted = YcConstants.INT_DB_DATA_DELETED;
                    mStaffInfoQueryViewModel.SelectedStaffInfo.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mStaffInfoQueryViewModel.SelectedStaffInfo.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    mStaffInfoQueryViewModel.GridRefresh();
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
                if (mStaffInfoQueryViewModel.SelectedStaffInfo != null && mStaffInfoQueryViewModel.SelectedStaffInfo.Id != 0)
                {
                    mEntityService.Entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, mStaffInfoQueryViewModel.SelectedStaffInfo);
                }
                mLogbookMainViewModel.ContentView = mStaffInfoQueryViewModel.View;
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
            int deptId;
            try
            {
                if (mStaffInfoQueryViewModel.IsSelectDepartmentEnabled)
                {
                    deptId = Convert.ToInt32(mStaffInfoQueryViewModel.SelectedDepartment);
                }
                else
                {
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }

                DateTime startDate = (DateTime)mStaffInfoQueryViewModel.SelectedStartDate;
                DateTime endDate = (DateTime)mStaffInfoQueryViewModel.SelectedEndDate;

                mStaffInfoQueryViewModel.LbStaffinfos = mEntityService.QueryStaffInfos(deptId, startDate, endDate);
                if (mStaffInfoQueryViewModel.LbStaffinfos == null || mStaffInfoQueryViewModel.LbStaffinfos.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mStaffInfoQueryViewModel.GridRefresh();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool Save()
        {
            bool saved = false;

            try
            {
                if (!ValueCheck())
                {
                    return saved;
                }
                if (mStaffInfoViewModel.StaffInfoEntity.Id > 0)
                {
                    mStaffInfoViewModel.StaffInfoEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mStaffInfoViewModel.StaffInfoEntity.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges(); //update
                }
                else
                {
                    mStaffInfoViewModel.StaffInfoEntity.ConfigId = mLogbookMainViewModel.SelectedLogbook.Id;
                    mStaffInfoViewModel.StaffInfoEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    mStaffInfoViewModel.StaffInfoEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mStaffInfoViewModel.StaffInfoEntity.CreateTime = System.DateTime.Now;

                    mStaffInfoViewModel.StaffInfoEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mStaffInfoViewModel.StaffInfoEntity.UpdateTime = System.DateTime.Now;
                    mStaffInfoViewModel.StaffInfoEntity.IsDeleted = YcConstants.INT_DB_DATA_AVAILABLE;

                    mEntityService.Entities.ZdtzZdStaffInfoes.AddObject(mStaffInfoViewModel.StaffInfoEntity);

                    mEntityService.Entities.SaveChanges(); //insert
                }
                saved = true;

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                //返回列表页
                mLogbookMainViewModel.ContentView = mStaffInfoQueryViewModel.View;
            }
            catch (ValidationException ex)
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }
            catch (UpdateException ex)
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return saved;
        }

        private bool ValueCheck()
        {
            bool result = true;

            if (mStaffInfoViewModel.StaffInfoEntity.RecordTime == null
                || ValidateUtil.IsBlank(mStaffInfoViewModel.StaffInfoEntity.RecordTime.ToString())
                || ValidateUtil.IsBlank(mStaffInfoViewModel.StaffInfoEntity.Name)
                || ValidateUtil.IsBlank(mStaffInfoViewModel.StaffInfoEntity.PoliceNo)
                || ValidateUtil.IsBlank(mStaffInfoViewModel.StaffInfoEntity.Address)
                || ValidateUtil.IsBlank(mStaffInfoViewModel.StaffInfoEntity.Telephone)
                || ValidateUtil.IsBlank(mStaffInfoViewModel.StaffInfoEntity.Degree)
                || ValidateUtil.IsBlank(mStaffInfoViewModel.StaffInfoEntity.IdNo))
            {
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }

            return result;
        }
    }
}
    