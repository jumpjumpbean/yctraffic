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
    public class FzkPunishUpdateViewModel : LbBaseUpdateViewModel<IFzkPunishUpdateView>
    {
        #region Data

        private FzkPunishCase mPunishCaseEntity;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public FzkPunishUpdateViewModel(IFzkPunishUpdateView view)
            : base(view)
        {
            mPunishCaseEntity = new FzkPunishCase();
        }

        #endregion

        #region Properties

        public FzkPunishCase PunishCaseEntity
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

        private string fileLocalPath;
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

        private Visibility canUploadVisibal;
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

        private Visibility canDownloadVisibal;
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
    