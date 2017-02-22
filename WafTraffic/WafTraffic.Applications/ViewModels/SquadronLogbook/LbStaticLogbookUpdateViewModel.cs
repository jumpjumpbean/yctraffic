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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class LbStaticLogbookUpdateViewModel : ViewModel<ILbStaticLogbookUpdateView>
    {
        #region Data

        private ZdtzStaticTable mStaticLogbookEntity;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;
        private ICommand mUploadCommand;
        private string mUploadFullPath;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbStaticLogbookUpdateViewModel(ILbStaticLogbookUpdateView view)
            : base(view)
        {
            mStaticLogbookEntity = new ZdtzStaticTable();
        }

        #endregion

        #region Properties

        public ZdtzStaticTable StaticLogbookEntity
        {
            get { return mStaticLogbookEntity; }
            set
            {
                if (mStaticLogbookEntity != value)
                {
                    mStaticLogbookEntity = value;
                    RaisePropertyChanged("StaticLogbookEntity");
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

        public ICommand UploadCommand
        {
            get { return mUploadCommand; }
            set
            {
                if (mUploadCommand != value)
                {
                    mUploadCommand = value;
                    RaisePropertyChanged("UploadCommand");
                }
            }
        }

        public string UploadFullPath
        {
            get { return mUploadFullPath; }
            set
            {
                if (mUploadFullPath != value)
                {
                    mUploadFullPath = value;
                    RaisePropertyChanged("UploadFullPath");
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
    