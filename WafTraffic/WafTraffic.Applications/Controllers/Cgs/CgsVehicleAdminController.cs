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
    internal class CgsVehicleAdminController : Controller
    {
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private CgsVehicleAdminUpdateViewModel mCgsVehicleAdminUpdateViewModel;
        private CgsVehicleAdminQueryViewModel mCgsVehicleAdminQueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mDownloadCommand1;
        private readonly DelegateCommand mDownloadCommand2;

        private string fileFrom;
        private string fileTo;

        [ImportingConstructor]
        public CgsVehicleAdminController(CompositionContainer container, IEntityService entityService)
        {
            try
            {
                this.mEntityService = entityService;

                mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                mCgsVehicleAdminUpdateViewModel = container.GetExportedValue<CgsVehicleAdminUpdateViewModel>();
                mCgsVehicleAdminQueryViewModel = container.GetExportedValue<CgsVehicleAdminQueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.mDownloadCommand1 = new DelegateCommand(() => DownloadOper1(), null);
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
                mCgsVehicleAdminUpdateViewModel.SaveCommand = this.mSaveCommand;
                mCgsVehicleAdminUpdateViewModel.CancelCommand = this.mCancelCommand;
                mCgsVehicleAdminUpdateViewModel.DownloadCommand1 = this.mDownloadCommand1;
                mCgsVehicleAdminUpdateViewModel.DownloadCommand2 = this.mDownloadCommand2;

                mCgsVehicleAdminQueryViewModel.NewCommand = this.mNewCommand;
                mCgsVehicleAdminQueryViewModel.ModifyCommand = this.mModifyCommand;
                mCgsVehicleAdminQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mCgsVehicleAdminQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mCgsVehicleAdminQueryViewModel.QueryCommand = this.mQueryCommand;
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
                mCgsVehicleAdminUpdateViewModel.IsBrowse = false;
                mCgsVehicleAdminUpdateViewModel.PunishCaseEntity = new CgsVehicleAdminCase();
                mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.CaseTime = DateTime.Now;
                mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.OwnDepartmentId);
                mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.CreateName = AuthService.Instance.GetUserNameById(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.CreateId);
                mCgsVehicleAdminUpdateViewModel.CanUploadVisibal1 = System.Windows.Visibility.Visible;
                mCgsVehicleAdminUpdateViewModel.CanUploadVisibal2 = System.Windows.Visibility.Visible;
                mCgsVehicleAdminUpdateViewModel.CanDownloadVisibal1 = System.Windows.Visibility.Collapsed;
                mCgsVehicleAdminUpdateViewModel.CanDownloadVisibal2 = System.Windows.Visibility.Collapsed;
                mMainFrameViewModel.ContentView = mCgsVehicleAdminUpdateViewModel.View;
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
                mCgsVehicleAdminUpdateViewModel.IsBrowse = false;
                mCgsVehicleAdminUpdateViewModel.PunishCaseEntity = mCgsVehicleAdminQueryViewModel.SelectedCase;
                mCgsVehicleAdminUpdateViewModel.CanUploadVisibal1 = System.Windows.Visibility.Visible;
                mCgsVehicleAdminUpdateViewModel.CanUploadVisibal2 = System.Windows.Visibility.Visible;
                mCgsVehicleAdminUpdateViewModel.CanDownloadVisibal1 = System.Windows.Visibility.Collapsed;
                mCgsVehicleAdminUpdateViewModel.CanDownloadVisibal2 = System.Windows.Visibility.Collapsed;
                mMainFrameViewModel.ContentView = mCgsVehicleAdminUpdateViewModel.View;
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
                mCgsVehicleAdminUpdateViewModel.IsBrowse = true;
                mCgsVehicleAdminUpdateViewModel.PunishCaseEntity = mCgsVehicleAdminQueryViewModel.SelectedCase;

                mCgsVehicleAdminUpdateViewModel.CanUploadVisibal1 = System.Windows.Visibility.Collapsed;
                mCgsVehicleAdminUpdateViewModel.CanUploadVisibal2 = System.Windows.Visibility.Collapsed;

                if (!string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentName1))
                {
                    mCgsVehicleAdminUpdateViewModel.CanDownloadVisibal1 = System.Windows.Visibility.Visible;
                }
                else
                {
                    mCgsVehicleAdminUpdateViewModel.CanDownloadVisibal1 = System.Windows.Visibility.Collapsed;
                }

                if (!string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentName2))
                {
                    mCgsVehicleAdminUpdateViewModel.CanDownloadVisibal2 = System.Windows.Visibility.Visible;
                }
                else
                {
                    mCgsVehicleAdminUpdateViewModel.CanDownloadVisibal2 = System.Windows.Visibility.Collapsed;
                }

                mMainFrameViewModel.ContentView = mCgsVehicleAdminUpdateViewModel.View;
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
                    mCgsVehicleAdminQueryViewModel.SelectedCase.IsDeleted = true;
                    mCgsVehicleAdminQueryViewModel.SelectedCase.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mCgsVehicleAdminQueryViewModel.SelectedCase.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    mCgsVehicleAdminQueryViewModel.GridRefresh();
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
                mMainFrameViewModel.ContentView = mCgsVehicleAdminQueryViewModel.View;
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
                if (mCgsVehicleAdminQueryViewModel.IsSelectDepartmentEnabled)
                {
                    deptId = Convert.ToInt32(mCgsVehicleAdminQueryViewModel.SelectedDepartment);
                }
                else
                {
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }*/

                DateTime startDate = (DateTime)mCgsVehicleAdminQueryViewModel.SelectedStartDate;
                DateTime endDate = (DateTime)mCgsVehicleAdminQueryViewModel.SelectedEndDate;

                mCgsVehicleAdminQueryViewModel.PunishCases = mEntityService.QueryCgsVehicleAdminCases(startDate, endDate);
                if (mCgsVehicleAdminQueryViewModel.PunishCases == null || mCgsVehicleAdminQueryViewModel.PunishCases.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mCgsVehicleAdminQueryViewModel.GridRefresh();
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

            mCgsVehicleAdminUpdateViewModel.Show_LoadingMask(LoadingType.View);

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

                if (mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.Id > 0)
                {
                    mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;

                    if (!string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.FileLocalPath1))
                    {
                        oldFile = mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath1;
                        mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath1 =
                            ftp.UpdateFile(FtpFileType.Document, mCgsVehicleAdminUpdateViewModel.FileLocalPath1, oldFile);

                        if (string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath1))
                        {
                            throw new ValidationException();
                        }
                    }

                    if (!string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.FileLocalPath2))
                    {
                        oldFile = mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath2;
                        mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath2 =
                            ftp.UpdateFile(FtpFileType.Document, mCgsVehicleAdminUpdateViewModel.FileLocalPath2, oldFile);

                        if (string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath2))
                        {
                            throw new ValidationException();
                        }
                    }


                }
                else
                {
                    mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.CreateTime = System.DateTime.Now;

                    mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;
                    mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.IsDeleted = false;

                    if (!string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.FileLocalPath1))
                    {

                        mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath1 =
                            ftp.UploadFile(FtpFileType.Document, mCgsVehicleAdminUpdateViewModel.FileLocalPath1);

                        if (string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath1))
                        {
                            throw new ValidationException();
                        }
                    }

                    if (!string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.FileLocalPath2))
                    {

                        mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath2 =
                            ftp.UploadFile(FtpFileType.Document, mCgsVehicleAdminUpdateViewModel.FileLocalPath2);

                        if (string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath2))
                        {
                            throw new ValidationException();
                        }
                    }

                    mEntityService.Entities.CgsVehicleAdminCases.AddObject(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity);
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

            mCgsVehicleAdminUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mMainFrameViewModel.ContentView = mCgsVehicleAdminQueryViewModel.View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValueCheck()
        {
            if (mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.CaseTime == null
                || string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.CaseTime.ToString()))
            {
                MessageBox.Show("时间为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.Title))
            {
                MessageBox.Show("标题为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public bool DownloadOper1()
        {
            bool newer = true;
            try
            {
                SaveFileDialog sf = new SaveFileDialog();

                sf.AddExtension = true;
                sf.RestoreDirectory = true;
                sf.FileName = mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentName1;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    fileFrom = mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath1;
                    fileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download1);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    mCgsVehicleAdminUpdateViewModel.Show_LoadingMask(LoadingType.View);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("下载失败，文件未找到或已过期", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        private void worker_Download1(object sender, DoWorkEventArgs e)
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


        public bool DownloadOper2()
        {
            bool newer = true;
            try
            {
                SaveFileDialog sf = new SaveFileDialog();

                sf.AddExtension = true;
                sf.RestoreDirectory = true;
                sf.FileName = mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentName2;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    fileFrom = mCgsVehicleAdminUpdateViewModel.PunishCaseEntity.AttachmentPath2;
                    fileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download2);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    mCgsVehicleAdminUpdateViewModel.Show_LoadingMask(LoadingType.View);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("下载失败，文件未找到或已过期", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        private void worker_Download2(object sender, DoWorkEventArgs e)
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

                mCgsVehicleAdminUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
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
    