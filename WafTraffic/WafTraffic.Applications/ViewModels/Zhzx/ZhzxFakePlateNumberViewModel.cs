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
    public class ZhzxFakePlateNumberViewModel : ViewModel<IZhzxFakePlateNumberView>
    {

        private IQueryable<ZhzxTrafficViolation> fakePlateNumber;
        private ZhzxTrafficViolation selectedFakePlateNumber;

        private ICommand browseCommand;
        private ICommand queryCommand;
        private ICommand removeCommand;

        private DateTime startTime;
        private DateTime endTime;
        private string checkpoint;
        private string licensePlate;
        private List<Status> workflowStatusList;
        private int selectWorkflowStatusId;

        IEntityService entityservice;

        [ImportingConstructor]
        public ZhzxFakePlateNumberViewModel(IZhzxFakePlateNumberView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.QueryableZhzxFakePlateViolations != null)
            {
                this.fakePlateNumber = entityservice.QueryableZhzxFakePlateViolations;
            }
            else
            {
                this.fakePlateNumber = new List<ZhzxTrafficViolation>().AsQueryable(); //以防没有数据时出现异常
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
        }

        public IQueryable<ZhzxTrafficViolation> FakePlateNumber
        {
            get
            {
                return fakePlateNumber;
            }
            set
            {
                fakePlateNumber = value;
                RaisePropertyChanged("FakePlateNumber");
            }
        }

        public ZhzxTrafficViolation SelectedFakePlateNumber
        {
            get { return selectedFakePlateNumber; }
            set
            {
                if (selectedFakePlateNumber != value)
                {
                    selectedFakePlateNumber = value;
                    RaisePropertyChanged("SelectedFakePlateNumber");
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

        public ICommand RemoveCommand
        {
            get { return removeCommand; }
            set
            {
                if (removeCommand != value)
                {
                    removeCommand = value;
                    RaisePropertyChanged("RemoveCommand");
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


    }
}
    