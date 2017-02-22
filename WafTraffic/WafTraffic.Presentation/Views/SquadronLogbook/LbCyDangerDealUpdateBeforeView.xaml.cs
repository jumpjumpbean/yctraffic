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
using System.Windows.Media.Imaging;
using DotNet.Business;
using WafTraffic.Applications.Utils;
using WafTraffic.Applications.Common;
using System.Drawing;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// LbCyDangerDealUpdateBeforeView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ILbCyDangerDealUpdateBeforeView))]
    public partial class LbCyDangerDealUpdateBeforeView  : UserControl, ILbCyDangerDealUpdateBeforeView
    {
        private readonly Lazy<LbCyDangerDealUpdateBeforeViewModel> viewModel;
        private const int Type_ContentImg = 1;
        private const int Type_Rectification = 2;
        private const int Type_ReviewImg = 3;

        public LbCyDangerDealUpdateBeforeView()
        {
            InitializeComponent();
            viewModel = new Lazy<LbCyDangerDealUpdateBeforeViewModel>(() => ViewHelper.GetViewModel<LbCyDangerDealUpdateBeforeViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            //Loaded -= FirstTimeLoadedHandler;           
            int status = Convert.ToInt32(viewModel.Value.DangerDealEntity.Status);

            try
            {
                viewModel.Value.ContentLocalPath = string.Empty;
                viewModel.Value.RectificationLocalPath = string.Empty;
                viewModel.Value.ReviewLocalPath = string.Empty;
                //viewModel.Value.ShowSignImgCommand.Execute(null);
                //viewModel.Value.ShowThumbCommand.Execute(null);
                //this.imgContentImg.Source = new Bitmap(WafTraffic.Presentation.Properties.Resources.expired_picture).GetImageSourceByBitmap();
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void btnContentImgSelect_Click(object sender, RoutedEventArgs e)
        {
            //byte[] content;
            string picName;
            string localPath;
            UploadPicture(out localPath, out picName);
            //viewModel.Value.DangerDealEntity.ContentImg = content;
            viewModel.Value.ContentLocalPath = localPath;
            viewModel.Value.DangerDealEntity.ContentImgName = picName;
            this.tbContentImg.Text = picName;
        }

        //private void btnRectificationSelect_Click(object sender, RoutedEventArgs e)
        //{
        //    //byte[] content;
        //    string picName;
        //    string localPath;
        //    UploadPicture(out localPath, out picName);
        //    //viewModel.Value.DangerDealEntity.Rectification = content;
        //    viewModel.Value.RectificationLocalPath = localPath;
        //    viewModel.Value.DangerDealEntity.RectificationName = picName;
        //    this.tbRectification.Text = picName;
        //}

        /*
        private void btnReviewImgSelect_Click(object sender, RoutedEventArgs e)
        {
            //byte[] content;
            string picName;
            string localPath;
            UploadPicture(out localPath, out picName);
            //viewModel.Value.DangerDealEntity.ReviewImg = content;
            viewModel.Value.ReviewLocalPath = localPath;
            viewModel.Value.DangerDealEntity.ReviewImgName = picName;
            this.tbReviewImg.Text = picName;
        }*/

        private void UploadPicture(out string localPath, out string picName)
        {
            OpenFileDialog open = new OpenFileDialog();//定义打开文本框实体
            open.Multiselect = false;
            open.Title = "上传附件";//对话框标题
            open.Filter = "全部文件|*.*";//文件扩展名
            open.FilterIndex = 1;
            if ((bool)open.ShowDialog().GetValueOrDefault())//打开
            {
                localPath = open.FileName;
                picName = open.SafeFileName;
                //text = open.FileName;
            }
            else
            {
                localPath = string.Empty;
                picName = string.Empty;
                //text = string.Empty;
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();

            double top = printArea.Margin.Top;
            top += 60;                  //top + 60 , 以防打印顶头
            printArea.Margin = new Thickness(0, top, 0, 0);

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    dialog.PrintVisual(printArea, "中队台账");
                }
                catch (SystemException)
                {
                    MessageBox.Show("打印出错");
                }
            }

            top -= 60;
            printArea.Margin = new Thickness(0, top, 0, 0);
        }

        public void Show_Loading(LoadingType type)
        {
            switch (type)
            {
                //case LoadingType.Content:
                //    this.loadingContent.Visibility = System.Windows.Visibility.Visible;
                ////    break;
                //case LoadingType.Rectification:
                //    this.loadingRectification.Visibility = System.Windows.Visibility.Visible;
                //    break;
                case LoadingType.Review:
                    //this.loadingReview.Visibility = System.Windows.Visibility.Visible;
                    break;
                case LoadingType.View:
                default:
                    this._loading.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }

        public void Shutdown_Loading(LoadingType type)
        {
            switch (type)
            {
                //case LoadingType.Content:
                //    this.loadingContent.Visibility = System.Windows.Visibility.Collapsed;
                //    break;
                //case LoadingType.Rectification:
                //    this.loadingRectification.Visibility = System.Windows.Visibility.Collapsed;
                //    break;
                case LoadingType.Review:
                    //this.loadingReview.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case LoadingType.View:
                default:
                    this._loading.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }
    }
}
    