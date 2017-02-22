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
    public class HotLineShowViewModel : ViewModel<IHotLineShowView>
    {
        private MayorHotlineTaskTable mHotLineTask;
        private ICommand mRetreatCommand;
        private ICommand mShowContentCommand;
        private ICommand mShowVerifyCommand;
        private BitmapImage mContentImg;
        private BitmapImage mVerifyImg;
        //private ICommand showPictrueCmd20;
        //private ICommand showChkPicCmd21;

        [ImportingConstructor]
        public HotLineShowViewModel(IHotLineShowView view)
            : base(view)
        {
            mHotLineTask = new MayorHotlineTaskTable();
        }

        /*
        public ICommand ShowPictrueCmd20
        {
            get { return showPictrueCmd20; }
            set
            {
                if (showPictrueCmd20 != value)
                {
                    showPictrueCmd20 = value;
                    RaisePropertyChanged("ShowPictrueCmd20");
                }
            }
        }

        public ICommand ShowChkPicCmd21
        {
            get { return showChkPicCmd21; }
            set
            {
                if (showChkPicCmd21 != value)
                {
                    showChkPicCmd21 = value;
                    RaisePropertyChanged("ShowChkPicCmd21");
                }
            }
        }
        */

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

        public BitmapImage VerifyImg
        {
            get { return mVerifyImg; }
            set
            {
                if (mVerifyImg != value)
                {
                    mVerifyImg = value;
                    //防止多线程时访问异常
                    if (mVerifyImg != null)
                    {
                        mVerifyImg.Freeze();
                    }
                    RaisePropertyChanged("VerifyImg");
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

        private Visibility canVerifyFileDownloadVisibal;
        public Visibility CanVerifyFileDownloadVisibal
        {
            get { return canVerifyFileDownloadVisibal; }
            set
            {
                if (canVerifyFileDownloadVisibal != value)
                {
                    canVerifyFileDownloadVisibal = value;
                    RaisePropertyChanged("CanVerifyFileDownloadVisibal");
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

        public ICommand ShowVerifyCommand
        {
            get { return mShowVerifyCommand; }
            set
            {
                if (mShowVerifyCommand != value)
                {
                    mShowVerifyCommand = value;
                    RaisePropertyChanged("ShowVerifyCommand");
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

    public enum HotLineFileType
    { 
        None,
        ContentFile,
        VerifyFile   
    }
}
