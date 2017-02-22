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
    public class ZhzxElectronMonitorDetailViewModel : ViewModel<IZhzxElectronMonitorDetailView>
    {
        private ZhzxElectronMonitor zhzxElectronMonitor;

        private ICommand saveCommand;
        private ICommand retreatCommand;

        private Visibility canSave;
        private Visibility canUpdaterVisibal;

        private bool canBaseInfoEdit;

        [ImportingConstructor]
        public ZhzxElectronMonitorDetailViewModel(IZhzxElectronMonitorDetailView view)
            : base(view)
        {
            zhzxElectronMonitor = new ZhzxElectronMonitor();
        }

        public ZhzxElectronMonitor ZhzxElectronMonitor
        {
            get { return zhzxElectronMonitor; }
             set
             {
                 if (zhzxElectronMonitor != value)
                 {
                     zhzxElectronMonitor = value;
                     RaisePropertyChanged("ZhzxElectronMonitor");
                 }
             }
        }

         public ICommand SaveCommand
         {
             get { return saveCommand; }
             set
             {
                 if (saveCommand != value)
                 {
                     saveCommand = value;
                     RaisePropertyChanged("SaveCommand");
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

         public Visibility CanUpdaterVisibal
         {
             get { return canUpdaterVisibal; }
             set
             {
                 if (canUpdaterVisibal != value)
                 {
                     canUpdaterVisibal = value;
                     RaisePropertyChanged("CanUpdaterVisibal");
                 }
             }
         }


        public Visibility CanSave
        {
            get { return canSave; }
            set
            {
                if (canSave != value)
                {
                    canSave = value;
                    RaisePropertyChanged("CanSave");
                }
            }
        }


        public bool CanBaseInfoEdit
        {
            get { return canBaseInfoEdit; }
            set
            {
                if (canBaseInfoEdit != value)
                {
                    canBaseInfoEdit = value;
                    RaisePropertyChanged("CanBaseInfoEdit");
                }
            }
        }
    }
}