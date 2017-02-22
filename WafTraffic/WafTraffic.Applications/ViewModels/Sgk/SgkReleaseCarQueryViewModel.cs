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
using System.Linq;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class SgkReleaseCarQueryViewModel : ViewModel<ISgkReleaseCarQueryView>
    {
        #region Data


        private ICommand newCommand;
        private ICommand modifyCommand;
        private ICommand deleteCommand;
        private ICommand browseCommand;
        private ICommand approveCommand;
        private ICommand leaveReturnCommand;

        private ICommand queryCommand;

        private DateTime startDate;
        private DateTime endDate;
        private String keyWord;

        private SgkReleaseCar selectedYellowMarkCar;
        private IQueryable<SgkReleaseCar> cgsYellowMarkCars;

        IEntityService entityservice;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public SgkReleaseCarQueryViewModel(ISgkReleaseCarQueryView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.QuerySgkReleaseCars != null)
            {
                this.cgsYellowMarkCars = entityservice.QuerySgkReleaseCars;
            }
            else
            {
                this.cgsYellowMarkCars = new List<SgkReleaseCar>().AsQueryable(); //以防没有数据时出现异常
            }

        }

        #endregion

        #region Properties

        private string operation;
        public string Operation
        {
            get
            {
                return operation;
            }
            set
            {
                if (operation != value)
                {
                    operation = value;
                    RaisePropertyChanged("Operation");
                }
            }
        }


        public IQueryable<SgkReleaseCar> SgkReleaseCars
        {
            get
            {
                return cgsYellowMarkCars;
            }
            set
            {
                cgsYellowMarkCars = value;
                RaisePropertyChanged("SgkReleaseCars");
            }
        }

        public SgkReleaseCar SelectedYellowMarkCar
        {
            get { return selectedYellowMarkCar; }
            set
            {
                if (selectedYellowMarkCar != value)
                {
                    selectedYellowMarkCar = value;
                    RaisePropertyChanged("SelectedYellowMarkCar");
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

        public ICommand LeaveReturnCommand
        {
            get { return leaveReturnCommand; }
            set
            {
                if (leaveReturnCommand != value)
                {
                    leaveReturnCommand = value;
                    RaisePropertyChanged("LeaveReturnCommand");
                }
            }
        }
                
        //public Visibility CanAddShow
        //{
        //    get
        //    {
        //        if (CurrentLoginService.Instance.IsAuthorized("yctraffic.SgkReleaseCar.Add"))
        //        {
        //            canAddShow = System.Windows.Visibility.Visible;
        //        }
        //        else
        //        {
        //            canAddShow = System.Windows.Visibility.Hidden;
        //        }

        //        return canAddShow;
        //    }
        //}


        //public Visibility CanDelete
        //{
        //    get
        //    {
        //        if (CurrentLoginService.Instance.IsAuthorized("yctraffic.SgkReleaseCar.Delete"))
        //        {
        //            canDelete = System.Windows.Visibility.Visible;
        //        }
        //        else
        //        {
        //            canDelete = System.Windows.Visibility.Hidden;
        //        }

        //        return canDelete;
        //    }
        //}

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    RaisePropertyChanged("StartDate");
                }
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    RaisePropertyChanged("EndDate");
                }
            }
        }

        public String KeyWord
        {
            get { return keyWord; }
            set
            {
                if (keyWord != value)
                {
                    keyWord = value;
                    RaisePropertyChanged("KeyWord");
                }
            }
        }

        #endregion

        #region Members

        public void GridRefresh()
        {
             ViewCore.PagingReload();
        }

        #endregion
    }
}
    