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
    public class LbStaticLogbookQueryViewModel : LbBaseQueryViewModel<ILbStaticLogbookQueryView>
    {
        #region Data

        private ZdtzStaticTable mSelectedStaticLogbook;
        private IQueryable<ZdtzStaticTable> mLbStaticLogbooks;
        private string mInputTitle;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbStaticLogbookQueryViewModel(ILbStaticLogbookQueryView view, IEntityService entityservice)
            : base(view, entityservice)
        {
        }

        #endregion

        #region Properties

        public IQueryable<ZdtzStaticTable> LbStaticLogbooks
        {
            get
            {
                return mLbStaticLogbooks;
            }
            set
            {
                mLbStaticLogbooks = value;
                RaisePropertyChanged("LbStaticLogbooks");
            }
        }

        public ZdtzStaticTable SelectedStaticLogbook
        {
            get { return mSelectedStaticLogbook; }
            set
            {
                if (mSelectedStaticLogbook != value)
                {
                    mSelectedStaticLogbook = value;
                    RaisePropertyChanged("SelectedStaticLogbook");
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

        #endregion

        #region Members

        public override void GridRefresh()
        {
             ViewCore.PagingReload();
        }

        #endregion
    }
}
    