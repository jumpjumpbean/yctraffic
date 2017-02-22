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
    /// HealthArchiveView.xaml �Ľ����߼�
    /// </summary>
    [Export(typeof(IHealthArchiveView))]
    public partial class HealthArchiveView  : UserControl, IHealthArchiveView
    {
        private readonly Lazy<HealthArchiveViewModel> viewModel;

        public HealthArchiveView()
        {
            InitializeComponent();
            viewModel = new Lazy<HealthArchiveViewModel>(() => ViewHelper.GetViewModel<HealthArchiveViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            //Loaded -= FirstTimeLoadedHandler;

            if ( string.IsNullOrEmpty(viewModel.Value.HealthArchive.CreateUserName) )
            {
                tbkCreateName.Text = CurrentLoginService.Instance.CurrentUserInfo.RealName;
            }

            if (viewModel.Value.Operation == "Browse")
            {
                btnSave.Visibility = System.Windows.Visibility.Hidden;
                //tbCheckFile.Visibility = System.Windows.Visibility.Collapsed;
                //btnSelect.Visibility = System.Windows.Visibility.Collapsed;
                //gdCheckFile.Visibility = System.Windows.Visibility.Visible;

                //�ؼ�ֻ��
                tbProgress.IsReadOnly = true;
                tbName.IsReadOnly = true;
                _minimum.IsReadOnly = true;
                tbDesc.IsReadOnly = true;
            }
            else
            {
                btnSave.Visibility = System.Windows.Visibility.Visible;
                //tbCheckFile.Visibility = System.Windows.Visibility.Visible;
                //btnSelect.Visibility = System.Windows.Visibility.Visible;
                //gdCheckFile.Visibility = System.Windows.Visibility.Collapsed;
                //ȡ���ؼ�ֻ��
                tbProgress.IsReadOnly = false;
                tbName.IsReadOnly = false;
                _minimum.IsReadOnly = false;
                tbDesc.IsReadOnly = false;
            }

        }

        //private void btnSelect_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog open = new OpenFileDialog();//������ı���ʵ��
        //    open.Multiselect = false;
        //    open.Title = "¼��ͼƬ";//�Ի������
        //    open.Filter = "jpegͼƬ|*.jpg;*.jpeg|bmpͼƬ|*.bmp|ȫ���ļ�|*.*";//�ļ���չ��
        //    open.FilterIndex = 1;
        //    if ((bool)open.ShowDialog().GetValueOrDefault())//��
        //    {
        //        viewModel.Value.CheckFileLocalPath = open.FileName;
        //        viewModel.Value.HealthArchive.CheckFileName = open.SafeFileName;
        //        tbCheckFile.Text = open.SafeFileName;
        //    }
        //    else
        //    {
        //        viewModel.Value.CheckFileLocalPath = string.Empty;
        //        viewModel.Value.HealthArchive.CheckFileName = string.Empty;
        //        tbCheckFile.Text = string.Empty;
        //    }
        //}

        //public void Show_Loading(LoadingType type)
        //{
        //    switch (type)
        //    {
        //        case LoadingType.CheckFile:
        //            this.loadingCheckFile.Visibility = System.Windows.Visibility.Visible;
        //            break;
        //        case LoadingType.View:
        //        default:
        //            this._loading.Visibility = System.Windows.Visibility.Visible;
        //            break;
        //    }
        //}

        //public void Shutdown_Loading(LoadingType type)
        //{
        //    switch (type)
        //    {
        //        case LoadingType.CheckFile:
        //            this.loadingCheckFile.Visibility = System.Windows.Visibility.Collapsed;
        //            break;
        //        case LoadingType.View:
        //        default:
        //            this._loading.Visibility = System.Windows.Visibility.Collapsed;
        //            break;
        //    }
        //}

    }
}
    