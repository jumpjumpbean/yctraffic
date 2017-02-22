//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------
/// 
/// 修改记录
///     2012.02.02 版本：3.8 sunmiao    保证UserCenterDbConnection\BusinessDbConnection的初始化
///     2012.02.02 版本：3.8 zhangyi    修改OrganizeDynamicLoading = true。
///     2011.10.07 版本：2.3 JiRiGaLa   每个数据库都支持多类型数据库。
///     2011.07.15 版本：2.2 JiRiGaLa   参数信息整理，获取硬件信息的功能部分进行分离。
///     2011.07.07 版本：2.1 zgl        优化获取IP地址和Mac地址的方法
///		2010.09.19 版本：2.0 JiRiGaLa   彻底进行重构。
///		2007.04.02 版本：1.2 JiRiGaLa   进行主键优化。
///		2007.01.02 版本：1.1 JiRiGaLa   进行主键优化。
///		2006.02.05 版本：1.1 JiRiGaLa	重新调整主键的规范化。
///		2004.11.19 版本：1.0 JiRiGaLa	主键创建。
///		
/// 版本：3.8
///

using System;

namespace DotNet.Utilities
{
    /// <summary>
    /// BaseSystemInfo
    /// 这是系统的核心基础信息部分 
    /// </summary>
    public class BaseSystemInfo
    {
        /// <summary>
        /// 是否已经成功登录系统
        /// </summary>
        public static bool LogOned = false;

        /// <summary>
        /// 用户在线状态
        /// </summary>
        public static int OnLineState = 0;


        #region 客户端的配置信息部分
        
        /// <summary>
        /// 当前用户名
        /// </summary>
        public static string CurrentUserName = string.Empty;

        /// <summary>
        /// 当前密码
        /// </summary>
        public static string CurrentPassword = string.Empty;

        /// <summary>
        /// 登录是否保存密码，默认能记住密码会好用一些
        /// </summary>
        public static bool RememberPassword = true;

        /// <summary>
        /// 是否自动登录，默认不自动登录会好一些
        /// </summary>
        public static bool AutoLogOn = false;

        /// <summary>
        /// 客户端加密存储密码，这个应该是要加密才可以
        /// </summary>
        public static bool ClientEncryptPassword = true;

        /// <summary>
        /// 远程调用Service用户名（为了提高软件的安全性）
        /// </summary>
        public static string ServiceUserName = "bop";

        /// <summary>
        /// 远程调用Service密码（为了提高软件的安全性）
        /// </summary>
        public static string ServicePassword = "bop";
        
        /// <summary>
        /// 默认加载所有用户用户量特别大时的优化配置项目，默认是小规模用户
        /// </summary>
        public static bool LoadAllUser = true;

        /// <summary>
        /// 动态加载组织机构树，当数据量非常庞大时用的参数，默认是小规模用户
        /// </summary>
        public static bool OrganizeDynamicLoading = true;

        /// <summary>
        /// 是否采用多语言
        /// </summary>
        public static bool MultiLanguage = false;

        /// <summary>
        /// 当前客户选择的语言
        /// </summary>
        public static string CurrentLanguage = "zh-CN";

        /// <summary>
        /// 当前语言
        /// </summary>
        public static string Themes = string.Empty;

        private int lockWaitMinute = 60;

        /// <summary>
        /// 锁定等待时间分钟
        /// </summary>
        public int LockWaitMinute
        {
            get { return lockWaitMinute; }
            set { lockWaitMinute = value; }
        }

        /// <summary>
        /// 即时通信指向的网站地址
        /// </summary>
        public static string WebHostUrl = "WebHostUrl";

        #endregion
        
        #region 服务器端的配置信息

        /// <summary>
        /// 允许新用户注册
        /// </summary>
        public static bool AllowUserRegister = true;

        /// <summary>
        /// 是否启用即时内部消息
        /// </summary>
        public static bool UseMessage = false;

        /// <summary>
        /// 是否启用表格数据权限
        /// 启用分级管理范围权限设置，启用逐级授权
        /// </summary>
        public static bool UsePermissionScope = false;

