using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Windows;
using DotNet.Business;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using WafTraffic.Applications.Common;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// HotLineCheckView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IHotLineCheckView))]
    public partial class HotLineCheckView : UserControl, IHotLineCheckView
    {
        private readonly Lazy<HotLineCheckViewModel> viewModel;  
        public HotLineCheckView()
        {
            InitializeComponent();
            viewModel = new Lazy<HotLineCheckViewModel>(() => ViewHelper.GetViewModel<HotLineCheckViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            //Loaded -= FirstTimeLoadedHandler;
            viewModel.Value.VerifyLocalPath = string.Empty;
            //viewModel.Value.ShowThumbCommand.Execute(null);
        }
        
        private void btnSelect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();//定义打开文本框实体
            open.Multiselect = false;
            open.Title = "上传附件";//对话框标题
            open.Filter = "全部文件|*.*";//文件扩展名
            open.FilterIndex = 1;
            if ((bool)open.ShowDialog().GetValueOrDefault())//打开
            {
                viewModel.Value.VerifyLocalPath = open.FileName;
                viewModel.Value.HotLineTask.VerifyFileName = open.SafeFileName;
                tbVerifyFile.Text = open.SafeFileName;
            }
            else
            {
                viewModel.Value.VerifyLocalPath = string.Empty;
                viewModel.Value.HotLineTask.VerifyFileName = string.Empty;
                tbVerifyFile.Text = string.Empty;
            }
        }

        public void ShowLoading(LoadingType type)
        {
            switch (type)
            {
                //case LoadingType.Content:
                //    this.loadingContent.Visibility = System.Windows.Visibility.Visible;
                //    break; ;
                case LoadingType.View:
                default:
                    this._loading.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }

        public void ShutdownLoading(LoadingType type)
        {
            switch (type)
            {
                //case LoadingType.Content:
                //    this.loadingContent.Visibility = System.Windows.Visibility.Collapsed;
                //    break;
                case LoadingType.View:
                default:
                    this._loading.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }

        private void ContentDownload_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.FileType = HotLineFileType.ContentFile;
        }
    }
}
