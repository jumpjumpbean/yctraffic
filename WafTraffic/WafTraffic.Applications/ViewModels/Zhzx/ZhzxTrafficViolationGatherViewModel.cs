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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ZhzxTrafficViolationGatherViewModel : ViewModel<IZhzxTrafficViolationGatherView>
    {
        private ICommand uploadQueryGatherCommand;
        private ICommand approveQueryGatherCommand;
        private ICommand retreatCommand;

        private DateTime uploadStartDate;
        private DateTime uploadEndDate;
        private DateTime approveStartDate;
        private DateTime approveEndDate;

        private List<ZhzxViolationGatherTable> violationUploadGatherList;
        private List<ZhzxViolationGatherTable> violationApproveGatherList;
        private ObservableCollection<ZhzxViolationGatherElementTable> violationUploadGatherDataSource;
        private ObservableCollection<ZhzxViolationGatherElementTable> violationApproveGatherDataSource;


        IEntityService entityservice;

        [ImportingConstructor]
        public ZhzxTrafficViolationGatherViewModel(IZhzxTrafficViolationGatherView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

        }

        public ICommand UploadQueryGatherCommand
        {
            get { return uploadQueryGatherCommand; }
            set
            {
                if (uploadQueryGatherCommand != value)
                {
                    uploadQueryGatherCommand = value;
                    RaisePropertyChanged("UploadQueryGatherCommand");
                }
            }
        }

        public ICommand ApproveQueryGatherCommand
        {
            get { return approveQueryGatherCommand; }
            set
            {
                if (approveQueryGatherCommand != value)
                {
                    approveQueryGatherCommand = value;
                    RaisePropertyChanged("ApproveQueryGatherCommand");
                }
            }
        }

        public ICommand RetreatCommand
        {
            get { return retreatCommand; }
            set
            {
                if (retreatCommand != value)
                {
                    retreatCommand = value;
                    RaisePropertyChanged("RetreatCommand");
                }
            }
        }

        public List<ZhzxViolationGatherTable> ViolationUploadGatherList
        {
            get
            {
                return violationUploadGatherList;
            }
            set
            {
                violationUploadGatherList = value;
                RaisePropertyChanged("ViolationUploadGatherList");
            }
        }

        public List<ZhzxViolationGatherTable> ViolationApproveGatherList
        {
            get
            {
                return violationApproveGatherList;
            }
            set
            {
                violationApproveGatherList = value;
                RaisePropertyChanged("ViolationApproveGatherList");
            }
        }

        public ObservableCollection<ZhzxViolationGatherElementTable> ViolationUploadGatherDataSource
        {
            get
            {
                return violationUploadGatherDataSource;
            }
            set
            {
                violationUploadGatherDataSource = value;
                RaisePropertyChanged("ViolationUploadGatherDataSource");
            }
        }

        public ObservableCollection<ZhzxViolationGatherElementTable> ViolationApproveGatherDataSource
        {
            get
            {
                return violationApproveGatherDataSource;
            }
            set
            {
                violationApproveGatherDataSource = value;
                RaisePropertyChanged("ViolationApproveGatherDataSource");
            }
        }

        public DateTime UploadStartDate
        {
            get { return uploadStartDate; }
            set
            {
                if (uploadStartDate != value)
                {
                    uploadStartDate = value;
                    RaisePropertyChanged("UploadStartDate");
                }
            }
        }

        public DateTime UploadEndDate
        {
            get { return uploadEndDate; }
            set
            {
                if (uploadEndDate != value)
                {
                    uploadEndDate = value;
                    RaisePropertyChanged("UploadEndDate");
                }
            }
        }

        public DateTime ApproveStartDate
        {
            get { return approveStartDate; }
            set
            {
                if (approveStartDate != value)
                {
                    approveStartDate = value;
                    RaisePropertyChanged("ApproveStartDate");
                }
            }
        }

        public DateTime ApproveEndDate
        {
            get { return approveEndDate; }
            set
            {
                if (approveEndDate != value)
                {
                    approveEndDate = value;
                    RaisePropertyChanged("ApproveEndDate");
                }
            }
        }

        public void UploadReprotRefresh()
        {
            ViewCore.UploadReportReload();
        }

        public void ApproveReprotRefresh()
        {
            ViewCore.ApproveReportReload();
        }

        public void Close()
        {
            ViewCore.Close();
        }

        
    }
}
    