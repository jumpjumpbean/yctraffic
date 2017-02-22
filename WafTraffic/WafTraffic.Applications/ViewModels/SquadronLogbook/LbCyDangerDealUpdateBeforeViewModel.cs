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
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class LbCyDangerDealUpdateBeforeViewModel : ViewModel<ILbCyDangerDealUpdateBeforeView>
    {
        #region Data

        private ZdtzCyDangerDeal mDangerDealEntity;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;
        private bool mIsBrowse;
        private string mContentLocalPath;
        private string mRectificationLocalPath;
        private string mReviewLocalPath;
        private List<BaseUserEntity> mLeaderList = null;
        //private int mSelectedSubLeader = 0;
        private List<BaseOrganizeEntity> mDepartmentList = null;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbCyDangerDealUpdateBeforeViewModel(ILbCyDangerDealUpdateBeforeView view)
            : base(view)
        {
            mDangerDealEntity = new ZdtzCyDangerDeal();
        }

        #endregion

        #region Properties

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

        public string ContentLocalPath
        {
            get { return mContentLocalPath; }
            set
            {
                if (mContentLocalPath != value)
                {
                    mContentLocalPath = value;
                    RaisePropertyChanged("ContentLocalPath");
                }
            }
        }

        public string RectificationLocalPath
        {
            get { return mRectificationLocalPath; }
            set
            {
                if (mRectificationLocalPath != value)
                {
                    mRectificationLocalPath = value;
                    RaisePropertyChanged("RectificationLocalPath");
                }
            }
        }

        public string ReviewLocalPath
        {
            get { return mReviewLocalPath; }
            set
            {
                if (mReviewLocalPath != value)
                {
                    mReviewLocalPath = value;
                    RaisePropertyChanged("ReviewLocalPath");
                }
            }
        }

        private ICommand contentDownloadCommand;
        public ICommand ContentDownloadCommand
        {
            get { return contentDownloadCommand; }
            set
            {
                if (contentDownloadCommand != value)
                {
                    contentDownloadCommand = value;
                    RaisePropertyChanged("ContentDownloadCommand");
                }
            }
        }

        public ZdtzCyDangerDeal DangerDealEntity
        {
            get { return mDangerDealEntity; }
            set
            {
                //if (mDangerDealEntity != value)
                //{
                    mDangerDealEntity = value;
                    RaisePropertyChanged("DangerDealEntity");
                    RaisePropertyChanged("IsBaseInfoReadOnly");
                    RaisePropertyChanged("IsBaseInfoEnabled");
                    RaisePropertyChanged("IsContentDownloadVisible");
                    RaisePropertyChanged("IsContentUploadVisible");
                    RaisePropertyChanged("IsZxkInfoEnabled");
                    RaisePropertyChanged("IsZxkInfoVisible");
                    RaisePropertyChanged("IsRectificationInputVisible");
                    RaisePropertyChanged("IsRectificationImgVisible");
                    RaisePropertyChanged("IsSjzxVerifyInfoVisible");
                    RaisePropertyChanged("IsSjzxInfoReadOnly");
                    RaisePropertyChanged("IsZxkInfoReadOnly");
                    RaisePropertyChanged("IsSubLeaderInfoReadOnly");
                    RaisePropertyChanged("IsSubLeaderInfoVisible");
                    RaisePropertyChanged("IsSubLeaderSignButtonVisible");
                //}
            }
        }

        public ICommand SaveCommand
        {
            get { return mSaveCommand; }
            set
            {
                if (mSaveCommand != value)
                {
                    mSaveCommand = value;
                    RaisePropertyChanged("SaveCommand");
                }
            }
        }

        public bool IsBrowse
        {
            get { return mIsBrowse; }
            set
            {
                mIsBrowse = value;
                RaisePropertyChanged("IsBrowse");
                RaisePropertyChanged("IsBaseInfoReadOnly");
                RaisePropertyChanged("IsBaseInfoEnabled");
                RaisePropertyChanged("IsContentDownloadVisible");
                RaisePropertyChanged("IsContentUploadVisible");
                RaisePropertyChanged("IsZxkInfoEnabled");
                RaisePropertyChanged("IsZxkInfoVisible");
                RaisePropertyChanged("IsRectificationInputVisible");
                RaisePropertyChanged("IsRectificationImgVisible");
                RaisePropertyChanged("IsSjzxVerifyInfoVisible");
                RaisePropertyChanged("IsSjzxInfoReadOnly");
                RaisePropertyChanged("IsZxkInfoReadOnly");
                //RaisePropertyChanged("IsReviewImgVisible");
                RaisePropertyChanged("IsSaveButtonVisible");
                RaisePropertyChanged("IsPrintButtonVisible");
                RaisePropertyChanged("IsSubLeaderInfoReadOnly");
                RaisePropertyChanged("IsSubLeaderInfoVisible");
                RaisePropertyChanged("IsSubLeaderSignButtonVisible");
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

        public bool IsBaseInfoReadOnly
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    return (mIsBrowse || mDangerDealEntity.Status >= YcConstants.INT_DANGER_DEAL_SATUS_NEW);
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

        public bool IsContentDownloadVisible
        {
            get
            {               
                if (mDangerDealEntity != null)
                {
                    if (!string.IsNullOrEmpty(mDangerDealEntity.ContentImgName) && mIsBrowse)    
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsContentUploadVisible
        {
            get
            {
                if (mIsBrowse)
                {
                    return false;
                }
                return !IsContentDownloadVisible;
            }
        }

        //public bool IsContetnImgVisible
        //{
        //    get
        //    {
        //        return !IsContentInputVisible;
        //    }
        //}

        public bool IsZxkInfoReadOnly
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    return (mIsBrowse || mDangerDealEntity.Status >= YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE);
                }
                else
                {
                    return mIsBrowse;
                }
            }
        }

        public bool IsZxkInfoEnabled
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    return !(mIsBrowse || mDangerDealEntity.Status >= YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE);
                }
                else
                {
                    return !mIsBrowse;
                }
            }
        }

        public bool IsZxkInfoVisible
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    if (mIsBrowse)
                    {
                        return (mDangerDealEntity.Status >= YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE);
                    }
                    else
                    {
                        return (mDangerDealEntity.Status >= YcConstants.INT_DANGER_DEAL_SATUS_NEW);
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsRectificationInputVisible
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    return (!mIsBrowse && mDangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_NEW);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsRectificationImgVisible
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    return (mDangerDealEntity.Status > YcConstants.INT_DANGER_DEAL_SATUS_NEW);
                    //return (mDangerDealEntity.Status > YcConstants.INT_DANGER_DEAL_SATUS_NEW 
                      //  || (mIsBrowse && mDangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_NEW));
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsSubLeaderInfoReadOnly
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    return (mIsBrowse || mDangerDealEntity.Status >= YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY);
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
                if (mDangerDealEntity != null)
                {
                    if (mIsBrowse)
                    {
                        return (mDangerDealEntity.Status >= YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY);
                    }
                    else
                    {
                        return (mDangerDealEntity.Status >= YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE);
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsSubLeaderSignButtonVisible
        {
            get
            {
                if (mIsBrowse)
                {
                    return false;
                }
                if (mDangerDealEntity != null)
                {
                    if (mDangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsSjzxInfoReadOnly
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    return (mIsBrowse || mDangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_END);
                }
                else
                {
                    return mIsBrowse;
                }
            }
        }

        public bool IsSjzxVerifyInfoVisible
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    if (mIsBrowse)
                    {
                        return (mDangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_END);
                    }
                    else
                    {
                        return (mDangerDealEntity.Status >= YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY);
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /*
        public bool IsReviewInputVisible
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    return (!mIsBrowse && mDangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsReviewImgVisible
        {
            get
            {
                if (mDangerDealEntity != null)
                {
                    return (mDangerDealEntity.Status > YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY);
                    //return (mDangerDealEntity.Status > YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY
                      //  || (mIsBrowse && mDangerDealEntity.Status == YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY));
                }
                else
                {
                    return false;
                }
            }
        }
        */

        public bool IsSaveButtonVisible
        {
            get
            {
                return !mIsBrowse;
            }
        }

        public bool IsPrintButtonVisible
        {
            get
            {
                return mIsBrowse;
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

        //public BitmapImage SubLeaderSignImg
        //{
        //    get { return mSubLeaderSignImg; }
        //    set
        //    {
        //        if (mSubLeaderSignImg != value)
        //        {
        //            mSubLeaderSignImg = value;
        //            //防止多线程时访问异常
        //            if (mSubLeaderSignImg != null)
        //            {
        //                mSubLeaderSignImg.Freeze();
        //            }
        //            RaisePropertyChanged("SubLeaderSignImg");
        //        }
        //    }
        //}

        #endregion

        #region Members

        public void Show_LoadingMask(LoadingType type)
        {
            ViewCore.Show_Loading(type);
        }

        public void Shutdown_LoadingMask(LoadingType type)
        {
            ViewCore.Shutdown_Loading(type);
        }

        #endregion
    }
}
    