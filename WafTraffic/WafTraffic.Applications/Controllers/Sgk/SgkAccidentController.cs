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
    internal class SgkAccidentController : Controller
    {
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private SgkAccidentUpdateViewModel mSgkAccidentUpdateViewModel;
        private SgkAccidentQueryViewModel mSgkAccidentQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mDownloadCommand;

        private string fileFrom;
        private string fileTo;

        [ImportingConstructor]
        public SgkAccidentController(CompositionContainer container, IEntityService entityService)
        {
            try
            {
                this.mEntityService = entityService;

                mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                mSgkAccidentUpdateViewModel = container.GetExportedValue<SgkAccidentUpdateViewModel>();
                mSgkAccidentQueryViewModel = container.GetExportedValue<SgkAccidentQueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mDownloadCommand = new DelegateCommand(() => DownloadOper(), null);
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
                mSgkAccidentUpdateViewModel.SaveCommand = this.mSaveCommand;
                mSgkAccidentUpdateViewModel.CancelCommand = this.mCancelCommand;
                mSgkAccidentUpdateViewModel.DownloadCommand = this.mDownloadCommand;

                mSgkAccidentQueryViewModel.NewCommand = this.mNewCommand;
                mSgkAccidentQueryViewModel.ModifyCommand = this.mModifyCommand;
                mSgkAccidentQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mSgkAccidentQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mSgkAccidentQueryViewModel.QueryCommand = this.mQueryCommand;
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
                mSgkAccidentUpdateViewModel.IsBrowse = false;
                mSgkAccidentUpdateViewModel.PunishCaseEntity = new SgkAccidentCase();
                mSgkAccidentUpdateViewModel.PunishCaseEntity.CaseTime = DateTime.Now;
                mSgkAccidentUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mSgkAccidentUpdateViewModel.PunishCaseEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mSgkAccidentUpdateViewModel.PunishCaseEntity.OwnDepartmentId);
                mSgkAccidentUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mSgkAccidentUpdateViewModel.PunishCaseEntity.CreateName = AuthService.Instance.GetUserNameById(mSgkAccidentUpdateViewModel.PunishCaseEntity.CreateId);

                mSgkAccidentUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                mSgkAccidentUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                
                mMainFrameViewModel.ContentView = mSgkAccidentUpdateViewModel.View;
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
                mSgkAccidentUpdateViewModel.IsBrowse = false;
                mSgkAccidentUpdateViewModel.PunishCaseEntity = mSgkAccidentQueryViewModel.SelectedCase;

                mSgkAccidentUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                mSgkAccidentUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;

                mMainFrameViewModel.ContentView = mSgkAccidentUpdateViewModel.View;
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
                mSgkAccidentUpdateViewModel.IsBrowse = true;
                mSgkAccidentUpdateViewModel.PunishCaseEntity = mSgkAccidentQueryViewModel.SelectedCase;

                mSgkAccidentUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Collapsed;

                if (!string.IsNullOrEmpty(mSgkAccidentUpdateViewModel.PunishCaseEntity.AttachmentName))
                {
                    mSgkAccidentUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Visible;
                }
                else
                {
                    mSgkAccidentUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                }

                mMainFrameViewModel.ContentView = mSgkAccidentUpdateViewModel.View;
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
                    mSgkAccidentQueryViewModel.SelectedCase.IsDeleted = true;
                    mSgkAccidentQueryViewModel.SelectedCase.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mSgkAccidentQueryViewModel.SelectedCase.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    mSgkAccidentQueryViewModel.GridRefresh();
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
                mMainFrameViewModel.ContentView = mSgkAccidentQueryViewModel.View;
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
                if (mSgkAccidentQueryViewModel.IsSelectDepartmentEnabled)
                {
                    deptId = Convert.ToInt32(mSgkAccidentQueryViewModel.SelectedDepartment);
                }
                else
                {
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }*/

                DateTime startDate = (DateTime)mSgkAccidentQueryViewModel.SelectedStartDate;
                DateTime endDate = (DateTime)mSgkAccidentQueryViewModel.SelectedEndDate;

                mSgkAccidentQueryViewModel.PunishCases = mEntityService.QuerySgkAccidentCases(startDate, endDate);
                if (mSgkAccidentQueryViewModel.PunishCases == null || mSgkAccidentQueryViewModel.PunishCases.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mSgkAccidentQueryViewModel.GridRefresh();
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

            mSgkAccidentUpdateViewModel.Show_LoadingMask(LoadingType.View);

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

                if (mSgkAccidentUpdateViewModel.PunishCaseEntity.Id > 0)
                {
                    mSgkAccidentUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mSgkAccidentUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;

                    if (!string.IsNullOrEmpty(mSgkAccidentUpdateViewModel.FileLocalPath))
                    {
                        oldFile = mSgkAccidentUpdateViewModel.PunishCaseEntity.AttachmentPath;
                        mSgkAccidentUpdateViewModel.PunishCaseEntity.AttachmentPath =
                            ftp.UpdateFile(FtpFileType.Document, mSgkAccidentUpdateViewModel.FileLocalPath, oldFile);

                        if (string.IsNullOrEmpty(mSgkAccidentUpdateViewModel.PunishCaseEntity.AttachmentPath))
                        {
                            throw new ValidationException();
                        }
                    }
                }
                else
                {
                    mSgkAccidentUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    mSgkAccidentUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mSgkAccidentUpdateViewModel.PunishCaseEntity.CreateTime = System.DateTime.Now;

                    mSgkAccidentUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mSgkAccidentUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;
                    mSgkAccidentUpdateViewModel.PunishCaseEntity.IsDeleted = false;

                    if (!string.IsNullOrEmpty(mSgkAccidentUpdateViewModel.FileLocalPath))
                    {

                        mSgkAccidentUpdateViewModel.PunishCaseEntity.AttachmentPath =
                            ftp.UploadFile(FtpFileType.Document, mSgkAccidentUpdateViewModel.FileLocalPath);

                        if (string.IsNullOrEmpty(mSgkAccidentUpdateViewModel.PunishCaseEntity.AttachmentPath))
                        {
                            throw new ValidationException();
                        }
                    }

                    mEntityService.Entities.SgkAccidentCases.AddObject(mSgkAccidentUpdateViewModel.PunishCaseEntity);
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

            mSgkAccidentUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mMainFrameViewModel.ContentView = mSgkAccidentQueryViewModel.View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValueCheck()
        {
            if (mSgkAccidentUpdateViewModel.PunishCaseEntity.CaseTime == null
                || string.IsNullOrEmpty(mSgkAccidentUpdateViewModel.PunishCaseEntity.CaseTime.ToString()))
            {
                MessageBox.Show("时间为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(mSgkAccidentUpdateViewModel.PunishCaseEntity.Title))
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
                sf.FileName = mSgkAccidentUpdateViewModel.PunishCaseEntity.AttachmentName;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    fileFrom = mSgkAccidentUpdateViewModel.PunishCaseEntity.AttachmentPath;
                    fileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    mSgkAccidentUpdateViewModel.Show_LoadingMask(LoadingType.View);
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

                mSgkAccidentUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
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
    