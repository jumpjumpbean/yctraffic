//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------
/// 
/// 修改纪录
///		2007.05.17 版本：1.0	JiRiGaLa 建立，为了提高效率分开建立了类。
///	
/// 版本：3.1
///

using System;
using System.Globalization;

namespace DotNet.Utilities
{
    /// <summary>
    ///	AppMessage
    /// 通用讯息控制基类
    /// </summary>
    public class AppMessage
    {
        /// <summary>
        /// 提示信息
        /// </summary>
        public static string MSG0000 = "提示信息";

        /// <summary>
        /// 发生未知错误。
        /// </summary>
        public static string MSG0001 = "发生未知错误。";

        /// <summary>
        /// 数据库连接不正常。
        /// </summary>
        public static string MSG0002 = "数据库连接不正常。";

        /// <summary>
        /// WebService连接不正常。
        /// </summary>
        public static string MSG0003 = "WebService连接不正常。";

        /// <summary>
        /// 无任何数据被修改。
        /// </summary>
        public static string MSG0004 = "无任何数据被修改。";

        /// <summary>
        /// 记录未找到，可能已被其他人删除。
        /// </summary>
        public static string MSG0005 = "记录未找到，可能已被其他人删除。";

        /// <summary>
        /// 数据已被其他人修改，请按F5键重新更新取得数据。
        /// </summary>
        public static string MSG0006 = "数据已被其他人修改，请按F5键重新更新取得数据。";

        /// <summary>
        /// 请输入{0}，不允许为空。
        /// </summary>
        public static string MSG0007 = "请输入 {0} ，不允许为空。";

        /// <summary>
        /// {0} 已重复。
        /// </summary>
        public static string MSG0008 = "{0} 已重复。";

        /// <summary>
        /// 新增成功。
        /// </summary>
        public static string MSG0009 = "新增成功。";

        /// <summary>
        /// 更新成功。
        /// </summary>
        public static string MSG0010 = "更新成功。";

        /// <summary>
        /// 保存成功。
        /// </summary>
        public static string MSG0011 = "保存成功。";

        /// <summary>
        /// 批量储存成功。
        /// </summary>
        public static string MSG0012 = "批量保存成功。";

        /// <summary>
        /// 删除成功。
        /// </summary>
        public static string MSG0013 = "删除成功。";

        /// <summary>
        /// 批量删除成功。
        /// </summary>
        public static string MSG0014 = "批量删除成功。";

        /// <summary>
        /// 您确认删除吗？
        /// </summary>
        public static string MSG0015 = "您确认删除吗？";

        /// <summary>
        /// 您确认删除 '{0}' 吗？
        /// </summary>
        public static string MSG0016 = "您确认删除 {0} 吗？";

        /// <summary>
        /// 当前记录不允许被删除。
        /// </summary>
        public static string MSG0017 = "当前记录不允许被删除。";

        /// <summary>
        /// 当前记录 '{0}' 不允许被删除。
        /// </summary>
        public static string MSG0018 = "当前记录 {0} 不允许被删除。";

        /// <summary>
        /// 当前记录不允许被编辑,请按F5键重新获得最新资料。
        /// </summary>
        public static string MSG0019 = "当前记录不允许被编辑，请按F5键重新获得最新资料。";

        /// <summary>
        /// 当前记录 '{0}' 不允许被编辑，请按F5键重新获得最新资料。
        /// </summary>
        public static string MSG0020 = "当前记录 {0} 不允许被编辑，请按F5键重新获得最新资料。";

        /// <summary>
        /// 当前记录已是第一笔记录。
        /// </summary>
        public static string MSG0021 = "当前记录已是第一笔记录。";

        /// <summary>
        /// 当前记录已是最后一笔记录。
        /// </summary>
        public static string MSG0022 = "当前记录已是最后一笔记录。";

        /// <summary>
        /// 请至少选择一项。
        /// </summary>
        public static string MSGC023 = "请至少选择一项。";

        /// <summary>
        /// 只能选择一笔数据。
        /// </summary>
        public static string MSGC024 = "只能选择一笔数据。";

        /// <summary>
        /// 请至少选择一项 '{0}'。
        /// </summary>
        public static string MSG0024 = "请至少选择一项 {0}。";

        /// <summary>
        /// '{0}' 不能大于 '{1}'。
        /// </summary>
        public static string MSG0025 = "{0} 不能大于 {1}。";

        /// <summary>
        /// '{0}' 不能小于 '{1}'。
        /// </summary>
        public static string MSG0026 = "{0} 不能小于 {1}。";

        /// <summary>
        /// '{0}' 不能等于 '{1}'。
        /// </summary>
        public static string MSG0027 = "{0} 不能等于 {1}。";

        /// <summary>
        ///'{0}' 不是有效的日期。
        /// </summary>
        public static string MSG0028 = "{0} 不是有效的日期。";

        /// <summary>
        /// '{0}' 不是有效的字符。
        /// </summary>
        public static string MSG0029 = "{0} 不是有效的字符。";

        /// <summary>
        /// '{0}' 不是有效的数字。
        /// </summary>
        public static string MSG0030 = "{0} 不是有效的数字。";

        /// <summary>
        /// '{0}' 不是有效的金额。
        /// </summary>
        public static string MSG0031 = "{0} 不是有效的金额。";

        /// <summary>
        /// '{0}'名不能包含：\ / : * ? " < > |
        /// </summary>
        public static string MSG0032 = "{0} 名包含非法字符。";

        /// <summary>
        /// 数据已经被引用，有关联资料在。
        /// </summary>
        public static string MSG0033 = "资料已经被引用，有关联资料在。";

        /// <summary>
        /// 数据已经被引用，有关联资料在，是否强制删除资料？
        /// </summary>
        public static string MSG0034 = "资料已经被引用，有关联资料在，是否强制删除资料？";

        /// <summary>
        /// {0} 有子节点不允许被删除，有子节点还未被删除。
        /// </summary>
        public static string MSG0035 = "{0} 有子节点不允许被删除，有子节点还未被删除。";

