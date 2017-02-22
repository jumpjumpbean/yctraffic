using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System.Windows.Input;
using WafTraffic.Applications.Services;
using DotNet.Business;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class LbConfigQueryViewModel : ViewModel<ILbConfigQueryView>
    {
        #region Data

        private ZdtzConfigTable mSelectedConfig;
        private IQueryable<ZdtzConfigTable> mLbConfigs;

        private ICommand mNewCommand;
        private ICommand mModifyCommand;
        private ICommand mDeleteCommand;
        private ICommand mQueryCommand;
        private ICommand mBrowseCommand;
        private string mInputTitle;
        IEntityService mEntityservice;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbConfigQueryViewModel(ILbConfigQueryView view, IEntityService entityservice)
            : base(view)
        {
            try
            {
                this.mEntityservice = entityservice;
                if (entityservice != null)
                {
                    this.mLbConfigs = entityservice.EnumLbConfigs;
                }
                else
                {
                    this.mLbConfigs = new List<ZdtzConfigTable>().AsQueryable();
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

        }

        #endregion

        #region Properties

        public IQueryable<ZdtzConfigTable> LbConfigs
        {
            get
            {
                return mLbConfigs;
            }
            set
            {
                mLbConfigs = value;
                RaisePropertyChanged("LbConfigs");
            }
        }

        public ZdtzConfigTable SelectedConfig
        {
            get { return mSelectedConfig; }
            set
            {
                if (mSelectedConfig != value)
                {
                    mSelectedConfig = value;
                    RaisePropertyChanged("SelectedConfig");
                }
            }
        }

        public string InputTitle
        {
            get { return mInputTitle; }
            set
            {
                if (mInputTitle != value)
                {
                    mInputTitle = value;
                    RaisePropertyChanged("InputTitle");
                }
            }
        }

        public ICommand NewCommand
        {
            get { return mNewCommand; }
            set
            {
                if (mNewCommand != value)
                {
                    mNewCommand = value;
                    RaisePropertyChanged("NewCommand");
                }
            }
        }

        public ICommand ModifyCommand
        {
            get { return mModifyCommand; }
            set
            {
                if (mModifyCommand != value)
                {
                    mModifyCommand = value;
                    RaisePropertyChanged("ModifyCommand");
                }
            }
        }

        public ICommand DeleteCommand
        {
            get { return mDeleteCommand; }
            set
            {
                if (mDeleteCommand != value)
                {
                    mDeleteCommand = value;
                    RaisePropertyChanged("DeleteCommand");
                }
            }
        }

        public ICommand QueryCommand
        {
            get { return mQueryCommand; }
            set
            {
                if (mQueryCommand != value)
                {
                    mQueryCommand = value;
                    RaisePropertyChanged("QueryCommand");
                }
            }
        }

        public ICommand BrowseCommand
        {
            get { return mBrowseCommand; }
            set
            {
                if (mBrowseCommand != value)
                {
                    mBrowseCommand = value;
                    RaisePropertyChanged("BrowseCommand");
                }
            }
        }

        #endregion

        #region Members

        public void GridRefresh()
        {
             ViewCore.PagingReload();
        }

        #endregion
    }
}
