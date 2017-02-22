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
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.ViewModels
{
     [Export]
    public class HotLineViewModel : ViewModel<IHotLineView>
    {
        private MayorHotlineTaskTable mHotLineTask;
        private List<BaseOrganizeEntity> mDepartmentList;
        private ICommand mSaveCommand;
        private ICommand mRetreatCommand;
        //private ICommand mShowPictrueCmd20;
        private Visibility mCanSave = Visibility.Visible;
        private Visibility mCanSendDDZ = Visibility.Visible; 
        private Visibility mCanSendVice = Visibility.Visible;
        private string mContentLocalPath;

        [ImportingConstructor]
        public HotLineViewModel(IHotLineView view)
            : base(view)
        {
            mHotLineTask = new MayorHotlineTaskTable();
            mDepartmentList = new List<BaseOrganizeEntity>();

             BaseOrganizeManager origanizeService = new BaseOrganizeManager();
             DataTable departmentDT = origanizeService.GetDepartmentDT(""); //根节点 parnetid
             BaseOrganizeEntity entity;
             foreach (DataRow dr in departmentDT.Rows)
             {
                 entity = new BaseOrganizeEntity(dr);
                 mDepartmentList.Add( entity );
             }
        }

        public string ContentLocalPath
        {
            get { return mContentLocalPath; }
            set
            {
                if (mContentLocalPath != value)
                {
                    mContentLocalPath = value;
                    RaisePropertyChanged("ContentLocalPath");
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

        /*
        public ICommand ShowPictrueCmd20
        {
            get { return mShowPictrueCmd20; }
            set
            {
                if (mShowPictrueCmd20 != value)
                {
                    mShowPictrueCmd20 = value;
                    RaisePropertyChanged("ShowPictrueCmd20");
                }
            }
        }
        */

        public MayorHotlineTaskTable HotLineTask
        {
            get { return mHotLineTask; }
            set
            {
                if (mHotLineTask != value)
                {
                    mHotLineTask = value;
                    RaisePropertyChanged("HotLineTask");
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

        public Visibility CanSave
        {
            get { return mCanSave; }
            set
            {
                if (mCanSave != value)
                {
                    mCanSave = value;
                    RaisePropertyChanged("CanSave");
                }
            }
        }

        public Visibility CanSendDDZ
        {
            get { return mCanSendDDZ; }
            set
            {
                if (mCanSendDDZ != value)
                {
                    mCanSendDDZ = value;
                    RaisePropertyChanged("CanSendDDZ");
                }
            }
        }

        public Visibility CanSendVice
        {
            get { return mCanSendVice; }
            set
            {
                if (mCanSendVice != value)
                {
                    mCanSendVice = value;
                    RaisePropertyChanged("CanSendVice");
                }
            }
        }

        #region Members

        public void ShowLoadingMask(LoadingType type)
        {
            ViewCore.ShowLoading(type);
        }

        public void ShutdownLoadingMask(LoadingType type)
        {
            ViewCore.ShutdownLoading(type);
        }

        #endregion

    }
}
