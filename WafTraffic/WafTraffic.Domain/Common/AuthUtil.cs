using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNet.Business;
using WafTraffic.Domain.Common;

namespace WafTraffic.Domain
{
    internal class AuthUtil
    {
        #region Data

        private static AuthUtil instance = null;
        private static object locker = new object();

        #region Zhzx

        private bool mZhzxRequestCanListAll = false;
        private bool mZhzxRequestCanBrowse = false;
        private bool mZhzxRequestCanAdd = false;
        private bool mZhzxRequestCanModify = false;
        private bool mZhzxRequestCanDelete = false;
        private bool mZhzxRequestCanDeal = false;

        #endregion

        #region Kjk

        private bool mKjkRequestCanListAll = false;
        private bool mKjkRequestCanBrowse = false;
        private bool mKjkRequestCanAdd = false;
        private bool mKjkRequestCanModify = false;
        private bool mKjkRequestCanDelete = false;
        private bool mKjkRequestCanDeal = false;

        #endregion

        #region Ssk

        private bool mSskRequestCanListAll = false;
        private bool mSskRequestCanBrowse = false;
        private bool mSskRequestCanAdd = false;
        private bool mSskRequestCanModify = false;
        private bool mSskRequestCanDelete = false;
        private bool mSskRequestCanDeal = false;

        #endregion

        #endregion

        #region Constructor

        public static AuthUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new AuthUtil();
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

        #endregion

        #region Kjk

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

        #endregion

        #region Ssk

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

        #region Members

        private void Initialize()
        {
            InitZhzx();
            InitKjk();
        }

        private void InitZhzx()
        {
            try
            {
                mZhzxRequestCanListAll = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_ZHZX_REQUEST_LISTALL);
                mZhzxRequestCanBrowse = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_ZHZX_REQUEST_BROWSE);
                mZhzxRequestCanAdd = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_ZHZX_REQUEST_ADD);
                mZhzxRequestCanModify = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_ZHZX_REQUEST_MODIFY);
                mZhzxRequestCanDelete = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_ZHZX_REQUEST_DELETE);
                mZhzxRequestCanDeal = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_ZHZX_REQUEST_DEAL);
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
                mKjkRequestCanListAll = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_KJK_REQUEST_LISTALL);
                mKjkRequestCanBrowse = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_KJK_REQUEST_BROWSE);
                mKjkRequestCanAdd = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_KJK_REQUEST_ADD);
                mKjkRequestCanModify = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_KJK_REQUEST_MODIFY);
                mKjkRequestCanDelete = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_KJK_REQUEST_DELETE);
                mKjkRequestCanDeal = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_KJK_REQUEST_DEAL);
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
                mKjkRequestCanListAll = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_SSK_REQUEST_LISTALL);
                mKjkRequestCanBrowse = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_SSK_REQUEST_BROWSE);
                mKjkRequestCanAdd = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_SSK_REQUEST_ADD);
                mKjkRequestCanModify = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_SSK_REQUEST_MODIFY);
                mKjkRequestCanDelete = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_SSK_REQUEST_DELETE);
                mKjkRequestCanDeal = CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_SSK_REQUEST_DEAL);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }

        #endregion
    }
}