        /// <summary>
        /// {0} 不能移动到 {1}。
        /// </summary>
        public static string MSG0036 = "{0} 不能移动到 {1}。";

        /// <summary>
        /// {0} 下的子节点不能移动到 {1}。
        /// </summary>
        public static string MSG0037 = "{0} 下的子节点不能移动到 {1}。";

        /// <summary>
        /// 确认移动 {0} 到 {1} 吗？
        /// </summary>
        public static string MSG0038 = "确认移动 {0} 到 {1} 吗？";

        /// <summary>
        /// '{0}' 不等于 '{1}'。
        /// </summary>
        public static string MSG0039 = "{0} 不等于 {1}。";

        /// <summary>
        /// {0} 错误。
        /// </summary>
        public static string MSG0040 = "{0} 错误。";

        /// <summary>
        /// 确认审核通过吗？
        /// </summary>
        public static string MSG0041 = "确认审核通过吗？";

        /// <summary>
        /// 确认审核退回吗？
        /// </summary>
        public static string MSG0042 = "确认审核退回吗？";

        /// <summary>
        /// 不能锁定数据。
        /// </summary>
        public static string MSG0043 = "不能锁定数据。";

        /// <summary>
        /// 成功锁定数据。
        /// </summary>
        public static string MSG0044 = "成功锁定数据。";

        /// <summary>
        /// 数据已经改变，想储存数据吗？
        /// </summary>
        public static string MSG0045 = "数据已经改变，想储存数据吗？";

        /// <summary>
        /// 最近 {0} 次内密码不能重复。。
        /// </summary>
        public static string MSG0046 = "最近 {0} 次内密码不能重复。。";

        /// <summary>
        /// 密码已过期，账号被锁定，请联系系统管理员。
        /// </summary>
        public static string MSG0047 = "密码已过期，账号被锁定，请联系系统管理员。";

        /// <summary>
        /// 拒绝登入，用户已经在在线。
        /// </summary>
        public static string MSG0048 = "拒绝登入，用户已经在在线。";

        /// <summary>
        /// 拒绝登入，网卡Mac位置不符合限制条件。
        /// </summary>
        public static string MSG0049 = "拒绝登入，网卡Mac位置不符合限制条件。";

        /// <summary>
        /// 拒绝登入，IP位置不符限制条件。
        /// </summary>
        public static string MSG0050 = "拒绝登入，IP位置不符限制条件。";

        /// <summary>
        /// 已到在线用户最大数量限制。
        /// </summary>
        public static string MSG0051 = "已到在线用户最大数量限制。";

        /// <summary>
        /// IP位置格式不正确。
        /// </summary>
        public static string MSG0052 = "IP位置格式不正确。";

        /// <summary>
        /// MAC位置格式不正确。
        /// </summary>
        public static string MSG0053 = "MAC位置格式不正确。";

        /// <summary>
        /// 请填写IP位置或MAC位置信息。
        /// </summary>
        public static string MSG0054 = "请填写IP位置或MAC位置信息。";

        /// <summary>
        /// 存在相同的IP位置。
        /// </summary>
        public static string MSG0055 = "存在相同的IP位置。";

        /// <summary>
        /// IP位置新增成功。
        /// </summary>
        public static string MSG0056 = "IP位置新增成功。";

        /// <summary>
        /// IP位置新增失败。
        /// </summary>
        public static string MSG0057 = "IP位置新增失败。";

        /// <summary>
        /// 存在相同的MAC位置。
        /// </summary>
        public static string MSG0058 = "存在相同的MAC位置。";

        /// <summary>
        /// MAC位置新增成功。
        /// </summary>
        public static string MSG0059 = "  MAC位置新增成功。";

        /// <summary>
        /// 请先新增该职员的登入系统用户信息。
        /// </summary>
        public static string MSG0060 = "请先新增该职员的登入系统用户信息。";

        /// <summary>
        /// MAC位置新增失败。
        /// </summary>
        public static string MSG0061 = "  MAC位置新增失败。";

        /// <summary>
        /// 请设定新密码，原始密码未曾修改过。
        /// </summary>
        public static string MSG0062 = "请设定新密码，原始密码未曾修改过。";

        /// <summary>
        /// 请设定新密码，30天内未曾修改过密码。
        /// </summary>
        public static string MSG0063 = "请设定新密码，30天内未曾修改过密码。";

        /// <summary>
        /// 您输入的分钟数值不正确，请检查。
        /// </summary>
        public static string MSG0064 = "您输入的分钟数值不正确，请检查。";

        /// <summary>
        /// 数据已经改变，不储存数据？
        /// </summary>
        public static string MSG0065 = "数据已经改变，不储存数据？";

        /// <summary>
        /// 您确认移除吗？
        /// </summary>
        public static string MSG0075 = "您确认移除吗？";

        /// <summary>
        /// 您确认移除 {0} 吗？
        /// </summary>
        public static string MSG0076 = "您确认移除 {0} 吗？";

        /// <summary>
        /// 成功删除 {0} 笔记录。
        /// </summary>
        public static string MSG0077 = "成功删除 {0} 笔记录。";

        /// <summary>
        /// 用户登入被拒，用户审核中。
        /// </summary>
        public static string MSG0078 = "用户登入被拒，用户审核中。";

        /// <summary>
        /// 用户被锁定，登入被拒绝，请联系系统管理员。
        /// </summary>
        public static string MSG0079 = "用户被锁定，登入被拒绝，请联系系统管理员。";

        /// <summary>
        /// 用户账号未被激活，请及时激活用户账号。
        /// </summary>
        public static string MSG0080 = "用户账号未被激活，请及时激活用户账号。";

        /// <summary>
        /// 用户被锁定，登入被拒绝，不可早于：
        /// </summary>
        public static string MSG0081 = "用户被锁定，登入被拒绝，不可早于：";

