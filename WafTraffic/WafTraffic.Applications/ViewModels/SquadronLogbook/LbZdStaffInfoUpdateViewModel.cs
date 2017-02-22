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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class LbZdStaffInfoUpdateViewModel : LbBaseUpdateViewModel<ILbZdStaffInfoUpdateView>
    {
        #region Data

        private ZdtzZdStaffInfo mStaffInfoEntity;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbZdStaffInfoUpdateViewModel(ILbZdStaffInfoUpdateView view)
            : base(view)
        {
            mStaffInfoEntity = new ZdtzZdStaffInfo();
        }

        #endregion

        #region Properties

        public ZdtzZdStaffInfo StaffInfoEntity
        {
            get { return mStaffInfoEntity; }
            set
            {
                if (mStaffInfoEntity != value)
                {
                    mStaffInfoEntity = value;
                    RaisePropertyChanged("StaffInfoEntity");
                }
            }
        }

        #endregion
    }
}
    