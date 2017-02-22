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
    public class GggsPublishNoticeListViewModel : ViewModel<IGggsPublishNoticeListView>
    {
        private List<BaseOrganizeEntity> departmentList;

        private IQueryable<GggsPublishNotice> gggsPublishNotice;
        private GggsPublishNotice selectedGggsPublishNotice;

        private string selectDepartCode = string.Empty;
        private DateTime startDate;
        private DateTime endDate;
        private String keyWord;
        private List<string> categoryList;
        private string selectCategory;

        private ICommand newCommand;
        private ICommand modifyCommand;
        private ICommand deleteCommand;        
        private ICommand browseCommand;
        private ICommand queryCommand;
        private ICommand approveCommand;
        private Visibility canAddShow;
        

        IEntityService entityservice;

        [ImportingConstructor]
        public GggsPublishNoticeListViewModel(IGggsPublishNoticeListView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.QueryableGggsPublishNotice != null)
            {
                this.gggsPublishNotice = entityservice.QueryableGggsPublishNotice;
            }
            else
            {
                this.gggsPublishNotice = new List<GggsPublishNotice>().AsQueryable(); //以防没有数据时出现异常
            }


            categoryList = new List<string>();
            categoryList.Add(YcConstantTable.STR_GGGS_CATEGORY_ALL);
            categoryList.Add(YcConstantTable.STR_GGGS_CATEGORY_1);
            categoryList.Add(YcConstantTable.STR_GGGS_CATEGORY_2);
            categoryList.Add(YcConstantTable.STR_GGGS_CATEGORY_3);




        }

        public List<string> CategoryList
        {
            get
            {
                return categoryList;
            }
            set
            {
                if (categoryList != value)
                {
                    categoryList = value;
                    RaisePropertyChanged("CategoryList");
                }
            }
        }

        public string SelectCategory
        {
            get { return selectCategory; }
            set
            {
                if (selectCategory != value)
                {
                    selectCategory = value;
                    RaisePropertyChanged("SelectCategory");
                }
            }
        }

        public IQueryable<GggsPublishNotice> GggsPublishNotice
        {
            get
            {
                return gggsPublishNotice;
            }
            set
            {
                if (gggsPublishNotice != value)
                {
                    gggsPublishNotice = value;
                    RaisePropertyChanged("GggsPublishNotice");
                }
            }
        }

        public GggsPublishNotice SelectedGggsPublishNotice
        {
            get { return selectedGggsPublishNotice; }
            set
            {
                if (selectedGggsPublishNotice != value)
                {
                    selectedGggsPublishNotice = value;
                    RaisePropertyChanged("SelectedGggsPublishNotice");
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

        public Visibility CanAddShow
        {
            get
            {
                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.Gggs.PublishNotice.Add"))
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

    