        /// <summary>
        /// 用户被锁定，登入被拒绝，不可晚于：
        /// </summary>
        public static string MSG0082 = "用户被锁定，登入被拒绝，不可晚于：";

        /// <summary>
        /// 用户被锁定，登入被拒绝，锁定开始日期：
        /// </summary>
        public static string MSG0083 = "用户被锁定，登入被拒绝，锁定开始日期：";

        /// <summary>
        /// 用户被锁定，登入被拒绝，锁定结束日期：
        /// </summary>
        public static string MSG0084 = "用户被锁定，登入被拒绝，锁定结束日期：";

        /// <summary>
        /// IP Address 不正确。
        /// </summary>
        public static string MSG0085 = "IP Address 不正确。";

        /// <summary>
        /// MAC Address 不正确。
        /// </summary>
        public static string MSG0086 = "MAC Address 不正确。";

        /// <summary>
        /// 用户已经上线，不允许重复登入。
        /// </summary>
        public static string MSG0087 = "用户已经上线，不允许重复登入。";

        /// <summary>
        /// 密码错误，登入被拒绝。
        /// </summary>
        public static string MSG0088 = "密码错误，登入被拒绝。";

        /// <summary>
        /// 已超出用户在线数量上限：
        /// </summary>
        public static string MSG0089 = "已超出用户在线数量上限：";

        /// <summary>
        /// 登入被拒绝。
        /// </summary>
        public static string MSG0090 = "登入被拒绝。";

        /// <summary>
        /// 新增操作权限项。
        /// </summary>
        public static string MSG0091 = "新增操作权限项。";

        /// <summary>
        /// 登入开始时间
        /// </summary>
        public static string MSG0092 = "登入开始时间";

        /// <summary>
        /// 登入结束时间
        /// </summary>
        public static string MSG0093 = "登入结束时间";

        /// <summary>
        /// 暂停开始时间
        /// </summary>
        public static string MSG0094 = "暂停开始时间";

        /// <summary>
        /// 暂停结束日期
        /// </summary>
        public static string MSG0095 = "暂停结束日期";

        /// <summary>
        /// {0} 在在线，不允许删除。
        /// </summary>
        public static string MSG0100 = "{0} 在在线，不允许删除。";

        /// <summary>
        /// 目前用户 {0} 不允许删除自己。
        /// </summary>
        public static string MSG0101 = "目前用户 {0} 不允许删除自己。";

        /// 您确认清除权限吗？
        /// </summary>
        public static string MSG0600 = "您确认清除权限吗？";

        /// <summary>
        /// 已经成功连接到目标数据。
        /// </summary>
        public static string MSG0700 = "已经成功连接到目标数据。";

        /// <summary>
        /// 调用被拒绝，未经授权的访问。
        /// </summary>
        public static string MSG0800 = "调用被拒绝，未经授权的访问。";

        /// <summary>
        /// 服务调用被拒绝，用户未登入。
        /// </summary>
        public static string MSG0900 = "服务调用被拒绝，用户未登入。";

        /// <summary>
        /// 系统设定讯息错误，请与软件开发商联系。
        /// </summary>
        public static string MSG1000 = "系统设定讯息错误，请与软件开发商联系。";

        /// <summary>
        /// 您确认重置功能选单吗？
        /// </summary>
        public static string MSG1001 = "您确认重置功能选单吗？";

        /// <summary>
        /// {0} 不正确，请重新输入。
        /// </summary>
        public static string MSG2000 = "{0} 不正确，请重新输入。";

        public static string MSG3000 = "您确认初始化系统吗？";
        public static string MSG3010 = "操作成功。";
        public static string MSG3020 = "操作失败。";

        public static string MSG9800 = "值";
        public static string MSG9900 = "公司";
        public static string MSG9901 = "部门";

        public static string MSG9910 = "用户未设置电子邮件地址。";
        public static string MSG9911 = "用户账号被锁定。";
        public static string MSG9912 = "用户还未激活账号。";
        public static string MSG9913 = "用户账号已被激活。";
        public static string MSG9914 = "用户关联。";
        public static string MSG9915 = "请设置约束条件。";
        public static string MSG9916 = "显示   ▽";
        public static string MSG9917 = "隐藏   △";
        public static string MSG9918 = "验证表达式成功。";
        public static string MSG9919 = "请输入条件。";
        public static string MSG9920 = "请输入内容。";
        public static string MSG9921 = "缺少（ 符号。";
        public static string MSG9922 = "缺少 ）符号。";

        public static string MSGS857 = "签名私钥。";
        public static string MSGS864 = "签名密码。";

        public static string MSGS957 = "通讯用户名称。";
        public static string MSGS964 = "通讯密码。";

        public static string MSGS965 = "验证码";


        public static string MSG8000 = "密码强度不符合要求，密码至少为6位数，且为数字加字母的组合。";

        public static string MSG8900 = "工号"; 
        public static string MSG9000 = "用户名称或密码错误。";
        public static string MSG9955 = "资料。";
        public static string MSG9956 = "未找到满足条件的记录。";
        public static string MSG9957 = "用户名";
        public static string MSG9958 = "数据验证错误。";
        public static string MSG9959 = "新密码";
        public static string MSG9960 = "确认密码";
        public static string MSG9961 = "原密码";
        public static string MSG9962 = "修改 {0} 成功。";
        public static string MSG9963 = "设置 {0} 成功。";
        public static string MSG9964 = "密码";
        public static string MSG9965 = "执行成功。";
        public static string MSG9966 = "用户没有找到，请注意大小写。";
        public static string MSG9967 = "密码错误，请注意大小写。";
        public static string MSG9968 = "登入被拒绝，请与管理员联系。";
        public static string MSG9969 = "基础编码";
        public static string MSG9970 = "职员";
        public static string MSG9971 = "组织机构";
        public static string MSG9972 = "角色";
        public static string MSG9973 = "选单";
        public static string MSG9974 = "文件夹";
        public static string MSG9975 = "权限";
        public static string MSG9976 = "主键";
        public static string MSG9977 = "编号";
        public static string MSG9978 = "名称";
        public static string MSG9979 = "父节点主键";
        public static string MSG9980 = "父节点名称";
        public static string MSG9981 = "功能分类主键";
        public static string MSG9982 = "唯一识别主键";
        public static string MSG9983 = "主题";
        public static string MSG9984 = "内容";
        public static string MSG9985 = "状态代码";
        public static string MSG9986 = "次数";
        public static string MSG9987 = "有效";
        public static string MSG9988 = "描述";
        public static string MSG9989 = "排序码";
        public static string MSG9990 = "建立者主键";
        public static string MSG9991 = "建立时间";
        public static string MSG9992 = "最后修改者主键";
        public static string MSG9993 = "最后修改时间";
        public static string MSG9994 = "排序";
        public static string MSG9995 = "主键";
        public static string MSG9996 = "索引";
        public static string MSG9997 = "字段";
        public static string MSG9998 = "数据表";
        public static string MSG9999 = "数据库";

