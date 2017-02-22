using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using DotNet.Business;
using WafTraffic.Applications.Common;
using System.Data;

namespace WafTraffic.Applications.Services
{

    internal class AuthService : IAuthService
    {
        #region Data

        private static AuthService instance = null;
        private static object locker = new object();
        private List<BaseOrganizeEntity> mDeptList;
        private List<BaseUserEntity> mSubLeaderList;

        #region Zhzx

        private bool mZhzxRequestCanListAll = false;
        private bool mZhzxRequestCanBrowse = false;
        private bool mZhzxRequestCanAdd = false;
        private bool mZhzxRequestCanModify = false;
        private bool mZhzxRequestCanDelete = false;
        private bool mZhzxRequestCanDeal = false;
        private bool isZhzxCharged = false;

        #endregion

        #region Kjss

        private bool mKjkRequestCanListAll = false;
        private bool mKjkRequestCanBrowse = false;
        private bool mKjkRequestCanAdd = false;
        private bool mKjkRequestCanModify = false;
        private bool mKjkRequestCanDelete = false;
        private bool mKjkRequestCanDeal = false;

        private bool mSskRequestCanListAll = false;
        private bool mSskRequestCanBrowse = false;
        private bool mSskRequestCanAdd = false;
        private bool mSskRequestCanModify = false;
        private bool mSskRequestCanDelete = false;
        private bool mSskRequestCanDeal = false;

        #endregion

        #endregion

        #region Constructor

        public static AuthService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new AuthService();
                            instance.Initialize();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Properties

        #region Zhzx

        public bool ZhzxRequestCanListAll
        {
            get { return mZhzxRequestCanListAll; }
        }

        public bool ZhzxRequestCanBrowse
        {
            get { return mZhzxRequestCanBrowse; }
        }

        public bool ZhzxRequestCanAdd
        {
            get { return mZhzxRequestCanAdd; }
        }

        public bool ZhzxRequestCanModify
        {
            get { return mZhzxRequestCanModify; }
        }

        public bool ZhzxRequestCanDelete
        {
            get { return mZhzxRequestCanDelete; }
        }

        public bool ZhzxRequestCanDeal
        {
            get { return mZhzxRequestCanDeal; }
        }

        public bool IsZhzxCharged       //是不是分管指挥中心
        {
            get { return isZhzxCharged; }
        }

        public List<BaseOrganizeEntity> FddZwChargeDepts
        {
            get { return mDeptList; }
        }

        // 大队领导
        public List<BaseUserEntity> SubLeaderGroup
        {
            get { return mSubLeaderList; }
        }

        #endregion

        #region Kjss

        public bool KjkRequestCanListAll
        {
            get { return mKjkRequestCanListAll; }
        }

        public bool KjkRequestCanBrowse
        {
            get { return mKjkRequestCanBrowse; }
        }

        public bool KjkRequestCanAdd
        {
            get { return mKjkRequestCanAdd; }
        }

        public bool KjkRequestCanModify
        {
            get { return mKjkRequestCanModify; }
        }

        public bool KjkRequestCanDelete
        {
            get { return mKjkRequestCanDelete; }
        }

        public bool KjkRequestCanDeal
        {
            get { return mKjkRequestCanDeal; }
        }


        public bool SskRequestCanListAll
        {
            get { return mSskRequestCanListAll; }
        }

        public bool SskRequestCanBrowse
        {
            get { return mSskRequestCanBrowse; }
        }

        public bool SskRequestCanAdd
        {
            get { return mSskRequestCanAdd; }
        }

        public bool SskRequestCanModify
        {
            get { return mSskRequestCanModify; }
        }

        public bool SskRequestCanDelete
        {
            get { return mSskRequestCanDelete; }
        }

        public bool SskRequestCanDeal
        {
            get { return mSskRequestCanDeal; }
        }

        #endregion

        #endregion

        #region Member

        private void Initialize()
        {
            InitFddZw();
            InitZhzx();
            InitKjk();
            InitSsk();
            InitSubLeaderGroup();
        }

