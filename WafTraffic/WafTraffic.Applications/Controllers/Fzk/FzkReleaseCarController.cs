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
    internal class FzkReleaseCarController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mainFrameViewModel;
        private FzkReleaseCarUpdateViewModel logbookUpdateViewModel;
        private FzkReleaseCarQueryViewModel logbookQueryViewModel;
        private FzkReleaseCarPrintViewModel fzkReleasePrintViewModel;
        //private LbStaticLogbookDetailsViewModel mStaticLogbookDetailsViewModel;
        private LbStaticLogbookDocViewModel mStaticLogbookDocViewModel;

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
        private readonly DelegateCommand downloadCommand;
        private readonly DelegateCommand displayCommand;

        private string mFileFrom;
        private string mFileTo;

        [ImportingConstructor]
        public FzkReleaseCarController(CompositionContainer container, 
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
                logbookUpdateViewModel = container.GetExportedValue<FzkReleaseCarUpdateViewModel>();
                logbookQueryViewModel = container.GetExportedValue<FzkReleaseCarQueryViewModel>();
                fzkReleasePrintViewModel = container.GetExportedValue<FzkReleaseCarPrintViewModel>();
                //mStaticLogbookDetailsViewModel = container.GetExportedValue<LbStaticLogbookDetailsViewModel>();
                mStaticLogbookDocViewModel = container.GetExportedValue<LbStaticLogbookDocViewModel>();
                

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
                this.downloadCommand = new DelegateCommand(() => DownloadOper(), null);
                this.displayCommand = new DelegateCommand(() => DisplayOper(), null);
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
                logbookUpdateViewModel.DownloadCommand = this.downloadCommand;
                logbookUpdateViewModel.DisplayCommand = this.displayCommand;

                logbookQueryViewModel.NewCommand = this.mNewCommand;
                logbookQueryViewModel.ModifyCommand = this.mModifyCommand;
                logbookQueryViewModel.DeleteCommand = this.mDeleteCommand;
                logbookQueryViewModel.QueryCommand = this.mQueryCommand;
                logbookQueryViewModel.BrowseCommand = this.mBrowseCommand;
                logbookQueryViewModel.ApproveCommand = this.mApproveCommand;

                fzkReleasePrintViewModel.PrintRetreatCommand = this.mPrintRetreatCommand;
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
                logbookUpdateViewModel.CanPrintVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanChargeSignVisible = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsTitleReadOnly = false;
                logbookUpdateViewModel.CanDepartEnable = true;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = true;

                logbookUpdateViewModel.ReleaseCarEntity = new FzkReleaseCar();

                //BaseOrganizeEntity org = logbookUpdateViewModel.DepartmentList.Find(
                //    instance => (instance.Id == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId)

                //);

                //if (org != null)
                //{
                //    logbookUpdateViewModel.ReleaseCarEntity.PersonDepartmentId = org.Id;
                //    logbookUpdateViewModel.ReleaseCarEntity.PersonDepartmentName = org.FullName;
                //}

                logbookUpdateViewModel.ReleaseCarEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                //logbookUpdateViewModel.ReleaseCarEntity.CreateName = AuthService.Instance.GetUserNameById(logbookUpdateViewModel.ReleaseCarEntity.CreateId);


                //logbookUpdateViewModel.ReleaseCarEntity.PersonName = AuthService.Instance.GetUserNameById(logbookUpdateViewModel.ReleaseCarEntity.CreateId);
                //logbookUpdateViewModel.ReleaseCarEntity.Applicant = logbookUpdateViewModel.ReleaseCarEntity.PersonName;

                //logbookUpdateViewModel.ReleaseCarEntity.ApplicationDate = System.DateTime.Now;

                if (logbookUpdateViewModel.ChargeSignImg != null)
                {
                    logbookUpdateViewModel.ChargeSignImg.StreamSource.Dispose();
                    logbookUpdateViewModel.ChargeSignImg = null;
                }

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
                logbookUpdateViewModel.CanPrintVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.CanChargeSignVisible = System.Windows.Visibility.Collapsed;

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

                if (logbookUpdateViewModel.ChargeSignImg != null)
                {
                    logbookUpdateViewModel.ChargeSignImg.StreamSource.Dispose();
                    logbookUpdateViewModel.ChargeSignImg = null;
                }

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

            try
            {
                logbookUpdateViewModel.ReleaseCarEntity = logbookQueryViewModel.SelectedYellowMarkCar;

                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanPrintVisibal = System.Windows.Visibility.Visible;

                logbookUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Collapsed;

                if (!string.IsNullOrEmpty(logbookQueryViewModel.SelectedYellowMarkCar.StrSpare1))
                {
                    logbookUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Visible;
                }
                else
                {
                    logbookUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                }

                logbookUpdateViewModel.CanChargeSignVisible = System.Windows.Visibility.Collapsed;

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

                if (logbookUpdateViewModel.ChargeSignImg != null)
                {
                    logbookUpdateViewModel.ChargeSignImg.StreamSource.Dispose();
                    logbookUpdateViewModel.ChargeSignImg = null;
                }

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
                logbookQueryViewModel.FzkReleaseCars = mEntityService.QueryFzkReleaseCars.Where<FzkReleaseCar>
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

                if (logbookQueryViewModel.FzkReleaseCars == null || logbookQueryViewModel.FzkReleaseCars.Count() == 0)
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

            FtpHelper ftp = null;

            if (!ValueCheck())
            {
                return false;
            }

            try
            {
                ftp = new FtpHelper();

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

                    logbookUpdateViewModel.ReleaseCarEntity.StrSpare2 = ftp.UploadFile(FtpFileType.Document, logbookUpdateViewModel.UploadFullPath);
                    if (ValidateUtil.IsBlank(logbookUpdateViewModel.ReleaseCarEntity.StrSpare2))
                    {
                        throw new ValidationException();
                    }

                    mEntityService.Entities.FzkReleaseCars.AddObject(logbookUpdateViewModel.ReleaseCarEntity);
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

            if (logbookUpdateViewModel.ReleaseCarEntity.ApproveResult != null && logbookUpdateViewModel.ReleaseCarEntity.IsChargeSigned == false)
            {
                MessageBox.Show("负责人没有签名", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                logbookUpdateViewModel.ReleaseCarEntity = logbookQueryViewModel.SelectedYellowMarkCar;

                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanPrintVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanChargeInfoVisible = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanChargeSignVisible = System.Windows.Visibility.Visible;

                logbookUpdateViewModel.IsTitleReadOnly = true;
                logbookUpdateViewModel.CanDepartEnable = false;
                logbookUpdateViewModel.IsApprovalInfoReadOnly = false;

                logbookUpdateViewModel.ReleaseCarEntity.ApproveResult = "同意";
                logbookUpdateViewModel.ReleaseCarEntity.ApprovalName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                logbookUpdateViewModel.ReleaseCarEntity.ApprovalTime = System.DateTime.Now;

                if (logbookUpdateViewModel.ChargeSignImg != null)
                {
                    logbookUpdateViewModel.ChargeSignImg.StreamSource.Dispose();
                    logbookUpdateViewModel.ChargeSignImg = null;
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
                fzkReleasePrintViewModel.ReleaseCarEntity = logbookUpdateViewModel.ReleaseCarEntity;

                try
                {
                    fzkReleasePrintViewModel.ReleaseCarEntity.ChargeSignatureImg = ImageUtil.BitmapimageToBytes(logbookUpdateViewModel.ChargeSignImg);

                }
                catch (System.Exception ex)
                {
                    CurrentLoginService.Instance.LogException(ex);
                }

                mainFrameViewModel.ContentView = fzkReleasePrintViewModel.View;

                fzkReleasePrintViewModel.ReloadGatherReport();
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
                if (fzkReleasePrintViewModel != null)
                {
                    fzkReleasePrintViewModel.Close();
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
                if (logbookUpdateViewModel.ChargeSignImg != null)
                {
                    logbookUpdateViewModel.ChargeSignImg.StreamSource.Dispose();
                    logbookUpdateViewModel.ChargeSignImg = null;
                }
                BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo, CurrentLoginService.Instance.CurrentUserInfo.Id);
                logbookUpdateViewModel.ChargeSignImg = ftp.DownloadFile(user.AnswerQuestion);
                logbookUpdateViewModel.ReleaseCarEntity.ChargeId = CurrentLoginService.Instance.CurrentUserInfo.Id;
                logbookUpdateViewModel.ReleaseCarEntity.IsChargeSigned = true;
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
                    //if (logbookUpdateViewModel.ChargeSignImg != null)
                    //{
                    //    logbookUpdateViewModel.ChargeSignImg.StreamSource.Dispose();
                    //    logbookUpdateViewModel.ChargeSignImg = null;
                    //}
                    BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo,
                        logbookUpdateViewModel.ReleaseCarEntity.ChargeId);

                    if (user != null)
                    {
                        logbookUpdateViewModel.ChargeSignImg = ftp.DownloadFile(user.AnswerQuestion);
                    }

                    /*
                    BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo,
                        mRequestUpdateViewModel.EquipmentRequestEntity.SubLeaderId.ToString());
                    mRequestUpdateViewModel.SubLeaderSignImg = ftp.DownloadFile(user.AnswerQuestion);
                     */
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

        public bool DownloadOper()
        {
            bool newer = true;
            try
            {
                SaveFileDialog sf = new SaveFileDialog();

                sf.AddExtension = true;
                sf.RestoreDirectory = true;
                sf.FileName = logbookUpdateViewModel.ReleaseCarEntity.StrSpare1;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    mFileFrom = logbookUpdateViewModel.ReleaseCarEntity.StrSpare2;
                    mFileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    logbookUpdateViewModel.Show_LoadingMask();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("下载失败，文件未找到或已过期", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        private void worker_Download(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;
            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                e.Result = ftp.DownloadFile(mFileFrom, mFileTo);
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

        private void worker_DownloadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                bool ret = (bool)e.Result;

                logbookUpdateViewModel.Shutdown_LoadingMask();

                if (ret)
                {
                    MessageBox.Show("下载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("下载失败，文件未找到或已过期", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool DisplayOper()
        {
            bool newer = true;

            try
            {
                string tempPath = System.IO.Path.GetTempPath();
                mFileFrom = logbookUpdateViewModel.ReleaseCarEntity.StrSpare2;
                mFileTo = tempPath + logbookUpdateViewModel.ReleaseCarEntity.StrSpare1;
                mStaticLogbookDocViewModel.FilePath = mFileTo;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_Display);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DisplayCompleted);
                worker.RunWorkerAsync();

                logbookUpdateViewModel.Show_LoadingMask();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        private void worker_Display(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;
            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                e.Result = ftp.DownloadFile(mFileFrom, mFileTo);
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

        private void worker_DisplayCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                bool ret = (bool)e.Result;

                logbookUpdateViewModel.Shutdown_LoadingMask();
                if (ret)
                {
                    mStaticLogbookDocViewModel.CloseCommand = this.mCancelCommand;
                    mainFrameViewModel.ContentView = mStaticLogbookDocViewModel.View;
                }
                else
                {
                    MessageBox.Show("发生错误，打开失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

    }
}
    