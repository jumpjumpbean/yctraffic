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
    /// UserEditView.xaml 的交互逻辑
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
            OpenFileDialog open = new OpenFileDialog();//定义打开文本框实体
            open.Multiselect = false;
            open.Title = "录入图片";//对话框标题
            open.Filter = "jpeg图片|*.jpg;*.jpeg|bmp图片|*.bmp|全部文件|*.*";//文件扩展名
            open.FilterIndex = 1;
            if ((bool)open.ShowDialog().GetValueOrDefault())//打开
            {
                if (ImageUtil.Instance.ValidateImage(open.FileName, 100 * 1024))
                {
                    viewModel.Value.SignPath = open.FileName;
                    //viewModel.Value.User.Description = open.SafeFileName;
                    tbSign.Text = open.SafeFileName;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("签名图片大小不能超过100k，请重新上传", "错误", 
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
    