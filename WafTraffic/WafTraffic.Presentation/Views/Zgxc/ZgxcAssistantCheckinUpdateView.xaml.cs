using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Domain;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using Microsoft.Win32;
using System.IO;
using DotNet.Business;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// ZgxcAssistantCheckinUpdateView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IZgxcAssistantCheckinUpdateView))]
    public partial class ZgxcAssistantCheckinUpdateView  : UserControl, IZgxcAssistantCheckinUpdateView
    {
        private readonly Lazy<ZgxcAssistantCheckinUpdateViewModel> viewModel;

        public ZgxcAssistantCheckinUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZgxcAssistantCheckinUpdateViewModel>(() => ViewHelper.GetViewModel<ZgxcAssistantCheckinUpdateViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            // this.tbUploadFile.Text = string.Empty;
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Title = "选择上传文件";
            open.Filter = "全部文件|*.*";
            open.FilterIndex = 1;
            if ((bool)open.ShowDialog().GetValueOrDefault())//打开
            {
                try
                {
                    this.tbUploadFile.Text = open.SafeFileName;
                    viewModel.Value.UploadFullPath = open.FileName;
                    viewModel.Value.AssistantCheckinEntity.UploadFileName = open.SafeFileName;                    
                }
                catch (System.Exception ex)
                {
                    CurrentLoginService.Instance.LogException(ex);
                }
            }
        }

        public void Show_Loading()
        {
            this._loading.Visibility = System.Windows.Visibility.Visible;
        }

        public void Shutdown_Loading()
        {
            this._loading.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
    