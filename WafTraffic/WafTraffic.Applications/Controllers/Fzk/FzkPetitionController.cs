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
    internal class FzkPetitionController : Controller
    {
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private FzkPetitionUpdateViewModel mFzkPetitionUpdateViewModel;
        private FzkPetitionQueryViewModel mFzkPetitionQueryViewModel;

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
        public FzkPetitionController(CompositionContainer container, IEntityService entityService)
        {
            try
            {
                this.mEntityService = entityService;

                mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                mFzkPetitionUpdateViewModel = container.GetExportedValue<FzkPetitionUpdateViewModel>();
                mFzkPetitionQueryViewModel = container.GetExportedValue<FzkPetitionQueryViewModel>();

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
                mFzkPetitionUpdateViewModel.SaveCommand = this.mSaveCommand;
                mFzkPetitionUpdateViewModel.CancelCommand = this.mCancelCommand;
                mFzkPetitionUpdateViewModel.DownloadCommand = this.mDownloadCommand;

                mFzkPetitionQueryViewModel.NewCommand = this.mNewCommand;
                mFzkPetitionQueryViewModel.ModifyCommand = this.mModifyCommand;
                mFzkPetitionQueryViewModel.DeleteCommand = this.mDeleteCommand;
                mFzkPetitionQueryViewModel.BrowseCommand = this.mBrowseCommand;
                mFzkPetitionQueryViewModel.QueryCommand = this.mQueryCommand;
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
                mFzkPetitionUpdateViewModel.IsBrowse = false;

                mFzkPetitionUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                mFzkPetitionUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;

                mFzkPetitionUpdateViewModel.PetitionEntity = new FzkPetition();
                mFzkPetitionUpdateViewModel.PetitionEntity.PetitionTime = DateTime.Now;
                mFzkPetitionUpdateViewModel.PetitionEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mFzkPetitionUpdateViewModel.PetitionEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mFzkPetitionUpdateViewModel.PetitionEntity.OwnDepartmentId);
                mFzkPetitionUpdateViewModel.PetitionEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mFzkPetitionUpdateViewModel.PetitionEntity.CreateName = AuthService.Instance.GetUserNameById(mFzkPetitionUpdateViewModel.PetitionEntity.CreateId);
                mMainFrameViewModel.ContentView = mFzkPetitionUpdateViewModel.View;
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
                mFzkPetitionUpdateViewModel.IsBrowse = false;

                mFzkPetitionUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                mFzkPetitionUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;

                mFzkPetitionUpdateViewModel.PetitionEntity = mFzkPetitionQueryViewModel.SelectedCase;
                mMainFrameViewModel.ContentView = mFzkPetitionUpdateViewModel.View;
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
                mFzkPetitionUpdateViewModel.IsBrowse = true;

                mFzkPetitionUpdateViewModel.PetitionEntity = mFzkPetitionQueryViewModel.SelectedCase;

                mFzkPetitionUpdateViewModel.CanUploadVisibal = System.Windows.Visibility.Collapsed;

                if (!string.IsNullOrEmpty(mFzkPetitionUpdateViewModel.PetitionEntity.AttachmentName))
                {
                    mFzkPetitionUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Visible;
                }
                else
                {
                    mFzkPetitionUpdateViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                }

                mMainFrameViewModel.ContentView = mFzkPetitionUpdateViewModel.View;
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
                    mFzkPetitionQueryViewModel.SelectedCase.IsDeleted = true;
                    mFzkPetitionQueryViewModel.SelectedCase.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFzkPetitionQueryViewModel.SelectedCase.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    mFzkPetitionQueryViewModel.GridRefresh();
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
                mMainFrameViewModel.ContentView = mFzkPetitionQueryViewModel.View;
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
                if (mFzkPetitionQueryViewModel.IsSelectDepartmentEnabled)
                {
                    deptId = Convert.ToInt32(mFzkPetitionQueryViewModel.SelectedDepartment);
                }
                else
                {
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }*/

                DateTime startDate = (DateTime)mFzkPetitionQueryViewModel.SelectedStartDate;
                DateTime endDate = (DateTime)mFzkPetitionQueryViewModel.SelectedEndDate;

                mFzkPetitionQueryViewModel.PetitionCases = mEntityService.QueryFzkPetitionCases(startDate, endDate);
                if (mFzkPetitionQueryViewModel.PetitionCases == null || mFzkPetitionQueryViewModel.PetitionCases.Count() == 0)
                {
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mFzkPetitionQueryViewModel.GridRefresh();
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

            mFzkPetitionUpdateViewModel.Show_LoadingMask(LoadingType.View);

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
                if (mFzkPetitionUpdateViewModel.PetitionEntity.Id > 0)
                {
                    mFzkPetitionUpdateViewModel.PetitionEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFzkPetitionUpdateViewModel.PetitionEntity.UpdateTime = System.DateTime.Now;

                    if (!string.IsNullOrEmpty(mFzkPetitionUpdateViewModel.FileLocalPath))
                    {
                        oldFile = mFzkPetitionUpdateViewModel.PetitionEntity.AttachmentPath;
                        mFzkPetitionUpdateViewModel.PetitionEntity.AttachmentPath =
                            ftp.UpdateFile(FtpFileType.Document, mFzkPetitionUpdateViewModel.FileLocalPath, oldFile);


                        if (string.IsNullOrEmpty(mFzkPetitionUpdateViewModel.PetitionEntity.AttachmentPath))
                        {
                            throw new ValidationException();
                        }
                    }
                }
                else
                {
                    mFzkPetitionUpdateViewModel.PetitionEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    mFzkPetitionUpdateViewModel.PetitionEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFzkPetitionUpdateViewModel.PetitionEntity.CreateTime = System.DateTime.Now;

                    mFzkPetitionUpdateViewModel.PetitionEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mFzkPetitionUpdateViewModel.PetitionEntity.UpdateTime = System.DateTime.Now;
                    mFzkPetitionUpdateViewModel.PetitionEntity.IsDeleted = false;

                    if (!string.IsNullOrEmpty(mFzkPetitionUpdateViewModel.FileLocalPath))
                    {

                        mFzkPetitionUpdateViewModel.PetitionEntity.AttachmentPath =
                            ftp.UploadFile(FtpFileType.Document, mFzkPetitionUpdateViewModel.FileLocalPath);

                        if (string.IsNullOrEmpty(mFzkPetitionUpdateViewModel.PetitionEntity.AttachmentPath))
                        {
                            throw new ValidationException();
                        }
                    }

                    mEntityService.Entities.FzkPetitions.AddObject(mFzkPetitionUpdateViewModel.PetitionEntity);
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

            mFzkPetitionUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mMainFrameViewModel.ContentView = mFzkPetitionQueryViewModel.View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValueCheck()
        {
            if (mFzkPetitionUpdateViewModel.PetitionEntity.PetitionTime == null
                || string.IsNullOrEmpty(mFzkPetitionUpdateViewModel.PetitionEntity.PetitionTime.ToString()))
            {
                MessageBox.Show("时间为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(mFzkPetitionUpdateViewModel.PetitionEntity.PetitionName))
            {
                MessageBox.Show("信访人姓名为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(mFzkPetitionUpdateViewModel.PetitionEntity.Gender))
            {
                MessageBox.Show("信访人性别为必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                sf.FileName = mFzkPetitionUpdateViewModel.PetitionEntity.AttachmentName;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    fileFrom = mFzkPetitionUpdateViewModel.PetitionEntity.AttachmentPath;
                    fileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    mFzkPetitionUpdateViewModel.Show_LoadingMask(LoadingType.View);
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

                mFzkPetitionUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
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
    