        //韩峰修改20101106
        public static string MSG0200 = "您确认清除用户角色关联吗？";
        public static string MSG0201 = "您选择的档案不存在，请重新选择。";
        public static string MSG0202 = "提示信息";
        public static string MSG0203 = "您确认移动 \" {0} \" 到 \" {1} \" 吗？";
        public static string MSG0204 = "您确认退出应用程序吗？";
        public static string MSG0205 = "档案 {0} 已存在，要覆盖服务器上的档案吗？";
        public static string MSG0206 = "已经超过了上传限制，请检查要上传的档案大小。";
        public static string MSG0207 = "您确认要删除图片吗？";
        public static string MSG0208 = "开始时间不能大于结束时间。";
        public static string MSG0209 = "清除成功。";
        public static string MSG0210 = "重置成功。";
        public static string MSG0211 = "已输入 {0} 次错误密码，不再允许继续登入，请重新启动程序进行登入。";
        public static string MSG0212 = "查询内容";
        public static string MSG0213 = "编号总长度不要超过40位。";
        public static string MSG0214 = "编号产生成功。";
        public static string MSG0215 = "增序列";
        public static string MSG0216 = "减序列";
        public static string MSG0217 = "步调";
        public static string MSG0218 = "序列重置成功。";
        public static string MSG0219 = "您确认重置序列吗？";
        public static string MSG0223 = "用户名称不允许为空，请输入。";
        public static string MSG0225 = "目前节点上有资料。";
        public static string MSG0226 = "无法删除自己。";
        public static string MSG0228 = "设置关联用户成功。";
        public static string MSG0229 = "所在单位不允许为空，请选择。";
        public static string MSG0230 = "申请账号更新成功，请等待审核。";
        public static string MSG0231 = "密码不等于确认密码，请确认后重新输入。";
        public static string MSG0232 = "用户名称";
        public static string MSG0233 = "姓名";
        public static string MSG0234 = "E-mail 格式不正确，请重新输入。";
        public static string MSG0235 = "申请账号成功，请等待审核。";
        public static string MSG0236 = "导出的目标文件已存在，要覆盖 \"{0}\" 吗？";
        public static string MSG0237 = "发送电子邮件成功。";
        public static string MSG0238 = "清除异常信息成功。";
        public static string MSG0239 = "您确认清除异常信息吗？";
        public static string MSG0240 = "内容不能为空";
        public static string MSG0241 = "发送电子邮件失败。";
        public static string MSG0242 = "移动成功。";
        public static string MSG0243 = "程序异常报告";
        public static string MSG0244 = "您选择的文档不存在，请重新选择。";
        public static string MSG0245 = "用户、角色必须选择一个。";
        public static string MSG0246 = "没有要复制的数据！";
        public static string MSG0247 = "审核通过 {0} 项。";
        public static string MSG0248 = "审核通过失败。";
        public static string MSG0249 = "请选需要处理的数据。";
        public static string MSG0250 = "您确认审核通过此单据吗？";
        public static string MSG0251 = "成功退回单据。";
        public static string MSG0252 = "请选需要处理的数据。";
        public static string MSG0253 = "您确认不输入退回理由吗？";
        public static string MSG0255 = "您确认退回此单据吗？";
        public static string MSG0256 = "工作流程发送成功。";
        public static string MSG0257 = "工作流程发送失败。";
        public static string MSG0258 = "审核通过单据。";
        public static string MSG0259 = "请选需要处理的数据。";
        public static string MSG0260 = "请选择提交给哪位用户。";
        public static string MSG0261 = "最终审核通过 {0} 项。";
        public static string MSG0262 = "最终审核失败。";
        public static string MSG0263 = "请选需要处理的数据。";
        public static string MSG0264 = "成功退回单据。";
        public static string MSG0265 = "请选择需要处理的数据。";
        public static string MSG0266 = "您确认不输入退回理由吗？";
        public static string MSG0267 = "您确认退回此单据吗？";
        public static string MSG0268 = "审核流程发送成功。";
        public static string MSG0269 = "请选需要处理的数据。";
        public static string MSG0270 = "请选择提交给哪位用户。";
        public static string MSG0271 = "您确认提交给 {0} 吗？";
        public static string MSG0272 = "审核流程撤销成功 {0} 项。";
        public static string MSG0273 = "审核流程撤销失败。";
        public static string MSG0274 = "请选需要处理的数据。";
        public static string MSG0275 = "您确认不输入退回理由吗？";
        public static string MSG0276 = "您确认撤销撤销审核流程中的单据吗？";
        public static string MSG0277 = "请选择提交给哪个用户审核。";
        public static string MSG0278 = "您确认提交给用户 {0} 审核吗？";
        public static string MSG0279 = "工作流程发送成功。";
        public static string MSG0280 = "工作流程发送失败。";
        public static string MSG0281 = "您确认替换文件 {0} 吗？";
        public static string MSG0282 = "上级机构";
        public static string MSG0283 = "编号产生成功";
        public static string MSG0284 = "已修改配置信息，需要保存吗？";
        public static string MSG0285 = "没有要保存的资料！";
        public static string MSG0286 = "单位名称";
        public static string MSG0300 = "下线通知，您的账号在另一地点登录，您被迫下线。";

