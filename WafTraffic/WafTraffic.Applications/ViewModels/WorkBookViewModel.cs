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
    public class WorkBookViewModel : ViewModel<IWorkBookView>
    {
        private string workContents;
        //private int workContentCount;
        private int selectMonth;
        private int selectYear;
        private readonly IShellService shellService;

        private ICommand queryCommand;

        IEntityService entityservice;

        [ImportingConstructor]
        public WorkBookViewModel(IWorkBookView view, IEntityService entityservice, IShellService shellService)
            : base(view)
        {
            this.entityservice = entityservice;
            this.shellService = shellService;
            //string workContents = entityservice.GetWorkContents(DateTime.Now.Year, DateTime.Now.Month, CurrentLoginService.Instance.CurrentUserInfo.DepartmentCode + "%");
            string workContents = entityservice.GetWorkContents(DateTime.Now.Year, DateTime.Now.Month, "10.10%");
            
        }

        public IShellService ShellService { get { return shellService; } }

        //public int WorkContentCount
        //{
        //    get { return workContentCount; }
        //    set
        //    {
        //        if (workContentCount != value)
        //        {
        //            workContentCount = value;
        //            RaisePropertyChanged("WorkContentCount");
        //        }
        //    }
        //}

        public string WorkContents
        {
            get { return workContents; }
            set
            {
                if (workContents != value)
                {
                    workContents = value;
                    RaisePropertyChanged("WorkContents");
                }
            }
        }

        public int SelectMonth
        {
            get { return selectMonth; }
            set
            {
                if (selectMonth != value)
                {
                    selectMonth = value;
                    RaisePropertyChanged("SelectMonth");
                }
            }
        }

        public int SelectYear
        {
            get { return selectYear; }
            set
            {
                if (selectYear != value)
                {
                    selectYear = value;
                    RaisePropertyChanged("SelectYear");
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
    }
}
    