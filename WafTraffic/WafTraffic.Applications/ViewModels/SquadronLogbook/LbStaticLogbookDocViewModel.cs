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
    public class LbStaticLogbookDocViewModel : ViewModel<ILbStaticLogbookDocView>
    {
        #region Data

        private string mFilePath;
        private ICommand mCloseCommand;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbStaticLogbookDocViewModel(ILbStaticLogbookDocView view)
            : base(view)
        {
        }

        #endregion

        #region Properties

        public string FilePath
        {
            get { return mFilePath; }
            set
            {
                if (mFilePath != value)
                {
                    mFilePath = value;
                    RaisePropertyChanged("FilePath");
                }
            }
        }

        public ICommand CloseCommand
        {
            get { return mCloseCommand; }
            set
            {
                if (mCloseCommand != value)
                {
                    mCloseCommand = value;
                    RaisePropertyChanged("CloseCommand");
                }
            }
        }

        public void Close()
        {
            ViewCore.Close();
        }

        #endregion
    }
}
    