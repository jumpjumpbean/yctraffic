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
    public class CgsKeyVehicleLogbookUpdateViewModel : ViewModel<ICgsKeyVehicleLogbookUpdateView>
    {
        #region Data

        private CgsKeyVehicleLogbook keyVehicleLogbookEntity;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;

        private Visibility canSaveVisibal;

        private bool canGovCarEnable;
        private bool isTitleReadOnly;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public CgsKeyVehicleLogbookUpdateViewModel(ICgsKeyVehicleLogbookUpdateView view)
            : base(view)
        {
            keyVehicleLogbookEntity = new CgsKeyVehicleLogbook();
        }

        #endregion

        #region Properties

        public CgsKeyVehicleLogbook KeyVehicleLogbookEntity
        {
            get { return keyVehicleLogbookEntity; }
            set
            {
                if (keyVehicleLogbookEntity != value)
                {
                    keyVehicleLogbookEntity = value;
                    RaisePropertyChanged("KeyVehicleLogbookEntity");
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

        public bool CanGovCarEnable
        {
            get { return canGovCarEnable; }
            set
            {
                if (canGovCarEnable != value)
                {
                    canGovCarEnable = value;
                    RaisePropertyChanged("CanGovCarEnable");
                }
            }
        }

        #endregion


    }
}
    