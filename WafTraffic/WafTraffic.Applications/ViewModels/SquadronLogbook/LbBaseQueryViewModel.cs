using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;
using System.Windows.Input;
using DotNet.Business;
using System.Windows;
using WafTraffic.Applications.Services;
using WafTraffic.Applications.Common;
using System.ComponentModel.Composition;

namespace WafTraffic.Applications.ViewModels
{
    public class LbBaseQueryViewModel<TView> : ViewModel<TView> where TView : global::System.Waf.Applications.IView
    {
        #region Data

        private ICommand mNewCommand;
        private ICommand mModifyCommand;
        private ICommand mDeleteCommand;
        private ICommand mDealCommand;
        private ICommand mQueryCommand;
        private ICommand mBrowseCommand;
        private string mSelectedDepartment;
        private DateTime? mSelectedStartDate;
        private DateTime? mSelectedEndDate;
        private List<BaseOrganizeEntity> departmentList;
        private Visibility mAddPermissionVisibility;
        private Visibility mDeletePermissionVisibility;
        private Visibility mModifyPermissionVisibility;
        private Visibility mBrowsePermissionVisibility;
        private Visibility mListAllPermissionVisibility;
        private Visibility mDealPermissionVisibility;
        private Visibility mVerifyPermissionVisibility;
        private bool mHaveAddPermission;
        private bool mHaveListAllPermission;
        private bool mHaveListChargeDeptsPermission;
        IEntityService entityservice;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbBaseQueryViewModel(TView view, IEntityService entityservice)
            : base(view)
        {
            try
            {
                this.entityservice = entityservice;
                mAddPermissionVisibility = Visibility.Hidden;
                mDeletePermissionVisibility = Visibility.Hidden;
                mModifyPermissionVisibility = Visibility.Hidden;
                mBrowsePermissionVisibility = Visibility.Hidden;
                mListAllPermissionVisibility = Visibility.Hidden;
                mHaveAddPermission = false;
                mHaveListAllPermission = false;
                mHaveListChargeDeptsPermission = false;
                mSelectedStartDate = DateTime.Today;
                mSelectedEndDate = DateTime.Today;

                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_ADD))
                {
                    mAddPermissionVisibility = Visibility.Visible;
                    mHaveAddPermission = true;
                }
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_DELETE))
                {
                    mDeletePermissionVisibility = Visibility.Visible;
                }
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_MODIFY))
                {
                    mModifyPermissionVisibility = Visibility.Visible;
                }
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_BROWSE))
                {
                    mBrowsePermissionVisibility = Visibility.Visible;
                }
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    mListAllPermissionVisibility = Visibility.Visible;
                    mHaveListAllPermission = true;
                }
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_DEAL))
                {
                    mDealPermissionVisibility = Visibility.Visible;
                }
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_VERIFY))
                {
                    mVerifyPermissionVisibility = Visibility.Visible;
                }
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
                {
                    mHaveListChargeDeptsPermission = true;
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

        }

        #endregion

        #region Properties

        public List<BaseOrganizeEntity> DepartmentList
        {
            get { return departmentList; }
            set
            {
                if (departmentList != value)
                {
                    departmentList = value;
                    RaisePropertyChanged("DepartmentList");
                }
            }
        }

        public string SelectedDepartment
        {
            get { return mSelectedDepartment; }
            set
            {
                if (mSelectedDepartment != value)
                {
                    mSelectedDepartment = value;
                    RaisePropertyChanged("SelectedDepartment");
                }
            }
        }

        public DateTime? SelectedStartDate
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

        public DateTime? SelectedEndDate
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

        public ICommand NewCommand
        {
            get { return mNewCommand; }
            set
            {
                if (mNewCommand != value)
                {
                    mNewCommand = value;
                    RaisePropertyChanged("NewCommand");
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

        public Visibility BrowsePermissionVisibility
        {
            get { return mBrowsePermissionVisibility; }
        }

        public Visibility ModifyPermissionVisibility
        {
            get { return mModifyPermissionVisibility; }
        }

        public Visibility DeletePermissionVisibility
        {
            get { return mDeletePermissionVisibility; }
        }

        public Visibility AddPermissionVisibility
        {
            get { return mAddPermissionVisibility; }
        }

        public Visibility ListAllPermissionVisibility
        {
            get { return mListAllPermissionVisibility; }
        }

        public Visibility DealPermissionVisibility
        {
            get { return mDealPermissionVisibility; }
        }

        public Visibility VerifyPermissionVisibility
        {
            get { return mVerifyPermissionVisibility; }
        }

        public bool HaveAddPermission
        {
            get { return mHaveAddPermission; }
        }

        public bool HaveListAllPermission
        {
            get { return mHaveListAllPermission; }
        }

        public bool IsSelectDepartmentEnabled
        {
            get { return (mHaveListAllPermission || mHaveListChargeDeptsPermission); }
        }

        #endregion

        #region Members

        public virtual void GridRefresh()
        {

        }

        #endregion
    }
}
