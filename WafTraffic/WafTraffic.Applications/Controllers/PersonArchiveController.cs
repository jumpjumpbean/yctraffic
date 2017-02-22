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
    internal class PersonArchiveController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private MainFrameViewModel mMainFrameViewModel;
        private PersonArchiveListViewModel mPersonArchiveListViewModel;
        private PersonArchiveViewModel mPersonArchiveViewModel;
        private ShellViewModel mShellViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mRetreatCommand;

        [ImportingConstructor]
        public PersonArchiveController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            this.mContainer = container;
            this.mMessageService = messageService;
            this.mShellService = shellService;
            this.mEntityService = entityService;

            mMainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
            mPersonArchiveListViewModel = container.GetExportedValue<PersonArchiveListViewModel>();
            mPersonArchiveViewModel = container.GetExportedValue<PersonArchiveViewModel>();
            mShellViewModel = container.GetExportedValue<ShellViewModel>();

            this.mNewCommand = new DelegateCommand(() => NewOper(), null);
            this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
            this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
            this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
            this.mSaveCommand = new DelegateCommand(() => Save(), null);
            this.mRetreatCommand = new DelegateCommand(() => RetreatOper(), null);

        }

        public void Initialize()
        {
            AddWeakEventListener(mPersonArchiveListViewModel, PersonArchiveListViewModelPropertyChanged);

            mPersonArchiveListViewModel.NewCommand = this.mNewCommand;
            mPersonArchiveListViewModel.ModifyCommand = this.mModifyCommand;
            mPersonArchiveListViewModel.DeleteCommand = this.mDeleteCommand;           
            mPersonArchiveListViewModel.BrowseCommand = this.mBrowseCommand;

            mPersonArchiveViewModel.SaveCommand = this.mSaveCommand;
            mPersonArchiveViewModel.RetreatCommand = this.mRetreatCommand;
        }

        //public bool ShowThumOper()
        //{
        //    bool result = false;

        //    try
        //    {
        //        if (mPersonArchiveViewModel.PhotoImg != null)
        //        {
        //            mPersonArchiveViewModel.PhotoImg.StreamSource.Dispose();
        //            mPersonArchiveViewModel.PhotoImg = null;
        //        }

        //        if (!string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.PhotoThumb))
        //        {
        //            BackgroundWorker worker = new BackgroundWorker();
        //            worker.DoWork += new DoWorkEventHandler(workerShowThumb);
        //            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerShowThumbCompleted);

        //            worker.RunWorkerAsync();

        //            mPersonArchiveViewModel.Show_LoadingMask(LoadingType.Photo);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return result;
        //}

        //private void workerShowThumb(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        mPersonArchiveViewModel.PhotoImg = ftp.DownloadFile(mPersonArchiveViewModel.PersonArchive.PhotoThumb);
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

        //private void workerShowThumbCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    bool ret = (bool)e.Result;

        //    mPersonArchiveViewModel.Shutdown_LoadingMask(LoadingType.Photo);
        //}

        //public bool ShowPhotoOper()
        //{
        //    bool result = false;

        //    try
        //    {
        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.DoWork += new DoWorkEventHandler(workerShowPhoto);
        //        worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerShowPhotoCompleted);

        //        worker.RunWorkerAsync();

        //        mPersonArchiveViewModel.Show_LoadingMask(LoadingType.View);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return result;
        //}

        //private void workerShowPhoto(object sender, DoWorkEventArgs e)
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

        //private void workerShowPhotoCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    bool ret = (bool)e.Result;

        //    mShellViewModel.ShowMyImage(30, false);

        //    mPersonArchiveViewModel.Shutdown_LoadingMask(LoadingType.View);
        //}

        public bool NewOper()
        {
            bool newer = true;
            mPersonArchiveViewModel.Operation = "New";
            mPersonArchiveViewModel.PersonArchive = new PersonArchiveTable();
            mMainFrameViewModel.ContentView = mPersonArchiveViewModel.View;
            return newer;
        }

        public bool ModifyOper()
        {
            bool newer = true;
            mPersonArchiveViewModel.Operation = "Modify";
            mPersonArchiveViewModel.PersonArchive = mPersonArchiveListViewModel.SelectedPersonArchive;
            mMainFrameViewModel.ContentView = mPersonArchiveViewModel.View;
            return newer;
        }

        public bool RetreatOper()
        {
            bool retreat = true;
            mPersonArchiveViewModel.Operation = "";
            try
            {
                mMainFrameViewModel.ContentView = mPersonArchiveListViewModel.View;
            }
            catch (System.Exception ex)
            {
                retreat = false;
                //errlog处理
                CurrentLoginService.Instance.LogException(ex);
            }

            return retreat;
        }

        public bool DeleteOper()
        {            
            bool deler = true;
            mPersonArchiveViewModel.Operation = "Delete";
            try
            {
                if (mMessageService.ShowYesNoQuestion(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "确定要删除吗？")))
                {
                    if (mPersonArchiveListViewModel.SelectedPersonArchive != null)
                    {
                        //if (!string.IsNullOrEmpty(mPersonArchiveListViewModel.SelectedPersonArchive.Photo))
                        //{
                        //    ftp.RemoveFile(mPersonArchiveListViewModel.SelectedPersonArchive.Photo);
                        //    ftp.RemoveFile(mPersonArchiveListViewModel.SelectedPersonArchive.PhotoThumb);
                        //}
                        mPersonArchiveListViewModel.SelectedPersonArchive.IsDeleted = true;
                        mPersonArchiveListViewModel.SelectedPersonArchive.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                        mPersonArchiveListViewModel.SelectedPersonArchive.UpdateTime = System.DateTime.Now;
                        //entityService.PersonArchives.Remove(personArchiveListViewModel.SelectedPersonArchive);
                        mEntityService.Entities.SaveChanges();
                        //刷新DataGrid
                        mPersonArchiveListViewModel.GridRefresh();

                        mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "删除成功."));
                    }
                    else
                    {
                        mMessageService.ShowError(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "未找到"));
                    }
                }
                else
                {
                    deler = false;
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
            mPersonArchiveViewModel.Operation = "Browse";
            mPersonArchiveViewModel.PersonArchive = mPersonArchiveListViewModel.SelectedPersonArchive;
            mMainFrameViewModel.ContentView = mPersonArchiveViewModel.View;

            return deal;
        }

        /*
        public bool Save()
        {
            bool saved = false;
            if (string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.Name))
            {
                mMessageService.ShowMessage("姓名是必填项");
                return false;
            }

            if (mPersonArchiveViewModel.PersonArchive.PoliceTypeId == null || mPersonArchiveViewModel.PersonArchive.PoliceTypeId == 0)
            {
                mMessageService.ShowMessage("类型是必填项");
                return false;
            }

            if (mPersonArchiveViewModel.PersonArchive.DepartmentId == null || mPersonArchiveViewModel.PersonArchive.DepartmentId == 0)
            {
                mMessageService.ShowMessage("科室是必填项");
                return false;
            }

            if (string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.PoliceNo))
            {
                mMessageService.ShowMessage("警号是必填项");
                return false;
            }

            if (string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.CardNo))
            {
                mMessageService.ShowMessage("身份证号是必填项");
                return false;
            }

            //不管是修改还是删除都要修改这些值
            if (mPersonArchiveViewModel.PersonArchive.PoliceTypeId != null)
            {
                if (mPersonArchiveViewModel.PersonArchive.PoliceTypeId == 1)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceType = "干警";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTypeId == 2)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceType = "协警";
                }
            }

            if (mPersonArchiveViewModel.PersonArchive.PoliticalId != null)
            {
                if (mPersonArchiveViewModel.PersonArchive.PoliticalId == 1)
                {
                    mPersonArchiveViewModel.PersonArchive.Political = "中共党员";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliticalId == 2)
                {
                    mPersonArchiveViewModel.PersonArchive.Political = "共青团员";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliticalId == 3)
                {
                    mPersonArchiveViewModel.PersonArchive.Political = "民主党派";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliticalId == 4)
                {
                    mPersonArchiveViewModel.PersonArchive.Political = "群众";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliticalId == 5)
                {
                    mPersonArchiveViewModel.PersonArchive.Political = "其它";
                }

            }

            if (mPersonArchiveViewModel.PersonArchive.EducationId != null)
            {
                if (mPersonArchiveViewModel.PersonArchive.EducationId == 1)
                {
                    mPersonArchiveViewModel.PersonArchive.Education = "博士";
                }
                else if (mPersonArchiveViewModel.PersonArchive.EducationId == 2)
                {
                    mPersonArchiveViewModel.PersonArchive.Education = "硕士";
                }
                else if (mPersonArchiveViewModel.PersonArchive.EducationId == 3)
                {
                    mPersonArchiveViewModel.PersonArchive.Education = "本科";
                }
                else if (mPersonArchiveViewModel.PersonArchive.EducationId == 4)
                {
                    mPersonArchiveViewModel.PersonArchive.Education = "大专";
                }
                else if (mPersonArchiveViewModel.PersonArchive.EducationId == 5)
                {
                    mPersonArchiveViewModel.PersonArchive.Education = "中专";
                }
                else if (mPersonArchiveViewModel.PersonArchive.EducationId == 6)
                {
                    mPersonArchiveViewModel.PersonArchive.Education = "高中";
                }
                else if (mPersonArchiveViewModel.PersonArchive.EducationId == 7)
                {
                    mPersonArchiveViewModel.PersonArchive.Education = "其它";
                }

            }

            if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId != null)
            {
                if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 1)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "总警监";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 2)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "副总警监";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 3)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "一级警监";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 4)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "二级警监";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 5)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "三级警监";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 6)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "一级警督";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 7)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "二级警督";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 8)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "三级警督";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 9)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "一级警司";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 10)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "二级警司";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 11)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "三级警司";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 12)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "一级警员";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 13)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "二级警员";
                }
                else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 14)
                {
                    mPersonArchiveViewModel.PersonArchive.PoliceTitle = "其它";
                }

            }

            try
            {
                //有可能修改处理科室
                mPersonArchiveViewModel.PersonArchive.DepartmentName = mPersonArchiveViewModel.DepartmentList.Find(instance => (instance.Id == mPersonArchiveViewModel.PersonArchive.DepartmentId)).FullName;
                mPersonArchiveViewModel.PersonArchive.DepartmentCode = mPersonArchiveViewModel.DepartmentList.Find(instance => (instance.Id == mPersonArchiveViewModel.PersonArchive.DepartmentId)).Code;


                if (mPersonArchiveViewModel.PersonArchive.Id > 0)
                {
                    mPersonArchiveViewModel.PersonArchive.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPersonArchiveViewModel.PersonArchive.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mPersonArchiveViewModel.PersonArchive.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges(); //update
                }
                else
                {
                    mPersonArchiveViewModel.PersonArchive.IsDeleted = false;
                    mPersonArchiveViewModel.PersonArchive.CreateUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPersonArchiveViewModel.PersonArchive.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mPersonArchiveViewModel.PersonArchive.CreateTime = System.DateTime.Now;

                    mPersonArchiveViewModel.PersonArchive.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPersonArchiveViewModel.PersonArchive.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mPersonArchiveViewModel.PersonArchive.UpdateTime = System.DateTime.Now;

                    mEntityService.Entities.PersonArchiveTables.AddObject(mPersonArchiveViewModel.PersonArchive);

                    mEntityService.Entities.SaveChanges(); //insert
                }
                saved = true;


                mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                //返回列表页
                mMainFrameViewModel.ContentView = mPersonArchiveListViewModel.View;

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
        */

        public bool Save()
        {
            bool saver = true;

            if (!ValueCheck())
            {
                return false;
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(workerSave);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerSaveCompleted);

            worker.RunWorkerAsync();


            return saver;
        }

        private void workerSave(object sender, DoWorkEventArgs e)
        {

            try
            {
                e.Result = false;
                //不管是修改还是删除都要修改这些值
                if (mPersonArchiveViewModel.PersonArchive.PoliceTypeId != null)
                {
                    if (mPersonArchiveViewModel.PersonArchive.PoliceTypeId == 1)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceType = "干警";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTypeId == 2)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceType = "协警";
                    }
                }

                if (mPersonArchiveViewModel.PersonArchive.PoliticalId != null)
                {
                    if (mPersonArchiveViewModel.PersonArchive.PoliticalId == 1)
                    {
                        mPersonArchiveViewModel.PersonArchive.Political = "中共党员";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliticalId == 2)
                    {
                        mPersonArchiveViewModel.PersonArchive.Political = "共青团员";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliticalId == 3)
                    {
                        mPersonArchiveViewModel.PersonArchive.Political = "民主党派";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliticalId == 4)
                    {
                        mPersonArchiveViewModel.PersonArchive.Political = "群众";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliticalId == 5)
                    {
                        mPersonArchiveViewModel.PersonArchive.Political = "其它";
                    }

                }

                if (mPersonArchiveViewModel.PersonArchive.EducationId != null)
                {
                    if (mPersonArchiveViewModel.PersonArchive.EducationId == 1)
                    {
                        mPersonArchiveViewModel.PersonArchive.Education = "博士";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.EducationId == 2)
                    {
                        mPersonArchiveViewModel.PersonArchive.Education = "硕士";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.EducationId == 3)
                    {
                        mPersonArchiveViewModel.PersonArchive.Education = "本科";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.EducationId == 4)
                    {
                        mPersonArchiveViewModel.PersonArchive.Education = "大专";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.EducationId == 5)
                    {
                        mPersonArchiveViewModel.PersonArchive.Education = "中专";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.EducationId == 6)
                    {
                        mPersonArchiveViewModel.PersonArchive.Education = "高中";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.EducationId == 7)
                    {
                        mPersonArchiveViewModel.PersonArchive.Education = "其它";
                    }

                }

                if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId != null)
                {
                    if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 1)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "总警监";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 2)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "副总警监";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 3)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "一级警监";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 4)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "二级警监";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 5)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "三级警监";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 6)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "一级警督";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 7)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "二级警督";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 8)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "三级警督";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 9)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "一级警司";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 10)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "二级警司";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 11)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "三级警司";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 12)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "一级警员";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 13)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "二级警员";
                    }
                    else if (mPersonArchiveViewModel.PersonArchive.PoliceTitleId == 14)
                    {
                        mPersonArchiveViewModel.PersonArchive.PoliceTitle = "其它";
                    }

                }

                //有可能修改处理科室
                //mPersonArchiveViewModel.PersonArchive.DepartmentName = mPersonArchiveViewModel.DepartmentList.Find(instance => (instance.Id == mPersonArchiveViewModel.PersonArchive.DepartmentId)).FullName;
                //mPersonArchiveViewModel.PersonArchive.DepartmentCode = mPersonArchiveViewModel.DepartmentList.Find(instance => (instance.Id == mPersonArchiveViewModel.PersonArchive.DepartmentId)).Code;

                if (mPersonArchiveViewModel.PersonArchive.Id > 0)
                {
                    mPersonArchiveViewModel.PersonArchive.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPersonArchiveViewModel.PersonArchive.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mPersonArchiveViewModel.PersonArchive.UpdateTime = System.DateTime.Now;
                    //if(!string.IsNullOrEmpty(mPersonArchiveViewModel.PhotoLocalPath))
                    //{
                    //    oldFile = mPersonArchiveViewModel.PersonArchive.Photo;
                    //    oldThumb = mPersonArchiveViewModel.PersonArchive.PhotoThumb;
                    //    mPersonArchiveViewModel.PersonArchive.PhotoThumb = ftp.UpdateFile(FtpFileType.Thumbnail, mPersonArchiveViewModel.PhotoLocalPath, oldThumb);
                    //    mPersonArchiveViewModel.PersonArchive.Photo = ftp.UpdateFile(FtpFileType.Picture, mPersonArchiveViewModel.PhotoLocalPath, oldFile);
                    //    if (string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.PhotoThumb)
                    //        || string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.Photo))
                    //    {
                    //        throw new ValidationException();
                    //    }
                    //}
                }
                else
                {
                    //if (!string.IsNullOrEmpty(mPersonArchiveViewModel.PhotoLocalPath))
                    //{
                    //    mPersonArchiveViewModel.PersonArchive.PhotoThumb = ftp.UploadFile(FtpFileType.Thumbnail, mPersonArchiveViewModel.PhotoLocalPath);
                    //    mPersonArchiveViewModel.PersonArchive.Photo = ftp.UploadFile(FtpFileType.Picture, mPersonArchiveViewModel.PhotoLocalPath);
                    //    if (string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.PhotoThumb)
                    //        || string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.Photo))
                    //    {
                    //        throw new ValidationException();
                    //    }
                    //}
                    mPersonArchiveViewModel.PersonArchive.IsDeleted = false;
                    mPersonArchiveViewModel.PersonArchive.CreateUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPersonArchiveViewModel.PersonArchive.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mPersonArchiveViewModel.PersonArchive.CreateTime = System.DateTime.Now;

                    mPersonArchiveViewModel.PersonArchive.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mPersonArchiveViewModel.PersonArchive.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
                    mPersonArchiveViewModel.PersonArchive.UpdateTime = System.DateTime.Now;

                    mEntityService.Entities.PersonArchiveTables.AddObject(mPersonArchiveViewModel.PersonArchive);
                }
                mEntityService.Entities.SaveChanges();
                e.Result = true;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void workerSaveCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mMainFrameViewModel.ContentView = mPersonArchiveListViewModel.View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValueCheck()
        {
            if (string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.Name))
            {
                mMessageService.ShowMessage("姓名是必填项");
                return false;
            }

            //if (string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.PoliceNo))
            //{
            //    mMessageService.ShowMessage("警号是必填项");
            //    return false;
            //}

            if (string.IsNullOrEmpty(mPersonArchiveViewModel.PersonArchive.CardNo))
            {
                mMessageService.ShowMessage("身份证号是必填项");
                return false;
            }

            return true;
        }

        private void PersonArchiveListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedTreeNode" || e.PropertyName == "SelectedPolicalTypeId")
            {
                string selectCode = string.Empty;
                if (mPersonArchiveListViewModel.SelectedTreeNode != null && mPersonArchiveListViewModel.SelectedTreeNode.Code != null)
                {
                    selectCode = mPersonArchiveListViewModel.SelectedTreeNode.Code;
                }

                try
                {
                    mPersonArchiveListViewModel.PersonArchives = mEntityService.EnumPersonArchives.Where<PersonArchiveTable>
                        (
                            entity =>
                                (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                                && ((!string.IsNullOrEmpty(selectCode)) ? (entity.DepartmentCode.IndexOf(selectCode) == 0) : true)
                                && ((mPersonArchiveListViewModel.SelectedPolicalTypeId != 0) ? (entity.PoliceTypeId == mPersonArchiveListViewModel.SelectedPolicalTypeId) : true)

                        );

                    //列表页
                    mPersonArchiveListViewModel.GridRefresh();
                }
                catch (System.Exception ex)
                {
                    //errlog处理
                    CurrentLoginService.Instance.LogException(ex);
                }
            }
        }

    }
}
    