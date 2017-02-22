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
using System.Data.Common;
using WafTraffic.Applications.Utils;
using DotNet.Utilities;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class LbCyDangerDealController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private LogbookMainViewModel mLogbookMainViewModel;
        private FrequentMainViewModel mFrequentMainViewModel;
        private LbCyDangerDealUpdateBeforeViewModel mDangerDealUpdateBeforeViewModel;
        private LbCyDangerDealUpdateAfterViewModel mDangerDealUpdateAfterViewModel;
        private LbCyDangerDealQueryViewModel mDangerDealQueryViewModel;
        private ShellViewModel mShellViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand dangerIdSearchCommand;
        private readonly DelegateCommand mContentDownloadCommand;

        private List<BaseOrganizeEntity> mDepartmentList = null;

        private string fileFrom;
        private string fileTo;

        [ImportingConstructor]
        public LbCyDangerDealController(CompositionContainer container, 
            System.Waf.Applications.Services.IMessageService messageService, 
            IShellService shellService, IEntityService entityService)
        {
            this.mContainer = container;
            this.mMessageService = messageService;
            this.mShellService = shellService;
            this.mEntityService = entityService;

            mLogbookMainViewModel = container.GetExportedValue<LogbookMainViewModel>();
            mFrequentMainViewModel = container.GetExportedValue<FrequentMainViewModel>();
            mDangerDealUpdateBeforeViewModel = container.GetExportedValue<LbCyDangerDealUpdateBeforeViewModel>();
            mDangerDealUpdateAfterViewModel = container.GetExportedValue<LbCyDangerDealUpdateAfterViewModel>();
            mDangerDealQueryViewModel = container.GetExportedValue<LbCyDangerDealQueryViewModel>();
            mShellViewModel = container.GetExportedValue<ShellViewModel>();

            this.mNewCommand = new DelegateCommand(() => NewOper(), null);
            this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
            this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
            this.mSaveCommand = new DelegateCommand(() => Save(), null);
            this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
            this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
            this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
            this.dangerIdSearchCommand = new DelegateCommand(() => DangerIdSearch());
            this.mContentDownloadCommand = new DelegateCommand(() => DownloadOper());
        }

        public void Initialize()
        {
            mDangerDealUpdateBeforeViewModel.SaveCommand = this.mSaveCommand;
            mDangerDealUpdateBeforeViewModel.CancelCommand = this.mCancelCommand;
            mDangerDealUpdateBeforeViewModel.ContentDownloadCommand = this.mContentDownloadCommand;
            mDangerDealUpdateBeforeViewModel.ContentLocalPath = string.Empty;
            mDangerDealUpdateBeforeViewModel.RectificationLocalPath = string.Empty;
            mDangerDealUpdateBeforeViewModel.ReviewLocalPath = string.Empty;
            mDangerDealUpdateBeforeViewModel.LeaderList = AuthService.Instance.SubLeaderGroup;
          
            mDangerDealUpdateAfterViewModel.SaveCommand = this.mSaveCommand;
            mDangerDealUpdateAfterViewModel.CancelCommand = this.mCancelCommand;
            mDangerDealUpdateAfterViewModel.ContentDownloadCommand = this.mContentDownloadCommand;
            mDangerDealUpdateAfterViewModel.DangerIdSearchCommand = this.dangerIdSearchCommand;
            mDangerDealUpdateAfterViewModel.ContentLocalPath = string.Empty;
            mDangerDealUpdateAfterViewModel.RectificationLocalPath = string.Empty;
            mDangerDealUpdateAfterViewModel.ReviewLocalPath = string.Empty;
            mDangerDealUpdateAfterViewModel.LeaderList = AuthService.Instance.SubLeaderGroup;
             

            mDangerDealQueryViewModel.NewCommand = this.mNewCommand;
            mDangerDealQueryViewModel.ModifyCommand = this.mModifyCommand;
            mDangerDealQueryViewModel.DeleteCommand = this.mDeleteCommand;
            mDangerDealQueryViewModel.QueryCommand = this.mQueryCommand;
            mDangerDealQueryViewModel.BrowseCommand = this.mBrowseCommand;
 
            RefreshQueryData();

            InitDepartmentList();
            mDangerDealUpdateBeforeViewModel.DepartmentList = this.mDepartmentList;
            mDangerDealUpdateAfterViewModel.DepartmentList = this.mDepartmentList;
        }

        private void InitDepartmentList()
        {
            try
            {
                mDepartmentList = new List<BaseOrganizeEntity>();

                BaseOrganizeManager origanizeService = new BaseOrganizeManager();
                DataTable departmentDT = origanizeService.GetDepartmentDT("");
                BaseOrganizeEntity entity;
                foreach (DataRow dr in departmentDT.Rows)
                {
                    entity = new BaseOrganizeEntity(dr);
                    mDepartmentList.Add(entity);
                }
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
                int type = YcConstants.INT_DANGER_DEAL_TYPE_BEFORE;
                if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                {
                    type = YcConstants.INT_DANGER_DEAL_TYPE_AFTER;
                }
                mDangerDealQueryViewModel.LbDangerDeals = this.mEntityService.EnumLbDangerDeals(type);
                mDangerDealQueryViewModel.GridRefresh();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool NewOper()
        {
            bool newer = true;

            if (mDangerDealQueryViewModel.IsAfterDangerDeal)
            {
                mDangerDealUpdateAfterViewModel.DangerDealEntity = new ZdtzCyDangerDeal();

                mDangerDealUpdateAfterViewModel.DangerDealEntity.DealDate = DateTime.Now;
                mDangerDealUpdateAfterViewModel.DangerDealEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mDangerDealUpdateAfterViewModel.DangerDealEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mDangerDealUpdateAfterViewModel.DangerDealEntity.OwnDepartmentId);
                mDangerDealUpdateAfterViewModel.DangerDealEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mDangerDealUpdateAfterViewModel.DangerDealEntity.CreateName = AuthService.Instance.GetUserNameById(mDangerDealUpdateAfterViewModel.DangerDealEntity.CreateId);
                
                mDangerDealUpdateAfterViewModel.IsBrowse = false;
                if (mDangerDealQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mDangerDealUpdateAfterViewModel.View;
                }else{
                    mFrequentMainViewModel.ContentView = mDangerDealUpdateAfterViewModel.View;
                }
            }
            else
            {
                mDangerDealUpdateBeforeViewModel.DangerDealEntity = new ZdtzCyDangerDeal();
                mDangerDealUpdateBeforeViewModel.DangerDealEntity.HappenDate = DateTime.Now;
                mDangerDealUpdateBeforeViewModel.DangerDealEntity.DealDate = DateTime.Now;
                mDangerDealUpdateBeforeViewModel.DangerDealEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                mDangerDealUpdateBeforeViewModel.DangerDealEntity.OwnDepartmentName = AuthService.Instance.GetDepartmentNameById(mDangerDealUpdateBeforeViewModel.DangerDealEntity.OwnDepartmentId);
                mDangerDealUpdateBeforeViewModel.DangerDealEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                mDangerDealUpdateBeforeViewModel.DangerDealEntity.CreateName = AuthService.Instance.GetUserNameById(mDangerDealUpdateBeforeViewModel.DangerDealEntity.CreateId);

                mDangerDealUpdateBeforeViewModel.DangerDealEntity.StrSpare1 = DateTime.Now.ToString("yyyyMMddHHmm"); //隐患编号

                BaseOrganizeEntity org = mDangerDealUpdateBeforeViewModel.DepartmentList.Find(
                    entity => (entity.Id == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId));

                if (org != null)
                {
                    mDangerDealUpdateBeforeViewModel.DangerDealEntity.ReportDepartment = org.FullName;
                }
                mDangerDealUpdateBeforeViewModel.IsBrowse = false;
                if (mDangerDealQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mDangerDealUpdateBeforeViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mDangerDealUpdateBeforeViewModel.View;
                }
            }

            return newer;
        }

        public bool ModifyOper()
        {
            bool newer = true;

            try
            {
                if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                {
                    mDangerDealUpdateAfterViewModel.DangerDealEntity = mDangerDealQueryViewModel.SelectedDangerDeal;
                    mDangerDealUpdateAfterViewModel.IsBrowse = false;
                    //int curUserDepart = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);

                    mDangerDealUpdateAfterViewModel.DangerDealEntity.SubLeaderComment = "同意";

                    if (mDangerDealQueryViewModel.IsFromAll)
                    {
                        mLogbookMainViewModel.ContentView = mDangerDealUpdateAfterViewModel.View;
                    }
                    else
                    {
                        mFrequentMainViewModel.ContentView = mDangerDealUpdateAfterViewModel.View;
                    }
                }
                else
                {
                    mDangerDealUpdateBeforeViewModel.DangerDealEntity = mDangerDealQueryViewModel.SelectedDangerDeal;
                    mDangerDealUpdateBeforeViewModel.IsBrowse = false;
                    //int curUserDepart = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);

                    mDangerDealUpdateBeforeViewModel.DangerDealEntity.SubLeaderComment = "同意";

                    if (mDangerDealQueryViewModel.IsFromAll)
                    {
                        mLogbookMainViewModel.ContentView = mDangerDealUpdateBeforeViewModel.View;
                    }
                    else
                    {
                        mFrequentMainViewModel.ContentView = mDangerDealUpdateBeforeViewModel.View;
                    }
                
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
            bool newer = true;
            FtpHelper ftp = null;
            try
            {
                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    ftp = new FtpHelper();
                    if (!ValidateUtil.IsBlank(mDangerDealQueryViewModel.SelectedDangerDeal.ContentImg))
                    {
                        ftp.RemoveFile(mDangerDealQueryViewModel.SelectedDangerDeal.ContentImg);
                        ftp.RemoveFile(mDangerDealQueryViewModel.SelectedDangerDeal.ContentThumb);
                    }
                    if (!ValidateUtil.IsBlank(mDangerDealQueryViewModel.SelectedDangerDeal.Rectification))
                    {
                        ftp.RemoveFile(mDangerDealQueryViewModel.SelectedDangerDeal.Rectification);
                        ftp.RemoveFile(mDangerDealQueryViewModel.SelectedDangerDeal.RectificationThumb);
                    }
                    if (!ValidateUtil.IsBlank(mDangerDealQueryViewModel.SelectedDangerDeal.ReviewImg))
                    {
                        ftp.RemoveFile(mDangerDealQueryViewModel.SelectedDangerDeal.ReviewImg);
                        ftp.RemoveFile(mDangerDealQueryViewModel.SelectedDangerDeal.ReviewThumb);
                    }
                    mDangerDealQueryViewModel.SelectedDangerDeal.IsDeleted = YcConstants.INT_DB_DATA_DELETED;
                    mDangerDealQueryViewModel.SelectedDangerDeal.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    mDangerDealQueryViewModel.SelectedDangerDeal.UpdateTime = System.DateTime.Now;
                    //mDangerDealUpdateViewModel.IsNew = false;
                    //mEntityService.LbDangerDeals.Remove(mDangerDealQueryViewModel.SelectedDangerDeal);
                    mEntityService.Entities.SaveChanges();
                    //刷新DataGrid
                    mDangerDealQueryViewModel.GridRefresh();

                    mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "删除成功."));
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

        public bool CancelOper()
        {
            bool newer = true;
            try
            {
                if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                {
                    if (mDangerDealUpdateAfterViewModel.DangerDealEntity != null)
                    {
                        if (mDangerDealUpdateAfterViewModel.DangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE)
                        {
                            //mDangerDealUpdateAfterViewModel.SubLeaderSignImg = null;
                        }
                    }
                }
                else
                {
                    if (mDangerDealUpdateBeforeViewModel.DangerDealEntity != null)
                    {
                        if (mDangerDealUpdateBeforeViewModel.DangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE)
                        {
                            //mDangerDealUpdateBeforeViewModel.SubLeaderSignImg = null;
                        }
                    }
                
                }
                if (mDangerDealQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mDangerDealQueryViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mDangerDealQueryViewModel.View;
                }
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

            if (mDangerDealQueryViewModel.IsAfterDangerDeal)
            {

                mDangerDealUpdateAfterViewModel.IsBrowse = true;
                mDangerDealUpdateAfterViewModel.DangerDealEntity = mDangerDealQueryViewModel.SelectedDangerDeal;
                if (mDangerDealQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mDangerDealUpdateAfterViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mDangerDealUpdateAfterViewModel.View;
                }
            }
            else
            {
                mDangerDealUpdateBeforeViewModel.IsBrowse = true;
                mDangerDealUpdateBeforeViewModel.DangerDealEntity = mDangerDealQueryViewModel.SelectedDangerDeal;
                if (mDangerDealQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mDangerDealUpdateBeforeViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mDangerDealUpdateBeforeViewModel.View;
                }
            
            
            }
            return newer;
        }

        public bool QueryOper()
        {
            bool newer = true;
            int deptId;
            string inputTitle;
            if (mDangerDealQueryViewModel.HaveListAllPermission)
            {
                deptId = Convert.ToInt32(mDangerDealQueryViewModel.SelectedDepartment);
            }
            else
            {
                deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
            }
            inputTitle = mDangerDealQueryViewModel.InputTitle;
            DateTime startDate = (DateTime)mDangerDealQueryViewModel.SelectedStartDate;
            DateTime endDate = (DateTime)mDangerDealQueryViewModel.SelectedEndDate;
            int type = YcConstants.INT_DANGER_DEAL_TYPE_BEFORE;
            if (mDangerDealQueryViewModel.IsAfterDangerDeal)
            {
                type = YcConstants.INT_DANGER_DEAL_TYPE_AFTER;
            }

            mDangerDealQueryViewModel.LbDangerDeals = mEntityService.QueryDangerDeals(deptId, startDate, endDate, type, inputTitle);

            if (mDangerDealQueryViewModel.LbDangerDeals == null || mDangerDealQueryViewModel.LbDangerDeals.Count() == 0)
            {
                MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            mDangerDealQueryViewModel.GridRefresh();

            return newer;
        }

         public bool Save()
        {
            bool saver = true;
            bool dealer = false;

            if (mDangerDealQueryViewModel.IsAfterDangerDeal)
            {
                IQueryable<ZdtzCyDangerDeal> tempDangerDeals =
                    this.mEntityService.EnumLbDangerDeals(YcConstants.INT_DANGER_DEAL_TYPE_BEFORE);

                foreach (ZdtzCyDangerDeal entry in tempDangerDeals)
                {
                    if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                    {
                        if (entry.StrSpare1 == mDangerDealUpdateAfterViewModel.DangerDealEntity.StrSpare1)
                        {
                            dealer = true;
                            break;
                        }
                    }
                    else
                    {
                        if (entry.StrSpare1 == mDangerDealUpdateBeforeViewModel.DangerDealEntity.StrSpare1)
                        {
                            dealer = true;
                            break;
                        }
                    }
                }

                if (!dealer)
                {
                    mMessageService.ShowMessage("该隐患编号不存在，请使用已存在的编号");
                    return saver;
                }
            }

            dealer = false;
            if (mDangerDealQueryViewModel.IsAfterDangerDeal)
            {
                if (mDangerDealUpdateAfterViewModel.DangerDealEntity.Id == 0)
                {
                    IQueryable<ZdtzCyDangerDeal> tempDangerDeals =
                            this.mEntityService.EnumLbDangerDeals(YcConstants.INT_DANGER_DEAL_TYPE_AFTER);

                    foreach (ZdtzCyDangerDeal entry in tempDangerDeals)
                    {
                        if (entry.StrSpare1 == mDangerDealUpdateAfterViewModel.DangerDealEntity.StrSpare1)
                        {
                            dealer = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (mDangerDealUpdateBeforeViewModel.DangerDealEntity.Id == 0)
                {
                    IQueryable<ZdtzCyDangerDeal> tempDangerDeals =
                        this.mEntityService.EnumLbDangerDeals(YcConstants.INT_DANGER_DEAL_TYPE_BEFORE);

                    foreach (ZdtzCyDangerDeal entry in tempDangerDeals)
                    {
                        if (entry.StrSpare1 == mDangerDealUpdateBeforeViewModel.DangerDealEntity.StrSpare1)
                        {
                            dealer = true;
                            break;
                        }
                    }
                }
            }

            if (dealer)
            {
                mMessageService.ShowMessage("该隐患编号已经存在");
                return saver;
            }


            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(workerSave);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerSaveCompleted);

            worker.RunWorkerAsync();

            if (mDangerDealQueryViewModel.IsAfterDangerDeal)
            {
                mDangerDealUpdateAfterViewModel.Show_LoadingMask(LoadingType.View);
            }
            else
            {
                mDangerDealUpdateBeforeViewModel.Show_LoadingMask(LoadingType.View);
            }

            return saver;
        }

        private void workerSave(object sender, DoWorkEventArgs e)
        {
            string oldFile;
            FtpHelper ftp = null;

            try
            {
                e.Result = false;
                ftp = new FtpHelper();

                #region after
                if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                {
                    if (mDangerDealUpdateAfterViewModel.DangerDealEntity.Id > 0)
                    {
                        mDangerDealUpdateAfterViewModel.DangerDealEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                        mDangerDealUpdateAfterViewModel.DangerDealEntity.UpdateTime = System.DateTime.Now;
                        if (!ValidateUtil.IsBlank(mDangerDealUpdateAfterViewModel.ContentLocalPath))
                        {
                            oldFile = mDangerDealUpdateAfterViewModel.DangerDealEntity.ContentImg;
                           
                            mDangerDealUpdateAfterViewModel.DangerDealEntity.ContentImg = ftp.UpdateFile(FtpFileType.Document, mDangerDealUpdateAfterViewModel.ContentLocalPath, oldFile);
                            if (ValidateUtil.IsBlank(mDangerDealUpdateAfterViewModel.DangerDealEntity.ContentImg))
                            {
                                throw new ValidationException();
                            }
                        }
                        if (mDangerDealUpdateAfterViewModel.DangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_NEW)
                        {
                            mDangerDealUpdateAfterViewModel.DangerDealEntity.Status = YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE;
                            mDangerDealUpdateAfterViewModel.DangerDealEntity.StatusName = YcConstants.STR_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE;
                        }
                        else if (mDangerDealUpdateAfterViewModel.DangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE)
                        {
                            mDangerDealUpdateAfterViewModel.DangerDealEntity.Status = YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY;
                            mDangerDealUpdateAfterViewModel.DangerDealEntity.StatusName = YcConstants.STR_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY;
                        }
                        else if (mDangerDealUpdateAfterViewModel.DangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY)
                        {
                            mDangerDealUpdateAfterViewModel.DangerDealEntity.Status = YcConstants.INT_DANGER_DEAL_SATUS_END;
                            mDangerDealUpdateAfterViewModel.DangerDealEntity.StatusName = YcConstants.STR_DANGER_DEAL_SATUS_END;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(mDangerDealUpdateAfterViewModel.DangerDealEntity.ReportDepartment))
                        {
                            mMessageService.ShowMessage("上报单位是必填项");
                            return;
                        }
                        if (string.IsNullOrEmpty(mDangerDealUpdateAfterViewModel.DangerDealEntity.Location))
                        {
                            mMessageService.ShowMessage("路段地点是必填项");
                            return;
                        }
                        if (!ValidateUtil.IsBlank(mDangerDealUpdateAfterViewModel.ContentLocalPath))
                        {
                            mDangerDealUpdateAfterViewModel.DangerDealEntity.ContentImg = ftp.UploadFile(FtpFileType.Document, mDangerDealUpdateAfterViewModel.ContentLocalPath);
                            if (ValidateUtil.IsBlank(mDangerDealUpdateAfterViewModel.DangerDealEntity.ContentImg))
                            {
                                throw new ValidationException();
                            }
                        }

                        mDangerDealUpdateAfterViewModel.DangerDealEntity.Status = YcConstants.INT_DANGER_DEAL_SATUS_NEW;
                        mDangerDealUpdateAfterViewModel.DangerDealEntity.StatusName = YcConstants.STR_DANGER_DEAL_SATUS_NEW;
                        int configId = 0;
                        if (mDangerDealQueryViewModel.IsFromAll)
                        {
                            configId = mLogbookMainViewModel.SelectedLogbook.Id;
                        }
                        else
                        {
                            configId = mFrequentMainViewModel.SelectedFrequent.Id;
                        }
                        mDangerDealUpdateAfterViewModel.DangerDealEntity.ConfigId = configId;
                        if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                        {
                            mDangerDealUpdateAfterViewModel.DangerDealEntity.LogbookType = YcConstants.INT_DANGER_DEAL_TYPE_AFTER;
                        }
                        mDangerDealUpdateAfterViewModel.DangerDealEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                        mDangerDealUpdateAfterViewModel.DangerDealEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                        mDangerDealUpdateAfterViewModel.DangerDealEntity.CreateTime = System.DateTime.Now;

                        mDangerDealUpdateAfterViewModel.DangerDealEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                        mDangerDealUpdateAfterViewModel.DangerDealEntity.UpdateTime = System.DateTime.Now;
                        mDangerDealUpdateAfterViewModel.DangerDealEntity.IsDeleted = YcConstants.INT_DB_DATA_AVAILABLE;

                        mEntityService.Entities.ZdtzCyDangerDeals.AddObject(mDangerDealUpdateAfterViewModel.DangerDealEntity);
                    }
                }
                #endregion
                else
                {
                    if (mDangerDealUpdateBeforeViewModel.DangerDealEntity.Id > 0)
                    {
                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.UpdateTime = System.DateTime.Now;
                        if (!ValidateUtil.IsBlank(mDangerDealUpdateBeforeViewModel.ContentLocalPath))
                        {
                            oldFile = mDangerDealUpdateBeforeViewModel.DangerDealEntity.ContentImg;
                           
                            mDangerDealUpdateBeforeViewModel.DangerDealEntity.ContentImg = ftp.UpdateFile(FtpFileType.Document, mDangerDealUpdateBeforeViewModel.ContentLocalPath, oldFile);
                            if (ValidateUtil.IsBlank(mDangerDealUpdateBeforeViewModel.DangerDealEntity.ContentImg))
                            {
                                throw new ValidationException();
                            }
                        }

                        if (mDangerDealUpdateBeforeViewModel.DangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_NEW)
                        {
                            mDangerDealUpdateBeforeViewModel.DangerDealEntity.Status = YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE;
                            mDangerDealUpdateBeforeViewModel.DangerDealEntity.StatusName = YcConstants.STR_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE;
                        }
                        else if (mDangerDealUpdateBeforeViewModel.DangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE)
                        {
                            mDangerDealUpdateBeforeViewModel.DangerDealEntity.Status = YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY;
                            mDangerDealUpdateBeforeViewModel.DangerDealEntity.StatusName = YcConstants.STR_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY;
                        }
                        else if (mDangerDealUpdateBeforeViewModel.DangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY)
                        {
                            mDangerDealUpdateBeforeViewModel.DangerDealEntity.Status = YcConstants.INT_DANGER_DEAL_SATUS_END;
                            mDangerDealUpdateBeforeViewModel.DangerDealEntity.StatusName = YcConstants.STR_DANGER_DEAL_SATUS_END;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(mDangerDealUpdateBeforeViewModel.DangerDealEntity.StrSpare1))
                        {
                            mMessageService.ShowMessage("隐患编号是必填项");
                            return;
                        }
                        if (string.IsNullOrEmpty(mDangerDealUpdateBeforeViewModel.DangerDealEntity.ReportDepartment))
                        {
                            mMessageService.ShowMessage("上报单位是必填项");
                            return;
                        }
                        if (string.IsNullOrEmpty(mDangerDealUpdateBeforeViewModel.DangerDealEntity.Location))
                        {
                            mMessageService.ShowMessage("路段地点是必填项");
                            return;
                        }
                        if (!ValidateUtil.IsBlank(mDangerDealUpdateBeforeViewModel.ContentLocalPath))
                        {
                            
                            mDangerDealUpdateBeforeViewModel.DangerDealEntity.ContentImg = ftp.UploadFile(FtpFileType.Document, mDangerDealUpdateBeforeViewModel.ContentLocalPath);
                            if (ValidateUtil.IsBlank(mDangerDealUpdateBeforeViewModel.DangerDealEntity.ContentImg))
                            {
                                throw new ValidationException();
                            }
                        }

                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.Status = YcConstants.INT_DANGER_DEAL_SATUS_NEW;
                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.StatusName = YcConstants.STR_DANGER_DEAL_SATUS_NEW;
                        int configId = 0;
                        if (mDangerDealQueryViewModel.IsFromAll)
                        {
                            configId = mLogbookMainViewModel.SelectedLogbook.Id;
                        }
                        else
                        {
                            configId = mFrequentMainViewModel.SelectedFrequent.Id;
                        }
                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.ConfigId = configId;
                        if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                        {
                            mDangerDealUpdateBeforeViewModel.DangerDealEntity.LogbookType = YcConstants.INT_DANGER_DEAL_TYPE_AFTER;
                        }
                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.CreateTime = System.DateTime.Now;

                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.UpdateTime = System.DateTime.Now;
                        mDangerDealUpdateBeforeViewModel.DangerDealEntity.IsDeleted = YcConstants.INT_DB_DATA_AVAILABLE;

                        mEntityService.Entities.ZdtzCyDangerDeals.AddObject(mDangerDealUpdateBeforeViewModel.DangerDealEntity);
                    }
                
                }
                mEntityService.Entities.SaveChanges();
                e.Result = true;
                /*
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                RefreshQueryData();
                mLogbookMainViewModel.ContentView = mStaticLogbookQueryViewModel.View;
                    */
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void workerSaveCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            if (mDangerDealQueryViewModel.IsAfterDangerDeal)
            {

                mDangerDealUpdateAfterViewModel.Shutdown_LoadingMask(LoadingType.View);
            }
            else
            {
                mDangerDealUpdateBeforeViewModel.Shutdown_LoadingMask(LoadingType.View);
            }

            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                if (mDangerDealQueryViewModel.IsFromAll)
                {
                    mLogbookMainViewModel.ContentView = mDangerDealQueryViewModel.View;
                }
                else
                {
                    mFrequentMainViewModel.ContentView = mDangerDealQueryViewModel.View;
                }
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public bool DangerIdSearch()
        {

            if (string.IsNullOrEmpty(mDangerDealUpdateAfterViewModel.DangerDealEntity.StrSpare1))
            {
                mMessageService.ShowMessage("隐患编号是必填项");
                return false;
            }

            IQueryable<ZdtzCyDangerDeal> tempDangerDeals =
                this.mEntityService.EnumLbDangerDeals(YcConstants.INT_DANGER_DEAL_TYPE_BEFORE);

            foreach (ZdtzCyDangerDeal entry in tempDangerDeals)
            {
                if (entry.StrSpare1 == mDangerDealUpdateAfterViewModel.DangerDealEntity.StrSpare1)
                {
                    mDangerDealUpdateAfterViewModel.DangerDealEntity.HappenDate = entry.HappenDate;
                    mDangerDealUpdateAfterViewModel.DangerDealEntity.ReportDepartment = entry.ReportDepartment;
                    mDangerDealUpdateAfterViewModel.DangerDealEntity.Location = entry.Location;
                    mDangerDealUpdateAfterViewModel.DangerDealEntity.DangerDescription = entry.DangerDescription;

                    return true;
                }
            }

            mMessageService.ShowMessage("该隐患编号不存在，请使用已存在的编号");
            return false;
        }


        public bool DownloadOper()
        {
            bool newer = true;
            try
            {
                SaveFileDialog sf = new SaveFileDialog();

                sf.AddExtension = true;
                sf.RestoreDirectory = true;

                if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                {
                    sf.FileName = mDangerDealUpdateAfterViewModel.DangerDealEntity.ContentImgName;
                }
                else
                {
                    sf.FileName = mDangerDealUpdateBeforeViewModel.DangerDealEntity.ContentImgName;
                }

                if (sf.ShowDialog() == DialogResult.OK)
                {
                    if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                    {
                        fileFrom = mDangerDealUpdateAfterViewModel.DangerDealEntity.ContentImg;
                    }
                    else
                    {
                        fileFrom = mDangerDealUpdateBeforeViewModel.DangerDealEntity.ContentImg;
                    }

                    fileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                    {
                        mDangerDealUpdateAfterViewModel.Show_LoadingMask(LoadingType.View);
                    }
                    else
                    {
                        mDangerDealUpdateBeforeViewModel.Show_LoadingMask(LoadingType.View);
                    }
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

                if (mDangerDealQueryViewModel.IsAfterDangerDeal)
                {
                    mDangerDealUpdateAfterViewModel.Shutdown_LoadingMask(LoadingType.View);
                }
                else
                {
                    mDangerDealUpdateBeforeViewModel.Shutdown_LoadingMask(LoadingType.View);
                }

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

        //public bool ShowSignImg()
        //{
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.DoWork += new DoWorkEventHandler(WorkerShowSignImg);
        //    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerShowSignImgCompleted);

        //    worker.RunWorkerAsync();

        //    mDangerDealUpdateViewModel.Show_LoadingMask(LoadingType.View);
        //    return true;
        //}

        //private void WorkerShowSignImg(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;

        //    try
        //    {
        //        e.Result = false;
        //        if (mDangerDealUpdateViewModel.DangerDealEntity.IsSubLeaderSigned)
        //        {
        //            ftp = new FtpHelper();
        //            if (mDangerDealUpdateViewModel.SubLeaderSignImg != null)
        //            {
        //                mDangerDealUpdateViewModel.SubLeaderSignImg.StreamSource.Dispose();
        //                mDangerDealUpdateViewModel.SubLeaderSignImg = null;
        //            }
        //            BaseUserEntity user = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo,
        //                mDangerDealUpdateViewModel.DangerDealEntity.SubLeaderId.ToString());
        //            mDangerDealUpdateViewModel.SubLeaderSignImg = ftp.DownloadFile(user.AnswerQuestion);
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

        //private void WorkerShowSignImgCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    bool ret = (bool)e.Result;

        //    mDangerDealUpdateViewModel.Shutdown_LoadingMask(LoadingType.View);
        //}

    }
}
    