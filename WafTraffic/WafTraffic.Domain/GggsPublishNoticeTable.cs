using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WafTraffic.Domain.Properties;
using System.Waf.Foundation;
using System.Windows;
using DotNet.Business;
using WafTraffic.Domain.Common;

namespace WafTraffic.Domain
{
    public partial class GggsPublishNotice
    {
        private int curUserId;

        private Visibility canModify;
        public Visibility CanModify
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canModify = System.Windows.Visibility.Hidden;


                if (CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_PUBLISHNOTICE_MODIFY))
                {
                    canModify = System.Windows.Visibility.Visible;
                }

                if (this.Status == null && this.CreateId == curUserId) //未审核时，创建者可以修改
                {
                    canModify = System.Windows.Visibility.Visible;
                }

                return canModify;
            }
        }

        private Visibility canDelete;
        public Visibility CanDelete
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canDelete = System.Windows.Visibility.Hidden;

                if (CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_PUBLISHNOTICE_DELETE))
                {
                    canDelete = System.Windows.Visibility.Visible;
                }


                if (this.Status == null && this.CreateId == curUserId) //未审核时，创建者可以删除
                {
                    canDelete = System.Windows.Visibility.Visible;
                }

                return canDelete;
            }
        }

        private Visibility canApprove;
        public Visibility CanApprove
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canApprove = System.Windows.Visibility.Hidden;

                if (this.Status == null) //未审批
                {
                    if (CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_PUBLISHNOTICE_APPROVE))
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                }
                else
                {
                    if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                }

                return canApprove;
            }
        }
    }
}

