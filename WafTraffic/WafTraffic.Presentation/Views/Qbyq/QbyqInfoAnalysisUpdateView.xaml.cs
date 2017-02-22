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
using WafTraffic.Applications.Common;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// QbyqInfoAnalysisUpdateView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IQbyqInfoAnalysisUpdateView))]
    public partial class QbyqInfoAnalysisUpdateView  : UserControl, IQbyqInfoAnalysisUpdateView
    {
        private readonly Lazy<QbyqInfoAnalysisUpdateViewModel> viewModel;

        public QbyqInfoAnalysisUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<QbyqInfoAnalysisUpdateViewModel>(() => ViewHelper.GetViewModel<QbyqInfoAnalysisUpdateViewModel>(this));

            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            this.dpCaseTime.IsEnabled = viewModel.Value.IsNewOrModify;
            this.btnBack.Visibility = viewModel.Value.BrowseVisibility;
            //this.btnPrint.Visibility = viewModel.Value.BrowseVisibility;
            this.btnSave.Visibility = viewModel.Value.NewOrModifyVisibility;
            this.btnCancel.Visibility = viewModel.Value.NewOrModifyVisibility;
        }

        private void btnAttachmentNameSelect_Click(object sender, RoutedEventArgs e)
        {
            string picName;
            string localPath;
            UploadPicture(out localPath, out picName);

            this.tbAttachFileName.Text = picName;
            viewModel.Value.PunishCaseEntity.AttachmentName = picName;
            viewModel.Value.FileLocalPath = localPath;
        }

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
    