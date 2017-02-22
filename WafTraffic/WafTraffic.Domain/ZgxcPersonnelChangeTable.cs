using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WafTraffic.Domain.Properties;
using System.Waf.Foundation;
using System.Windows;
using DotNet.Business;

namespace WafTraffic.Domain
{
    public partial class ZgxcPersonnelChange
    {
        private int curUserId;

        private Visibility canModify;
        public Visibility CanModify
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canModify = System.Windows.Visibility.Hidden;

                if (this.RecordStatus == "待审核") //未审批时，创建者可以修改
                {
                    if (this.CreateId != null && this.CreateId == curUserId)
                    {
                        canModify = System.Windows.Visibility.Visible;
                    }
                }
                else
                { 
                
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

                if (this.RecordStatus == "待审核") //未审批时，创建者可以修改
                {
                    if (this.CreateId != null && this.CreateId == curUserId)
                    {
                        canDelete = System.Windows.Visibility.Visible;
                    }
                }
                else
                {

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

                if (this.RecordStatus == "待审核" &&　CurrentLoginService.Instance.IsAuthorized("yctraffic.ZgxcPersonnelChange.Approve"))
                {
                    canApprove = System.Windows.Visibility.Visible;
                }
                else if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    canApprove = System.Windows.Visibility.Visible;
                }

                return canApprove;
            }
        }

        private Visibility canSign;
        public Visibility CanSign
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canSign = System.Windows.Visibility.Hidden;

                if (this.RecordStatus == "已审核" && CurrentLoginService.Instance.IsAuthorized("yctraffic.ZgxcPersonnelChange.Sign"))
                {
                    canSign = System.Windows.Visibility.Visible;
                }
                else if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    canSign = System.Windows.Visibility.Visible;
                }

                return canSign;
            }
        }

        private Visibility canArchive;
        public Visibility CanArchive
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canArchive = System.Windows.Visibility.Hidden;

                if (this.RecordStatus == "已签收" && CurrentLoginService.Instance.IsAuthorized("yctraffic.ZgxcPersonnelChange.Archive"))
                {
                    canArchive = System.Windows.Visibility.Visible;
                }
                else if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    canArchive = System.Windows.Visibility.Visible;
                }

                return canArchive;
            }
        }


        


    }
}

