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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ZxkOrderUpdateViewModel : LbBaseUpdateViewModel<IZxkOrderUpdateView>
    {
        #region Data

        private ZxkOrderCase mPunishCaseEntity;
        private string fileLocalPath;
        private string fileLocalPath2;
        private Visibility canUploadVisibal;
        private Visibility canDownloadVisibal;
        private ICommand downloadCommand;
        private ICommand downloadCommand2;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public ZxkOrderUpdateViewModel(IZxkOrderUpdateView view)
            : base(view)
        {
            mPunishCaseEntity = new ZxkOrderCase();
        }

        #endregion

        #region Properties

        public ZxkOrderCase PunishCaseEntity
        {
            get { return mPunishCaseEntity; }
            set
            {
                if (mPunishCaseEntity != value)
                {
                    mPunishCaseEntity = value;
                    RaisePropertyChanged("PunishCaseEntity");
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

        public ICommand DownloadCommand2
        {
            get { return downloadCommand2; }
            set
            {
                if (downloadCommand2 != value)
                {
                    downloadCommand2 = value;
                    RaisePropertyChanged("DownloadCommand2");
                }
            }
        }

        public string FileLocalPath
        {
            get { return fileLocalPath; }
            set
            {
                if (fileLocalPath != value)
                {
                    fileLocalPath = value;
                    RaisePropertyChanged("FileLocalPath");
                }
            }
        }

        public string FileLocalPath2
        {
            get { return fileLocalPath2; }
            set
            {
                if (fileLocalPath2 != value)
                {
                    fileLocalPath2 = value;
                    RaisePropertyChanged("FileLocalPath2");
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

        public void Shutdown_LoadingMask(LoadingType type)
        {
            ViewCore.Shutdown_Loading(type);
        }

        public void Show_LoadingMask(LoadingType type)
        {
            ViewCore.Show_Loading(type);
        }

        #endregion
    }
}
    