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
    internal class ZgxcAssistantCheckinController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mainFrameViewModel;
        private ZgxcAssistantCheckinUpdateViewModel logbookUpdateViewModel;
        private ZgxcAssistantCheckinQueryViewModel logbookQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mDownloadCommand;

        private string mFileFrom;
        private string mFileTo;

        [ImportingConstructor]
        public ZgxcAssistantCheckinController(CompositionContainer container, 
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
                logbookUpdateViewModel = container.GetExportedValue<ZgxcAssistantCheckinUpdateViewModel>();
                logbookQueryViewModel = container.GetExportedValue<ZgxcAssistantCheckinQueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mDownloadCommand = new DelegateCommand(() => DownloadOper(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
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
                logbookUpdateViewModel.DownloadCommand = this.mDownloadCommand;
                logbookUpdateViewModel.UploadFullPath = string.Empty;
                logbookUpdateViewModel.Shutdown_LoadingMask();

                logbookQueryViewModel.NewCommand = this.mNewCommand;
                logbookQueryViewModel.ModifyCommand = this.mModifyCommand;
                logbookQueryViewModel.DeleteCommand = this.mDeleteCommand;
                logbookQueryViewModel.QueryCommand = this.mQueryCommand;
                logbookQueryViewModel.BrowseCommand = this.mBrowseCommand;
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
                logbookUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsRecordDateReadOnly = false;
                logbookUpdateViewModel.IsTitleReadOnly = false;

                logbookUpdateViewModel.AssistantCheckinEntity = new ZgxcAssistantCheckin();
                logbookUpdateViewModel.AssistantCheckinEntity.RecordDate = DateTime.Today;
                logbookUpdateViewModel.AssistantCheckinEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                logbookUpdateViewModel.AssistantCheckinEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(logbookUpdateViewModel.AssistantCheckinEntity.OwnDepartmentId);
                logbookUpdateViewModel.AssistantCheckinEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                logbookUpdateViewModel.AssistantCheckinEntity.CreateName = AuthService.Instance.GetUserNameById(logbookUpdateViewModel.AssistantCheckinEntity.CreateId);
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
                logbookUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                logbookUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;

                logbookUpdateViewModel.IsRecordDateReadOnly = false;
                logbookUpdateViewModel.IsTitleReadOnly = false;

                logbookUpdateViewModel.AssistantCheckinEntity = logbookQueryViewModel.SelectedAssistantCheckin;
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
            FtpHelper ftp = null;
            try
            {
                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    ftp = new FtpHelper();
                    ftp.RemoveFile(logbookQueryViewModel.SelectedAssistantCheckin.Content);
                    logbookQueryViewModel.SelectedAssistantCheckin.IsDeleted = true;
                    logbookQueryViewModel.SelectedAssistantCheckin.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookQueryViewModel.SelectedAssistantCheckin.UpdateTime = System.DateTime.Now;
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
            finally
            {
                if (ftp != null)
                {
                    ftp.Disconnect();
                    ftp = null;
                }
            }

            return deleter;
        }

        //public bool DisplayOper()
        //{
        //    bool newer = true;

        //    try
        //    {
        //        string tempPath = System.IO.Path.GetTempPath();
        //        mFileFrom = logbookDetailsViewModel.AssistantCheckinEntity.Content;
        //        mFileTo = tempPath + logbookDetailsViewModel.AssistantCheckinEntity.UploadFileName;
        //        logbookDocViewModel.FilePath = mFileTo;

        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.DoWork += new DoWorkEventHandler(worker_Display);
        //        worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DisplayCompleted);
        //        worker.RunWorkerAsync();

        //        logbookDetailsViewModel.Show_LoadingMask();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return newer;
        //}

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

        //private void worker_DisplayCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    try
        //    {
        //        bool ret = (bool)e.Result;

        //        if (ret)
        //        {
        //            mainFrameViewModel.ContentView = logbookDocViewModel.View;
        //        }
        //        else
        //        {
        //            MessageBox.Show("发生错误，打开失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }
        //}

        public bool CloseOper()
        {
            bool newer = true;

            try
            {

            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool DownloadOper()
        {
            bool newer = true;
            try
            {
                SaveFileDialog sf = new SaveFileDialog();

                sf.AddExtension = true;
                sf.RestoreDirectory = true;
                sf.FileName = logbookUpdateViewModel.AssistantCheckinEntity.UploadFileName;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    mFileFrom = logbookUpdateViewModel.AssistantCheckinEntity.Content;
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

        public bool UploadOper()
        {
            bool newer = true;
           
            return newer;
        }

        public bool BrowseOper()
        {
            bool newer = true;

            try
            {
                logbookUpdateViewModel.CanSaveVisibal = System.Windows.Visibility.Collapsed;
                logbookUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Collapsed;

                if (!string.IsNullOrEmpty(logbookQueryViewModel.SelectedAssistantCheckin.UploadFileName))
                {
                    logbookUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Visible;
                }
                else
                {
                    logbookUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                }

                logbookUpdateViewModel.IsRecordDateReadOnly = true;
                logbookUpdateViewModel.IsTitleReadOnly = true;

                logbookUpdateViewModel.AssistantCheckinEntity = logbookQueryViewModel.SelectedAssistantCheckin;
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
                logbookQueryViewModel.ZgxcAssistantCheckins = mEntityService.QueryableAssistantCheckins.Where<ZgxcAssistantCheckin>
                (
                    entity =>
                        ((logbookQueryViewModel.SelectDepartId == 0) ? true : (entity.OwnDepartmentId == logbookQueryViewModel.SelectDepartId))
                            &&
                        (entity.RecordDate.Value >= logbookQueryViewModel.StartDate)
                            &&
                        (entity.RecordDate.Value <= logbookQueryViewModel.EndDate)
                            &&
                        (string.IsNullOrEmpty(logbookQueryViewModel.KeyWord)? true : (entity.Title.Contains(logbookQueryViewModel.KeyWord)))
                            &&
                        (entity.IsDeleted == false)
                );

                if (logbookQueryViewModel.ZgxcAssistantCheckins == null || logbookQueryViewModel.ZgxcAssistantCheckins.Count() == 0)
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

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_Save);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_SaveCompleted);

            worker.RunWorkerAsync();

            logbookUpdateViewModel.Show_LoadingMask();

            return saver;
        }

        private void worker_Save(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;

            try
            {
                e.Result = false;

                ftp = new FtpHelper();
                if (logbookUpdateViewModel.AssistantCheckinEntity.Id > 0)
                {
                    if (!string.IsNullOrEmpty(logbookUpdateViewModel.UploadFullPath))
                    {
                        string oldFile = logbookUpdateViewModel.AssistantCheckinEntity.Content;
                        logbookUpdateViewModel.AssistantCheckinEntity.Content = ftp.UpdateFile(FtpFileType.Document, logbookUpdateViewModel.UploadFullPath, oldFile);
                        if (ValidateUtil.IsBlank(logbookUpdateViewModel.AssistantCheckinEntity.Content))
                        {
                            throw new ValidationException();
                        }
                    }
                    logbookUpdateViewModel.AssistantCheckinEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.AssistantCheckinEntity.UpdateTime = System.DateTime.Now;
                }
                else
                {
                    logbookUpdateViewModel.AssistantCheckinEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    logbookUpdateViewModel.AssistantCheckinEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.AssistantCheckinEntity.CreateTime = System.DateTime.Now;
                    logbookUpdateViewModel.AssistantCheckinEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    logbookUpdateViewModel.AssistantCheckinEntity.UpdateTime = System.DateTime.Now;
                    logbookUpdateViewModel.AssistantCheckinEntity.IsDeleted = false;

                    logbookUpdateViewModel.AssistantCheckinEntity.Content = ftp.UploadFile(FtpFileType.Document, logbookUpdateViewModel.UploadFullPath);
                    if (ValidateUtil.IsBlank(logbookUpdateViewModel.AssistantCheckinEntity.Content))
                    {
                        throw new ValidationException();
                    }

                    mEntityService.Entities.ZgxcAssistantCheckins.AddObject(logbookUpdateViewModel.AssistantCheckinEntity);
                }
                mEntityService.Entities.SaveChanges();
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

        private void worker_SaveCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;
            
            logbookUpdateViewModel.Shutdown_LoadingMask();
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                //RefreshQueryData();
                mainFrameViewModel.ContentView = logbookQueryViewModel.View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValueCheck()
        {
            bool result = true;

            if (logbookUpdateViewModel.AssistantCheckinEntity.RecordDate == null 
                || ValidateUtil.IsBlank(logbookUpdateViewModel.AssistantCheckinEntity.RecordDate.ToString())
                || ValidateUtil.IsBlank(logbookUpdateViewModel.AssistantCheckinEntity.Title))
            {
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
                return result;
            }

            if (string.IsNullOrEmpty(logbookUpdateViewModel.AssistantCheckinEntity.UploadFileName))
            {
                MessageBox.Show("请选择上传文件。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
                return result;
            }

            return result;
        }

        //public void Close()
        //{
        //    try
        //    {
        //        if (logbookDocViewModel != null)
        //        {
        //            logbookDocViewModel.Close();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }
        //}
    }
}
    