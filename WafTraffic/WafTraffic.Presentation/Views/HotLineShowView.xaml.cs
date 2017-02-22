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
    /// HotLineShowView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IHotLineShowView))]
    public partial class HotLineShowView : UserControl, IHotLineShowView
    {
        private readonly Lazy<HotLineShowViewModel> viewModel;  
        public HotLineShowView()
        {
            InitializeComponent();
            viewModel = new Lazy<HotLineShowViewModel>(() => ViewHelper.GetViewModel<HotLineShowViewModel>(this));

            //Loaded += FirstTimeLoadedHandler;
        }

        //private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        //{
        //    //Loaded -= FirstTimeLoadedHandler;
        //    try
        //    {
        //        viewModel.Value.ShowThumbCommand.Execute(null);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        CurrentLoginService.Instance.LogException(ex);
        //    }
        //}

        public void ShowLoading(LoadingType type)
        {
            switch (type)
            {
                //case LoadingType.Content:
                //    this.loadingContent.Visibility = System.Windows.Visibility.Visible;
                //    break;
                //case LoadingType.VerifyFile:
                //    this.loadingVerify.Visibility = System.Windows.Visibility.Visible;
                //    break;;
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
                //case LoadingType.VerifyFile:
                //    this.loadingVerify.Visibility = System.Windows.Visibility.Collapsed;
                //    break;
                case LoadingType.View:
                default:
                    this._loading.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }

        private void VerifyDownload_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.FileType = HotLineFileType.VerifyFile;
        }

        private void ContentDownload_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.FileType = HotLineFileType.ContentFile;
        }
    }
}