        /// <summary>
        /// 启用按用户权限
        /// </summary>
        public static bool UseUserPermission = true;

        /// <summary>
        /// 启用模块菜单权限
        /// </summary>
        public static bool UseModulePermission = false;

        /// <summary>
        /// 启用操作权限
        /// </summary>
        public static bool UsePermissionItem = false;

        /// <summary>
        /// 启用数据表的约束条件限制
        /// </summary>
        public static bool UseTableScopePermission = false;

        /// <summary>
        /// 启用数据表的列权限
        /// </summary>
        public static bool UseTableColumnPermission = true;

        /// <summary>
        /// 设置手写签名
        /// </summary>
        public static bool HandwrittenSignature = true;

        /// <summary>
        /// 登录历史纪录
        /// </summary>
        public static bool RecordLogOnLog = true;

        /// <summary>
        /// 是否进行日志记录
        /// </summary>
        public static bool RecordLog = true;

        /// <summary>
        /// 是否更新访问日期信息
        /// </summary>
        public static bool UpdateVisit = true;

        /// <summary>
        /// 同时在线用户数量限制
        /// </summary>
        public static int OnLineLimit = 0;

        /// <summary>
        /// 是否检查用户IP地址
        /// </summary>
        public static bool CheckIPAddress = false;        

        /// <summary>
        /// 是否登记异常
        /// </summary>
        public static bool LogException = true;

        /// <summary>
        /// 是否登记数据库操作
        /// </summary>
        public static bool LogSQL = false;

        /// <summary>
        /// 是否登记到 Windows 系统异常中
        /// </summary>
        public static bool EventLog = false;

        #endregion

        #region 服务器端安全设置

        public static bool CheckPasswordStrength = false;

        /// <summary>
        /// 服务器端加密存储密码
        /// </summary>
        public static bool ServerEncryptPassword = true;

        /// <summary>
        /// 密码最小长度
        /// </summary>
        public static int PasswordMiniLength = 6;

        /// <summary>
        /// 必须字母+数字组合
        /// </summary>
        public static bool NumericCharacters = true; 

        /// <summary>
        /// 密码修改周期(月)
        /// </summary>
        public static int PasswordChangeCycle = 3;

        /// <summary>
        /// 禁止用户重复登录
        /// 只允许登录一次
        /// </summary>
        public static bool CheckOnLine = false;

        /// <summary>
        /// 用户名最小长度
        /// </summary>
        public static int AccountMinimumLength = 4;

        /// <summary>
        /// 密码错误锁定次数
        /// </summary>
        public static int PasswordErrowLockLimit = 5;

        /// <summary>
        /// 密码错误锁定周期(分钟)
        /// </summary>
        public static int PasswordErrowLockCycle = 30;

        #endregion

        #region 数据库连接相关配置

        /// <summary>
        /// 业务数据库类别
        /// </summary>
        public static CurrentDbType BusinessDbType = CurrentDbType.SqlServer;

        /// <summary>
        /// 用户数据库类别
        /// </summary>
        public static CurrentDbType UserCenterDbType = CurrentDbType.SqlServer;

        /// <summary>
        /// 工作流数据库类别
        /// </summary>
        public static CurrentDbType WorkFlowDbType = CurrentDbType.SqlServer;

        /// <summary>
        /// 是否加数据库连接
        /// </summary>
        public static bool EncryptDbConnection = false;

        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string userCenterDbConnection = string.Empty;
        public static string UserCenterDbConnection
        {
            get
            {
                if (String.IsNullOrEmpty(userCenterDbConnection)) 
                {
                    BaseConfiguration.GetSetting();
                }
                return userCenterDbConnection;
            }
            set
            {
                userCenterDbConnection = value;
            }
        }

        /// <summary>
        /// 数据库连接的字符串
        /// </summary>
        public static string UserCenterDbConnectionString = string.Empty;

        /// <summary>
        /// 业务数据库
        /// </summary>
        public static string businessDbConnection = string.Empty;
        public static string BusinessDbConnection
        {
            get
            {
                if (String.IsNullOrEmpty(businessDbConnection))
                {
                    BaseConfiguration.GetSetting();
                }
                return businessDbConnection;
            }
            set
            {
                businessDbConnection = value;
            }
        }

