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
    /// CgsVehicleAdminUpdateView.xaml �Ľ����߼�
    /// </summary>
    [Export(typeof(ICgsVehicleAdminUpdateView))]
    public partial class CgsVehicleAdminUpdateView  : UserControl, ICgsVehicleAdminUpdateView
    {
        private readonly Lazy<CgsVehicleAdminUpdateViewModel> viewModel;

        public CgsVehicleAdminUpdateView()
        {
            InitializeComponent();
            viewModel = new Lazy<CgsVehicleAdminUpdateViewModel>(() => ViewHelper.GetViewModel<CgsVehicleAdminUpdateViewModel>(this));

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

        //private void btnPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    PrintDialog dialog = new PrintDialog();

        //    double top = printArea.Margin.Top;
        //    top += 60;                  //top + 60 , �Է���ӡ��ͷ
        //    printArea.Margin = new Thickness(0, top, 0, 0);

        //    if (dialog.ShowDialog() == true)
        //    {
        //        try
        //        {
        //            dialog.PrintVisual(printArea, "�ж�̨��");
        //        }
        //        catch (SystemException)
        //        {
        //            MessageBox.Show("��ӡ����");
        //        }
        //    }

        //    top -= 60;
        //    printArea.Margin = new Thickness(0, top, 0, 0);
        //}

        private void btnAttachmentName1Select_Click(object sender, RoutedEventArgs e)
        {
            string picName1;
            string localPath1;
            UploadPicture(out localPath1, out picName1);

            this.tbAttachFileName1.Text = picName1;
            viewModel.Value.PunishCaseEntity.AttachmentName1 = picName1;
            viewModel.Value.FileLocalPath1 = localPath1;
        }

        private void btnAttachmentName2Select_Click(object sender, RoutedEventArgs e)
        {
            string picName2;
            string localPath2;
            UploadPicture(out localPath2, out picName2);

            this.tbAttachFileName2.Text = picName2;
            viewModel.Value.PunishCaseEntity.AttachmentName2 = picName2;
            viewModel.Value.FileLocalPath2 = localPath2;
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
    