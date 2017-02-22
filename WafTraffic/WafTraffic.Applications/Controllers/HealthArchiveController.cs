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
using WafTraffic.Applications.Common;
using WafTraffic.Applications.Utils;
using System.Windows.Forms;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class HealthArchiveController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private HealthArchiveListViewModel mHealthArchiveListViewModel;
        private HealthArchiveViewModel mHealthArchiveViewModel;
        private ShellViewModel mShellViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mRetreatCommand;

        [ImportingConstructor]
        public HealthArchiveController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            this.mContainer = container;
            this.mMessageService = messageService;
            this.mShellService = shellService;
            this.mEntityService = entityService;

            mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
            mHealthArchiveListViewModel = container.GetExportedValue<HealthArchiveListViewModel>();
            mHealthArchiveViewModel = container.GetExportedValue<HealthArchiveViewModel>();
            mShellViewModel = container.GetExportedValue<ShellViewModel>();

            this.mNewCommand = new DelegateCommand(() => NewOper(), null);
            this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
            this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
            this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
            this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
            this.mSaveCommand = new DelegateCommand(() => Save(), null);
            this.mRetreatCommand = new DelegateCommand(() => RetreatOper(), null);

        }

        public void Initialize()
        {
            //AddWeakEventListener(healthArchiveListViewModel, HealthArchiveListViewModelPropertyChanged);

            mHealthArchiveListViewModel.NewCommand = this.mNewCommand;
            mHealthArchiveListViewModel.QueryCommand = this.mQueryCommand;

            mHealthArchiveListViewModel.ModifyCommand = this.mModifyCommand;
            mHealthArchiveListViewModel.DeleteCommand = this.mDeleteCommand;           
            mHealthArchiveListViewModel.BrowseCommand = this.mBrowseCommand;

            mHealthArchiveViewModel.SaveCommand = this.mSaveCommand;
            mHealthArchiveViewModel.RetreatCommand = this.mRetreatCommand;
        }

        //public bool ShowThumOper()
        //{
        //    bool result = false;

        //    try
        //    {
        //        if (mHealthArchiveViewModel.CheckFileImg != null)
        //        {
        //            mHealthArchiveViewModel.CheckFileImg.StreamSource.Dispose();
        //            mHealthArchiveViewModel.CheckFileImg = null;
        //        }

        //        if (!string.IsNullOrEmpty(mHealthArchiveViewModel.HealthArchive.CheckFileThumb))
        //        {
        //            BackgroundWorker worker = new BackgroundWorker();
        //            worker.DoWork += new DoWorkEventHandler(WorkerShowThumb);
        //            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowThumbCompleted);

        //            worker.RunWorkerAsync();

        //            //mHealthArchiveViewModel.Show_LoadingMask(LoadingType.CheckFile);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return result;
        //}

        //private void WorkerShowThumb(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        mHealthArchiveViewModel.CheckFileImg = ftp.DownloadFile(mHealthArchiveViewModel.HealthArchive.CheckFileThumb);
        //        e.Result = true;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }
        //    finally
        //    {
        //        if (ftp != null)
        //        {
        //            ftp.Disconnect();
        //            ftp = null;
        //        }
        //    }
        //}

        //private void WorkerShowThumbCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    bool ret = (bool)e.Result;

        //    //mHealthArchiveViewModel.Shutdown_LoadingMask(LoadingType.CheckFile);
        //}

        //public bool ShowCheckFileOper()
        //{
        //    bool result = false;

        //    try
        //    {
        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.DoWork += new DoWorkEventHandler(WorkerShowCheckFile);
        //        worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowCheckFileCompleted);

        //        worker.RunWorkerAsync();

        //        //mHealthArchiveViewModel.Show_LoadingMask(LoadingType.View);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return result;
        //}

        //private void WorkerShowCheckFile(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        if (mShellViewModel.SourceImage != null)
        //        {
        //            mShellViewModel.SourceImage.StreamSource.Dispose();
        //            mShellViewModel.SourceImage = null;
        //        }
        //        mShellViewModel.SourceImage = ftp.DownloadFile(mHealthArchiveViewModel.HealthArchive.CheckFile);
        //        e.Result = true;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }
        //    finally
        //    {
        //        if (ftp != null)
        //        {
        //            ftp.Disconnect();
        //            ftp = null;
        //        }
        //    }
        //}

        //private void WorkerShowCheckFileCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    bool ret = (bool)e.Result;

        //    mShellViewModel.ShowMyImage(30, false);

        //    //mHealthArchiveViewModel.Shutdown_LoadingMask(LoadingType.View);
        //}

        public bool NewOper()
        {
            bool newer = true;
            mHealthArchiveViewModel.Operation = "New";
            mHealthArchiveViewModel.HealthArchive = new HealthArchiveTable();
            mMainFrameViewModel.ContentView = mHealthArchiveViewModel.View;
            return newer;
        }

        public bool QueryOper()
        {
            bool deal = true;
            mHealthArchiveViewModel.Operation = "Query";
            try
            {
                mHealthArchiveListViewModel.HealthArchives = mEntityService.EnumHealthArchives.Where<HealthArchiveTable>
                       (
                           entity =>
                               ((!string.IsNullOrEmpty(mHealthArchiveListViewModel.SelectDepartCode)) ? (entity.DepartmentCode.IndexOf(mHealthArchiveListViewModel.SelectDepartCode) == 0) : true)
                               &&
                               ((mHealthArchiveListViewModel.SelectYear != 0) ? (entity.CheckTime.Value.Year == mHealthArchiveListViewModel.SelectYear) : true)
                       );

                //列表页
                mHealthArchiveListViewModel.GridRefresh();
            }
            catch (System.Exception ex)
            {
                deal = false;
                //errlog处理
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool RetreatOper()
        {
            bool newer = true;
            mHealthArchiveViewModel.Operation = "Retreat";
            mHealthArchiveViewModel.HealthArchive = null;

            mMainFrameViewModel.ContentView = mHealthArchiveListViewModel.View;
            return newer;
        }

        public bool ModifyOper()
        {
            bool newer = true;
            mHealthArchiveViewModel.Operation = "Modify";
            mHealthArchiveViewModel.HealthArchive = mHealthArchiveListViewModel.SelectedHealthArchive;
            mMainFrameViewModel.ContentView = mHealthArchiveViewModel.View;
            return newer;
        }

        public bool DeleteOper()
        {            
            bool deler = true;
            mHealthArchiveViewModel.Operation = "Delete";

            try
            {
                if (mHealthArchiveListViewModel.SelectedHealthArchive != null)
                {
                    mHealthArchiveListViewModel.SelectedHealthArchive.IsDeleted = true;
                    //entityService.HealthArchives.Remove(healthArchiveListViewModel.SelectedHealthArchive);
                    mEntityService.Entities.SaveChanges();
                    //刷新DataGrid
                    mHealthArchiveListViewModel.GridRefresh();

                    mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "删除成功."));
                }
                else
                {
                    mMessageService.ShowError(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "未找到"));
                }
            }
            catch (System.Exception ex)
            {
                deler = false;
                //errlog处理
                CurrentLoginService.Instance.LogException(ex);
            }            

            return deler;
        }

        public bool BrowseOper()
        {
            bool deal = true;
            mHealthArchiveViewModel.Operation = "Browse";
            mHealthArchiveViewModel.HealthArchive = mHealthArchiveListViewModel.SelectedHealthArchive;
            mMainFrameViewModel.ContentView = mHealthArchiveViewModel.View;

            return deal;
        }

        public bool Save()
        {
            bool saved = false;

            if (string.IsNullOrEmpty(mHealthArchiveViewModel.HealthArchive.Name))
            {
                mMessageService.ShowMessage("体检人是必填项");
                return false;
            }

            if (mHealthArchiveViewModel.HealthArchive.CheckTime == null || mHealthArchiveViewModel.HealthArchive.CheckTime == DateTime.MinValue)
            {
                mMessageService.ShowMessage("体检日期是必填项");
                return false;
            }

            if (string.IsNullOrEmpty(mHealthArchiveViewModel.HealthArchive.StrSpare1))
            {
                mMessageService.ShowMessage("体检过程是必填项");
                return false;
            }

            if (string.IsNullOrEmpty(mHealthArchiveViewModel.HealthArchive.CheckResult))
            {
                mMessageService.ShowMessage("体检结果是必填项");
                return false;
            }

            try
            {
                if (mHealthArchiveViewModel.HealthArchive.Id > 0)
                {
                    mHealthArchiveViewModel.HealthArchive.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHealthArchiveViewModel.HealthArchive.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHealthArchiveViewModel.HealthArchive.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges(); //update
                }
                else
                {
                    if (CurrentLoginService.Instance.CurrentUserInfo.DepartmentId != null)
                    {
                        mHealthArchiveViewModel.HealthArchive.DepartmentId = CurrentLoginService.Instance.CurrentUserInfo.DepartmentId;
                        mHealthArchiveViewModel.HealthArchive.DepartmentCode = mHealthArchiveListViewModel.DepartmentList.Find(instance => (instance.Id == mHealthArchiveViewModel.HealthArchive.DepartmentId)).Code;
                        mHealthArchiveViewModel.HealthArchive.DepartmentName = CurrentLoginService.Instance.CurrentUserInfo.DepartmentName;
                    }

                    mHealthArchiveViewModel.HealthArchive.IsDeleted = false;
                    mHealthArchiveViewModel.HealthArchive.CreateUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHealthArchiveViewModel.HealthArchive.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHealthArchiveViewModel.HealthArchive.CreateTime = System.DateTime.Now;

                    mHealthArchiveViewModel.HealthArchive.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHealthArchiveViewModel.HealthArchive.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHealthArchiveViewModel.HealthArchive.UpdateTime = System.DateTime.Now;

                    mEntityService.Entities.HealthArchiveTables.AddObject(mHealthArchiveViewModel.HealthArchive);

                    mEntityService.Entities.SaveChanges(); //insert
                }
                saved = true;
                

                mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                //返回列表页
                mMainFrameViewModel.ContentView = mHealthArchiveListViewModel.View;

            }
            catch (ValidationException e)
            {
                mMessageService.ShowError(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidEntities, e.Message));
            }
            catch (UpdateException e)
            {
                mMessageService.ShowError(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidFields, e.InnerException.Message));
            }
            catch (System.Exception ex)
            {
                saved = false;
                //errlog处理
                CurrentLoginService.Instance.LogException(ex);
            }   
            
            return saved;
        }


        //public bool Save()
        //{
        //    bool saver = true;

        //    if (!ValueCheck())
        //    {
        //        return false;
        //    }

        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.DoWork += new DoWorkEventHandler(WorkerSave);
        //    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerSaveCompleted);

        //    worker.RunWorkerAsync();

        //    //mHealthArchiveViewModel.Show_LoadingMask(LoadingType.View);

        //    return saver;
        //}

        //private void WorkerSave(object sender, DoWorkEventArgs e)
        //{
        //    string oldFile;
        //    string oldThumb;
        //    FtpHelper ftp = null;

        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        if (mHealthArchiveViewModel.HealthArchive.Id > 0)
        //        {
        //            mHealthArchiveViewModel.HealthArchive.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
        //            mHealthArchiveViewModel.HealthArchive.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
        //            mHealthArchiveViewModel.HealthArchive.UpdateTime = System.DateTime.Now;
        //            //if (!string.IsNullOrEmpty(mHealthArchiveViewModel.CheckFileLocalPath))
        //            {
        //                oldFile = mHealthArchiveViewModel.HealthArchive.CheckFile;
        //                oldThumb = mHealthArchiveViewModel.HealthArchive.CheckFileThumb;
        //                //mHealthArchiveViewModel.HealthArchive.CheckFileThumb = ftp.UpdateFile(FtpFileType.Thumbnail, mHealthArchiveViewModel.CheckFileLocalPath, oldThumb);
        //                //mHealthArchiveViewModel.HealthArchive.CheckFile = ftp.UpdateFile(FtpFileType.Picture, mHealthArchiveViewModel.CheckFileLocalPath, oldFile);
        //                if (string.IsNullOrEmpty(mHealthArchiveViewModel.HealthArchive.CheckFileThumb)
        //                    || string.IsNullOrEmpty(mHealthArchiveViewModel.HealthArchive.CheckFile))
        //                {
        //                    throw new ValidationException();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //if (!string.IsNullOrEmpty(mHealthArchiveViewModel.CheckFileLocalPath))
        //            {
        //                //mHealthArchiveViewModel.HealthArchive.CheckFileThumb = ftp.UploadFile(FtpFileType.Thumbnail, mHealthArchiveViewModel.CheckFileLocalPath);
        //                //mHealthArchiveViewModel.HealthArchive.CheckFile = ftp.UploadFile(FtpFileType.Picture, mHealthArchiveViewModel.CheckFileLocalPath);
        //                if (string.IsNullOrEmpty(mHealthArchiveViewModel.HealthArchive.CheckFileThumb)
        //                    || string.IsNullOrEmpty(mHealthArchiveViewModel.HealthArchive.CheckFile))
        //                {
        //                    throw new ValidationException();
        //                }
        //            }
        //            mHealthArchiveViewModel.HealthArchive.Name = CurrentLoginService.Instance.CurrentUserInfo.RealName;
        //            if (CurrentLoginService.Instance.CurrentUserInfo.DepartmentId != null)
        //            {
        //                mHealthArchiveViewModel.HealthArchive.DepartmentId = CurrentLoginService.Instance.CurrentUserInfo.DepartmentId;
        //                mHealthArchiveViewModel.HealthArchive.DepartmentCode = mHealthArchiveListViewModel.DepartmentList.Find(instance => (instance.Id == mHealthArchiveViewModel.HealthArchive.DepartmentId)).Code;
        //                mHealthArchiveViewModel.HealthArchive.DepartmentName = CurrentLoginService.Instance.CurrentUserInfo.DepartmentName;
        //            }

        //            mHealthArchiveViewModel.HealthArchive.IsDeleted = false;
        //            mHealthArchiveViewModel.HealthArchive.CreateUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
        //            mHealthArchiveViewModel.HealthArchive.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
        //            mHealthArchiveViewModel.HealthArchive.CreateTime = System.DateTime.Now;

        //            mHealthArchiveViewModel.HealthArchive.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
        //            mHealthArchiveViewModel.HealthArchive.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
        //            mHealthArchiveViewModel.HealthArchive.UpdateTime = System.DateTime.Now;

        //            mEntityService.Entities.HealthArchiveTables.AddObject(mHealthArchiveViewModel.HealthArchive);
        //        }
        //        mEntityService.Entities.SaveChanges();
        //        e.Result = true;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }
        //    finally
        //    {
        //        if (ftp != null)
        //        {
        //            ftp.Disconnect();
        //            ftp = null;
        //        }
        //    }
        //}

        //private void WorkerSaveCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    bool ret = (bool)e.Result;

        //    //mHealthArchiveViewModel.Shutdown_LoadingMask(LoadingType.View);
        //    if (ret)
        //    {
        //        MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        //返回列表页
        //        mMainFrameViewModel.ContentView = mHealthArchiveListViewModel.View;
        //    }
        //    else
        //    {
        //        MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private bool ValueCheck()
        {
            if (string.IsNullOrEmpty(mHealthArchiveViewModel.HealthArchive.PoliceNumber))
            {
                mMessageService.ShowMessage("警号是必填项");
                return false;
            }

            if (string.IsNullOrEmpty(mHealthArchiveViewModel.HealthArchive.IdCardNumber))
            {
                mMessageService.ShowMessage("身份证号是必填项");
                return false;
            }

            if (mHealthArchiveViewModel.HealthArchive.CheckTime == null || mHealthArchiveViewModel.HealthArchive.CheckTime == DateTime.MinValue)
            {
                mMessageService.ShowMessage("查体时间是必填项");
                return false;
            }

            return true;
        }

        //private void HealthArchiveListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "QueryCommand")
        //    {               
        //        healthArchiveListViewModel.HealthArchives = entityService.Entities.HealthArchiveTables.Where<HealthArchiveTable>
        //            (
        //                entity =>
        //                    ((!string.IsNullOrEmpty(healthArchiveListViewModel.SelectDepartCode)) ? (entity.DepartmentCode.IndexOf(healthArchiveListViewModel.SelectDepartCode) == 0) : true)
        //                    &&
        //                    ((healthArchiveListViewModel.SelectYear != 0) ? (entity.CheckTime.Value.Year == healthArchiveListViewModel.SelectYear) : true)
        //            );

        //        //列表页
        //        healthArchiveListViewModel.GridRefresh();
        //    }
        //}

    }
}