        // 重新登入时，登入窗口名称改变为重新登入”
        public static string MSGReLogOn = "重新登入";

        // BaseUserManager登入服务讯息参数
        public static string BaseUserManager = "登入服务";
        public static string BaseUserManager_LogOn = "登入操作";
        public static string BaseUserManager_LogOnSuccess = "登入成功";

        // ExceptionService异常纪录服务及相关的方法名称定义
        public static string ExceptionService = "异常纪录服务";
        public static string ExceptionService_GetDT = "取得异常列表";
        public static string ExceptionService_Delete = "删除异常";
        public static string ExceptionService_BatchDelete = "批量删除异常";
        public static string ExceptionService_Truncate = "清除全部异常";

        // FileService档案服务及相关的方法名称定义
        public static string FileService = "档案服务";
        public static string FileService_GetEntity = "取得实体";
        public static string FileService_Exists = "判断是否存在";
        public static string FileService_Download = "下载文件";
        public static string FileService_Upload = "上传档案";
        public static string FileService_GetDTByFolder = "依文件夹取得档案列表";
        public static string FileService_DeleteByFolder = "依文件夹删除档案";

        // FolderService文件夹服务及相关的方法名称定义
        public static string FolderService = "文件夹服务";

        // ItemDetailsService选项明细管理服务及相关的方法名称定义
        public static string ItemDetailsService = "选项明细管理服务";
        public static string ItemDetailsService_GetDT = "取得列表";
        public static string ItemDetailsService_GetDTByParent = "依父节点取得列表";
        public static string ItemDetailsService_GetDTByCode = "依编号取得列表";
        public static string ItemDetailsService_GetDSByCodes = "批量取得资料";
        public static string ItemDetailsService_GetEntity = "取得实体";
        public static string ItemDetailsService_Add = "新增实体";
        public static string ItemDetailsService_Update = "更新实体";
        public static string ItemDetailsService_Delete = "删除实体";
        public static string ItemDetailsService_SetDeleted = "批量标示删除标志";
        public static string ItemDetailsService_BatchMoveTo = "批量移动";
        public static string ItemDetailsService_BatchDelete = "批量删除";
        public static string ItemDetailsService_BatchSave = "批量储存";

        // ItemsService选项管理服务及相关的方法名称定义
        public static string ItemsService = "选项管理服务";
        public static string ItemsService_GetDT = "取得列表";
        public static string ItemsService_GetEntity = "取得实体";
        public static string ItemsService_GetDTByParent = "依父节点取得列表";
        public static string ItemsService_Add = "新增实体";
        public static string ItemsService_Update = "更新实体";
        public static string ItemsService_CreateTable = "新增数据表";
        public static string ItemsService_Delete = "删除实体";
        public static string ItemsService_BatchMoveTo = "批量移动";
        public static string ItemsService_BatchDelete = "批量删除";
        public static string ItemsService_BatchSave = "批量储存";

        // LogOnService登入服务及相关的方法名称定义
        public static string LogOnService = "登入服务";
        public static string LogOnService_GetUserDT = "取得用户列表";
        public static string LogOnService_GetStaffUserDT = "取得内部员工列表";
        public static string LogOnService_OnLine = "用户在线报导";
        public static string LogOnService_OnExit = "用户退出应用程序";
        public static string LogOnService_SetPassword = "设定用户密码";
        public static string LogOnService_ChangePassword = "用户变更密码";
        public static string LogOnService_ChangeCommunicationPassword = "用户变更通讯密码";
        public static string LogOnService_CommunicationPassword = "验证用户通讯密码";
        public static string LogOnService_CreateDigitalSignature = "建立数字证书签名";
        public static string LogOnService_GetPublicKey = "取得当前用户的公钥";
        public static string LogOnService_ChangeSignedPassword = "用户变更签名密码";
        public static string LogOnService_SignedPassword = "验证用户数字签名密码";

        // LogService日志服务及相关的方法名称定义
        public static string LogService = "日志服务";
        public static string LogService_GetLogGeneral = "取得用户访问情况日志";
        public static string LogService_ResetVisitInfo = "重置用户访问情况";
        public static string LogService_GetDTByDate = "依日期取得日志";
        public static string LogService_GetDTByModule = "依选单取得日志";
        public static string LogService_GetDTByUser = "依用户取得日志";
        public static string LogService_Delete = "删除日志";
        public static string LogService_BatchDelete = "批量删除日志";
        public static string LogService_Truncate = "清除全部日志";
        public static string LogService_GetDTApplicationByDate = "依日期取得日志(商务)";
        public static string LogService_BatchDeleteApplication = "批量删除日志(商务)";
        public static string LogService_TruncateApplication = "清除全部日志(商务)";

        // MessageService讯息服务及相关的方法名称定义
        public static string MessageService = "讯息服务";
        public static string MessageService_GetInnerOrganize = "取得内部组织机构";
        public static string MessageService_GetUserDTByDepartment = "按部门取得用户信息";
        public static string MessageService_BatchSend = "批量发送站内讯息";
        public static string MessageService_Send = "发送讯息";
        public static string MessageService_MessageChek = "取得讯息状态";
        public static string MessageService_ReadFromReceiver = "取得特定用户的新讯息";
        public static string MessageService_Read = "阅读讯息";

