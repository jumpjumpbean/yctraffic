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
using System.ComponentModel.Composition;
using WafTraffic.Applications.Views;
using WafTraffic.Applications.ViewModels;
using System.Waf.Applications;
using System.IO;
using DotNet.Business;
using WafTraffic.Applications.Utils;

namespace WafTraffic.Presentation.Views.SquadronLogbook
{
    /// <summary>
    /// LbStaticLogbookDocView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ILbStaticLogbookDocView))]
    public partial class LbStaticLogbookDocView : UserControl, ILbStaticLogbookDocView
    {
        private readonly Lazy<LbStaticLogbookDocViewModel> viewModel;

        public LbStaticLogbookDocView()
        {
            InitializeComponent();
            try
            {
                viewModel = new Lazy<LbStaticLogbookDocViewModel>(() => ViewHelper.GetViewModel<LbStaticLogbookDocViewModel>(this));

                this.framer.BeginInit();
                this.framer.Titlebar = false;
                this.framer.Toolbars = false;
                this.framer.Menubar = false;
                // 防止出现Unable to display the inactive document问题
                this.framer.ActivationPolicy = DSOFramer.dsoActivationPolicy.dsoKeepUIActiveOnAppDeactive;
                this.framer.EndInit();

                Loaded += new RoutedEventHandler(LbStaticLogbookDocView_Loaded);
                Unloaded += new RoutedEventHandler(LbStaticLogbookDocView_Unloaded);
            }
            catch (Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        // 防止退出时未关闭文件导致再打开时崩溃问题
        void LbStaticLogbookDocView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void LbStaticLogbookDocView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.framer.Open(viewModel.Value.FilePath);
                this.framer.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开失败，仅支持office标准格式文档。 请先下载该文件再查看。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.framer.Close();
                File.Delete(viewModel.Value.FilePath);
            }
            catch (Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public void Close()
        {
            try
            {
                if (File.Exists(viewModel.Value.FilePath))
                {
                    this.framer.Close();
                    File.Delete(viewModel.Value.FilePath);
                }
            }
            catch (Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }
    }
}
