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
using WafTraffic.Applications.Utils;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class ZxkOrderController : Controller
    {
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private ZxkOrderUpdateViewModel mZxkOrderUpdateViewModel;
        private ZxkOrderQueryViewModel mZxkOrderQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mDownloadCommand;
        private readonly DelegateCommand mDownloadCommand2;

        private string fileFrom;
        private string fileTo;

        [ImportingConstructor]
        public ZxkOrderController(CompositionContainer container, IEntityService entityService)
        {
            try
            {
                this.mEntityService = entityService;

                mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                mZxkOrderUpdateViewModel = container.GetExportedValue<ZxkOrderUpdateViewModel>();
                mZxkOrderQueryViewModel = container.GetExportedValue<ZxkOrderQueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mDownloadCommand = new DelegateCommand(() => DownloadOper(), null);
                this.mDownloadCommand2 = new DelegateCommand(() => DownloadOper2(), null);
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
                mZxkOrderUpdateViewModel.SaveCommand = this.mSaveCommand;
                mZxkOrderUpdateViewModel.CancelCommand = this.mCancelCommand;
                mZxkOrderUpdateViewModel.DownloadCommand = this.mDownloadCommand;
                mZxkOrderUpdateViewModel.DownloadCommand2 = this.mDownloadCommand2;

                mZxkOrderQueryViewModel.NewCommand = this.mNewCommand;
                mZxkOrderQueryViewModel.ModifyCommand = this.mModifyCommand;
                mZxkOrderQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mZxkOrderQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mZxkOrderQueryViewModel.QueryCommand = this.mQueryCommand;
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
                mZxkOrderUpdateViewModel.IsBrowse = false;
                mZxkOrderUpdateViewModel.PunishCaseEntity = new ZxkOrderCase();
                mZxkOrderUpdateViewModel.PunishCaseEntity.CaseTime = DateTime.Now;
                mZxkOrderUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mZxkOrderUpdateViewModel.PunishCaseEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mZxkOrderUpdateViewModel.PunishCaseEntity.OwnDepartmentId);
                mZxkOrderUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mZxkOrderUpdateViewModel.PunishCaseEntity.CreateName = AuthService.Instance.GetUserNameById(mZxkOrderUpdateViewModel.PunishCaseEntity.CreateId);

                mZxkOrderUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                mZxkOrderUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                
                mMainFrameViewModel.ContentView = mZxkOrderUpdateViewModel.View;
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
                mZxkOrderUpdateViewModel.IsBrowse = false;
                mZxkOrderUpdateViewModel.PunishCaseEntity = mZxkOrderQueryViewModel.SelectedCase;

                mZxkOrderUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                mZxkOrderUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;

                mMainFrameViewModel.ContentView = mZxkOrderUpdateViewModel.View;
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
                mZxkOrderUpdateViewModel.IsBrowse = true;
                mZxkOrderUpdateViewModel.PunishCaseEntity = mZxkOrderQueryViewModel.SelectedCase;

                mZxkOrderUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Collapsed;

                if (!string.IsNullOrEmpty(mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentName))
                {
                    mZxkOrderUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Visible;
                }
                else
                {
                    mZxkOrderUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                }

                mMainFrameViewModel.ContentView = mZxkOrderUpdateViewModel.View;
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
                    mZxkOrderQueryViewModel.SelectedCase.IsDeleted = true;
                    mZxkOrderQueryViewModel.SelectedCase.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mZxkOrderQueryViewModel.SelectedCase.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    mZxkOrderQueryViewModel.GridRefresh();
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
                mMainFrameViewModel.ContentView = mZxkOrderQueryViewModel.View;
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
            //int deptId;
            try
            {
                /*
                if (mZxkOrderQueryViewModel.IsSelectDepartmentEnabled)
                {
                    deptId = Convert.ToInt32(mZxkOrderQueryViewModel.SelectedDepartment);
                }
                else
                {
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }*/

                DateTime startDate = (DateTime)mZxkOrderQueryViewModel.SelectedStartDate;
                DateTime endDate = (DateTime)mZxkOrderQueryViewModel.SelectedEndDate;

                mZxkOrderQueryViewModel.PunishCases = mEntityService.QueryZxkOrderCases(startDate, endDate);
                if (mZxkOrderQueryViewModel.PunishCases == null || mZxkOrderQueryViewModel.PunishCases.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mZxkOrderQueryViewModel.GridRefresh();
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

            if (!ValueCheck())
            {
                return saved;
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(WorkerSave);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerSaveCompleted);

            worker.RunWorkerAsync();

            mZxkOrderUpdateViewModel.Show_LoadingMask(LoadingType.View);

            return saved;
        }

        private void WorkerSave(object sender, DoWorkEventArgs e)
        {
            string oldFile;
            FtpHelper ftp = null;

            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (mZxkOrderUpdateViewModel.PunishCaseEntity.Id > 0)
                {
                    mZxkOrderUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mZxkOrderUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;

                    if (!string.IsNullOrEmpty(mZxkOrderUpdateViewModel.FileLocalPath))
                    {
                        oldFile = mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath;
                        mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath =
                            ftp.UpdateFile(FtpFileType.Document, mZxkOrderUpdateViewModel.FileLocalPath, oldFile);

                        oldFile = mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath2;
                        mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath2 =
                            ftp.UpdateFile(FtpFileType.Document, mZxkOrderUpdateViewModel.FileLocalPath2, oldFile);

                        if (string.IsNullOrEmpty(mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath) 
                            || string.IsNullOrEmpty(mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath2))
                        {
                            throw new ValidationException();
                        }
                    }          
                }
                else
                {
                    mZxkOrderUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    mZxkOrderUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mZxkOrderUpdateViewModel.PunishCaseEntity.CreateTime = System.DateTime.Now;

                    mZxkOrderUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mZxkOrderUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;
                    mZxkOrderUpdateViewModel.PunishCaseEntity.IsDeleted = false;

                    if (!string.IsNullOrEmpty(mZxkOrderUpdateViewModel.FileLocalPath))
                    {

                        mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath =
                            ftp.UploadFile(FtpFileType.Document, mZxkOrderUpdateViewModel.FileLocalPath);

                        mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath2 =
                            ftp.UploadFile(FtpFileType.Document, mZxkOrderUpdateViewModel.FileLocalPath2);

                        if (string.IsNullOrEmpty(mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath) ||
                            string.IsNullOrEmpty(mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath2))
                        {
                            throw new ValidationException();
                        }
                    }

                    mEntityService.Entities.ZxkOrderCases.AddObject(mZxkOrderUpdateViewModel.PunishCaseEntity);
                }

                e.Result = true;
                mEntityService.Entities.SaveChanges();
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

        private void WorkerSaveCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            mZxkOrderUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mMainFrameViewModel.ContentView = mZxkOrderQueryViewModel.View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValueCheck()
        {
            if (mZxkOrderUpdateViewModel.PunishCaseEntity.CaseTime == null
                || string.IsNullOrEmpty(mZxkOrderUpdateViewModel.PunishCaseEntity.CaseTime.ToString()))
            {
                MessageBox.Show("时间为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(mZxkOrderUpdateViewModel.PunishCaseEntity.CaseIndex))
            {
                MessageBox.Show("案件编号为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(mZxkOrderUpdateViewModel.PunishCaseEntity.Title))
            {
                MessageBox.Show("标题为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public bool DownloadOper()
        {
            bool newer = true;
            try
            {
                SaveFileDialog sf = new SaveFileDialog();

                sf.AddExtension = true;
                sf.RestoreDirectory = true;
                sf.FileName = mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentName;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    fileFrom = mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath;
                    fileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    mZxkOrderUpdateViewModel.Show_LoadingMask(LoadingType.View);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("下载失败，文件未找到或已过期", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool DownloadOper2()
        {
            bool newer = true;
            try
            {
                SaveFileDialog sf = new SaveFileDialog();

                sf.AddExtension = true;
                sf.RestoreDirectory = true;
                sf.FileName = mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentName2;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    fileFrom = mZxkOrderUpdateViewModel.PunishCaseEntity.AttachmentPath2;
                    fileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    mZxkOrderUpdateViewModel.Show_LoadingMask(LoadingType.View);
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
                e.Result = ftp.DownloadFile(fileFrom, fileTo);
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

                mZxkOrderUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
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
    }
}
    