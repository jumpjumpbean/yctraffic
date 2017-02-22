using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WafTraffic.Domain.Common;
using DotNet.Business;

namespace WafTraffic.Domain
{
    public partial class KjssEquipmentRequest
    {

        #region Data

        private bool mCanBrowse;
        private bool mCanModify;
        private bool mCanDelete;
        private bool mCanDeal;

        #endregion

        #region Properties

        public bool CanBrowse
        {
            get
            {
                mCanBrowse = AuthUtil.Instance.KjkRequestCanBrowse;
                return mCanBrowse;
            }
        }

        public bool CanModify
        {
            get
            {

                if (this.Status == YcConstantTable.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE
                    && CurrentLoginService.Instance.CurrentUserInfo.RealName == this.Applicant
                    && AuthUtil.Instance.KjkRequestCanModify)
                {
                    mCanModify = true;
                }
                else
                {
                    mCanModify = false;
                }

                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    mCanModify = true;
                }
                
                return mCanModify;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (this.Status == YcConstantTable.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE
                    && CurrentLoginService.Instance.CurrentUserInfo.RealName == this.Applicant
                    && AuthUtil.Instance.KjkRequestCanDelete)
                {
                    mCanDelete = true;
                }
                else
                {
                    mCanDelete = false;
                }

                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    mCanDelete = true;
                }

                return mCanDelete;
            }
        }

        public bool CanDeal
        {
            get
            {
                mCanDeal = false;
                if (AuthUtil.Instance.KjkRequestCanDeal)
                {
                    switch (this.Status)
                    {
                        case YcConstantTable.INT_KJSS_REQSTAT_NULL:
                            if (CurrentLoginService.Instance.CurrentUserInfo.Id.Equals(this.CreateId.ToString()))
                            {
                                mCanDeal = true;
                            }
                            break;
                        case YcConstantTable.INT_KJSS_REQSTAT_SUB_LEADER_APPROVE:
                            if (CurrentLoginService.Instance.CurrentUserInfo.Id.Equals(this.SubLeaderId.ToString()))
                            {
                                mCanDeal = true;
                            }
                            break;
                        case YcConstantTable.INT_KJSS_REQSTAT_DDZ_APPROVE:
                            if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_DDZ)
                            {
                                mCanDeal = true;
                            }
                            break;
                        case YcConstantTable.INT_KJSS_REQSTAT_BGS_EXECUTE:
                            if (CurrentLoginService.Instance.CurrentUserInfo.DepartmentId == YcConstantTable.ORGID_BGS)
                            {
                                mCanDeal = true;
                            }
                            break;
                        case YcConstantTable.INT_KJSS_REQSTAT_ZHZX_EXECUTE:
                            if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZHZX_ZR
                                || CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZHZX_SCRY
                                || CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZHZX_SHRY)
                            {
                                mCanDeal = true;
                            }
                            break;
                        /*
                        case YcConstantTable.INT_ZHZX_REQSTAT_REQDEPT_APPROVE:
                            if (CurrentLoginService.Instance.CurrentUserInfo.Id.Equals(this.CreateId.ToString()))
                            {
                                mCanDeal = true;
                            }
                            break;
                        case YcConstantTable.INT_ZHZX_REQSTAT_DDZ_SUPERVISE:
                            if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_DDZ)
                            {
                                mCanDeal = true;
                            }
                            break;
                         */
                        default:
                            break;
                    }
                }

                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator) mCanDeal = true;

                return mCanDeal;
            }
        }

        #endregion

    }
}
