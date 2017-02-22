using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows;
using WafTraffic.Domain.Common;
using WafTraffic.Applications.Common;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// HotLineView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IHotLineView))]
    public partial class HotLineView  : UserControl, IHotLineView
    {
        private readonly Lazy<HotLineViewModel> viewModel;

        public HotLineView()
        {
            InitializeComponent();
            viewModel = new Lazy<HotLineViewModel>(() => ViewHelper.GetViewModel<HotLineViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            //Loaded -= FirstTimeLoadedHandler;
            viewModel.Value.ContentLocalPath = string.Empty;
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
                viewModel.Value.ContentLocalPath = open.FileName;
                viewModel.Value.HotLineTask.ContentPictureName = open.SafeFileName;
                tbContentPicture.Text = open.SafeFileName;
            }
            else
            {
                viewModel.Value.ContentLocalPath = string.Empty;
                viewModel.Value.HotLineTask.ContentPictureName = string.Empty;
                tbContentPicture.Text = string.Empty;
            }
        }
      

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.HotLineTask.Status = "待处理"; ////待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  归档 :8
            viewModel.Value.HotLineTask.StatusId = Convert.ToInt32(HotLineStatus.WaitDeal);

        }

        private void btnSendDDZ_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.HotLineTask.Status = "发给大队长"; ////待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  归档 :8
            viewModel.Value.HotLineTask.StatusId = Convert.ToInt32(HotLineStatus.ToDDZ);

        }

        private void btnSendVice_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.HotLineTask.Status = "发给政委";////待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  归档 :8
            viewModel.Value.HotLineTask.StatusId = Convert.ToInt32(HotLineStatus.ToZW);

        }

        public void ShowLoading(LoadingType type)
        {
            switch (type)
            {
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
                case LoadingType.View:
                default:
                    this._loading.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }
    }
}
