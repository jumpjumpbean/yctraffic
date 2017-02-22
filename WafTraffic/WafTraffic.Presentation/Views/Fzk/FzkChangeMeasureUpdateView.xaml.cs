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
    /// FzkChangeMeasureUpdateView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IFzkChangeMeasureUpdateView))]
    public partial class FzkChangeMeasureUpdateView  : UserControl, IFzkChangeMeasureUpdateView
    {
        private readonly Lazy<FzkChangeMeasureUpdateViewModel> viewModel;

        public FzkChangeMeasureUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<FzkChangeMeasureUpdateViewModel>(() => ViewHelper.GetViewModel<FzkChangeMeasureUpdateViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            // this.tbUploadFile.Text = string.Empty;
        }

        //private void btnUpload_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog open = new OpenFileDialog();
        //    open.Multiselect = false;
        //    open.Title = "选择上传文件";
        //    open.Filter = "全部文件|*.*";
        //    open.FilterIndex = 1;
        //    if ((bool)open.ShowDialog().GetValueOrDefault())//打开
        //    {
        //        try
        //        {
        //            this.tbUploadFile.Text = open.SafeFileName;
        //            viewModel.Value.UploadFullPath = open.FileName;
        //            viewModel.Value.ChangeMeasureEntity.UploadFileName = open.SafeFileName;                    
        //        }
        //        catch (System.Exception ex)
        //        {
        //            CurrentLoginService.Instance.LogException(ex);
        //        }
        //    }
        //}

        //public void Show_Loading()
        //{
        //    this._loading.Visibility = System.Windows.Visibility.Visible;
        //}

        //public void Shutdown_Loading()
        //{
        //    this._loading.Visibility = System.Windows.Visibility.Collapsed;
        //}
    }
}
    