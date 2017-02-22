using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WafTraffic.Domain.Properties;
using System.Waf.Foundation;
using System.Windows;
using DotNet.Business;

namespace WafTraffic.Domain
{
    public partial class CgsYellowMarkCar
    {
        private int curUserId;


        private Visibility canDelete;
        public Visibility CanDelete
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canDelete = System.Windows.Visibility.Hidden;

                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.CgsYellowMarkCar.Delete"))
                {
                    canDelete = System.Windows.Visibility.Visible;         
                }

                return canDelete;
            }
        }
    }
}

