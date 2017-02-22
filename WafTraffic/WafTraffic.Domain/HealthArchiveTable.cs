using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WafTraffic.Domain.Properties;
using System.Waf.Foundation;
using System.Windows;
using DotNet.Business;

namespace WafTraffic.Domain
{
    public partial class HealthArchiveTable
    {
        private int curUserId;

        private Visibility canModify;
        public Visibility CanModify
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canModify = System.Windows.Visibility.Visible;

                if (!CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator && 
                                this.CreateUserId != null && this.CreateUserId != curUserId) 
                {
                    canModify = System.Windows.Visibility.Collapsed;
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
                canDelete = System.Windows.Visibility.Visible;
                if (!CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator && 
                            this.CreateUserId != null && this.CreateUserId != curUserId) 
                {
                    canDelete = System.Windows.Visibility.Collapsed;
                }
                return canDelete;
            }
        }
    }
}

