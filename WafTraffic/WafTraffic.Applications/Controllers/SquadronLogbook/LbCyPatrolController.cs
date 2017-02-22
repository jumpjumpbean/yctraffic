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
    internal class LbCyPatrolController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private LogbookMainViewModel mLogbookMainViewModel;
        private FrequentMainViewModel mFrequentMainViewModel;
        private LbCyPatrolUpdateViewModel mPatrolUpdateViewModel;
        private LbCyPatrolQueryViewModel mPatrolQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mBrowseCommand;

        [ImportingConstructor]
        public LbCyPatrolController(CompositionContainer container, 
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
                mFrequentMainViewModel = container.GetExportedValue<FrequentMainViewModel>();
                mPatrolUpdateViewModel = container.GetExportedValue<LbCyPatrolUpdateViewModel>();
                mPatrolQueryViewModel = container.GetExportedValue<LbCyPatrolQueryViewModel>();

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
                mPatrolUpdateViewModel.SaveCommand = this.mSaveCommand;
                mPatrolUpdateViewModel.CancelCommand = this.mCancelCommand;

                mPatrolQueryViewModel.NewCommand = this.mNewCommand;
                mPatrolQueryViewModel.ModifyCommand = this.mModifyCommand;
                mPatrolQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mPatrolQueryViewModel.QueryCommand = this.mQueryCommand;
                mPatrolQueryViewModel.BrowseCommand = this.mBrowseCommand;
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
                mPatrolUpdateViewModel.PatrolEntity = new ZdtzCyPatrol();
                mPatrolUpdateViewModel.PatrolEntity.PatrolDate = DateTime.Now;

                mPatrolUpdateViewModel.PatrolEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mPatrolUpdateViewModel.PatrolEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mPatrolUpdateViewModel.PatrolEntity.OwnDepartmentId);
                mPatrolUpdateViewModel.PatrolEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mPatrolUpdateViewModel.PatrolEntity.CreateName = AuthService.Instance.GetUserNameById(mPatrolUpdateViewModel.PatrolEntity.CreateId);

                mPatrolUpdateViewModel.IsBrowse = false;
                if (mPatrolQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mPatrolUpdateViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mPatrolUpdateViewModel.View;
                }
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
                mPatrolUpdateViewModel.IsBrowse = false;
                mPatrolUpdateViewModel.PatrolEntity = mPatrolQueryViewModel.SelectedPatrol;
                if (mPatrolQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mPatrolUpdateViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mPatrolUpdateViewModel.View;
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool DeleteOper()
        {            
            bool newer = true;

            try
            {
                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    mPatrolQueryViewModel.SelectedPatrol.IsDeleted = YcConstants.INT_DB_DATA_DELETED;
                    mPatrolQueryViewModel.SelectedPatrol.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPatrolQueryViewModel.SelectedPatrol.UpdateTime = System.DateTime.Now;
                    //mEntityService.LbPatrols.Remove(mPatrolQueryViewModel.SelectedPatrol);
                    mEntityService.Entities.SaveChanges();
                    //刷新DataGrid
                    mPatrolQueryViewModel.GridRefresh();

                    mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "删除成功."));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误，删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool CancelOper()
        {
            bool newer = true;

            try
            {
                if (mPatrolQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mPatrolQueryViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mPatrolQueryViewModel.View;
                }
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
                mPatrolUpdateViewModel.IsBrowse = true;
                mPatrolUpdateViewModel.PatrolEntity = mPatrolQueryViewModel.SelectedPatrol;
                if (mPatrolQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mPatrolUpdateViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mPatrolUpdateViewModel.View;
                }
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
                if (mPatrolQueryViewModel.IsSelectDepartmentEnabled)
                {
                    deptId = Convert.ToInt32(mPatrolQueryViewModel.SelectedDepartment);
                }
                else
                {
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }
                DateTime startDate = (DateTime)mPatrolQueryViewModel.SelectedStartDate;
                DateTime endDate = (DateTime)mPatrolQueryViewModel.SelectedEndDate;

                mPatrolQueryViewModel.LbPatrols = mEntityService.QueryPatrols(deptId, startDate, endDate);

                if (mPatrolQueryViewModel.LbPatrols == null || mPatrolQueryViewModel.LbPatrols.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mPatrolQueryViewModel.GridRefresh();
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
                if (mPatrolUpdateViewModel.PatrolEntity.Id > 0)
                {
                    mPatrolUpdateViewModel.PatrolEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPatrolUpdateViewModel.PatrolEntity.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges(); //update
                }
                else
                {
                    int configId = 0;
                    if (mPatrolQueryViewModel.IsFromAll)
                    {
                        configId = mLogbookMainViewModel.SelectedLogbook.Id;
                    }
                    else
                    {
                        configId = mFrequentMainViewModel.SelectedFrequent.Id;
                    }
                    mPatrolUpdateViewModel.PatrolEntity.ConfigId = configId;
                    //mPatrolUpdateViewModel.PatrolEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    //mPatrolUpdateViewModel.PatrolEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPatrolUpdateViewModel.PatrolEntity.CreateTime = System.DateTime.Now;

                    mPatrolUpdateViewModel.PatrolEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPatrolUpdateViewModel.PatrolEntity.UpdateTime = System.DateTime.Now;
                    mPatrolUpdateViewModel.PatrolEntity.IsDeleted = YcConstants.INT_DB_DATA_AVAILABLE;

                    mEntityService.Entities.ZdtzCyPatrols.AddObject(mPatrolUpdateViewModel.PatrolEntity);

                    mEntityService.Entities.SaveChanges(); //insert
                }
                saved = true;

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                //返回列表页
                if (mPatrolQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mPatrolQueryViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mPatrolQueryViewModel.View;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return saved;
        }

        private bool ValueCheck()
        {
            bool result = true;

            if (ValidateUtil.IsBlank(mPatrolUpdateViewModel.PatrolEntity.Content))
            {
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }

            return result;
        }
    }
}
    