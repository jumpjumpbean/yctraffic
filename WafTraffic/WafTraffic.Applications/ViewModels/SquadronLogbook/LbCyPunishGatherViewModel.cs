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
    public class LbCyPunishGatherViewModel : ViewModel<ILbCyPunishGatherView>
    {
        private ICommand gatherRetreatCommand;
        private ICommand gatherQueryCommand;

        private DateTime gatherStartDate;
        private DateTime gatherEndDate;

        private List<ZdtzCyPunishGatherTable> punishGatherList;

        IEntityService entityservice;

        [ImportingConstructor]
        public LbCyPunishGatherViewModel(ILbCyPunishGatherView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;
            gatherStartDate = DateTime.Today;
            gatherEndDate = DateTime.Today;
        }

        public ICommand GatherRetreatCommand
        {
            get { return gatherRetreatCommand; }
            set
            {
                if (gatherRetreatCommand != value)
                {
                    gatherRetreatCommand = value;
                    RaisePropertyChanged("GatherRetreatCommand");
                }
            }
        }

        public ICommand GatherQueryCommand
        {
            get { return gatherQueryCommand; }
            set
            {
                if (gatherQueryCommand != value)
                {
                    gatherQueryCommand = value;
                    RaisePropertyChanged("GatherQueryCommand");
                }
            }
        }

        public DateTime GatherStartDate
        {
            get { return gatherStartDate; }
            set
            {
                if (gatherStartDate != value)
                {
                    gatherStartDate = value;
                    RaisePropertyChanged("GatherStartDate");
                }
            }
        }

        public DateTime GatherEndDate
        {
            get { return gatherEndDate; }
            set
            {
                if (gatherEndDate != value)
                {
                    gatherEndDate = value;
                    RaisePropertyChanged("GatherEndDate");
                }
            }
        }

        public List<ZdtzCyPunishGatherTable> PunishGatherList
        {
            get { return punishGatherList; }
            set
            {
                if (punishGatherList != value)
                {
                    punishGatherList = value;
                    RaisePropertyChanged("PunishGatherList");
                }
            }
        }

        public void ReloadGatherReport()
        {
            ViewCore.GatherReportReload();
        }

    }
}
    