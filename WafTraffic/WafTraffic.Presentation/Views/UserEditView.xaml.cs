using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using Microsoft.Win32;
using WafTraffic.Applications.Utils;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// UserEditView.xaml �Ľ����߼�
    /// </summary>
    [Export(typeof(IUserEditView))]
    public partial class UserEditView  : UserControl, IUserEditView
    {
        private readonly Lazy<UserEditViewModel> viewModel;

        public UserEditView()
        {
            InitializeComponent();
            viewModel = new Lazy<UserEditViewModel>(() => ViewHelper.GetViewModel<UserEditViewModel>(this));
        }

        private void btnSignSelect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();//������ı���ʵ��
            open.Multiselect = false;
            open.Title = "¼��ͼƬ";//�Ի������
            open.Filter = "jpegͼƬ|*.jpg;*.jpeg|bmpͼƬ|*.bmp|ȫ���ļ�|*.*";//�ļ���չ��
            open.FilterIndex = 1;
            if ((bool)open.ShowDialog().GetValueOrDefault())//��
            {
                if (ImageUtil.Instance.ValidateImage(open.FileName, 100 * 1024))
                {
                    viewModel.Value.SignPath = open.FileName;
                    //viewModel.Value.User.Description = open.SafeFileName;
                    tbSign.Text = open.SafeFileName;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("ǩ��ͼƬ��С���ܳ���100k���������ϴ�", "����", 
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    viewModel.Value.SignPath = string.Empty;
                    //viewModel.Value.User.Description = string.Empty;
                    tbSign.Text = string.Empty;
                }
            }
            else
            {
                viewModel.Value.SignPath = string.Empty;
                //viewModel.Value.User.Description = string.Empty;
                tbSign.Text = string.Empty;
            }
        }
    }
}
    