        /// <summary>
        /// 业务数据库（连接串，可能是加密的）
        /// </summary>
        public static string BusinessDbConnectionString = string.Empty;

        /// <summary>
        /// 工作流数据库
        /// </summary>
        public static string workFlowDbConnection = string.Empty;
        public static string WorkFlowDbConnection
        {
            get
            {
                if (String.IsNullOrEmpty(workFlowDbConnection))
                {
                    BaseConfiguration.GetSetting();
                }
                return workFlowDbConnection;
            }
            set
            {
                workFlowDbConnection = value;
            }
        }

        /// <summary>
        /// 工作流数据库（连接串，可能是加密的）
        /// </summary>
        public static string WorkFlowDbConnectionString = string.Empty;

        #endregion
        
        #region 系统性的参数配置
        
        /// <summary>
        /// 软件是否需要注册
        /// </summary>
        public static bool NeedRegister = false;   //需要检查注册时，改为true     

        /// <summary>
        /// 检查周期5分钟内不在线的，就认为是已经没在线了，生命周期检查
        /// </summary>
        public static int OnLineTime0ut = 5*60 +20;

        /// <summary>
        /// 每过1分钟，检查一次在线状态
        /// </summary>
        public static int OnLineCheck = 60;

        /// <summary>
        /// 注册码
        /// </summary>
        public static string RegisterKey = string.Empty;

        /// <summary>
        /// 当前网站的安装地址
        /// </summary>
        public static string StartupPath = string.Empty;

        /// <summary>
        /// 是否区分大小写
        /// </summary>
        public static bool MatchCase = true;

        /// <summary>
        /// 最多获取数据的行数限制
        /// </summary>
        public static int TopLimit = 200;

        /// <summary>
        /// 锁不住记录时的循环次数
        /// </summary>
        public static int LockNoWaitCount = 5;

        /// <summary>
        /// 锁不住记录时的等待时间
        /// </summary>
        public static int LockNoWaitTickMilliSeconds = 30;

        /// <summary>
        /// 是否采用服务器端缓存
        /// </summary>
        public static bool ServerCache = false;

        /// <summary>
        /// 最后更新日期
        /// </summary>
        public static string LastUpdate = "2011.10.07";

        /// <summary>
        /// 当前版本
        /// </summary>
        public static string Version = "3.7";

        /// <summary>
        /// 每个操作是否进行信息提示。
        /// </summary>
        public static bool ShowInformation = true;

        /// <summary>
        /// 时间格式
        /// </summary>
        public static string TimeFormat = "HH:mm:ss";

        /// <summary>
        /// 日期短格式
        /// </summary>
        public static string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// 日期长格式
        /// </summary>
        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 当前软件Id
        /// </summary>
        public static string SoftName = string.Empty;

        /// <summary>
        /// 软件的名称
        /// </summary>
        public static string SoftFullName = string.Empty;

        /// <summary>
        /// 根菜单编号
        /// </summary>
        public static string RootMenuCode = string.Empty;

        /// <summary>
        /// 是否采用客户端缓存
        /// </summary>
        public static bool ClientCache = false;

        /// <summary>
        /// 当前客户公司名称
        /// </summary>
        public static string CustomerCompanyName = string.Empty;
        
        /// <summary>
        /// 系统图标文件
        /// </summary>
        public static string AppIco = "Resource\\Form.ico";

        /// <summary>
        /// RegistryKey、Configuration、UserConfig 注册表或者配置文件读取参数
        /// </summary>
        public static ConfigurationCategory ConfigurationFrom = ConfigurationCategory.UserConfig; //确认从自定义配置文件读取

        public static string RegisterException = "请您联系： Bop 手机：136 电子邮件：bop@mail.com 对软件进行注册。";

        public static string CustomerPhone = "0532-12345678";			    // 当前客户公司电话
        public static string CompanyName = "bop";	// 公司名称
        public static string CompanyPhone = "12345678";			    // 公司电话

