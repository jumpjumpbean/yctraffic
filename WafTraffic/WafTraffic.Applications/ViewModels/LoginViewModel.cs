using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;

namespace WafTraffic.Applications.ViewModels
{
     [Export]
    public class LoginViewModel : ViewModel<ILoginView>
    {
         private ICommand loginCommand;
         private string userName = string.Empty;
         private string password = string.Empty;

         [ImportingConstructor]
         public LoginViewModel(ILoginView view)
            : base(view)
        {
        }

         public ICommand LoginCommand
         {
             get { return loginCommand; }
             set
             {
                 if (loginCommand != value)
                 {
                     loginCommand = value;
                     RaisePropertyChanged("LoginCommand");
                 }
             }
         }

         public string UserName
         {
             get { return userName; }
             set
             {
                 if (userName != value)
                 {
                     userName = value;
                     RaisePropertyChanged("UserName");
                 }
             }
         }

         public string Password
         {
             get { return password; }
             set
             {
                 if (password != value)
                 {
                     password = value;
                     RaisePropertyChanged("Password");
                 }
             }
         }


         public string DocumentName { get { return "欢迎登录"; } }
    }
}
