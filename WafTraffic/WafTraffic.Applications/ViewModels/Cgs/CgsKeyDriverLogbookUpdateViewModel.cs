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
    public class CgsKeyDriverLogbookUpdateViewModel : ViewModel<ICgsKeyDriverLogbookUpdateView>
    {
        #region Data

        private CgsKeyDriverLogbook keyDriverLogbookEntity;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;

        private Visibility canSaveVisibal;

        private bool canGovCarEnable;
        private bool isTitleReadOnly;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public CgsKeyDriverLogbookUpdateViewModel(ICgsKeyDriverLogbookUpdateView view)
            : base(view)
        {
            keyDriverLogbookEntity = new CgsKeyDriverLogbook();
        }

        #endregion

        #region Properties

        public CgsKeyDriverLogbook KeyDriverLogbookEntity
        {
            get { return keyDriverLogbookEntity; }
            set
            {
                if (keyDriverLogbookEntity != value)
                {
                    keyDriverLogbookEntity = value;
                    RaisePropertyChanged("KeyDriverLogbookEntity");
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
    