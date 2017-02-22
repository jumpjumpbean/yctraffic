using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Windows.Input;
using WafTraffic.Applications.Properties;
using WafTraffic.Applications.Views;
using WafTraffic.Applications.Controllers;
using System;
using WafTraffic.Applications.Services;
using System.Windows;
using System.Windows.Media;
using WafTraffic.Domain;
using System.Windows.Media.Imaging;


namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ShellViewModel : ViewModel<IShellView>
    {
        private readonly IMessageService messageService;
        private readonly IShellService shellService;
        private readonly DelegateCommand aboutCommand;
        private ICommand saveCommand;
        private ICommand exitCommand;
        private ICommand showAlarmCommand;
        private ICommand showViolationPicCommand;
        private bool isValid = true;
        private string databasePath = WafTraffic.Applications.Properties.Resources.NotAvailable;
        private object contentView;
        private object alarmWindowView;

        private ZhzxPicture zhzxPictureEntity;
        private MayorHotlineTaskTable hotLinePitcure;
        private BitmapImage mSourceImage;
        private int currentPicId;

        [ImportingConstructor]
        public ShellViewModel(IShellView view, IMessageService messageService, IPresentationService presentationService,
            IShellService shellService)
            : base(view)
        {
            this.messageService = messageService;
            this.shellService = shellService;
            this.aboutCommand = new DelegateCommand(ShowAboutMessage);
            view.Closing += ViewClosing;
            view.Closed += ViewClosed;

            // Restore the window size when the values are valid.
            if (Settings.Default.Left >= 0 && Settings.Default.Top >= 0 && Settings.Default.Width > 0 && Settings.Default.Height > 0
                && Settings.Default.Left + Settings.Default.Width <= presentationService.VirtualScreenWidth
                && Settings.Default.Top + Settings.Default.Height <= presentationService.VirtualScreenHeight)
            {
                ViewCore.Left = Settings.Default.Left;
                ViewCore.Top = Settings.Default.Top;
                ViewCore.Height = Settings.Default.Height;
                ViewCore.Width = Settings.Default.Width;
            }
            ViewCore.IsMaximized = Settings.Default.IsMaximized;
        }

        public object ContentView
        {
            get { return contentView; }
            set
            {
                if (contentView != value)
                {
                    contentView = value;
                    RaisePropertyChanged("ContentView");
                }
            }
        }

        public string Title { get { return ApplicationInfo.ProductName; } }

        private string softwareVersion;
        public string SoftwareVersion 
        {
            get { return softwareVersion; }
            set
            {
                if (softwareVersion != value)
                {
                    softwareVersion = value;
                    RaisePropertyChanged("SoftwareVersion");
                }
            }
        }

        public IShellService ShellService { get { return shellService; } }

        public ICommand AboutCommand { get { return aboutCommand; } }

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                if (saveCommand != value)
                {
                    saveCommand = value;
                    RaisePropertyChanged("SaveCommand");
                }
            }
        }

        public ICommand ExitCommand
        {
            get { return exitCommand; }
            set
            {
                if (exitCommand != value)
                {
                    exitCommand = value;
                    RaisePropertyChanged("ExitCommand");
                }
            }
        }

        public ICommand ShowAlarmCommand
        {
            get { return showAlarmCommand; }
            set
            {
                if (showAlarmCommand != value)
                {
                    showAlarmCommand = value;
                    RaisePropertyChanged("ShowAlarmCommand");
                }
            }
        }

        public ICommand ShowViolationPicCommand
        {
            get { return showViolationPicCommand; }
            set
            {
                if (showViolationPicCommand != value)
                {
                    showViolationPicCommand = value;
                    RaisePropertyChanged("ShowViolationPicCommand");
                }
            }
        }

        public object AlarmWindowView
        {
            get { return alarmWindowView; }
            set
            {
                if (alarmWindowView != value)
                {
                    alarmWindowView = value;
                    RaisePropertyChanged("AlarmWindowView");
                }
            }
        }

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                if (isValid != value)
                {
                    isValid = value;
                    RaisePropertyChanged("IsValid");
                }
            }
        }

        public string DatabasePath
        {
            get { return databasePath; }
            set
            {
                if (databasePath != value)
                {
                    databasePath = value;
                    RaisePropertyChanged("DatabasePath");
                }
            }
        }


        public event CancelEventHandler Closing;


        public void Show()
        {
            ViewCore.Show();
        }

        public void Close()
        {
            ViewCore.Close();
        }

        protected virtual void OnClosing(CancelEventArgs e)
        {
            if (Closing != null) { Closing(this, e); }
        }

        private void ViewClosing(object sender, CancelEventArgs e)
        {
            OnClosing(e);
        }

        private void ViewClosed(object sender, EventArgs e)
        {
            Settings.Default.Left = ViewCore.Left;
            Settings.Default.Top = ViewCore.Top;
            Settings.Default.Height = ViewCore.Height;
            Settings.Default.Width = ViewCore.Width;
            Settings.Default.IsMaximized = ViewCore.IsMaximized;
        }

        private void ShowAboutMessage()
        {
            messageService.ShowMessage(View, string.Format(CultureInfo.CurrentCulture, WafTraffic.Applications.Properties.Resources.AboutText,
                ApplicationInfo.ProductName, ApplicationInfo.Version));
        }

        public void LetAlarmButtonSingDance()
        {
            ViewCore.BtnAlarm_Sing();
            ViewCore.BtnAlarm_Dance();
        }

        public void ShowMyImage(int picId, bool isNextBtnNeeded = true)
        {
            ViewCore.OpenChildWindow(picId, isNextBtnNeeded);
        }

        public void CloseMyImage()
        {
            ViewCore.CloseChildWindow();
        }

        private Visibility canAlarmShow;
        public Visibility CanAlarmShow
        {
            get
            {
                return canAlarmShow;
            }
            set
            {
                if (canAlarmShow != value)
                {
                    canAlarmShow = value;
                    RaisePropertyChanged("CanAlarmShow");
                }
            }
        }

        private Visibility canAlarmNotifyShow;
        public Visibility CanAlarmNotifyShow
        {
            get
            {
                return canAlarmNotifyShow;
            }
            set
            {
                if (canAlarmNotifyShow != value)
                {
                    canAlarmNotifyShow = value;
                    RaisePropertyChanged("CanAlarmNotifyShow");
                }
            }
        }

        private Visibility canbtnLastOrNextShow;
        public Visibility CanbtnLastOrNextShow
        {
            get
            {
                return canbtnLastOrNextShow;
            }
            set
            {
                if (canbtnLastOrNextShow != value)
                {
                    canbtnLastOrNextShow = value;
                    RaisePropertyChanged("CanbtnLastOrNextShow");
                }
            }
        }

        private Brush backgroundColor = new SolidColorBrush(Colors.Transparent);
        public Brush BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                if (backgroundColor != value)
                {
                    backgroundColor = value;
                    RaisePropertyChanged("BackgroundColor");
                }
            }
        }

        public ZhzxPicture ZhzxPictureEntity
        {
            get { return zhzxPictureEntity; }
            set
            {
                if (zhzxPictureEntity != value)
                {
                    zhzxPictureEntity = value;
                    RaisePropertyChanged("ZhzxPictureEntity");
                }
            }
        }

        public BitmapImage SourceImage
        {
            get { return mSourceImage; }
            set
            {
                if (mSourceImage != value)
                {
                    mSourceImage = value;
                    //防止多线程时访问异常
                    if (mSourceImage != null)
                    {
                        mSourceImage.Freeze();
                    }
                    RaisePropertyChanged("SourceImage");
                }
            }
        }

        public int CurrentPicId
        {
            get { return currentPicId; }
            set
            {
                if (currentPicId != value)
                {
                    currentPicId = value;
                    RaisePropertyChanged("CurrentPicId");
                }
            }
        }

        public MayorHotlineTaskTable HotLinePitcure
        {
            get { return hotLinePitcure; }
            set
            {
                if (hotLinePitcure != value)
                {
                    hotLinePitcure = value;
                    RaisePropertyChanged("HotLinePitcure");
                }
            }
        }
    }
}