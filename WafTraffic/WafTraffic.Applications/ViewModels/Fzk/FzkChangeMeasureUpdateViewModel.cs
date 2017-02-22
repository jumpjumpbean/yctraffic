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
    public class FzkChangeMeasureUpdateViewModel : ViewModel<IFzkChangeMeasureUpdateView>
    {
        #region Data

        private FzkChangeMeasure changeMeasureEntity;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;
        private ICommand downloadCommand;
        private ICommand approveSaveCommand;

        private Visibility canSaveVisibal;
        private Visibility canApproveVisibal;

        private bool isBaseInfoReadOnly;
        private bool isApproveInfoReadOnly;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public FzkChangeMeasureUpdateViewModel(IFzkChangeMeasureUpdateView view)
            : base(view)
        {
            changeMeasureEntity = new FzkChangeMeasure();
        }

        #endregion

        #region Properties

        public FzkChangeMeasure ChangeMeasureEntity
        {
            get { return changeMeasureEntity; }
            set
            {
                if (changeMeasureEntity != value)
                {
                    changeMeasureEntity = value;
                    RaisePropertyChanged("ChangeMeasureEntity");
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

        public ICommand ApproveSaveCommand
        {
            get { return approveSaveCommand; }
            set
            {
                if (approveSaveCommand != value)
                {
                    approveSaveCommand = value;
                    RaisePropertyChanged("ApproveSaveCommand");
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

        public Visibility CanApproveVisibal
        {
            get { return canApproveVisibal; }
            set
            {
                if (canApproveVisibal != value)
                {
                    canApproveVisibal = value;
                    RaisePropertyChanged("CanApproveVisibal");
                }
            }
        }


        public bool IsBaseInfoReadOnly
        {
            get { return isBaseInfoReadOnly; }
            set
            {
                if (isBaseInfoReadOnly != value)
                {
                    isBaseInfoReadOnly = value;
                    RaisePropertyChanged("IsBaseInfoReadOnly");
                }
            }
        }

        public bool IsApproveInfoReadOnly
        {
            get { return isApproveInfoReadOnly; }
            set
            {
                if (isApproveInfoReadOnly != value)
                {
                    isApproveInfoReadOnly = value;
                    RaisePropertyChanged("IsApproveInfoReadOnly");
                }
            }
        }

        #endregion

        #region Members

        //public void Show_LoadingMask()
        //{
        //    ViewCore.Show_Loading();
        //}

        //public void Shutdown_LoadingMask()
        //{
        //    ViewCore.Shutdown_Loading();
        //}

        #endregion
    }
}
    