        // ModuleService选单服务及相关的方法名称定义
        public static string ModuleService = "选单服务";
        public static string ModuleService_GetDT = "取得列表";
        public static string ModuleService_GetEntity = "取得实体";
        public static string ModuleService_GetFullNameByCode = "透过编号取得选单名称";
        public static string ModuleService_Add = "新增选单";
        public static string ModuleService_Update = "更新选单";
        public static string ModuleService_GetDTByParent = "依父节点取得列表";
        public static string ModuleService_Delete = "删除选单";
        public static string ModuleService_BatchDelete = "批量删除";
        public static string ModuleService_SetDeleted = "批量标示删除标志";
        public static string ModuleService_MoveTo = "移动选单";
        public static string ModuleService_BatchMoveTo = "批量移动";
        public static string ModuleService_BatchSave = "批量储存";
        public static string ModuleService_SetSortCode = "储存排序顺序";
        public static string ModuleService_GetPermissionDT = "取得关联的权限项列表";
        public static string ModuleService_GetIdsByPermission = "依操作权限项取得关联的选单主键";

        public static string ModuleService_BatchAddPermissions = "选单批量新增关联操作权限项";
        public static string ModuleService_BatchAddModules = "新增操作权限项关联选单";
        public static string ModuleService_BatchDletePermissions = "删除选单与操作权限项的关联";
        public static string ModuleService_BatchDleteModules = "删除操作权限项与选单的关联";

        // OrganizeService组织机构服务及相关的方法名称定义
        public static string OrganizeService = "组织机构服务";
        public static string OrganizeService_Add = "新增实体";
        public static string OrganizeService_AddByDetail = "依明细情况新增实体";
        public static string OrganizeService_GetEntity = "取得实体";
        public static string OrganizeService_GetDT = "取得列表";
        public static string OrganizeService_GetDTByParent = "依父节点取得列表";
        public static string OrganizeService_GetInnerOrganizeDT = "取得内部组织机构";
        public static string OrganizeService_GetCompanyDT = "取得公司列表";
        public static string OrganizeService_GetDepartmentDT = "取得部门列表";
        public static string OrganizeService_Search = "查询组织机构";
        public static string OrganizeService_Update = "更新组织机构";
        public static string OrganizeService_Delete = "删除组织机构";
        public static string OrganizeService_BatchDelete = "批量删除";
        public static string OrganizeService_SetDeleted = "批量标示删除标志";
        public static string OrganizeService_BatchSave = "批量储存";
        public static string OrganizeService_MoveTo = "移动组织机构";
        public static string OrganizeService_BatchMoveTo = "批量移动";
        public static string OrganizeService_BatchSetCode = "批量重新产生编号";
        public static string OrganizeService_BatchSetSortCode = "批量重新产生排序码";

        // ParameterService参数服务及相关的方法名称定义
        public static string ParameterService = "参数服务";
        public static string ParameterService_GetParameter = "取得参数值";
        public static string ParameterService_SetParameter = "设置参数值";
        public static string ParameterService_GetDTByParameter = "取得列表";
        public static string ParameterService_GetDTParameterCode = "依编号取得列表";
        public static string ParameterService_DeleteByParameter = "删除参数";
        public static string ParameterService_DeleteByParameterCode = "依参数编号删除";
        public static string ParameterService_Delete = "删除参数";
        public static string ParameterService_BatchDelete = "批量删除参数";

        // PermissionItemService操作权限项定义服务及相关的方法名称定义
        public static string PermissionItemService = "操作权限项定义服务";
        public static string PermissionItemService_Add = "新增操作权限项";
        public static string PermissionItemService_GetDT = "取得列表";
        public static string PermissionItemService_GetDTByParent = "依父节点取得列表";
        public static string PermissionItemService_GetLicensedDT = "取得授权范围";
        public static string PermissionItemService_GetEntity = "取得实体";
        public static string PermissionItemService_GetEntityByCode = "依编号取得实体";
        public static string PermissionItemService_Update = "更新实体";
        public static string PermissionItemService_MoveTo = "移动实体";
        public static string PermissionItemService_BatchMoveTo = "批量移动实体";
        public static string PermissionItemService_Delete = "删除实体";
        public static string PermissionItemService_BatchDelete = "批量删除实体";
        public static string PermissionItemService_SetDeleted = "批量标示删除标志";
        public static string PermissionItemService_BatchSave = "批量储存";
        public static string PermissionItemService_BatchSetSortCode = "重新产生排序码";
        public static string PermissionItemService_GetIdsByModule = "按模块主键获取操作权限项主键数组";

