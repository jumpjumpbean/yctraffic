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
using System.Windows.Media.Imaging;
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class PersonArchiveViewModel : ViewModel<IPersonArchiveView>
    {
        private PersonArchiveTable mPersonArchive;
        private ICommand mSaveCommand;
        private ICommand mRetreatCommand;
        private ICommand mShowPhotoCommand;
        private List<BaseOrganizeEntity> mDepartmentList;
        private string mOperation;

        [ImportingConstructor]
        public PersonArchiveViewModel(IPersonArchiveView view)
            : base(view)
        {
            mPersonArchive = new PersonArchiveTable();

            mDepartmentList = new List<BaseOrganizeEntity>();

            BaseOrganizeManager origanizeService = new BaseOrganizeManager();
            DataTable departmentDT = origanizeService.GetDepartmentDT(""); //¸ù½Úµã parnetid
            BaseOrganizeEntity entity;
            foreach (DataRow dr in departmentDT.Rows)
            {
                entity = new BaseOrganizeEntity(dr);
                mDepartmentList.Add(entity);
            }
        }

        public ICommand ShowPhotoCommand
        {
            get { return mShowPhotoCommand; }
            set
            {
                if (mShowPhotoCommand != value)
                {
                    mShowPhotoCommand = value;
                    RaisePropertyChanged("ShowPhotoCommand");
                }
            }
        }

        public String Operation
        {
            get { return mOperation; }
            set
            {
                if (mOperation != value)
                {
                    mOperation = value;
                    RaisePropertyChanged("Operation");
                }
            }
        }

        public PersonArchiveTable PersonArchive
         {
             get { return mPersonArchive; }
             set
             {
                 if (mPersonArchive != value)
                 {
                     mPersonArchive = value;
                     RaisePropertyChanged("PersonArchive");
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

        public List<BaseOrganizeEntity> DepartmentList
         {
             get { return mDepartmentList; }
             set
             {
                 if (mDepartmentList != value)
                 {
                     mDepartmentList = value;
                     RaisePropertyChanged("DepartmentList");
                 }
             }
         }

        public ICommand RetreatCommand
         {
             get { return mRetreatCommand; }
             set
             {
                 if (mRetreatCommand != value)
                 {
                     mRetreatCommand = value;
                     RaisePropertyChanged("RetreatCommand");
                 }
             }
         }

        #region Members

        //public void Show_LoadingMask(LoadingType type)
        //{
        //    ViewCore.Show_Loading(type);
        //}

        //public void Shutdown_LoadingMask(LoadingType type)
        //{
        //    ViewCore.Shutdown_Loading(type);
        //}

        #endregion
    }
}

    