        public static string Copyright = "Copyright 2012 Bop Tech";
        public static string BugFeedback = @"mailto:fankui@Hotmail.com?subject=系统反馈&body=这里输入您宝贵的反馈";
        public static string IEDownloadUrl = @"http://download.microsoft.com/download/ie6sp1/finrel/6_sp1/W98NT42KMeXP/CN/ie6setup.exe";

        public static string HelpNamespace = string.Empty;
        public static string UploadDirectory = "Document/";
        #endregion
        
        #region 客户端动态加载程序相关
        
        /// <summary>
        /// 主应用程序集名
        /// </summary>
        public static string MainAssembly = string.Empty;
        public static string MainForm = "MainForm";

        public static string LogOnAssembly = "DotNet.WinForm";
        public static string LogOnForm = "FrmLogOnByName";

        public static string ServiceFactory = "ServiceFactory";
        public static string Service = "DotNet.Business";
        // public static string DbHelperClass = "DotNet.Utilities.SqlHelper";
        public static string DbHelperAssmely = "DotNet.Utilities";

        #endregion
        
        #region 系统邮件错误报告反馈相关

        /// <summary>
        /// 发送给谁，用,;符号隔开
        /// </summary>
        public static string ErrorReportTo = "abc@mail.com";

        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public static string ErrorReportMailServer = "";            
   
        /// <summary>
        /// 用户名
        /// </summary>
        public static string ErrorReportMailUserName = "";          

        /// <summary>
        /// 密码
        /// </summary>
        public static string ErrorReportMailPassword = "";          

        #endregion


        private static BaseUserInfo userInfo = null;

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public static BaseUserInfo UserInfo
        {
            get
            {
                if (userInfo == null)
                {
                    userInfo = new BaseUserInfo();
                    // IP地址
                    if (String.IsNullOrEmpty(userInfo.IPAddress))
                    {
                        userInfo.IPAddress = MachineInfo.GetIPAddress();
                    }
                    //Mac地址  add by zgl
                    if (String.IsNullOrEmpty(userInfo.MACAddress))
                    {
                        userInfo.MACAddress = MachineInfo.GetMacAddress();
                    }

                    // 主键
                    if (String.IsNullOrEmpty(userInfo.Id))
                    {
                        userInfo.Id = MachineInfo.GetIPAddress();
                    }
                    // 用户名
                    if (String.IsNullOrEmpty(userInfo.UserName))
                    {
                        userInfo.UserName = System.Environment.MachineName;
                    }
                    // 真实姓名
                    if (String.IsNullOrEmpty(userInfo.RealName))
                    {
                        userInfo.RealName = System.Environment.UserName;
                    }
                    userInfo.ServiceUserName = BaseSystemInfo.ServiceUserName;
                    userInfo.ServicePassword = BaseSystemInfo.ServicePassword;
                }
                return userInfo;
            }
            set
            {
                userInfo = value;
            }
        }

        /// <summary>
        /// C/S程序保存登录信息部分
        /// </summary>
        /// <param name="userInfo">用户</param>
        public static void LogOn(BaseUserInfo userInfo)
        {
            BaseSystemInfo.UserInfo = userInfo;
        }
        
        /// <summary>
        /// 验证用户是否是授权的用户
        /// 不是任何人都可以调用服务的，将来这里还可以进行扩展的
        /// 例如用IP地址限制等等
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <returns>验证成功</returns>
        public static bool IsAuthorized(BaseUserInfo userInfo)
        {
            if (userInfo == null)
            {
                return false;
            }
            // 若系统设置的用户名是空的，那就不用判断了
            if (string.IsNullOrEmpty(ServiceUserName))
            {
                return true;
            }
            // 若系统设置的用密码是空的，那就不用判断了
            if (string.IsNullOrEmpty(ServicePassword))
            {
                return true;
            }
            // 调用服务器的用户名、密码都对了，才可以调用服务程序，否则认为是非授权的操作
            if (ServiceUserName.Equals(userInfo.ServiceUserName) && ServicePassword.Equals(userInfo.ServicePassword))
            {
                return true;
            }
            return false;
        }
    }
}