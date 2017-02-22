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
    public class ZhzxElectronMonitorListViewModel : ViewModel<IZhzxElectronMonitorListView>
    {
        private IQueryable<ZhzxElectronMonitor> zhzxElectronMonitor;
        private ZhzxElectronMonitor selectedZhzxElectronMonitor;

        private String checkpointKeyWord;
        private String statusKeyWord;

        private ICommand newCommand;
        private ICommand modifyCommand;
        private ICommand deleteCommand;        
        private ICommand browseCommand;
        private ICommand queryCommand;
        private Visibility canAddShow;
        

        IEntityService entityservice;

        [ImportingConstructor]
        public ZhzxElectronMonitorListViewModel(IZhzxElectronMonitorListView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.QueryableZhzxElectronMonitor != null)
            {
                this.zhzxElectronMonitor = entityservice.QueryableZhzxElectronMonitor;
            }
            else
            {
                this.zhzxElectronMonitor = new List<ZhzxElectronMonitor>().AsQueryable(); //以防没有数据时出现异常
            }
        }

        public IQueryable<ZhzxElectronMonitor> ZhzxElectronMonitor
        {
            get
            {
                return zhzxElectronMonitor;
            }
            set
            {
                if (zhzxElectronMonitor != value)
                {
                    zhzxElectronMonitor = value;
                    RaisePropertyChanged("ZhzxElectronMonitor");
                }
            }
        }

        public ZhzxElectronMonitor SelectedZhzxElectronMonitor
        {
            get { return selectedZhzxElectronMonitor; }
            set
            {
                if (selectedZhzxElectronMonitor != value)
                {
                    selectedZhzxElectronMonitor = value;
                    RaisePropertyChanged("SelectedZhzxElectronMonitor");
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
        public String CheckpointKeyWord
        {
            get { return checkpointKeyWord; }
            set
            {
                if (checkpointKeyWord != value)
                {
                    checkpointKeyWord = value;
                    RaisePropertyChanged("CheckpointKeyWord");
                }
            }
        }

        public String StatusKeyWord
        {
            get { return statusKeyWord; }
            set
            {
                if (statusKeyWord != value)
                {
                    statusKeyWord = value;
                    RaisePropertyChanged("StatusKeyWord");
                }
            }
        }

        public Visibility CanAddShow
        {
            get
            {
                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.Zhzx.ElectronMonitor.Add"))
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

    