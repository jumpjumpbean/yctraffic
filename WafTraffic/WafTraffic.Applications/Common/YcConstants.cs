using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WafTraffic.Applications.Common
{
    public class YcConstants
    {
        // 逻辑删除
        public const int INT_DB_DATA_DELETED = 1;
        public const int INT_DB_DATA_AVAILABLE = 0;

        // 权限
        public const string STR_AUTH_LOGBOOK_LISTALL = "yctraffic.SquadronLogbook.ListAll";
        public const string STR_AUTH_LOGBOOK_LISTCHARGEDEPTS = "yctraffic.SquadronLogbook.ListChargeDepts";
        public const string STR_AUTH_LOGBOOK_ADD = "yctraffic.SquadronLogbook.Add";
        public const string STR_AUTH_LOGBOOK_DELETE = "yctraffic.SquadronLogbook.Delete";
        public const string STR_AUTH_LOGBOOK_MODIFY = "yctraffic.SquadronLogbook.Modify";
        public const string STR_AUTH_LOGBOOK_BROWSE = "yctraffic.SquadronLogbook.Browse";
        public const string STR_AUTH_LOGBOOK_DEAL = "yctraffic.SquadronLogbook.Deal";
        public const string STR_AUTH_LOGBOOK_VERIFY = "yctraffic.SquadronLogbook.Verify";
        public const string STR_AUTH_FDDZW_ZGXCK = "yctraffic.FddZw.Zgxck";
        public const string STR_AUTH_FDDZW_KJSSK = "yctraffic.FddZw.Kjssk";
        public const string STR_AUTH_FDDZW_SGK = "yctraffic.FddZw.Sgk";
        public const string STR_AUTH_FDDZW_CGS = "yctraffic.FddZw.Cgs";
        public const string STR_AUTH_FDDZW_FZXFK = "yctraffic.FddZw.Fzxfk";
        public const string STR_AUTH_FDDZW_ZHZX = "yctraffic.FddZw.Zhzx";
        public const string STR_AUTH_FDDZW_ZXK = "yctraffic.FddZw.Zxk";
        public const string STR_AUTH_FDDZW_1ZD = "yctraffic.FddZw.1zd";
        public const string STR_AUTH_FDDZW_2ZD = "yctraffic.FddZw.2zd";
        public const string STR_AUTH_FDDZW_3ZD = "yctraffic.FddZw.3zd";
        public const string STR_AUTH_FDDZW_4ZD = "yctraffic.FddZw.4zd";
        public const string STR_AUTH_FDDZW_5ZD = "yctraffic.FddZw.5zd";
        public const string STR_AUTH_FDDZW_6ZD = "yctraffic.FddZw.6zd";
        public const string STR_AUTH_FDDZW_7ZD = "yctraffic.FddZw.7zd";
        public const string STR_AUTH_ZHZX_REQUEST_LISTALL = "yctraffic.Zhzx.Request.ListAll";
        public const string STR_AUTH_ZHZX_REQUEST_BROWSE = "yctraffic.Zhzx.Request.Browse";
        public const string STR_AUTH_ZHZX_REQUEST_ADD = "yctraffic.Zhzx.Request.Add";
        public const string STR_AUTH_ZHZX_REQUEST_MODIFY = "yctraffic.Zhzx.Request.Modify";
        public const string STR_AUTH_ZHZX_REQUEST_DELETE = "yctraffic.Zhzx.Request.Delete";
        public const string STR_AUTH_ZHZX_REQUEST_DEAL = "yctraffic.Zhzx.Request.Deal";
        public const string STR_AUTH_FZK_BROWSE = "yctraffic.Fzk.Browse";
        public const string STR_AUTH_FZK_ADD = "yctraffic.Fzk.Add";
        public const string STR_AUTH_FZK_MODIFY = "yctraffic.Fzk.Modify";
        public const string STR_AUTH_FZK_DELETE = "yctraffic.Fzk.Delete";

        public const string STR_AUTH_CGS_BROWSE = "yctraffic.Cgs.Browse";
        public const string STR_AUTH_CGS_ADD = "yctraffic.Cgs.Add";
        public const string STR_AUTH_CGS_MODIFY = "yctraffic.Cgs.Modify";
        public const string STR_AUTH_CGS_DELETE = "yctraffic.Cgs.Delete";

        public const string STR_AUTH_SGK_BROWSE = "yctraffic.Sgk.Browse";
        public const string STR_AUTH_SGK_ADD = "yctraffic.Sgk.Add";
        public const string STR_AUTH_SGK_MODIFY = "yctraffic.Sgk.Modify";
        public const string STR_AUTH_SGK_DELETE = "yctraffic.Sgk.Delete";

        public const string STR_AUTH_ZXK_BROWSE = "yctraffic.Zxk.Browse";
        public const string STR_AUTH_ZXK_ADD = "yctraffic.Zxk.Add";
        public const string STR_AUTH_ZXK_MODIFY = "yctraffic.Zxk.Modify";
        public const string STR_AUTH_ZXK_DELETE = "yctraffic.Zxk.Delete";

        public const string STR_AUTH_QBYQ_BROWSE = "yctraffic.Qbyq.Browse";
        public const string STR_AUTH_QBYQ_ADD = "yctraffic.Qbyq.Add";
        public const string STR_AUTH_QBYQ_MODIFY = "yctraffic.Qbyq.Modify";
        public const string STR_AUTH_QBYQ_DELETE = "yctraffic.Qbyq.Delete";

        public const string STR_AUTH_KJK_REQUEST_LISTALL = "yctraffic.Kjk.Request.ListAll";
        public const string STR_AUTH_KJK_REQUEST_BROWSE = "yctraffic.Kjk.Request.Browse";
        public const string STR_AUTH_KJK_REQUEST_ADD = "yctraffic.Kjk.Request.Add";
        public const string STR_AUTH_KJK_REQUEST_MODIFY = "yctraffic.Kjk.Request.Modify";
        public const string STR_AUTH_KJK_REQUEST_DELETE = "yctraffic.Kjk.Request.Delete";
        public const string STR_AUTH_KJK_REQUEST_DEAL = "yctraffic.Kjk.Request.Deal";

        public const string STR_AUTH_SSK_REQUEST_LISTALL = "yctraffic.Ssk.Request.ListAll";
        public const string STR_AUTH_SSK_REQUEST_BROWSE = "yctraffic.Ssk.Request.Browse";
        public const string STR_AUTH_SSK_REQUEST_ADD = "yctraffic.Ssk.Request.Add";
        public const string STR_AUTH_SSK_REQUEST_MODIFY = "yctraffic.Ssk.Request.Modify";
        public const string STR_AUTH_SSK_REQUEST_DELETE = "yctraffic.Ssk.Request.Delete";
        public const string STR_AUTH_SSK_REQUEST_DEAL = "yctraffic.Ssk.Request.Deal";


        public const string STR_AUTH_CYPUNISH_GATHER = "yctraffic.SquadronLogbook.CyPunish.Gather";

        // 部门
        public const int INT_DEPT_ID_ZGXCK = 100000012;
        public const int INT_DEPT_ID_KJSSK = 100000018;
        public const int INT_DEPT_ID_SGK = 100000019;
        public const int INT_DEPT_ID_CGS = 100000020;
        public const int INT_DEPT_ID_FZXFK = 100000022;
        public const int INT_DEPT_ID_ZHZX = 100000021;
        public const int INT_DEPT_ID_ZXK = 100000017;
        public const int INT_DEPT_ID_1ZD = 100000013;
        public const int INT_DEPT_ID_2ZD = 100000014;
        public const int INT_DEPT_ID_3ZD = 100000023;
        public const int INT_DEPT_ID_4ZD = 100000024;
        public const int INT_DEPT_ID_5ZD = 100000025;
        public const int INT_DEPT_ID_6ZD = 100000026;
        public const int INT_DEPT_ID_7ZD = 100000027;
        public const int INT_DEPT_ID_DDLD = 100000028;
        public const int INT_COMPANY_ID = 100000011;

        // 角色
        public const int INT_ROLE_DDZ_ID = 10000047;
        public const int INT_ROLE_FDD_ID = 10000052;
        public const int INT_ROLE_ZW_ID = 10000053;
        public const int INT_ROLE_ADMIN_ID = 10000000;
        public const int INT_ROLE_KJSS_ZR = 10000055;  // 科技设施科主任
        public const int INT_ROLE_KJSS_KY = 10000062;  // 科技设施科科员

        public const int INT_ROLE_FDD_KJSS = 10000076; //分管科技设施副大队
        public const int INT_ROLE_FDD_SGK = 10000077; //分管事故科副大队
        public const int INT_ROLE_FDD_CGFZ = 10000078; //分管车管法治副大队
        public const int INT_ROLE_FDD_ZHZX = 10000079; //分管指挥中心副大队
        public const int INT_ROLE_FDD_ZXK = 10000080; //分管秩序科副大队
        public const int INT_ROLE_FDD_QT = 10000052; //分管其他副大队

        // 错误消息
        public const string STR_ERROR_MSG_BLANK_VALUE = "必须输入项不能为空";

        #region 中队台账

        //常用台账：隐患排查状态
        public const int INT_DANGER_DEAL_SATUS_NEW = 1;
        public const int INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE = 2;
        public const int INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY = 3;
        public const int INT_DANGER_DEAL_SATUS_END = 4;

        public const string STR_DANGER_DEAL_SATUS_NEW = "等待整改项";
        public const string STR_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE = "等待分管领导审批";
        public const string STR_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY = "等待签收";
        public const string STR_DANGER_DEAL_SATUS_END = "已完成";

        // 隐患排查台账类型
        public const int INT_DANGER_DEAL_TYPE_BEFORE = 0;
        public const int INT_DANGER_DEAL_TYPE_AFTER = 1;

        // 台账配置
        public const int INT_LBCONFIG_NODE_LEVEL_1 = 1;
        public const int INT_LBCONFIG_NODE_LEVEL_2 = 2;
        public const int INT_LBCONFIG_TYPE_NORMAL = 0;
        public const int INT_LBCONFIG_TYPE_FREQUENT = 1;
        public const string STR_LBCONFIG_CODE_START = "1000";

        #endregion

        #region 指挥中心

        // 流程状态
        public const int INT_ZHZX_STATUS_WAITING_FOR_UPLOAD = 1;

        public const int INT_ZHZX_REQSTAT_NULL = 0;
        public const int INT_ZHZX_REQSTAT_SUB_LEADER_APPROVE = 1;
        public const int INT_ZHZX_REQSTAT_DDZ_APPROVE = 2;  
        public const int INT_ZHZX_REQSTAT_BGS_EXECUTE = 3;
        public const int INT_ZHZX_REQSTAT_ZHZX_EXECUTE = 4;
        public const int INT_ZHZX_REQSTAT_COMPLETED = 5;
        /* 2015/3/9 客户办公用品申请流程变更，暂时保留原流程处理
        public const int INT_ZHZX_REQSTAT_ZHZX_FIRST_CHECK = 1;
        public const int INT_ZHZX_REQSTAT_DDZ_APPROVE = 2;
        public const int INT_ZHZX_REQSTAT_ZHZX_APPROVE = 3;
        public const int INT_ZHZX_REQSTAT_REQDEPT_APPROVE = 4;
        public const int INT_ZHZX_REQSTAT_DDZ_SUPERVISE = 5;
        //public const int INT_ZHZX_REQSTAT_ZHZX_EXECUTE = 5;
        //public const int INT_ZHZX_REQSTAT_REQDEPT_VERIFY = 6;
        public const int INT_ZHZX_REQSTAT_COMPLETED = 7;
        public const int INT_ZHZX_REQSTAT_ZHZX_REJECTED = 8;
        //public const int INT_ZHZX_REQSTAT_DDZ_REJECTED = 9;
        //public const int INT_ZHZX_REQSTAT_REQDEPT_REJECTED = 10;
        */

        public const string STR_ZHZX_REQSTAT_NULL = "申请人未提交";
        public const string STR_ZHZX_REQSTAT_SUB_LEADER_APPROVE = "分管副大队长审核";
        public const string STR_ZHZX_REQSTAT_DDZ_APPROVE = "大队长审核";
        public const string STR_ZHZX_REQSTAT_BGS_EXECUTE = "办公室执行";
        public const string STR_ZHZX_REQSTAT_ZHZX_EXECUTE = "指挥中心签收";
        public const string STR_ZHZX_REQSTAT_COMPLETED = "已完成";
        /* 2015/3/9 客户办公用品申请流程变更，暂时保留原流程处理
        public const string STR_ZHZX_REQSTAT_ZHZX_FIRST_CHECK = "指挥中心初审";
        public const string STR_ZHZX_REQSTAT_DDZ_APPROVE = "大队长审批";
        public const string STR_ZHZX_REQSTAT_ZHZX_APPROVE = "指挥中心审核";
        public const string STR_ZHZX_REQSTAT_REQDEPT_APPROVE = "申请单位审核";
        public const string STR_ZHZX_REQSTAT_DDZ_SUPERVISE = "大队长督办";
        //public const string STR_ZHZX_REQSTAT_ZHZX_EXECUTE = "指挥中心执行";
        //public const string STR_ZHZX_REQSTAT_REQDEPT_VERIFY = "申请单位复查";
        public const string STR_ZHZX_REQSTAT_COMPLETED = "已完成";
        public const string STR_ZHZX_REQSTAT_ZHZX_REJECTED = "指挥中心退回";
        //public const string STR_ZHZX_REQSTAT_DDZ_REJECTED = "大队长退回";
        //public const string STR_ZHZX_REQSTAT_REQDEPT_REJECTED = "申请单位退回";
        */

        // 申请类型
        public const int INT_ZHZX_REQTYPE_ADD = 1;
        public const int INT_ZHZX_REQTYPE_MAINTAIN = 2;
        public const int INT_ZHZX_REQTYPE_APPLY = 3;
        public const string STR_ZHZX_REQTYPE_ADD = "添加";
        public const string STR_ZHZX_REQTYPE_MAINTAIN = "维修";
        public const string STR_ZHZX_REQTYPE_APPLY = "申领";

        // 天网违章Excel
        public const int INT_ZHZX_ROW_TITLE = 1;

        public const int INT_ZHZX_COL_CHECK_POINT_NAME = 0;
        public const int INT_ZHZX_COL_CAPTURE_LOCATION = 1;
        public const int INT_ZHZX_COL_LICENSE_PLATE_NUMBER = 2;
        public const int INT_ZHZX_COL_OWNERSHIP_OF_LAND = 3;
        public const int INT_ZHZX_COL_SPEED = 4;
        public const int INT_ZHZX_COL_VIOLATION_TYPE = 5;
        public const int INT_ZHZX_COL_VEHICLE_TYPE = 6;
        public const int INT_ZHZX_COL_LICENSE_PLATE_COLOR = 7;
        public const int INT_ZHZX_COL_VEHICLE_COLOR = 8;
        public const int INT_ZHZX_COL_CAPTURE_TIME = 9;
        public const int INT_ZHZX_COL_DATA_STATUS = 10;
        public const int INT_ZHZX_COL_SOURCE_PICTURE_1 = 11;
        public const int INT_ZHZX_COL_SOURCE_PICTURE_2 = 12;
        public const int INT_ZHZX_COL_SOURCE_PICTURE_3 = 13;
        public const int INT_ZHZX_COL_COMPOSED_PICTURE = 17;

        public const string STR_ZHZX_COL_CHECK_POINT_NAME = "卡口名称";
        public const string STR_ZHZX_COL_CAPTURE_LOCATION = "抓拍地点";
        public const string STR_ZHZX_COL_LICENSE_PLATE_NUMBER = "车牌号码";
        public const string STR_ZHZX_COL_OWNERSHIP_OF_LAND = "归属地";
        public const string STR_ZHZX_COL_SPEED = "车速";
        public const string STR_ZHZX_COL_VIOLATION_TYPE = "违章类型";
        public const string STR_ZHZX_COL_VEHICLE_TYPE = "车辆类型";
        public const string STR_ZHZX_COL_LICENSE_PLATE_COLOR = "车牌颜色";
        public const string STR_ZHZX_COL_VEHICLE_COLOR = "车身颜色";
        public const string STR_ZHZX_COL_CAPTURE_TIME = "抓拍时间";
        public const string STR_ZHZX_COL_DATA_STATUS = "状态";

        public const int INT_ZHZX_MAX_PICTURE_COUNT = 30000;

        #endregion

        #region 科技设施

        //科技设施申请流程
        public const int INT_KJSS_REQSTAT_NULL = 0;
        public const int INT_KJSS_REQSTAT_SUB_LEADER_APPROVE = 1;
        public const int INT_KJSS_REQSTAT_DDZ_APPROVE = 2;
        public const int INT_KJSS_REQSTAT_BGS_EXECUTE = 3;
        public const int INT_KJSS_REQSTAT_ZHZX_EXECUTE = 4;
        public const int INT_KJSS_REQSTAT_COMPLETED = 5;

        public const string STR_KJSS_REQSTAT_NULL = "申请人未提交";
        public const string STR_KJSS_REQSTAT_SUB_LEADER_APPROVE = "分管副大队长审核";
        public const string STR_KJSS_REQSTAT_DDZ_APPROVE = "大队长审核";
        public const string STR_KJSS_REQSTAT_BGS_EXECUTE = "办公室执行";
        public const string STR_KJSS_REQSTAT_ZHZX_EXECUTE = "指挥中心签收";
        public const string STR_KJSS_REQSTAT_COMPLETED = "已完成";

        #endregion
    }

    public enum LoadingType
    {
        Content,
        Rectification,
        Review,
        Photo,
        CheckFile,
        VerifyFile,
        View
    }
}
