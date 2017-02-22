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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class UserAdminViewModel : ViewModel<IUserAdminView>
    {
        private BaseUserEntity selectedUser;
        private List<BaseUserEntity> users;

        private ICommand newCommand;       
        private ICommand modifyCommand;
        private ICommand deleteCommand;

        [ImportingConstructor]
        public UserAdminViewModel(IUserAdminView view)
            : base(view)
        {
            //users = new List<BaseUserEntity>();
            //DataTable dt = DotNetService.Instance.UserService.GetDT(CurrentLoginService.Instance.CurrentUserInfo);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    users.Add(new BaseUserEntity(dr));
            //}                      
        }

        public BaseUserEntity SelectedUser
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

        public List<BaseUserEntity> Users
        {
            get 
            {
                users = new List<BaseUserEntity>();
                DataTable dt = DotNetService.Instance.UserService.GetDT(CurrentLoginService.Instance.CurrentUserInfo);
                foreach (DataRow dr in dt.Rows)
                {
                    users.Add(new BaseUserEntity(dr));
                }   

                return users; 
            }
            set
            {
                if (users != value)
                {
                    users = value;
                    RaisePropertyChanged("Users");
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

        public void GridRefresh()
        {
            ViewCore.PagingReload();
        }
    }
}
    