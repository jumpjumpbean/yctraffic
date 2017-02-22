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
    public class MonthRegisterViewModel : ViewModel<IMonthRegisterView>
    {
        private MonthRegisterTable monthRegister;

        private string operation;

        private ICommand saveCommand;
        private ICommand submitCommand;
        private ICommand retreatCommand;

        [ImportingConstructor]
        public MonthRegisterViewModel(IMonthRegisterView view)
            : base(view)
        {
            monthRegister = new MonthRegisterTable();
        }

        public MonthRegisterTable MonthRegister
        {
            get { return monthRegister; }
            set
            {
                if (monthRegister != value)
                {
                    monthRegister = value;
                    RaisePropertyChanged("MonthRegister");
                }
            }
        }

        public String Operation
        {
            get { return operation; }
            set
            {
                if (operation != value)
                {
                    operation = value;
                    RaisePropertyChanged("Operation");
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

        public ICommand SubmitCommand
        {
            get { return submitCommand; }
            set
            {
                if (submitCommand != value)
                {
                    submitCommand = value;
                    RaisePropertyChanged("SubmitCommand");
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
    