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
using System.Windows.Media.Imaging;
using WafTraffic.Applications.Common;
using System.Windows;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class HotLineDealViewModel : ViewModel<IHotLineDealView>
    {
        private MayorHotlineTaskTable mHotLineTask;
        private List<BaseOrganizeEntity> mDepartmentList;

        //private ICommand saveCommand;
        private ICommand mSaveDealCommand;
        private ICommand mRetreatCommand;
        //private ICommand mShowPictrueCmd20;
        private ICommand mShowThumbCommand;
        private ICommand mShowContentCommand;
        private BitmapImage mContentImg;

        [ImportingConstructor]
        public HotLineDealViewModel(IHotLineDealView view)
            : base(view)
        {
            mHotLineTask = new MayorHotlineTaskTable();
            mDepartmentList = new List<BaseOrganizeEntity>();

            BaseOrganizeManager origanizeService = new BaseOrganizeManager();
            DataTable departmentDT = origanizeService.GetDepartmentDT(""); //根节点 parnetid
            BaseOrganizeEntity entity;
            foreach (DataRow dr in departmentDT.Rows)
            {
                entity = new BaseOrganizeEntity(dr);
                mDepartmentList.Add(entity);
            }
        }

        public BitmapImage ContentImg
        {
            get { return mContentImg; }
            set
            {
                if (mContentImg != value)
                {
                    mContentImg = value;
                    //防止多线程时访问异常
                    if (mContentImg != null)
                    {
                        mContentImg.Freeze();
                    }
                    RaisePropertyChanged("ContentImg");
                }
            }
        }

        public ICommand ShowThumbCommand
        {
            get { return mShowThumbCommand; }
            set
            {
                if (mShowThumbCommand != value)
                {
                    mShowThumbCommand = value;
                    RaisePropertyChanged("ShowThumbCommand");
                }
            }
        }

        public ICommand ShowContentCommand
        {
            get { return mShowContentCommand; }
            set
            {
                if (mShowContentCommand != value)
                {
                    mShowContentCommand = value;
                    RaisePropertyChanged("ShowContentCommand");
                }
            }
        }

        public MayorHotlineTaskTable HotLineTask
        {
            get { return mHotLineTask; }
            set
            {
                if (mHotLineTask != value)
                {
                    mHotLineTask = value;
                    RaisePropertyChanged("HotLineTask");
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

        public ICommand SaveDealCommand
        {
            get { return mSaveDealCommand; }
            set
            {
                if (mSaveDealCommand != value)
                {
                    mSaveDealCommand = value;
                    RaisePropertyChanged("SaveDealCommand");
                }
            }
        }

        public ICommand RetreatCommand
        {
            get { return mRetreatCommand; }
            set
            {
                if (mRetreatCommand != value)
                {
                    mRetreatCommand = value;
                    RaisePropertyChanged("RetreatCommand");
                }
            }
        }

        private ICommand displayCommand;
        public ICommand DisplayCommand
        {
            get { return displayCommand; }
            set
            {
                if (displayCommand != value)
                {
                    displayCommand = value;
                    RaisePropertyChanged("DisplayCommand");
                }
            }
        }

        private ICommand downloadCommand;
        public ICommand DownloadCommand
        {
            get { return downloadCommand; }
            set
            {
                if (downloadCommand != value)
                {
                    downloadCommand = value;
                    RaisePropertyChanged("DownloadCommand");
                }
            }
        }

        private Visibility canContentFileDownloadVisibal;
        public Visibility CanContentFileDownloadVisibal
        {
            get { return canContentFileDownloadVisibal; }
            set
            {
                if (canContentFileDownloadVisibal != value)
                {
                    canContentFileDownloadVisibal = value;
                    RaisePropertyChanged("CanContentFileDownloadVisibal");
                }
            }
        }

        private HotLineFileType fileType;
        public HotLineFileType FileType
        {
            get { return fileType; }
            set
            {
                if (fileType != value)
                {
                    fileType = value;
                }
            }
        }

        /*
        public ICommand ShowPictrueCmd20
        {
            get { return mShowPictrueCmd20; }
            set
            {
                if (mShowPictrueCmd20 != value)
                {
                    mShowPictrueCmd20 = value;
                    RaisePropertyChanged("ShowPictrueCmd20");
                }
            }
        }
        */

        //public ICommand SaveCommand
        //{
        //    get { return saveCommand; }
        //    set
        //    {
        //        if (saveCommand != value)
        //        {
        //            saveCommand = value;
        //            RaisePropertyChanged("SaveCommand");
        //        }
        //    }
        //}

        #region Members

        public void ShowLoadingMask(LoadingType type)
        {
            ViewCore.ShowLoading(type);
        }

        public void ShutdownLoadingMask(LoadingType type)
        {
            ViewCore.ShutdownLoading(type);
        }

        #endregion

    }
}
