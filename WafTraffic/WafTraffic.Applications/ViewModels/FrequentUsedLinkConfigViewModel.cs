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
    public class FrequentUsedLinkConfigViewModel : ViewModel<IFrequentUsedLinkConfigView>
    {

       
        private ICommand saveCommand;
        private ICommand retreatCommand;

        IEntityService entityservice;

        private FrequentUsedLink frequentlyUsedLink;

        [ImportingConstructor]
        public FrequentUsedLinkConfigViewModel(IFrequentUsedLinkConfigView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;
        }

        public FrequentUsedLink FrequentlyUsedLink
        {
            get
            {
                return frequentlyUsedLink;
            }
            set
            {
                if (frequentlyUsedLink != value)
                {
                    frequentlyUsedLink = value;
                    RaisePropertyChanged("FrequentlyUsedLink");
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





    }
}
    