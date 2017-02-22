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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class LbStaticLogbookDetailsViewModel : ViewModel<ILbStaticLogbookDetailsView>
    {
        #region Data

        private ZdtzStaticTable mStaticLogbookEntity;
        private ICommand mDisplayCommand;
        private ICommand mCancelCommand;
        private ICommand mDownloadCommand;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbStaticLogbookDetailsViewModel(ILbStaticLogbookDetailsView view)
            : base(view)
        {
            mStaticLogbookEntity = new ZdtzStaticTable();
        }

        #endregion

        #region Properties

        public ZdtzStaticTable StaticLogbookEntity
        {
            get { return mStaticLogbookEntity; }
            set
            {
                if (mStaticLogbookEntity != value)
                {
                    mStaticLogbookEntity = value;
                    RaisePropertyChanged("StaticLogbookEntity");
                }
            }
        }

        public ICommand DisplayCommand
        {
            get { return mDisplayCommand; }
            set
            {
                if (mDisplayCommand != value)
                {
                    mDisplayCommand = value;
                    RaisePropertyChanged("DisplayCommand");
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
            get { return mDownloadCommand; }
            set
            {
                if (mDownloadCommand != value)
                {
                    mDownloadCommand = value;
                    RaisePropertyChanged("DownloadCommand");
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
    