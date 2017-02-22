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

namespace WafTraffic.Applications.ViewModels
{ 
    [Export]
    public class HealthArchiveViewModel : ViewModel<IHealthArchiveView>
    {
        private HealthArchiveTable mHealthArchive;
        private ICommand mSaveCommand;
        private ICommand mRetreatCommand;
        private string mOperation;

        [ImportingConstructor]
        public HealthArchiveViewModel(IHealthArchiveView view)
            : base(view)
        {
            mHealthArchive = new HealthArchiveTable();

        }

        //public string CheckFileLocalPath
        //{
        //    get { return mCheckFileLocalPath; }
        //    set
        //    {
        //        if (mCheckFileLocalPath != value)
        //        {
        //            mCheckFileLocalPath = value;
        //            RaisePropertyChanged("CheckFileLocalPath");
        //        }
        //    }
        //}

        //public BitmapImage CheckFileImg
        //{
        //    get { return mCheckFileImg; }
        //    set
        //    {
        //        if (mCheckFileImg != value)
        //        {
        //            mCheckFileImg = value;
        //            //防止多线程时访问异常
        //            if (mCheckFileImg != null)
        //            {
        //                mCheckFileImg.Freeze();
        //            }
        //            RaisePropertyChanged("CheckFileImg");
        //        }
        //    }
        //}

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

        //public ICommand ShowCheckFileCommand
        //{
        //    get { return mShowCheckFileCommand; }
        //    set
        //    {
        //        if (mShowCheckFileCommand != value)
        //        {
        //            mShowCheckFileCommand = value;
        //            RaisePropertyChanged("ShowCheckFileCommand");
        //        }
        //    }
        //}

        public String Operation
        {
            get { return mOperation; }
            set
            {
                if (mOperation != value)
                {
                    mOperation = value;
                    RaisePropertyChanged("Operation");
                }
            }
        }

         public HealthArchiveTable HealthArchive
         {
             get { return mHealthArchive; }
             set
             {
                 if (mHealthArchive != value)
                 {
                     mHealthArchive = value;
                     RaisePropertyChanged("HealthArchive");
                 }
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
    }
}
    