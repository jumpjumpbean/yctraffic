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
using WafTraffic.Domain.Common;
using WafTraffic.Applications.Common;
using WafTraffic.Applications.Utils;
using System.Windows.Forms;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class HotLineController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mMainFrameViewModel;
        private HotLineListViewModel mHotLineListViewModel;
        private HotLineViewModel mHotLineViewModel;
        private HotLineDealViewModel mHotLineDealViewModel;
        private HotLineCheckViewModel mHotLineCheckViewModel;
        private HotLineShowViewModel mHotLineShowViewModel;
        private ShellViewModel mShellViewModel;
        //private LbStaticLogbookDetailsViewModel mStaticLogbookDetailsViewModel;
        private LbStaticLogbookDocViewModel mStaticLogbookDocViewModel;


        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mDealCommand;
        private readonly DelegateCommand mCheckCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mRetreatCommand;
        private readonly DelegateCommand mQueryCommand;

        private readonly DelegateCommand mDownloadCommand;

        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mSaveDealCommand;
        private readonly DelegateCommand mSaveCheckCommand;
        //private readonly DelegateCommand mShowThumbCommand;
        private readonly DelegateCommand mShowContentCommand;
        private readonly DelegateCommand mShowVerifyCommand;
        private readonly DelegateCommand mShowDealContentCommand;
        private readonly DelegateCommand mShowCheckContentCommand;
        private readonly DelegateCommand displayCommand;

        private string mFileFrom;
        private string mFileTo;

        [ImportingConstructor]
        public HotLineController(CompositionContainer container,IShellService shellService, System.Waf.Applications.Services.IMessageService messageService, IEntityService entityService)
        {
            this.container = container;
            this.messageService = messageService;
            this.shellService = shellService;
            this.entityService = entityService;

            mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
            mHotLineListViewModel = container.GetExportedValue<HotLineListViewModel>();
            mHotLineViewModel = container.GetExportedValue<HotLineViewModel>();
            mHotLineDealViewModel = container.GetExportedValue<HotLineDealViewModel>();
            mHotLineCheckViewModel = container.GetExportedValue<HotLineCheckViewModel>();
            mHotLineShowViewModel = container.GetExportedValue<HotLineShowViewModel>();
            mShellViewModel = container.GetExportedValue<ShellViewModel>();
            //mStaticLogbookDetailsViewModel = container.GetExportedValue<LbStaticLogbookDetailsViewModel>();
            mStaticLogbookDocViewModel = container.GetExportedValue<LbStaticLogbookDocViewModel>();

            this.mNewCommand = new DelegateCommand(() => NewOper(), null);
            this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
            this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
            this.mDealCommand = new DelegateCommand(() => DealOper(), null);
            this.mCheckCommand = new DelegateCommand(() => CheckOper(), null);
            this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
            this.mRetreatCommand = new DelegateCommand(() => RetreatOper(), null);
            this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);

            this.mDownloadCommand = new DelegateCommand(() => DownloadOper(), null);

            this.mSaveCommand = new DelegateCommand(() => Save(), CanSave);
            this.mSaveDealCommand = new DelegateCommand(() => SaveDeal(), CanSaveDeal);
            this.mSaveCheckCommand = new DelegateCommand(() => SaveCheck(), null);
            this.mShowContentCommand = new DelegateCommand(() => ShowContentOper(), null);
            this.mShowVerifyCommand = new DelegateCommand(() => ShowVerifyOper(), null);
            this.mShowDealContentCommand = new DelegateCommand(() => ShowDealContentOper(), null);
            this.mShowCheckContentCommand = new DelegateCommand(() => ShowCheckContentOper(), null);
            this.displayCommand = new DelegateCommand(() => DisplayOper(), null);

        }

        public void Initialize()
        {
            //AddWeakEventListener(userListViewModel, ModuleListViewModelPropertyChanged);

            mHotLineListViewModel.NewCommand = this.mNewCommand;
            mHotLineListViewModel.ModifyCommand = this.mModifyCommand;
            mHotLineListViewModel.DeleteCommand = this.mDeleteCommand;
            mHotLineListViewModel.DealCommand = this.mDealCommand;
            mHotLineListViewModel.CheckCommand = this.mCheckCommand;
            mHotLineListViewModel.BrowseCommand = this.mBrowseCommand;
            mHotLineListViewModel.QueryCommand = this.mQueryCommand;

            mHotLineViewModel.SaveCommand = this.mSaveCommand;
            mHotLineViewModel.RetreatCommand = this.mRetreatCommand;
            mHotLineViewModel.ContentLocalPath = string.Empty;

            mHotLineDealViewModel.SaveDealCommand = this.mSaveDealCommand;
            mHotLineDealViewModel.RetreatCommand = this.mRetreatCommand;
            mHotLineDealViewModel.ShowContentCommand = this.mShowDealContentCommand;
            mHotLineDealViewModel.DownloadCommand = this.mDownloadCommand;
            mHotLineDealViewModel.DisplayCommand = this.displayCommand;

            mHotLineCheckViewModel.SaveCheckCommand = this.mSaveCheckCommand;
            mHotLineCheckViewModel.RetreatCommand = this.mRetreatCommand;
            mHotLineCheckViewModel.ShowContentCommand = this.mShowCheckContentCommand;
            mHotLineCheckViewModel.DownloadCommand = this.mDownloadCommand;
            mHotLineCheckViewModel.DisplayCommand = this.displayCommand;
            mHotLineCheckViewModel.VerifyLocalPath = string.Empty;

            mHotLineShowViewModel.RetreatCommand = this.mRetreatCommand;
            mHotLineShowViewModel.ShowContentCommand = this.mShowContentCommand;
            mHotLineShowViewModel.ShowVerifyCommand = this.mShowVerifyCommand;
            mHotLineShowViewModel.DownloadCommand = this.mDownloadCommand;
            mHotLineShowViewModel.DisplayCommand = this.displayCommand;
        }

        #region HotLineShowView

        //public bool ShowThumOper()
        //{
        //    bool result = false;

        //    try
        //    {
        //        ClearThumb();

        //        if (!string.IsNullOrEmpty(mHotLineShowViewModel.HotLineTask.ContentThumb))
        //        {
        //            ShowContentThum();
        //        }
        //        if (!string.IsNullOrEmpty(mHotLineShowViewModel.HotLineTask.VerifyThum))
        //        {
        //            ShowVerifyThum();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return result;
        //}

        //private void ClearThumb()
        //{
        //    if (mHotLineShowViewModel.ContentImg != null)
        //    {
        //        mHotLineShowViewModel.ContentImg.StreamSource.Dispose();
        //        mHotLineShowViewModel.ContentImg = null;
        //    }
        //    if (mHotLineShowViewModel.VerifyImg != null)
        //    {
        //        mHotLineShowViewModel.VerifyImg.StreamSource.Dispose();
        //        mHotLineShowViewModel.VerifyImg = null;
        //    }
        //}

        //private void ShowContentThum()
        //{
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.DoWork += new DoWorkEventHandler(WorkerShowContentThumb);
        //    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowContentThumbCompleted);

        //    worker.RunWorkerAsync();

        //    mHotLineShowViewModel.ShowLoadingMask(LoadingType.Content);
        //}

        //private void WorkerShowContentThumb(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        mHotLineShowViewModel.ContentImg = ftp.DownloadFile(mHotLineShowViewModel.HotLineTask.ContentThumb);
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

        //private void WorkerShowContentThumbCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    bool ret = (bool)e.Result;

        //    mHotLineShowViewModel.ShutdownLoadingMask(LoadingType.Content);
        //}

        //private void ShowVerifyThum()
        //{
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.DoWork += new DoWorkEventHandler(WorkerShowVerifyThumb);
        //    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowVerifyThumbCompleted);

        //    worker.RunWorkerAsync();

        //    mHotLineShowViewModel.ShowLoadingMask(LoadingType.VerifyFile);
        //}

        //private void WorkerShowVerifyThumb(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        mHotLineShowViewModel.VerifyImg = ftp.DownloadFile(mHotLineShowViewModel.HotLineTask.VerifyThum);
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

        //private void WorkerShowVerifyThumbCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    bool ret = (bool)e.Result;

        //    mHotLineShowViewModel.ShutdownLoadingMask(LoadingType.VerifyFile);
        //}

        public bool ShowContentOper()
        {
            bool result = false;

            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(WorkerShowContent);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowImageCompleted);

                worker.RunWorkerAsync();

                mHotLineShowViewModel.ShowLoadingMask(LoadingType.View);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }

        private void WorkerShowContent(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;
            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (mShellViewModel.SourceImage != null)
                {
                    mShellViewModel.SourceImage.StreamSource.Dispose();
                    mShellViewModel.SourceImage = null;
                }
                mShellViewModel.SourceImage = ftp.DownloadFile(mHotLineShowViewModel.HotLineTask.ContentPicture);
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

        public bool ShowVerifyOper()
        {
            bool result = false;

            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(WorkerShowVerify);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowImageCompleted);

                worker.RunWorkerAsync();

                mHotLineShowViewModel.ShowLoadingMask(LoadingType.View);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }

        private void WorkerShowVerify(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;
            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (mShellViewModel.SourceImage != null)
                {
                    mShellViewModel.SourceImage.StreamSource.Dispose();
                    mShellViewModel.SourceImage = null;
                }
                mShellViewModel.SourceImage = ftp.DownloadFile(mHotLineShowViewModel.HotLineTask.VerifyFile);
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

        private void WorkerShowImageCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            mShellViewModel.ShowMyImage(30, false);

            mHotLineShowViewModel.ShutdownLoadingMask(LoadingType.View);
        }

        #endregion

        #region HotLineDealView

        private void WorkerShowDealContentThumbCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            mHotLineDealViewModel.ShutdownLoadingMask(LoadingType.Content);
        }

        public bool ShowDealContentOper()
        {
            bool result = false;

            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(WorkerShowDealContent);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowDealImageCompleted);

                worker.RunWorkerAsync();

                mHotLineDealViewModel.ShowLoadingMask(LoadingType.View);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }

        private void WorkerShowDealContent(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;
            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (mShellViewModel.SourceImage != null)
                {
                    mShellViewModel.SourceImage.StreamSource.Dispose();
                    mShellViewModel.SourceImage = null;
                }
                mShellViewModel.SourceImage = ftp.DownloadFile(mHotLineDealViewModel.HotLineTask.ContentPicture);
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

        private void WorkerShowDealImageCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            mShellViewModel.ShowMyImage(30, false);

            mHotLineDealViewModel.ShutdownLoadingMask(LoadingType.View);
        }

        #endregion

        #region HotLineCheckView


        //public bool ShowCheckThumOper()
        //{
        //    bool result = false;

        //    try
        //    {
        //        if (mHotLineCheckViewModel.ContentImg != null)
        //        {
        //            mHotLineCheckViewModel.ContentImg.StreamSource.Dispose();
        //            mHotLineCheckViewModel.ContentImg = null;
        //        }

        //        if (!string.IsNullOrEmpty(mHotLineCheckViewModel.HotLineTask.ContentThumb))
        //        {
        //            ShowCheckContentThum();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return result;
        //}

        private void ShowCheckContentThum()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(WorkerShowCheckContentThumb);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowCheckContentThumbCompleted);

            worker.RunWorkerAsync();

            mHotLineCheckViewModel.ShowLoadingMask(LoadingType.Content);
        }

        private void WorkerShowCheckContentThumb(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;
            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                mHotLineCheckViewModel.ContentImg = ftp.DownloadFile(mHotLineCheckViewModel.HotLineTask.ContentThumb);
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

        private void WorkerShowCheckContentThumbCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            mHotLineCheckViewModel.ShutdownLoadingMask(LoadingType.Content);
        }

        public bool ShowCheckContentOper()
        {
            bool result = false;

            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(WorkerShowCheckContent);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowCheckImageCompleted);

                worker.RunWorkerAsync();

                mHotLineCheckViewModel.ShowLoadingMask(LoadingType.View);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }

        private void WorkerShowCheckContent(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;
            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (mShellViewModel.SourceImage != null)
                {
                    mShellViewModel.SourceImage.StreamSource.Dispose();
                    mShellViewModel.SourceImage = null;
                }
                mShellViewModel.SourceImage = ftp.DownloadFile(mHotLineCheckViewModel.HotLineTask.ContentPicture);
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

        private void WorkerShowCheckImageCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            mShellViewModel.ShowMyImage(30, false);

            mHotLineCheckViewModel.ShutdownLoadingMask(LoadingType.View);
        }

        #endregion

        /*
public bool ShowPictrue20s()
{
    bool show = true;

    mShellViewModel.HotLinePitcure = mHotLineShowViewModel.HotLineTask;
    mShellViewModel.ShowMyImage(20);

    return show;
}

public bool ShowPictrue20d()
{
    bool show = true;

    mShellViewModel.HotLinePitcure = mHotLineDealViewModel.HotLineTask;
    mShellViewModel.ShowMyImage(20);

    return show;
}

public bool ShowPictrue20c()
{
    bool show = true;

    mShellViewModel.HotLinePitcure = mHotLineCheckViewModel.HotLineTask;
    mShellViewModel.ShowMyImage(20);

    return show;
}


public bool ShowChkPic21()
{
    bool show = true;

    shellViewModel.HotLinePitcure = hotLineCheckViewModel.HotLineTask;
    shellViewModel.ShowMyImage(21);

    return show;
}
public bool ShowChkPic21s()
{
    bool show = true;

    shellViewModel.HotLinePitcure = hotLineShowViewModel.HotLineTask;
    shellViewModel.ShowMyImage(21);

    return show;
}
*/

        public bool NewOper()
        {
            bool newer = true;
            mHotLineViewModel.HotLineTask = new MayorHotlineTaskTable();

            mHotLineViewModel.CanSave = System.Windows.Visibility.Visible;
            mHotLineViewModel.CanSendDDZ = System.Windows.Visibility.Visible;
            mHotLineViewModel.CanSendVice = System.Windows.Visibility.Visible;
            mHotLineViewModel.HotLineTask.CreateDate = DateTime.Now;

            mMainFrameViewModel.ContentView = mHotLineViewModel.View;

            return newer;
        }

        public bool QueryOper()
        {
            bool newer = true;
            //以entityService.EnumHotLineTasks为基础来查询
            mHotLineListViewModel.HotLineTasks = entityService.EnumHotLineTasks.Where<MayorHotlineTaskTable>(entity =>
                ((mHotLineListViewModel.SelectDepartId == 0 || object.Equals(entity.OwnDepartmentId, null) ) ? true : entity.OwnDepartmentId == mHotLineListViewModel.SelectDepartId)
                && (mHotLineListViewModel.SelectContents.Trim() == "" ? true : entity.Contents.Contains(mHotLineListViewModel.SelectContents.Trim()))
                );
            //entityService.Entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, hotLineListViewModel.HotLineTasks);
            mHotLineListViewModel.GridRefresh();

            return newer;
        }

        public bool ModifyOper()
        {
            bool newer = true;
            mHotLineViewModel.HotLineTask = mHotLineListViewModel.SelectedHotline;

            mHotLineViewModel.CanSave = System.Windows.Visibility.Visible;
            mHotLineViewModel.CanSendDDZ = System.Windows.Visibility.Visible;
            mHotLineViewModel.CanSendVice = System.Windows.Visibility.Visible;

            mMainFrameViewModel.ContentView = mHotLineViewModel.View;

            return newer;
        }

        public bool DeleteOper()
        {            
            bool newer = true;
            FtpHelper ftp = null;

            try
            {
                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (mHotLineListViewModel.SelectedHotline != null)
                    {
                        ftp = new FtpHelper();

                        if (!string.IsNullOrEmpty(mHotLineListViewModel.SelectedHotline.ContentPicture))
                        {
                            ftp.RemoveFile(mHotLineListViewModel.SelectedHotline.ContentPicture);
                            ftp.RemoveFile(mHotLineListViewModel.SelectedHotline.ContentThumb);
                        }
                        if (!string.IsNullOrEmpty(mHotLineListViewModel.SelectedHotline.VerifyFile))
                        {
                            ftp.RemoveFile(mHotLineListViewModel.SelectedHotline.VerifyFile);
                            ftp.RemoveFile(mHotLineListViewModel.SelectedHotline.VerifyThum);
                        }
                        mHotLineListViewModel.SelectedHotline.IsDeleted = true;
                        //entityService.HotLineTasks.Remove(hotLineListViewModel.SelectedHotline);
                        entityService.Entities.SaveChanges();
                        //刷新DataGrid
                        mHotLineListViewModel.GridRefresh();
                        MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("未找到", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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

            return newer;
        }

        public bool DealOper()
        {
            bool deal = true;

            //if (hotLineListViewModel.SelectedHotline.OwnDepartmentId != Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId))
            //{
            //    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "不是本科室人员，无权操作."));
            //    return deal;
            //}
            //if (hotLineListViewModel.SelectedHotline.StatusId == 3)
            //{
            //    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "已经归档，操作无效."));
            //    return deal;
            //}

            mHotLineDealViewModel.HotLineTask = mHotLineListViewModel.SelectedHotline;

            if (!string.IsNullOrEmpty(mHotLineDealViewModel.HotLineTask.ContentPictureName))
            {
                mHotLineDealViewModel.CanContentFileDownloadVisibal = System.Windows.Visibility.Visible;
            }
            else
            {
                mHotLineDealViewModel.CanContentFileDownloadVisibal = System.Windows.Visibility.Collapsed;
            }

            //if (!string.IsNullOrEmpty(mHotLineShowViewModel.HotLineTask.VerifyFileName))
            //{
            //    mHotLineDealViewModel.CanVerifyFileDownloadVisibal = System.Windows.Visibility.Visible;
            //}
            //else
            //{
            //    mHotLineDealViewModel.CanVerifyFileDownloadVisibal = System.Windows.Visibility.Collapsed;
            //}
           
            
            mMainFrameViewModel.ContentView = mHotLineDealViewModel.View;

            return deal;
        }

        public bool CheckOper()
        {
            bool deal = true;
            mHotLineCheckViewModel.HotLineTask = mHotLineListViewModel.SelectedHotline;

            if (!string.IsNullOrEmpty(mHotLineCheckViewModel.HotLineTask.ContentPictureName))
            {
                mHotLineCheckViewModel.CanContentFileDownloadVisibal = System.Windows.Visibility.Visible;
            }
            else
            {
                mHotLineCheckViewModel.CanContentFileDownloadVisibal = System.Windows.Visibility.Collapsed;
            }

            mMainFrameViewModel.ContentView = mHotLineCheckViewModel.View;

            return deal;
        }

        public bool BrowseOper()
        {
            bool deal = true;
            mHotLineShowViewModel.HotLineTask = mHotLineListViewModel.SelectedHotline;

            if (!string.IsNullOrEmpty(mHotLineShowViewModel.HotLineTask.ContentPictureName))
            {
                mHotLineShowViewModel.CanContentFileDownloadVisibal = System.Windows.Visibility.Visible;
            }
            else
            {
                mHotLineShowViewModel.CanContentFileDownloadVisibal = System.Windows.Visibility.Collapsed;
            }

            if (!string.IsNullOrEmpty(mHotLineShowViewModel.HotLineTask.VerifyFileName))
            {
                mHotLineShowViewModel.CanVerifyFileDownloadVisibal = System.Windows.Visibility.Visible;
            }
            else
            {
                mHotLineShowViewModel.CanVerifyFileDownloadVisibal = System.Windows.Visibility.Collapsed;
            }

            mMainFrameViewModel.ContentView = mHotLineShowViewModel.View;

            return deal;
        }

        public bool RetreatOper()
        {
            bool deal = true;

            mMainFrameViewModel.ContentView = mHotLineListViewModel.View;

            return deal;
        }

        public bool CanSave()
        {
            //写在此处会提示二次
            //if (hotLineViewModel.HotLineTask.StatusId == 2 && (hotLineViewModel.HotLineTask.IsComplainPolice == null || hotLineViewModel.HotLineTask.IsComplainPolice == 0))
            //{
            //    if (!messageService.ShowYesNoQuestion(hotLineViewModel.View, "不是投诉民警记录，确认要发送给大队长吗？"))                
            //    {
            //        return false;
            //    }
            //}

            //if (hotLineViewModel.HotLineTask.StatusId == 4 && (hotLineViewModel.HotLineTask.OwnDepartmentId == null || hotLineViewModel.HotLineTask.OwnDepartmentId == 0 ))
            //{
            //    messageService.ShowMessage("请选择科室，再发送。");
            //    return false;
            //}
                 

            return true;
        }

        public bool Save()
        {
            bool saver = false;

            if (mHotLineViewModel.HotLineTask.CreateDate == null || mHotLineViewModel.HotLineTask.CreateDate == DateTime.MinValue)
            {
                messageService.ShowMessage("创建时间是必填项");
                return false;
            }

            //if (hotLineViewModel.HotLineTask.OwnDepartmentId == null || hotLineViewModel.HotLineTask.OwnDepartmentId == 0)
            //{
            //    messageService.ShowMessage("负责科室是必填项");
            //    return false;
            //}

            if (mHotLineViewModel.HotLineTask.DueDate == null || mHotLineViewModel.HotLineTask.DueDate == DateTime.MinValue)
            {
                messageService.ShowMessage("到期时间是必填项");
                return false;
            }
            //从CanSave移到此处
            if (mHotLineViewModel.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ToDDZ) && (mHotLineViewModel.HotLineTask.IsComplainPolice == null || mHotLineViewModel.HotLineTask.IsComplainPolice == 0))
            {
                if (!messageService.ShowYesNoQuestion(mHotLineViewModel.View, "不是投诉民警记录，确认要发送给大队长吗？"))
                {
                    return false;
                }
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(WorkerSave);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerSaveCompleted);

            worker.RunWorkerAsync();

            mHotLineViewModel.ShowLoadingMask(LoadingType.View);
            saver = true;
            return saver;
        }

        private void WorkerSave(object sender, DoWorkEventArgs e)
        {
            string oldFile;
            FtpHelper ftp = null;

            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (mHotLineViewModel.HotLineTask.Id > 0)
                {
                    if (!string.IsNullOrEmpty(mHotLineViewModel.ContentLocalPath))
                    {
                        if (string.IsNullOrEmpty(mHotLineViewModel.HotLineTask.ContentPicture))
                        {
                            //mHotLineViewModel.HotLineTask.ContentThumb = ftp.UploadFile(FtpFileType.Thumbnail, mHotLineViewModel.ContentLocalPath);
                            mHotLineViewModel.HotLineTask.ContentPicture = ftp.UploadFile(FtpFileType.Document, mHotLineViewModel.ContentLocalPath);
                        }
                        else
                        {
                            oldFile = mHotLineViewModel.HotLineTask.ContentPicture;
                            //oldThumb = mHotLineViewModel.HotLineTask.ContentThumb;
                            //mHotLineViewModel.HotLineTask.ContentThumb = ftp.UpdateFile(FtpFileType.Thumbnail, mHotLineViewModel.ContentLocalPath, oldThumb);
                            mHotLineViewModel.HotLineTask.ContentPicture = ftp.UpdateFile(FtpFileType.Document, mHotLineViewModel.ContentLocalPath, oldFile);
                        }
                        if (string.IsNullOrEmpty(mHotLineViewModel.HotLineTask.ContentPicture))
                        {
                            throw new ValidationException();
                        }
                    }
                    //有可能修改处理科室
                    if (!object.Equals(null, mHotLineViewModel.HotLineTask.OwnDepartmentId))
                    {
                        mHotLineViewModel.HotLineTask.OwnDepartmentName = mHotLineViewModel.DepartmentList.Find(instance => (instance.Id == mHotLineViewModel.HotLineTask.OwnDepartmentId)).FullName;
                    }

                    mHotLineViewModel.HotLineTask.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineViewModel.HotLineTask.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHotLineViewModel.HotLineTask.UpdateTime = System.DateTime.Now;
                }
                else
                {
                    if (!string.IsNullOrEmpty(mHotLineViewModel.ContentLocalPath))
                    {
                        //mHotLineViewModel.HotLineTask.ContentThumb = ftp.UploadFile(FtpFileType.Thumbnail, mHotLineViewModel.ContentLocalPath);
                        mHotLineViewModel.HotLineTask.ContentPicture = ftp.UploadFile(FtpFileType.Document, mHotLineViewModel.ContentLocalPath);
                        if (string.IsNullOrEmpty(mHotLineViewModel.HotLineTask.ContentPicture))
                        {
                            throw new ValidationException();
                        }
                    }
                    if (!object.Equals(null, mHotLineViewModel.HotLineTask.OwnDepartmentId))
                    {
                        mHotLineViewModel.HotLineTask.OwnDepartmentName = mHotLineViewModel.DepartmentList.Find(instance => (instance.Id == mHotLineViewModel.HotLineTask.OwnDepartmentId)).FullName;
                    }

                    //hotLineViewModel.HotLineTask.Status = "待处理"; //待处理 :1， 发给大队长：2， 发给政委：3， 发给科室：4， 回复大队长：5，  已处理 :6，  归档 :7
                    //hotLineViewModel.HotLineTask.StatusId = 1;

                    mHotLineViewModel.HotLineTask.IsDeleted = false;
                    mHotLineViewModel.HotLineTask.CreateUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineViewModel.HotLineTask.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHotLineViewModel.HotLineTask.CreateTime = System.DateTime.Now;

                    mHotLineViewModel.HotLineTask.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineViewModel.HotLineTask.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHotLineViewModel.HotLineTask.UpdateTime = System.DateTime.Now;

                    entityService.Entities.MayorHotlineTaskTables.AddObject(mHotLineViewModel.HotLineTask);
                }
                entityService.Entities.SaveChanges();
                e.Result = true;
                //更新一下列表数据源  不需要更新数据源
                //entityService.HotLineTasks = new EntityObservableCollection<MayorHotlineTaskTable>(entityService.Entities.MayorHotlineTaskTables);
                //entityService.HotLineTasks = entityService.Entities.MayorHotlineTaskTables.ToList<MayorHotlineTaskTable>();
                //hotLineListViewModel.HotLineTasks = entityService.HotLineTasks;

                //messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, operString));

                if (mHotLineViewModel.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ToDDZ) || mHotLineViewModel.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ToZW))
                {
                    mHotLineViewModel.CanSave = System.Windows.Visibility.Collapsed;
                    mHotLineViewModel.CanSendDDZ = System.Windows.Visibility.Collapsed;
                    mHotLineViewModel.CanSendVice = System.Windows.Visibility.Collapsed;
                }
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

            mHotLineViewModel.ShutdownLoadingMask(LoadingType.View);
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                //mainFrameViewModel.ContentView = container.GetExportedValue<HotLineListViewModel>().View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
        public bool Save()
        {
            bool saved = false;
            string operString = "操作成功.";

            if (mHotLineViewModel.HotLineTask.CreateDate == null || mHotLineViewModel.HotLineTask.CreateDate == DateTime.MinValue)
            {
                messageService.ShowMessage("创建时间是必填项");
                return false;
            }

            //if (hotLineViewModel.HotLineTask.OwnDepartmentId == null || hotLineViewModel.HotLineTask.OwnDepartmentId == 0)
            //{
            //    messageService.ShowMessage("负责科室是必填项");
            //    return false;
            //}

            if (mHotLineViewModel.HotLineTask.DueDate == null || mHotLineViewModel.HotLineTask.DueDate == DateTime.MinValue)
            {
                messageService.ShowMessage("到期时间是必填项");
                return false;
            }    
            //从CanSave移到此处
            if (mHotLineViewModel.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ToDDZ) && (mHotLineViewModel.HotLineTask.IsComplainPolice == null || mHotLineViewModel.HotLineTask.IsComplainPolice == 0))
            {
                if (!messageService.ShowYesNoQuestion(mHotLineViewModel.View, "不是投诉民警记录，确认要发送给大队长吗？"))
                {
                    return false;
                }
            }

            try
            {
                if (mHotLineViewModel.HotLineTask.Id > 0)
                {
                    //有可能修改处理科室
                    if (!object.Equals(null, mHotLineViewModel.HotLineTask.OwnDepartmentId))
                    {
                        mHotLineViewModel.HotLineTask.OwnDepartmentName = mHotLineViewModel.DepartmentList.Find(instance => (instance.Id == mHotLineViewModel.HotLineTask.OwnDepartmentId)).FullName;
                    }

                    mHotLineViewModel.HotLineTask.UpdaterId = Convert.ToInt32( CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineViewModel.HotLineTask.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHotLineViewModel.HotLineTask.UpdateTime = System.DateTime.Now;
                    entityService.Entities.SaveChanges(); //update
                }
                else
                {
                    if (!object.Equals(null, mHotLineViewModel.HotLineTask.OwnDepartmentId))
                    {
                        mHotLineViewModel.HotLineTask.OwnDepartmentName = mHotLineViewModel.DepartmentList.Find(instance => (instance.Id == mHotLineViewModel.HotLineTask.OwnDepartmentId)).FullName;
                    }

                    //hotLineViewModel.HotLineTask.Status = "待处理"; //待处理 :1， 发给大队长：2， 发给政委：3， 发给科室：4， 回复大队长：5，  已处理 :6，  归档 :7
                    //hotLineViewModel.HotLineTask.StatusId = 1;

                    mHotLineViewModel.HotLineTask.IsDeleted = false;
                    mHotLineViewModel.HotLineTask.CreateUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineViewModel.HotLineTask.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHotLineViewModel.HotLineTask.CreateTime = System.DateTime.Now;

                    mHotLineViewModel.HotLineTask.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineViewModel.HotLineTask.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHotLineViewModel.HotLineTask.UpdateTime = System.DateTime.Now;

                    entityService.Entities.MayorHotlineTaskTables.AddObject(mHotLineViewModel.HotLineTask);

                    entityService.Entities.SaveChanges(); //insert

                    operString = "保存成功.";
                }
                saved = true;
                //更新一下列表数据源  不需要更新数据源
                //entityService.HotLineTasks = new EntityObservableCollection<MayorHotlineTaskTable>(entityService.Entities.MayorHotlineTaskTables);
                //entityService.HotLineTasks = entityService.Entities.MayorHotlineTaskTables.ToList<MayorHotlineTaskTable>();
                //hotLineListViewModel.HotLineTasks = entityService.HotLineTasks;

                messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, operString));

                if (mHotLineViewModel.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ToDDZ) || mHotLineViewModel.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ToZW))
                {
                    mHotLineViewModel.CanSave = System.Windows.Visibility.Collapsed;
                    mHotLineViewModel.CanSendDDZ = System.Windows.Visibility.Collapsed;
                    mHotLineViewModel.CanSendVice = System.Windows.Visibility.Collapsed;
                }
                //返回列表页
                //mainFrameViewModel.ContentView = container.GetExportedValue<HotLineListViewModel>().View;
            }
            catch (ValidationException e)
            {
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidEntities, e.Message));
            }
            catch (UpdateException e)
            {
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
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
        */

        public bool CanSaveDeal()
        {
            
            return true;
        }

        public bool SaveDeal()
        {
            bool saved = false;
            if (mHotLineDealViewModel.HotLineTask.OwnDepartmentId == null || mHotLineDealViewModel.HotLineTask.OwnDepartmentId == 0)
            {
                MessageBox.Show("负责科室是必填项", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //if (hotLineViewModel.HotLineTask.IsComplainPolice != 1 && hotLineViewModel.HotLineTask.StatusId == 5)
            //{
            //    return messageService.ShowYesNoQuestion("不是投诉民警记录，确认要回复大队长吗？");
            //}
            //if (hotLineDealViewModel.HotLineTask.OwnDepartmentId != Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId))
            //{
            //    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "不是本科室人员，无权操作."));
            //    return saved;
            //}
            //待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  已归档 :8
            if (mHotLineDealViewModel.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.Archive))
            {
                MessageBox.Show("已经归档，操作无效", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int i = 0;
            try
            {
                if (!object.Equals(null, mHotLineDealViewModel.HotLineTask.OwnDepartmentId))
                {
                    mHotLineDealViewModel.HotLineTask.OwnDepartmentName = mHotLineDealViewModel.DepartmentList.Find(instance => (instance.Id == mHotLineDealViewModel.HotLineTask.OwnDepartmentId)).FullName;
                }

                if (mHotLineDealViewModel.HotLineTask.Id > 0)
                {
                    //hotLineDealViewModel.HotLineTask.Status = "已处理";
                    //hotLineDealViewModel.HotLineTask.StatusId = 6;
                    //hotLineDealViewModel.HotLineTask.SovleUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    //hotLineDealViewModel.HotLineTask.SovleUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    mHotLineDealViewModel.HotLineTask.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineDealViewModel.HotLineTask.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHotLineDealViewModel.HotLineTask.UpdateTime = System.DateTime.Now;
                    i = entityService.Entities.SaveChanges(); //update
                }
                
                saved = true;

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "操作成功."));
                //返回列表页
                mMainFrameViewModel.ContentView = container.GetExportedValue<HotLineListViewModel>().View;
            }
            catch (System.Exception ex)
            {
                saved = false;
                //errlog处理
                CurrentLoginService.Instance.LogException(ex);
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return saved;
        }

        public bool SaveCheck()
        {
            bool saver = false;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(WorkerSaveCheck);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerSaveCheckCompleted);

            worker.RunWorkerAsync();

            mHotLineCheckViewModel.ShowLoadingMask(LoadingType.View);
            saver = true;
            return saver;
        }

        private void WorkerSaveCheck(object sender, DoWorkEventArgs e)
        {
            string oldFile;
            FtpHelper ftp = null;

            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (mHotLineCheckViewModel.HotLineTask.Id > 0)
                {
                    if (!string.IsNullOrEmpty(mHotLineCheckViewModel.VerifyLocalPath))
                    {
                        if (string.IsNullOrEmpty(mHotLineCheckViewModel.HotLineTask.VerifyFile))
                        {
                            mHotLineCheckViewModel.HotLineTask.VerifyFile = ftp.UploadFile(FtpFileType.Document, mHotLineCheckViewModel.VerifyLocalPath);
                        }
                        else
                        {
                            oldFile = mHotLineCheckViewModel.HotLineTask.VerifyFile;
                            mHotLineCheckViewModel.HotLineTask.VerifyFile = ftp.UpdateFile(FtpFileType.Document, mHotLineCheckViewModel.VerifyLocalPath, oldFile);
                        }
                        if (string.IsNullOrEmpty(mHotLineCheckViewModel.HotLineTask.VerifyFile))
                        {
                            throw new ValidationException();
                        }
                    }
                    mHotLineCheckViewModel.HotLineTask.Status = "已归档";
                    mHotLineCheckViewModel.HotLineTask.StatusId = Convert.ToInt32(HotLineStatus.Archive);
                    mHotLineCheckViewModel.HotLineTask.VerifyUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineCheckViewModel.HotLineTask.VerifyUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    mHotLineCheckViewModel.HotLineTask.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineCheckViewModel.HotLineTask.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHotLineCheckViewModel.HotLineTask.UpdateTime = System.DateTime.Now;
                    entityService.Entities.SaveChanges(); //update
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

        private void WorkerSaveCheckCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            mHotLineCheckViewModel.ShutdownLoadingMask(LoadingType.View);
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                //mainFrameViewModel.ContentView = container.GetExportedValue<HotLineListViewModel>().View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool DownloadOper()
        {
            bool dealer = true;
            try
            {
                SaveFileDialog sf = new SaveFileDialog();

                sf.AddExtension = true;
                sf.RestoreDirectory = true;

                if (mHotLineShowViewModel.FileType == HotLineFileType.ContentFile || mHotLineCheckViewModel.FileType == HotLineFileType.ContentFile || 
                    mHotLineDealViewModel.FileType == HotLineFileType.ContentFile)
                {
                    sf.FileName = mHotLineListViewModel.SelectedHotline.ContentPictureName;
                }
                else
                {
                    sf.FileName = mHotLineListViewModel.SelectedHotline.VerifyFileName;
                }

                if (sf.ShowDialog() == DialogResult.OK)
                {
                    if (mHotLineShowViewModel.FileType == HotLineFileType.ContentFile || mHotLineCheckViewModel.FileType == HotLineFileType.ContentFile ||
                        mHotLineDealViewModel.FileType == HotLineFileType.ContentFile)
                    {
                        mFileFrom = mHotLineListViewModel.SelectedHotline.ContentPicture;
                    }
                    else
                    {
                        mFileFrom = mHotLineListViewModel.SelectedHotline.VerifyFile;
                    }
                    mFileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    mHotLineShowViewModel.ShowLoadingMask(LoadingType.Content);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("下载失败，文件未找到或已过期", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }
            finally
            {
                mHotLineShowViewModel.FileType = HotLineFileType.None;
                mHotLineCheckViewModel.FileType = HotLineFileType.None;
                mHotLineDealViewModel.FileType = HotLineFileType.None;
            }

            return dealer;
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

                mHotLineShowViewModel.ShutdownLoadingMask(LoadingType.Content);

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

                if (mHotLineShowViewModel.FileType == HotLineFileType.ContentFile || mHotLineCheckViewModel.FileType == HotLineFileType.ContentFile ||
                    mHotLineDealViewModel.FileType == HotLineFileType.ContentFile)
                {
                    mFileFrom = mHotLineListViewModel.SelectedHotline.ContentPicture;
                    mFileTo = tempPath + mHotLineListViewModel.SelectedHotline.ContentPictureName;
                }
                else
                {
                    mFileFrom = mHotLineListViewModel.SelectedHotline.VerifyFile;
                    mFileTo = tempPath + mHotLineListViewModel.SelectedHotline.VerifyFileName;
                }

                mStaticLogbookDocViewModel.FilePath = mFileTo;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_Display);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DisplayCompleted);
                worker.RunWorkerAsync();

                if (mHotLineShowViewModel.FileType == HotLineFileType.ContentFile || mHotLineShowViewModel.FileType == HotLineFileType.VerifyFile)
                {
                    mHotLineShowViewModel.ShowLoadingMask(LoadingType.Content);
                }
                else if (mHotLineCheckViewModel.FileType == HotLineFileType.ContentFile || mHotLineCheckViewModel.FileType == HotLineFileType.VerifyFile)
                {
                    mHotLineCheckViewModel.ShowLoadingMask(LoadingType.Content);
                }
                else
                {
                    mHotLineDealViewModel.ShowLoadingMask(LoadingType.Content);
                }
            }
            catch (System.Exception ex)
            {
                mHotLineShowViewModel.FileType = HotLineFileType.None;
                mHotLineCheckViewModel.FileType = HotLineFileType.None;
                mHotLineDealViewModel.FileType = HotLineFileType.None;
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
                mHotLineShowViewModel.FileType = HotLineFileType.None;
                mHotLineCheckViewModel.FileType = HotLineFileType.None;
                mHotLineDealViewModel.FileType = HotLineFileType.None;
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

                if (mHotLineShowViewModel.FileType == HotLineFileType.ContentFile || mHotLineShowViewModel.FileType == HotLineFileType.VerifyFile)
                {
                    mHotLineShowViewModel.ShutdownLoadingMask(LoadingType.Content);
                }
                else if (mHotLineCheckViewModel.FileType == HotLineFileType.ContentFile || mHotLineCheckViewModel.FileType == HotLineFileType.VerifyFile)
                {
                    mHotLineCheckViewModel.ShutdownLoadingMask(LoadingType.Content);
                }
                else
                {
                    mHotLineDealViewModel.ShutdownLoadingMask(LoadingType.Content);
                }

                if (ret)
                {
                    mStaticLogbookDocViewModel.CloseCommand = this.mRetreatCommand;
                    mMainFrameViewModel.ContentView = mStaticLogbookDocViewModel.View;
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
            finally
            {
                mHotLineShowViewModel.FileType = HotLineFileType.None;
                mHotLineCheckViewModel.FileType = HotLineFileType.None;
                mHotLineDealViewModel.FileType = HotLineFileType.None;
            }
        }


        /*
        public bool SaveCheck()
        {
            //待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  已归档 :8
            bool saved = false;

            try
            {
                if (mHotLineCheckViewModel.HotLineTask.Id > 0)
                {
                    mHotLineCheckViewModel.HotLineTask.Status = "已归档";
                    mHotLineCheckViewModel.HotLineTask.StatusId = Convert.ToInt32(HotLineStatus.Archive);
                    mHotLineCheckViewModel.HotLineTask.VerifyUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineCheckViewModel.HotLineTask.VerifyUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    mHotLineCheckViewModel.HotLineTask.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mHotLineCheckViewModel.HotLineTask.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mHotLineCheckViewModel.HotLineTask.UpdateTime = System.DateTime.Now;
                    entityService.Entities.SaveChanges(); //update
                }

                saved = true;

                //messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "操作成功."));
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mMainFrameViewModel.ContentView = container.GetExportedValue<HotLineListViewModel>().View;
            }
            catch (System.Exception ex)
            {
                saved = false;
                //errlog处理
                CurrentLoginService.Instance.LogException(ex);
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return saved;
        }
        */
    }
}
