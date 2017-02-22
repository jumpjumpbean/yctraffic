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
    public class HotLineListViewModel : ViewModel<IHotLineListView>
    {
        private List<BaseOrganizeEntity> departmentList;
        private int selectDepartId = 0;
        private string selectContents = string.Empty;

        private MayorHotlineTaskTable selectedHotline;
        private IQueryable<MayorHotlineTaskTable> hotLineTasks;
        private ICommand newCommand;        
        private ICommand modifyCommand;
        private ICommand deleteCommand;
        private ICommand dealCommand;
        private ICommand checkCommand;
        private ICommand browseCommand;

        private ICommand queryCommand;

        IEntityService entityservice;

         [ImportingConstructor]
        public HotLineListViewModel(IHotLineListView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;
            //if (entityservice.EnumHotLineTasks == null) { throw new ArgumentNullException("EnumHotLineTasks"); }
            if (entityservice.EnumHotLineTasks != null)
            {
                this.hotLineTasks = entityservice.EnumHotLineTasks;
            }
            else
            {
                this.hotLineTasks = new List<MayorHotlineTaskTable>().AsQueryable(); //以防没有数据时出现异常
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

         public IQueryable<MayorHotlineTaskTable> HotLineTasks
         {
             get
             {                  
                 return hotLineTasks;
             }
             set
             {
                 hotLineTasks = value;
                 RaisePropertyChanged("HotLineTasks");
             }
         }

         public MayorHotlineTaskTable SelectedHotline
         {
             get { return selectedHotline; }
             set
             {
                 if (selectedHotline != value)
                 {
                     selectedHotline = value;
                     RaisePropertyChanged("SelectedHotline");
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

         public ICommand DealCommand
         {
             get { return dealCommand; }
             set
             {
                 if (dealCommand != value)
                 {
                     dealCommand = value;
                     RaisePropertyChanged("DealCommand");
                 }
             }
         }

         public ICommand CheckCommand
         {
             get { return checkCommand; }
             set
             {
                 if (checkCommand != value)
                 {
                     checkCommand = value;
                     RaisePropertyChanged("CheckCommand");
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
         public int SelectDepartId
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

         public string SelectContents
         {
             get { return selectContents; }
             set
             {
                 if (selectContents != value)
                 {
                     selectContents = value;
                     RaisePropertyChanged("SelectContents");
                 }
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
