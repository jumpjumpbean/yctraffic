namespace DotNet.Business
{
    using DotNet.Utilities;
    using System;

    public class ConstraintUtil
    {
        public static string ParameterReference = "当前用户主键：用户主键（CurrentUserId）\r\n当前用户编号：用户编号（CurrentUserCode）\r\n当前用户名  ：用户名（CurrentUserName）\r\n当前用户姓名：用户姓名（CurrentRealName）\r\n所在公司主键：公司主键（CurrentCompanyId）\r\n所在公司名称：公司名称（CurrentCompanyName）\r\n所在公司编号：公司编号（CurrentCompanyCode）\r\n所在部门主键：部门主键（CurrentDepartmentId）\r\n所在部门名称：部门名称（CurrentDepartmentName）\r\n所在部门编号：部门编号（CurrentDepartmentCode）\r\n所在工作组主键：工作组主键（CurrentWorkgroupId）\r\n所在工作组名称：工作组名称（CurrentWorkgroupName）\r\n所在工作组编号：工作组编号（CurrentWorkgroupCode）";

        public static string PrepareParameter(string constraint)
        {
            return PrepareParameter(BaseSystemInfo.UserInfo, constraint);
        }

        public static string PrepareParameter(BaseUserInfo userInfo, string constraint)
        {
            constraint = constraint.Replace("用户主键", userInfo.Id);
            constraint = constraint.Replace("CurrentUserId", userInfo.Id);
            constraint = constraint.Replace("用户编号", userInfo.Code);
            constraint = constraint.Replace("CurrentUserCode", userInfo.Code);
            constraint = constraint.Replace("用户名", userInfo.UserName);
            constraint = constraint.Replace("CurrentUserName", userInfo.UserName);
            constraint = constraint.Replace("用户姓名", userInfo.RealName);
            constraint = constraint.Replace("CurrentRealName", userInfo.RealName);
            constraint = constraint.Replace("公司主键", userInfo.CompanyId.ToString());
            constraint = constraint.Replace("CurrentCompanyId", userInfo.CompanyId.ToString());
            constraint = constraint.Replace("部门主键", userInfo.DepartmentId.ToString());
            constraint = constraint.Replace("CurrentDepartmentId", userInfo.DepartmentId.ToString());
            constraint = constraint.Replace("工作组主键", userInfo.WorkgroupId.ToString());
            constraint = constraint.Replace("CurrentWorkgroupId", userInfo.WorkgroupId.ToString());
            return constraint;
        }
    }
}

