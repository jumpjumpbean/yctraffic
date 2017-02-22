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
        private MaterialDeclareViewModel materialDeclareViewModel;   //�����걨ģ��������
        private MaterialDeclareBuildViewModel materialDeclareBuildViewModel;  //�������Ͻ���
        private MaterialDeclareGatherViewModel materialDeclareGatherViewModel; // ��ѯͳ�ƽ���

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
                materialDeclareViewModel = container.GetExportedValue<MaterialDeclareViewModel>();    //�����걨ģ��������
                materialDeclareBuildViewModel = container.GetExportedValue<MaterialDeclareBuildViewModel>(); //�������Ͻ���
                materialDeclareGatherViewModel = container.GetExportedValue<MaterialDeclareGatherViewModel>(); //��ѯͳ�ƽ���

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
                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator) //����Ա���Ը�
                {
                    materialDeclareBuildViewModel.CanDepartEdit = true;     // ���Ը�
                    materialDeclareBuildViewModel.CanTitleEdit = false;     // ���Ը�
                    materialDeclareBuildViewModel.CanAuthorEdit = false;    // ���Ը�
                    materialDeclareBuildViewModel.CanDeclareTimeEdit = false;// ���Ը�
                    materialDeclareBuildViewModel.CanIssueTimeEdit = false; // ���Ը�
                    materialDeclareBuildViewModel.CanScoreEdit = true;     // ���Ը�

                    materialDeclareBuildViewModel.CanSave = System.Windows.Visibility.Visible;
                    materialDeclareBuildViewModel.CanCreatorVisibal = System.Windows.Visibility.Collapsed;

                }
                else    //�����߿����޸�
                {
                    materialDeclareBuildViewModel.CanDepartEdit = true;     // ���Ը�
                    materialDeclareBuildViewModel.CanTitleEdit = false;     // ���Ը�
                    materialDeclareBuildViewModel.CanAuthorEdit = false;    // ���Ը�
                    materialDeclareBuildViewModel.CanDeclareTimeEdit = false;// ���Ը�
                    materialDeclareBuildViewModel.CanIssueTimeEdit = true; // ���Ը�
                    materialDeclareBuildViewModel.CanScoreEdit = false;     // ���ܸ�

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
                    DialogResult result = MessageBox.Show("ȷ��ɾ����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        materialDeclareViewModel.SelectedMaterialDeclare.IsDeleted = 1;  // set IsDeleted to 1, means this record was deleted by logical, but not real deleted.

                        entityService.Entities.SaveChanges();
                        //ˢ��DataGrid
                        materialDeclareViewModel.GridRefresh();

                        messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "ɾ���ɹ�."));
                    }
                }
                else
                {
                    messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "δ�ҵ�"));
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

                //�б�ҳ
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

                //�б�ҳ
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
                messageService.ShowMessage("���ſ����Ǳ�����");
                return false;
            }

            if (string.IsNullOrEmpty(materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialTitle))
            {
                messageService.ShowMessage("������Ŀ�Ǳ�����");
                return false;
            }

            if (materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialDeclareTime == null || materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialDeclareTime == DateTime.MinValue)
            {
                messageService.ShowMessage("�걨ʱ���Ǳ�����");
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
                        messageService.ShowMessage("����ʱ�������Ǳ�����");
                        return false;
                    }

                    if (materialDeclareBuildViewModel.MaterialDeclareBuild.Score != null) //����ʱ����
                    {

                        if (materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialIssueTime == null || materialDeclareBuildViewModel.MaterialDeclareBuild.MaterialIssueTime == DateTime.MinValue)
                        {
                            messageService.ShowMessage("����ʱ������ʱ���Ǳ�����");
                            return false;
                        }


                        DialogResult result = MessageBox.Show("���ٴ�ȷ���걨���ݼ���������Ƿ�����\n\r����һ����ɣ��걨���ݼ�����������޷��޸ģ�\n\r\n\rȷ�����������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

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


                messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture, "����ɹ�."));
                //�����б�ҳ
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
    