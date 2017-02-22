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
    internal class ZhzxFilterController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private ZhzxFilterUpdateViewModel mFilterUpdateViewModel;
        private ZhzxFilterQueryViewModel mFilterQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;

        [ImportingConstructor]
        public ZhzxFilterController(CompositionContainer container, 
            System.Waf.Applications.Services.IMessageService messageService, 
            IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.mContainer = container;
                this.mMessageService = messageService;
                this.mShellService = shellService;
                this.mEntityService = entityService;

                mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                mFilterUpdateViewModel = container.GetExportedValue<ZhzxFilterUpdateViewModel>();
                mFilterQueryViewModel = container.GetExportedValue<ZhzxFilterQueryViewModel>();

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
                mFilterUpdateViewModel.SaveCommand = this.mSaveCommand;
                mFilterUpdateViewModel.CancelCommand = this.mCancelCommand;

                mFilterQueryViewModel.NewCommand = this.mNewCommand;
                mFilterQueryViewModel.ModifyCommand = this.mModifyCommand;
                mFilterQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mFilterQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mFilterQueryViewModel.QueryCommand = this.mQueryCommand;
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
                if (!HaveAuth()) return false;

                mFilterUpdateViewModel.IsBrowse = false;
                mFilterUpdateViewModel.FilterEntity = new ZhzxRedNameList();
                mMainFrameViewModel.ContentView = mFilterUpdateViewModel.View;
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
                if (!HaveAuth()) return false;

                mFilterUpdateViewModel.IsBrowse = false;
                mFilterUpdateViewModel.FilterEntity = mFilterQueryViewModel.SelectedFilter;
                mMainFrameViewModel.ContentView = mFilterUpdateViewModel.View;
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
                if (!HaveAuth()) return false;

                mFilterUpdateViewModel.IsBrowse = true;
                mFilterUpdateViewModel.FilterEntity = mFilterQueryViewModel.SelectedFilter;
                mMainFrameViewModel.ContentView = mFilterUpdateViewModel.View;
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
                if (!HaveAuth()) return false;

                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    mFilterQueryViewModel.SelectedFilter.IsDeleted = YcConstants.INT_DB_DATA_DELETED;
                    mFilterQueryViewModel.SelectedFilter.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFilterQueryViewModel.SelectedFilter.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    mFilterQueryViewModel.GridRefresh();
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
                if (mFilterQueryViewModel.SelectedFilter != null && mFilterQueryViewModel.SelectedFilter.Id != 0)
                {
                    mEntityService.Entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, mFilterQueryViewModel.SelectedFilter);
                }
                mMainFrameViewModel.ContentView = mFilterQueryViewModel.View;
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
            string inputPlateNumber;
            try
            {
                if (!HaveAuth()) return false;

                inputPlateNumber = mFilterQueryViewModel.InputPlateNumber;

                mFilterQueryViewModel.Filters = mEntityService.QueryZhzxRedNameLists(inputPlateNumber);
                if (mFilterQueryViewModel.Filters == null || mFilterQueryViewModel.Filters.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mFilterQueryViewModel.GridRefresh();
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
                if (!HaveAuth()) return false;

                if (!ValueCheck())
                {
                    return saved;
                }
                if (mFilterUpdateViewModel.FilterEntity.Id > 0)
                {
                    mFilterUpdateViewModel.FilterEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFilterUpdateViewModel.FilterEntity.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges(); //update
                }
                else
                {
                    mFilterUpdateViewModel.FilterEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFilterUpdateViewModel.FilterEntity.CreateTime = System.DateTime.Now;

                    mFilterUpdateViewModel.FilterEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFilterUpdateViewModel.FilterEntity.UpdateTime = System.DateTime.Now;
                    mFilterUpdateViewModel.FilterEntity.IsDeleted = YcConstants.INT_DB_DATA_AVAILABLE;

                    mEntityService.ZhzxRedNameLists.Add(mFilterUpdateViewModel.FilterEntity);

                    mEntityService.Entities.SaveChanges(); //insert
                }
                saved = true;

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                //返回列表页
                mMainFrameViewModel.ContentView = mFilterQueryViewModel.View;
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

            if (ValidateUtil.IsBlank(mFilterUpdateViewModel.FilterEntity.LicensePlateNumber))
            {
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }

            return result;
        }

        private bool HaveAuth()
        {
            bool haveAuth = (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator
                || CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstants.INT_ROLE_DDZ_ID);
            if (!haveAuth)
            {
                MessageBox.Show("无操作权限", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return haveAuth;
        }
    }
}
    