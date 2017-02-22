using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using System.Windows;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;
using DotNet.Business;
using System.Data;
using System.Windows.Media.Imaging;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class FzkReleaseCarUpdateViewModel : ViewModel<IFzkReleaseCarUpdateView>
    {
        #region Data

        private FzkReleaseCar releaseCarEntity;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;
        private ICommand printCommand;

        private BitmapImage chargeSignImg;

        private Visibility canSaveVisibal;
        private Visibility canPrintVisibal;
        private Visibility canChargeSignVisible;
        private Visibility canChargeInfoVisible;

        private bool canDepartEnable;
        private bool isTitleReadOnly;
        private bool isApprovalInfoReadOnly;
        private bool isReturnBackReadOnly;

        private List<BaseOrganizeEntity> departmentList;
        private List<BaseUserEntity> baseUerList;

        private string mUploadFullPath;

        private Visibility canUploadVisibal;
        private Visibility canDownloadVisibal;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public FzkReleaseCarUpdateViewModel(IFzkReleaseCarUpdateView view)
            : base(view)
        {
            releaseCarEntity = new FzkReleaseCar();


            //departmentList = new List<BaseOrganizeEntity>();

            //BaseOrganizeManager origanizeService = new BaseOrganizeManager();
            //DataTable departmentDT = origanizeService.GetDepartmentDT(""); //根节点 parnetid
            //BaseOrganizeEntity entity;
            //foreach (DataRow dr in departmentDT.Rows)
            //{
            //    entity = new BaseOrganizeEntity(dr);
            //    departmentList.Add(entity);
            //}


            //BaseUserManager baseUserManager = new BaseUserManager();
            //DataTable baseUserDT = baseUserManager.GetDTByDepartment("100000028");   // 获取大队领导的所有用户

            //baseUerList = new List<BaseUserEntity>();
            //foreach (DataRow dr in baseUserDT.Rows)
            //{
            //    baseUerList.Add(new BaseUserEntity(dr));
            //}
        }

        #endregion

        #region Properties

        public List<BaseOrganizeEntity> DepartmentList
        {
            get { return departmentList; }
            set
            {
                if (departmentList != value)
                {
                    departmentList = value;
                    RaisePropertyChanged("DepartmentList");
                }
            }
        }

        public List<BaseUserEntity> BaseUerList
        {
            get { return baseUerList; }
            set
            {
                if (baseUerList != value)
                {
                    baseUerList = value;
                    RaisePropertyChanged("BaseUerList");
                }
            }
        }

        public FzkReleaseCar ReleaseCarEntity
        {
            get { return releaseCarEntity; }
            set
            {
                if (releaseCarEntity != value)
                {
                    releaseCarEntity = value;
                    RaisePropertyChanged("ReleaseCarEntity");
                }
            }
        }

        public BitmapImage ChargeSignImg
        {
            get { return chargeSignImg; }
            set
            {
                if (chargeSignImg != value)
                {
                    chargeSignImg = value;
                    //防止多线程时访问异常
                    if (chargeSignImg != null)
                    {
                        chargeSignImg.Freeze();
                    }
                    RaisePropertyChanged("ChargeSignImg");
                }
            }
        }

        public ICommand SaveCommand
        {
            get { return mSaveCommand; }
            set
            {
                if (mSaveCommand != value)
                {
                    mSaveCommand = value;
                    RaisePropertyChanged("SaveCommand");
                }
            }
        }

        public ICommand CancelCommand
        {
            get { return mCancelCommand; }
            set
            {
                if (mCancelCommand != value)
                {
                    mCancelCommand = value;
                    RaisePropertyChanged("CancelCommand");
                }
            }
        }

        public ICommand PrintCommand
        {
            get { return printCommand; }
            set
            {
                if (printCommand != value)
                {
                    printCommand = value;
                    RaisePropertyChanged("PrintCommand");
                }
            }
        }

        private ICommand mShowSignImgCommand;
        public ICommand ShowSignImgCommand
        {
            get { return mShowSignImgCommand; }
            set
            {
                if (mShowSignImgCommand != value)
                {
                    mShowSignImgCommand = value;
                    RaisePropertyChanged("ShowSignImgCommand");
                }
            }
        }

        private ICommand chargeSignCommand;
        public ICommand ChargeSignCommand
        {
            get { return chargeSignCommand; }
            set
            {
                if (chargeSignCommand != value)
                {
                    chargeSignCommand = value;
                    RaisePropertyChanged("ChargeSignCommand");
                }
            }
        }

        private ICommand downloadCommand;
        public ICommand DownloadCommand
        {
            get { return downloadCommand; }
            set
            {
                if (downloadCommand != value)
                {
                    downloadCommand = value;
                    RaisePropertyChanged("DownloadCommand");
                }
            }
        }

        private ICommand displayCommand;
        public ICommand DisplayCommand
        {
            get { return displayCommand; }
            set
            {
                if (displayCommand != value)
                {
                    displayCommand = value;
                    RaisePropertyChanged("DisplayCommand");
                }
            }
        }

        public Visibility CanSaveVisibal
        {
            get { return canSaveVisibal; }
            set
            {
                if (canSaveVisibal != value)
                {
                    canSaveVisibal = value;
                    RaisePropertyChanged("CanSaveVisibal");
                }
            }
        }

        public Visibility CanPrintVisibal
        {
            get { return canPrintVisibal; }
            set
            {
                if (canPrintVisibal != value)
                {
                    canPrintVisibal = value;
                    RaisePropertyChanged("CanPrintVisibal");
                }
            }
        }

        public bool IsTitleReadOnly
        {
            get { return isTitleReadOnly; }
            set
            {
                if (isTitleReadOnly != value)
                {
                    isTitleReadOnly = value;
                    RaisePropertyChanged("IsTitleReadOnly");
                }
            }
        }

        public bool IsApprovalInfoReadOnly
        {
            get { return isApprovalInfoReadOnly; }
            set
            {
                if (isApprovalInfoReadOnly != value)
                {
                    isApprovalInfoReadOnly = value;
                    RaisePropertyChanged("IsApprovalInfoReadOnly");
                }
            }
        }

        public bool IsReturnBackReadOnly
        {
            get { return isReturnBackReadOnly; }
            set
            {
                if (isReturnBackReadOnly != value)
                {
                    isReturnBackReadOnly = value;
                    RaisePropertyChanged("IsReturnBackReadOnly");
                }
            }
        }

        public bool CanDepartEnable
        {
            get { return canDepartEnable; }
            set
            {
                if (canDepartEnable != value)
                {
                    canDepartEnable = value;
                    RaisePropertyChanged("CanDepartEnable");
                }
            }
        }

        public Visibility CanChargeSignVisible
        {
            get { return canChargeSignVisible; }
            set
            {
                if (canChargeSignVisible != value)
                {
                    canChargeSignVisible = value;
                    RaisePropertyChanged("CanChargeSignVisible");
                }
            }
        }

        public Visibility CanChargeInfoVisible
        {
            get { return canChargeInfoVisible; }
            set
            {
                if (canChargeInfoVisible != value)
                {
                    canChargeInfoVisible = value;
                    RaisePropertyChanged("CanChargeInfoVisible");
                }
            }
        }

        public string UploadFullPath
        {
            get { return mUploadFullPath; }
            set
            {
                if (mUploadFullPath != value)
                {
                    mUploadFullPath = value;
                    RaisePropertyChanged("UploadFullPath");
                }
            }
        }

        public Visibility CanUploadVisibal
        {
            get { return canUploadVisibal; }
            set
            {
                if (canUploadVisibal != value)
                {
                    canUploadVisibal = value;
                    RaisePropertyChanged("CanUploadVisibal");
                }
            }
        }

        public Visibility CanDownloadVisibal
        {
            get { return canDownloadVisibal; }
            set
            {
                if (canDownloadVisibal != value)
                {
                    canDownloadVisibal = value;
                    RaisePropertyChanged("CanDownloadVisibal");
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

        #endregion


    }
}
    