using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;
using System.Windows.Media.Imaging;
using WafTraffic.Applications.Common;
using System.Windows;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class HotLineCheckViewModel : ViewModel<IHotLineCheckView>
    {
        private MayorHotlineTaskTable mHotLineTask;
        private ICommand mSaveCheckCommand;
        private ICommand mRetreatCommand;
        private ICommand mShowContentCommand;
        private BitmapImage mContentImg;
        private string mVerifyLocalPath;
        //private ICommand mShowChkPicCmd21;
        //private ICommand mShowPictrueCmd20;

        [ImportingConstructor]
        public HotLineCheckViewModel(IHotLineCheckView view)
            : base(view)
        {
            mHotLineTask = new MayorHotlineTaskTable();
        }

        public string VerifyLocalPath
        {
            get { return mVerifyLocalPath; }
            set
            {
                if (mVerifyLocalPath != value)
                {
                    mVerifyLocalPath = value;
                    RaisePropertyChanged("VerifyLocalPath");
                }
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

        //public ICommand ShowThumbCommand
        //{
        //    get { return mShowThumbCommand; }
        //    set
        //    {
        //        if (mShowThumbCommand != value)
        //        {
        //            mShowThumbCommand = value;
        //            RaisePropertyChanged("ShowThumbCommand");
        //        }
        //    }
        //}

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

        public ICommand SaveCheckCommand
        {
            get { return mSaveCheckCommand; }
            set
            {
                if (mSaveCheckCommand != value)
                {
                    mSaveCheckCommand = value;
                    RaisePropertyChanged("SaveCheckCommand");
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

        public ICommand ShowChkPicCmd21
        {
            get { return mShowChkPicCmd21; }
            set
            {
                if (mShowChkPicCmd21 != value)
                {
                    mShowChkPicCmd21 = value;
                    RaisePropertyChanged("ShowChkPicCmd21");
                }
            }
        }
        */

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
