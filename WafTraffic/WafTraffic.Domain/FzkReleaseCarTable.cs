using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WafTraffic.Domain.Properties;
using System.Waf.Foundation;
using System.Windows;
using DotNet.Business;

namespace WafTraffic.Domain
{
    public partial class FzkReleaseCar
    {
        private int curUserId;

        private Visibility canModify;
        public Visibility CanModify
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canModify = System.Windows.Visibility.Hidden;


                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.FzkReleaseCar.Modify"))
                {
                    canModify = System.Windows.Visibility.Visible;
                }

                if (string.IsNullOrEmpty(this.ApproveResult)) //未审批时，创建者可以修改
                {
                    if (this.CreateId != null && this.CreateId == curUserId)
                    {
                        canModify = System.Windows.Visibility.Visible;
                    }
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

                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.FzkReleaseCar.Delete"))
                {
                    canDelete = System.Windows.Visibility.Visible;
                }


                if (string.IsNullOrEmpty(this.ApproveResult)) //未审批时，创建者可以删除
                {
                    if (this.CreateId != null && this.CreateId == curUserId)
                    {
                        canDelete = System.Windows.Visibility.Visible;
                    }
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

                if (string.IsNullOrEmpty(this.ApproveResult) && (this.IsChargeSigned == false)) //未审批
                {
                    if (CurrentLoginService.Instance.IsAuthorized("yctraffic.FzkReleaseCar.Approve"))
                    //if (curUserId == ApproverId)
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

        private byte[] personSignatureImg;
        public byte[] PersonSignatureImg
        {
            get
            {
                return personSignatureImg;
            }
            set
            {
                if (personSignatureImg != value)
                {
                    personSignatureImg = value;
                }
            }
        }


        private byte[] chargeSignatureImg;
        public byte[] ChargeSignatureImg
        {
            get
            {
                return chargeSignatureImg;
            }
            set
            {
                if (chargeSignatureImg != value)
                {
                    chargeSignatureImg = value;
                }
            }
        }
    }
}

