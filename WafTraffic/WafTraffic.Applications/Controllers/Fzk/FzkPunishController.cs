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
    internal class FzkPunishController : Controller
    {
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private FzkPunishUpdateViewModel mFzkPunishUpdateViewModel;
        private FzkPunishQueryViewModel mFzkPunishQueryViewModel;

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
        public FzkPunishController(CompositionContainer container, IEntityService entityService)
        {
            try
            {
                this.mEntityService = entityService;

                mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                mFzkPunishUpdateViewModel = container.GetExportedValue<FzkPunishUpdateViewModel>();
                mFzkPunishQueryViewModel = container.GetExportedValue<FzkPunishQueryViewModel>();

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
                mFzkPunishUpdateViewModel.SaveCommand = this.mSaveCommand;
                mFzkPunishUpdateViewModel.CancelCommand = this.mCancelCommand;
                mFzkPunishUpdateViewModel.DownloadCommand = this.mDownloadCommand;

                mFzkPunishQueryViewModel.NewCommand = this.mNewCommand;
                mFzkPunishQueryViewModel.ModifyCommand = this.mModifyCommand;
                mFzkPunishQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mFzkPunishQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mFzkPunishQueryViewModel.QueryCommand = this.mQueryCommand;
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
                mFzkPunishUpdateViewModel.IsBrowse = false;
                mFzkPunishUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                mFzkPunishUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;


                mFzkPunishUpdateViewModel.PunishCaseEntity = new FzkPunishCase();
                mFzkPunishUpdateViewModel.PunishCaseEntity.CaseTime = DateTime.Now;
                mFzkPunishUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mFzkPunishUpdateViewModel.PunishCaseEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mFzkPunishUpdateViewModel.PunishCaseEntity.OwnDepartmentId);
                mFzkPunishUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mFzkPunishUpdateViewModel.PunishCaseEntity.CreateName = AuthService.Instance.GetUserNameById(mFzkPunishUpdateViewModel.PunishCaseEntity.CreateId);
                mMainFrameViewModel.ContentView = mFzkPunishUpdateViewModel.View;
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
                mFzkPunishUpdateViewModel.IsBrowse = false;
                mFzkPunishUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                mFzkPunishUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
    

                mFzkPunishUpdateViewModel.PunishCaseEntity = mFzkPunishQueryViewModel.SelectedCase;
                mMainFrameViewModel.ContentView = mFzkPunishUpdateViewModel.View;
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
                mFzkPunishUpdateViewModel.IsBrowse = true;

                mFzkPunishUpdateViewModel.PunishCaseEntity = mFzkPunishQueryViewModel.SelectedCase;

                mFzkPunishUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Collapsed;

                if (!string.IsNullOrEmpty(mFzkPunishUpdateViewModel.PunishCaseEntity.AttachmentName))
                {
                    mFzkPunishUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Visible;
                }
                else
                {
                    mFzkPunishUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                }

                
                mMainFrameViewModel.ContentView = mFzkPunishUpdateViewModel.View;
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
                    mFzkPunishQueryViewModel.SelectedCase.IsDeleted = true;
                    mFzkPunishQueryViewModel.SelectedCase.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFzkPunishQueryViewModel.SelectedCase.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    mFzkPunishQueryViewModel.GridRefresh();
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
                mMainFrameViewModel.ContentView = mFzkPunishQueryViewModel.View;
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
                if (mFzkPunishQueryViewModel.IsSelectDepartmentEnabled)
                {
                    deptId = Convert.ToInt32(mFzkPunishQueryViewModel.SelectedDepartment);
                }
                else
                {
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }*/

                DateTime startDate = (DateTime)mFzkPunishQueryViewModel.SelectedStartDate;
                DateTime endDate = (DateTime)mFzkPunishQueryViewModel.SelectedEndDate;

                mFzkPunishQueryViewModel.PunishCases = mEntityService.QueryFzkPunishCases(startDate, endDate);
                if (mFzkPunishQueryViewModel.PunishCases == null || mFzkPunishQueryViewModel.PunishCases.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mFzkPunishQueryViewModel.GridRefresh();
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

            mFzkPunishUpdateViewModel.Show_LoadingMask(LoadingType.View);

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

                if (mFzkPunishUpdateViewModel.PunishCaseEntity.Id > 0)
                {
                    mFzkPunishUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFzkPunishUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;

                    if (!string.IsNullOrEmpty(mFzkPunishUpdateViewModel.FileLocalPath))
                    {
                        oldFile = mFzkPunishUpdateViewModel.PunishCaseEntity.AttachmentPath;
                        mFzkPunishUpdateViewModel.PunishCaseEntity.AttachmentPath =
                            ftp.UpdateFile(FtpFileType.Document, mFzkPunishUpdateViewModel.FileLocalPath, oldFile);

                        if (string.IsNullOrEmpty(mFzkPunishUpdateViewModel.PunishCaseEntity.AttachmentPath))
                        {
                            throw new ValidationException();
                        }
                    }


                }
                else
                {
                    mFzkPunishUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    mFzkPunishUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFzkPunishUpdateViewModel.PunishCaseEntity.CreateTime = System.DateTime.Now;

                    mFzkPunishUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFzkPunishUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;
                    mFzkPunishUpdateViewModel.PunishCaseEntity.IsDeleted = false;

                    if (!string.IsNullOrEmpty(mFzkPunishUpdateViewModel.FileLocalPath))
                    {

                        mFzkPunishUpdateViewModel.PunishCaseEntity.AttachmentPath =
                            ftp.UploadFile(FtpFileType.Document, mFzkPunishUpdateViewModel.FileLocalPath);

                        if (string.IsNullOrEmpty(mFzkPunishUpdateViewModel.PunishCaseEntity.AttachmentPath))
                        {
                            throw new ValidationException();
                        }
                    }

                    mEntityService.Entities.FzkPunishCases.AddObject(mFzkPunishUpdateViewModel.PunishCaseEntity);
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

            mFzkPunishUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mMainFrameViewModel.ContentView = mFzkPunishQueryViewModel.View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //public bool Save()
        //{
        //    bool saved = false;

        //    try
        //    {
        //        if (!ValueCheck())
        //        {
        //            return saved;
        //        }
        //        if (mFzkPunishUpdateViewModel.PunishCaseEntity.Id > 0)
        //        {
        //            mFzkPunishUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
        //            mFzkPunishUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;
        //        }
        //        else
        //        {
        //            mFzkPunishUpdateViewModel.PunishCaseEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
        //            mFzkPunishUpdateViewModel.PunishCaseEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
        //            mFzkPunishUpdateViewModel.PunishCaseEntity.CreateTime = System.DateTime.Now;

        //            mFzkPunishUpdateViewModel.PunishCaseEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
        //            mFzkPunishUpdateViewModel.PunishCaseEntity.UpdateTime = System.DateTime.Now;
        //            mFzkPunishUpdateViewModel.PunishCaseEntity.IsDeleted = false;

        //            mEntityService.Entities.FzkPunishCases.AddObject(mFzkPunishUpdateViewModel.PunishCaseEntity);
        //        }
        //        mEntityService.Entities.SaveChanges();
        //        saved = true;

        //        MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        //mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
        //        //返回列表页
        //        mMainFrameViewModel.ContentView = mFzkPunishQueryViewModel.View;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return saved;
        //}

        private bool ValueCheck()
        {
            if (mFzkPunishUpdateViewModel.PunishCaseEntity.CaseTime == null
                || string.IsNullOrEmpty(mFzkPunishUpdateViewModel.PunishCaseEntity.CaseTime.ToString()))
            {
                MessageBox.Show("时间为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(mFzkPunishUpdateViewModel.PunishCaseEntity.Title))
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
                sf.FileName = mFzkPunishUpdateViewModel.PunishCaseEntity.AttachmentName;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    fileFrom = mFzkPunishUpdateViewModel.PunishCaseEntity.AttachmentPath;
                    fileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    mFzkPunishUpdateViewModel.Show_LoadingMask(LoadingType.View);
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

                mFzkPunishUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
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
    