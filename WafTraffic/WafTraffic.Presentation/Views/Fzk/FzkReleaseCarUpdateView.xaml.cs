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
    /// FzkReleaseCarUpdateView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IFzkReleaseCarUpdateView))]
    public partial class FzkReleaseCarUpdateView  : UserControl, IFzkReleaseCarUpdateView
    {
        private readonly Lazy<FzkReleaseCarUpdateViewModel> viewModel;

        public FzkReleaseCarUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<FzkReleaseCarUpdateViewModel>(() => ViewHelper.GetViewModel<FzkReleaseCarUpdateViewModel>(this));

            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.Value.ShowSignImgCommand.Execute(null);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
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
                    viewModel.Value.ReleaseCarEntity.StrSpare1 = open.SafeFileName;
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
    