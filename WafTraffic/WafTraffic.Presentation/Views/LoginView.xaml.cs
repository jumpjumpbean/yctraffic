using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WafTraffic.Applications.Views;
using System.ComponentModel.Composition;
using WafTraffic.Applications.ViewModels;
using System.Waf.Applications;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
   [Export(typeof(ILoginView))]
    public partial class LoginView : UserControl, ILoginView
    {
       private readonly Lazy<LoginViewModel> viewModel;

        public LoginView()
        {
            InitializeComponent();
            viewModel = new Lazy<LoginViewModel>(() => ViewHelper.GetViewModel<LoginViewModel>(this));
            
        }

       private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(tbUserName);
        }

        private LoginViewModel ViewModel { get { return viewModel.Value; } }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void tbnLogIn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.Password = this.pbPwd.Password;
        }
    }
}