        // PermissionService权限判断服务及相关的方法名称定义
        public static string PermissionService = "权限判断服务";
        public static string PermissionService_IsInRole = "用户是否在指定的角色中";
        public static string PermissionService_IsAuthorizedByUser = "该用户是否有相应的操作权限";
        public static string PermissionService_IsAuthorizedByRole = "该角色是否有相应的操作权限";
        public static string PermissionService_IsAdministratorByUser = "该用户是否为超级管理员";
        public static string PermissionService_GetPermissionDTByUser = "取得该用户的所有权限列表";
        public static string PermissionService_IsModuleAuthorized = "当前用户是否对某个选单有相应的权限";
        public static string PermissionService_IsModuleAuthorizedByUser = "该用户是否对某个选单有相应的权限";
        public static string PermissionService_GetUserPermissionScope = "取得用户的数据权限范围";
        public static string PermissionService_GetOrganizeDTByPermission = "依某个权限域取得组织机构列表";
        public static string PermissionService_GetOrganizeIdsByPermission = "依某个数据权限取得组织机构主键数组";
        public static string PermissionService_GetRoleDTByPermission = "依某个权限域取得角色列表";
        public static string PermissionService_GetRoleIdsByPermission = "按权限取得角色数组列表";
        public static string PermissionService_GetUserDTByPermission = "依某个权限域取得用户列表";
        public static string PermissionService_GetUserIdsByPermission = "依某个数据权限取得用户主键数组";
        public static string PermissionService_GetModuleDTByPermission = "依某个权限域取得选单列表";
        public static string PermissionService_GetPermissionItemDTByPermission = "用户的所有可授权范围(有授权权限的权限列表)";
        public static string PermissionService_GetRolePermissionItemIds = "取得角色权限主键数组";
        public static string PermissionService_GrantRolePermissions = "授予角色的权限";
        public static string PermissionService_GrantRolePermissionById = "授予角色的权限";
        public static string PermissionService_RevokeRolePermissions = "删除角色的权限";
        public static string PermissionService_ClearRolePermission = "清除角色权限";
        public static string PermissionService_RevokeRolePermissionById = "删除角色的权限";
        public static string PermissionService_GetRoleScopeUserIds = "取得角色的某个权限域的组织范围";
        public static string PermissionService_GetRoleScopeRoleIds = "取得角色的某个权限域的组织范围";
        public static string PermissionService_GetRoleScopeOrganizeIds = "取得角色的某个权限域的组织范围";
        public static string PermissionService_GrantRoleUserScopes = "授予角色的某个权限域的组织范围";
        public static string PermissionService_RevokeRoleUserScopes = "删除角色的某个权限域的组织范围";
        public static string PermissionService_GrantRoleRoleScopes = "授予角色的某个权限域的组织范围";
        public static string PermissionService_RevokeRoleRoleScopes = "删除角色的某个权限域的组织范围";
        public static string PermissionService_GrantRoleOrganizeScopes = "授予角色的某个权限域的组织范围";
        public static string PermissionService_RevokeRoleOrganizeScopes = "删除角色的某个权限域的组织范围";
        public static string PermissionService_GetRoleScopePermissionItemIds = "取得角色授权权限列表";
        public static string PermissionService_GrantRolePermissionItemScopes = "授予角色的授权权限范围";
        public static string PermissionService_RevokeRolePermissionItemScopes = "授予角色的授权权限范围";
        public static string PermissionService_ClearRolePermissionScope = "清除角色权限范围";
        public static string PermissionService_GetUserPermissionItemIds = "取得用户权力主键数组";
        public static string PermissionService_GrantUserPermissions = "授予用户操作权限";
        public static string PermissionService_GrantUserPermissionById = "授予用户操作权限";
        public static string PermissionService_RevokeUserPermission = "删除用户操作权限";
        public static string PermissionService_RevokeUserPermissionById = "删除用户操作权限";
        public static string PermissionService_GetUserScopeOrganizeIds = "取得用户的某个权限域的组织范围";
        public static string PermissionService_GrantUserOrganizeScopes = "设置用户的某个权限域的组织范围";
        public static string PermissionService_RevokeUserOrganizeScopes = "设置用户的某个权限域的组织范围";
        public static string PermissionService_GetUserScopeUserIds = "取得用户的某个权限域的组织范围";
        public static string PermissionService_GrantUserUserScopes = "设置用户的某个权限域的组织范围";
        public static string PermissionService_RevokeUserUserScopes = "设置用户的某个权限域的用户范围";
        public static string PermissionService_GetUserScopeRoleIds = "取得用户的某个权限域的用户范围";
        public static string PermissionService_GrantUserRoleScopes = "设置用户的某个权限域的用户范围";
        public static string PermissionService_RevokeUserRoleScopes = "设置用户的某个权限域的用户范围";
        public static string PermissionService_GetUserScopePermissionItemIds = "取得用户授权权限列表";
        public static string PermissionService_GrantUserPermissionItemScopes = "授予用户的授权权限范围";
        public static string PermissionService_RevokeUserPermissionItemScopes = "删除用户的授权权限范围";
        public static string PermissionService_ClearUserPermission = "清除用户权力";
        public static string PermissionService_ClearUserPermissionScope = "清除用户权力范围";
        public static string PermissionService_GetModuleDTByUser = "取得用户有访问权限的选单";
        public static string PermissionService_GetUserScopeModuleIds = "取得用户选单权限范围主键数组";
        public static string PermissionService_GrantUserModuleScopes = "授予用户选单的权限范围";
        public static string PermissionService_GrantUserModuleScope = "授予用户选单的权限范围";
        public static string PermissionService_RevokeUserModuleScope = "删除用户选单的权限范围";
        public static string PermissionService_RevokeUserModuleScopes = "删除用户选单的权限范围";
        public static string PermissionService_GetRoleScopeModuleIds = "取得用户选单权限范围主键数组";
        public static string PermissionService_GrantRoleModuleScopes = "授予用户选单的权限范围";
        public static string PermissionService_GrantRoleModuleScope = "授予用户选单的权限范围";
        public static string PermissionService_RevokeRoleModuleScopes = "删除用户选单的权限范围";
        public static string PermissionService_RevokeRoleModuleScope = "删除用户选单的权限范围";
        public static string PermissionService_GetResourcePermissionItemIds = "取得资源权限主键数组";
        public static string PermissionService_GrantResourcePermission = "授予资源的权限";
        public static string PermissionService_RevokeResourcePermission = "删除资源的权限";
        public static string PermissionService_GetPermissionScopeTargetIds = "取得资源权限范围主键数组";
        public static string PermissionService_RevokePermissionScopeTargets = "删除资源的权限范围";
        public static string PermissionService_ClearPermissionScopeTarget = "删除资源的权限范围";
        public static string PermissionService_GetResourceScopeIds = "取得用户的某个资源的权限范围";
        public static string PermissionService_GetTreeResourceScopeIds = "取得用户的某个资源的权限范围(树型资源)";

        // RoleService角色管理服务及相关的方法名称定义
        public static string RoleService = "角色管理服务";
        public static string RoleService_Add = "新增角色";
        public static string RoleService_GetDT = "取得列表";
        public static string RoleService_GetDTByOrganize = "依组织机构取得角色列表";
        public static string RoleService_GetEntity = "取得实体";
        public static string RoleService_Update = "更新实体";
        public static string RoleService_GetDTByIds = "依主键数组取得角色列表";
        public static string RoleService_Search = "查询角色列表";
        public static string RoleService_BatchSave = "批量储存角色";
        public static string RoleService_MoveTo = "移动角色数据";
        public static string RoleService_BatchMoveTo = "批量移动角色数据";
        public static string RoleService_ResetSortCode = "排序角色顺序";
        public static string RoleService_GetRoleUserIds = "取得角色中的用户主键";
        public static string RoleService_AddUserToRole = "用户新增至角色";
        public static string RoleService_RemoveUserFromRole = "将用户从角色中移除";
        public static string RoleService_Delete = "删除角色";
        public static string RoleService_BatchDelete = "批量删除角色";
        public static string RoleService_SetDeleted = "批量标示删除标志";
        public static string RoleService_ClearRoleUser = "清除角色用户关联";

