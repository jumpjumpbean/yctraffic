using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class UserViewModel : ViewModel<IUserView>
    {
        private bool isValid = true;
        private Base_User user;
        private ICommand createNewEmailCommand;


        [ImportingConstructor]
        public UserViewModel(IUserView view)
            : base(view)
        {
        }


        public bool IsEnabled { get { return User != null; } }

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                if (isValid != value)
                {
                    isValid = value;
                    RaisePropertyChanged("IsValid");
                }
            }
        }

        public Base_User User
        {
            get { return user; }
            set
            {
                if (user != value)
                {
                    user = value;
                    RaisePropertyChanged("User");
                    RaisePropertyChanged("IsEnabled");
                }
            }
        }

        public ICommand CreateNewEmailCommand
        {
            get { return createNewEmailCommand; }
            set
            {
                if (createNewEmailCommand != value)
                {
                    createNewEmailCommand = value;
                    RaisePropertyChanged("CreateNewEmailCommand");
                }
            }
        }
    }
}
