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
    public class PersonArchiveListViewModel : ViewModel<IPersonArchiveListView>
    {
        private PersonArchiveTable selectedPersonArchive;
        private Node selectedTreeNode;
        private int selectedPolicalTypeId;
        private IQueryable<PersonArchiveTable> personArchives;

        private ICommand newCommand;
        private ICommand modifyCommand;
        private ICommand deleteCommand;        
        private ICommand browseCommand;

        IEntityService entityservice;

         [ImportingConstructor]
        public PersonArchiveListViewModel(IPersonArchiveListView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.EnumHotLineTasks != null)
            {
                this.personArchives = entityservice.EnumPersonArchives;
            }
            else
            {
                this.personArchives = new List<PersonArchiveTable>().AsQueryable(); //以防没有数据时出现异常
            }
        }

        public IQueryable<PersonArchiveTable> PersonArchives
        {
            get
            {
                return personArchives;
            }
            set
            {
                personArchives = value;
                RaisePropertyChanged("PersonArchives");
            }
        }

        public PersonArchiveTable SelectedPersonArchive
        {
            get { return selectedPersonArchive; }
            set
            {
                if (selectedPersonArchive != value)
                {
                    selectedPersonArchive = value;
                    RaisePropertyChanged("SelectedPersonArchive");
                }
            }
        }

        public Node SelectedTreeNode
        {
            get { return selectedTreeNode; }
            set
            {
                if (selectedTreeNode != value)
                {
                    selectedTreeNode = value;
                    RaisePropertyChanged("SelectedTreeNode");
                }
            }
        }

        public int SelectedPolicalTypeId
        {
            get { return selectedPolicalTypeId; }
            set
            {
                if (selectedPolicalTypeId != value)
                {
                    selectedPolicalTypeId = value;
                    RaisePropertyChanged("SelectedPolicalTypeId");
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

        public void GridRefresh()
        {
            ViewCore.PagingReload();
        }

    }
}
