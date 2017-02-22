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
    public partial class SgkReleaseCar
    {
        private int curUserId;

        private Visibility canModify;
        public Visibility CanModify
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canModify = System.Windows.Visibility.Hidden;


                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.SgkReleaseCar.Modify"))
                {
                    canModify = System.Windows.Visibility.Visible;
                }

                if (IsChargeSigned == false) //未审批时，创建者可以修改
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

                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.SgkReleaseCar.Delete"))
                {
                    canDelete = System.Windows.Visibility.Visible;
                }


                if (IsChargeSigned == false) //未审批时，创建者可以删除
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

                BaseUserEntity entity = null;

                System.Data.DataTable dt = DotNetService.Instance.UserService.Search(CurrentLoginService.Instance.CurrentUserInfo, "xingqingsheng", null, null);

                if (dt.Rows.Count == 1)
                {
                    entity = new BaseUserEntity(dt);
                }

                int ChargeId1 = 10000932;
                int ChargeId2 = (int)entity.Id;
                int ChargeId3 = 10000927;

                if (IsChargeSigned == false)
                {
                    if (curUserId == ChargeId1)
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                }
                if (IsChargeSigned == true && IsSubLeader1Signed == false)
                {
                    if (curUserId == ChargeId2)
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                }
                if (IsSubLeader1Signed == true && IsSubLeader2Signed == false)
                {
                    if (curUserId == ChargeId3)
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                }

                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    canApprove = System.Windows.Visibility.Visible;
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

        private byte[] fDDSignatureImg;
        public byte[] FDDSignatureImg
        {
            get
            {
                return fDDSignatureImg;
            }
            set
            {
                if (fDDSignatureImg != value)
                {
                    fDDSignatureImg = value;
                }
            }
        }
    }
}

