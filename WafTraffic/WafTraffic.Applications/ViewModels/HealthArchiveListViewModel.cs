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
using WafTraffic.Applications.Common;
using System.Windows;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class HealthArchiveListViewModel : ViewModel<IHealthArchiveListView>
    {
        private List<BaseOrganizeEntity> departmentList;

        private HealthArchiveTable selectedHealthArchive;
        private IQueryable<HealthArchiveTable> healthArchives;

        private string selectDepartCode = string.Empty;
        private int selectYear = 0;

        private ICommand newCommand;
        private ICommand queryCommand;

        private ICommand modifyCommand;
        private ICommand deleteCommand;
        private ICommand browseCommand;        

        IEntityService entityservice;

        [ImportingConstructor]
        public HealthArchiveListViewModel(IHealthArchiveListView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.EnumHealthArchives != null)
            {
                this.healthArchives = entityservice.EnumHealthArchives;
            }
            else
            {
                this.healthArchives = new List<HealthArchiveTable>().AsQueryable(); //以防没有数据时出现异常
            }


            departmentList = new List<BaseOrganizeEntity>();

            BaseOrganizeManager origanizeService = new BaseOrganizeManager();
            DataTable departmentDT = origanizeService.GetDepartmentDT(""); //根节点 parnetid
            BaseOrganizeEntity entity;
            foreach (DataRow dr in departmentDT.Rows)
            {
                entity = new BaseOrganizeEntity(dr);
                departmentList.Add(entity);
            }

            entity = origanizeService.GetEntity(YcConstants.INT_COMPANY_ID);
            departmentList.Add(entity);
            //ALL
            BaseOrganizeEntity currentEntity = new BaseOrganizeEntity();
            currentEntity.Code = "";
            currentEntity.Id = 0;
            currentEntity.FullName = "全部";

            departmentList.Insert(0, currentEntity);
        }

        public IQueryable<HealthArchiveTable> HealthArchives
        {
            get
            {
                return healthArchives;
            }
            set
            {
                healthArchives = value;
                RaisePropertyChanged("HealthArchives");
            }
        }

        public HealthArchiveTable SelectedHealthArchive
        {
            get { return selectedHealthArchive; }
            set
            {
                if (selectedHealthArchive != value)
                {
                    selectedHealthArchive = value;
                    RaisePropertyChanged("SelectedHealthArchive");
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

        public Visibility CanAddShow
        {
            get 
            {
                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.HealthArchive.Add"))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }        
            }
        }

        public void GridRefresh()
        {
            ViewCore.PagingReload();
        }

        //查询条件
        public string SelectDepartCode
        {
            get { return selectDepartCode; }
            set
            {
                if (selectDepartCode != value)
                {
                    selectDepartCode = value;
                    RaisePropertyChanged("SelectDepartCode");
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

        public int CurrentYear
        {
            get { return System.DateTime.Now.Year; }
            set
            {
            }
        }

        public List<BaseOrganizeEntity> DepartmentList
        {
            get { return departmentList; }
            set
            {
                if (departmentList != value)
                {
                    departmentList = value;
                    RaisePropertyChanged("DepartmentList");
                }
            }
        }

    }
}
