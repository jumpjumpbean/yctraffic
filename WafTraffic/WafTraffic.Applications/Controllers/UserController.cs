using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.Foundation;
using WafTraffic.Applications.Properties;
using WafTraffic.Applications.Services;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;

namespace WafTraffic.Applications.Controllers
{
    /// <summary>
    /// Responsible for the person management and the master / detail views.
    /// </summary>
    [Export]
    internal class UserController : Controller
    {
        private readonly CompositionContainer container;
        private readonly IMessageService messageService;
        private readonly IShellService shellService;
        private readonly IEntityService entityService;

        private readonly UserViewModel userViewModel;

        private UserListViewModel userListViewModel;

        private readonly DelegateCommand saveCommand;
        


        [ImportingConstructor]
        public UserController(CompositionContainer container, IMessageService messageService, IShellService shellService,
            IEntityService entityService)
        {
            this.container = container;
            this.messageService = messageService;
            this.shellService = shellService;
            this.entityService = entityService;

            this.userListViewModel = container.GetExportedValue<UserListViewModel>();
            //this.userViewModel = userViewModel;

            this.saveCommand = new DelegateCommand(() => Save(), CanSave);
        }


        public void Initialize()
        {
            //AddWeakEventListener(personViewModel, PersonViewModelPropertyChanged);
            AddWeakEventListener(userListViewModel, ModuleListViewModelPropertyChanged);

            //IUserListView userListView = container.GetExportedValue<IUserListView>();
            //userListViewModel = new UserListViewModel(userListView, entityService);

           

            //shellService.UserListView = userListViewModel.View;
            //shellService.UserView = userViewModel.View;

            //userListViewModel.SelectedUser = userListViewModel.Users.FirstOrDefault();
            //userListViewModel.Focus();
            userListViewModel.SaveCommand = this.saveCommand;
        }

        public bool CanSave()
        {
            return true;
        }

        public bool Save()
        {
            bool saved = false;
            if (!CanSave())
            {
                throw new InvalidOperationException("You must not call Save when CanSave returns false.");
            }
            try
            {
                entityService.Entities.SaveChanges();
                saved = true;

                messageService.ShowMessage(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                   "保存成功."));
            }
            catch (ValidationException e)
            {
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidEntities, e.Message));
            }
            catch (UpdateException e)
            {
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidFields, e.InnerException.Message));
            }
            return saved;
        }

        private void UpdateCommands()
        {
            saveCommand.RaiseCanExecuteChanged();
        }

        private void ModuleListViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedUser")
            {
                //userViewModel.User = userListViewModel.SelectedUser;
                //userListViewModel.User = userListViewModel.SelectedUser;
                //UpdateCommands();
            }
            if (e.PropertyName == "SaveCommand")
            {
                UpdateCommands();
            }
        }

        //private void PersonViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    //if (e.PropertyName == "IsValid")
        //    //{
        //    //    UpdateCommands();
        //    //}
        //}
    }
}
