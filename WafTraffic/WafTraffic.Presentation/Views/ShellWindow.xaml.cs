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
using FirstFloor.ModernUI.Windows.Controls;
using WafTraffic.Presentation.Services;
using System.Windows.Media.Animation;
using WafTraffic.Applications.Utils;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// ShellWindow.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IShellView))]
    public partial class ShellWindow : ModernWindow, IShellView
    {
        private readonly Lazy<ShellViewModel> viewModel;

        private System.Windows.Point origin;
        private System.Windows.Point start;

        private double previousHeight;
        private double previousWidth;

        
        public ShellWindow()
        {
            InitializeComponent();

            viewModel = new Lazy<ShellViewModel>(() => ViewHelper.GetViewModel<ShellViewModel>(this));
            //this.WindowState = WindowState.Maximized;
            //this.Topmost = false;

            this.Left = 0;//设置位置
            this.Top = 0;
            Rect rc = SystemParameters.WorkArea;//获取工作区大小
            this.Width = rc.Width;
            this.Height = rc.Height;

            this.previousHeight = this._childWindow.Height;
            this.previousWidth = this._childWindow.Width;
        }

        public bool IsMaximized
        {
            get { return WindowState == WindowState.Maximized; }
            set
            {
                if (value)
                {
                    WindowState = WindowState.Maximized;
                }
                else if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void BtnAlarm_Dance()
        {

            RotateTransform rtf = new RotateTransform();
            Storyboard storyboard = new Storyboard();

            btnAlarm.RenderTransform = rtf;
          
            DoubleAnimation dbAscending = new DoubleAnimation(-15, 15, new Duration(TimeSpan.FromSeconds(0.1)));
            
            dbAscending.RepeatBehavior = new RepeatBehavior(10);

            storyboard.Children.Add(dbAscending);
            Storyboard.SetTarget(dbAscending, btnAlarm);
            Storyboard.SetTargetProperty(dbAscending, new PropertyPath("RenderTransform.Angle"));


            storyboard.Completed += new EventHandler(BtnAlarmGoBackAngle);

            storyboard.Begin();
        }

        // fuck~~~~~~~ storyboard.Begin()结束后，button是15°斜着的，不知道怎么把button正过来，
        // 只能再写个动画，把button从15°转回0°， ~~~~(>_<)~~~~    --add by Nick  
        public void BtnAlarmGoBackAngle(object sender, EventArgs e)
        {

            RotateTransform rtf = new RotateTransform();
            Storyboard storyboard = new Storyboard();

            btnAlarm.RenderTransform = rtf;

            DoubleAnimation dbAscending = new DoubleAnimation(15, 0, new Duration(TimeSpan.FromSeconds(0.05)));

            dbAscending.RepeatBehavior = new RepeatBehavior(1);

            storyboard.Children.Add(dbAscending);
            Storyboard.SetTarget(dbAscending, btnAlarm);
            Storyboard.SetTargetProperty(dbAscending, new PropertyPath("RenderTransform.Angle"));

            storyboard.Begin();
        }

        public void BtnAlarm_Sing()
        {
            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri(Environment.CurrentDirectory + @"\Resources\Audioes\notice.wav"));
            player.Volume = 1;
            player.Play();
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ChangImgSize(e.Delta > 0);
        }

        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (myImage.IsMouseCaptured) return;
            myImage.CaptureMouse();
            start = e.GetPosition(this);
            origin.X = myImage.RenderTransform.Value.OffsetX;
            origin.Y = myImage.RenderTransform.Value.OffsetY;
            e.Handled = true;
        }

        private void image1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            myImage.ReleaseMouseCapture();
        }

        private void image1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!myImage.IsMouseCaptured) return;
            System.Windows.Point point = e.MouseDevice.GetPosition(this);
            Matrix m = myImage.RenderTransform.Value;
            m.OffsetX = origin.X + (point.X - start.X);
            m.OffsetY = origin.Y + (point.Y - start.Y);

            myImage.RenderTransform = new MatrixTransform(m);
        }

        private void btnBig_Click(object sender, RoutedEventArgs e)
        {

            this.myImage.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1, 0, 0));

            this._childWindow.Height = SystemParameters.WorkArea.Height - 25;  // 25 is the vlaue of WindowContainer's margin.
            this._childWindow.Width = SystemParameters.WorkArea.Width;
            this._childWindow.Top = 0;
            this._childWindow.Left = 0;
        }

        private void btnSmall_Click(object sender, RoutedEventArgs e)
        {
            this._childWindow.Close();

            myImage.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1, 0, 0));
            
            this._childWindow.Height = previousHeight;
            this._childWindow.Width = previousWidth;

            this._childWindow.Top = SystemParameters.WorkArea.Height - this._childWindow.Height - 25; // 25 is the vlaue of WindowContainer's margin.
            this._childWindow.Left = 0;
            this._childWindow.Show();

        }

        private void ChangImgSize(bool big)
        {
            Matrix m = myImage.RenderTransform.Value;
            System.Windows.Point p = new System.Windows.Point((myImage.ActualWidth) / 2, (myImage.ActualHeight) / 2);
            if (big)
            {
                m.ScaleAtPrepend(1.1, 1.1, p.X, p.Y);
            }
            else
            {
                m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, p.X, p.Y);
            }

            myImage.RenderTransform = new MatrixTransform(m);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (viewModel.Value.CurrentPicId == 1)
                {
                    viewModel.Value.CurrentPicId = 2;    
                }
                else if (viewModel.Value.CurrentPicId == 2)
                {
                    viewModel.Value.CurrentPicId = 3;
                }
                else if (viewModel.Value.CurrentPicId == 3)
                {
                    viewModel.Value.CurrentPicId = 4;
                }
                else if (viewModel.Value.CurrentPicId == 4)
                {
                    viewModel.Value.CurrentPicId = 1;
                }

                viewModel.Value.ShowViolationPicCommand.Execute(null);
            }
            catch (SystemException)
            {
                this.myImage.Source = new Bitmap(WafTraffic.Presentation.Properties.Resources.expired_picture).GetImageSourceByBitmap();
            }
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (viewModel.Value.CurrentPicId == 1)
                {
                    viewModel.Value.CurrentPicId = 4;
                }
                else if (viewModel.Value.CurrentPicId == 2)
                {
                    viewModel.Value.CurrentPicId = 1;
                }
                else if (viewModel.Value.CurrentPicId == 3)
                {
                    viewModel.Value.CurrentPicId = 2;
                }
                else if (viewModel.Value.CurrentPicId == 4)
                {
                    viewModel.Value.CurrentPicId = 3;
                }

                viewModel.Value.ShowViolationPicCommand.Execute(null);
            }
            catch (SystemException)
            {
                this.myImage.Source = new Bitmap(WafTraffic.Presentation.Properties.Resources.expired_picture).GetImageSourceByBitmap();
            }
        }

        public void OpenChildWindow(int picId, bool isNextBtnNeeded = true)  
        {
            try
            {
                if (!isNextBtnNeeded)
                {
                    this.btnNext.Visibility = Visibility.Collapsed;
                    this.btnLast.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.btnNext.Visibility = Visibility.Visible;
                    this.btnLast.Visibility = Visibility.Visible;
                }

                this.myImage.Source = viewModel.Value.SourceImage;


                if (this.myImage.Source == null)
                {
                    this.myImage.Source = new Bitmap(WafTraffic.Presentation.Properties.Resources.expired_picture).GetImageSourceByBitmap();
                }
            }
            catch (SystemException)
            {
                this.myImage.Source = new Bitmap(WafTraffic.Presentation.Properties.Resources.expired_picture).GetImageSourceByBitmap();
            }

            viewModel.Value.CurrentPicId = picId;

            this.myImage.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1, 0, 0));

            this._childWindow.Height = previousHeight;
            this._childWindow.Width = previousWidth;
            this._childWindow.Top = SystemParameters.WorkArea.Height - this._childWindow.Height - 25; // 25 is the vlaue of WindowContainer's margin.
            this._childWindow.Left = 0;
            this._childWindow.Show();
        }

        public void CloseChildWindow()
        {
            if (this._childWindow.WindowState == Xceed.Wpf.Toolkit.WindowState.Open)
            {
                this._childWindow.Close();
            }
        }

    }

}