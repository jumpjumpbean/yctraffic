using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.DataModels;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class UserListViewModel : ViewModel<IUserListView>
    {
        private readonly IEnumerable<Base_User> users;

        private Base_User selectedUser;
        private ICommand saveCommand;

        [ImportingConstructor]
       public UserListViewModel(IUserListView view, IEntityService entityservice)
            : base(view)
        {
            if (entityservice.Users == null) { throw new ArgumentNullException("users"); }

            this.users = entityservice.Users;
        }


        public IEnumerable<Base_User> Users { get { return users; } }

       
        public IEnumerable<Base_User> UserCollectionView { get; set; }


        public Base_User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
                    RaisePropertyChanged("SelectedUser");
                }
            }
        }

   

        public void Focus()
        {
            ViewCore.FocusFirstCell();
        }

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                if (saveCommand != value)
                {
                    saveCommand = value;
                    RaisePropertyChanged("SaveCommand");
                }
            }
        }
      
    }
}
