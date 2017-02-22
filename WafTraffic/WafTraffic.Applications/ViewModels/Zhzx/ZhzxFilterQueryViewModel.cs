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
using System.Linq;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ZhzxFilterQueryViewModel : ViewModel<IZhzxFilterQueryView>
    {
        #region Data

        private ZhzxRedNameList mSelectedFilter;
        private IQueryable<ZhzxRedNameList> mFilters;

        private ICommand mNewCommand;
        private ICommand mModifyCommand;
        private ICommand mDeleteCommand;
        private ICommand mDealCommand;
        private ICommand mQueryCommand;
        private ICommand mBrowseCommand;
        private string mInputPlateNumber;
        IEntityService entityservice;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public ZhzxFilterQueryViewModel(IZhzxFilterQueryView view, IEntityService entityservice)
            : base(view)
        {
            try
            {
                this.entityservice = entityservice;
                if (entityservice.EnumZhzxRedNameLists != null)
                {
                    this.mFilters = entityservice.EnumZhzxRedNameLists;
                }
                else
                {
                    this.mFilters = new List<ZhzxRedNameList>().AsQueryable();
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

        }

        #endregion

        #region Properties

        public IQueryable<ZhzxRedNameList> Filters
        {
            get
            {
                return mFilters;
            }
            set
            {
                mFilters = value;
                RaisePropertyChanged("Filters");
            }
        }

        public ZhzxRedNameList SelectedFilter
        {
            get { return mSelectedFilter; }
            set
            {
                if (mSelectedFilter != value)
                {
                    mSelectedFilter = value;
                    RaisePropertyChanged("SelectedFilter");
                }
            }
        }

        public string InputPlateNumber
        {
            get { return mInputPlateNumber; }
            set
            {
                if (mInputPlateNumber != value)
                {
                    mInputPlateNumber = value;
                    RaisePropertyChanged("InputPlateNumber");
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

        public ICommand DealCommand
        {
            get { return mDealCommand; }
            set
            {
                if (mDealCommand != value)
                {
                    mDealCommand = value;
                    RaisePropertyChanged("DealCommand");
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

        public void StoreWins_Refresh(IQueryable<ZhzxRedNameList> itemsSource)
        {
            entityservice.Entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, itemsSource);
        }

        #endregion
    }
}
    