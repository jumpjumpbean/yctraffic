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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class MaterialDeclareViewModel : ViewModel<IMaterialDeclareView>
    {
        private List<BaseOrganizeEntity> departmentList;

        private IQueryable<MaterialDeclareTable> materialDeclare;
        private MaterialDeclareTable seletedMaterialDeclare;

        private string selectDepartCode = string.Empty;
        private DateTime startDate;
        private DateTime endDate;
        private String keyWord;

        private ICommand newCommand;
        private ICommand modifyCommand;
        private ICommand deleteCommand;        
        private ICommand browseCommand;
        private ICommand queryCommand;
        private ICommand gatherCommand;
        private ICommand approveCommand;
        private ICommand noApproveCommand;

        private Visibility canSummaryShow;
        private Visibility canAddShow;
        

        IEntityService entityservice;

        [ImportingConstructor]
        public MaterialDeclareViewModel(IMaterialDeclareView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.EnumMaterialDeclares != null)
            {
                this.materialDeclare = entityservice.EnumMaterialDeclares;
            }
            else
            {
                this.materialDeclare = new List<MaterialDeclareTable>().AsQueryable(); //以防没有数据时出现异常
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

        public IQueryable<MaterialDeclareTable> MaterialDeclare
        {
            get
            {
                return materialDeclare;
            }
            set
            {
                materialDeclare = value;
                RaisePropertyChanged("MaterialDeclare");
            }
        }

        public MaterialDeclareTable SelectedMaterialDeclare
        {
            get { return seletedMaterialDeclare; }
            set
            {
                if (seletedMaterialDeclare != value)
                {
                    seletedMaterialDeclare = value;
                    RaisePropertyChanged("SelectedMaterialDeclare");
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

        public ICommand NoApproveCommand
        {
            get { return noApproveCommand; }
            set
            {
                if (noApproveCommand != value)
                {
                    noApproveCommand = value;
                    RaisePropertyChanged("NoApproveCommand");
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

        public Visibility CanSummaryShow
        {
            get
            {
                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.MaterialDeclare.Summary"))
                {
                    canSummaryShow = System.Windows.Visibility.Visible;
                }
                else 
                {
                    canSummaryShow = System.Windows.Visibility.Hidden;
                }

                return canSummaryShow;
            }

        }

        public Visibility CanAddShow
        {
            get
            {
                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.MaterialDeclare.New"))
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

    