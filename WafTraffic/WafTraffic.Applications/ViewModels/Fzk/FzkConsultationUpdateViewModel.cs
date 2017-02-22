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
    public class FzkConsultationUpdateViewModel : ViewModel<IFzkConsultationUpdateView>
    {
        #region Data

        private FzkConsultation consultationEntity;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;

        private Visibility canSaveVisibal;

        private bool isRecordDateReadOnly;
        private bool isTitleReadOnly;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public FzkConsultationUpdateViewModel(IFzkConsultationUpdateView view)
            : base(view)
        {
            consultationEntity = new FzkConsultation();
        }

        #endregion

        #region Properties

        public FzkConsultation ConsultationEntity
        {
            get { return consultationEntity; }
            set
            {
                if (consultationEntity != value)
                {
                    consultationEntity = value;
                    RaisePropertyChanged("ConsultationEntity");
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


    }
}
    