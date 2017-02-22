using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Waf;
using WafTraffic.Domain;
using DotNet.Utilities;
using WafTraffic.Presentation.Services;
using DotNet.Business;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using WafTraffic.Applications.Common;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// GggsPublishNoticeDetailView.xaml �Ľ����߼�
    /// </summary>
    [Export(typeof(IGggsPublishNoticeDetailView))]
    public partial class GggsPublishNoticeDetailView : UserControl, IGggsPublishNoticeDetailView
    {
        private readonly Lazy<GggsPublishNoticeDetailViewModel> viewModel;

        public GggsPublishNoticeDetailView()
        {
            InitializeComponent();
            viewModel = new Lazy<GggsPublishNoticeDetailViewModel>(() => ViewHelper.GetViewModel<GggsPublishNoticeDetailViewModel>(this));

        }

        private void btnAttachmentName1Select_Click(object sender, RoutedEventArgs e)
        {
            string picName;
            string localPath;
            UploadPicture(out localPath, out picName);

            this.tbAttachFileName1.Text = picName;
            viewModel.Value.GggsPublishNotice.AttachmentName1 = picName;
            viewModel.Value.FileLocalPath = localPath;
        }

        private void UploadPicture(out string localPath, out string picName)
        {
            OpenFileDialog open = new OpenFileDialog();//������ı���ʵ��
            open.Multiselect = false;
            open.Title = "�ϴ�����";//�Ի������
            open.Filter = "ȫ���ļ�|*.*";//�ļ���չ��
            open.FilterIndex = 1;
            if ((bool)open.ShowDialog().GetValueOrDefault())//��
            {
                localPath = open.FileName;
                picName = open.SafeFileName;
            }
            else
            {
                localPath = string.Empty;
                picName = string.Empty;
            }
        }

        public void Show_Loading(LoadingType type)
        {
            switch (type)
            {
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
                case LoadingType.View:
                default:
                    this._loading.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }
    }
}
