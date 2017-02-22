using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using WafTraffic.Domain;
using DotNet.Business;
using System.Data;
using WafTraffic.Applications.Common;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// PersonArchiveView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IPersonArchiveView))]
    public partial class PersonArchiveView  : UserControl, IPersonArchiveView
    {
        private readonly Lazy<PersonArchiveViewModel> viewModel;
        
        public PersonArchiveView()
        {
            InitializeComponent();
            viewModel = new Lazy<PersonArchiveViewModel>(() => ViewHelper.GetViewModel<PersonArchiveViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            //Loaded -= FirstTimeLoadedHandler;
            //viewModel.Value.ShowThumbCommand.Execute(null);

            if (viewModel.Value.Operation == "Browse")
            {
                btnSave.Visibility = System.Windows.Visibility.Collapsed;
                //tbPhoto.Visibility = System.Windows.Visibility.Collapsed;
                //btnSelect.Visibility = System.Windows.Visibility.Collapsed;
                //gdPhoto.Visibility = System.Windows.Visibility.Visible;
                //控件只读
                tbName.IsReadOnly = true;

                cbxSex.IsEnabled = false;
                dateBirthday.IsReadOnly = true;

                tbJobTitle.IsReadOnly = true;
                tbRetireTime.IsReadOnly = true;
                tbEduBackground.IsReadOnly = true;
                tbDegreeInfo.IsReadOnly = true;
                tbWorkResume.IsReadOnly = true;
                tbCommendInfo.IsReadOnly = true;
                tbHealthInfo.IsReadOnly = true;
                tbCreateUserName.IsReadOnly = true;
                tbMotto.IsReadOnly = true;
                tbApproverName.IsReadOnly = true;
                dateCreateTime.IsReadOnly = true;


                cbxPolitical.IsEnabled = false;
                datePartyTime.IsReadOnly = true;
                cbxEducation.IsEnabled = false;
                tbGraduation.IsReadOnly = true;
                datePoliceTime.IsReadOnly = true;
                tbNative.IsReadOnly = true;
                tbHomeLocation.IsReadOnly = true;
                tbPoliceNo.IsReadOnly = true;

                tbLinkMethod.IsReadOnly = true;
                tbCardNo.IsReadOnly = true;
                dateWorkTime.IsReadOnly = true;
                tbResume.IsReadOnly = true;
                tbHumanRelation.IsReadOnly = true;
                tbCommentMark.IsReadOnly = true;
            }
            else
            {
                btnSave.Visibility = System.Windows.Visibility.Visible;
                //tbPhoto.Visibility = System.Windows.Visibility.Visible;
                //btnSelect.Visibility = System.Windows.Visibility.Visible;
                //gdPhoto.Visibility = System.Windows.Visibility.Collapsed;
                //取消控件只读
                tbName.IsReadOnly = false;

                cbxSex.IsEnabled = true;
                dateBirthday.IsReadOnly = false;

                tbJobTitle.IsReadOnly = false;
                tbRetireTime.IsReadOnly = false;
                tbEduBackground.IsReadOnly = false;
                tbDegreeInfo.IsReadOnly = false;
                tbWorkResume.IsReadOnly = false;
                tbCommendInfo.IsReadOnly = false;
                tbHealthInfo.IsReadOnly = false;
                tbCreateUserName.IsReadOnly = false;
                tbMotto.IsReadOnly = false;
                tbApproverName.IsReadOnly = false;
                dateCreateTime.IsReadOnly = false;


                cbxPolitical.IsEnabled = true;
                datePartyTime.IsReadOnly = false;
                cbxEducation.IsEnabled = true;
                tbGraduation.IsReadOnly = false;
                datePoliceTime.IsReadOnly = false;
                tbNative.IsReadOnly = false;
                tbHomeLocation.IsReadOnly = false;
                tbPoliceNo.IsReadOnly = false;

                tbLinkMethod.IsReadOnly = false;
                tbCardNo.IsReadOnly = false;
                dateWorkTime.IsReadOnly = false;
                tbResume.IsReadOnly = false;
                tbHumanRelation.IsReadOnly = false;
                tbCommentMark.IsReadOnly = false;
            }

        }

        //private void btnSelect_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog open = new OpenFileDialog();//定义打开文本框实体
        //    open.Multiselect = false;
        //    open.Title = "录入图片";//对话框标题
        //    open.Filter = "jpeg图片|*.jpg;*.jpeg|bmp图片|*.bmp|全部文件|*.*";//文件扩展名
        //    open.FilterIndex = 1;
        //    if ((bool)open.ShowDialog().GetValueOrDefault())//打开
        //    {
        //        viewModel.Value.PhotoLocalPath = open.FileName;
        //        viewModel.Value.PersonArchive.PhotoName = open.SafeFileName;
        //        tbPhoto.Text = open.SafeFileName;
        //    }
        //    else
        //    {
        //        viewModel.Value.PhotoLocalPath = string.Empty;
        //        viewModel.Value.PersonArchive.PhotoName = string.Empty;
        //        tbPhoto.Text = string.Empty;
        //    }
        //}

        //public void Show_Loading(LoadingType type)
        //{
        //    switch (type)
        //    {
        //        case LoadingType.Photo:
        //            this.loadingPhoto.Visibility = System.Windows.Visibility.Visible;
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
        //        case LoadingType.Photo:
        //            this.loadingPhoto.Visibility = System.Windows.Visibility.Collapsed;
        //            break;
        //        case LoadingType.View:
        //        default:
        //            this._loading.Visibility = System.Windows.Visibility.Collapsed;
        //            break;
        //    }
        //}

    }
}
    