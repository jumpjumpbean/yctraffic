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
    public class LbZdStaffInfoQueryViewModel : LbBaseQueryViewModel<ILbZdStaffInfoQueryView>
    {
        #region Data

        private ZdtzZdStaffInfo mSelectedStaffInfo;
        private IQueryable<ZdtzZdStaffInfo> mLbStaffinfos;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbZdStaffInfoQueryViewModel(ILbZdStaffInfoQueryView view, IEntityService entityservice)
            : base(view, entityservice)
        {
            if (entityservice.EnumLbStaffInfos != null)
            {
                this.mLbStaffinfos = entityservice.EnumLbStaffInfos;
            }
            else
            {
                this.mLbStaffinfos = new List<ZdtzZdStaffInfo>().AsQueryable(); //以防没有数据时出现异常
            }

        }

        #endregion

        #region Properties

        public IQueryable<ZdtzZdStaffInfo> LbStaffinfos
        {
            get
            {
                return mLbStaffinfos;
            }
            set
            {
                mLbStaffinfos = value;
                RaisePropertyChanged("LbStaffinfos");
            }
        }

        public ZdtzZdStaffInfo SelectedStaffInfo
        {
            get { return mSelectedStaffInfo; }
            set
            {
                if (mSelectedStaffInfo != value)
                {
                    mSelectedStaffInfo = value;
                    RaisePropertyChanged("SelectedStaffInfo");
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
    