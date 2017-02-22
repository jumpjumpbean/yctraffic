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
    internal class FzkChangeMeasureController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mainFrameViewModel;
        private FzkChangeMeasureUpdateViewModel logbookUpdateViewModel;
        private FzkChangeMeasureQueryViewModel logbookQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mApproveCommand;
        private readonly DelegateCommand mApproveSaveCommand;

        [ImportingConstructor]
        public FzkChangeMeasureController(CompositionContainer container, 
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
                logbookUpdateViewModel = container.GetExportedValue<FzkChangeMeasureUpdateViewModel>();
                logbookQueryViewModel = container.GetExportedValue<FzkChangeMeasureQueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mApproveCommand = new DelegateCommand(() => ApproveOper(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mApproveSaveCommand = new DelegateCommand(() => ApproveSaveOper(), null);
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
                logbookUpdateViewModel.ApproveSaveCommand = this.mApproveSaveCommand;

                logbookQueryViewModel.NewCommand = this.mNewCommand;
                logbookQueryViewModel.ModifyCommand = this.mModifyCommand;
                logbookQueryViewModel.DeleteCommand = this.mDeleteCommand;
                logbookQueryViewModel.QueryCommand = this.mQueryCommand;
                logbookQueryViewModel.BrowseCommand = this.mBrowseCommand;
                logbookQueryViewModel.ApproveCommand = this.mApproveCommand;
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
                logbookUpdateViewModel.CanApproveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsBaseInfoReadOnly = false;
                logbookUpdateViewModel.IsApproveInfoReadOnly = true;

                logbookUpdateViewModel.ChangeMeasureEntity = new FzkChangeMeasure();
                logbookUpdateViewModel.ChangeMeasureEntity.PunishTime = DateTime.Now;
                logbookUpdateViewModel.ChangeMeasureEntity.PunishReason = "使用安全技术条件不符合国家标准要求的车辆运输危险化学品、驾驶拼装车上路";
                logbookUpdateViewModel.ChangeMeasureEntity.OtherPhone = "无";
                logbookUpdateViewModel.ChangeMeasureEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                logbookUpdateViewModel.ChangeMeasureEntity.CreateName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                logbookUpdateViewModel.ChangeMeasureEntity.ReportPolice = CurrentLoginService.Instance.CurrentUserInfo.RealName;
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
                logbookUpdateViewModel.CanApproveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsBaseInfoReadOnly = false;
                logbookUpdateViewModel.IsApproveInfoReadOnly = true;

                logbookUpdateViewModel.ChangeMeasureEntity = logbookQueryViewModel.SelectedPublicityLogbook;
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
                    logbookQueryViewModel.SelectedPublicityLogbook.IsDeleted = true;
                    logbookQueryViewModel.SelectedPublicityLogbook.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookQueryViewModel.SelectedPublicityLogbook.UpdateTime = System.DateTime.Now;
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
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanApproveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsBaseInfoReadOnly = true;
                logbookUpdateViewModel.IsApproveInfoReadOnly = true;

                logbookUpdateViewModel.ChangeMeasureEntity = logbookQueryViewModel.SelectedPublicityLogbook;
                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool ApproveOper()
        {
            bool newer = true;

            try
            {
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanApproveVisibal = System.Windows.Visibility.Visible;

                logbookUpdateViewModel.IsBaseInfoReadOnly = true;
                logbookUpdateViewModel.IsApproveInfoReadOnly = false;

                logbookUpdateViewModel.ChangeMeasureEntity = logbookQueryViewModel.SelectedPublicityLogbook;

                logbookUpdateViewModel.ChangeMeasureEntity.ApproveResult = "同意";
                logbookUpdateViewModel.ChangeMeasureEntity.ApprovalName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
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
                logbookQueryViewModel.FzkChangeMeasures = mEntityService.QueryFzkChangeMeasures.Where<FzkChangeMeasure>
                (
                    entity =>
                        (string.IsNullOrEmpty(logbookQueryViewModel.KeyWord_Vehicle) ? true : (entity.VehicleNo.Contains(logbookQueryViewModel.KeyWord_Vehicle)))
                            &&
                        (entity.PunishTime.Value >= logbookQueryViewModel.StartDate)
                            &&
                        (entity.PunishTime.Value <= logbookQueryViewModel.EndDate)
                            &&
                        (string.IsNullOrEmpty(logbookQueryViewModel.KeyWord_Name) ? true : (entity.Name.Contains(logbookQueryViewModel.KeyWord_Name)))
                            &&
                        (entity.IsDeleted == false)
                );

                if (logbookQueryViewModel.FzkChangeMeasures == null || logbookQueryViewModel.FzkChangeMeasures.Count() == 0)
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

                if (logbookUpdateViewModel.ChangeMeasureEntity.Id > 0)
                {
                    logbookUpdateViewModel.ChangeMeasureEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.ChangeMeasureEntity.UpdateTime = System.DateTime.Now;
                }
                else
                {
                    logbookUpdateViewModel.ChangeMeasureEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.ChangeMeasureEntity.CreateTime = System.DateTime.Now;
                    logbookUpdateViewModel.ChangeMeasureEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.ChangeMeasureEntity.UpdateTime = System.DateTime.Now;
                    logbookUpdateViewModel.ChangeMeasureEntity.IsDeleted = false;

                    mEntityService.Entities.FzkChangeMeasures.AddObject(logbookUpdateViewModel.ChangeMeasureEntity);
                }
                mEntityService.Entities.SaveChanges();

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mainFrameViewModel.ContentView = logbookQueryViewModel.View;

            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return saver;
        }

 

        private bool ValueCheck()
        {
            bool result = true;

            if (string.IsNullOrEmpty(logbookUpdateViewModel.ChangeMeasureEntity.Name)
                || logbookUpdateViewModel.ChangeMeasureEntity.PunishTime == null
                || ValidateUtil.IsBlank(logbookUpdateViewModel.ChangeMeasureEntity.PunishTime.ToString())
                || string.IsNullOrEmpty(logbookUpdateViewModel.ChangeMeasureEntity.VehicleNo)
                || string.IsNullOrEmpty(logbookUpdateViewModel.ChangeMeasureEntity.DriverLicenseNo))
            {
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;     
            }
            return result;
        }

        private void ApproveSaveOper()
        {
            logbookUpdateViewModel.ChangeMeasureEntity.ApprovalTime = System.DateTime.Now;

            mEntityService.Entities.SaveChanges();

            MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //返回列表页
            //RefreshQueryData();
            mainFrameViewModel.ContentView = logbookQueryViewModel.View;

        }
    }
}
    