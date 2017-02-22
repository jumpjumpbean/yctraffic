using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DotNet.Business;
using WafTraffic.Domain.Common;

namespace WafTraffic.Domain
{
    public partial class ZdtzCyDangerDeal
    {
        #region Constructors

        public ZdtzCyDangerDeal()
        {
        }

        #endregion

        #region Members

        private Visibility canBrowse;
        public Visibility CanBrowse
        {
            get
            {
                canBrowse = System.Windows.Visibility.Collapsed;
                if (CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_LOGBOOK_BROWSE))
                {
                    canBrowse = System.Windows.Visibility.Visible;
                }
                return canBrowse;
            }
        }

        private Visibility canModify;
        public Visibility CanModify
        {
            get
            {
                canModify = System.Windows.Visibility.Collapsed;
                // 一般来说，已提交的请求不允许提交者再更改
                // 该台账中如果允许编辑，则控件的Visibility需要区分此次动作是Modify/Deal/Check，
                // 否则Modify时针对审批人的部分项目也变得可见
                /*
                if (CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_LOGBOOK_MODIFY)
                    && this.Status < 3)
                {
                    canModify = System.Windows.Visibility.Visible;
                }*/
                return canModify;
            }
        }

        private Visibility canDelete;
        public Visibility CanDelete
        {
            get
            {
                canDelete = System.Windows.Visibility.Collapsed;
                if (CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_LOGBOOK_DELETE))
                {
                    canDelete = System.Windows.Visibility.Visible;
                }
                return canDelete;
            }
        }

        private Visibility canDeal;
        public Visibility CanDeal
        {
            get
            {
                canDeal = System.Windows.Visibility.Collapsed;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_LOGBOOK_DEAL))
                //{
                    switch (this.Status)
                    {
                        case YcConstantTable.INT_DANGER_DEAL_SATUS_NEW:
                            if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZXK_KZ
                                || CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZXK_YG)
                            {
                                canDeal = System.Windows.Visibility.Visible;
                            }
                            break;
                        case YcConstantTable.INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE:
                            if (CurrentLoginService.Instance.CurrentUserInfo.Id.Equals(this.SubLeaderId.ToString()))
                            {
                                canDeal = System.Windows.Visibility.Visible;
                            }
                            break;
                        default:
                            break;
                    }
                //}
                return canDeal;
            }
        }

        private Visibility canCheck;
        public Visibility CanCheck
        {
            get
            {
                canCheck = System.Windows.Visibility.Collapsed;
                if (CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_LOGBOOK_VERIFY)
                    && this.Status == YcConstantTable.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY) //已上传整改项
                {
                    canCheck = System.Windows.Visibility.Visible;
                }
                return canCheck;
            }
        }

        #endregion
    }
}
