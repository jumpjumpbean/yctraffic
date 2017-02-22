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
using WafTraffic.Applications.Utils;
using WafTraffic.Applications.Common;
using DotNet.Utilities;
using System.Windows.Forms;
using System.IO;
using System.Data.Entity;
using System.Transactions;
using System.Data.Common;
using System.Drawing;
using System.Data.SqlClient;
using WafTraffic.Domain.Common;
using System.Collections.ObjectModel;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class ZhzxTrafficViolationController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private BackgroundWorker importWorker;
        private BackgroundWorker exportWorker;

        private int filtedCnt = 0;
        private int cnt = 0;
        private int percentage;

        private bool isFakePlateRetreat = false;

        private MainFrameViewModel mainFrameViewModel;
        private ZhzxTrafficViolationViewModel zhzxTrafficViolationViewModel;
        private ZhzxTrafficViolationDetailsViewModel zhzxTrafficViolationDetailsViewModel;
        private ZhzxTrafficViolationGatherViewModel zhzxTrafficViolationGatherViewModel;
        private ZhzxFilterQueryViewModel zhzxFilterQueryViewModel;
        private ZhzxFakePlateNumberViewModel zhzxFakePlateNumberViewModel;
        private ShellViewModel shellViewModel;

        private ZhzxPicture picEntity;
        private ZhzxThumbnail thumEntity;

        private ZhzxTrafficViolation tmpZhzxTrafficViolation = new ZhzxTrafficViolation();

        private readonly DelegateCommand mShowThumbCommand;
        private readonly DelegateCommand queryCommand;
        private readonly DelegateCommand uploadQueryGatherCommand;
        private readonly DelegateCommand approveQueryGatherCommand;
        private readonly DelegateCommand browseCommand;
        private readonly DelegateCommand approveCommand;
        private readonly DelegateCommand batchApproveCommand;
        private readonly DelegateCommand retreatCommand;
        private readonly DelegateCommand submitCommand;
        private readonly DelegateCommand rejectCommand;
        private readonly DelegateCommand openImageCommand1;
        //private readonly DelegateCommand openImageCommand2;
        //private readonly DelegateCommand openImageCommand3;
        //private readonly DelegateCommand openImageCommand4;
        private readonly DelegateCommand importCommand;
        private readonly DelegateCommand exportCommand;
        private readonly DelegateCommand gatherCommand;
        private readonly DelegateCommand xxportCancelCommand;
        private readonly DelegateCommand retreatGatherCommand;
        private readonly DelegateCommand showViolationPicCommand;
        private readonly DelegateCommand addFakePlateCommand;
        private readonly DelegateCommand fakePlateBrowseCommand;
        private readonly DelegateCommand removeCommand;
        private readonly DelegateCommand fakePlateQueryCommand;


        [ImportingConstructor]
        public ZhzxTrafficViolationController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.container = container;
                this.messageService = messageService;
                this.shellService = shellService;
                this.entityService = entityService;

                mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                zhzxTrafficViolationViewModel = container.GetExportedValue<ZhzxTrafficViolationViewModel>();
                zhzxTrafficViolationDetailsViewModel = container.GetExportedValue<ZhzxTrafficViolationDetailsViewModel>();
                zhzxTrafficViolationGatherViewModel = container.GetExportedValue<ZhzxTrafficViolationGatherViewModel>();
                zhzxFilterQueryViewModel = container.GetExportedValue<ZhzxFilterQueryViewModel>();
                zhzxFakePlateNumberViewModel = container.GetExportedValue<ZhzxFakePlateNumberViewModel>();
                shellViewModel = container.GetExportedValue<ShellViewModel>();

                this.mShowThumbCommand = new DelegateCommand(() => ShowThumOper(), null);
                this.browseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.queryCommand = new DelegateCommand(() => QueryOper(), null);
                this.approveCommand = new DelegateCommand(() => ApproveOper(), null);
                this.batchApproveCommand = new DelegateCommand(() => BatchApproveOper(), null);
                this.retreatCommand = new DelegateCommand(() => RetreatOper(), null);
                this.submitCommand = new DelegateCommand(() => SubmitOper(), null);
                this.rejectCommand = new DelegateCommand(() => RejectOper(), null);
                this.importCommand = new DelegateCommand(() => ImportOper(), null);
                this.xxportCancelCommand = new DelegateCommand(() => XxportCancelOper(), null);
                this.exportCommand = new DelegateCommand(() => ExportOper(), null);
                this.gatherCommand = new DelegateCommand(() => GatherOper(), null);
                this.openImageCommand1 = new DelegateCommand(() => ShowPictureOper1(), null);
                //this.openImageCommand2 = new DelegateCommand(() => ShowPictureOper2(), null);
                //this.openImageCommand3 = new DelegateCommand(() => ShowPictureOper3(), null);
                //this.openImageCommand4 = new DelegateCommand(() => ShowPictureOper4(), null);
                this.uploadQueryGatherCommand = new DelegateCommand(() => UploadQueryGatherOper(), null);
                this.approveQueryGatherCommand = new DelegateCommand(() => ApproveQueryGatherOper(), null);
                this.retreatGatherCommand = new DelegateCommand(() => RetreatOper(), null);
                this.showViolationPicCommand = new DelegateCommand(() => ShowPicOper(), null);
                this.addFakePlateCommand = new DelegateCommand(() => AddFakePlateOper(), null);
                this.fakePlateBrowseCommand = new DelegateCommand(() => FakePlateBrowseOper(), null);
                this.removeCommand = new DelegateCommand(() => RemoveFakePlateOper(), null);
                this.fakePlateQueryCommand = new DelegateCommand(() => FakePlateQueryOper(), null);
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

                zhzxTrafficViolationViewModel.ImportCommand = this.importCommand;
                zhzxTrafficViolationViewModel.XxportCancelCommand = this.xxportCancelCommand;
                zhzxTrafficViolationViewModel.ExportCommand = this.exportCommand;
                zhzxTrafficViolationViewModel.GatherCommand = this.gatherCommand;
                zhzxTrafficViolationViewModel.BrowseCommand = this.browseCommand;
                zhzxTrafficViolationViewModel.QueryCommand = this.queryCommand;
                zhzxTrafficViolationViewModel.ApproveCommand = this.approveCommand;
                zhzxTrafficViolationViewModel.BatchApproveCommand = this.batchApproveCommand;

                zhzxTrafficViolationDetailsViewModel.OpenImageCommand1 = this.openImageCommand1;
                //zhzxTrafficViolationDetailsViewModel.OpenImageCommand2 = this.openImageCommand2;
                //zhzxTrafficViolationDetailsViewModel.OpenImageCommand3 = this.openImageCommand3;
                //zhzxTrafficViolationDetailsViewModel.OpenImageCommand4 = this.openImageCommand4;
                zhzxTrafficViolationDetailsViewModel.RetreatCommand = this.retreatCommand;
                zhzxTrafficViolationDetailsViewModel.SubmitCommand = this.submitCommand;
                zhzxTrafficViolationDetailsViewModel.RejectCommand = this.rejectCommand;
                zhzxTrafficViolationDetailsViewModel.ShowThumbCommand = this.mShowThumbCommand;
                zhzxTrafficViolationDetailsViewModel.AddFakePlateCommand = this.addFakePlateCommand;

                zhzxTrafficViolationGatherViewModel.UploadQueryGatherCommand = this.uploadQueryGatherCommand;
                zhzxTrafficViolationGatherViewModel.ApproveQueryGatherCommand = this.approveQueryGatherCommand;
                zhzxTrafficViolationGatherViewModel.RetreatCommand = this.retreatGatherCommand;

                zhzxFakePlateNumberViewModel.BrowseCommand = this.fakePlateBrowseCommand;
                zhzxFakePlateNumberViewModel.RemoveCommand = this.removeCommand;
                zhzxFakePlateNumberViewModel.QueryCommand = this.fakePlateQueryCommand;

                shellViewModel.ShowViolationPicCommand = this.showViolationPicCommand;

                if ((importWorker != null && importWorker.IsBusy) || (exportWorker != null && exportWorker.IsBusy))
                {
                    zhzxTrafficViolationViewModel.IsBusyIndicatorVisible = System.Windows.Visibility.Visible;
                }
                else
                {
                    zhzxTrafficViolationViewModel.IsBusyIndicatorVisible = System.Windows.Visibility.Collapsed;
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool ApproveOper()
        {
            bool dealer = true;
            isFakePlateRetreat = false;
            zhzxTrafficViolationDetailsViewModel.CanRejectShow = System.Windows.Visibility.Visible;
            zhzxTrafficViolationDetailsViewModel.CanRetreatShow = System.Windows.Visibility.Visible;
            zhzxTrafficViolationDetailsViewModel.CanSubmitShow = System.Windows.Visibility.Visible;
            zhzxTrafficViolationDetailsViewModel.CanAddFakePlatShow
                = zhzxTrafficViolationViewModel.SelectedTrafficViolation.IsFakeNumber ?
                    System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

            if (zhzxTrafficViolationViewModel.SelectedTrafficViolation.WorkflowStatusPhrase == YcConstantTable.STR_ZHZX_WORKFLOW_UPLOAD_PENDING)  //待上传
            {
                zhzxTrafficViolationDetailsViewModel.DetailsReadOnly = false;
                zhzxTrafficViolationDetailsViewModel.IsComboBoxEnabled = true;
            }
            else
            {
                zhzxTrafficViolationDetailsViewModel.DetailsReadOnly = true;
                zhzxTrafficViolationDetailsViewModel.IsComboBoxEnabled = false;
            }

            try
            {
                zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity = zhzxTrafficViolationViewModel.SelectedTrafficViolation;

                //zhzxTrafficViolationViewModel.SelectedTrafficViolation.DCopyTo(tmpZhzxTrafficViolation);

                zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity = entityService.QueryableZhzxThumbnail.Where(
                    p => (p.Code == zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ThumbnailCode)).FirstOrDefault();

                zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity = entityService.QueryableZhzxPicture.Where(
                    p => (p.Code == zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ThumbnailCode)).FirstOrDefault();

                mainFrameViewModel.ContentView = zhzxTrafficViolationDetailsViewModel.View;

            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }


            return dealer;
        }

        public bool BatchApproveOper()
        {
            bool dealer = true;

            zhzxTrafficViolationViewModel.RecordCount = zhzxTrafficViolationViewModel.TrafficViolation.Count<ZhzxTrafficViolation>();
            if (
                (zhzxTrafficViolationViewModel.TrafficViolation.Count<ZhzxTrafficViolation>(entity => entity.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_PENDING))  //待审核
                        != zhzxTrafficViolationViewModel.RecordCount
            )
            {
                messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                                "批量审核操作仅支持流程状态为待审核的记录，请重新确认所选记录的流程状态。"));
                return false;
            }

            try
            {
                foreach (ZhzxTrafficViolation entity in zhzxTrafficViolationViewModel.TrafficViolation)
                {
                    if (entity != null)
                    {
                        entity.ApprovalName = CurrentLoginService.Instance.CurrentUserInfo.UserName;
                        entity.ApprovalTime = System.DateTime.Now;
                        entity.WorkflowStatus = YcConstantTable.INT_ZHZX_WORKFLOW_APPROVED;  //置为审核通过状态
                    }
                }

                entityService.Entities.SaveChanges();
                MessageBox.Show(string.Format("{0}条记录审核成功。", zhzxTrafficViolationViewModel.RecordCount),
                            "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("批量审核失败。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CurrentLoginService.Instance.LogException(ex);
            }
            finally
            {
                zhzxTrafficViolationViewModel.GridRefresh();
            }


            return dealer;
        }

        public bool QueryOper()
        {
            bool deal = true;
            try
            {
                zhzxTrafficViolationViewModel.TrafficViolation = entityService.Entities.ZhzxTrafficViolations.Where<ZhzxTrafficViolation>
                    (
                        entity =>
                             (entity.CaptureTime.Value >= zhzxTrafficViolationViewModel.StartTime)
                                &&
                             (entity.CaptureTime.Value <= zhzxTrafficViolationViewModel.EndTime)
                                &&
                             ((zhzxTrafficViolationViewModel.SelectWorkflowStatusId == YcConstantTable.INT_ZHZX_WORKFLOW_ALL) ? true : (entity.WorkflowStatus == zhzxTrafficViolationViewModel.SelectWorkflowStatusId))
                                &&
                             ((zhzxTrafficViolationViewModel.SelectViolationTypePhrase == YcConstantTable.STR_ZHZX_VIOTYPE_ALL) ? true : (entity.ViolationType == zhzxTrafficViolationViewModel.SelectViolationTypePhrase))
                                &&
                             ((string.IsNullOrEmpty(zhzxTrafficViolationViewModel.Checkpoint)) ? true : (entity.CheckpointName.Contains(zhzxTrafficViolationViewModel.Checkpoint)))
                                &&
                             ((string.IsNullOrEmpty(zhzxTrafficViolationViewModel.LicensePlate)) ? true : (entity.LicensePlateNumber.Contains(zhzxTrafficViolationViewModel.LicensePlate)))
                    );

                //列表页
                zhzxTrafficViolationViewModel.GridRefresh();      
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool FakePlateQueryOper()
        {
            bool deal = true;
            try
            {
                zhzxFakePlateNumberViewModel.FakePlateNumber = entityService.Entities.ZhzxTrafficViolations.Where<ZhzxTrafficViolation>
                    (
                        entity =>
                             (entity.IsFakeNumber == true)
                                &&
                             (entity.CaptureTime.Value >= zhzxFakePlateNumberViewModel.StartTime)
                                &&
                             (entity.CaptureTime.Value <= zhzxFakePlateNumberViewModel.EndTime)
                                &&
                             ((zhzxFakePlateNumberViewModel.SelectWorkflowStatusId == YcConstantTable.INT_ZHZX_WORKFLOW_ALL) ? true : (entity.WorkflowStatus == zhzxFakePlateNumberViewModel.SelectWorkflowStatusId))
                                &&
                             ((string.IsNullOrEmpty(zhzxFakePlateNumberViewModel.Checkpoint)) ? true : (entity.CheckpointName.Contains(zhzxFakePlateNumberViewModel.Checkpoint)))
                                &&
                             ((string.IsNullOrEmpty(zhzxFakePlateNumberViewModel.LicensePlate)) ? true : (entity.LicensePlateNumber == zhzxFakePlateNumberViewModel.LicensePlate))
                    );

                //列表页
                zhzxFakePlateNumberViewModel.GridRefresh();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool BrowseOper()
        {
            bool deal = true;
            isFakePlateRetreat = false;
            zhzxTrafficViolationDetailsViewModel.CanRejectShow = System.Windows.Visibility.Collapsed;
            zhzxTrafficViolationDetailsViewModel.CanRetreatShow = System.Windows.Visibility.Visible;
            zhzxTrafficViolationDetailsViewModel.CanSubmitShow = System.Windows.Visibility.Collapsed;
            zhzxTrafficViolationDetailsViewModel.CanAddFakePlatShow 
                = zhzxTrafficViolationViewModel.SelectedTrafficViolation.IsFakeNumber?
                    System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

            zhzxTrafficViolationDetailsViewModel.DetailsReadOnly = true;
            zhzxTrafficViolationDetailsViewModel.IsComboBoxEnabled = false;

            try
            {
                zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity = zhzxTrafficViolationViewModel.SelectedTrafficViolation;

                zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity = entityService.QueryableZhzxThumbnail.Where(
                    p => (p.Code == zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ThumbnailCode)).FirstOrDefault();

                zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity = entityService.QueryableZhzxPicture.Where(
                    p => (p.Code == zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ThumbnailCode)).FirstOrDefault();

                mainFrameViewModel.ContentView = zhzxTrafficViolationDetailsViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool RetreatOper()
        {
            bool newer = true;

            if (isFakePlateRetreat)
            {
                isFakePlateRetreat = false;
                mainFrameViewModel.ContentView = zhzxFakePlateNumberViewModel.View;
            }
            else
            {
                mainFrameViewModel.ContentView = zhzxTrafficViolationViewModel.View;
            }

            return newer;
        }

        public void worker_DoImportWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (ValidateUtil.IsBlank(zhzxTrafficViolationViewModel.SourceDataPath))
                {
                    // MessageBox.Show("Excel路径不正确", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                IEnumerable<ZhzxTrafficViolationExt> entities = ExcelReader.Instance.ReadTrafficViolationExcel(zhzxTrafficViolationViewModel.SourceDataPath);

                zhzxTrafficViolationViewModel.RecordCount = entities.Count<ZhzxTrafficViolationExt>();
                zhzxTrafficViolationViewModel.XxportRecordCountPhrase = string.Format("共{0} 条记录", zhzxTrafficViolationViewModel.RecordCount);
                importWorker.ReportProgress(0, "正在连接数据库...");

                if (entities != null)
                {
                    //Server上打开MSDTC后仍然报错无法通信，放弃使用TransactionScope
                    //using (TransactionScope ts = new TransactionScope())

                    var trafficContext = new yctrafficEntities();

                    DbConnection connTraffic = trafficContext.Connection;
                    //DbConnection connPicture = pictureContext.Connection;
                    if (connTraffic.State != ConnectionState.Open) connTraffic.Open();
                    //if (connPicture.State != ConnectionState.Open) connPicture.Open();
                    using (DbTransaction tsTraffic = connTraffic.BeginTransaction())
                    {
                        {
                            picEntity = new ZhzxPicture();
                            thumEntity = new ZhzxThumbnail();

                            FtpHelper ftp = new FtpHelper();

                            ftp.ViolationImagePath = Guid.NewGuid().ToString();

                            try
                            {
                                foreach (ZhzxTrafficViolationExt extEntity in entities)
                                {
                                    ZhzxTrafficViolation entity = extEntity.ZhzxTrafficViolationEntity;
                                    if (entity != null)
                                    {
                                        if (importWorker.CancellationPending)
                                        {
                                            e.Cancel = true;
                                            ftp.RemoveDirectory(ftp.ViolationPicSubRoot);
                                            ftp.RemoveDirectory(ftp.ViolationThumbSubRoot);
                                            MessageBox.Show("取消成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }

                                        Guid guid = Guid.NewGuid();

                                        zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity = new ZhzxPicture();

                                        if (!ValidateUtil.IsBlank(extEntity.ComposedPicturePath))
                                        {
                                            zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.ComposedPicture = ftp.UploadFile(FtpFileType.ViolationPicture, extEntity.ComposedPicturePath);
                                        }
                                        //if (!ValidateUtil.IsBlank(extEntity.SourcePicturePath1))
                                        //{
                                        //    zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.SourcePicture1 = ftp.UploadFile(FtpFileType.ViolationPicture, extEntity.SourcePicturePath1);
                                        //}
                                        //if (!ValidateUtil.IsBlank(extEntity.SourcePicturePath2))
                                        //{
                                        //    zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.SourcePicture2 = ftp.UploadFile(FtpFileType.ViolationPicture, extEntity.SourcePicturePath2);
                                        //}
                                        //if (!ValidateUtil.IsBlank(extEntity.SourcePicturePath3))
                                        //{
                                        //    zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.SourcePicture3 = ftp.UploadFile(FtpFileType.ViolationPicture, extEntity.SourcePicturePath3);
                                        //}

                                        zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.Code = guid.ToString();

                                        zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                                        zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.CreateTime = System.DateTime.Now;
                                        zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                                        zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.UpdateTime = System.DateTime.Now;
                                        zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.IsDeleted = false;
                                        if (!importWorker.CancellationPending)
                                        {
                                            trafficContext.ZhzxPictures.AddObject(zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity);
                                        }
                                        else
                                        {
                                            e.Cancel = true;
                                            ftp.RemoveDirectory(ftp.ViolationPicSubRoot);
                                            ftp.RemoveDirectory(ftp.ViolationThumbSubRoot);
                                            MessageBox.Show("取消成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }

                                        zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity = new ZhzxThumbnail();

                                        if (!ValidateUtil.IsBlank(extEntity.ComposedPicturePath))
                                        {
                                            zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.ComposedThumbnail = ftp.UploadFile(FtpFileType.ViolationThumbnail, extEntity.ComposedPicturePath);
                                        }
                                        //if (!ValidateUtil.IsBlank(extEntity.SourcePicturePath1))
                                        //{
                                        //    zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.SourceThumbnail1 = ftp.UploadFile(FtpFileType.ViolationThumbnail, extEntity.SourcePicturePath1);
                                        //}
                                        //if (!ValidateUtil.IsBlank(extEntity.SourcePicturePath2))
                                        //{
                                        //    zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.SourceThumbnail2 = ftp.UploadFile(FtpFileType.ViolationThumbnail, extEntity.SourcePicturePath2);
                                        //}
                                        //if (!ValidateUtil.IsBlank(extEntity.SourcePicturePath3))
                                        //{
                                        //    zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.SourceThumbnail3 = ftp.UploadFile(FtpFileType.ViolationThumbnail, extEntity.SourcePicturePath3);
                                        //}

                                        zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.Code = guid.ToString();
                                        zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                                        zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.CreateTime = System.DateTime.Now;
                                        zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                                        zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.UpdateTime = System.DateTime.Now;
                                        zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.IsDeleted = false;
                                        if (!importWorker.CancellationPending)
                                        {
                                            trafficContext.ZhzxThumbnails.AddObject(zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity);
                                        }
                                        else
                                        {
                                            e.Cancel = true;
                                            ftp.RemoveDirectory(ftp.ViolationPicSubRoot);
                                            ftp.RemoveDirectory(ftp.ViolationThumbSubRoot);
                                            MessageBox.Show("取消成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }

                                        entity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                                        entity.CreateTime = System.DateTime.Now;
                                        entity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                                        entity.UpdateTime = System.DateTime.Now;
                                        entity.WorkflowStatus = YcConstants.INT_ZHZX_STATUS_WAITING_FOR_UPLOAD;
                                        entity.PictureCode = guid.ToString();
                                        entity.ThumbnailCode = guid.ToString();
                                        if (!importWorker.CancellationPending)
                                        {
                                            trafficContext.ZhzxTrafficViolations.AddObject(entity);
                                        }
                                        else
                                        {
                                            e.Cancel = true;
                                            ftp.RemoveDirectory(ftp.ViolationPicSubRoot);
                                            ftp.RemoveDirectory(ftp.ViolationThumbSubRoot);
                                            MessageBox.Show("取消成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }

                                        cnt += 1;
                                        percentage = (cnt * 97) / zhzxTrafficViolationViewModel.RecordCount;  //进度最高为97%
                                        importWorker.ReportProgress(percentage, string.Format("当前导入进度：{0} %", percentage));
                                    }
                                }
                                if (!importWorker.CancellationPending)
                                {
                                    trafficContext.SaveChanges();
                                    importWorker.ReportProgress(99, string.Format("当前导入进度：{0} %", 99));        // 至此进度置为99%

                                    tsTraffic.Commit();
                                    //tsPicture.Commit();
                                    importWorker.ReportProgress(100, string.Format("当前导入进度：{0} %", 100));       // 进度置为100%

                                    MessageBox.Show("批量导入成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    e.Cancel = true;
                                    ftp.RemoveDirectory(ftp.ViolationPicSubRoot);
                                    ftp.RemoveDirectory(ftp.ViolationThumbSubRoot);
                                    MessageBox.Show("取消成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }                
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show("发生错误，Excel已被打开或数据不正确，批量导入失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                tsTraffic.Rollback();
                                //tsPicture.Rollback();
                                CurrentLoginService.Instance.LogException(ex);
                            }
                            finally
                            {
                                if (e.Cancel)
                                {
                                    tsTraffic.Rollback();
                                }

                                if (tsTraffic != null) tsTraffic.Dispose();

                                if (connTraffic != null && connTraffic.State != ConnectionState.Closed)
                                {
                                    connTraffic.Close();
                                }

                                ftp.ViolationPicSubRoot = null;
                                ftp.ViolationThumbSubRoot = null;

                                if (ftp != null)
                                {
                                    ftp.Disconnect();
                                    ftp = null;
                                }

                                if (trafficContext != null) trafficContext.Dispose();
                                
                                cnt = 0;
                                percentage = 0;
                                zhzxTrafficViolationViewModel.RecordCount = 0;
                                zhzxTrafficViolationViewModel.ProgressBarValue = 0;
                                zhzxTrafficViolationViewModel.XxportProgress = null;
                                zhzxTrafficViolationViewModel.XxportRecordCountPhrase = null;
                                zhzxTrafficViolationViewModel.XxportTitle = null;
                                zhzxTrafficViolationViewModel.SourceDataPath = null;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("发生错误，Excel已被打开或数据不正确，批量导入失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        void worker_RunImportWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            zhzxTrafficViolationViewModel.IsBusyIndicatorVisible = System.Windows.Visibility.Collapsed;     

            zhzxTrafficViolationViewModel.GridRefresh();

            //int picCnt = entityService.GetPictureCount();
            //if (picCnt > YcConstants.INT_ZHZX_MAX_PICTURE_COUNT)              // 大于最大条数自动删除最早的图片
            //{
            //    this.CheckClearOldestPictures(picCnt - YcConstants.INT_ZHZX_MAX_PICTURE_COUNT);
            //}
        }

        void worker_XxportProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            zhzxTrafficViolationViewModel.ProgressBarValue = e.ProgressPercentage;
            zhzxTrafficViolationViewModel.XxportProgress = e.UserState.ToString();
        }
        
        public void ImportOper()
        {
            if (string.IsNullOrEmpty(zhzxTrafficViolationViewModel.SourceDataPath))
            {
                return;
            }

            importWorker = new BackgroundWorker();
            importWorker.WorkerReportsProgress = true;
            importWorker.WorkerReportsProgress = true;
            importWorker.WorkerSupportsCancellation = true;

            importWorker.DoWork += new DoWorkEventHandler(worker_DoImportWork);
            importWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunImportWorkerCompleted);
            importWorker.ProgressChanged += new ProgressChangedEventHandler(worker_XxportProgressChanged);

            zhzxTrafficViolationViewModel.ProgressCancelVisibility = System.Windows.Visibility.Visible;
            zhzxTrafficViolationViewModel.IsBusyIndicatorVisible = System.Windows.Visibility.Visible;
            zhzxTrafficViolationViewModel.XxportTitle = "正在导入违章记录...";

            importWorker.RunWorkerAsync();      
        }

        public void XxportCancelOper()
        {
            if (importWorker != null && importWorker.IsBusy)
            {
                importWorker.CancelAsync();
            }
            else if (exportWorker != null && exportWorker.IsBusy)
            {
                exportWorker.CancelAsync();
            }
        }

        //public void CheckClearOldestPictures(int cnt)
        //{
        //    try
        //    {
        //        entityService.RemoveOldestPicture(cnt);
        //        entityService.RemoveOldestThumbnail(cnt);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }
        //}

        public bool SubmitOper()
        {
            bool saved = false;

            try
            {

                if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.LicensePlateNumber))
                {
                    messageService.ShowMessage("车牌号码不能为空");
                    return false;
                }

                if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.OwnershipOfLand))
                {
                    messageService.ShowMessage("归属地不能为空");
                    return false;
                }

                if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.Speed))
                {
                    messageService.ShowMessage("车速不能为空");
                    return false;
                }

                if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.VehicleType))
                {
                    messageService.ShowMessage("车辆类型不能为空");
                    return false;
                }

                if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ViolationType))
                {
                    messageService.ShowMessage("违章类型不能为空");
                    return false;
                }

                if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.LicensePlateColor))
                {
                    messageService.ShowMessage("车牌颜色不能为空");
                    return false;
                }

                if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.VehicleColor))
                {
                    messageService.ShowMessage("车身颜色不能为空");
                    return false;
                }

                if (zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_PENDING)    // "待审核"
                {
                    DialogResult result = MessageBox.Show("审核一旦提交，结果将无法修改！\n\r\n\r确定提交吗？", 
                        "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                    if (result == DialogResult.No)
                    {
                        return false;
                    }
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ApprovalName = CurrentLoginService.Instance.CurrentUserInfo.UserName;
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ApprovalTime = System.DateTime.Now;
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.WorkflowStatus = YcConstantTable.INT_ZHZX_WORKFLOW_APPROVED;  // "审核通过"


                    string tmpCheckpointName = zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.CheckpointName;
                    string tmpLicensePlateNumber = zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.LicensePlateNumber;

                    // 如果同一车辆同一地点违章超过5次，在累计违章表中生成一条记录
                    int vioCnt = entityService.ViolationCountPerLocateAndPlate(tmpCheckpointName,tmpLicensePlateNumber);

                    if (vioCnt >= 4)            //目前该车在同一地点已有四次违章
                    {
                        if (
                            entityService.Entities.ZhzxTotalViolations.Where(entity =>
                                (entity.CheckpointName == tmpCheckpointName && entity.LicensePlateNumber == tmpLicensePlateNumber)).Count() == 0
                        )   // 如果累计违章表中不存在这条记录，创建一条
                        {
                            ZhzxTotalViolation tmpZhzxTotalViolation = new ZhzxTotalViolation();
                            tmpZhzxTotalViolation.CheckpointName = zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.CheckpointName;
                            tmpZhzxTotalViolation.LicensePlateNumber = zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.LicensePlateNumber;
                            tmpZhzxTotalViolation.ViolationCount = vioCnt + 1;
                            tmpZhzxTotalViolation.EarliestViolation = zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.CaptureTime;  //暂时没用
                            tmpZhzxTotalViolation.LatestViolation = zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.CaptureTime;

                            entityService.Entities.ZhzxTotalViolations.AddObject(tmpZhzxTotalViolation);
                        }
                        else
                        {
                            ZhzxTotalViolation tmpZhzxTotalViolation = entityService.Entities.ZhzxTotalViolations.Where(entity =>
                                (entity.CheckpointName == tmpCheckpointName && entity.LicensePlateNumber == tmpLicensePlateNumber)).FirstOrDefault<ZhzxTotalViolation>();

                            tmpZhzxTotalViolation.ViolationCount += 1;
                            tmpZhzxTotalViolation.LatestViolation = zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.CaptureTime;
                        }
                    }
                }
                else if (zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_UPLOAD_PENDING) // "待上传"
                {
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.UploadName = CurrentLoginService.Instance.CurrentUserInfo.UserName;
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.UploadTime = System.DateTime.Now;
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.WorkflowStatus = YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_PENDING; // "待审核"
                }

                zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.UpdateTime = System.DateTime.Now;


                entityService.Entities.SaveChanges();

                saved = true;


                messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "操作成功.\n转入下一条..."));
                //加载下一条记录
                try
                {
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity =
                        zhzxTrafficViolationViewModel.TrafficViolation.OrderBy(p => p.Id).First(p => (p.Id > zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.Id));
                }
                catch (System.InvalidOperationException)
                {
                    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "已是最后一条..."));
                }

                zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity = entityService.QueryableZhzxThumbnail.Where(
                    p => (p.Code == zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ThumbnailCode)).FirstOrDefault();

                zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity = entityService.QueryableZhzxPicture.Where(
                    p => (p.Code == zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ThumbnailCode)).FirstOrDefault();

                ShowThumOper();

                mainFrameViewModel.ContentView = zhzxTrafficViolationDetailsViewModel.View;
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

            return saved;
        }

        void worker_RunExportWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            zhzxTrafficViolationViewModel.IsBusyIndicatorVisible = System.Windows.Visibility.Collapsed;

            zhzxTrafficViolationViewModel.GridRefresh();
        }

        public void worker_DoExportWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (zhzxTrafficViolationViewModel.TrafficViolation != null)
                {
                    filtedCnt = 0;
                    //zhzxTrafficViolationViewModel.XxportRecordCount = zhzxTrafficViolationViewModel.TrafficViolation.Count<ZhzxTrafficViolation>();
                    zhzxTrafficViolationViewModel.XxportRecordCountPhrase = string.Format("共{0} 条记录", zhzxTrafficViolationViewModel.RecordCount);
                    exportWorker.ReportProgress(0, "正在连接数据库...");

                    if (!Directory.Exists(zhzxTrafficViolationViewModel.TargetFolderPath))
                    {
                        Directory.CreateDirectory(zhzxTrafficViolationViewModel.TargetFolderPath);
                    }

                    foreach (ZhzxTrafficViolation entity in zhzxTrafficViolationViewModel.TrafficViolation)
                    {
                        if (entity != null)
                        {
                            cnt += 1;
                            
                            foreach(ZhzxRedNameList redName in zhzxFilterQueryViewModel.Filters)
                            {
                                if (entity.LicensePlateNumber == redName.LicensePlateNumber)
                                {
                                    filtedCnt += 1;

                                    entity.WorkflowStatus = YcConstantTable.INT_ZHZX_WORKFLOW_FILTERED;  //状态置为被过滤

                                    goto Label_1;
                                }
                            }

                            string ownSuffix = cnt.ToString().PadLeft(6, '0') + "-" + entity.LicensePlateNumber;

                            string fileName = zhzxTrafficViolationViewModel.TargetFolderPath + "\\" + ownSuffix;

                            if (Directory.Exists(zhzxTrafficViolationViewModel.TargetFolderPath))
                            {
                                try
                                {
                                    FtpHelper ftp = new FtpHelper();

                                    string fileFrom = entityService.QueryableZhzxPicture.Where(
                                        p => (p.Code == entity.PictureCode)).FirstOrDefault().ComposedPicture;
                                    string fileTo = fileName + ".jpg";

                                    ftp.DownloadFile(fileFrom, fileTo);
                                }
                                catch (System.Exception ex)
                                {
                                    CurrentLoginService.Instance.LogException(ex);
                                }
                            }

                            string content = GetTextContent(entity);

                            try
                            {
                                File.WriteAllText(fileName + ".txt", content, System.Text.Encoding.UTF8);
                            }
                            catch (System.Exception ex)
                            {
                                CurrentLoginService.Instance.LogException(ex);
                            }

                        Label_1:

                            if (exportWorker.CancellationPending)
                            {
                                e.Cancel = true;

                                MessageBox.Show(string.Format("取消成功。{0}条已导出，其中{1}条被过滤。", cnt, filtedCnt), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            percentage = (cnt * 100) / zhzxTrafficViolationViewModel.RecordCount;
                            exportWorker.ReportProgress(percentage, string.Format("当前导出进度：{0} %", percentage));
                        }
                    }

                    entityService.Entities.SaveChanges();

                    MessageBox.Show(string.Format("批量导出成功。共{0}条记录，其中{1}条被过滤。", cnt, filtedCnt), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("发生错误，批量导出失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }
            finally
            {
                if (e.Cancel)
                {
                    entityService.Entities.SaveChanges();
                }

                filtedCnt = 0;
                cnt = 0;
                percentage = 0;
                zhzxTrafficViolationViewModel.RecordCount = 0;
                zhzxTrafficViolationViewModel.ProgressBarValue = 0;
                zhzxTrafficViolationViewModel.XxportProgress = null;
                zhzxTrafficViolationViewModel.XxportRecordCountPhrase = null;
                zhzxTrafficViolationViewModel.XxportTitle = null;
                zhzxTrafficViolationViewModel.TargetFolderPath = null;
            }
        }

        string GetTextContent(ZhzxTrafficViolation entity)
        {
            string content = "卡口名称:\r\n";
            content += entity.CheckpointName + "\r\n";
            content += "*****************\r\n";
            content += "抓拍地点:\r\n";
            content += entity.CaptureLocation + "\r\n";
            content += "*****************\r\n";
            content += "车牌号码:\r\n";
            content +=  entity.LicensePlateNumber + "\r\n";
            content += "*****************\r\n";
            content += "归属地:\r\n";
            content += entity.OwnershipOfLand + "\r\n";
            content += "*****************\r\n";
            content += "车速:\r\n";
            content += entity.Speed + "\r\n";
            content += "*****************\r\n";
            content += "违章类型:\r\n";
            content += entity.ViolationType + "\r\n";
            content += "*****************\r\n";
            content += "车辆类型:\r\n";
            content += entity.VehicleType + "\r\n";
            content += "*****************\r\n";
            content += "车牌颜色:\r\n";
            content += entity.LicensePlateColor + "\r\n";
            content += "*****************\r\n";
            content += "车身颜色:\r\n";
            content += entity.VehicleColor + "\r\n";
            content += "*****************\r\n";
            content += "抓拍时间:\r\n";
            content += string.Format("{0:yyyy-MM-dd HH:mm:ss}", entity.CaptureTime) + "\r\n";
            content += "*****************\r\n";
            content += "状态:\r\n";
            content += entity.DataStatus + "\r\n";
            content += "*****************\r\n";

            return content;
        }

        public void ExportOper()
        {
            if (string.IsNullOrEmpty(zhzxTrafficViolationViewModel.TargetFolderPath))
            {
                return;
            }

            zhzxTrafficViolationViewModel.RecordCount = zhzxTrafficViolationViewModel.TrafficViolation.Count<ZhzxTrafficViolation>();
            if (
                (zhzxTrafficViolationViewModel.TrafficViolation.Count<ZhzxTrafficViolation>(entity => entity.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_PENDING))
                        != zhzxTrafficViolationViewModel.RecordCount
            )
            {
                messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, 
                                "批量导出操作仅支持流程状态为待审核的记录，请重新确认所选记录的流程状态。"));
                return;
            }

            //DialogResult result = MessageBox.Show("导出过程中可能会修改流程状态，取消操作也无法恢复！\n\r\n\r确定导出吗？",
            //                    "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

            //if (result == DialogResult.No)
            //{
            //    return;
            //}

            exportWorker = new BackgroundWorker();
            exportWorker.WorkerReportsProgress = true;
            exportWorker.WorkerReportsProgress = true;
            exportWorker.WorkerSupportsCancellation = true;

            exportWorker.DoWork += new DoWorkEventHandler(worker_DoExportWork);
            exportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunExportWorkerCompleted);
            exportWorker.ProgressChanged += new ProgressChangedEventHandler(worker_XxportProgressChanged);

            zhzxTrafficViolationViewModel.ProgressCancelVisibility = System.Windows.Visibility.Visible;
            zhzxTrafficViolationViewModel.IsBusyIndicatorVisible = System.Windows.Visibility.Visible;
            zhzxTrafficViolationViewModel.XxportTitle = "正在导出违章记录...";

            exportWorker.RunWorkerAsync();
        }

        public bool GatherOper()
        { 
            bool deal = true;
            isFakePlateRetreat = false;

            mainFrameViewModel.ContentView = zhzxTrafficViolationGatherViewModel.View;

            return deal;
        }

        //public bool RetreatGatherOper()
        //{
        //    bool newer = true;

        //    mainFrameViewModel.ContentView = zhzxTrafficViolationViewModel.View;

        //    return newer;
        //}

        public bool UploadQueryGatherOper()
        {
            bool deal = true;
            try
            {
                zhzxTrafficViolationGatherViewModel.ViolationUploadGatherList =
                    entityService.GetViolationUploadGatherByCaptureTime(zhzxTrafficViolationGatherViewModel.UploadStartDate, zhzxTrafficViolationGatherViewModel.UploadEndDate);

                this.GenViolationUploadGatherElement();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            zhzxTrafficViolationGatherViewModel.UploadReprotRefresh();

            return deal;
        }

        public bool ApproveQueryGatherOper()
        {
            bool deal = true;
            try
            {
                zhzxTrafficViolationGatherViewModel.ViolationApproveGatherList =
                    entityService.GetViolationApprovalGatherByCaptureTime(zhzxTrafficViolationGatherViewModel.ApproveStartDate, zhzxTrafficViolationGatherViewModel.ApproveEndDate);

                this.GenViolationApproveGatherElement();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            zhzxTrafficViolationGatherViewModel.ApproveReprotRefresh();

            return deal;
        }

        public bool RejectOper()
        {
            bool deal = false;

            if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.LicensePlateNumber))
            {
                messageService.ShowMessage("车牌号码不能为空");
                return false;
            }

            if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.OwnershipOfLand))
            {
                messageService.ShowMessage("归属地不能为空");
                return false;
            }

            if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.Speed))
            {
                messageService.ShowMessage("车速不能为空");
                return false;
            }

            if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.VehicleType))
            {
                messageService.ShowMessage("车辆类型不能为空");
                return false;
            }

            if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ViolationType))
            {
                messageService.ShowMessage("违章类型不能为空");
                return false;
            }

            if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.LicensePlateColor))
            {
                messageService.ShowMessage("车牌颜色不能为空");
                return false;
            }

            if (string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.VehicleColor))
            {
                messageService.ShowMessage("车身颜色不能为空");
                return false;
            }

            try
            {
                DialogResult result = MessageBox.Show("审核一旦打回，结果将无法修改！\n\r\n\r确定打回吗？", 
                    "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                if (result == DialogResult.No)
                {
                    return false;
                }
                if (zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_UPLOAD_PENDING) // "待上传"
                {
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.UploadName = CurrentLoginService.Instance.CurrentUserInfo.UserName;
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.UploadTime = System.DateTime.Now;
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.WorkflowStatus = YcConstantTable.INT_ZHZX_WORKFLOW_UPLOAD_REJECT;  // "上传打回"
                }
                else if (zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_PENDING) // "待审核"
                {
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ApprovalName = CurrentLoginService.Instance.CurrentUserInfo.UserName;
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ApprovalTime = System.DateTime.Now;
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.WorkflowStatus = YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_REJECT; // "审核打回"
                }

                zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.UpdateTime = System.DateTime.Now;

                entityService.Entities.SaveChanges();

                deal = true;

                messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "操作成功.\n转入下一条..."));

                try
                {
                    zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity =
                        zhzxTrafficViolationViewModel.TrafficViolation.OrderBy(p => p.Id).First(p => (p.Id > zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.Id));
                }
                catch (System.InvalidOperationException)
                {
                    messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "已是最后一条..."));
                }

                //zhzxTrafficViolationViewModel.SelectedTrafficViolation.DCopyTo(tmpZhzxTrafficViolation);

                zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity = entityService.QueryableZhzxThumbnail.Where(
                    p => (p.Code == zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ThumbnailCode)).FirstOrDefault();

                zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity = entityService.QueryableZhzxPicture.Where(
                    p => (p.Code == zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.ThumbnailCode)).FirstOrDefault();

                ShowThumOper();

                mainFrameViewModel.ContentView = zhzxTrafficViolationDetailsViewModel.View;
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

            return deal;
        }

        #region ShowThum
        

        public bool ShowThumOper()
        {
            bool result = false;

            try
            {
                //ClearThumb();

                if (!string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.ComposedThumbnail))
                {
                    this.ShowComposedThum();
                }
                else
                {
                    zhzxTrafficViolationDetailsViewModel.ComposedThumbnailImg = null;
                }
                //if (!string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.SourceThumbnail1))
                //{
                //    this.ShowThum(1);
                //}
                //if (!string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.SourceThumbnail2))
                //{
                //    this.ShowThum(2);
                //}
                //if (!string.IsNullOrEmpty(zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.SourceThumbnail3))
                //{
                //    this.ShowThum(3);
                //}
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }

        //private void ClearThumb()
        //{
        //    if (mDangerDealUpdateViewModel.ConetentImg != null)
        //    {
        //        mDangerDealUpdateViewModel.ConetentImg.StreamSource.Dispose();
        //        mDangerDealUpdateViewModel.ConetentImg = null;
        //    }
        //    if (mDangerDealUpdateViewModel.RectificationImg != null)
        //    {
        //        mDangerDealUpdateViewModel.RectificationImg.StreamSource.Dispose();
        //        mDangerDealUpdateViewModel.RectificationImg = null;
        //    }
        //    if (mDangerDealUpdateViewModel.ReviewImg != null)
        //    {
        //        mDangerDealUpdateViewModel.ReviewImg.StreamSource.Dispose();
        //        mDangerDealUpdateViewModel.ReviewImg = null;
        //    }
        //}

        private void ShowComposedThum()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(workerShowComposedThum);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerShowComposedThumCompleted);

            worker.RunWorkerAsync();

            zhzxTrafficViolationDetailsViewModel.Show_LoadingMask();
        }

        private void workerShowComposedThum(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;   
            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                zhzxTrafficViolationDetailsViewModel.ComposedThumbnailImg = ftp.DownloadFile(zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.ComposedThumbnail);
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

        private void workerShowComposedThumCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool ret = (bool)e.Result;

            zhzxTrafficViolationDetailsViewModel.Shutdown_LoadingMask();
        }

        //private void ShowThum(int picId)
        //{
        //    BackgroundWorker worker = new BackgroundWorker();

        //    if (picId == 1)
        //    {
        //        worker.DoWork += new DoWorkEventHandler(workerShowThum1);
        //    }
        //    if (picId == 2)
        //    {
        //        worker.DoWork += new DoWorkEventHandler(workerShowThum2);
        //    }
        //    if (picId == 3)
        //    {
        //        worker.DoWork += new DoWorkEventHandler(workerShowThum3);
        //    }

        //    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerShowComposedThumCompleted);

        //    worker.RunWorkerAsync();

        //    zhzxTrafficViolationDetailsViewModel.Show_LoadingMask();
        //}

        //private void workerShowThum1(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        zhzxTrafficViolationDetailsViewModel.Thumbnail1Img = ftp.DownloadFile(zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.SourceThumbnail1);
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

        //private void workerShowThum2(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        zhzxTrafficViolationDetailsViewModel.Thumbnail2Img = ftp.DownloadFile(zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.SourceThumbnail2);
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

        //private void workerShowThum3(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        zhzxTrafficViolationDetailsViewModel.Thumbnail3Img = ftp.DownloadFile(zhzxTrafficViolationDetailsViewModel.ZhzxThumbnailEntity.SourceThumbnail3);
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

        #endregion


        #region ShowPicture


        public bool ShowPicOper()
        {
            bool result = false;

            try
            {
                if (shellViewModel.CurrentPicId == 1)
                {
                    this.ShowPictureOper1();
                }
                //if (shellViewModel.CurrentPicId == 2)
                //{
                //    this.ShowPictureOper2();
                //}
                //if (shellViewModel.CurrentPicId == 3)
                //{
                //    this.ShowPictureOper3();
                //}
                //if (shellViewModel.CurrentPicId == 4)
                //{
                //    this.ShowPictureOper4();
                //}

            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }


        public bool ShowPictureOper1()
        {
            bool result = false;

            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_ShowPicture1);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted1);

                worker.RunWorkerAsync();

                zhzxTrafficViolationDetailsViewModel.Show_LoadingMask();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }

        void worker_ShowPicture1(object sender, DoWorkEventArgs e)
        {
            FtpHelper ftp = null;
            try
            {
                e.Result = false;
                ftp = new FtpHelper();
                if (shellViewModel.SourceImage != null)
                {
                    shellViewModel.SourceImage.StreamSource.Dispose();
                    shellViewModel.SourceImage = null;
                }

                shellViewModel.SourceImage = ftp.DownloadFile(this.zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.ComposedPicture);
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

        void worker_RunWorkerCompleted1(object sender, RunWorkerCompletedEventArgs e)
        {
            shellViewModel.ShowMyImage(1);

            zhzxTrafficViolationDetailsViewModel.Shutdown_LoadingMask();
        }


        //public bool ShowPictureOper2()
        //{
        //    bool result = false;

        //    try
        //    {
        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.DoWork += new DoWorkEventHandler(worker_ShowPicture2);
        //        worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted2);

        //        worker.RunWorkerAsync();

        //        zhzxTrafficViolationDetailsViewModel.Show_LoadingMask();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return result;
        //}

        //void worker_ShowPicture2(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        if (shellViewModel.SourceImage != null)
        //        {
        //            shellViewModel.SourceImage.StreamSource.Dispose();
        //            shellViewModel.SourceImage = null;
        //        }

        //        shellViewModel.SourceImage = ftp.DownloadFile(this.zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.SourcePicture1);
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

        //void worker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    shellViewModel.ShowMyImage(2);

        //    zhzxTrafficViolationDetailsViewModel.Shutdown_LoadingMask();
        //}

        //public bool ShowPictureOper3()
        //{
        //    bool result = false;

        //    try
        //    {
        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.DoWork += new DoWorkEventHandler(worker_ShowPicture3);
        //        worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted3);

        //        worker.RunWorkerAsync();

        //        zhzxTrafficViolationDetailsViewModel.Show_LoadingMask();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return result;
        //}

        //void worker_ShowPicture3(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        if (shellViewModel.SourceImage != null)
        //        {
        //            shellViewModel.SourceImage.StreamSource.Dispose();
        //            shellViewModel.SourceImage = null;
        //        }

        //        shellViewModel.SourceImage = ftp.DownloadFile(this.zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.SourcePicture2);
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

        //void worker_RunWorkerCompleted3(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    shellViewModel.ShowMyImage(3);

        //    zhzxTrafficViolationDetailsViewModel.Shutdown_LoadingMask();
        //}

        //public bool ShowPictureOper4()
        //{
        //    bool result = false;

        //    try
        //    {
        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.DoWork += new DoWorkEventHandler(worker_ShowPicture4);
        //        worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted4);

        //        worker.RunWorkerAsync();

        //        zhzxTrafficViolationDetailsViewModel.Show_LoadingMask();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }

        //    return result;
        //}

        //void worker_ShowPicture4(object sender, DoWorkEventArgs e)
        //{
        //    FtpHelper ftp = null;
        //    try
        //    {
        //        e.Result = false;
        //        ftp = new FtpHelper();
        //        if (shellViewModel.SourceImage != null)
        //        {
        //            shellViewModel.SourceImage.StreamSource.Dispose();
        //            shellViewModel.SourceImage = null;
        //        }

        //        shellViewModel.SourceImage = ftp.DownloadFile(this.zhzxTrafficViolationDetailsViewModel.ZhzxPictureEntity.SourcePicture3);
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

        //void worker_RunWorkerCompleted4(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    shellViewModel.ShowMyImage(4);

        //    zhzxTrafficViolationDetailsViewModel.Shutdown_LoadingMask();
        //}

        #endregion

        public void GenViolationUploadGatherElement()
        {
            if (zhzxTrafficViolationGatherViewModel.ViolationUploadGatherList.Count() == 0)
            {
                zhzxTrafficViolationGatherViewModel.ViolationUploadGatherDataSource = new ObservableCollection<ZhzxViolationGatherElementTable>();
            }
            else if (zhzxTrafficViolationGatherViewModel.ViolationUploadGatherList.Count() != 0)
            {
                zhzxTrafficViolationGatherViewModel.ViolationUploadGatherDataSource = new ObservableCollection<ZhzxViolationGatherElementTable>();
                string name = zhzxTrafficViolationGatherViewModel.ViolationUploadGatherList.First().Name;
                ZhzxViolationGatherElementTable element = new ZhzxViolationGatherElementTable();

                foreach (ZhzxViolationGatherTable temp in zhzxTrafficViolationGatherViewModel.ViolationUploadGatherList)
                {
                    if (temp.Name != name)
                    {
                        name = temp.Name;

                        zhzxTrafficViolationGatherViewModel.ViolationUploadGatherDataSource.Add(element);
                        element = new ZhzxViolationGatherElementTable();
                    }

                    element.UploadName = name;

                    #region Gen_Element

                    if (temp.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_PENDING)      //待审核状态,
                    {
                        if (temp.ViolationType == "闯红灯")
                        {
                            element.Upload_Redlight = temp.ViolationCnt;
                            element.Upload_Count += element.Upload_Redlight;
                        }
                        if (temp.ViolationType == "不按车道行驶")
                        {
                            element.Upload_WrongRoad = temp.ViolationCnt;
                            element.Upload_Count += element.Upload_WrongRoad;
                        }
                        if (temp.ViolationType == "逆行")
                        {
                            element.Upload_Reverse = temp.ViolationCnt;
                            element.Upload_Count += element.Upload_Reverse;
                        }
                        if (temp.ViolationType == "压黄线")
                        {
                            element.Upload_YellowLine = temp.ViolationCnt;
                            element.Upload_Count += element.Upload_YellowLine;
                        }
                        if (temp.ViolationType == "压线")
                        {
                            element.Upload_WhiteLine = temp.ViolationCnt;
                            element.Upload_Count += element.Upload_WhiteLine;
                        }
                        if (temp.ViolationType == "违章变道")
                        {
                            element.Upload_ChangeRoad = temp.ViolationCnt;
                            element.Upload_Count += element.Upload_ChangeRoad;
                        }
                        if (temp.ViolationType == "不系安全带")
                        {
                            element.Upload_NoBelt = temp.ViolationCnt;
                            element.Upload_Count += element.Upload_NoBelt;
                        }
                    }
                    if (temp.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_UPLOAD_REJECT)      //上传打回状态,
                    {
                        if (temp.ViolationType == "闯红灯")
                        {
                            element.UpldRjct_Redlight = temp.ViolationCnt;
                            element.UpldRjct_Count += element.UpldRjct_Redlight;
                        }
                        if (temp.ViolationType == "不按车道行驶")
                        {
                            element.UpldRjct_WrongRoad = temp.ViolationCnt;
                            element.UpldRjct_Count += element.UpldRjct_WrongRoad;
                        }
                        if (temp.ViolationType == "逆行")
                        {
                            element.UpldRjct_Reverse = temp.ViolationCnt;
                            element.UpldRjct_Count += element.UpldRjct_Reverse;
                        }
                        if (temp.ViolationType == "压黄线")
                        {
                            element.UpldRjct_YellowLine = temp.ViolationCnt;
                            element.UpldRjct_Count += element.UpldRjct_YellowLine;
                        }
                        if (temp.ViolationType == "压线")
                        {
                            element.UpldRjct_WhiteLine = temp.ViolationCnt;
                            element.UpldRjct_Count += element.UpldRjct_WhiteLine;
                        }
                        if (temp.ViolationType == "违章变道")
                        {
                            element.UpldRjct_ChangeRoad = temp.ViolationCnt;
                            element.UpldRjct_Count += element.UpldRjct_ChangeRoad;
                        }
                        if (temp.ViolationType == "不系安全带")
                        {
                            element.UpldRjct_NoBelt = temp.ViolationCnt;
                            element.UpldRjct_Count += element.UpldRjct_NoBelt;
                        }
                    }
                    if (temp.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_FILTERED)      //被过滤状态,
                    {
                        if (temp.ViolationType == "闯红灯")
                        {
                            element.Filter_Redlight = temp.ViolationCnt;
                            element.Filter_Count += element.Filter_Redlight;
                        }
                        if (temp.ViolationType == "不按车道行驶")
                        {
                            element.Filter_WrongRoad = temp.ViolationCnt;
                            element.Filter_Count += element.Filter_WrongRoad;
                        }
                        if (temp.ViolationType == "逆行")
                        {
                            element.Filter_Reverse = temp.ViolationCnt;
                            element.Filter_Count += element.Filter_Reverse;
                        }
                        if (temp.ViolationType == "压黄线")
                        {
                            element.Filter_YellowLine = temp.ViolationCnt;
                            element.Filter_Count += element.Filter_YellowLine;
                        }
                        if (temp.ViolationType == "压线")
                        {
                            element.Filter_WhiteLine = temp.ViolationCnt;
                            element.Filter_Count += element.Filter_WhiteLine;
                        }
                        if (temp.ViolationType == "违章变道")
                        {
                            element.Filter_ChangeRoad = temp.ViolationCnt;
                            element.Filter_Count += element.Filter_ChangeRoad;
                        }
                        if (temp.ViolationType == "不系安全带")
                        {
                            element.Filter_NoBelt = temp.ViolationCnt;
                            element.Filter_Count += element.Filter_NoBelt;
                        }
                    }
                    if (temp.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_REJECT)      //审核打回状态,
                    {
                        if (temp.ViolationType == "闯红灯")
                        {
                            element.AprvRjct_Redlight = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_Redlight;
                        }
                        if (temp.ViolationType == "不按车道行驶")
                        {
                            element.AprvRjct_WrongRoad = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_WrongRoad;
                        }
                        if (temp.ViolationType == "逆行")
                        {
                            element.AprvRjct_Reverse = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_Reverse;
                        }
                        if (temp.ViolationType == "压黄线")
                        {
                            element.AprvRjct_YellowLine = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_YellowLine;
                        }
                        if (temp.ViolationType == "压线")
                        {
                            element.AprvRjct_WhiteLine = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_WhiteLine;
                        }
                        if (temp.ViolationType == "违章变道")
                        {
                            element.AprvRjct_ChangeRoad = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_ChangeRoad;
                        }
                        if (temp.ViolationType == "不系安全带")
                        {
                            element.AprvRjct_NoBelt = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_NoBelt;
                        }
                    }
                    if (temp.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVED)      //审核通过状态,
                    {
                        if (temp.ViolationType == "闯红灯")
                        {
                            element.Approved_Redlight = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_Redlight;
                        }
                        if (temp.ViolationType == "不按车道行驶")
                        {
                            element.Approved_WrongRoad = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_WrongRoad;
                        }
                        if (temp.ViolationType == "逆行")
                        {
                            element.Approved_Reverse = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_Reverse;
                        }
                        if (temp.ViolationType == "压黄线")
                        {
                            element.Approved_YellowLine = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_YellowLine;
                        }
                        if (temp.ViolationType == "压线")
                        {
                            element.Approved_WhiteLine = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_WhiteLine;
                        }
                        if (temp.ViolationType == "违章变道")
                        {
                            element.Approved_ChangeRoad = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_ChangeRoad;
                        }
                        if (temp.ViolationType == "不系安全带")
                        {
                            element.Approved_NoBelt = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_NoBelt;
                        }
                    }
                    #endregion Gen_Element
                }

                zhzxTrafficViolationGatherViewModel.ViolationUploadGatherDataSource.Add(element);   // add 最后一个element
            
            }
        }

        public void GenViolationApproveGatherElement()
        {
            if (zhzxTrafficViolationGatherViewModel.ViolationApproveGatherList.Count() == 0)
            {
                zhzxTrafficViolationGatherViewModel.ViolationApproveGatherDataSource = new ObservableCollection<ZhzxViolationGatherElementTable>();
            }
            else if (zhzxTrafficViolationGatherViewModel.ViolationApproveGatherList.Count() != 0)
            {
                zhzxTrafficViolationGatherViewModel.ViolationApproveGatherDataSource = new ObservableCollection<ZhzxViolationGatherElementTable>();
                string name = zhzxTrafficViolationGatherViewModel.ViolationApproveGatherList.First().Name;
                ZhzxViolationGatherElementTable element = new ZhzxViolationGatherElementTable();

                foreach (ZhzxViolationGatherTable temp in zhzxTrafficViolationGatherViewModel.ViolationApproveGatherList)
                {
                    if (temp.Name != name)
                    {
                        name = temp.Name;

                        zhzxTrafficViolationGatherViewModel.ViolationApproveGatherDataSource.Add(element);
                        element = new ZhzxViolationGatherElementTable();
                    }

                    element.ApprovalName = name;

                    #region Gen_Element
   
                    if (temp.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_REJECT)      //审核打回状态,
                    {
                        if (temp.ViolationType == "闯红灯")
                        {
                            element.AprvRjct_Redlight = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_Redlight;
                        }
                        if (temp.ViolationType == "不按车道行驶")
                        {
                            element.AprvRjct_WrongRoad = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_WrongRoad;
                        }
                        if (temp.ViolationType == "逆行")
                        {
                            element.AprvRjct_Reverse = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_Reverse;
                        }
                        if (temp.ViolationType == "压黄线")
                        {
                            element.AprvRjct_YellowLine = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_YellowLine;
                        }
                        if (temp.ViolationType == "压线")
                        {
                            element.AprvRjct_WhiteLine = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_WhiteLine;
                        }
                        if (temp.ViolationType == "违章变道")
                        {
                            element.AprvRjct_ChangeRoad = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_ChangeRoad;
                        }
                        if (temp.ViolationType == "不系安全带")
                        {
                            element.AprvRjct_NoBelt = temp.ViolationCnt;
                            element.AprvRjct_Count += element.AprvRjct_NoBelt;
                        }
                    }
                    if (temp.WorkflowStatus == YcConstantTable.INT_ZHZX_WORKFLOW_APPROVED)      //审核通过状态,
                    {
                        if (temp.ViolationType == "闯红灯")
                        {
                            element.Approved_Redlight = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_Redlight;
                        }
                        if (temp.ViolationType == "不按车道行驶")
                        {
                            element.Approved_WrongRoad = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_WrongRoad;
                        }
                        if (temp.ViolationType == "逆行")
                        {
                            element.Approved_Reverse = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_Reverse;
                        }
                        if (temp.ViolationType == "压黄线")
                        {
                            element.Approved_YellowLine = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_YellowLine;
                        }
                        if (temp.ViolationType == "压线")
                        {
                            element.Approved_WhiteLine = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_WhiteLine;
                        }
                        if (temp.ViolationType == "违章变道")
                        {
                            element.Approved_ChangeRoad = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_ChangeRoad;
                        }
                        if (temp.ViolationType == "不系安全带")
                        {
                            element.Approved_NoBelt = temp.ViolationCnt;
                            element.Approved_Count += element.Approved_NoBelt;
                        }
                    }
                    #endregion Gen_Element
                }

                zhzxTrafficViolationGatherViewModel.ViolationApproveGatherDataSource.Add(element);   // add 最后一个element

            }
        }


        public void Close()
        {
            try
            {
                if (zhzxTrafficViolationGatherViewModel != null)
                {
                    zhzxTrafficViolationGatherViewModel.Close();
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public void AddFakePlateOper()
        {
            zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.IsFakeNumber = true;
            zhzxTrafficViolationDetailsViewModel.ZhzxTrafficViolationEntity.WorkflowStatus = YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_REJECT;

            entityService.Entities.SaveChanges();


            messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "操作成功."));

        
        }

        public void RemoveFakePlateOper()
        {
            zhzxFakePlateNumberViewModel.SelectedFakePlateNumber.IsFakeNumber = false;

            entityService.Entities.SaveChanges();


            messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "操作成功."));

            zhzxFakePlateNumberViewModel.GridRefresh(); 


        }

        public void FakePlateBrowseOper()
        {
            zhzxTrafficViolationViewModel.SelectedTrafficViolation = zhzxFakePlateNumberViewModel.SelectedFakePlateNumber;

            BrowseOper();
            isFakePlateRetreat = true;
        }
    }
}
    