        private void InitFddZw()
        {
            try
            {
                mDeptList = null;

                if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstants.INT_ROLE_FDD_ID
                    || CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstants.INT_ROLE_ZW_ID)
                {
                    BaseOrganizeManager origanizeService = new BaseOrganizeManager();
                    BaseOrganizeEntity entity;
                    mDeptList = new List<BaseOrganizeEntity>();

                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_ZGXCK))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_ZGXCK);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_KJSSK))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_KJSSK);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_SGK))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_SGK);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_CGS))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_CGS);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_FZXFK))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_FZXFK);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_ZHZX))
                    {
                        isZhzxCharged = true;
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_ZHZX);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_ZXK))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_ZXK);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_1ZD))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_1ZD);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_2ZD))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_2ZD);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_3ZD))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_3ZD);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_4ZD))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_4ZD);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_5ZD))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_5ZD);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_6ZD))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_6ZD);
                        mDeptList.Add(entity);
                    }
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_FDDZW_7ZD))
                    {
                        entity = origanizeService.GetEntity(YcConstants.INT_DEPT_ID_7ZD);
                        mDeptList.Add(entity);
                    }
                    entity = origanizeService.GetEntity(YcConstants.INT_COMPANY_ID);
                    mDeptList.Add(entity);
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void InitZhzx()
        {
            try
            {
                mZhzxRequestCanListAll = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_ZHZX_REQUEST_LISTALL);
                mZhzxRequestCanBrowse = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_ZHZX_REQUEST_BROWSE);
                mZhzxRequestCanAdd = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_ZHZX_REQUEST_ADD);
                mZhzxRequestCanModify = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_ZHZX_REQUEST_MODIFY);
                mZhzxRequestCanDelete = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_ZHZX_REQUEST_DELETE);
                mZhzxRequestCanDeal = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_ZHZX_REQUEST_DEAL);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void InitKjk()
        {
            try
            {
                mKjkRequestCanListAll = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_KJK_REQUEST_LISTALL);
                mKjkRequestCanBrowse = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_KJK_REQUEST_BROWSE);
                mKjkRequestCanAdd = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_KJK_REQUEST_ADD);
                mKjkRequestCanModify = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_KJK_REQUEST_MODIFY);
                mKjkRequestCanDelete = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_KJK_REQUEST_DELETE);
                mKjkRequestCanDeal = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_KJK_REQUEST_DEAL);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void InitSsk()
        {
            try
            {
                mSskRequestCanListAll = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_SSK_REQUEST_LISTALL);
                mSskRequestCanBrowse = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_SSK_REQUEST_BROWSE);
                mSskRequestCanAdd = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_SSK_REQUEST_ADD);
                mSskRequestCanModify = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_SSK_REQUEST_MODIFY);
                mSskRequestCanDelete = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_SSK_REQUEST_DELETE);
                mSskRequestCanDeal = CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_SSK_REQUEST_DEAL);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        private void InitSubLeaderGroup()
        {
            try
            {
                mSubLeaderList = null;

                DataTable leaderDT = DotNetService.Instance.UserService.GetDTByDepartment(
                    CurrentLoginService.Instance.CurrentUserInfo, 
                    YcConstants.INT_DEPT_ID_DDLD.ToString(), false);
                BaseUserEntity entity;
                mSubLeaderList = new List<BaseUserEntity>();
                foreach (DataRow dr in leaderDT.Rows)
                {
                    entity = new BaseUserEntity(dr);
                    if (entity.RoleId != YcConstants.INT_ROLE_DDZ_ID)
                    {
                        mSubLeaderList.Add(entity);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        public int GetSquadronLogbookOwnDept()
        {
            if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstants.INT_ROLE_ADMIN_ID)
            {
                return Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.CompanyId);
            }
            else
            {
                return Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
            }
        }

        public string GetDepartmentNameById(int? id)
        {
            string deptName = "";
            try
            {
                if (id != null)
                {
                    BaseOrganizeManager origanizeService = new BaseOrganizeManager();
                    BaseOrganizeEntity entity = origanizeService.GetEntity(id);
                    deptName = entity.FullName;
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
            return deptName;
        }

        public string GetUserNameById(int? id)
        {
            string userName = "";
            try
            {
                if (id != null)
                {
                    BaseUserManager userService = new BaseUserManager();
                    BaseUserEntity entity = userService.GetEntity(id);
                    userName = entity.RealName;
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
            return userName;
        }

        public bool IsLeader(int? departmentId)
        {
            return (departmentId == YcConstants.INT_DEPT_ID_DDLD);
        }

        #endregion

    }
}
