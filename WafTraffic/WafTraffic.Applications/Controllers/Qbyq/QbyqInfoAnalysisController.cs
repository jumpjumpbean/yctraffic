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
    internal class QbyqInfoAnalysisController : Controller
    {
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private QbyqInfoAnalysisUpdateViewModel mQbyqInfoAnalysisUpdateViewModel;
        private QbyqInfoAnalysisQueryViewModel mQbyqInfoAnalysisQueryViewModel;

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
        public QbyqInfoAnalysisController(CompositionContainer container, IEntityService entityService)
        {
            try
            {
                this.mEntityService = entityService;

                mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                mQbyqInfoAnalysisUpdateViewModel = container.GetExportedValue<QbyqInfoAnalysisUpdateViewModel>();
                mQbyqInfoAnalysisQueryViewModel = container.GetExportedValue<QbyqInfoAnalysisQueryViewModel>();

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
                mQbyqInfoAnalysisUpdateViewModel.SaveCommand = this.mSaveCommand;
                mQbyqInfoAnalysisUpdateViewModel.CancelCommand = this.mCancelCommand;
                mQbyqInfoAnalysisUpdateViewModel.DownloadCommand = this.mDownloadCommand;

                mQbyqInfoAnalysisQueryViewModel.NewCommand = this.mNewCommand;
                mQbyqInfoAnalysisQueryViewModel.ModifyCommand = this.mModifyCommand;
                mQbyqInfoAnalysisQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mQbyqInfoAnalysisQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mQbyqInfoAnalysisQueryViewModel.QueryCommand = this.mQueryCommand;
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
                mQbyqInfoAnalysisUpdateViewModel.IsBrowse = false;
                mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity = new QbyqInfoAnalysisCase();
                mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.CaseTime = DateTime.Now;
                mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.OwnDepartmentId);
                mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.CreateName = AuthService.Instance.GetUserNameById(mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.CreateId);

                mQbyqInfoAnalysisUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                mQbyqInfoAnalysisUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                mMainFrameViewModel.ContentView = mQbyqInfoAnalysisUpdateViewModel.View;
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
                mQbyqInfoAnalysisUpdateViewModel.IsBrowse = false;
                mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity = mQbyqInfoAnalysisQueryViewModel.SelectedCase;

                mQbyqInfoAnalysisUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                mQbyqInfoAnalysisUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;

                mMainFrameViewModel.ContentView = mQbyqInfoAnalysisUpdateViewModel.View;
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
                mQbyqInfoAnalysisUpdateViewModel.IsBrowse = true;
                mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity = mQbyqInfoAnalysisQueryViewModel.SelectedCase;

                mQbyqInfoAnalysisUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Collapsed;

                if (!string.IsNullOrEmpty(mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.AttachmentName))
                {
                    mQbyqInfoAnalysisUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Visible;
                }
                else
                {
                    mQbyqInfoAnalysisUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                }

                mMainFrameViewModel.ContentView = mQbyqInfoAnalysisUpdateViewModel.View;

                if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstants.INT_ROLE_DDZ_ID)
                {
                    mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.IsRead1 = true;
                    mEntityService.Entities.SaveChanges(); 
                }
                if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstants.INT_ROLE_ZW_ID)
                {
                    mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.IsRead2 = true;
                    mEntityService.Entities.SaveChanges();
                }
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
                    mQbyqInfoAnalysisQueryViewModel.SelectedCase.IsDeleted = true;
                    mQbyqInfoAnalysisQueryViewModel.SelectedCase.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mQbyqInfoAnalysisQueryViewModel.SelectedCase.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    mQbyqInfoAnalysisQueryViewModel.GridRefresh();
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
                mMainFrameViewModel.ContentView = mQbyqInfoAnalysisQueryViewModel.View;
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
                if (mQbyqInfoAnalysisQueryViewModel.IsSelectDepartmentEnabled)
                {
                    deptId = Convert.ToInt32(mQbyqInfoAnalysisQueryViewModel.SelectedDepartment);
                }
                else
                {
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }*/

                DateTime startDate = (DateTime)mQbyqInfoAnalysisQueryViewModel.SelectedStartDate;
                DateTime endDate = (DateTime)mQbyqInfoAnalysisQueryViewModel.SelectedEndDate;

                mQbyqInfoAnalysisQueryViewModel.PunishCases = mEntityService.QueryQbyqInfoAnalysisCases(startDate, endDate);
                if (mQbyqInfoAnalysisQueryViewModel.PunishCases == null || mQbyqInfoAnalysisQueryViewModel.PunishCases.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mQbyqInfoAnalysisQueryViewModel.GridRefresh();
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

            mQbyqInfoAnalysisUpdateViewModel.Show_LoadingMask(LoadingType.View);

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


                if (mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.Id > 0)
                {
                    mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;

                    if (!string.IsNullOrEmpty(mQbyqInfoAnalysisUpdateViewModel.FileLocalPath))
                    {
                        oldFile = mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.AttachmentPath;
                        mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.AttachmentPath =
                            ftp.UpdateFile(FtpFileType.Document, mQbyqInfoAnalysisUpdateViewModel.FileLocalPath, oldFile);

                        if (string.IsNullOrEmpty(mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.AttachmentPath))
                        {
                            throw new ValidationException();
                        }
                    }
                }
                else
                {
                    mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.CreateTime = System.DateTime.Now;

                    mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;
                    mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.IsDeleted = false;

                    if (!string.IsNullOrEmpty(mQbyqInfoAnalysisUpdateViewModel.FileLocalPath))
                    {

                        mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.AttachmentPath =
                            ftp.UploadFile(FtpFileType.Document, mQbyqInfoAnalysisUpdateViewModel.FileLocalPath);

                        if (string.IsNullOrEmpty(mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.AttachmentPath))
                        {
                            throw new ValidationException();
                        }
                    }

                    mEntityService.Entities.QbyqInfoAnalysisCases.AddObject(mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity);
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

            mQbyqInfoAnalysisUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mMainFrameViewModel.ContentView = mQbyqInfoAnalysisQueryViewModel.View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValueCheck()
        {
            if (mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.CaseTime == null
                || string.IsNullOrEmpty(mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.CaseTime.ToString()))
            {
                MessageBox.Show("时间为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.CaseIndex))
            {
                MessageBox.Show("案件编号为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.CaseName))
            {
                MessageBox.Show("案件名称为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                sf.FileName = mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.AttachmentName;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    fileFrom = mQbyqInfoAnalysisUpdateViewModel.PunishCaseEntity.AttachmentPath;
                    fileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    mQbyqInfoAnalysisUpdateViewModel.Show_LoadingMask(LoadingType.View);
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

                mQbyqInfoAnalysisUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
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
    