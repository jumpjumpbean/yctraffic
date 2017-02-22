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
    public class ZgxcPublicityLogbookQueryViewModel : ViewModel<IZgxcPublicityLogbookQueryView>
    {
        #region Data

        private List<BaseOrganizeEntity> departmentList;

        private ICommand newCommand;
        private ICommand modifyCommand;
        private ICommand deleteCommand;
        private ICommand browseCommand;
        private ICommand queryCommand;

        private int? selectDepartId;
        private DateTime startDate;
        private DateTime endDate;
        private String keyWord;

        private ZgxcPublicityLogbook selectedPublicityLogbook;
        private IQueryable<ZgxcPublicityLogbook> zgxcPublicityLogbooks;

        IEntityService entityservice;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public ZgxcPublicityLogbookQueryViewModel(IZgxcPublicityLogbookQueryView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.QueryablePublicityLogbooks != null)
            {
                this.zgxcPublicityLogbooks = entityservice.QueryablePublicityLogbooks;
            }
            else
            {
                this.zgxcPublicityLogbooks = new List<ZgxcPublicityLogbook>().AsQueryable(); //以防没有数据时出现异常
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

            //ALL
            BaseOrganizeEntity currentEntity = new BaseOrganizeEntity();
            currentEntity.Code = "";
            currentEntity.Id = 0;
            currentEntity.FullName = "全部";

            departmentList.Insert(0, currentEntity);
        }

        #endregion

        #region Properties

        public IQueryable<ZgxcPublicityLogbook> ZgxcPublicityLogbooks
        {
            get
            {
                return zgxcPublicityLogbooks;
            }
            set
            {
                zgxcPublicityLogbooks = value;
                RaisePropertyChanged("ZgxcPublicityLogbooks");
            }
        }

        public ZgxcPublicityLogbook SelectedPublicityLogbook
        {
            get { return selectedPublicityLogbook; }
            set
            {
                if (selectedPublicityLogbook != value)
                {
                    selectedPublicityLogbook = value;
                    RaisePropertyChanged("SelectedPublicityLogbook");
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

        public List<BaseOrganizeEntity> DepartmentList
        {
            get{ return departmentList; }
            set
            {
                if (departmentList != value)
                { 
                    departmentList = value;
                    RaisePropertyChanged("DepartmentList");
                }
            }
        }

        public int? SelectDepartId
        {
            get { return selectDepartId; }
            set
            {
                if (selectDepartId != value)
                {
                    selectDepartId = value;
                    RaisePropertyChanged("SelectDepartId");
                }
            }
        }

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
    