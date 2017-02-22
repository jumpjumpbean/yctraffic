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
using System.Linq;
using WafTraffic.Domain.Common;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ZhzxTotalViolationListViewModel : ViewModel<IZhzxTotalViolationListView>
    {

        private IQueryable<ZhzxTotalViolation> zhzxTotalViolation;
        private ZhzxTotalViolation selectedZhzxTotalViolation;


        private String plateNumber;
        private String checkpoint;

        private ICommand deleteCommand;        
        private ICommand browseCommand;
        private ICommand queryCommand;
        

        IEntityService entityservice;

        [ImportingConstructor]
        public ZhzxTotalViolationListViewModel(IZhzxTotalViolationListView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.QueryableZhzxTotalViolation != null)
            {
                this.zhzxTotalViolation = entityservice.QueryableZhzxTotalViolation;
            }
            else
            {
                this.zhzxTotalViolation = new List<ZhzxTotalViolation>().AsQueryable(); //以防没有数据时出现异常
            }
        }

        public IQueryable<ZhzxTotalViolation> ZhzxTotalViolation
        {
            get
            {
                return zhzxTotalViolation;
            }
            set
            {
                if (zhzxTotalViolation != value)
                {
                    zhzxTotalViolation = value;
                    RaisePropertyChanged("ZhzxTotalViolation");
                }
            }
        }

        public ZhzxTotalViolation SelectedZhzxTotalViolation
        {
            get { return selectedZhzxTotalViolation; }
            set
            {
                if (selectedZhzxTotalViolation != value)
                {
                    selectedZhzxTotalViolation = value;
                    RaisePropertyChanged("SelectedZhzxTotalViolation");
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

        public void GridRefresh()
        {
            ViewCore.PagingReload();
        }

        //查询条件


        public String PlateNumber
        {
            get { return plateNumber; }
            set
            {
                if (plateNumber != value)
                {
                    plateNumber = value;
                    RaisePropertyChanged("PlateNumber");
                }
            }
        }

        public String Checkpoint
        {
            get { return checkpoint; }
            set
            {
                if (checkpoint != value)
                {
                    checkpoint = value;
                    RaisePropertyChanged("Checkpoint");
                }
            }
        }



    }
}

    