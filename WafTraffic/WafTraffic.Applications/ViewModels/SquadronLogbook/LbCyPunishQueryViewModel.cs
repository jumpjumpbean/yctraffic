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
    public class LbCyPunishQueryViewModel : LbBaseQueryViewModel<ILbCyPunishQueryView>
    {
        #region Data

        private ZdtzCyPunish mSelectedPunish;
        private IQueryable<ZdtzCyPunish> mLbPunishs;
        private bool isFromAll = false;
        private Visibility gatherPermissionVisibility;

        private ICommand gatherCommand;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbCyPunishQueryViewModel(ILbCyPunishQueryView view, IEntityService entityservice)
            : base(view, entityservice)
        {
            if (entityservice.EnumLbPunishs != null)
            {
                this.mLbPunishs = entityservice.EnumLbPunishs;
            }
            else
            {
                this.mLbPunishs = new List<ZdtzCyPunish>().AsQueryable(); //以防没有数据时出现异常
            }

            if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_CYPUNISH_GATHER))
            {
                gatherPermissionVisibility = Visibility.Visible;
            }
            else
            {
                gatherPermissionVisibility = Visibility.Hidden;
            }
        }

        #endregion

        #region Properties

        public IQueryable<ZdtzCyPunish> LbPunishs
        {
            get
            {
                return mLbPunishs;
            }
            set
            {
                mLbPunishs = value;
                RaisePropertyChanged("LbPunishs");
            }
        }

        public ZdtzCyPunish SelectedPunish
        {
            get { return mSelectedPunish; }
            set
            {
                if (mSelectedPunish != value)
                {
                    mSelectedPunish = value;
                    RaisePropertyChanged("SelectedPunish");
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

        public ICommand GatherCommand
        {
            get { return gatherCommand; }
            set
            {
                if (gatherCommand != value)
                {
                    gatherCommand = value;
                    RaisePropertyChanged("GatherCommand");
                }
            }
        }

        public Visibility GatherPermissionVisibility
        {
            get { return gatherPermissionVisibility; }
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
    