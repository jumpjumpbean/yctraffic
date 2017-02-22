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
    internal class LbStaticLogbookController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private LogbookMainViewModel mLogbookMainViewModel;
        private LbStaticLogbookUpdateViewModel mStaticLogbookUpdateViewModel;
        private LbStaticLogbookQueryViewModel mStaticLogbookQueryViewModel;
        private LbStaticLogbookDetailsViewModel mStaticLogbookDetailsViewModel;
        private LbStaticLogbookDocViewModel mStaticLogbookDocViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mUploadCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mDisplayCommand;
        private readonly DelegateCommand mDownloadCommand;
        private readonly DelegateCommand mCloseCommand;

        private string mFileFrom;
        private string mFileTo;

        [ImportingConstructor]
        public LbStaticLogbookController(CompositionContainer container, 
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
                mStaticLogbookUpdateViewModel = container.GetExportedValue<LbStaticLogbookUpdateViewModel>();
                mStaticLogbookQueryViewModel = container.GetExportedValue<LbStaticLogbookQueryViewModel>();
                mStaticLogbookDetailsViewModel = container.GetExportedValue<LbStaticLogbookDetailsViewModel>();
                mStaticLogbookDocViewModel = container.GetExportedValue<LbStaticLogbookDocViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mUploadCommand = new DelegateCommand(() => UploadOper(), null);
                this.mDisplayCommand = new DelegateCommand(() => DisplayOper(), null);
                this.mDownloadCommand = new DelegateCommand(() => DownloadOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mCloseCommand = new DelegateCommand(() => CloseOper(), null);
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
                mStaticLogbookUpdateViewModel.SaveCommand = this.mSaveCommand;
                mStaticLogbookUpdateViewModel.CancelCommand = this.mCancelCommand;
                mStaticLogbookUpdateViewModel.UploadCommand = this.mUploadCommand;
                mStaticLogbookUpdateViewModel.UploadFullPath = string.Empty;
                mStaticLogbookUpdateViewModel.Shutdown_LoadingMask();

                mStaticLogbookDetailsViewModel.DisplayCommand = this.mDisplayCommand;
                mStaticLogbookDetailsViewModel.DownloadCommand = this.mDownloadCommand;
                mStaticLogbookDetailsViewModel.CancelCommand = this.mCancelCommand;
                mStaticLogbookDetailsViewModel.Shutdown_LoadingMask();

                //mStaticLogbookDocViewModel.CloseCommand = this.mCloseCommand;

                mStaticLogbookQueryViewModel.NewCommand = this.mNewCommand;
                mStaticLogbookQueryViewModel.ModifyCommand = this.mModifyCommand;
                mStaticLogbookQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mStaticLogbookQueryViewModel.QueryCommand = this.mQueryCommand;
                mStaticLogbookQueryViewModel.BrowseCommand = this.mBrowseCommand;
                RefreshQueryData();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void RefreshQueryData()
        {
            try
            {
                mStaticLogbookQueryViewModel.LbStaticLogbooks = this.mEntityService.EnumLbStatics(mLogbookMainViewModel.SelectedLogbook.Id);
                mStaticLogbookQueryViewModel.GridRefresh();
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
                mStaticLogbookUpdateViewModel.StaticLogbookEntity = new ZdtzStaticTable();
                mStaticLogbookUpdateViewModel.StaticLogbookEntity.RecordDate = DateTime.Today;
                mStaticLogbookUpdateViewModel.StaticLogbookEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mStaticLogbookUpdateViewModel.StaticLogbookEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mStaticLogbookUpdateViewModel.StaticLogbookEntity.OwnDepartmentId);
                mStaticLogbookUpdateViewModel.StaticLogbookEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mStaticLogbookUpdateViewModel.StaticLogbookEntity.CreateName = AuthService.Instance.GetUserNameById(mStaticLogbookUpdateViewModel.StaticLogbookEntity.CreateId);
                mLogbookMainViewModel.ContentView = mStaticLogbookUpdateViewModel.View;
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
                mStaticLogbookUpdateViewModel.StaticLogbookEntity = mStaticLogbookQueryViewModel.SelectedStaticLogbook;
                mLogbookMainViewModel.ContentView = mStaticLogbookUpdateViewModel.View;
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
                    ftp.RemoveFile(mStaticLogbookQueryViewModel.SelectedStaticLogbook.Content);
                    mStaticLogbookQueryViewModel.SelectedStaticLogbook.IsDeleted = YcConstants.INT_DB_DATA_DELETED;
                    mStaticLogbookQueryViewModel.SelectedStaticLogbook.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mStaticLogbookQueryViewModel.SelectedStaticLogbook.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    mStaticLogbookQueryViewModel.GridRefresh();
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

        public bool DisplayOper()
        {
            bool newer = true;

            try
            {
                string tempPath = System.IO.Path.GetTempPath();
                mFileFrom = mStaticLogbookDetailsViewModel.StaticLogbookEntity.Content;
                mFileTo = tempPath + mStaticLogbookDetailsViewModel.StaticLogbookEntity.UploadFileName;
                mStaticLogbookDocViewModel.FilePath = mFileTo;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_Display);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DisplayCompleted);
                worker.RunWorkerAsync();

                mStaticLogbookDetailsViewModel.Show_LoadingMask();
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

                mStaticLogbookDetailsViewModel.Shutdown_LoadingMask();
                if (ret)
                {
                    mLogbookMainViewModel.ContentView = mStaticLogbookDocViewModel.View;
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

        public bool CloseOper()
        {
            bool newer = true;

            try
            {
                mLogbookMainViewModel.ContentView = mStaticLogbookDetailsViewModel.View;
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
                sf.FileName = mStaticLogbookDetailsViewModel.StaticLogbookEntity.UploadFileName;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    mFileFrom = mStaticLogbookDetailsViewModel.StaticLogbookEntity.Content;
                    mFileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    mStaticLogbookDetailsViewModel.Show_LoadingMask();
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

                mStaticLogbookDetailsViewModel.Shutdown_LoadingMask();
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
                /*
                if (mStaticLogbookQueryViewModel.SelectedStaticLogbook != null && mStaticLogbookQueryViewModel.SelectedStaticLogbook.Id != 0)
                {
                    mEntityService.Entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, mStaticLogbookQueryViewModel.SelectedStaticLogbook);
                }
                */
                mLogbookMainViewModel.ContentView = mStaticLogbookQueryViewModel.View;
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
                mStaticLogbookDetailsViewModel.StaticLogbookEntity = mStaticLogbookQueryViewModel.SelectedStaticLogbook;
                mLogbookMainViewModel.ContentView = mStaticLogbookDetailsViewModel.View;
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
            string inputTitle;

            try
            {
                if (mStaticLogbookQueryViewModel.IsSelectDepartmentEnabled)
                {
                    deptId = Convert.ToInt32(mStaticLogbookQueryViewModel.SelectedDepartment);
                }
                else
                {
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }
                inputTitle = mStaticLogbookQueryViewModel.InputTitle;
                DateTime startDate = (DateTime)mStaticLogbookQueryViewModel.SelectedStartDate;
                DateTime endDate = (DateTime)mStaticLogbookQueryViewModel.SelectedEndDate;

                mStaticLogbookQueryViewModel.LbStaticLogbooks = mEntityService.QueryStatics(
                    mLogbookMainViewModel.SelectedLogbook.Id, Convert.ToInt32(deptId), startDate, endDate, inputTitle);

                if (mStaticLogbookQueryViewModel.LbStaticLogbooks == null || mStaticLogbookQueryViewModel.LbStaticLogbooks.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mStaticLogbookQueryViewModel.GridRefresh();
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

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_Save);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_SaveCompleted);

            worker.RunWorkerAsync();

            mStaticLogbookUpdateViewModel.Show_LoadingMask();

            return saver;
        }

        private void worker_Save(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;

            try
            {
                e.Result = false;
                if (!ValueCheck())
                {
                    return;
                }
                ftp = new FtpHelper();
                if (mStaticLogbookUpdateViewModel.StaticLogbookEntity.Id > 0)
                {
                    if (!string.IsNullOrEmpty(mStaticLogbookUpdateViewModel.UploadFullPath))
                    {
                        string oldFile = mStaticLogbookUpdateViewModel.StaticLogbookEntity.Content;
                        mStaticLogbookUpdateViewModel.StaticLogbookEntity.Content = ftp.UpdateFile(FtpFileType.Document, mStaticLogbookUpdateViewModel.UploadFullPath, oldFile);
                        if (ValidateUtil.IsBlank(mStaticLogbookUpdateViewModel.StaticLogbookEntity.Content))
                        {
                            throw new ValidationException();
                        }
                    }
                    mStaticLogbookUpdateViewModel.StaticLogbookEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mStaticLogbookUpdateViewModel.StaticLogbookEntity.UpdateTime = System.DateTime.Now;
                }
                else
                {
                    mStaticLogbookUpdateViewModel.StaticLogbookEntity.ConfigId = mLogbookMainViewModel.SelectedLogbook.Id;
                    mStaticLogbookUpdateViewModel.StaticLogbookEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    mStaticLogbookUpdateViewModel.StaticLogbookEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mStaticLogbookUpdateViewModel.StaticLogbookEntity.CreateTime = System.DateTime.Now;
                    mStaticLogbookUpdateViewModel.StaticLogbookEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mStaticLogbookUpdateViewModel.StaticLogbookEntity.UpdateTime = System.DateTime.Now;
                    mStaticLogbookUpdateViewModel.StaticLogbookEntity.IsDeleted = YcConstants.INT_DB_DATA_AVAILABLE;

                    mStaticLogbookUpdateViewModel.StaticLogbookEntity.Content = ftp.UploadFile(FtpFileType.Document, mStaticLogbookUpdateViewModel.UploadFullPath);
                    if (ValidateUtil.IsBlank(mStaticLogbookUpdateViewModel.StaticLogbookEntity.Content))
                    {
                        throw new ValidationException();
                    }

                    mEntityService.LbStatics.AddObject(mStaticLogbookUpdateViewModel.StaticLogbookEntity);
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
            
            mStaticLogbookUpdateViewModel.Shutdown_LoadingMask();
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                RefreshQueryData();
                mLogbookMainViewModel.ContentView = mStaticLogbookQueryViewModel.View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValueCheck()
        {
            bool result = true;

            if (mStaticLogbookUpdateViewModel.StaticLogbookEntity.RecordDate == null 
                || ValidateUtil.IsBlank(mStaticLogbookUpdateViewModel.StaticLogbookEntity.RecordDate.ToString())
                || ValidateUtil.IsBlank(mStaticLogbookUpdateViewModel.StaticLogbookEntity.Title))
            {
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }

            return result;
        }

        public void Close()
        {
            try
            {
                if (mStaticLogbookDocViewModel != null)
                {
                    mStaticLogbookDocViewModel.Close();
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }
    }
}
    