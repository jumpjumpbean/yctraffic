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
    internal interface IMayorHotlineTaskTable
    {
        //[Required(ErrorMessage = "到期时间是必填项")]
        //Nullable<global::System.DateTime> DueDate { get; set; }
    }


    public partial class MayorHotlineTaskTable : IMayorHotlineTaskTable
    {  
        //private Visibility canBrowse;
        //public Visibility CanBrowse
        //{
        //    get
        //    {
        //        canBrowse = System.Windows.Visibility.Visible;                
        //        return canBrowse;
        //    }
        //}

        private Visibility canModify;
        public Visibility CanModify
        {
            get
            {
                canModify = System.Windows.Visibility.Visible;

                if (this.StatusId > Convert.ToInt32(HotLineStatus.WaitDeal))
                {
                    canModify = System.Windows.Visibility.Hidden;
                }
                return canModify;
            }
        }

        private Visibility canDelete;
        public Visibility CanDelete
        {
            get
            {
                canDelete = System.Windows.Visibility.Visible;
                if (this.StatusId > Convert.ToInt32(HotLineStatus.WaitDeal))
                {
                    canDelete = System.Windows.Visibility.Hidden;
                }
                return canDelete;
            }
        }
        //待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  归档 :8
        private Visibility canDeal;
        public Visibility CanDeal
        {
            get
            {
                canDeal = System.Windows.Visibility.Hidden;
                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    if (this.StatusId == Convert.ToInt32(HotLineStatus.ToDDZ) || this.StatusId == Convert.ToInt32(HotLineStatus.ToZW) || this.StatusId == Convert.ToInt32(HotLineStatus.ZWToKS) || this.StatusId == Convert.ToInt32(HotLineStatus.DDZToKS) || this.StatusId == Convert.ToInt32(HotLineStatus.ReplyDDZ))
                    {
                        canDeal = System.Windows.Visibility.Visible;
                    }
                }
                else if ((this.StatusId == Convert.ToInt32(HotLineStatus.ZWToKS) && !object.Equals(null, this.OwnDepartmentId) && this.OwnDepartmentId == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId)
                    || (this.StatusId == Convert.ToInt32(HotLineStatus.DDZToKS) && !object.Equals(null, this.OwnDepartmentId) && this.OwnDepartmentId == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId)
                    || (this.StatusId == Convert.ToInt32(HotLineStatus.ToZW) && CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZW)
                    || ((this.StatusId == Convert.ToInt32(HotLineStatus.ToDDZ) || this.StatusId == Convert.ToInt32(HotLineStatus.ReplyDDZ)) && CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_DDZ))
                  //  || (this.StatusId == 6 && this.SovleUserId == Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id))) 
                {
                    canDeal = System.Windows.Visibility.Visible;
                }
                return canDeal;
            }
        }

        private Visibility canCheck;
        public Visibility CanCheck
        {
            get
            {
                canCheck = System.Windows.Visibility.Visible;
                if (this.StatusId != Convert.ToInt32(HotLineStatus.Dealed)) //已处理 :7
                {
                    canCheck = System.Windows.Visibility.Hidden;
                }
                return canCheck;
            }
        }
    }
}
