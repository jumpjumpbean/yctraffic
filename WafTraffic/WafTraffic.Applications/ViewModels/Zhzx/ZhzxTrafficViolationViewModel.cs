using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using WafTraffic.Domain.Common;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;
using DotNet.Business;
using System.Data;
using System.Windows;
using WafTraffic.Applications.Common;
using System.Linq;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ZhzxTrafficViolationViewModel : ViewModel<IZhzxTrafficViolationView>
    {

        private IQueryable<ZhzxTrafficViolation> trafficViolation;
        private ZhzxTrafficViolation selectedTrafficViolation;


        private Visibility isBusyIndicatorVisible;
        private Visibility progressCancelVisibility;
        private Visibility batchImportVisibility;
        private Visibility batchExportVisibility;
        private Visibility batchApproveVisibility;

        private int recordCount;
        private int progressBarValue;
        private string xxportProgress;
        private string xxportRecordCountPhrase;
        private string xxportTitle;

        private ICommand browseCommand;
        private ICommand queryCommand;
        private ICommand approveCommand;
        private ICommand batchApproveCommand;
        private ICommand importCommand;
        private ICommand xxportCancelCommand;
        private ICommand exportCommand;
        private ICommand gatherCommand;

        private string sourceDataPath;
        private string targetFolderPath;

        private DateTime startTime;
        private DateTime endTime;
        private string checkpoint;
        private string licensePlate;
        private List<Status> workflowStatusList;
        private List<ViolationType> violationTypeList;
        private int selectWorkflowStatusId;
        private string selectViolationTypePhrase;

        IEntityService entityservice;

        [ImportingConstructor]
        public ZhzxTrafficViolationViewModel(IZhzxTrafficViolationView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.EnumZhzxTrafficViolations != null)
            {
                this.trafficViolation = entityservice.EnumZhzxTrafficViolations;
            }
            else
            {
                this.trafficViolation = new List<ZhzxTrafficViolation>().AsQueryable(); //以防没有数据时出现异常
            }


            workflowStatusList = new List<Status>();
            Status temp = new Status(YcConstantTable.INT_ZHZX_WORKFLOW_ALL, YcConstantTable.STR_ZHZX_WORKFLOW_ALL);
            workflowStatusList.Add(temp);

            temp = new Status(YcConstantTable.INT_ZHZX_WORKFLOW_UPLOAD_PENDING, YcConstantTable.STR_ZHZX_WORKFLOW_UPLOAD_PENDING);
            workflowStatusList.Add(temp);

            temp = new Status(YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_PENDING, YcConstantTable.STR_ZHZX_WORKFLOW_APPROVE_PENDING);
            workflowStatusList.Add(temp);

            temp = new Status(YcConstantTable.INT_ZHZX_WORKFLOW_UPLOAD_REJECT, YcConstantTable.STR_ZHZX_WORKFLOW_UPLOAD_REJECT);
            workflowStatusList.Add(temp);

            temp = new Status(YcConstantTable.INT_ZHZX_WORKFLOW_FILTERED, YcConstantTable.STR_ZHZX_WORKFLOW_FILTERED);
            workflowStatusList.Add(temp);

            temp = new Status(YcConstantTable.INT_ZHZX_WORKFLOW_APPROVE_REJECT, YcConstantTable.STR_ZHZX_WORKFLOW_APPROVE_REJECT);
            workflowStatusList.Add(temp);

            temp = new Status(YcConstantTable.INT_ZHZX_WORKFLOW_APPROVED, YcConstantTable.STR_ZHZX_WORKFLOW_APPROVED);
            workflowStatusList.Add(temp);


            violationTypeList = new List<ViolationType>();
            violationTypeList.Add(new ViolationType(YcConstantTable.STR_ZHZX_VIOTYPE_ALL));
            violationTypeList.Add(new ViolationType(YcConstantTable.STR_ZHZX_VIOTYPE_REDLIGHT));
            violationTypeList.Add(new ViolationType(YcConstantTable.STR_ZHZX_VIOTYPE_WRONGROAD));
            violationTypeList.Add(new ViolationType(YcConstantTable.STR_ZHZX_VIOTYPE_REVERSE));
            violationTypeList.Add(new ViolationType(YcConstantTable.STR_ZHZX_VIOTYPE_YELLOWLINE));
            violationTypeList.Add(new ViolationType(YcConstantTable.STR_ZHZX_VIOTYPE_WHITELINE));
            violationTypeList.Add(new ViolationType(YcConstantTable.STR_ZHZX_VIOTYPE_CHANGEROAD));
            violationTypeList.Add(new ViolationType(YcConstantTable.STR_ZHZX_VIOTYPE_NOBELT));

        }

        public IQueryable<ZhzxTrafficViolation> TrafficViolation
        {
            get
            {
                return trafficViolation;
            }
            set
            {
                trafficViolation = value;
                RaisePropertyChanged("TrafficViolation");
            }
        }

        public ZhzxTrafficViolation SelectedTrafficViolation
        {
            get { return selectedTrafficViolation; }
            set
            {
                if (selectedTrafficViolation != value)
                {
                    selectedTrafficViolation = value;
                    RaisePropertyChanged("SelectedTrafficViolation");
                }
            }
        }

        public string Checkpoint
        {
            get { return checkpoint; }
            set
            {
                if (checkpoint != value)
                {
                    checkpoint = value;
                    RaisePropertyChanged("Checkpoint");
                }
            }
        }

        public string LicensePlate
        {
            get { return licensePlate; }
            set
            {
                if (licensePlate != value)
                {
                    licensePlate = value;
                    RaisePropertyChanged("LicensePlate");
                }
            }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    RaisePropertyChanged("StartTime");
                }
            }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    RaisePropertyChanged("EndTime");
                }
            }
        }

        public void GridRefresh()
        {
            ViewCore.PagingReload();
        }

        public ICommand QueryCommand
        {
            get { return queryCommand; }
            set
            {
                if (queryCommand != value)
                {
                    queryCommand = value;
                    RaisePropertyChanged("QueryCommand");
                }
            }
        }

        public ICommand BrowseCommand
        {
            get { return browseCommand; }
            set
            {
                if (browseCommand != value)
                {
                    browseCommand = value;
                    RaisePropertyChanged("BrowseCommand");
                }
            }
        }

        public ICommand ApproveCommand
        {
            get { return approveCommand; }
            set
            {
                if (approveCommand != value)
                {
                    approveCommand = value;
                    RaisePropertyChanged("ApproveCommand");
                }
            }
        }

        public ICommand BatchApproveCommand
        {
            get { return batchApproveCommand; }
            set
            {
                if (batchApproveCommand != value)
                {
                    batchApproveCommand = value;
                    RaisePropertyChanged("BatchApproveCommand");
                }
            }
        } 

        public ICommand ImportCommand
        {
            get { return importCommand; }
            set
            {
                if (importCommand != value)
                {
                    importCommand = value;
                    RaisePropertyChanged("ImportCommand");
                }
            }
        }


        public ICommand XxportCancelCommand
        {
            get { return xxportCancelCommand; }
            set
            {
                if (xxportCancelCommand != value)
                {
                    xxportCancelCommand = value;
                    RaisePropertyChanged("XxportCancelCommand");
                }
            }
        }


        public ICommand ExportCommand
        {
            get { return exportCommand; }
            set
            {
                if (exportCommand != value)
                {
                    exportCommand = value;
                    RaisePropertyChanged("ExportCommand");
                }
            }
        }

        public ICommand GatherCommand
        {
            get { return gatherCommand; }
            set
            {
                if (gatherCommand != value)
                {
                    gatherCommand = value;
                    RaisePropertyChanged("GatherCommand");
                }
            }
        }

        public string SourceDataPath
        {
            get { return sourceDataPath; }
            set
            {
                if (sourceDataPath != value)
                {
                    sourceDataPath = value;
                    RaisePropertyChanged("SourceDataPath");
                }
            }
        }

        public string TargetFolderPath
        {
            get { return targetFolderPath; }
            set
            {
                if (targetFolderPath != value)
                {
                    targetFolderPath = value;
                    RaisePropertyChanged("TargetFolderPath");
                }
            }
        }

        public int SelectWorkflowStatusId
        {
            get { return selectWorkflowStatusId; }
            set
            {
                if (selectWorkflowStatusId != value)
                {
                    selectWorkflowStatusId = value;
                    RaisePropertyChanged("SelectWorkflowStatusId");
                }
            }
        }

        public List<Status> WorkflowStatusList
        {
            get { return workflowStatusList; }
            set
            {
                if (workflowStatusList != value)
                {
                    workflowStatusList = value;
                    RaisePropertyChanged("WorkflowStatusList");
                }
            }
        }

        public string SelectViolationTypePhrase
        {
            get { return selectViolationTypePhrase; }
            set
            {
                if (selectViolationTypePhrase != value)
                {
                    selectViolationTypePhrase = value;
                    RaisePropertyChanged("SelectViolationTypePhrase");
                }
            }
        }

        public List<ViolationType> ViolationTypeList
        {
            get { return violationTypeList; }
            set
            {
                if (violationTypeList != value)
                {
                    violationTypeList = value;
                    RaisePropertyChanged("ViolationTypeList");
                }
            }
        }

        public Visibility IsBusyIndicatorVisible
        {
            get { return isBusyIndicatorVisible; }
            set
            {
                if (isBusyIndicatorVisible != value)
                {
                    isBusyIndicatorVisible = value;
                    RaisePropertyChanged("IsBusyIndicatorVisible");
                }
            }
        }

        public Visibility ProgressCancelVisibility
        {
            get { return progressCancelVisibility; }
            set
            {
                if (progressCancelVisibility != value)
                {
                    progressCancelVisibility = value;
                    RaisePropertyChanged("ProgressCancelVisibility");
                }
            }
        }

        public Visibility BatchImportVisibility
        {
            get 
            {
                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.TrafficViolation.BatchImport") ||
                    (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstants.INT_ROLE_FDD_ID && AuthService.Instance.IsZhzxCharged) 
                )
                {
                    batchImportVisibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    batchImportVisibility = System.Windows.Visibility.Collapsed;
                }
                
                return batchImportVisibility; 
            }
        }

        public Visibility BatchExportVisibility
        {
            get 
            {
                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.TrafficViolation.BatchExport") ||
                    (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstants.INT_ROLE_FDD_ID && AuthService.Instance.IsZhzxCharged)
                )
                {
                    batchExportVisibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    batchExportVisibility = System.Windows.Visibility.Collapsed;
                }
                
                return batchExportVisibility; 
            }

        }

        public Visibility BatchApproveVisibility
        {
            get
            {
                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.TrafficViolation.BatchCheck") ||
                    (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstants.INT_ROLE_FDD_ID && AuthService.Instance.IsZhzxCharged)
                )
                {
                    batchApproveVisibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    batchApproveVisibility = System.Windows.Visibility.Collapsed;
                }

                return batchApproveVisibility;
            }
        }

        public int RecordCount
        {
            get { return recordCount; }
            set
            {
                if (recordCount != value)
                {
                    recordCount = value;
                    RaisePropertyChanged("RecordCount");
                }
            }
        }

        public string XxportRecordCountPhrase
        {
            get { return xxportRecordCountPhrase; }
            set
            {
                if (xxportRecordCountPhrase != value)
                {
                    xxportRecordCountPhrase = value;
                    RaisePropertyChanged("XxportRecordCountPhrase");
                }
            }
        }

        public string XxportTitle
        {
            get { return xxportTitle; }
            set
            {
                if (xxportTitle != value)
                {
                    xxportTitle = value;
                    RaisePropertyChanged("XxportTitle");
                }
            }
        }

        public int ProgressBarValue
        {
            get { return progressBarValue; }
            set
            {
                if (progressBarValue != value)
                {
                    progressBarValue = value;
                    RaisePropertyChanged("ProgressBarValue");
                }
            }
        }

        public string XxportProgress
        {
            get { return xxportProgress; }
            set
            {
                if (xxportProgress != value)
                {
                    xxportProgress = value;
                    RaisePropertyChanged("XxportProgress");
                }
            }
        }
    }

    public class Status
    {
        private int workflowStatusId;
        private string workflowStatusPhrase;
        public override string ToString()
        {
            return this.WorkflowStatusPhrase;
        }
        
        public Status(int id, string phrase)
        {
            this.WorkflowStatusId = id;
            this.WorkflowStatusPhrase = phrase;
        }

        public int WorkflowStatusId
        {
            get { return workflowStatusId; }
            set
            {
                if (workflowStatusId != value)
                {
                    workflowStatusId = value;
                }
            }
        }

        public string WorkflowStatusPhrase
        {
            get { return workflowStatusPhrase; }
            set
            {
                if (workflowStatusPhrase != value)
                {
                    workflowStatusPhrase = value;
                }
            }
        } 
    }

    public class ViolationType
    {
        //private int violationTypeId;
        private string violationTypePhrase;
        public override string ToString()
        {
            return this.violationTypePhrase;
        }

        public ViolationType(string phrase)
        {
            //this.violationTypeId = id;
            this.violationTypePhrase = phrase;
        }

        //public int ViolationTypeId
        //{
        //    get { return violationTypeId; }
        //    set
        //    {
        //        if (violationTypeId != value)
        //        {
        //            violationTypeId = value;
        //        }
        //    }
        //}

        public string ViolationTypePhrase
        {
            get { return violationTypePhrase; }
            set
            {
                if (violationTypePhrase != value)
                {
                    violationTypePhrase = value;
                }
            }
        }
    }
}
    