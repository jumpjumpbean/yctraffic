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

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class MaterialDeclareController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;
        private MaterialDeclareViewModel materialDeclareViewModel;   //材料申报模块主界面
        private MaterialDeclareBuildViewModel materialDeclareBuildViewModel;  //创建材料界面
        private MaterialDeclareGatherViewModel materialDeclareGatherViewModel; // 查询统计界面

        private MaterialDeclareTable tmpMaterialDeclareBuild = new MaterialDeclareTable();

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand modifyCommand;
        private readonly DelegateCommand deleteCommand;
        private readonly DelegateCommand browseCommand;
        private readonly DelegateCommand saveCommand;
        private readonly DelegateCommand retreatCommand;
        private readonly DelegateCommand queryCommand;
        private readonly DelegateCommand gatherCommand;
        private readonly DelegateCommand scoreQueryGatherCommand;
        private readonly DelegateCommand amountQueryGatherCommand;
        private readonly DelegateCommand gatherRetreatCommand;
        private readonly DelegateCommand approveCommand;
        private readonly DelegateCommand noApproveCommand;

        [ImportingConstructor]
        public MaterialDeclareController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {
            try
            {
                this.container = container;
                this.messageService = messageService;
                this.shellService = shellService;
                this.entityService = entityService;

                mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
                materialDeclareViewModel = container.GetExportedValue<MaterialDeclareViewModel>();    //材料申报模块主界面
                materialDeclareBuildViewModel = container.GetExportedValue<MaterialDeclareBuildViewModel>(); //创建材料界面
                materialDeclareGatherViewModel = container.GetExportedValue<MaterialDeclareGatherViewModel>(); //查询统计界面

                this.newCommand = new DelegateCommand(() => NewOper(), null);
                this.modifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.deleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.saveCommand = new DelegateCommand(() => Save(), null);
                this.retreatCommand = new DelegateCommand(() => RetreatOper(), null);
                this.browseCommand = new DelegateCommand(() => BrowseOper(), null);
                this.queryCommand = new DelegateCommand(() => QueryOper(), null);
                this.gatherCommand = new DelegateCommand(() => GatherOper(), null);
                this.scoreQueryGatherCommand = new DelegateCommand(() => ScoreQueryGatherOper(), null);
                this.amountQueryGatherCommand = new DelegateCommand(() => AmountQueryGatherOper(), null);
                this.gatherRetreatCommand = new DelegateCommand(() => GatherRetreatOper(), null);
                this.approveCommand = new DelegateCommand(() => ApproveOpre(), null);
                this.noApproveCommand = new DelegateCommand(() => NoApproveOper(), null);
            }
            catch(System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public void Initialize()
        {
            // AddWeakEventListener(materialDeclareViewModel, PersonArchiveListViewModelPropertyChanged);

            try
            {
                materialDeclareViewModel.NewCommand = this.newCommand;
                materialDeclareViewModel.ModifyCommand = this.modifyCommand;
                materialDeclareViewModel.DeleteCommand = this.deleteCommand;
                materialDeclareViewModel.BrowseCommand = this.browseCommand;
                materialDeclareViewModel.QueryCommand = this.queryCommand;
                materialDeclareViewModel.GatherCommand = this.gatherCommand;
                materialDeclareViewModel.ApproveCommand = this.approveCommand;
                materialDeclareViewModel.NoApproveCommand = this.noApproveCommand;

                materialDeclareBuildViewModel.SaveCommand = this.saveCommand;
                materialDeclareBuildViewModel.RetreatCommand = this.retreatCommand;

                materialDeclareGatherViewModel.GatherRetreatCommand = this.gatherRetreatCommand;
                materialDeclareGatherViewModel.ScoreQueryGatherCommand = this.scoreQueryGatherCommand;
                materialDeclareGatherViewModel.AmountQueryGatherCommand = this.amountQueryGatherCommand;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public bool NewOper()
        {
            bool newer = true;
            materialDeclareBuildViewModel.Operation = "New";

            try
            {
                materialDeclareBuildViewModel.CanDepartEdit = true;
                materialDeclareBuildViewModel.CanTitleEdit = false;
                materialDeclareBuildViewModel.CanAuthorEdit = false;
                materialDeclareBuildViewModel.CanDeclareTimeEdit = false;
                materialDeclareBuildViewModel.CanIssueTimeEdit = true;
                materialDeclareBuildViewModel.CanScoreEdit = false;

                materialDeclareBuildViewModel.CanSave = System.Windows.Visibility.Visible;
                materialDeclareBuildViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;

                materialDeclareBuildViewModel.MaterialDeclareBuild = new MaterialDeclareTable();


                BaseOrganizeEntity org = materialDeclareBuildViewModel.DepartmentList.Find(
                    instance => (instance.Id == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId)
                );

                if (org != null)
                {
                    materialDeclareBuildViewModel.MaterialDeclareBuild.DepartmentId = org.Id;
                    materialDeclareBuildViewModel.MaterialDeclareBuild.DepartmentName = org.FullName;
                    materialDeclareBuildViewModel.MaterialDeclareBuild.DepartmentCode = org.Code;
                }

                mainFrameViewModel.ContentView = materialDeclareBuildViewModel.View;
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
            materialDeclareBuildViewModel.Operation = "Modify";

            try
            {
                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator) //管理员可以改
                {
                    materialDeclareBuildViewModel.CanDepartEdit = true;     // 可以改
                    materialDeclareBuildViewModel.CanTitleEdit = false;     // 可以改
                    materialDeclareBuildViewModel.CanAuthorEdit = false;    // 可以改
                    materialDeclareBuildViewModel.CanDeclareTimeEdit = false;// 可以改
                    materialDeclareBuildViewModel.CanIssueTimeEdit = false; // 可以改
                    materialDeclareBuildViewModel.CanScoreEdit = true;     // 可以改

                    materialDeclareBuildViewModel.CanSave = System.Windows.Visibility.Visible;
                    materialDeclareBuildViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;

                }
                else    //创建者可以修改
                {
                    materialDeclareBuildViewModel.CanDepartEdit = true;     // 可以改
                    materialDeclareBuildViewModel.CanTitleEdit = false;     // 可以改
                    materialDeclareBuildViewModel.CanAuthorEdit = false;    // 可以改
                    materialDeclareBuildViewModel.CanDeclareTimeEdit = false;// 可以改
                    materialDeclareBuildViewModel.CanIssueTimeEdit = true; // 可以改
                    materialDeclareBuildViewModel.CanScoreEdit = false;     // 不能改

                    materialDeclareBuildViewModel.CanSave = System.Windows.Visibility.Visible;
                    materialDeclareBuildViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;
                }

                materialDeclareBuildViewModel.MaterialDeclareBuild = materialDeclareViewModel.SelectedMaterialDeclare;

                mainFrameViewModel.ContentView = materialDeclareBuildViewModel.View;
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
            materialDeclareBuildViewModel.Operation = "Delete";

            try
            {
                if (materialDeclareViewModel.SelectedMaterialDeclare != null &&
                    entityService.Entities.MaterialDeclareTables.Select(entity => entity.Id == materialDeclareViewModel.SelectedMaterialDeclare.Id).Count() > 0)
                {
                    DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        materialDeclareViewModel.SelectedMaterialDeclare.IsDeleted = 1;  // set IsDeleted to 1, means this record was deleted by logical, but not real deleted.

                        entityService.Entities.SaveChanges();
                        //刷新DataGrid
                        materialDeclareViewModel.GridRefresh();

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
                materialDeclareBuildViewModel.Operation = "Browse";
                materialDeclareBuildViewModel.CanDepartEdit = false;
                materialDeclareBuildViewModel.CanTitleEdit = true;
                materialDeclareBuildViewModel.CanAuthorEdit = true;
                materialDeclareBuildViewModel.CanDeclareTimeEdit = true;
                materialDeclareBuildViewModel.CanIssueTimeEdit = true;
                materialDeclareBuildViewModel.CanScoreEdit = false;

                materialDeclareBuildViewModel.CanSave = System.Windows.Visibility.Collapsed;
                materialDeclareBuildViewModel.CanCreatorVisibal = System.Windows.Visibility.Visible;

                materialDeclareBuildViewModel.MaterialDeclareBuild = materialDeclareViewModel.SelectedMaterialDeclare;

                mainFrameViewModel.ContentView = materialDeclareBuildViewModel.View;
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
                materialDeclareBuildViewModel.Operation = "Query";
                materialDeclareViewModel.MaterialDeclare = entityService.EnumMaterialDeclares.Where<MaterialDeclareTable>
                    (
                        entity =>
                            ((!string.IsNullOrEmpty(materialDeclareViewModel.SelectDepartCode)) ? (entity.DepartmentCode.IndexOf(materialDeclareViewModel.SelectDepartCode) == 0) : true)
                                &&
                             (entity.MaterialDeclareTime.Value >= materialDeclareViewModel.StartDate)
                                &&
                             (entity.MaterialDeclareTime.Value <= materialDeclareViewModel.EndDate)
                                &&
                             ((string.IsNullOrEmpty(materialDeclareViewModel.KeyWord)) ? true : (entity.Author.Contains(materialDeclareViewModel.KeyWord) || entity.MaterialTitle.Contains(materialDeclareViewModel.KeyWord)))
                                &&
                            (entity.IsDeleted == 0)
                    );

                //列表页
                materialDeclareViewModel.GridRefresh();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool GatherOper()
        {
            bool deal = true;

            try
            {
                mainFrameViewModel.ContentView = materialDeclareGatherViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool ScoreQueryGatherOper()
        {
            bool deal = true;

            try
            {
                materialDeclareGatherViewModel.GatherMaterialScoreList =
                    entityService.GetGatherMaterialByIssueTime(materialDeclareGatherViewModel.IssueStartDate, materialDeclareGatherViewModel.IssueEndDate);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool AmountQueryGatherOper()
        {
            bool deal = true;

            try
            {
                materialDeclareGatherViewModel.GatherMaterialAmountList =
                    entityService.GetGatherMaterialByDeclareTime(materialDeclareGatherViewModel.DeclareStartDate, materialDeclareGatherViewModel.DeclareEndDate);
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
                materialDeclareBuildViewModel.Operation = "Retreat";

                mainFrameViewModel.ContentView = materialDeclareViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool GatherRetreatOper()
        {
            bool newer = true;
            try
            {
                materialDeclareBuildViewModel.Operation = "GatherRetreat";
                materialDeclareBuildViewModel.MaterialDeclareBuild = null;
                mainFrameViewModel.ContentView = materialDeclareViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return newer;
        }

        public bool ApproveOpre()
        {
            bool deal = true;
            materialDeclareBuildViewModel.Operation = "Approve";

            try
            {
                materialDeclareBuildViewModel.CanDepartEdit = false;
                materialDeclareBuildViewModel.CanTitleEdit = true;
                materialDeclareBuildViewModel.CanAuthorEdit = true;
                materialDeclareBuildViewModel.CanDeclareTimeEdit = true;
                materialDeclareBuildViewModel.CanIssueTimeEdit = false;
                materialDeclareBuildViewModel.CanScoreEdit = true;

                materialDeclareBuildViewModel.CanSave = System.Windows.Visibility.Visible;
                materialDeclareBuildViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;

                materialDeclareBuildViewModel.MaterialDeclareBuild = materialDeclareViewModel.SelectedMaterialDeclare;

                mainFrameViewModel.ContentView = materialDeclareBuildViewModel.View;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }

        public bool NoApproveOper()
        {
            bool deal = true;

            try
            {
                materialDeclareBuildViewModel.Operation = "NoApprove";
                materialDeclareViewModel.MaterialDeclare = entityService.EnumNoApproveMaterialDeclares;

                //列表页
                materialDeclareViewModel.GridRefresh();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return deal;
        }


        public bool Save()
        {
            bool saved = false;

            if (materialDeclareBuildViewModel.MaterialDeclareBuild.DepartmentId == null || materialDeclareBuildViewModel.MaterialDeclareBuild.DepartmentId == 0)
            {
                messageService.ShowMessage("部门科室是必填项");
                return false;
            }

            if (string.IsNullOrEmpty(materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialTitle))
            {
                messageService.ShowMessage("材料题目是必填项");
                return false;
            }

            if (materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialDeclareTime == null || materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialDeclareTime == DateTime.MinValue)
            {
                messageService.ShowMessage("申报时间是必填项");
                return false;
            }

            try
            {
                BaseOrganizeEntity org = materialDeclareBuildViewModel.DepartmentList.Find(
                    instance => (instance.Id == materialDeclareBuildViewModel.MaterialDeclareBuild.DepartmentId)
                );

                if (materialDeclareBuildViewModel.MaterialDeclareBuild.Id > 0)
                {
                    if (materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialIssueTime != null && materialDeclareBuildViewModel.MaterialDeclareBuild.Score == null)
                    {
                        messageService.ShowMessage("审批时，评分是必填项");
                        return false;
                    }

                    if (materialDeclareBuildViewModel.MaterialDeclareBuild.Score != null) //审批时保存
                    {

                        if (materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialIssueTime == null || materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialIssueTime == DateTime.MinValue)
                        {
                            messageService.ShowMessage("审批时，发表时间是必填项");
                            return false;
                        }


                        DialogResult result = MessageBox.Show("请再次确认申报内容及审批结果是否无误。\n\r审批一旦完成，申报内容及审批结果将无法修改！\n\r\n\r确定完成审批吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                        if (result == DialogResult.Yes)
                        {
                            materialDeclareBuildViewModel.MaterialDeclareBuild.ApprovalTime = System.DateTime.Now;

                            materialDeclareBuildViewModel.MaterialDeclareBuild.ApprovalName =
                                CurrentLoginService.Instance.CurrentUserInfo.RealName;

                            materialDeclareBuildViewModel.MaterialDeclareBuild.ApprovalUserId =
                                Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                        }
                        else
                        {
                            return false;
                        }
                    }

                    materialDeclareBuildViewModel.MaterialDeclareBuild.DepartmentName = org.FullName;
                    materialDeclareBuildViewModel.MaterialDeclareBuild.DepartmentCode = org.Code;

                    entityService.Entities.SaveChanges(); //update
                }
                else
                {
                    materialDeclareBuildViewModel.MaterialDeclareBuild.CreateUserName =
                        CurrentLoginService.Instance.CurrentUserInfo.RealName;

                    materialDeclareBuildViewModel.MaterialDeclareBuild.CreateUserId =
                        Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);

                    materialDeclareBuildViewModel.MaterialDeclareBuild.DepartmentName = org.FullName;

                    materialDeclareBuildViewModel.MaterialDeclareBuild.DepartmentCode = org.Code;

                    materialDeclareBuildViewModel.MaterialDeclareBuild.SubmitTime = System.DateTime.Now;

                    entityService.Entities.MaterialDeclareTables.AddObject(materialDeclareBuildViewModel.MaterialDeclareBuild);

                    entityService.Entities.SaveChanges(); //insert
                }
                saved = true;


                messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                //返回列表页
                mainFrameViewModel.ContentView = materialDeclareViewModel.View;

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
    }
}
    