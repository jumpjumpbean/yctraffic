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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class LbBaseUpdateViewModel<TView> : ViewModel<TView> where TView : global::System.Waf.Applications.IView
    {
        #region Data

        private ICommand mSaveCommand;
        private ICommand mCancelCommand;
        private bool mIsBrowse;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbBaseUpdateViewModel(TView view)
            : base(view)
        {
            mIsBrowse = false;
        }

        #endregion

        #region Properties

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

        public bool IsBrowse
        {
            get { return mIsBrowse; }
            set
            {
                if (mIsBrowse != value)
                {
                    mIsBrowse = value;
                    RaisePropertyChanged("IsBrowse");
                    RaisePropertyChanged("IsNewOrModify");
                    RaisePropertyChanged("BrowseVisibility");
                    RaisePropertyChanged("NewOrModifyVisibility");
                }
            }
        }

        public bool IsNewOrModify
        {
            get { return !mIsBrowse; }
        }

        public Visibility BrowseVisibility
        {
            get {
                if (mIsBrowse)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }

        public Visibility NewOrModifyVisibility
        {
            get
            {
                if (mIsBrowse)
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        #endregion
    }
}
