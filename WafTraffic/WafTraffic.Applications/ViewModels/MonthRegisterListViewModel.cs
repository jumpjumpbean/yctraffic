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
using System.Linq;

namespace WafTraffic.Applications.ViewModels
{    

    [Export]
    public class MonthRegisterListViewModel : ViewModel<IMonthRegisterListView>
    {
        private MonthRegisterTable selectedMonthRegister;
        private IQueryable<MonthRegisterTable> monthRegisters;
       
        private ICommand newCommand;
        private ICommand queryCommand;
        private ICommand gatherCommand;

        private ICommand modifyCommand;
        private ICommand deleteCommand;
        private ICommand browseCommand;
        private ICommand approveCommand;

        private DateTime minMonth;
        private DateTime maxMonth;

        IEntityService entityservice;

        [ImportingConstructor]
        public MonthRegisterListViewModel(IMonthRegisterListView view, IEntityService entityservice)
            : base(view)
        { 
        
            this.entityservice = entityservice;

            //if (entityservice.EnumMonthRegisters != null)
            //{
            //    this.monthRegisters = entityservice.EnumMonthRegisters;
            //}
            //else
            //{
            //    this.monthRegisters = new List<MonthRegisterTable>().AsQueryable(); //以防没有数据时出现异常
            //}

            this.minMonth = DateTime.Parse(DateTime.Now.Year + "-"+DateTime.Now.Month+"-01");
            this.maxMonth = DateTime.Parse(DateTime.Now.AddMonths(1).Year + "-" + DateTime.Now.AddMonths(1).Month + "-01");
           
        }

        public IQueryable<MonthRegisterTable> MonthRegisters
        {
            get
            {
                return monthRegisters;
            }
            set
            {
                monthRegisters = value;
                RaisePropertyChanged("MonthRegisters");
            }
        }

        public MonthRegisterTable SelectedMonthRegister
        {
            get { return selectedMonthRegister; }
            set
            {
                if (selectedMonthRegister != value)
                {
                    selectedMonthRegister = value;
                    RaisePropertyChanged("SelectedMonthRegister");
                }
            }
        }

        public DateTime MinMonth
        {
            get { return minMonth; }
            set
            {
                if (minMonth != value)
                {
                    minMonth = value;
                    RaisePropertyChanged("MinMonth");
                }
            }
        }

        public DateTime MaxMonth
        {
            get { return maxMonth; }
            set
            {
                if (maxMonth != value)
                {
                    maxMonth = value;
                    RaisePropertyChanged("MaxMonth");
                }
            }
        }

        public ICommand NewCommand
        {
            get { return newCommand; }
            set
            {
                if (newCommand != value)
                {
                    newCommand = value;
                    RaisePropertyChanged("NewCommand");
                }
            }
        }

        public ICommand QueryCommand
        {
            get { return queryCommand; }
            set
            {
                if (queryCommand != value)
                {
                    queryCommand = value;
                    RaisePropertyChanged("QueryCommand");
                }
            }
        }

        public ICommand GatherCommand
        {
            get { return gatherCommand; }
            set
            {
                if (gatherCommand != value)
                {
                    gatherCommand = value;
                    RaisePropertyChanged("GatherCommand");
                }
            }
        }


        public ICommand ModifyCommand
        {
            get { return modifyCommand; }
            set
            {
                if (modifyCommand != value)
                {
                    modifyCommand = value;
                    RaisePropertyChanged("ModifyCommand");
                }
            }
        }

        public ICommand DeleteCommand
        {
            get { return deleteCommand; }
            set
            {
                if (deleteCommand != value)
                {
                    deleteCommand = value;
                    RaisePropertyChanged("DeleteCommand");
                }
            }
        }

        public ICommand BrowseCommand
        {
            get { return browseCommand; }
            set
            {
                if (browseCommand != value)
                {
                    browseCommand = value;
                    RaisePropertyChanged("BrowseCommand");
                }
            }
        }

        public ICommand ApproveCommand
        {
            get { return approveCommand; }
            set
            {
                if (approveCommand != value)
                {
                    approveCommand = value;
                    RaisePropertyChanged("ApproveCommand");
                }
            }
        }

        public void GridRefresh()
        {
            ViewCore.PagingReload();
        }
      
    }
}
