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
    internal class ZgxcAskForLeaveController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private short operationCode;

        private MainFrameViewModel mainFrameViewModel;
        private ZgxcAskForLeaveUpdateViewModel logbookUpdateViewModel;
        private ZgxcAskForLeaveQueryViewModel logbookQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mApproveCommand;
        private readonly DelegateCommand mLeaveReturnCommand;


        [ImportingConstructor]
        public ZgxcAskForLeaveController(CompositionContainer container, 
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
                logbookUpdateViewModel = container.GetExportedValue<ZgxcAskForLeaveUpdateViewModel>();
                logbookQueryViewModel = container.GetExportedValue<ZgxcAskForLeaveQueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mApproveCommand = new DelegateCommand(() => ApproveOper(), null);
                this.mLeaveReturnCommand = new DelegateCommand(() => LeaveReturnOper(), null);
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
                logbookQueryViewModel.ApproveCommand = this.mApproveCommand;
                logbookQueryViewModel.LeaveReturnCommand = this.mLeaveReturnCommand;
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
                operationCode = 1;
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;

                logbookUpdateViewModel.IsTitleReadOnly = false;
                logbookUpdateViewModel.CanDepartEnable = true;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = true;
                logbookUpdateViewModel.IsReturnBackReadOnly = true;

                logbookUpdateViewModel.AskForLeaveEntity = new ZgxcAskForLeave();

                BaseOrganizeEntity org = logbookUpdateViewModel.DepartmentList.Find(
                    instance => (instance.Id == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId)
                );

                if (org != null)
                {
                    logbookUpdateViewModel.AskForLeaveEntity.PersonDepartmentId = org.Id;
                    logbookUpdateViewModel.AskForLeaveEntity.PersonDepartmentName = org.FullName;
                }

                logbookUpdateViewModel.AskForLeaveEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                //logbookUpdateViewModel.AskForLeaveEntity.CreateName = AuthService.Instance.GetUserNameById(logbookUpdateViewModel.AskForLeaveEntity.CreateId);


                logbookUpdateViewModel.AskForLeaveEntity.PersonName = AuthService.Instance.GetUserNameById(logbookUpdateViewModel.AskForLeaveEntity.CreateId);
                logbookUpdateViewModel.AskForLeaveEntity.Applicant = logbookUpdateViewModel.AskForLeaveEntity.PersonName;

                logbookUpdateViewModel.AskForLeaveEntity.ApplicationDate = System.DateTime.Now;

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
                operationCode = 2;
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;

                logbookUpdateViewModel.IsTitleReadOnly = false;
                logbookUpdateViewModel.CanDepartEnable = true;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = true;
                logbookUpdateViewModel.IsReturnBackReadOnly = true;

                logbookUpdateViewModel.AskForLeaveEntity = logbookQueryViewModel.SelectedYellowMarkCar;
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
                operationCode = 3;
                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

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
                operationCode = 0;
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsTitleReadOnly = true;
                logbookUpdateViewModel.CanDepartEnable = false;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = true;
                logbookUpdateViewModel.IsReturnBackReadOnly = true;

                logbookUpdateViewModel.AskForLeaveEntity = logbookQueryViewModel.SelectedYellowMarkCar;
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
                logbookQueryViewModel.ZgxcAskForLeaves = mEntityService.QueryZgxcAskForLeaves.Where<ZgxcAskForLeave>
                (
                    entity =>
                        (entity.LeaveDateFrom.Value >= logbookQueryViewModel.StartDate)
                            &&
                        (entity.LeaveDateTo.Value <= logbookQueryViewModel.EndDate)
                            &&
                        (string.IsNullOrEmpty(logbookQueryViewModel.KeyWord) ? true : (entity.PersonName.Contains(logbookQueryViewModel.KeyWord)))
                            &&
                        (entity.IsDeleted == false)
                );

                if (logbookQueryViewModel.ZgxcAskForLeaves == null || logbookQueryViewModel.ZgxcAskForLeaves.Count() == 0)
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
            if (operationCode == 4)
            {
                if (string.IsNullOrEmpty(logbookUpdateViewModel.AskForLeaveEntity.ApproveComments) || logbookUpdateViewModel.AskForLeaveEntity.ApproveDate == null)
                {
                    MessageBox.Show("审批时，[审批意见]和[审批时间]是必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            try
            {
                BaseOrganizeEntity org = logbookUpdateViewModel.DepartmentList.Find(
                    instance => (instance.Id == logbookUpdateViewModel.AskForLeaveEntity.PersonDepartmentId)
                );

                BaseUserEntity usr = logbookUpdateViewModel.BaseUerList.Find(
                    instance => (instance.Id == logbookUpdateViewModel.AskForLeaveEntity.ApproverId)
                );


                if (logbookUpdateViewModel.AskForLeaveEntity.Id > 0)
                {
                    logbookUpdateViewModel.AskForLeaveEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.AskForLeaveEntity.UpdateTime = System.DateTime.Now;

                    logbookUpdateViewModel.AskForLeaveEntity.PersonDepartmentName = org.FullName;
                    logbookUpdateViewModel.AskForLeaveEntity.ApproverName = usr.RealName;
                }
                else
                {
                    //logbookUpdateViewModel.AskForLeaveEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    logbookUpdateViewModel.AskForLeaveEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.AskForLeaveEntity.CreateTime = System.DateTime.Now;
                    logbookUpdateViewModel.AskForLeaveEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.AskForLeaveEntity.UpdateTime = System.DateTime.Now;
                    logbookUpdateViewModel.AskForLeaveEntity.PersonDepartmentName = org.FullName;
                    logbookUpdateViewModel.AskForLeaveEntity.ApproverName = usr.RealName;
                    logbookUpdateViewModel.AskForLeaveEntity.IsDeleted = false;

                    mEntityService.Entities.ZgxcAskForLeaves.AddObject(logbookUpdateViewModel.AskForLeaveEntity);
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

            if (logbookUpdateViewModel.AskForLeaveEntity.PersonDepartmentId == 0 || logbookUpdateViewModel.AskForLeaveEntity.PersonDepartmentId == null
                || ValidateUtil.IsBlank(logbookUpdateViewModel.AskForLeaveEntity.PersonName)
                || logbookUpdateViewModel.AskForLeaveEntity.ApproverId == 0 || logbookUpdateViewModel.AskForLeaveEntity.ApproverId == null
                || logbookUpdateViewModel.AskForLeaveEntity.LeaveDays <= 0)
            {
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
                return result;
            }

            return result;
        }


        private bool ApproveOper()
        {
            bool result = true;

            try
            {
                operationCode = 4;
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;

                logbookUpdateViewModel.IsTitleReadOnly = true;
                logbookUpdateViewModel.CanDepartEnable = false;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = false;
                logbookUpdateViewModel.IsReturnBackReadOnly = true;

                logbookUpdateViewModel.AskForLeaveEntity = logbookQueryViewModel.SelectedYellowMarkCar;
                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }

        private bool LeaveReturnOper()
        {
            bool result = true;

            try
            {
                operationCode = 5;
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;

                logbookUpdateViewModel.IsTitleReadOnly = true;
                logbookUpdateViewModel.CanDepartEnable = false;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = true;
                logbookUpdateViewModel.IsReturnBackReadOnly = false;

                logbookUpdateViewModel.AskForLeaveEntity = logbookQueryViewModel.SelectedYellowMarkCar;
                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }
    }
}
    