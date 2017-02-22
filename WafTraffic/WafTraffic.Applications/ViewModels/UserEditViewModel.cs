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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class UserEditViewModel : ViewModel<IUserEditView>
    {
        private BaseUserEntity user;
        private List<BaseOrganizeEntity> departmentList;
        private List<BaseOrganizeEntity> companyList;

        private List<BaseRoleEntity> roleList;

        private ICommand saveCommand;
        private ICommand retreatCommand;
        private string mSignPath;
        private BitmapImage mSignImg;

        [ImportingConstructor]
        public UserEditViewModel(IUserEditView view)
            : base(view)
        {
            user = new BaseUserEntity();
            departmentList = new List<BaseOrganizeEntity>();

            BaseOrganizeManager origanizeService = new BaseOrganizeManager();
            DataTable departmentDT = origanizeService.GetDepartmentDT(""); //根节点 parnetid
            BaseOrganizeEntity entity;
            foreach (DataRow dr in departmentDT.Rows)
            {
                entity = new BaseOrganizeEntity(dr);
                departmentList.Add(entity);
            }

            companyList = new List<BaseOrganizeEntity>();
            DataTable companyDT = origanizeService.GetCompanyDT();
            foreach (DataRow dr in companyDT.Rows)
            {
                entity = new BaseOrganizeEntity(dr);
                companyList.Add(entity);
            }

            BaseRoleEntity roleEntity;
            roleList = new List<BaseRoleEntity>();
            DataTable roleDT = DotNetService.Instance.RoleService.GetDT(CurrentLoginService.Instance.CurrentUserInfo);
            foreach (DataRow dr in roleDT.Rows)
            {
                roleEntity = new BaseRoleEntity(dr);
                roleList.Add(roleEntity);
            }
             
        }

        public string SignPath
        {
            get { return mSignPath; }
            set
            {
                if (mSignPath != value)
                {
                    mSignPath = value;
                    RaisePropertyChanged("SignPath");
                }
            }
        }

        public BitmapImage SignImg
        {
            get { return mSignImg; }
            set
            {
                if (mSignImg != value)
                {
                    mSignImg = value;
                    //防止多线程时访问异常
                    if (mSignImg != null)
                    {
                        mSignImg.Freeze();
                    }
                    RaisePropertyChanged("SignImg");
                }
            }
        }

        public BaseUserEntity User
        {
            get { return user; }
            set
            {
                if (user != value)
                {
                    user = value;
                    RaisePropertyChanged("User");
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

        public List<BaseOrganizeEntity> CompanyList
        {
            get { return companyList; }
            set
            {
                if (companyList != value)
                {
                    companyList = value;
                    RaisePropertyChanged("CompanyList");
                }
            }
        }

        public List<BaseRoleEntity> RoleList
        {
            get { return roleList; }
            set
            {
                if (roleList != value)
                {
                    roleList = value;
                    RaisePropertyChanged("RoleList");
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
    }
}
    