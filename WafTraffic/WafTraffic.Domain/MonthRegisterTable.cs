using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WafTraffic.Domain.Properties;
using System.Waf.Foundation;
using System.Windows;
using DotNet.Business;
using System.Collections.Generic;
using WafTraffic.Domain.Common;

namespace WafTraffic.Domain
{
    public partial class MonthRegisterTable
    {
        private int curUserId;

        private Visibility canModify;
        public Visibility CanModify
        {
            get
            {
                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);
                canModify = System.Windows.Visibility.Visible;

                if ( (!CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator && this.CreateUserId != null && this.CreateUserId != curUserId)
                        || this.StatusId > Convert.ToInt32(MonthRegisterStatus.Create))
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
                if ( (!CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator && this.CreateUserId != null && this.CreateUserId != curUserId)
                    || this.StatusId > Convert.ToInt32(MonthRegisterStatus.Create))
                {
                    canDelete = System.Windows.Visibility.Collapsed;
                }
                return canDelete;
            }
        }

        private Visibility canApprove;
        public Visibility CanApprove
        {
            get
            {
                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    if (this.StatusId > Convert.ToInt32(MonthRegisterStatus.Create) && this.StatusId < Convert.ToInt32(MonthRegisterStatus.Approve))
                    {
                        return System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        return System.Windows.Visibility.Hidden;
                    }
                }

                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);

                string curDepartCode = "";
                if (CurrentLoginService.Instance.CurrentUserInfo.DepartmentId == null)
                {
                    curDepartCode = "-1";
                }
                else
                {
                    curDepartCode = DotNetService.Instance.OrganizeService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo, CurrentLoginService.Instance.CurrentUserInfo.DepartmentId.ToString()).Code;
                }
                canApprove = System.Windows.Visibility.Collapsed;

                RoleService roleService = new RoleService();
                string[] ksdzArray = roleService.GetRoleUserIds(CurrentLoginService.Instance.CurrentUserInfo, YcConstantTable.GROUPID_KSDZ.ToString()); //科所队长
                List<int?> ksdzList = new List<int?>();
                foreach (string tmpuserID in ksdzArray)
                {
                    ksdzList.Add(Convert.ToInt32(tmpuserID));
                }

                string[] viceUserArray = DotNetService.Instance.RoleService.GetRoleUserIds(CurrentLoginService.Instance.CurrentUserInfo, YcConstantTable.GROUPID_VICE.ToString()); //副大队长和政委
                List<int?> viceUserList = new List<int?>();
                foreach (string tmpuserID in viceUserArray)
                {
                    viceUserList.Add(Convert.ToInt32(tmpuserID));
                }

                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {//管理员填写大队长审批结果
                    if (this.StatusId > Convert.ToInt32(MonthRegisterStatus.Create) && this.StatusId < Convert.ToInt32(MonthRegisterStatus.Approve))
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.Approve")
                    &&
                    !CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ApproveKSDZ")
                    &&
                    !CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ApproveVice")
                    )
                { //审批本科室干警

                    if (this.StatusId > Convert.ToInt32(MonthRegisterStatus.Create) && this.StatusId < Convert.ToInt32(MonthRegisterStatus.Approve) && (this.DepartmentCode != null && this.DepartmentCode.IndexOf(curDepartCode) == 0) && !ksdzList.Contains(this.UserId) && !viceUserList.Contains(this.UserId))
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }

                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.Approve")
                      &&
                      CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ApproveKSDZ")
                      &&
                      CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ApproveVice"))
                {//审批科室队长月报,评价副大队长和政委 (及其部门干警)
                    if (this.StatusId > Convert.ToInt32(MonthRegisterStatus.Create) && this.StatusId < Convert.ToInt32(MonthRegisterStatus.Approve) && (viceUserList.Contains(this.UserId) || ksdzList.Contains(this.UserId) || (this.DepartmentCode != null && this.DepartmentCode.IndexOf(curDepartCode) == 0)))
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.Approve")
                &&
                CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ApproveKSDZ"))
                {//审批科室队长月报 (及其部门干警)
                    if (this.StatusId > Convert.ToInt32(MonthRegisterStatus.Create) && this.StatusId < Convert.ToInt32(MonthRegisterStatus.Approve) && (ksdzList.Contains(this.UserId) || (this.DepartmentCode != null && this.DepartmentCode.IndexOf(curDepartCode) == 0)))
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.Approve")
                &&
                CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ApproveVice"))
                {//评价副大队长和政委 (及其部门干警)
                    if (this.StatusId > Convert.ToInt32(MonthRegisterStatus.Create) && this.StatusId < Convert.ToInt32(MonthRegisterStatus.Approve) && (viceUserList.Contains(this.UserId) || (this.DepartmentCode != null && this.DepartmentCode.IndexOf(curDepartCode) == 0)))
                    {
                        canApprove = System.Windows.Visibility.Visible;
                    }
                }

                return canApprove;
            }
        }
    }
}
