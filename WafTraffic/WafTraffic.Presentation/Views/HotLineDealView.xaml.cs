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
using System.Windows.Media.Imaging;
using WafTraffic.Domain.Common;
using WafTraffic.Applications.Common;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// HotLineDealView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IHotLineDealView))]
    public partial class HotLineDealView  : UserControl, IHotLineDealView
    {
        private readonly Lazy<HotLineDealViewModel> viewModel;  
        public HotLineDealView()
        {
            InitializeComponent();
            viewModel = new Lazy<HotLineDealViewModel>(() => ViewHelper.GetViewModel<HotLineDealViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            //Loaded -= FirstTimeLoadedHandler;
            try
            {
                btnDeal.Visibility = Visibility.Visible;
                btnSendDepartment.Visibility = Visibility.Visible;
                btnReplyDDZ.Visibility = Visibility.Visible;

                if (viewModel.Value.HotLineTask.StatusId >= Convert.ToInt32(HotLineStatus.Dealed))
                {
                    btnDeal.Visibility = Visibility.Collapsed;
                    btnSendDepartment.Visibility = Visibility.Collapsed;
                    btnReplyDDZ.Visibility = Visibility.Collapsed;
                }

                if (viewModel.Value.HotLineTask.StatusId != Convert.ToInt32(HotLineStatus.ToDDZ) && viewModel.Value.HotLineTask.StatusId != Convert.ToInt32(HotLineStatus.ToZW) && viewModel.Value.HotLineTask.StatusId != Convert.ToInt32(HotLineStatus.ReplyDDZ))
                {
                    btnSendDepartment.Visibility = Visibility.Collapsed;
                    //btnDeal.Visibility = Visibility.Collapsed;
                }

                if (viewModel.Value.HotLineTask.StatusId != Convert.ToInt32(HotLineStatus.DDZToKS))
                {
                    btnReplyDDZ.Visibility = System.Windows.Visibility.Collapsed;
                }

                //else if (viewModel.Value.HotLineTask.IsComplainPolice != 1)
                //{
                //    btnReplyDDZ.Visibility = System.Windows.Visibility.Collapsed;
                //}

                if (viewModel.Value.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ZWToKS) || viewModel.Value.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.DDZToKS))
                {
                    cbxDepartment.Visibility = Visibility.Collapsed;
                    tbDepartment.Visibility = Visibility.Visible;
                    tbDepartNeed.Visibility = Visibility.Collapsed;
                }
                else
                {
                    cbxDepartment.Visibility = Visibility.Visible;
                    tbDepartment.Visibility = Visibility.Collapsed;
                    tbDepartNeed.Visibility = Visibility.Visible;
                }

                if (viewModel.Value.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ToDDZ) || viewModel.Value.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ToZW))
                {
                    tbSuggest.Visibility = System.Windows.Visibility.Collapsed;
                    tbDealSuggest.Visibility = System.Windows.Visibility.Visible;
                    tbResult.Visibility = System.Windows.Visibility.Collapsed;
                    tbDealResult.Visibility = System.Windows.Visibility.Visible;
                }
                else if (viewModel.Value.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ReplyDDZ))
                {
                    tbSuggest.Visibility = System.Windows.Visibility.Collapsed;
                    tbDealSuggest.Visibility = System.Windows.Visibility.Visible;
                    tbResult.Visibility = System.Windows.Visibility.Visible;
                    tbDealResult.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (viewModel.Value.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ZWToKS) || viewModel.Value.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.DDZToKS))
                {
                    tbSuggest.Visibility = System.Windows.Visibility.Visible;
                    tbDealSuggest.Visibility = System.Windows.Visibility.Collapsed;
                    tbResult.Visibility = System.Windows.Visibility.Collapsed;
                    tbDealResult.Visibility = System.Windows.Visibility.Visible;
                }
                viewModel.Value.ShowThumbCommand.Execute(null);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void btnSendDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Value.HotLineTask.OwnDepartmentId == null || viewModel.Value.HotLineTask.OwnDepartmentId == 0)
            {
                return;
            }

            if (viewModel.Value.HotLineTask.StatusId == Convert.ToInt32(HotLineStatus.ToZW))
            {
                viewModel.Value.HotLineTask.Status = "政委发给科室"; //待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  归档 :8
                viewModel.Value.HotLineTask.StatusId = Convert.ToInt32(HotLineStatus.ZWToKS);
            }
            else
            {
                viewModel.Value.HotLineTask.Status = "大队长发给科室"; //待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  归档 :8
                viewModel.Value.HotLineTask.StatusId = Convert.ToInt32(HotLineStatus.DDZToKS);
            }

            btnDeal.Visibility = Visibility.Collapsed;
            btnSendDepartment.Visibility = Visibility.Collapsed;
        }

        private void btnReplyDDZ_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Value.HotLineTask.Status = "回复大队长"; //待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  归档 :8
            viewModel.Value.HotLineTask.StatusId = Convert.ToInt32(HotLineStatus.ReplyDDZ);
            
            btnDeal.Visibility = Visibility.Collapsed;
            btnReplyDDZ.Visibility = Visibility.Collapsed;
        }

        private void btnDeal_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Value.HotLineTask.OwnDepartmentId == null || viewModel.Value.HotLineTask.OwnDepartmentId == 0)
            {
                return;
            }

            viewModel.Value.HotLineTask.Status = "已处理";
            viewModel.Value.HotLineTask.StatusId = Convert.ToInt32(HotLineStatus.Dealed);
            viewModel.Value.HotLineTask.SovleUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
            viewModel.Value.HotLineTask.SovleUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;

            btnDeal.Visibility = Visibility.Collapsed;
            btnSendDepartment.Visibility = Visibility.Collapsed;
            btnReplyDDZ.Visibility = Visibility.Collapsed;
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
