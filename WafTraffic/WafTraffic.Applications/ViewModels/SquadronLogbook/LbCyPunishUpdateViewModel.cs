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
    public class LbCyPunishUpdateViewModel : LbBaseUpdateViewModel<ILbCyPunishUpdateView>
    {
        #region Data

        private ZdtzCyPunish mPunishEntity;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbCyPunishUpdateViewModel(ILbCyPunishUpdateView view)
            : base(view)
        {
            mPunishEntity = new ZdtzCyPunish();
        }

        #endregion

        #region Properties

        public ZdtzCyPunish PunishEntity
        {
            get { return mPunishEntity; }
            set
            {
                if (mPunishEntity != value)
                {
                    mPunishEntity = value;
                    RaisePropertyChanged("PunishEntity");
                }
            }
        }

        #endregion
    }
}
    