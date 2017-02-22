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
    public class LbCyPatrolUpdateViewModel : LbBaseUpdateViewModel<ILbCyPatrolUpdateView>
    {
        #region Data

        private ZdtzCyPatrol mPatrolEntity;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbCyPatrolUpdateViewModel(ILbCyPatrolUpdateView view)
            : base(view)
        {
            mPatrolEntity = new ZdtzCyPatrol();
        }

        #endregion

        #region Properties

        public ZdtzCyPatrol PatrolEntity
        {
            get { return mPatrolEntity; }
            set
            {
                if (mPatrolEntity != value)
                {
                    mPatrolEntity = value;
                    RaisePropertyChanged("PatrolEntity");
                }
            }
        }

        #endregion
    }
}
    