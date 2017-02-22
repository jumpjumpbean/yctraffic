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
    internal class SgkReleaseCarController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mainFrameViewModel;
        private SgkReleaseCarUpdateViewModel logbookUpdateViewModel;
        private SgkReleaseCarQueryViewModel logbookQueryViewModel;
        private SgkReleaseCarPrintViewModel sgkReleasePrintViewModel;

       // private List<Status> mStatusList = null;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mApproveCommand;
        private readonly DelegateCommand mPrintCommand;
        private readonly DelegateCommand mPrintRetreatCommand;
        private readonly DelegateCommand chargeSignCommand;
        private readonly DelegateCommand showSignImgCommand;



        [ImportingConstructor]
        public SgkReleaseCarController(CompositionContainer container, 
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
                logbookUpdateViewModel = container.GetExportedValue<SgkReleaseCarUpdateViewModel>();
                logbookQueryViewModel = container.GetExportedValue<SgkReleaseCarQueryViewModel>();
                sgkReleasePrintViewModel = container.GetExportedValue<SgkReleaseCarPrintViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mApproveCommand = new DelegateCommand(() => ApproveOper(), null);
                this.mPrintCommand = new DelegateCommand(() => PrintOper(), null);
                this.mPrintRetreatCommand = new DelegateCommand(() => PrintRetreatOper(), null);
                this.chargeSignCommand = new DelegateCommand(() => ChargeSignOper(), null);
                this.showSignImgCommand = new DelegateCommand(() => ShowSignImg(), null);
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
                logbookUpdateViewModel.PrintCommand = this.mPrintCommand;
                logbookUpdateViewModel.ChargeSignCommand = this.chargeSignCommand;
                logbookUpdateViewModel.ShowSignImgCommand = this.showSignImgCommand;

                logbookQueryViewModel.NewCommand = this.mNewCommand;
                logbookQueryViewModel.ModifyCommand = this.mModifyCommand;
                logbookQueryViewModel.DeleteCommand = this.mDeleteCommand;
                logbookQueryViewModel.QueryCommand = this.mQueryCommand;
                logbookQueryViewModel.BrowseCommand = this.mBrowseCommand;
                logbookQueryViewModel.ApproveCommand = this.mApproveCommand;

                sgkReleasePrintViewModel.PrintRetreatCommand = this.mPrintRetreatCommand;

                InitChargeId();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }
        
        private void  InitChargeId()
        {
            BaseUserEntity entity = null;

            DataTable dt = DotNetService.Instance.UserService.Search(CurrentLoginService.Instance.CurrentUserInfo, "xingqingsheng", null,null);

            if (dt.Rows.Count == 1)
            {
                entity = new BaseUserEntity(dt);
            }

            logbookUpdateViewModel.ReleaseCarEntity.ChargeId = "10000932";
            logbookUpdateViewModel.ReleaseCarEntity.Node1Comment = entity.Id.ToString();
            logbookUpdateViewModel.ReleaseCarEntity.Node2Comment = "10000927";
        }


        //private void InitStatusList()
        //{
        //    mStatusList = new List<Status>();
        //    Status st = new Status(YcConstants.INT_ZHZX_REQSTAT_NULL, "全部");
        //    mStatusList.Add(st);
        //    st = new Status(YcConstants.INT_ZHZX_REQSTAT_SUB_LEADER_APPROVE, YcConstants.STR_ZHZX_REQSTAT_SUB_LEADER_APPROVE);
        //    mStatusList.Add(st);
        //    st = new Status(YcConstants.INT_ZHZX_REQSTAT_DDZ_APPROVE, YcConstants.STR_ZHZX_REQSTAT_DDZ_APPROVE);
        //    mStatusList.Add(st);
        //    st = new Status(YcConstants.INT_ZHZX_REQSTAT_BGS_EXECUTE, YcConstants.STR_ZHZX_REQSTAT_BGS_EXECUTE);
        //    mStatusList.Add(st);
        //    st = new Status(YcConstants.INT_ZHZX_REQSTAT_ZHZX_EXECUTE, YcConstants.STR_ZHZX_REQSTAT_ZHZX_EXECUTE);
        //    mStatusList.Add(st);
        //    st = new Status(YcConstants.INT_ZHZX_REQSTAT_COMPLETED, YcConstants.STR_ZHZX_REQSTAT_COMPLETED);
        //    mStatusList.Add(st);
        //}

        //private void InitRequestTypeList()
        //{
        //    mRequestTypeList = new List<Status>();
        //    Status st = new Status(YcConstants.INT_ZHZX_REQTYPE_ADD, YcConstants.STR_ZHZX_REQTYPE_ADD);
        //    mRequestTypeList.Add(st);
        //    st = new Status(YcConstants.INT_ZHZX_REQTYPE_MAINTAIN, YcConstants.STR_ZHZX_REQTYPE_MAINTAIN);
        //    mRequestTypeList.Add(st);
        //    st = new Status(YcConstants.INT_ZHZX_REQTYPE_APPLY, YcConstants.STR_ZHZX_REQTYPE_APPLY);
        //    mRequestTypeList.Add(st);
        //}

        public bool NewOper()
        {
            bool newer = true;

            logbookQueryViewModel.Operation = "New";
            try
            {
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanPrintVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.CanChargeInfoVisible = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanChargeSign1Visible = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanChargeSign2Visible = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanChargeSign3Visible = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsTitleReadOnly = false;
                logbookUpdateViewModel.CanDepartEnable = true;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = true;

                logbookUpdateViewModel.ReleaseCarEntity = new SgkReleaseCar();

                //BaseOrganizeEntity org = logbookUpdateViewModel.DepartmentList.Find(
                //    instance => (instance.Id == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId)

                //);

                //if (org != null)
                //{
                //    logbookUpdateViewModel.ReleaseCarEntity.PersonDepartmentId = org.Id;
                //    logbookUpdateViewModel.ReleaseCarEntity.PersonDepartmentName = org.FullName;
                //}

                logbookUpdateViewModel.ReleaseCarEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
               

                logbookUpdateViewModel.ReleaseCarEntity.Changes = "事故科长审核";

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

            logbookQueryViewModel.Operation = "Modify";

            ClearSignImgCache();

            try
            {
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanPrintVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.CanChargeSign1Visible = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanChargeSign2Visible = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanChargeSign3Visible = System.Windows.Visibility.Collapsed;

                if (logbookUpdateViewModel.ReleaseCarEntity.IsChargeSigned == true)
                {
                    logbookUpdateViewModel.CanChargeInfoVisible = System.Windows.Visibility.Visible;
                }
                else
                {
                    logbookUpdateViewModel.CanChargeInfoVisible = System.Windows.Visibility.Collapsed;
                }

                logbookUpdateViewModel.IsTitleReadOnly = false;
                logbookUpdateViewModel.CanDepartEnable = true;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = true;

                logbookUpdateViewModel.ReleaseCarEntity = logbookQueryViewModel.SelectedYellowMarkCar;
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

            ClearSignImgCache();

            try
            {
                logbookUpdateViewModel.ReleaseCarEntity = logbookQueryViewModel.SelectedYellowMarkCar;

                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanPrintVisibal = System.Windows.Visibility.Visible;
                
                logbookUpdateViewModel.CanChargeSign1Visible = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanChargeSign2Visible = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanChargeSign3Visible = System.Windows.Visibility.Collapsed;

                if (logbookUpdateViewModel.ReleaseCarEntity.IsChargeSigned == true)
                {
                    logbookUpdateViewModel.CanChargeInfoVisible = System.Windows.Visibility.Visible;
                }
                else
                {
                    logbookUpdateViewModel.CanChargeInfoVisible = System.Windows.Visibility.Collapsed;
                }


                logbookUpdateViewModel.IsTitleReadOnly = true;
                logbookUpdateViewModel.CanDepartEnable = false;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = true;

                
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
                logbookQueryViewModel.SgkReleaseCars = mEntityService.QuerySgkReleaseCars.Where<SgkReleaseCar>
                (
                    entity =>
                        (string.IsNullOrEmpty(logbookQueryViewModel.KeyWord) ? true : (entity.PlateNumber.Contains(logbookQueryViewModel.KeyWord)))
                            &&
                        (entity.ReleaseTime.Value >= logbookQueryViewModel.StartDate)
                            &&
                        (entity.ReleaseTime.Value <= logbookQueryViewModel.EndDate)
                            &&
                        (entity.IsDeleted == false)
                );

                if (logbookQueryViewModel.SgkReleaseCars == null || logbookQueryViewModel.SgkReleaseCars.Count() == 0)
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

                if (logbookQueryViewModel.Operation == "Approve")
                {

                    if (logbookUpdateViewModel.ReleaseCarEntity.Changes == "相关单位审核")
                    {
                        if (logbookUpdateViewModel.ReleaseCarEntity.IsChargeSigned == false) // 事故科长未签名
                        {
                            saver = false;
                        }
                    }
                    else if (logbookUpdateViewModel.ReleaseCarEntity.Changes == "分管副大队审核")
                    {
                        if (logbookUpdateViewModel.ReleaseCarEntity.IsSubLeader1Signed == false) // 救援中心科长未签名
                        {
                            saver = false;
                        }
                    }
                    else if (logbookUpdateViewModel.ReleaseCarEntity.Changes == "完成")
                    {
                        if (logbookUpdateViewModel.ReleaseCarEntity.IsSubLeader2Signed == false) // 分管副大队未签名
                        {
                            saver = false;
                        }
                    }

                    if (saver == false)
                    {
                        MessageBox.Show("请签名后再提交", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return saver;
                    }
                }

                if (logbookUpdateViewModel.ReleaseCarEntity.Id > 0)
                {
                    logbookUpdateViewModel.ReleaseCarEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.ReleaseCarEntity.UpdateTime = System.DateTime.Now;

                }
                else
                {
                    logbookUpdateViewModel.ReleaseCarEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.ReleaseCarEntity.CreateTime = System.DateTime.Now;
                    logbookUpdateViewModel.ReleaseCarEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.ReleaseCarEntity.UpdateTime = System.DateTime.Now;
                    logbookUpdateViewModel.ReleaseCarEntity.IsDeleted = false;
                    logbookUpdateViewModel.ReleaseCarEntity.IntSpare1 = 0;    // no use, just pass a non-null value to it

                    mEntityService.Entities.SgkReleaseCars.AddObject(logbookUpdateViewModel.ReleaseCarEntity);
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

            if (ValidateUtil.IsBlank(logbookUpdateViewModel.ReleaseCarEntity.PlateNumber)
                || ValidateUtil.IsBlank(logbookUpdateViewModel.ReleaseCarEntity.IdNumber))
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

            logbookQueryViewModel.Operation = "Approve";

            ClearSignImgCache();

            try
            {
                logbookUpdateViewModel.ReleaseCarEntity = logbookQueryViewModel.SelectedYellowMarkCar;

                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanPrintVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanChargeInfoVisible = System.Windows.Visibility.Visible;
                

                logbookUpdateViewModel.IsTitleReadOnly = true;
                logbookUpdateViewModel.CanDepartEnable = false;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = true;


                if (logbookUpdateViewModel.ReleaseCarEntity.Changes == "事故科长审核")
                {
                    logbookUpdateViewModel.CanChargeSign1Visible = System.Windows.Visibility.Visible;
                    logbookUpdateViewModel.CanChargeSign2Visible = System.Windows.Visibility.Collapsed;
                    logbookUpdateViewModel.CanChargeSign3Visible = System.Windows.Visibility.Collapsed;

                    logbookUpdateViewModel.ReleaseCarEntity.Changes = "相关单位审核";
                }
                else if (logbookUpdateViewModel.ReleaseCarEntity.Changes == "相关单位审核")
                {
                    logbookUpdateViewModel.CanChargeSign1Visible = System.Windows.Visibility.Collapsed;
                    logbookUpdateViewModel.CanChargeSign2Visible = System.Windows.Visibility.Visible;
                    logbookUpdateViewModel.CanChargeSign3Visible = System.Windows.Visibility.Collapsed;

                    logbookUpdateViewModel.ReleaseCarEntity.Changes = "分管副大队审核";
                }
                else if (logbookUpdateViewModel.ReleaseCarEntity.Changes == "分管副大队审核")
                {
                    logbookUpdateViewModel.CanChargeSign1Visible = System.Windows.Visibility.Collapsed;
                    logbookUpdateViewModel.CanChargeSign2Visible = System.Windows.Visibility.Collapsed;
                    logbookUpdateViewModel.CanChargeSign3Visible = System.Windows.Visibility.Visible;

                    logbookUpdateViewModel.IsApprovalInfoReadOnly = false;

                    logbookUpdateViewModel.ReleaseCarEntity.Changes = "完成";

                    logbookUpdateViewModel.ReleaseCarEntity.ApproveResult = "同意";
                    logbookUpdateViewModel.ReleaseCarEntity.ApprovalName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    logbookUpdateViewModel.ReleaseCarEntity.ApprovalTime = System.DateTime.Now;
                }

                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }

        private bool PrintOper()
        {
            bool result = true;

            try
            {
                sgkReleasePrintViewModel.ReleaseCarEntity = logbookUpdateViewModel.ReleaseCarEntity;

                try
                {
                    sgkReleasePrintViewModel.ReleaseCarEntity.ChargeSignatureImg = ImageUtil.BitmapimageToBytes(logbookUpdateViewModel.SgkChargeSignImg);
                    sgkReleasePrintViewModel.ReleaseCarEntity.PersonSignatureImg = ImageUtil.BitmapimageToBytes(logbookUpdateViewModel.RescueChargeSignImg);
                    sgkReleasePrintViewModel.ReleaseCarEntity.FDDSignatureImg = ImageUtil.BitmapimageToBytes(logbookUpdateViewModel.FDDSignImg);
                }
                catch (System.Exception ex)
                {
                    CurrentLoginService.Instance.LogException(ex);
                }

                mainFrameViewModel.ContentView = sgkReleasePrintViewModel.View;

                sgkReleasePrintViewModel.ReloadGatherReport();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }


        private bool PrintRetreatOper()
        {
            bool newer = true;

            try
            {
                ClearSignImgCache();

                mainFrameViewModel.ContentView = logbookUpdateViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public void Close()
        {
            try
            {
                if (sgkReleasePrintViewModel != null)
                {
                    sgkReleasePrintViewModel.Close();
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool ChargeSignOper()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(WorkerChargeSign);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerChargeSignCompleted);

            worker.RunWorkerAsync();

            logbookUpdateViewModel.Show_LoadingMask();
            return true;
        }

        private void WorkerChargeSign(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;

            try
            {
                e.Result = false;
                ftp = new FtpHelper();

                //if (logbookUpdateViewModel.ChargeSignImg != null)
                //{
                //    logbookUpdateViewModel.ChargeSignImg.StreamSource.Dispose();
                //    logbookUpdateViewModel.ChargeSignImg = null;
                //}

                BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo, CurrentLoginService.Instance.CurrentUserInfo.Id);

                if (logbookUpdateViewModel.WhosSign == 1)
                {
                    logbookUpdateViewModel.SgkChargeSignImg = ftp.DownloadFile(user.AnswerQuestion);
                    logbookUpdateViewModel.ReleaseCarEntity.ChargeId = CurrentLoginService.Instance.CurrentUserInfo.Id;
                    logbookUpdateViewModel.ReleaseCarEntity.IsChargeSigned = true;
                }
                if (logbookUpdateViewModel.WhosSign == 2)
                {
                    logbookUpdateViewModel.RescueChargeSignImg = ftp.DownloadFile(user.AnswerQuestion);
                    logbookUpdateViewModel.ReleaseCarEntity.Node1Comment = CurrentLoginService.Instance.CurrentUserInfo.Id;
                    logbookUpdateViewModel.ReleaseCarEntity.IsSubLeader1Signed = true;
                }
                if (logbookUpdateViewModel.WhosSign == 3)
                {
                    logbookUpdateViewModel.FDDSignImg = ftp.DownloadFile(user.AnswerQuestion);
                    logbookUpdateViewModel.ReleaseCarEntity.Node2Comment = CurrentLoginService.Instance.CurrentUserInfo.Id;
                    logbookUpdateViewModel.ReleaseCarEntity.IsSubLeader2Signed = true;
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

        private void WorkerChargeSignCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            logbookUpdateViewModel.Shutdown_LoadingMask();
        }

        public bool ShowSignImg()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(WorkerShowSignImg);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowSignImgCompleted);

            worker.RunWorkerAsync();

            logbookUpdateViewModel.Show_LoadingMask();
            return true;
        }

        private void WorkerShowSignImg(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;

            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (logbookUpdateViewModel.ReleaseCarEntity.IsChargeSigned)
                {
                    if (logbookUpdateViewModel.SgkChargeSignImg != null)
                    {
                        logbookUpdateViewModel.SgkChargeSignImg.StreamSource.Dispose();
                        logbookUpdateViewModel.SgkChargeSignImg = null;
                    }
                    BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo,
                        logbookUpdateViewModel.ReleaseCarEntity.ChargeId);

                    if (user != null)
                    {
                        logbookUpdateViewModel.SgkChargeSignImg = ftp.DownloadFile(user.AnswerQuestion);
                    }

                    /*
                    BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo,
                        mRequestUpdateViewModel.EquipmentRequestEntity.SubLeaderId.ToString());
                    mRequestUpdateViewModel.SubLeaderSignImg = ftp.DownloadFile(user.AnswerQuestion);
                     */
                }
                if (logbookUpdateViewModel.ReleaseCarEntity.IsSubLeader1Signed)
                {
                    if (logbookUpdateViewModel.RescueChargeSignImg != null)
                    {
                        logbookUpdateViewModel.RescueChargeSignImg.StreamSource.Dispose();
                        logbookUpdateViewModel.RescueChargeSignImg = null;
                    }
                    BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo,
                        logbookUpdateViewModel.ReleaseCarEntity.Node1Comment);

                    if (user != null)
                    {
                        logbookUpdateViewModel.RescueChargeSignImg = ftp.DownloadFile(user.AnswerQuestion);
                    }
                }
                if (logbookUpdateViewModel.ReleaseCarEntity.IsSubLeader2Signed)
                {
                    if (logbookUpdateViewModel.FDDSignImg != null)
                    {
                        logbookUpdateViewModel.FDDSignImg.StreamSource.Dispose();
                        logbookUpdateViewModel.FDDSignImg = null;
                    }
                    BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo,
                        logbookUpdateViewModel.ReleaseCarEntity.Node2Comment);

                    if (user != null)
                    {
                        logbookUpdateViewModel.FDDSignImg = ftp.DownloadFile(user.AnswerQuestion);
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

            logbookUpdateViewModel.Shutdown_LoadingMask();
        }


        private void ClearSignImgCache()
        {
            if (logbookUpdateViewModel.SgkChargeSignImg != null)
            {
                logbookUpdateViewModel.SgkChargeSignImg.StreamSource.Dispose();
                logbookUpdateViewModel.SgkChargeSignImg = null;
            }

            if (logbookUpdateViewModel.RescueChargeSignImg != null)
            {
                logbookUpdateViewModel.RescueChargeSignImg.StreamSource.Dispose();
                logbookUpdateViewModel.RescueChargeSignImg = null;
            }

            if (logbookUpdateViewModel.FDDSignImg != null)
            {
                logbookUpdateViewModel.FDDSignImg.StreamSource.Dispose();
                logbookUpdateViewModel.FDDSignImg = null;
            }
        }
    }
}
    