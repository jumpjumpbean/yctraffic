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
using DotNet.Utilities;
using System.Windows.Media.Imaging;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class SskRequestUpdateViewModel : ViewModel<ISskRequestUpdateView>
    {
        #region Data

        private SskEquipmentRequest mEntity = null;
        private List<BaseOrganizeEntity> mDepartmentList = null;
        private List<Status> mRequestTypeList = null;
        private List<BaseUserEntity> mLeaderList = null;
        private int mSelectedDepartment = 0;
        private int mSelectedRequestType = 0;
        private int mSelectedSubLeader = 0;

        private ICommand mSubmitCommand = null;
        private ICommand mCancelCommand = null;
        private ICommand mRejectCommand = null;
        private ICommand mSuperviseCommand = null;
        private ICommand mSubLeaderSignCommand = null;
        private ICommand mDdzSignCommand = null;
        private ICommand mShowSignImgCommand = null;

        private bool mIsBrowse;
        private bool mIsSuperviseButtonVisible;

        private BitmapImage mSubLeaderSignImg;
        private BitmapImage mDdzSignImg;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public SskRequestUpdateViewModel(ISskRequestUpdateView view)
            : base(view)
        {
            //mEntity = new KjkEquipmentRequest();
        }

        #endregion

        #region Properties

        public SskEquipmentRequest EquipmentRequestEntity
        {
            get { return mEntity; }
            set
            {
                //if (mEntity != value)
                //{
                    mEntity = value;
                    RaisePropertyChanged("EquipmentRequestEntity");
                    RaisePropertyChanged("IsBaseInfoReadOnly");
                    RaisePropertyChanged("IsBaseInfoEnabled");
                    RaisePropertyChanged("IsDdzInfoReadOnly");
                    //RaisePropertyChanged("IsAdvancedInfoReadOnly");
                    RaisePropertyChanged("IsSubLeaderInfoReadOnly");
                    RaisePropertyChanged("IsSubLeaderInfoVisible");
                    RaisePropertyChanged("IsDdzInfoVisible");
                    //RaisePropertyChanged("IsAdvancedInfoVisible");
                    RaisePropertyChanged("IsRejectButtonVisible");
                    //RaisePropertyChanged("IsSuperviseInfoReadOnly");
                    //RaisePropertyChanged("IsSuperviseInfoVisible");
                    RaisePropertyChanged("IsSubLeaderSignButtonVisible");
                    RaisePropertyChanged("IsDdzSignButtonVisible");
                    RaisePropertyChanged("IsSskInfoReadOnly");
                    RaisePropertyChanged("IsSskInfoVisible");
                //}
            }
        }

        public List<Status> RequestTypeList
        {
            get { return mRequestTypeList; }
            set
            {
                if (mRequestTypeList != value)
                {
                    mRequestTypeList = value;
                    RaisePropertyChanged("RequestTypeList");
                }
            }
        }

        public List<BaseOrganizeEntity> DepartmentList
        {
            get { return mDepartmentList; }
            set
            {
                if (mDepartmentList != value)
                {
                    mDepartmentList = value;
                    RaisePropertyChanged("DepartmentList");
                }
            }
        }

        public List<BaseUserEntity> LeaderList
        {
            get { return mLeaderList; }
            set
            {
                //if (mLeaderList != value)
                //{
                    mLeaderList = value;
                    RaisePropertyChanged("LeaderList");
                //}
            }
        }

        public int SelectedDepartment
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

        public int SelectedRequestType
        {
            get { return mSelectedRequestType; }
            set
            {
                if (mSelectedRequestType != value)
                {
                    mSelectedRequestType = value;
                    RaisePropertyChanged("SelectedRequestType");
                }
            }
        }

        public int SelectedSubLeader
        {
            get { return mSelectedSubLeader; }
            set
            {
                if (mSelectedSubLeader != value)
                {
                    mSelectedSubLeader = value;
                    RaisePropertyChanged("SelectedSubLeader");
                }
            }
        }

        public ICommand SubmitCommand
        {
            get { return mSubmitCommand; }
            set
            {
                if (mSubmitCommand != value)
                {
                    mSubmitCommand = value;
                    RaisePropertyChanged("SubmitCommand");
                }
            }
        }

        public ICommand CancelCommand
        {
            get { return mCancelCommand; }
            set
            {
                if (mCancelCommand != value)
                {
                    mCancelCommand = value;
                    RaisePropertyChanged("CancelCommand");
                }
            }
        }

        public ICommand RejectCommand
        {
            get { return mRejectCommand; }
            set
            {
                if (mRejectCommand != value)
                {
                    mRejectCommand = value;
                    RaisePropertyChanged("RejectCommand");
                }
            }
        }

        public ICommand SuperviseCommand
        {
            get { return mSuperviseCommand; }
            set
            {
                if (mSuperviseCommand != value)
                {
                    mSuperviseCommand = value;
                    RaisePropertyChanged("SuperviseCommand");
                }
            }
        }

        public ICommand SubLeaderSignCommand
        {
            get { return mSubLeaderSignCommand; }
            set
            {
                if (mSubLeaderSignCommand != value)
                {
                    mSubLeaderSignCommand = value;
                    RaisePropertyChanged("SubLeaderSignCommand");
                }
            }
        }

        public ICommand DdzSignCommand
        {
            get { return mDdzSignCommand; }
            set
            {
                if (mDdzSignCommand != value)
                {
                    mDdzSignCommand = value;
                    RaisePropertyChanged("DdzSignCommand");
                }
            }
        }

        public ICommand ShowSignImgCommand
        {
            get { return mShowSignImgCommand; }
            set
            {
                if (mShowSignImgCommand != value)
                {
                    mShowSignImgCommand = value;
                    RaisePropertyChanged("ShowSignImgCommand");
                }
            }
        }

        public BitmapImage SubLeaderSignImg
        {
            get { return mSubLeaderSignImg; }
            set
            {
                if (mSubLeaderSignImg != value)
                {
                    mSubLeaderSignImg = value;
                    //防止多线程时访问异常
                    if (mSubLeaderSignImg != null)
                    {
                        mSubLeaderSignImg.Freeze();
                    }
                    RaisePropertyChanged("SubLeaderSignImg");
                }
            }
        }

        public BitmapImage DdzSignImg
        {
            get { return mDdzSignImg; }
            set
            {
                if (mDdzSignImg != value)
                {
                    mDdzSignImg = value;
                    //防止多线程时访问异常
                    if (mDdzSignImg != null)
                    {
                        mDdzSignImg.Freeze();
                    }
                    RaisePropertyChanged("DdzSignImg");
                }
            }
        }

        public bool IsBrowse
        {
            get { return mIsBrowse; }
            set
            {
                //if (mIsBrowse != value)
                //{
                    mIsBrowse = value;
                    RaisePropertyChanged("IsBrowse");
                    RaisePropertyChanged("IsBaseInfoReadOnly");
                    RaisePropertyChanged("IsBaseInfoEnabled");
                    RaisePropertyChanged("IsDdzInfoReadOnly");
                    RaisePropertyChanged("IsSubLeaderInfoReadOnly");
                    //RaisePropertyChanged("IsAdvancedInfoReadOnly");
                    RaisePropertyChanged("IsSubmitButtonVisible");
                    RaisePropertyChanged("IsRejectButtonVisible");
                    //RaisePropertyChanged("IsSuperviseInfoReadOnly");
                    RaisePropertyChanged("IsSubLeaderSignButtonVisible");
                    RaisePropertyChanged("IsDdzSignButtonVisible");
                    RaisePropertyChanged("IsSskInfoReadOnly");
                    RaisePropertyChanged("IsSubLeaderInfoVisible");
                    RaisePropertyChanged("IsDdzInfoVisible");
                    RaisePropertyChanged("IsSskInfoVisible");
                //}
            }
        }

        public bool IsBaseInfoReadOnly
        {
            get 
            {
                if (mEntity != null)
                {
                    return (mIsBrowse || mEntity.Status >= YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE);
                }
                else
                {
                    return mIsBrowse; 
                }
            }
        }

        public bool IsBaseInfoEnabled
        {
            get
            {
                return !IsBaseInfoReadOnly;
            }
        }

        public bool IsSubLeaderInfoReadOnly
        {
            get
            {
                if (mEntity != null)
                {
                    return (mIsBrowse || mEntity.Status >= YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE);
                }
                else
                {
                    return mIsBrowse;
                }
            }
        }

        public bool IsSubLeaderInfoVisible
        {
            get
            {
                if (mEntity != null)
                {
                    if (mIsBrowse)
                    {
                        return (mEntity.Status >= YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE);
                    }
                    else
                    {
                        return (mEntity.Status >= YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE);
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsDdzInfoReadOnly
        {
            get
            {
                if (mEntity != null)
                {
                    return (mIsBrowse || mEntity.Status >= YcConstants.INT_KJSS_REQSTAT_ZHZX_EXECUTE);
                }
                else
                {
                    return mIsBrowse;
                }
            }
        }

        public bool IsDdzInfoVisible
        {
            get
            {
                if (mEntity != null)
                {
                    if (mIsBrowse)
                    {
                        return mIsBrowse;
                    }
                    else
                    {
                        return (mEntity.Status >= YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE);
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /*
        public bool IsAdvancedInfoReadOnly
        {
            get
            {
                if (mEntity != null)
                {
                    return (mIsBrowse || mEntity.Status >= YcConstants.INT_ZHZX_REQSTAT_REQDEPT_APPROVE);
                }
                else
                {
                    return mIsBrowse;
                }
            }
        }

        public bool IsAdvancedInfoVisible
        {
            get
            {
                if (mEntity != null)
                {
                    return (mEntity.Status >= YcConstants.INT_ZHZX_REQSTAT_ZHZX_APPROVE);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsSuperviseInfoReadOnly
        {
            get
            {
                if (mEntity != null)
                {
                    return (mIsBrowse || mEntity.Status != YcConstants.INT_ZHZX_REQSTAT_DDZ_SUPERVISE);
                }
                else
                {
                    return mIsBrowse;
                }
            }
        }

        public bool IsSuperviseInfoVisible
        {
            get
            {
                if (mEntity != null)
                {
                    return (!ValidateUtil.IsBlank(mEntity.SuperviseCommnet)
                        || mEntity.Status == YcConstants.INT_ZHZX_REQSTAT_DDZ_SUPERVISE);
                }
                else
                {
                    return false;
                }
            }
        }
        */

        public bool IsSubLeaderSignButtonVisible
        {
            get
            {
                if (mIsBrowse)
                {
                    return false;
                }
                if (mEntity != null)
                {
                    if (mEntity.Status == YcConstants.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsDdzSignButtonVisible
        {
            get
            {
                if (mIsBrowse)
                {
                    return false;
                }
                if (mEntity != null)
                {
                    if (mEntity.Status == YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsSskInfoReadOnly
        {
            get
            {
                if (mEntity != null)
                {
                    return (mIsBrowse || mEntity.Status == YcConstants.INT_KJSS_REQSTAT_COMPLETED);
                }
                else
                {
                    return mIsBrowse;
                }
            }
        }

        public bool IsSskInfoVisible
        {
            get
            {
                if (mEntity != null)
                {
                    if (mIsBrowse)
                    {
                        return (mEntity.Status == YcConstants.INT_KJSS_REQSTAT_COMPLETED);
                    }
                    else
                    {
                        return (mEntity.Status >= YcConstants.INT_KJSS_REQSTAT_ZHZX_EXECUTE);
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsRejectButtonVisible
        {
            get
            {
                if (mIsBrowse)
                {
                    return false;
                }
                if (mEntity != null)
                {
                    if (mEntity.Id > 0 && mEntity.Status > YcConstants.INT_KJSS_REQSTAT_NULL)
                    {
                        return true;
                    }   
                }
                return false;
            }
        }

        public bool IsSubmitButtonVisible
        {
            get
            {
                return !mIsBrowse;
            }
        }

        public bool IsSuperviseButtonVisible
        {
            get { return mIsSuperviseButtonVisible; }
            set
            {
                if (mIsSuperviseButtonVisible != value)
                {
                    mIsSuperviseButtonVisible = value;
                    RaisePropertyChanged("IsSuperviseButtonVisible");
                }
            }
        }

        #endregion

        #region Members

        public void Show_LoadingMask()
        {
            ViewCore.Show_Loading();
        }

        public void Shutdown_LoadingMask()
        {
            ViewCore.Shutdown_Loading();
        }

        #endregion
    }
}
    