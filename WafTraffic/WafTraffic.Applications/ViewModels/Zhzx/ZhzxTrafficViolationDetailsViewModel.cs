using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;
using DotNet.Business;
using System.Data;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ZhzxTrafficViolationDetailsViewModel : ViewModel<IZhzxTrafficViolationDetailsView>
    {
        private ZhzxTrafficViolation zhzxTrafficViolationEntity;

        private ICommand mShowThumbCommand;
        private ICommand openImageCommand1;
        private ICommand openImageCommand2;
        private ICommand openImageCommand3;
        private ICommand openImageCommand4;
        private ICommand retreatCommand;
        private ICommand submitCommand;
        private ICommand rejectCommand;
        private ICommand addFakePlateCommand;

        private BitmapImage composedThumbnailImg;
        private BitmapImage thumbnail1Img;
        private BitmapImage thumbnail2Img;
        private BitmapImage thumbnail3Img;

        private Visibility canRetreatShow;
        private Visibility canSubmitShow;
        private Visibility canRejectShow;
        private Visibility canAddFakePlatShow;

        private bool detailsReadOnly;
        private bool isComboBoxEnabled;

        private ZhzxThumbnail zhzxThumbnailEntity;
        private ZhzxPicture zhzxPictureEntity;

        private List<ViolationTypeCode> violationTypeCodeList;
        private string selectviolationType;

        private bool reloadPicture = false;

        [ImportingConstructor]
        public ZhzxTrafficViolationDetailsViewModel(IZhzxTrafficViolationDetailsView view, IEntityService entityservice)
            : base(view)
        {
            violationTypeCodeList = new List<ViolationTypeCode>();
            ViolationTypeCode temp = new ViolationTypeCode("不按车道行驶", "12080");
            violationTypeCodeList.Add(temp);

            temp = new ViolationTypeCode("压线", "13450");
            violationTypeCodeList.Add(temp);

            temp = new ViolationTypeCode("压黄线", "13440");
            violationTypeCodeList.Add(temp);

            temp = new ViolationTypeCode("闯红灯", "16250");
            violationTypeCodeList.Add(temp);

            temp = new ViolationTypeCode("逆行", "13010");
            violationTypeCodeList.Add(temp);

            temp = new ViolationTypeCode("违章变道", "12080");
            violationTypeCodeList.Add(temp);

            temp = new ViolationTypeCode("不系安全带", "60110");
            violationTypeCodeList.Add(temp);
        }

        public string SelectviolationType
        {
            get { return selectviolationType; }
            set
            {
                if (selectviolationType != value)
                {
                    selectviolationType = value;
                    RaisePropertyChanged("SelectviolationType");
                }
            }
        }

        public List<ViolationTypeCode> ViolationTypeCodeList
        {
            get { return violationTypeCodeList; }
            set
            {
                if (violationTypeCodeList != value)
                {
                    violationTypeCodeList = value;
                    RaisePropertyChanged("ViolationTypeCodeList");
                }
            }
        }

        public ZhzxTrafficViolation ZhzxTrafficViolationEntity
        {
            get { return zhzxTrafficViolationEntity; }
            set
            {
                if (zhzxTrafficViolationEntity != value)
                {
                    zhzxTrafficViolationEntity = value;
                    reloadPicture = true;
                    RaisePropertyChanged("ZhzxTrafficViolationEntity");
                }
            }
        }

        public bool ReloadPicture
        {
            get { return reloadPicture; }
            set
            {
                if (reloadPicture != value)
                {
                    reloadPicture = value;
                    RaisePropertyChanged("ReloadPicture");
                }
            }
        }

        public BitmapImage ComposedThumbnailImg
        {
            get { return composedThumbnailImg; }
            set
            {
                if (composedThumbnailImg != value)
                {
                    composedThumbnailImg = value;
                    //防止多线程时访问异常
                    if (composedThumbnailImg != null)
                    {
                        composedThumbnailImg.Freeze();
                    }
                    RaisePropertyChanged("ComposedThumbnailImg");
                }
            }
        }

        public BitmapImage Thumbnail1Img
        {
            get { return thumbnail1Img; }
            set
            {
                if (thumbnail1Img != value)
                {
                    thumbnail1Img = value;
                    //防止多线程时访问异常
                    if (thumbnail1Img != null)
                    {
                        thumbnail1Img.Freeze();
                    }
                    RaisePropertyChanged("Thumbnail1Img");
                }
            }
        }

        public BitmapImage Thumbnail2Img
        {
            get { return thumbnail2Img; }
            set
            {
                if (thumbnail2Img != value)
                {
                    thumbnail2Img = value;
                    //防止多线程时访问异常
                    if (thumbnail2Img != null)
                    {
                        thumbnail2Img.Freeze();
                    }
                    RaisePropertyChanged("Thumbnail2Img");
                }
            }
        }

        public BitmapImage Thumbnail3Img
        {
            get { return thumbnail3Img; }
            set
            {
                if (thumbnail3Img != value)
                {
                    thumbnail3Img = value;
                    //防止多线程时访问异常
                    if (thumbnail3Img != null)
                    {
                        thumbnail3Img.Freeze();
                    }
                    RaisePropertyChanged("Thumbnail3Img");
                }
            }
        }


        public ICommand ShowThumbCommand
        {
            get { return mShowThumbCommand; }
            set
            {
                if (mShowThumbCommand != value)
                {
                    mShowThumbCommand = value;
                    RaisePropertyChanged("ShowThumbCommand");
                }
            }
        }

        public ICommand OpenImageCommand1
        {
            get { return openImageCommand1; }
            set
            {
                if (openImageCommand1 != value)
                {
                    openImageCommand1 = value;
                    RaisePropertyChanged("OpenImageCommand1");
                }
            }
        }

        public ICommand OpenImageCommand2
        {
            get { return openImageCommand2; }
            set
            {
                if (openImageCommand2 != value)
                {
                    openImageCommand2 = value;
                    RaisePropertyChanged("OpenImageCommand2");
                }
            }
        }

        public ICommand OpenImageCommand3
        {
            get { return openImageCommand3; }
            set
            {
                if (openImageCommand3 != value)
                {
                    openImageCommand3 = value;
                    RaisePropertyChanged("OpenImageCommand3");
                }
            }
        }

        public ICommand OpenImageCommand4
        {
            get { return openImageCommand4; }
            set
            {
                if (openImageCommand4 != value)
                {
                    openImageCommand4 = value;
                    RaisePropertyChanged("OpenImageCommand4");
                }
            }
        }

        public ICommand SubmitCommand
        {
            get { return submitCommand; }
            set
            {
                if (submitCommand != value)
                {
                    submitCommand = value;
                    RaisePropertyChanged("SubmitCommand");
                }
            }
        }

        public ICommand RejectCommand
        {
            get { return rejectCommand; }
            set
            {
                if (rejectCommand != value)
                {
                    rejectCommand = value;
                    RaisePropertyChanged("RejectCommand");
                }
            }
        }

        public ICommand RetreatCommand
        {
            get { return retreatCommand; }
            set
            {
                if (retreatCommand != value)
                {
                    retreatCommand = value;
                    RaisePropertyChanged("RetreatCommand");
                }
            }
        }

        public ICommand AddFakePlateCommand
        {
            get { return addFakePlateCommand; }
            set
            {
                if (addFakePlateCommand != value)
                {
                    addFakePlateCommand = value;
                    RaisePropertyChanged("AddFakePlateCommand");
                }
            }
        }

        public ZhzxThumbnail ZhzxThumbnailEntity
        {
            get { return zhzxThumbnailEntity; }
            set
            {
                if (zhzxThumbnailEntity != value)
                {
                    zhzxThumbnailEntity = value;
                    RaisePropertyChanged("ZhzxThumbnailEntity");
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

        public Visibility CanRetreatShow
        {
            get { return canRetreatShow; }
            set
            {
                if (canRetreatShow != value)
                {
                    canRetreatShow = value;
                    RaisePropertyChanged("CanRetreatShow");
                }
            }
        }

        public Visibility CanSubmitShow
        {
            get { return canSubmitShow; }
            set
            {
                if (canSubmitShow != value)
                {
                    canSubmitShow = value;
                    RaisePropertyChanged("CanSubmitShow");
                }
            }
        }

        public Visibility CanRejectShow
        {
            get { return canRejectShow; }
            set
            {
                if (canRejectShow != value)
                {
                    canRejectShow = value;
                    RaisePropertyChanged("CanRejectShow");
                }
            }
        }

        public Visibility CanAddFakePlatShow
        {
            get { return canAddFakePlatShow; }
            set
            {
                if (canAddFakePlatShow != value)
                {
                    canAddFakePlatShow = value;
                    RaisePropertyChanged("CanAddFakePlatShow");
                }
            }
        }


        public bool DetailsReadOnly
        {
            get { return detailsReadOnly; }
            set
            {
                if (detailsReadOnly != value)
                {
                    detailsReadOnly = value;
                    RaisePropertyChanged("DetailsReadOnly");
                }
            }
        }

        public bool IsComboBoxEnabled
        {
            get { return isComboBoxEnabled; }
            set
            {
                if (isComboBoxEnabled != value)
                {
                    isComboBoxEnabled = value;
                    RaisePropertyChanged("IsComboBoxEnabled");
                }
            }
        }

        public void Show_LoadingMask()
        {
            ViewCore.Show_Loading();
        }

        public void Shutdown_LoadingMask()
        {
            ViewCore.Shutdown_Loading();
        }
    }

    public class ViolationTypeCode
    {
        private string violationType;
        private string violationCode;

        public override string ToString()
        {
            return this.ViolationType;
        }

        public ViolationTypeCode(string violationType, string violationCode)
        {
            this.ViolationType = violationType;
            this.ViolationCode = violationCode;
        }

        public string ViolationType
        {
            get { return violationType; }
            set
            {
                if (violationType != value)
                {
                    violationType = value;
                }
            }
        }

        public string ViolationCode
        {
            get { return violationCode; }
            set
            {
                if (violationCode != value)
                {
                    violationCode = value;
                }
            }
        }
    }


}
    