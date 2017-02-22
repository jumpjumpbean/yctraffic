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
    public class LbCyPatrolQueryViewModel : LbBaseQueryViewModel<ILbCyPatrolQueryView>
    {
        #region Data

        private ZdtzCyPatrol mSelectedPatrol;
        private IQueryable<ZdtzCyPatrol> mLbPatrols;
        private bool isFromAll = false;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbCyPatrolQueryViewModel(ILbCyPatrolQueryView view, IEntityService entityservice)
            : base(view, entityservice)
        {
            if (entityservice.EnumLbPatrols != null)
            {
                this.mLbPatrols = entityservice.EnumLbPatrols;
            }
            else
            {
                this.mLbPatrols = new List<ZdtzCyPatrol>().AsQueryable(); //以防没有数据时出现异常
            }
        }

        #endregion

        #region Properties

        public IQueryable<ZdtzCyPatrol> LbPatrols
        {
            get
            {
                return mLbPatrols;
            }
            set
            {
                mLbPatrols = value;
                RaisePropertyChanged("LbPatrols");
            }
        }

        public ZdtzCyPatrol SelectedPatrol
        {
            get { return mSelectedPatrol; }
            set
            {
                if (mSelectedPatrol != value)
                {
                    mSelectedPatrol = value;
                    RaisePropertyChanged("SelectedPatrol");
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

        #endregion

        #region Members

        public override void GridRefresh()
        {
             ViewCore.PagingReload();
        }

        #endregion
    }
}
    