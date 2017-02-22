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
    /// SgkAccidentUpdateView.xaml �Ľ����߼�
    /// </summary>
    [Export(typeof(ISgkAccidentUpdateView))]
    public partial class SgkAccidentUpdateView  : UserControl, ISgkAccidentUpdateView
    {
        private readonly Lazy<SgkAccidentUpdateViewModel> viewModel;

        public SgkAccidentUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<SgkAccidentUpdateViewModel>(() => ViewHelper.GetViewModel<SgkAccidentUpdateViewModel>(this));

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

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();

            double top = printArea.Margin.Top;
            top += 60;                  //top + 60 , �Է���ӡ��ͷ
            printArea.Margin = new Thickness(0, top, 0, 0);

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    dialog.PrintVisual(printArea, "�ж�̨��");
                }
                catch (SystemException)
                {
                    MessageBox.Show("��ӡ����");
                }
            }

            top -= 60;
            printArea.Margin = new Thickness(0, top, 0, 0);
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
    