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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ZgxcAskForLeaveUpdateViewModel : ViewModel<IZgxcAskForLeaveUpdateView>
    {
        #region Data

        private ZgxcAskForLeave askForLeaveEntity;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;

        private Visibility canSaveVisibal;

        private bool canDepartEnable;
        private bool isTitleReadOnly;
        private bool isApprovalInfoReadOnly;
        private bool isReturnBackReadOnly;

        private List<BaseOrganizeEntity> departmentList;
        private List<BaseUserEntity> baseUerList;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public ZgxcAskForLeaveUpdateViewModel(IZgxcAskForLeaveUpdateView view)
            : base(view)
        {
            askForLeaveEntity = new ZgxcAskForLeave();


            departmentList = new List<BaseOrganizeEntity>();

            BaseOrganizeManager origanizeService = new BaseOrganizeManager();
            DataTable departmentDT = origanizeService.GetDepartmentDT(""); //根节点 parnetid
            BaseOrganizeEntity entity;
            foreach (DataRow dr in departmentDT.Rows)
            {
                entity = new BaseOrganizeEntity(dr);
                departmentList.Add(entity);
            }


            BaseUserManager baseUserManager = new BaseUserManager();
            DataTable baseUserDT = baseUserManager.GetDTByDepartment("100000028");   // 获取大队领导的所有用户

            baseUerList = new List<BaseUserEntity>();
            foreach (DataRow dr in baseUserDT.Rows)
            {
                baseUerList.Add(new BaseUserEntity(dr));
            }
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

        public ZgxcAskForLeave AskForLeaveEntity
        {
            get { return askForLeaveEntity; }
            set
            {
                if (askForLeaveEntity != value)
                {
                    askForLeaveEntity = value;
                    RaisePropertyChanged("AskForLeaveEntity");
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

        #endregion


    }
}
    