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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class MaterialDeclareBuildViewModel : ViewModel<IMaterialDeclareBuildView>
    {
        private List<BaseOrganizeEntity> departmentList;
        private MaterialDeclareTable materialDeclareBuild;

        private ICommand saveCommand;
        private ICommand retreatCommand;

        private string operation;

        private Visibility canSave;
        private Visibility canCreatorVisibal;

        private bool canDepartEdit;
        private bool canTitleEdit;
        private bool canAuthorEdit;
        private bool canDeclareTimeEdit;
        private bool canIssueTimeEdit;
        private bool canScoreEdit;

        [ImportingConstructor]
        public MaterialDeclareBuildViewModel(IMaterialDeclareBuildView view)
            : base(view)
        {
            materialDeclareBuild = new MaterialDeclareTable();


            departmentList = new List<BaseOrganizeEntity>();

            BaseOrganizeManager origanizeService = new BaseOrganizeManager();
            DataTable departmentDT = origanizeService.GetDepartmentDT(""); //根节点 parnetid
            BaseOrganizeEntity entity;
            foreach (DataRow dr in departmentDT.Rows)
            {
                entity = new BaseOrganizeEntity(dr);
                departmentList.Add(entity);
            }

            //ALL
            //BaseOrganizeEntity currentEntity = new BaseOrganizeEntity();
            //currentEntity.Code = "";
            //currentEntity.Id = 0;
            //currentEntity.FullName = "全部";

            //departmentList.Insert(0, currentEntity);

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

        public MaterialDeclareTable MaterialDeclareBuild
        {
            get { return materialDeclareBuild; }
             set
             {
                 if (materialDeclareBuild != value)
                 {
                     materialDeclareBuild = value;
                     RaisePropertyChanged("MaterialDeclareBuild");
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

         public bool CanDepartEdit
         {
             get { return canDepartEdit; }
             set
             {
                 if (canDepartEdit != value)
                 {
                     canDepartEdit = value;
                     RaisePropertyChanged("CanDepartEdit");
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

         public bool CanDeclareTimeEdit
         {
             get { return canDeclareTimeEdit; }
             set
             {
                 if (canDeclareTimeEdit != value)
                 {
                     canDeclareTimeEdit = value;
                     RaisePropertyChanged("CanDeclareTimeEdit");
                 }
             }
         }

         public bool CanIssueTimeEdit
         {
             get { return canIssueTimeEdit; }
             set
             {
                 if (canIssueTimeEdit != value)
                 {
                     canIssueTimeEdit = value;
                     RaisePropertyChanged("CanIssueTimeEdit");
                 }
             }
         }

         public bool CanScoreEdit
         {
             get { return canScoreEdit; }
             set
             {
                 if (canScoreEdit != value)
                 {
                     canScoreEdit = value;
                     RaisePropertyChanged("CanScoreEdit");
                 }
             }
         }

    }
}