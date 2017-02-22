//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

using System;

namespace DotNet.Utilities
{
	/// <summary>
    /// BaseUserInfo
    /// 用户类
	/// 
	/// 修改纪录
    /// 
    ///		2011.09.12 JiRiGaLa 版本：2.1 公司名称、部门名称、工作组名称进行重构。
    ///		2011.05.11 JiRiGaLa 版本：2.0 增加安全通讯用户名、密码。
    ///		2008.08.26 JiRiGaLa 版本：1.2 整理主键。
    ///		2006.05.03 JiRiGaLa 版本：1.1 添加到工程项目中。
    ///		2006.01.21 JiRiGaLa 版本：1.0 远程传递参数用属性才可以。
	///		
	/// 版本：2.1
	///
	/// <author>
	///		<name>JiRiGaLa</name>
    ///		<date>2011.09.12</date>
	/// </author> 
	/// </summary>
	[Serializable]
	public class BaseUserInfo
	{
        public BaseUserInfo()
        {
            this.GetUserInfo();
        }

        /// <summary>
        /// 远程调用Service用户名（为了提高软件的安全性）
        /// </summary>
        private string serviceUserName = "Bop";
        public string ServiceUserName
        {
            get
            {
                return this.serviceUserName;
            }
            set
            {
                this.serviceUserName = value;
            }
        }

        /// <summary>
        /// 远程调用Service密码（为了提高软件的安全性）
        /// </summary>
        private string servicePassword = "Bop";
        public string ServicePassword
        {
            get
            {
                return this.servicePassword;
            }
            set
            {
                this.servicePassword = value;
            }
        }

        /// <summary>
        /// 单点登录唯一识别标识
        /// </summary>
        private string openId = string.Empty;
        public string OpenId
        {
            get
            {
                return this.openId;
            }
            set
            {
                this.openId = value;
            }
        }

        /// <summary>
        /// 目标用户
        /// </summary>
        private string targetUserId = string.Empty;
        public string TargetUserId
        {
            get
            {
                return this.targetUserId;
            }
            set
            {
                this.targetUserId = value;
            }
        }

        /// <summary>
        /// 用户主键
        /// </summary>
        private string id = string.Empty;
        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// 员工主键
        /// </summary>
        private string staffId = string.Empty;
        public string StaffId
        {
            get
            {
                return this.staffId;
            }
            set
            {
                this.staffId = value;
            }
        }
        
        /// <summary>
        /// 用户用户名
        /// </summary>
        private string userName = string.Empty;
        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }
        
        /// <summary>
        /// 用户姓名
        /// </summary>
        private string realName = string.Empty;
        public string RealName
        {
            get
            {
                return this.realName;
            }
            set
            {
                this.realName = value;
            }
        }

        /// <summary>
        /// 编号
        /// </summary>
        private string code = string.Empty;
        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        /// <summary>
        /// 当前的组织结构公司主键
        /// </summary>
        private int? companyId = null;
        public int? CompanyId
        {
            get
            {
                return this.companyId;
            }
            set
            {
                this.companyId = value;
            }
        }
        
        /// <summary>
        /// 当前的组织结构公司编号
        /// </summary>
        private string companyCode = string.Empty;
        public string CompanyCode
        {
            get
            {
                return this.companyCode;
            }
            set
            {
                this.companyCode = value;
            }
        }
        
        /// <summary>
        /// 当前的组织结构公司名称
        /// </summary>
        private string companyName = string.Empty;
        public string CompanyName
        {
            get
            {
                return this.companyName;
            }
            set
            {
                this.companyName = value;
            }
        }

        /// <summary>
        /// 当前的组织结构部门主键
        /// </summary>
        private int? departmentId = null;
        public int? DepartmentId
        {
            get
            {
                return this.departmentId;
            }
            set
            {
                this.departmentId = value;
            }
        }

        /// <summary>
        /// 当前的组织结构部门编号
        /// </summary>
        private string departmentCode = string.Empty;
        public string DepartmentCode
        {
            get
            {
                return this.departmentCode;
            }
            set
            {
                this.departmentCode = value;
            }
        }

        /// <summary>
        /// 当前的组织结构部门名称
        /// </summary>
        private string departmentName = string.Empty;
        public string DepartmentName
        {
            get
            {
                return this.departmentName;
            }
            set
            {
                this.departmentName = value;
            }
        }

        /// <summary>
        /// 当前的组织结构工作组主键
        /// </summary>
        private int? workgroupId = null;
        public int? WorkgroupId
        {
            get
            {
                return this.workgroupId;
            }
            set
            {
                this.workgroupId = value;
            }
        }

        /// <summary>
        /// 当前的组织结构工作组编号
        /// </summary>
        private string workgroupCode = string.Empty;
        public string WorkgroupCode
        {
            get
            {
                return this.workgroupCode;
            }
            set
            {
                this.workgroupCode = value;
            }
        }

        /// <summary>
        /// 当前的组织结构工作组名称
        /// </summary>
        private string workgroupName = string.Empty;
        public string WorkgroupName
        {
            get
            {
                return this.workgroupName;
            }
            set
            {
                this.workgroupName = value;
            }
        }       

        /// <summary>
        /// 默认角色
        /// </summary>
        private int? roleId = null;
        public int? RoleId
        {
            get
            {
                return this.roleId;
            }
            set
            {
                this.roleId = value;
            }
        }

        /// <summary>
        /// 默认角色名称
        /// </summary>
        private string roleName = string.Empty;
        public string RoleName
        {
            get
            {
                return this.roleName;
            }
            set
            {
                this.roleName = value;
            }
        }

        /// <summary>
        /// 安全级别
        /// </summary>
        private int securityLevel = 0;
        public int SecurityLevel
        {
            get
            {
                return this.securityLevel;
            }
            set
            {
                this.securityLevel = value;
            }
        }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        private bool isAdministrator = false;
        public bool IsAdministrator
        {
            get
            {
                return this.isAdministrator;
            }
            set
            {
                this.isAdministrator = value;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        private string password = string.Empty;
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        private string ipAddress = string.Empty;
        public string IPAddress
        {
            get
            {
                return this.ipAddress;
            }
            set
            {
                this.ipAddress = value;
            }
        }
        
        /// <summary>
        /// MAC地址
        /// </summary>
        private string macAddress = string.Empty;
        public string MACAddress
        {
            get
            {
                return this.macAddress;
            }
            set
            {
                this.macAddress = value;
            }
        }

        /// <summary>
        /// 当前语言选择
        /// </summary>
        private string currentLanguage = string.Empty;
        public string CurrentLanguage
        {
            get
            {
                return this.currentLanguage;
            }
            set
            {
                this.currentLanguage = value;
            }
        }

        /// <summary>
        /// 当前布局风格选择
        /// </summary>
        private string themes = string.Empty;
        public string Themes
        {
            get
            {
                return this.themes;
            }
            set
            {
                this.themes = value;
            }
        }

        #region public void GetUserInfo()
        /// <summary>
        /// 获取信息
        /// </summary>
        public void GetUserInfo()
		{
            this.ServiceUserName = BaseSystemInfo.ServiceUserName;
            this.ServicePassword = BaseSystemInfo.ServicePassword;
            this.CurrentLanguage = BaseSystemInfo.CurrentLanguage;
            this.Themes = BaseSystemInfo.Themes;
        }
		#endregion
	}
}