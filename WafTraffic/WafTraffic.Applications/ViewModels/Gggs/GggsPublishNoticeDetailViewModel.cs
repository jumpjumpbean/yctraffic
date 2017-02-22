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
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class GggsPublishNoticeDetailViewModel : ViewModel<IGggsPublishNoticeDetailView>
    {
        private List<BaseOrganizeEntity> departmentList;
        private GggsPublishNotice gggsPublishNotice;

        private ICommand saveCommand;
        private ICommand retreatCommand;
        private ICommand downloadCommand;

        private string operation;
        private string fileLocalPath;

        private Visibility canSave;
        private Visibility canCreatorVisibal;
        private Visibility canAuditorVisibal;
        private Visibility canUploadVisibal;
        private Visibility canDownloadVisibal;
        private Visibility canAboveSaveVisibal;
        private Visibility canBelowSaveVisibal;

        private bool canTitleEdit;
        private bool canCategoryEdit;
        private bool canAuthorEdit;
        private bool canRemarkEdit;
        private bool canStatusEdit;
        private bool canContentEdit;

        [ImportingConstructor]
        public GggsPublishNoticeDetailViewModel(IGggsPublishNoticeDetailView view)
            : base(view)
        {
            gggsPublishNotice = new GggsPublishNotice();

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

        public String Operation
        {
            get { return operation; }
            set
            {
                if (operation != value)
                {
                    operation = value;

                    RaisePropertyChanged("Operation");
                }
            }
        }

        public GggsPublishNotice GggsPublishNotice
        {
            get { return gggsPublishNotice; }
             set
             {
                 if (gggsPublishNotice != value)
                 {
                     gggsPublishNotice = value;
                     RaisePropertyChanged("GggsPublishNotice");
                 }
             }
        }

        public string FileLocalPath
        {
            get { return fileLocalPath; }
            set
            {
                if (fileLocalPath != value)
                {
                    fileLocalPath = value;
                    RaisePropertyChanged("FileLocalPath");
                }
            }
        }

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

         public Visibility CanSave
         {
             get { return canSave; }
             set
             {
                 if (canSave != value)
                 {
                     canSave = value;
                     RaisePropertyChanged("CanSave");
                 }
             }
         }

         public Visibility CanCreatorVisibal
         {
             get { return canCreatorVisibal; }
             set
             {
                 if (canCreatorVisibal != value)
                 {
                     canCreatorVisibal = value;
                     RaisePropertyChanged("CanCreatorVisibal");
                 }
             }
         }

        public Visibility CanAuditorVisibal
         {
             get { return canAuditorVisibal; }
             set
             {
                 if (canAuditorVisibal != value)
                 {
                     canAuditorVisibal = value;
                     RaisePropertyChanged("CanAuditorVisibal");
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

        public Visibility CanAboveSaveVisibal
         {
             get { return canAboveSaveVisibal; }
             set
             {
                 if (canAboveSaveVisibal != value)
                 {
                     canAboveSaveVisibal = value;
                     RaisePropertyChanged("CanAboveSaveVisibal");
                 }
             }
         }

        public Visibility CanBelowSaveVisibal
        {
            get { return canBelowSaveVisibal; }
            set
            {
                if (canBelowSaveVisibal != value)
                {
                    canBelowSaveVisibal = value;
                    RaisePropertyChanged("CanBelowSaveVisibal");
                }
            }
        }

        public bool CanContentEdit
        {
            get { return canContentEdit; }
            set
            {
                if (canContentEdit != value)
                {
                    canContentEdit = value;
                    RaisePropertyChanged("CanContentEdit");
                }
            }
        }

        public bool CanCategoryEdit
        {
            get { return canCategoryEdit; }
            set
            {
                if (canCategoryEdit != value)
                {
                    canCategoryEdit = value;
                    RaisePropertyChanged("CanCategoryEdit");
                }
            }
        }

         public bool CanTitleEdit
         {
             get { return canTitleEdit; }
             set
             {
                 if (canTitleEdit != value)
                 {
                     canTitleEdit = value;
                     RaisePropertyChanged("CanTitleEdit");
                 }
             }
         }

         public bool CanAuthorEdit
         {
             get { return canAuthorEdit; }
             set
             {
                 if (canAuthorEdit != value)
                 {
                     canAuthorEdit = value;
                     RaisePropertyChanged("CanAuthorEdit");
                 }
             }
         }

         public bool CanRemarkEdit
         {
             get { return canRemarkEdit; }
             set
             {
                 if (canRemarkEdit != value)
                 {
                     canRemarkEdit = value;
                     RaisePropertyChanged("CanRemarkEdit");
                 }
             }
         }

         public bool CanStatusEdit
         {
             get { return canStatusEdit; }
             set
             {
                 if (canStatusEdit != value)
                 {
                     canStatusEdit = value;
                     RaisePropertyChanged("CanStatusEdit");
                 }
             }
         }


         #region Members

         public void Show_LoadingMask(LoadingType type)
         {
             ViewCore.Show_Loading(type);
         }

         public void Shutdown_LoadingMask(LoadingType type)
         {
             ViewCore.Shutdown_Loading(type);
         }

         #endregion
    }
}