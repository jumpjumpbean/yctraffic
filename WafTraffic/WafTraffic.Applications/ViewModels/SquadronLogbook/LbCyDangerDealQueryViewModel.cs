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
    public class LbCyDangerDealQueryViewModel : LbBaseQueryViewModel<ILbCyDangerDealQueryView>
    {
        #region Data

        private ZdtzCyDangerDeal mSelectedDangerDeal;
        private IQueryable<ZdtzCyDangerDeal> mLbDangerDeals;
        private bool isFromAll = false;
        private bool isAfterDangerDeal = false;
        private string mInputTitle;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbCyDangerDealQueryViewModel(ILbCyDangerDealQueryView view, IEntityService entityservice)
            : base(view, entityservice)
        {
        }

        #endregion

        #region Properties
        public IQueryable<ZdtzCyDangerDeal> LbDangerDeals
        {
            get
            {
                return mLbDangerDeals;
            }
            set
            {
                mLbDangerDeals = value;
                RaisePropertyChanged("LbDangerDeals");
            }
        }

        public ZdtzCyDangerDeal SelectedDangerDeal
        {
            get { return mSelectedDangerDeal; }
            set
            {
                if (mSelectedDangerDeal != value)
                {
                    mSelectedDangerDeal = value;
                    RaisePropertyChanged("SelectedDangerDeal");
                }
            }
        }

        public bool IsFromAll
        {
            get { return isFromAll; }
            set
            {
                isFromAll = value;
                RaisePropertyChanged("IsFromAll");
            }
        }

        public bool IsAfterDangerDeal
        {
            get { return isAfterDangerDeal; }
            set
            {
                isAfterDangerDeal = value;
                RaisePropertyChanged("IsAfterDangerDeal");
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
    