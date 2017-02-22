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
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using WafTraffic.Applications.Utils;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// ZhzxTrafficViolationDetailsView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IZhzxTrafficViolationDetailsView))]
    public partial class ZhzxTrafficViolationDetailsView  : UserControl, IZhzxTrafficViolationDetailsView
    {
        private readonly Lazy<ZhzxTrafficViolationDetailsViewModel> viewModel;

        public ZhzxTrafficViolationDetailsView()
        {
            InitializeComponent();
            viewModel = new Lazy<ZhzxTrafficViolationDetailsViewModel>(() => ViewHelper.GetViewModel<ZhzxTrafficViolationDetailsViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                //viewModel.Value.ComposedThumbnailImg = string.Empty;
                //viewModel.Value.RectificationLocalPath = string.Empty;
                //viewModel.Value.ReviewLocalPath = string.Empty;
                viewModel.Value.ShowThumbCommand.Execute(null);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public void Show_Loading()
        {
            this._loading.Visibility = System.Windows.Visibility.Visible;
        }

        public void Shutdown_Loading()
        {
            this._loading.Visibility = System.Windows.Visibility.Collapsed;
        }

    }

    public static class ImageSourceHelper
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Bitmap转换为ImageSource
        /// </summary>
        /// <param name="bitmap">图片</param>
        /// <returns>ImageSource</returns>
        public static ImageSource GetImageSourceByBitmap(this System.Drawing.Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            if (!DeleteObject(hBitmap))//记得要进行内存释放。否则会有内存不足的报错。
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;
        }
    }


}
    