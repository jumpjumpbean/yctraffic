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
    public class ZgxcPersonnelChangeUpdateViewModel : ViewModel<IZgxcPersonnelChangeUpdateView>
    {
        #region Data

        private ZgxcPersonnelChange personnelChangeEntity;
        private List<BaseOrganizeEntity> departmentList;
        private ICommand mSaveCommand;
        private ICommand mCancelCommand;

        private Visibility canSaveVisibal;

        private bool isNameReadOnly;
        private bool isCommentsReadOnly;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public ZgxcPersonnelChangeUpdateViewModel(IZgxcPersonnelChangeUpdateView view)
            : base(view)
        {
            personnelChangeEntity = new ZgxcPersonnelChange();

            departmentList = new List<BaseOrganizeEntity>();

            BaseOrganizeManager origanizeService = new BaseOrganizeManager();
            DataTable departmentDT = origanizeService.GetDepartmentDT(""); //¸ù½Úµã parnetid
            BaseOrganizeEntity entity;
            foreach (DataRow dr in departmentDT.Rows)
            {
                entity = new BaseOrganizeEntity(dr);
                departmentList.Add(entity);
            }
        }

        #endregion

        #region Properties

        public ZgxcPersonnelChange PersonnelChangeEntity
        {
            get { return personnelChangeEntity; }
            set
            {
                if (personnelChangeEntity != value)
                {
                    personnelChangeEntity = value;
                    RaisePropertyChanged("PersonnelChangeEntity");
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

        private Visibility canSignVisibal;
        public Visibility CanSignVisibal
        {
            get { return canSignVisibal; }
            set
            {
                if (canSignVisibal != value)
                {
                    canSignVisibal = value;
                    RaisePropertyChanged("CanSignVisibal");
                }
            }
        }

        private Visibility canArchiveVisibal;
        public Visibility CanArchiveVisibal
        {
            get { return canArchiveVisibal; }
            set
            {
                if (canArchiveVisibal != value)
                {
                    canArchiveVisibal = value;
                    RaisePropertyChanged("CanArchiveVisibal");
                }
            }
        }


        public bool IsNameReadOnly
        {
            get { return isNameReadOnly; }
            set
            {
                if (isNameReadOnly != value)
                {
                    isNameReadOnly = value;
                    RaisePropertyChanged("IsNameReadOnly");
                }
            }
        }

        public bool IsCommentsReadOnly
        {
            get { return isCommentsReadOnly; }
            set
            {
                if (isCommentsReadOnly != value)
                {
                    isCommentsReadOnly = value;
                    RaisePropertyChanged("IsCommentsReadOnly");
                }
            }
        }

        private bool canPersonStatusEdit;
        public bool CanPersonStatusEdit
        {
            get { return canPersonStatusEdit; }
            set
            {
                if (canPersonStatusEdit != value)
                {
                    canPersonStatusEdit = value;
                    RaisePropertyChanged("CanPersonStatusEdit");
                }
            }
        }

        #endregion


    }
}
    