using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using WafTraffic.Domain.Common;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;
using DotNet.Business;
using System.Data;
using System.Windows;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class FzkReleaseCarPrintViewModel : ViewModel<IFzkReleaseCarPrintView>
    {
        private ICommand printRetreatCommand;

        private FzkReleaseCar releaseCarEntity;

        IEntityService entityservice;

        [ImportingConstructor]
        public FzkReleaseCarPrintViewModel(IFzkReleaseCarPrintView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;
        }

        public ICommand PrintRetreatCommand
        {
            get { return printRetreatCommand; }
            set
            {
                if (printRetreatCommand != value)
                {
                    printRetreatCommand = value;
                    RaisePropertyChanged("PrintRetreatCommand");
                }
            }
        }

        public FzkReleaseCar ReleaseCarEntity
        {
            get { return releaseCarEntity; }
            set
            {
                if (releaseCarEntity != value)
                {
                    releaseCarEntity = value;
                    RaisePropertyChanged("ReleaseCarEntity");
                }
            }
        }

        public void ReloadGatherReport()
        {
            ViewCore.GatherReportReload();
        }

        public void Close()
        {
            ViewCore.Close();
        }
    }
}
    