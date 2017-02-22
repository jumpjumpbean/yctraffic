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
    public class ZhzxFixedAssetsRegisterListViewModel : ViewModel<IZhzxFixedAssetsRegisterListView>
    {

        private IQueryable<ZhzxFixedAssetsRegister> zhzxFixedAssetsRegister;
        private ZhzxFixedAssetsRegister selectedZhzxFixedAssetsRegister;

        private string selectDepartCode = string.Empty;
        private DateTime startDate;
        private DateTime endDate;
        private String keyWord;

        private ICommand newCommand;
        private ICommand modifyCommand;
        private ICommand deleteCommand;        
        private ICommand browseCommand;
        private ICommand queryCommand;
        private Visibility canAddShow;
        

        IEntityService entityservice;

        [ImportingConstructor]
        public ZhzxFixedAssetsRegisterListViewModel(IZhzxFixedAssetsRegisterListView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.QueryableZhzxFixedAssetsRegister != null)
            {
                this.zhzxFixedAssetsRegister = entityservice.QueryableZhzxFixedAssetsRegister;
            }
            else
            {
                this.zhzxFixedAssetsRegister = new List<ZhzxFixedAssetsRegister>().AsQueryable(); //以防没有数据时出现异常
            }
        }

        public IQueryable<ZhzxFixedAssetsRegister> ZhzxFixedAssetsRegister
        {
            get
            {
                return zhzxFixedAssetsRegister;
            }
            set
            {
                if (zhzxFixedAssetsRegister != value)
                {
                    zhzxFixedAssetsRegister = value;
                    RaisePropertyChanged("ZhzxFixedAssetsRegister");
                }
            }
        }

        public ZhzxFixedAssetsRegister SelectedZhzxFixedAssetsRegister
        {
            get { return selectedZhzxFixedAssetsRegister; }
            set
            {
                if (selectedZhzxFixedAssetsRegister != value)
                {
                    selectedZhzxFixedAssetsRegister = value;
                    RaisePropertyChanged("SelectedZhzxFixedAssetsRegister");
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

        public void GridRefresh()
        {
            ViewCore.PagingReload();
        }

        //查询条件
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

        public Visibility CanAddShow
        {
            get
            {
                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.FixedAssetsRegister.Add"))
                {
                    canAddShow = System.Windows.Visibility.Visible;
                }
                else
                {
                    canAddShow = System.Windows.Visibility.Hidden;
                }

                return canAddShow;
            }         
        }

    }
}

    