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
    public class CgsVehicleAdminUpdateViewModel : LbBaseUpdateViewModel<ICgsVehicleAdminUpdateView>
    {
        #region Data

        private CgsVehicleAdminCase mPunishCaseEntity;
        
        

        #endregion

        #region Constructors

        [ImportingConstructor]
        public CgsVehicleAdminUpdateViewModel(ICgsVehicleAdminUpdateView view)
            : base(view)
        {
            mPunishCaseEntity = new CgsVehicleAdminCase();
        }

        #endregion

        #region Properties

        public CgsVehicleAdminCase PunishCaseEntity
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

        private ICommand downloadCommand1;
        public ICommand DownloadCommand1
        {
            get { return downloadCommand1; }
            set
            {
                if (downloadCommand1 != value)
                {
                    downloadCommand1 = value;
                    RaisePropertyChanged("DownloadCommand1");
                }
            }
        }

        private ICommand downloadCommand2;
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

        private string fileLocalPath1;
        public string FileLocalPath1
        {
            get { return fileLocalPath1; }
            set
            {
                if (fileLocalPath1 != value)
                {
                    fileLocalPath1 = value;
                    RaisePropertyChanged("FileLocalPath1");
                }
            }
        }

        private string fileLocalPath2;
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

        private Visibility canUploadVisibal1;
        public Visibility CanUploadVisibal1
        {
            get { return canUploadVisibal1; }
            set
            {
                if (canUploadVisibal1 != value)
                {
                    canUploadVisibal1 = value;
                    RaisePropertyChanged("CanUploadVisibal1");
                }
            }
        }

        private Visibility canUploadVisibal2;
        public Visibility CanUploadVisibal2
        {
            get { return canUploadVisibal2; }
            set
            {
                if (canUploadVisibal2 != value)
                {
                    canUploadVisibal2 = value;
                    RaisePropertyChanged("CanUploadVisibal2");
                }
            }
        }

        private Visibility canDownloadVisibal1;
        public Visibility CanDownloadVisibal1
        {
            get { return canDownloadVisibal1; }
            set
            {
                if (canDownloadVisibal1 != value)
                {
                    canDownloadVisibal1 = value;
                    RaisePropertyChanged("CanDownloadVisibal1");
                }
            }
        }

        private Visibility canDownloadVisibal2;
        public Visibility CanDownloadVisibal2
        {
            get { return canDownloadVisibal2; }
            set
            {
                if (canDownloadVisibal2 != value)
                {
                    canDownloadVisibal2 = value;
                    RaisePropertyChanged("CanDownloadVisibal2");
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
    