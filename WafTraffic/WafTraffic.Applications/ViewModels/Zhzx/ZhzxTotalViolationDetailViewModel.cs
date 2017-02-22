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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ZhzxTotalViolationDetailViewModel : ViewModel<IZhzxTotalViolationDetailView>
    {
        private ZhzxTotalViolation zhzxTotalViolation;

        private ICommand retreatCommand;

        [ImportingConstructor]
        public ZhzxTotalViolationDetailViewModel(IZhzxTotalViolationDetailView view)
            : base(view)
        {
            zhzxTotalViolation = new ZhzxTotalViolation();
        }

        public ZhzxTotalViolation ZhzxTotalViolation
        {
            get { return zhzxTotalViolation; }
             set
             {
                 if (zhzxTotalViolation != value)
                 {
                     zhzxTotalViolation = value;
                     RaisePropertyChanged("ZhzxTotalViolation");
                 }
             }
        }

         public ICommand RetreatCommand
         {
             get { return retreatCommand; }
             set
             {
                 if (retreatCommand != value)
                 {
                     retreatCommand = value;
                     RaisePropertyChanged("RetreatCommand");
                 }
             }
         }
    }
}