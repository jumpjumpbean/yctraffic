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
using WafTraffic.Domain.Common;


namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class YuChengMapViewModel : ViewModel<IYuChengMapView>
    {       


        [ImportingConstructor]
        public YuChengMapViewModel(IYuChengMapView view, IEntityService entityservice)
            : base(view)
        {


       
        }
    }
}
    