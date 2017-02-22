using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using System.Windows;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;
using DotNet.Business;
using System.Data;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ZgxcAssistantCheckinUpdateViewModel : ViewModel<IZgxcAssistantCheckinUpdateView>
    {
        #region Data

        private ZgxcAssistantCheckin publicityLogbookEntity;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;
        private ICommand downloadCommand;
        private string mUploadFullPath;

        private Visibility canUploadVisibal;
        private Visibility canDownloadVisibal;
        private Visibility canSaveVisibal;

        private bool isRecordDateReadOnly;
        private bool isTitleReadOnly;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public ZgxcAssistantCheckinUpdateViewModel(IZgxcAssistantCheckinUpdateView view)
            : base(view)
        {
            publicityLogbookEntity = new ZgxcAssistantCheckin();
        }

        #endregion

        #region Properties

        public ZgxcAssistantCheckin AssistantCheckinEntity
        {
            get { return publicityLogbookEntity; }
            set
            {
                if (publicityLogbookEntity != value)
                {
                    publicityLogbookEntity = value;
                    RaisePropertyChanged("AssistantCheckinEntity");
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

        public Visibility CanUploadVisibal
        {
            get { return canUploadVisibal; }
            set
            {
                if (canUploadVisibal != value)
                {
                    canUploadVisibal = value;
                    RaisePropertyChanged("CanUploadVisibal");
                }
            }
        }

        public Visibility CanDownloadVisibal
        {
            get { return canDownloadVisibal; }
            set
            {
                if (canDownloadVisibal != value)
                {
                    canDownloadVisibal = value;
                    RaisePropertyChanged("CanDownloadVisibal");
                }
            }
        }

        public Visibility CanSaveVisibal
        {
            get { return canSaveVisibal; }
            set
            {
                if (canSaveVisibal != value)
                {
                    canSaveVisibal = value;
                    RaisePropertyChanged("CanSaveVisibal");
                }
            }
        }

        public bool IsRecordDateReadOnly
        {
            get { return isRecordDateReadOnly; }
            set
            {
                if (isRecordDateReadOnly != value)
                {
                    isRecordDateReadOnly = value;
                    RaisePropertyChanged("IsRecordDateReadOnly");
                }
            }
        }

        public bool IsTitleReadOnly
        {
            get { return isTitleReadOnly; }
            set
            {
                if (isTitleReadOnly != value)
                {
                    isTitleReadOnly = value;
                    RaisePropertyChanged("IsTitleReadOnly");
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
    