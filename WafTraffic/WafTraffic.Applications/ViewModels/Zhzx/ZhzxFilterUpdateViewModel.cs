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
    public class ZhzxFilterUpdateViewModel : LbBaseUpdateViewModel<IZhzxFilterUpdateView>
    {
        #region Data

        private ZhzxRedNameList mEntity;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public ZhzxFilterUpdateViewModel(IZhzxFilterUpdateView view)
            : base(view)
        {
            mEntity = new ZhzxRedNameList();
        }

        #endregion

        #region Properties

        public ZhzxRedNameList FilterEntity
        {
            get { return mEntity; }
            set
            {
                if (mEntity != value)
                {
                    mEntity = value;
                    RaisePropertyChanged("FilterEntity");
                }
            }
        }

        #endregion
    }
}
    