        // SequenceService序列管理服务及相关的方法名称定义
        public static string SequenceService = "序列管理服务";
        public static string SequenceService_Add = "新增序列";
        public static string SequenceService_GetDT = "取得列表";
        public static string SequenceService_GetEntity = "取得实体";
        public static string SequenceService_Update = "更新序列";
        public static string SequenceService_Reset = "批量重置序列";
        public static string SequenceService_Delete = "删除序列";
        public static string SequenceService_BatchDelete = "批量删除序列";

        // StaffService职员管理服务及相关的方法名称定义
        public static string StaffService = "职员管理服务";
        public static string StaffService_GetAddressDT = "取得内部通讯簿";
        public static string StaffService_GetAddressPageDT = "取得内部通讯簿";
        public static string StaffService_UpdateAddress = "更新通讯地址";
        public static string StaffService_BatchUpdateAddress = "批量更新通讯地址";
        public static string StaffService_AddStaff = "新增职员";
        public static string StaffService_UpdateStaff = "更新职员";
        public static string StaffService_GetDT = "取得列表";
        public static string StaffService_GetEntity = "取得实体";
        public static string StaffService_GetDTByIds = "取得职员列表";
        public static string StaffService_GetDTByDepartment = "依部门取得列表";
        public static string StaffService_GetDTByOrganize = "依公司取得列表";
        public static string StaffService_SetStaffUser = "职员关联用户";

        // TableColumnsService表字段权限服务及相关的方法名称定义
        public static string TableColumnsService = "表字段权限服务";
        public static string TableColumnsService_GetDTByTable = "依表明取得字段列表";
        public static string TableColumnsService_GetConstraintDT = "取得约束条件(所有的约束)";
        public static string TableColumnsService_GetUserConstraint = "取得当前用户的约束条件";
        public static string TableColumnsService_SetConstraint = "设置约束条件";
        public static string TableColumnsService_BatchDeleteConstraint = "批量删除";

        // UserService用户管理服务及相关的方法名称定义
        public static string UserService = "用户管理服务";
        public static string UserService_Check = "请审核。";
        public static string UserService_Application = " 申请用户：";
        public static string UserService_AddUser = "新增用户";
        public static string UserService_GetDTByDepartment = "依部门取得用户列表";
        public static string UserService_GetEntity = "取得实体";
        public static string UserService_GetDT = "取得列表";
        public static string UserService_GetDTByRole = "依角色取得列表";
        public static string UserService_GetDTByIds = "依主键取得列表";
        public static string UserService_GetRoleDT = "取得用户的角色列表";
        public static string UserService_UserInRole = "判断用户是否在某个角色中";
        public static string UserService_UpdateUser = "更新用户";
        public static string UserService_Search = "查询用户";
        public static string UserService_SetUserAuditStates = "设置用户审核状态";
        public static string UserService_SetDefaultRole = "设置用户的预设角色";

        // WorkFlowActivityAdminService工作流程定义服务及相关的方法名称定义
        public static string WorkFlowActivityAdminService = "工作流程定义服务";

        // WorkFlowCurrentService当前工作流程服务
        public static string WorkFlowCurrentService = "当前工作流程服务";

        // WorkFlowProcessAdminService工作流程处理过程服务
        public static string WorkFlowProcessAdminService = "工作流程处理过程服务";

        // BillTemplateService单据模板服务
        public static string BillTemplateService = "单据模板服务";

        // DataGridView右键选单
        public static string clickToolStripMenuItem = "数据列设置";

        // BaseForm加载窗口
        public static string LoadWindow = "加载窗口";

        #region public static int GetLanguageResource() 从当前指定的语系包读取信息
        /// <summary>
        /// 从当前指定的语系包读取信息，用了反射循环遍历
        /// </summary>
        /// <returns></returns>
        public static int GetLanguageResource()
        {
            AppMessage appMessage = new AppMessage();
            return BaseBusinessLogic.GetLanguageResource(appMessage);
        }
        #endregion

        #region public static string Format(string value, params string[] messages) 格式化一个资源字符串
        /// <summary>
        /// 格式化一个资源字符串
        /// </summary>
        /// <param name="value">目标字符串</param>
        /// <param name="messages">传入的信息</param>
        /// <returns>字符串</returns>
        public static string Format(string value, params string[] messages)
        {
            return String.Format(CultureInfo.CurrentCulture, value, messages);
        }
        #endregion

        #region public static string GetMessage(string id) 读取一个资源定义
        /// <summary>
        /// 读取一个资源定义
        /// </summary>
        /// <param name="id">资源主键</param>
        /// <returns>字符串</returns>
        public static string GetMessage(string id)
        {
            string returnValue = string.Empty;
            returnValue = ResourceManagerWrapper.Instance.Get(id);
            return returnValue;
        }
        #endregion

        #region public static string GetMessage(string id, params string[] messages)
        /// <summary>
        /// 读取一个资源定义
        /// </summary>
        /// <param name="id">资源主键</param>
        /// <param name="messages">传入的信息</param>
        /// <returns>字符串</returns>
        public static string GetMessage(string id, params string[] messages)
        {
            string returnValue = string.Empty;
            returnValue = ResourceManagerWrapper.Instance.Get(id);
            returnValue = String.Format(CultureInfo.CurrentCulture, returnValue, messages);
            return returnValue;
        }
        #endregion
    }
}

