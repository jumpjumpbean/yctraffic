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
    public class SgkReleaseCarUpdateViewModel : ViewModel<ISgkReleaseCarUpdateView>
    {
        #region Data

        private SgkReleaseCar releaseCarEntity;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;
        private ICommand printCommand;
        private ICommand chargeSignCommand; 

        private Visibility canSaveVisibal;
        private Visibility canPrintVisibal;
        private Visibility canChargeSignVisible;
        private Visibility canChargeInfoVisible;

        private BitmapImage chargeSignImg;
        private BitmapImage sgkChargeSignImg;
        private BitmapImage rescueChargeSignImg;
        private BitmapImage fDDSignImg;

        private bool canDepartEnable;
        private bool isTitleReadOnly;
        private bool isApprovalInfoReadOnly;
        private bool isReturnBackReadOnly;

        private List<BaseOrganizeEntity> departmentList;
        private List<BaseUserEntity> baseUerList;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public SgkReleaseCarUpdateViewModel(ISgkReleaseCarUpdateView view)
            : base(view)
        {
            releaseCarEntity = new SgkReleaseCar();


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

        private int whosSign;
        public int WhosSign
        {
            get { return whosSign; }
            set
            {
                if (whosSign != value)
                {
                    whosSign = value;
                    RaisePropertyChanged("WhosSign");
                }
            }
        }

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

        public SgkReleaseCar ReleaseCarEntity
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

        //public BitmapImage ChargeSignImg
        //{
        //    get { return chargeSignImg; }
        //    set
        //    {
        //        if (chargeSignImg != value)
        //        {
        //            chargeSignImg = value;
        //            //防止多线程时访问异常
        //            if (chargeSignImg != null)
        //            {
        //                chargeSignImg.Freeze();
        //            }
        //            RaisePropertyChanged("ChargeSignImg");
        //        }
        //    }
        //}

        public BitmapImage SgkChargeSignImg
        {
            get { return sgkChargeSignImg; }
            set
            {
                if (sgkChargeSignImg != value)
                {
                    sgkChargeSignImg = value;
                    //防止多线程时访问异常
                    if (sgkChargeSignImg != null)
                    {
                        sgkChargeSignImg.Freeze();
                    }
                    RaisePropertyChanged("SgkChargeSignImg");
                }
            }
        }

        public BitmapImage RescueChargeSignImg
        {
            get { return rescueChargeSignImg; }
            set
            {
                if (rescueChargeSignImg != value)
                {
                    rescueChargeSignImg = value;
                    //防止多线程时访问异常
                    if (rescueChargeSignImg != null)
                    {
                        rescueChargeSignImg.Freeze();
                    }
                    RaisePropertyChanged("RescueChargeSignImg");
                }
            }
        }

        public BitmapImage FDDSignImg
        {
            get { return fDDSignImg; }
            set
            {
                if (fDDSignImg != value)
                {
                    fDDSignImg = value;
                    //防止多线程时访问异常
                    if (fDDSignImg != null)
                    {
                        fDDSignImg.Freeze();
                    }
                    RaisePropertyChanged("FDDSignImg");
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

        private Visibility canChargeSign1Visible;
        public Visibility CanChargeSign1Visible
        {
            get { return canChargeSign1Visible; }
            set
            {
                if (canChargeSign1Visible != value)
                {
                    canChargeSign1Visible = value;
                    RaisePropertyChanged("CanChargeSign1Visible");
                }
            }
        }

        private Visibility canChargeSign2Visible;
        public Visibility CanChargeSign2Visible
        {
            get { return canChargeSign2Visible; }
            set
            {
                if (canChargeSign2Visible != value)
                {
                    canChargeSign2Visible = value;
                    RaisePropertyChanged("CanChargeSign2Visible");
                }
            }
        }

        private Visibility canChargeSign3Visible;
        public Visibility CanChargeSign3Visible
        {
            get { return canChargeSign3Visible; }
            set
            {
                if (canChargeSign3Visible != value)
                {
                    canChargeSign3Visible = value;
                    RaisePropertyChanged("CanChargeSign3Visible");
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
    