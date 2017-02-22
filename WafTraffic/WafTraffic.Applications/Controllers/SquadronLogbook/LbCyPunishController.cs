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

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class LbCyPunishController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private LogbookMainViewModel mLogbookMainViewModel;
        private FrequentMainViewModel mFrequentMainViewModel;
        private LbCyPunishUpdateViewModel mPunishUpdateViewModel;
        private LbCyPunishQueryViewModel mPunishQueryViewModel;
        private LbCyPunishGatherViewModel mPunishGatherViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand gatherCommand;
        private readonly DelegateCommand gatherQueryCommand;
        private readonly DelegateCommand gatherRetreatCommand;

        [ImportingConstructor]
        public LbCyPunishController(CompositionContainer container, 
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
                mPunishUpdateViewModel = container.GetExportedValue<LbCyPunishUpdateViewModel>();
                mPunishQueryViewModel = container.GetExportedValue<LbCyPunishQueryViewModel>();
                mPunishGatherViewModel = container.GetExportedValue<LbCyPunishGatherViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.gatherCommand = new DelegateCommand(() => GatherOper(), null);
                this.gatherQueryCommand = new DelegateCommand(() => GatherQueryOper(), null);
                this.gatherRetreatCommand = new DelegateCommand(() => GatherRetreaOper(), null);
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
                mPunishUpdateViewModel.SaveCommand = this.mSaveCommand;
                mPunishUpdateViewModel.CancelCommand = this.mCancelCommand;

                mPunishQueryViewModel.NewCommand = this.mNewCommand;
                mPunishQueryViewModel.ModifyCommand = this.mModifyCommand;
                mPunishQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mPunishQueryViewModel.QueryCommand = this.mQueryCommand;
                mPunishQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mPunishQueryViewModel.GatherCommand = this.gatherCommand;

                mPunishGatherViewModel.GatherQueryCommand = this.gatherQueryCommand;
                mPunishGatherViewModel.GatherRetreatCommand = this.gatherRetreatCommand;
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
                mPunishUpdateViewModel.PunishEntity = new ZdtzCyPunish();
                mPunishUpdateViewModel.PunishEntity.PatrolDate = DateTime.Now;
                mPunishUpdateViewModel.IsBrowse = false;

                mPunishUpdateViewModel.PunishEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mPunishUpdateViewModel.PunishEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mPunishUpdateViewModel.PunishEntity.OwnDepartmentId);
                mPunishUpdateViewModel.PunishEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mPunishUpdateViewModel.PunishEntity.CreateName = AuthService.Instance.GetUserNameById(mPunishUpdateViewModel.PunishEntity.CreateId);

                if (mPunishQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mPunishUpdateViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mPunishUpdateViewModel.View;
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
                mPunishUpdateViewModel.IsBrowse = false;
                mPunishUpdateViewModel.PunishEntity = mPunishQueryViewModel.SelectedPunish;
                if (mPunishQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mPunishUpdateViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mPunishUpdateViewModel.View;
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
                mFrequentMainViewModel.ContentView = mPunishQueryViewModel.View;
                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    mPunishQueryViewModel.SelectedPunish.IsDeleted = YcConstants.INT_DB_DATA_DELETED;
                    mPunishQueryViewModel.SelectedPunish.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPunishQueryViewModel.SelectedPunish.UpdateTime = System.DateTime.Now;
                    //mEntityService.LbPunishs.Remove(mPunishQueryViewModel.SelectedPunish);
                    mEntityService.Entities.SaveChanges();
                    //刷新DataGrid
                    mPunishQueryViewModel.GridRefresh();

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
                if (mPunishQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mPunishQueryViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mPunishQueryViewModel.View;
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
            bool IsBrowse = true;

            try
            {
                mPunishUpdateViewModel.IsBrowse = true;
                mPunishUpdateViewModel.PunishEntity = mPunishQueryViewModel.SelectedPunish;
                if (mPunishQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mPunishUpdateViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mPunishUpdateViewModel.View;
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return IsBrowse;
        }

        public bool QueryOper()
        {
            bool newer = true;
            int deptId;

            try
            {
                if (mPunishQueryViewModel.IsSelectDepartmentEnabled)
                {
                    deptId = Convert.ToInt32(mPunishQueryViewModel.SelectedDepartment);
                }
                else
                {
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }
                DateTime startDate = (DateTime)mPunishQueryViewModel.SelectedStartDate;
                DateTime endDate = (DateTime)mPunishQueryViewModel.SelectedEndDate;

                mPunishQueryViewModel.LbPunishs = mEntityService.QueryPunishs(deptId, startDate, endDate);

                if (mPunishQueryViewModel.LbPunishs == null || mPunishQueryViewModel.LbPunishs.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mPunishQueryViewModel.GridRefresh();
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

                if (mPunishUpdateViewModel.PunishEntity.Id > 0)
                {
                    mPunishUpdateViewModel.PunishEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPunishUpdateViewModel.PunishEntity.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges(); //update
                }
                else
                {
                    int configId = 0;
                    if (mPunishQueryViewModel.IsFromAll)
                    {
                        configId = mLogbookMainViewModel.SelectedLogbook.Id;
                    }
                    else
                    {
                        configId = mFrequentMainViewModel.SelectedFrequent.Id;
                    }
                    mPunishUpdateViewModel.PunishEntity.ConfigId = configId;
                    //mPunishUpdateViewModel.PunishEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    //mPunishUpdateViewModel.PunishEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPunishUpdateViewModel.PunishEntity.CreateTime = System.DateTime.Now;

                    mPunishUpdateViewModel.PunishEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPunishUpdateViewModel.PunishEntity.UpdateTime = System.DateTime.Now;
                    mPunishUpdateViewModel.PunishEntity.IsDeleted = YcConstants.INT_DB_DATA_AVAILABLE;

                    mEntityService.Entities.ZdtzCyPunishes.AddObject(mPunishUpdateViewModel.PunishEntity);

                    mEntityService.Entities.SaveChanges(); //insert
                }
                saved = true;

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                //返回列表页
                if (mPunishQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mPunishQueryViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mPunishQueryViewModel.View;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return saved;
        }

        public bool GatherOper()
        {
            bool deal = true;

            try
            {
                mFrequentMainViewModel.ContentView = mPunishGatherViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool GatherQueryOper()
        {
            bool deal = true;

            try
            {
                mPunishGatherViewModel.PunishGatherList 
                    = mEntityService.GetGatherPunishByDate(mPunishGatherViewModel.GatherStartDate, mPunishGatherViewModel.GatherEndDate);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            mPunishGatherViewModel.ReloadGatherReport();

            return deal;
        }

        public bool GatherRetreaOper()
        {
            bool deal = true;

            if (mPunishQueryViewModel.IsFromAll)
            {
                mLogbookMainViewModel.ContentView = mPunishQueryViewModel.View;
            }
            else
            {
                mFrequentMainViewModel.ContentView = mPunishQueryViewModel.View;
            }

            return deal;
        }
    }
}
    