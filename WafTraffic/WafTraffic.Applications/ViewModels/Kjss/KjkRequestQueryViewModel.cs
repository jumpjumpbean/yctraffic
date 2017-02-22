using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
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
    public class KjkRequestQueryViewModel : ViewModel<IKjkRequestQueryView>
    {
        #region Data

        private KjssEquipmentRequest mSelectedRequest = null;
        private IQueryable<KjssEquipmentRequest> mRequests = null;
        private List<Status> mStatusList = null;
        private int mSelectedStatus = 0;
        private DateTime mSelectedStartDate;
        private DateTime mSelectedEndDate;

        private ICommand mAddCommand = null;
        private ICommand mModifyCommand = null;
        private ICommand mDeleteCommand = null;
        private ICommand mDealCommand = null;
        private ICommand mQueryCommand = null;
        private ICommand mBrowseCommand = null;

        IEntityService entityservice = null;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public KjkRequestQueryViewModel(IKjkRequestQueryView view, IEntityService entityservice)
            : base(view)
        {
            try
            {
                this.entityservice = entityservice;
                if (entityservice.EnumKjssEquipmentRequests != null)
                {
                    this.mRequests = entityservice.EnumKjssEquipmentRequests;
                }
                else
                {
                    this.mRequests = new List<KjssEquipmentRequest>().AsQueryable();
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

        }

        #endregion

        #region Properties

        public IQueryable<KjssEquipmentRequest> Requests
        {
            get
            {
                return mRequests;
            }
            set
            {
                mRequests = value;
                RaisePropertyChanged("Requests");
            }
        }

        public KjssEquipmentRequest SelectedRequest
        {
            get { return mSelectedRequest; }
            set
            {
                if (mSelectedRequest != value)
                {
                    mSelectedRequest = value;
                    RaisePropertyChanged("SelectedRequest");
                }
            }
        }

        public DateTime SelectedStartDate
        {
            get { return mSelectedStartDate; }
            set
            {
                if (mSelectedStartDate != value)
                {
                    mSelectedStartDate = value;
                    RaisePropertyChanged("SelectedStartDate");
                }
            }
        }

        public DateTime SelectedEndDate
        {
            get { return mSelectedEndDate; }
            set
            {
                if (mSelectedEndDate != value)
                {
                    mSelectedEndDate = value;
                    RaisePropertyChanged("SelectedEndDate");
                }
            }
        }

        public List<Status> StatusList
        {
            get { return mStatusList; }
            set
            {
                if (mStatusList != value)
                {
                    mStatusList = value;
                    RaisePropertyChanged("StatusList");
                }
            }
        }

        public int SelectedStatus
        {
            get { return mSelectedStatus; }
            set
            {
                if (mSelectedStatus != value)
                {
                    mSelectedStatus = value;
                    RaisePropertyChanged("SelectedStatus");
                }
            }
        }

        public ICommand AddCommand
        {
            get { return mAddCommand; }
            set
            {
                if (mAddCommand != value)
                {
                    mAddCommand = value;
                    RaisePropertyChanged("AddCommand");
                }
            }
        }

        public ICommand ModifyCommand
        {
            get { return mModifyCommand; }
            set
            {
                if (mModifyCommand != value)
                {
                    mModifyCommand = value;
                    RaisePropertyChanged("ModifyCommand");
                }
            }
        }

        public ICommand DeleteCommand
        {
            get { return mDeleteCommand; }
            set
            {
                if (mDeleteCommand != value)
                {
                    mDeleteCommand = value;
                    RaisePropertyChanged("DeleteCommand");
                }
            }
        }

        public ICommand DealCommand
        {
            get { return mDealCommand; }
            set
            {
                if (mDealCommand != value)
                {
                    mDealCommand = value;
                    RaisePropertyChanged("DealCommand");
                }
            }
        }

        public ICommand QueryCommand
        {
            get { return mQueryCommand; }
            set
            {
                if (mQueryCommand != value)
                {
                    mQueryCommand = value;
                    RaisePropertyChanged("QueryCommand");
                }
            }
        }

        public ICommand BrowseCommand
        {
            get { return mBrowseCommand; }
            set
            {
                if (mBrowseCommand != value)
                {
                    mBrowseCommand = value;
                    RaisePropertyChanged("BrowseCommand");
                }
            }
        }

        public bool CanAdd
        {
            get { return AuthService.Instance.KjkRequestCanAdd; }
        }

        #endregion

        #region Members

        public void GridRefresh()
        {
            ViewCore.PagingReload();
        }

        #endregion
    }
}
    