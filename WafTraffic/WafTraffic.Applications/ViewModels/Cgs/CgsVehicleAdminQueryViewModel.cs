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
    public class CgsVehicleAdminQueryViewModel : ViewModel<ICgsVehicleAdminQueryView>
    {
        #region Data

        private CgsVehicleAdminCase mSelectedCase;
        private IQueryable<CgsVehicleAdminCase> mPunishCases;
        private ICommand mNewCommand;
        private ICommand mModifyCommand;
        private ICommand mDeleteCommand;
        private ICommand mQueryCommand;
        private ICommand mBrowseCommand;
        private DateTime? mSelectedStartDate;
        private DateTime? mSelectedEndDate;

        private Visibility mAddPermissionVisibility;
        private Visibility mDeletePermissionVisibility;
        private Visibility mModifyPermissionVisibility;
        private Visibility mBrowsePermissionVisibility;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public CgsVehicleAdminQueryViewModel(ICgsVehicleAdminQueryView view, IEntityService entityservice)
            : base(view)
        {
            if (entityservice.EnumCgsVehicleAdminCases != null)
            {
                this.mPunishCases = entityservice.EnumCgsVehicleAdminCases;
            }
            else
            {
                this.mPunishCases = new List<CgsVehicleAdminCase>().AsQueryable(); //以防没有数据时出现异常
            }

            mDeletePermissionVisibility = Visibility.Collapsed;
            mModifyPermissionVisibility = Visibility.Collapsed;
            mBrowsePermissionVisibility = Visibility.Collapsed;
            mAddPermissionVisibility = Visibility.Collapsed;
            if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_CGS_DELETE))
            {
                mDeletePermissionVisibility = Visibility.Visible;
            }
            if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_CGS_MODIFY))
            {
                mModifyPermissionVisibility = Visibility.Visible;
            }
            if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_CGS_BROWSE))
            {
                mBrowsePermissionVisibility = Visibility.Visible;
            }
            if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_CGS_ADD))
            {
                mAddPermissionVisibility = Visibility.Visible;
            }
        }

        #endregion

        #region Properties

        public IQueryable<CgsVehicleAdminCase> PunishCases
        {
            get
            {
                return mPunishCases;
            }
            set
            {
                mPunishCases = value;
                RaisePropertyChanged("PunishCases");
            }
        }

        public CgsVehicleAdminCase SelectedCase
        {
            get { return mSelectedCase; }
            set
            {
                if (mSelectedCase != value)
                {
                    mSelectedCase = value;
                    RaisePropertyChanged("SelectedCase");
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

        #endregion

        #region Members

        public void GridRefresh()
        {
             ViewCore.PagingReload();
        }

        #endregion
    }
}
    