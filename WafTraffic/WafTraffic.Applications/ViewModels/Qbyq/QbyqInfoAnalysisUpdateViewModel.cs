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
    public class QbyqInfoAnalysisUpdateViewModel : LbBaseUpdateViewModel<IQbyqInfoAnalysisUpdateView>
    {
        #region Data

        private QbyqInfoAnalysisCase mPunishCaseEntity;
        private string fileLocalPath;
        private Visibility canUploadVisibal;
        private Visibility canDownloadVisibal;
        private ICommand downloadCommand;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public QbyqInfoAnalysisUpdateViewModel(IQbyqInfoAnalysisUpdateView view)
            : base(view)
        {
            mPunishCaseEntity = new QbyqInfoAnalysisCase();
        }

        #endregion

        #region Properties

        public QbyqInfoAnalysisCase PunishCaseEntity
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
    