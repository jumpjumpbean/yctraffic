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
using WafTraffic.Domain.Common;
using WafTraffic.Applications.Utils;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class SskRequestController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private SskRequestUpdateViewModel mRequestUpdateViewModel;
        private SskRequestQueryViewModel mRequestQueryViewModel;

        private readonly DelegateCommand mAddCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mSubmitCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mRejectCommand;
        private readonly DelegateCommand mDealCommand;
        //private readonly DelegateCommand mSuperviseCommand;

        private List<Status> mStatusList = null;
        private List<BaseOrganizeEntity> mDepartmentList = null;
        private List<Status> mRequestTypeList = null;

        [ImportingConstructor]
        public SskRequestController(CompositionContainer container, 
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
                mRequestUpdateViewModel = container.GetExportedValue<SskRequestUpdateViewModel>();
                mRequestQueryViewModel = container.GetExportedValue<SskRequestQueryViewModel>();

                this.mAddCommand = new DelegateCommand(() => AddOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSubmitCommand = new DelegateCommand(() => SubmitOper(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mRejectCommand = new DelegateCommand(() => RejectOper(), null);
                this.mDealCommand = new DelegateCommand(() => DealOper(), null);
                //this.mSuperviseCommand = new DelegateCommand(() => SuperviseOper(), null);
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
                mRequestUpdateViewModel.SubmitCommand = this.mSubmitCommand;
                mRequestUpdateViewModel.CancelCommand = this.mCancelCommand;
                mRequestUpdateViewModel.RejectCommand = this.mRejectCommand;
                //mRequestUpdateViewModel.SuperviseCommand = this.mSuperviseCommand;
                mRequestUpdateViewModel.SubLeaderSignCommand = new DelegateCommand(() => SubLeaderSignOper(), null);
                mRequestUpdateViewModel.DdzSignCommand = new DelegateCommand(() => DdzSignOper(), null);
                mRequestUpdateViewModel.ShowSignImgCommand = new DelegateCommand(() => ShowSignImg(), null);

                mRequestQueryViewModel.AddCommand = this.mAddCommand;
                mRequestQueryViewModel.ModifyCommand = this.mModifyCommand;
                mRequestQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mRequestQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mRequestQueryViewModel.QueryCommand = this.mQueryCommand;
                mRequestQueryViewModel.DealCommand = this.mDealCommand;

                InitStatusList();
                mRequestQueryViewModel.StatusList = this.mStatusList;
                InitRequestTypeList();
                mRequestUpdateViewModel.RequestTypeList = this.mRequestTypeList;
                InitDepartmentList();
                mRequestUpdateViewModel.DepartmentList = this.mDepartmentList;
                mRequestUpdateViewModel.LeaderList = AuthService.Instance.SubLeaderGroup;

            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void InitStatusList()
        {
            mStatusList = new List<Status>();
            Status st = new Status(YcConstants.INT_KJSS_REQSTAT_NULL, "全部");
            mStatusList.Add(st);
            st = new Status(YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE, YcConstants.STR_KJSS_REQSTAT_SUB_LEADER_APPROVE);
            mStatusList.Add(st);
            st = new Status(YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE, YcConstants.STR_KJSS_REQSTAT_DDZ_APPROVE);
            mStatusList.Add(st);
            st = new Status(YcConstants.INT_KJSS_REQSTAT_ZHZX_EXECUTE, YcConstants.STR_KJSS_REQSTAT_ZHZX_EXECUTE);
            mStatusList.Add(st);
            st = new Status(YcConstants.INT_KJSS_REQSTAT_COMPLETED, YcConstants.STR_KJSS_REQSTAT_COMPLETED);
            mStatusList.Add(st);
        }

        private void InitRequestTypeList()
        {
            mRequestTypeList = new List<Status>();
            Status st = new Status(YcConstants.INT_ZHZX_REQTYPE_ADD, YcConstants.STR_ZHZX_REQTYPE_ADD);
            mRequestTypeList.Add(st);
            st = new Status(YcConstants.INT_ZHZX_REQTYPE_MAINTAIN, YcConstants.STR_ZHZX_REQTYPE_MAINTAIN);
            mRequestTypeList.Add(st);
        }

        private void InitDepartmentList()
        {
            try
            {
                mDepartmentList = new List<BaseOrganizeEntity>();

                BaseOrganizeManager origanizeService = new BaseOrganizeManager();
                DataTable departmentDT = origanizeService.GetDepartmentDT("");
                BaseOrganizeEntity entity;
                foreach (DataRow dr in departmentDT.Rows)
                {
                    entity = new BaseOrganizeEntity(dr);
                    mDepartmentList.Add(entity);
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool AddOper()
        {
            bool newer = true;

            try
            {
                if (!AuthService.Instance.SskRequestCanAdd)
                {
                    MessageBox.Show("无操作权限！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                mRequestUpdateViewModel.IsBrowse = false;
                mRequestUpdateViewModel.EquipmentRequestEntity = new SskEquipmentRequest();
                mRequestUpdateViewModel.SubLeaderSignImg = null;
                mRequestUpdateViewModel.DdzSignImg = null;
                mMainFrameViewModel.ContentView = mRequestUpdateViewModel.View;
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
                if (!mRequestQueryViewModel.SelectedRequest.CanModify)
                {
                    MessageBox.Show("无操作权限！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                mRequestUpdateViewModel.IsBrowse = false;
                mRequestUpdateViewModel.EquipmentRequestEntity = mRequestQueryViewModel.SelectedRequest;
                mMainFrameViewModel.ContentView = mRequestUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool DealOper()
        {
            bool ret = false;

            try
            {
                if (!mRequestQueryViewModel.SelectedRequest.CanDeal)
                {
                    MessageBox.Show("无操作权限！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ret;
                }
                /*
                mRequestUpdateViewModel.IsSuperviseButtonVisible = (mRequestQueryViewModel.SelectedRequest.Status == YcConstants.INT_ZHZX_REQSTAT_REQDEPT_APPROVE
                    && IsOverTime());
                 */
                mRequestUpdateViewModel.IsBrowse = false;
                mRequestUpdateViewModel.EquipmentRequestEntity = mRequestQueryViewModel.SelectedRequest;
                mMainFrameViewModel.ContentView = mRequestUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return ret;
        }

        public bool BrowseOper()
        {
            bool newer = true;

            try
            {
                if (!mRequestQueryViewModel.SelectedRequest.CanBrowse)
                {
                    MessageBox.Show("无操作权限！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                mRequestUpdateViewModel.IsBrowse = true;
                mRequestUpdateViewModel.EquipmentRequestEntity = mRequestQueryViewModel.SelectedRequest;
                mMainFrameViewModel.ContentView = mRequestUpdateViewModel.View;
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
                if (!mRequestQueryViewModel.SelectedRequest.CanDelete)
                {
                    MessageBox.Show("无操作权限！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    mRequestQueryViewModel.SelectedRequest.IsDeleted = YcConstants.INT_DB_DATA_DELETED;
                    mRequestQueryViewModel.SelectedRequest.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mRequestQueryViewModel.SelectedRequest.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    mRequestQueryViewModel.GridRefresh();
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
                if (mRequestUpdateViewModel.EquipmentRequestEntity != null)
                {
                    if (mRequestUpdateViewModel.EquipmentRequestEntity.Status == YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE)
                    {
                        mRequestUpdateViewModel.SubLeaderSignImg = null;

                    }
                    else if (mRequestUpdateViewModel.EquipmentRequestEntity.Status == YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE)
                    {
                        mRequestUpdateViewModel.DdzSignImg = null;
                    }
                }
                mMainFrameViewModel.ContentView = mRequestQueryViewModel.View;
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
            int selectedStatus;
            try
            {
                selectedStatus = mRequestQueryViewModel.SelectedStatus;

                DateTime startDate = mRequestQueryViewModel.SelectedStartDate;
                DateTime endDate = mRequestQueryViewModel.SelectedEndDate;

                mRequestQueryViewModel.Requests = mEntityService.QuerySskEquipmentRequests(selectedStatus, startDate, endDate.AddDays(1));

                if (mRequestQueryViewModel.Requests == null || mRequestQueryViewModel.Requests.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mRequestQueryViewModel.GridRefresh();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool SubLeaderSignOper()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(WorkerSubLeaderSign);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerSubLeaderSignCompleted);

            worker.RunWorkerAsync();

            mRequestUpdateViewModel.Show_LoadingMask();
            return true;
        }

        private void WorkerSubLeaderSign(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;

            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (mRequestUpdateViewModel.SubLeaderSignImg != null)
                {
                    mRequestUpdateViewModel.SubLeaderSignImg.StreamSource.Dispose();
                    mRequestUpdateViewModel.SubLeaderSignImg = null;
                }
                BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo, CurrentLoginService.Instance.CurrentUserInfo.Id);
                mRequestUpdateViewModel.SubLeaderSignImg = ftp.DownloadFile(user.AnswerQuestion);
                mRequestUpdateViewModel.EquipmentRequestEntity.IsSubLeaderSigned = true;
                e.Result = true;
            }
            catch (System.Exception ex)
            {
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
        }

        private void WorkerSubLeaderSignCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            mRequestUpdateViewModel.Shutdown_LoadingMask();
        }

        public bool DdzSignOper()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(WorkerDdzSign);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerDdzSignCompleted);

            worker.RunWorkerAsync();

            mRequestUpdateViewModel.Show_LoadingMask();
            return true;
        }

        private void WorkerDdzSign(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;

            try
            {
                e.Result = false;
                if (CurrentLoginService.Instance.CurrentUserInfo.RoleId != YcConstants.INT_ROLE_DDZ_ID)
                {
                    return;
                }
                ftp = new FtpHelper();
                if (mRequestUpdateViewModel.DdzSignImg != null)
                {
                    mRequestUpdateViewModel.DdzSignImg.StreamSource.Dispose();
                    mRequestUpdateViewModel.DdzSignImg = null;
                }
                BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo, CurrentLoginService.Instance.CurrentUserInfo.Id);
                mRequestUpdateViewModel.DdzSignImg = ftp.DownloadFile(user.AnswerQuestion);
                mRequestUpdateViewModel.EquipmentRequestEntity.IsDDZSigned = true;
                e.Result = true;
            }
            catch (System.Exception ex)
            {
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
        }

        private void WorkerDdzSignCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            mRequestUpdateViewModel.Shutdown_LoadingMask();
        }

        public bool RejectOper()
        {
            bool rejected = false;
            try
            {
                /*
                if (!ValueCheck())
                {
                    return rejected;
                }*/
                DialogResult dlgResult = MessageBox.Show("确定退回吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgResult == DialogResult.Yes)
                {
                    if (mRequestUpdateViewModel.EquipmentRequestEntity.Id > 0)
                    {
                        int rejectStatus;
                        string rejectStatusDesc;
                        GetRejectStatus(mRequestUpdateViewModel.EquipmentRequestEntity.Status,
                            out rejectStatus, out rejectStatusDesc);
                        //if (rejectStatus > YcConstants.INT_ZHZX_REQSTAT_NULL)
                        {
                            if (mRequestUpdateViewModel.EquipmentRequestEntity.Status == YcConstants.INT_KJSS_REQSTAT_ZHZX_EXECUTE)
                            {
                                mRequestUpdateViewModel.EquipmentRequestEntity.Node4Comment = "";
                                mRequestUpdateViewModel.EquipmentRequestEntity.CompleteTime = null;
                            }
                            else if (mRequestUpdateViewModel.EquipmentRequestEntity.Status == YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE)
                            {
                                mRequestUpdateViewModel.EquipmentRequestEntity.Node3Comment = "";                                
                                mRequestUpdateViewModel.EquipmentRequestEntity.IsDDZSigned = false;
                                mRequestUpdateViewModel.DdzSignImg = null;
                            }
                            else if (mRequestUpdateViewModel.EquipmentRequestEntity.Status == YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE)
                            {
                                mRequestUpdateViewModel.EquipmentRequestEntity.Node2Comment = "";
                                mRequestUpdateViewModel.EquipmentRequestEntity.Deadline = null;
                                mRequestUpdateViewModel.EquipmentRequestEntity.IsSubLeaderSigned = false;
                                mRequestUpdateViewModel.SubLeaderSignImg = null;
                            }
                            mRequestUpdateViewModel.EquipmentRequestEntity.Status = rejectStatus;
                            mRequestUpdateViewModel.EquipmentRequestEntity.StatusDesc = rejectStatusDesc;
                            mRequestUpdateViewModel.EquipmentRequestEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                            mRequestUpdateViewModel.EquipmentRequestEntity.UpdateTime = System.DateTime.Now;
                            mEntityService.Entities.SaveChanges(); //update
                        }
                        /*
                        else
                        {
                            MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return rejected;
                        }
                         */
                    }
                    else
                    {
                        MessageBox.Show("状态错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return rejected;
                    }
                    rejected = true;

                    MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                    //返回列表页
                    mMainFrameViewModel.ContentView = mRequestQueryViewModel.View;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return rejected;
        }

        public bool SubmitOper()
        {
            bool saved = false;

            try
            {
                //if (!HaveAuth()) return false;

                if (!ValueCheck())
                {
                    return saved;
                }
                if (mRequestUpdateViewModel.EquipmentRequestEntity.Id > 0)
                {
                    int nextStatus;
                    string nextStatusDesc;
                    GetNextStatus(mRequestUpdateViewModel.EquipmentRequestEntity.Status,
                        out nextStatus, out nextStatusDesc);
                    if (nextStatus > YcConstants.INT_KJSS_REQSTAT_NULL)
                    {
                        mRequestUpdateViewModel.EquipmentRequestEntity.Status = nextStatus;
                        mRequestUpdateViewModel.EquipmentRequestEntity.StatusDesc = nextStatusDesc;
                        mRequestUpdateViewModel.EquipmentRequestEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                        mRequestUpdateViewModel.EquipmentRequestEntity.UpdateTime = System.DateTime.Now;
                        mEntityService.Entities.SaveChanges(); //update
                    }
                    else
                    {
                        MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return saved;
                    }
                }
                else
                {
                    mRequestUpdateViewModel.EquipmentRequestEntity.RequestTypeName = GetReqTypeName(mRequestUpdateViewModel.EquipmentRequestEntity.RequestType);
                    mRequestUpdateViewModel.EquipmentRequestEntity.RequestDeptName = GetDeptName(mRequestUpdateViewModel.EquipmentRequestEntity.RequestDept);
                    mRequestUpdateViewModel.EquipmentRequestEntity.Status = YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE;
                    mRequestUpdateViewModel.EquipmentRequestEntity.StatusDesc = YcConstants.STR_KJSS_REQSTAT_SUB_LEADER_APPROVE;

                    mRequestUpdateViewModel.EquipmentRequestEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mRequestUpdateViewModel.EquipmentRequestEntity.CreateTime = System.DateTime.Now;

                    mRequestUpdateViewModel.EquipmentRequestEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mRequestUpdateViewModel.EquipmentRequestEntity.UpdateTime = System.DateTime.Now;
                    mRequestUpdateViewModel.EquipmentRequestEntity.IsDeleted = YcConstants.INT_DB_DATA_AVAILABLE;

                    mEntityService.Entities.SskEquipmentRequests.AddObject(mRequestUpdateViewModel.EquipmentRequestEntity);

                    mEntityService.Entities.SaveChanges(); //insert
                }
                saved = true;

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                //返回列表页
                mMainFrameViewModel.ContentView = mRequestQueryViewModel.View;
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

            if (ValidateUtil.IsBlank(mRequestUpdateViewModel.EquipmentRequestEntity.EquipmentName))
            {
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }

            int curStatus = mRequestUpdateViewModel.EquipmentRequestEntity.Status;
            switch (curStatus)
            {
                case YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE:
                    if (ValidateUtil.IsBlank(mRequestUpdateViewModel.EquipmentRequestEntity.Node3Comment))
                    {
                        MessageBox.Show("大队长意见不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (!mRequestUpdateViewModel.EquipmentRequestEntity.IsDDZSigned)
                    {
                        DialogResult ret = MessageBox.Show("大队长未签名，确定提交吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (ret == DialogResult.No)
                        {
                            return false;
                        }
                    }
                    break;
                case YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE:
                    if (mRequestUpdateViewModel.EquipmentRequestEntity.Deadline == null
                        || ValidateUtil.IsBlank(mRequestUpdateViewModel.EquipmentRequestEntity.Deadline.ToString()))
                    {
                        MessageBox.Show("期限不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (string.IsNullOrEmpty(mRequestUpdateViewModel.EquipmentRequestEntity.Node2Comment))
                    {
                        MessageBox.Show("分管领导意见不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (!mRequestUpdateViewModel.EquipmentRequestEntity.IsSubLeaderSigned)
                    {
                        DialogResult ret = MessageBox.Show("分管领导未签名，确定提交吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (ret == DialogResult.No)
                        {
                            return false;  
                        }
                    }
                    break;
                case YcConstants.INT_KJSS_REQSTAT_ZHZX_EXECUTE:
                    break;
                /*
                case YcConstants.INT_ZHZX_REQSTAT_DDZ_SUPERVISE:
                    if (ValidateUtil.IsBlank(mRequestUpdateViewModel.EquipmentRequestEntity.SuperviseCommnet))
                    {
                        MessageBox.Show("大队长督办不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result = false;
                    }
                    break;
                 */
                default:
                    break;
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

        private void GetNextStatus(int curStatus, out int nextStatus, out string nextStatusDesc)
        {
            switch (curStatus)
            {
                case YcConstants.INT_KJSS_REQSTAT_NULL:
                    nextStatus = YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE;
                    nextStatusDesc = YcConstants.STR_KJSS_REQSTAT_SUB_LEADER_APPROVE;
                    break;
                case YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE:
                    nextStatus = YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE;
                    nextStatusDesc = YcConstants.STR_KJSS_REQSTAT_DDZ_APPROVE;
                    break;
                case YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE:
                    nextStatus = YcConstants.INT_KJSS_REQSTAT_ZHZX_EXECUTE;
                    nextStatusDesc = YcConstants.STR_KJSS_REQSTAT_ZHZX_EXECUTE;
                    break;
                case YcConstants.INT_KJSS_REQSTAT_ZHZX_EXECUTE:
                    nextStatus = YcConstants.INT_KJSS_REQSTAT_COMPLETED;
                    nextStatusDesc = YcConstants.STR_KJSS_REQSTAT_COMPLETED;
                    break;
                default:
                    nextStatus = YcConstants.INT_KJSS_REQSTAT_NULL;
                    nextStatusDesc = YcConstants.STR_KJSS_REQSTAT_NULL;
                    break;
            }
        }

        private void GetRejectStatus(int curStatus, out int rejectStatus, out string rejectStatusDesc)
        {
            switch (curStatus)
            {
                case YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE:
                    rejectStatus = YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE;
                    rejectStatusDesc = YcConstants.STR_KJSS_REQSTAT_SUB_LEADER_APPROVE;
                    break;
                case YcConstants.INT_KJSS_REQSTAT_ZHZX_EXECUTE:
                    rejectStatus = YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE;
                    rejectStatusDesc = YcConstants.STR_KJSS_REQSTAT_DDZ_APPROVE;
                    break;
                case YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE:
                default:
                    rejectStatus = YcConstants.INT_KJSS_REQSTAT_NULL;
                    rejectStatusDesc = YcConstants.STR_KJSS_REQSTAT_NULL;
                    break;
            }
        }

        private string GetDeptName(int deptId)
        {
            string name = "";

            BaseOrganizeEntity dept = mDepartmentList.Find(p => p.Id == deptId);
            if(dept != null)
            {
                name = dept.FullName;
            }
            return name;
        }

        private string GetReqTypeName(int reqType)
        {
            string name = "";

            if (mRequestTypeList != null)
            {
                foreach (Status st in mRequestTypeList)
                {
                    if (st.WorkflowStatusId == reqType)
                    {
                        name = st.WorkflowStatusPhrase;
                    }
                }
            }
            return name;
        }

        public bool ShowSignImg()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(WorkerShowSignImg);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowSignImgCompleted);

            worker.RunWorkerAsync();

            mRequestUpdateViewModel.Show_LoadingMask();
            return true;
        }

        private void WorkerShowSignImg(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;

            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (mRequestUpdateViewModel.EquipmentRequestEntity.IsSubLeaderSigned)
                {
                    if (mRequestUpdateViewModel.SubLeaderSignImg != null)
                    {
                        mRequestUpdateViewModel.SubLeaderSignImg.StreamSource.Dispose();
                        mRequestUpdateViewModel.SubLeaderSignImg = null;
                    }
                    DataTable dt = DotNetService.Instance.UserService.GetDTByRole(CurrentLoginService.Instance.CurrentUserInfo,
                        YcConstants.INT_ROLE_KJSS_ZR.ToString());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        BaseUserEntity user = new BaseUserEntity(dt.Rows[0]);
                        mRequestUpdateViewModel.SubLeaderSignImg = ftp.DownloadFile(user.AnswerQuestion);
                    }
                    /*
                    BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo,
                        mRequestUpdateViewModel.EquipmentRequestEntity.SubLeaderId.ToString());
                    mRequestUpdateViewModel.SubLeaderSignImg = ftp.DownloadFile(user.AnswerQuestion);
                     */
                }
                if (mRequestUpdateViewModel.EquipmentRequestEntity.IsDDZSigned)
                {
                    if (mRequestUpdateViewModel.DdzSignImg != null)
                    {
                        mRequestUpdateViewModel.DdzSignImg.StreamSource.Dispose();
                        mRequestUpdateViewModel.DdzSignImg = null;
                    }
                    DataTable dt = DotNetService.Instance.UserService.GetDTByRole(CurrentLoginService.Instance.CurrentUserInfo,
                        YcConstants.INT_ROLE_DDZ_ID.ToString());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        BaseUserEntity user = new BaseUserEntity(dt.Rows[0]);
                        mRequestUpdateViewModel.DdzSignImg = ftp.DownloadFile(user.AnswerQuestion);
                    }
                }
                e.Result = true;
            }
            catch (System.Exception ex)
            {
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
        }

        private void WorkerShowSignImgCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            mRequestUpdateViewModel.Shutdown_LoadingMask();
        }
    }
}
    