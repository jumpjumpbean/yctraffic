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
using WafTraffic.Applications.Utils;
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class GggsPublishNoticeController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private GggsPublishNoticeListViewModel gggsPublishNoticeListViewModel;   //公告公示模块主界面
        private GggsPublishNoticeDetailViewModel gggsPublishNoticeDetailViewModel;  
        //private MaterialDeclareGatherViewModel materialDeclareGatherViewModel; // 查询统计界面

        private GggsPublishNotice tmpGggsPublishNotice = new GggsPublishNotice();

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand modifyCommand;
        private readonly DelegateCommand deleteCommand;
        private readonly DelegateCommand browseCommand;
        private readonly DelegateCommand saveCommand;
        private readonly DelegateCommand retreatCommand;
        private readonly DelegateCommand queryCommand;
        private readonly DelegateCommand approveCommand;
        private readonly DelegateCommand downloadCommand;

        private string fileFrom;
        private string fileTo;

        [ImportingConstructor]
        public GggsPublishNoticeController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.container = container;
                this.messageService = messageService;
                this.shellService = shellService;
                this.entityService = entityService;

                mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                gggsPublishNoticeListViewModel = container.GetExportedValue<GggsPublishNoticeListViewModel>();
                gggsPublishNoticeDetailViewModel = container.GetExportedValue<GggsPublishNoticeDetailViewModel>(); 

                this.newCommand = new DelegateCommand(() => NewOper(), null);
                this.modifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.deleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.saveCommand = new DelegateCommand(() => Save(), null);
                this.retreatCommand = new DelegateCommand(() => RetreatOper(), null);
                this.browseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.queryCommand = new DelegateCommand(() => QueryOper(), null);
                this.approveCommand = new DelegateCommand(() => ApproveOper(), null);
                this.downloadCommand = new DelegateCommand(() => DownloadOper(), null);
            }
            catch(System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public void Initialize()
        {
            try
            {
                gggsPublishNoticeListViewModel.NewCommand = this.newCommand;
                gggsPublishNoticeListViewModel.ModifyCommand = this.modifyCommand;
                gggsPublishNoticeListViewModel.DeleteCommand = this.deleteCommand;
                gggsPublishNoticeListViewModel.BrowseCommand = this.browseCommand;
                gggsPublishNoticeListViewModel.QueryCommand = this.queryCommand;
                gggsPublishNoticeListViewModel.ApproveCommand = this.approveCommand;

                gggsPublishNoticeDetailViewModel.SaveCommand = this.saveCommand;
                gggsPublishNoticeDetailViewModel.RetreatCommand = this.retreatCommand;
                gggsPublishNoticeDetailViewModel.DownloadCommand = this.downloadCommand;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool NewOper()
        {
            bool newer = true;
            gggsPublishNoticeDetailViewModel.Operation = "New";

            try
            {
                gggsPublishNoticeDetailViewModel.CanTitleEdit = false;
                gggsPublishNoticeDetailViewModel.CanAuthorEdit = false;
                gggsPublishNoticeDetailViewModel.CanContentEdit = false;
                gggsPublishNoticeDetailViewModel.CanCategoryEdit = true;

                gggsPublishNoticeDetailViewModel.CanSave = System.Windows.Visibility.Visible;
                gggsPublishNoticeDetailViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;
                gggsPublishNoticeDetailViewModel.CanAuditorVisibal = System.Windows.Visibility.Collapsed;
                gggsPublishNoticeDetailViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                gggsPublishNoticeDetailViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                gggsPublishNoticeDetailViewModel.CanAboveSaveVisibal = System.Windows.Visibility.Visible;
                gggsPublishNoticeDetailViewModel.CanBelowSaveVisibal = System.Windows.Visibility.Collapsed;

                gggsPublishNoticeDetailViewModel.GggsPublishNotice = new GggsPublishNotice();


                BaseOrganizeEntity org = gggsPublishNoticeDetailViewModel.DepartmentList.Find(
                    instance => (instance.Id == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId)
                );

                if (org != null)
                {
                    gggsPublishNoticeDetailViewModel.GggsPublishNotice.DepartmentId = org.Id;
                    gggsPublishNoticeDetailViewModel.GggsPublishNotice.DepartmentName = org.FullName;
                }

                mainFrameViewModel.ContentView = gggsPublishNoticeDetailViewModel.View;
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
            gggsPublishNoticeDetailViewModel.Operation = "Modify";

            try
            {
                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator) //管理员可以改
                {
                    gggsPublishNoticeDetailViewModel.CanTitleEdit = false;     // 可以改
                    gggsPublishNoticeDetailViewModel.CanAuthorEdit = false;    // 可以改
                    gggsPublishNoticeDetailViewModel.CanContentEdit = false;     // 可以改
                    gggsPublishNoticeDetailViewModel.CanCategoryEdit = true;    // 可以改

                }
                else    //创建者可以修改
                {
                    gggsPublishNoticeDetailViewModel.CanTitleEdit = false;     // 可以改
                    gggsPublishNoticeDetailViewModel.CanAuthorEdit = false;    // 可以改
                    gggsPublishNoticeDetailViewModel.CanContentEdit = false;     // 可以改
                    gggsPublishNoticeDetailViewModel.CanCategoryEdit = true;    // 可以改
                }

                gggsPublishNoticeDetailViewModel.CanSave = System.Windows.Visibility.Visible;
                gggsPublishNoticeDetailViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;
                gggsPublishNoticeDetailViewModel.CanAuditorVisibal = System.Windows.Visibility.Collapsed;
                gggsPublishNoticeDetailViewModel.CanAboveSaveVisibal = System.Windows.Visibility.Visible;
                gggsPublishNoticeDetailViewModel.CanBelowSaveVisibal = System.Windows.Visibility.Collapsed;
                gggsPublishNoticeDetailViewModel.CanUploadVisibal = System.Windows.Visibility.Visible;
                gggsPublishNoticeDetailViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;

                gggsPublishNoticeDetailViewModel.GggsPublishNotice = gggsPublishNoticeListViewModel.SelectedGggsPublishNotice;

                mainFrameViewModel.ContentView = gggsPublishNoticeDetailViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool DeleteOper()
        {
            bool deler = true;
            gggsPublishNoticeDetailViewModel.Operation = "Delete";

            try
            {
                if (gggsPublishNoticeListViewModel.SelectedGggsPublishNotice != null &&
                    entityService.Entities.GggsPublishNotices.Select(entity => entity.Id == gggsPublishNoticeListViewModel.SelectedGggsPublishNotice.Id).Count() > 0)
                {
                    DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        gggsPublishNoticeListViewModel.SelectedGggsPublishNotice.IsDeleted = 1;  // set IsDeleted to 1, means this record was deleted by logical, but not real deleted.

                        entityService.Entities.SaveChanges();
                        //刷新DataGrid
                        gggsPublishNoticeListViewModel.GridRefresh();

                        messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "删除成功."));
                    }
                }
                else
                {
                    messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "未找到"));
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deler;
        }

        public bool BrowseOper()
        {
            bool deal = true;
            try
            {
                gggsPublishNoticeDetailViewModel.Operation = "Browse";
                gggsPublishNoticeDetailViewModel.CanCategoryEdit = false;
                gggsPublishNoticeDetailViewModel.CanTitleEdit = true;
                gggsPublishNoticeDetailViewModel.CanAuthorEdit = true;
                gggsPublishNoticeDetailViewModel.CanStatusEdit = false;
                gggsPublishNoticeDetailViewModel.CanRemarkEdit = true;
                gggsPublishNoticeDetailViewModel.CanContentEdit = true;

                gggsPublishNoticeDetailViewModel.CanSave = System.Windows.Visibility.Collapsed;
                gggsPublishNoticeDetailViewModel.CanCreatorVisibal = System.Windows.Visibility.Visible;
                gggsPublishNoticeDetailViewModel.CanAuditorVisibal = System.Windows.Visibility.Visible;
                gggsPublishNoticeDetailViewModel.CanAboveSaveVisibal = System.Windows.Visibility.Collapsed;
                gggsPublishNoticeDetailViewModel.CanBelowSaveVisibal = System.Windows.Visibility.Visible;

                gggsPublishNoticeDetailViewModel.CanUploadVisibal = System.Windows.Visibility.Collapsed;

                if (!string.IsNullOrEmpty(gggsPublishNoticeListViewModel.SelectedGggsPublishNotice.AttachmentName1))
                {
                    gggsPublishNoticeDetailViewModel.CanDownloadVisibal = System.Windows.Visibility.Visible;
                }
                else
                {
                    gggsPublishNoticeDetailViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                }

                gggsPublishNoticeDetailViewModel.GggsPublishNotice = gggsPublishNoticeListViewModel.SelectedGggsPublishNotice;

                mainFrameViewModel.ContentView = gggsPublishNoticeDetailViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool QueryOper()
        {
            bool deal = true;
            try
            {
                gggsPublishNoticeDetailViewModel.Operation = "Query";
                gggsPublishNoticeListViewModel.GggsPublishNotice = entityService.QueryableGggsPublishNotice.Where<GggsPublishNotice>
                    (
                        entity =>
                            (
                             (entity.CreateTime.Value >= gggsPublishNoticeListViewModel.StartDate)
                                &&
                             (entity.CreateTime.Value <= gggsPublishNoticeListViewModel.EndDate)
                                &&
                             ((string.IsNullOrEmpty(gggsPublishNoticeListViewModel.KeyWord)) ? true : (entity.Content.Contains(gggsPublishNoticeListViewModel.KeyWord) || entity.Title.Contains(gggsPublishNoticeListViewModel.KeyWord)))
                                &&
                            (entity.IsDeleted == 0))
                    );

                //列表页
                gggsPublishNoticeListViewModel.GridRefresh();
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

            try
            {
                gggsPublishNoticeDetailViewModel.Operation = "Retreat";

                mainFrameViewModel.ContentView = gggsPublishNoticeListViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool ApproveOper()
        {
            bool deal = true;
            gggsPublishNoticeDetailViewModel.Operation = "Approve";

            try
            {
                gggsPublishNoticeDetailViewModel.CanCategoryEdit = false;
                gggsPublishNoticeDetailViewModel.CanTitleEdit = true;
                gggsPublishNoticeDetailViewModel.CanAuthorEdit = true;
                gggsPublishNoticeDetailViewModel.CanRemarkEdit = false;
                gggsPublishNoticeDetailViewModel.CanStatusEdit = true;
                gggsPublishNoticeDetailViewModel.CanContentEdit = true;

                gggsPublishNoticeDetailViewModel.CanSave = System.Windows.Visibility.Visible;
                gggsPublishNoticeDetailViewModel.CanCreatorVisibal = System.Windows.Visibility.Visible;
                gggsPublishNoticeDetailViewModel.CanAuditorVisibal = System.Windows.Visibility.Collapsed;
                gggsPublishNoticeDetailViewModel.CanAboveSaveVisibal = System.Windows.Visibility.Collapsed;
                gggsPublishNoticeDetailViewModel.CanBelowSaveVisibal = System.Windows.Visibility.Visible;

                gggsPublishNoticeDetailViewModel.CanUploadVisibal = System.Windows.Visibility.Collapsed;

                if (!string.IsNullOrEmpty(gggsPublishNoticeListViewModel.SelectedGggsPublishNotice.AttachmentName1))
                {
                    gggsPublishNoticeDetailViewModel.CanDownloadVisibal = System.Windows.Visibility.Visible;
                }
                else
                {
                    gggsPublishNoticeDetailViewModel.CanDownloadVisibal = System.Windows.Visibility.Collapsed;
                }

                gggsPublishNoticeDetailViewModel.GggsPublishNotice = gggsPublishNoticeListViewModel.SelectedGggsPublishNotice;

                mainFrameViewModel.ContentView = gggsPublishNoticeDetailViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }


        public bool Save()
        {
            bool saver = true;

            if (!ValueCheck())
            {
                return false;
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(WorkerSave);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerSaveCompleted);

            worker.RunWorkerAsync();

            gggsPublishNoticeDetailViewModel.Show_LoadingMask(LoadingType.View);

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

                if (gggsPublishNoticeDetailViewModel.GggsPublishNotice.Id > 0)   //update
                {
                    if (gggsPublishNoticeDetailViewModel.Operation == "Approve") //审批时保存
                    {

                        gggsPublishNoticeDetailViewModel.GggsPublishNotice.AuditTime = System.DateTime.Now;

                        gggsPublishNoticeDetailViewModel.GggsPublishNotice.AuditorName =
                            CurrentLoginService.Instance.CurrentUserInfo.RealName;

                        gggsPublishNoticeDetailViewModel.GggsPublishNotice.AuditorId =
                            Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    }

                    if (!string.IsNullOrEmpty(gggsPublishNoticeDetailViewModel.FileLocalPath))
                    {
                        oldFile = gggsPublishNoticeDetailViewModel.GggsPublishNotice.AttachmentPath1;
                        gggsPublishNoticeDetailViewModel.GggsPublishNotice.AttachmentPath1 =
                            ftp.UpdateFile(FtpFileType.Document, gggsPublishNoticeDetailViewModel.FileLocalPath, oldFile);

                        if (string.IsNullOrEmpty(gggsPublishNoticeDetailViewModel.GggsPublishNotice.AttachmentPath1))
                        {
                            throw new ValidationException();
                        }
                    }

                }
                else  //insert
                {
                    gggsPublishNoticeDetailViewModel.GggsPublishNotice.CreateName =
                        CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    gggsPublishNoticeDetailViewModel.GggsPublishNotice.CreateId =
                        Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);

                    gggsPublishNoticeDetailViewModel.GggsPublishNotice.CreateTime = System.DateTime.Now;

                    if (!string.IsNullOrEmpty(gggsPublishNoticeDetailViewModel.FileLocalPath))
                    {

                        gggsPublishNoticeDetailViewModel.GggsPublishNotice.AttachmentPath1 =
                            ftp.UploadFile(FtpFileType.Document, gggsPublishNoticeDetailViewModel.FileLocalPath);

                        if (string.IsNullOrEmpty(gggsPublishNoticeDetailViewModel.GggsPublishNotice.AttachmentPath1))
                        {
                            throw new ValidationException();
                        }
                    }

                    entityService.Entities.GggsPublishNotices.AddObject(gggsPublishNoticeDetailViewModel.GggsPublishNotice);
                }

                e.Result = true;
                entityService.Entities.SaveChanges();
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

            gggsPublishNoticeDetailViewModel.Shutdown_LoadingMask(LoadingType.View);
            if (ret)
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //返回列表页
                mainFrameViewModel.ContentView = gggsPublishNoticeListViewModel.View;
            }
            else
            {
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValueCheck()
        {
            if (string.IsNullOrEmpty(gggsPublishNoticeDetailViewModel.GggsPublishNotice.Category))
            {
                messageService.ShowMessage("栏目是必填项");
                return false;
            }

            if (string.IsNullOrEmpty(gggsPublishNoticeDetailViewModel.GggsPublishNotice.Title))
            {
                messageService.ShowMessage("主题是必填项");
                return false;
            }

            if (gggsPublishNoticeDetailViewModel.Operation == "Approve") //审批时保存
            {
                if (string.IsNullOrEmpty(gggsPublishNoticeDetailViewModel.GggsPublishNotice.Status))
                {
                    messageService.ShowMessage("审核结果是必填项");
                    return false;
                }
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
                sf.FileName = gggsPublishNoticeDetailViewModel.GggsPublishNotice.AttachmentName1;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    fileFrom = gggsPublishNoticeDetailViewModel.GggsPublishNotice.AttachmentPath1;
                    fileTo = sf.FileName;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_Download);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_DownloadCompleted);
                    worker.RunWorkerAsync();

                    gggsPublishNoticeDetailViewModel.Show_LoadingMask(LoadingType.View);
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

                gggsPublishNoticeDetailViewModel.Shutdown_LoadingMask(LoadingType.View);
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
    