using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WafTraffic.Domain.Common
{
    public class YcConstantTable
    {
        public static int ORGID_SGK = 100000019; //事故科
        public static int ORGID_KJSSK = 100000018; //科技设施科
        public static int ORGID_BGS = 100000032;  // 办公室

        public static int GROUPID_KSDZ = 10000051;  //科所队长
        public static int GROUPID_VICE = 10000065;  //副大队长和政委

        public static int ROLEID_ZW = 10000053;  //政委
        public static int ROLEID_DDZ = 10000047;  //大队长

        public static int DEPARTID_ZGK = 100000012;  //政工科

        public static int COMPANY_ID = 100000011;  //禹城交警大队

        public static string PRIV_NeedAlarmMonthRegister = "yctraffic.MonthRegister.NeedAlarmMonthRegister";  //月度登记表录入提醒

        //public static string PRIV_LeadZD1 = "yctraffic.MonthRegister.LeadZD1";  //分管一中队
        //public static string DepartCode_ZD1 = "10.20";
        //public static string PRIV_LeadZD2 = "yctraffic.MonthRegister.LeadZD2";  //分管二中队
        //public static string DepartCode_ZD2 = "10.21";
        //public static string PRIV_LeadZD3 = "yctraffic.MonthRegister.LeadZD3";  //分管三中队
        //public static string DepartCode_ZD3 = "10.22";
        //public static string PRIV_LeadZD4 = "yctraffic.MonthRegister.LeadZD4";  //分管四中队
        //public static string DepartCode_ZD4 = "10.23";
        //public static string PRIV_LeadZD5 = "yctraffic.MonthRegister.LeadZD5";  //分管五中队
        //public static string DepartCode_ZD5 = "10.24";
        //public static string PRIV_LeadZD6 = "yctraffic.MonthRegister.LeadZD6";  //分管六中队
        //public static string DepartCode_ZD6 = "10.25";
        //public static string PRIV_LeadZD7 = "yctraffic.MonthRegister.LeadZD7";  //分管七中队
        //public static string DepartCode_ZD7 = "10.26";

        //public static string PRIV_LeadZG = "yctraffic.MonthRegister.LeadZG";  //分管政工科
        //public static string DepartCode_ZG = "10.10";
        //public static string PRIV_LeadSJ = "yctraffic.MonthRegister.LeadSJ";  //分管审计科
        //public static string DepartCode_SJ = "10.11";
        //public static string PRIV_LeadZX = "yctraffic.MonthRegister.LeadZX";  //分管秩序科
        //public static string DepartCode_ZX = "10.12";
        //public static string PRIV_LeadKJ = "yctraffic.MonthRegister.LeadKJ";  //分管科技设施科
        //public static string DepartCode_KJ = "10.13";
        //public static string PRIV_LeadSG = "yctraffic.MonthRegister.LeadSG";  //分管事故科
        //public static string DepartCode_SG = "10.14";
        //public static string PRIV_LeadFZ = "yctraffic.MonthRegister.LeadFZ";  //分管法制信访科
        //public static string DepartCode_FZ = "10.17";
        //public static string PRIV_LeadXX = "yctraffic.MonthRegister.LeadXX";  //分管信息中心
        //public static string DepartCode_XX = "10.90";
        //public static string PRIV_LeadZH = "yctraffic.MonthRegister.LeadZH";  //分管指挥中心
        //public static string DepartCode_ZH = "10.16";


        public static int ZGXC_ID = 10005098;                           //政工宣传module ID
        public static string MaterialDeclare_CODE   = "100.100.104";      //材料申报SubModule Code
        public static string HotLine_CODE           = "100.100.100";      //市长热线SubModule Code
        //public static string MonthRegister_CODE     = "100.100.103";      //月度登记表SubModule Code
        public static string ZgxcAskForLeave_CODE = "100.100.108";          //请销假系统SubModule Code

        public static int DBSX_ID = 10005121;                           //待办事项 module ID
        public static string AlarmNotify_CODE = "111.111.100";          //待办提醒SubModule Code

        public static int KJSSK_ID = 10005118;                           //科技设施科 module ID
        public static string EquipmentDueDate_CODE = "100.106.100";          //设备巡检SubModule Code

        public static int ZDTZ_ID = 10005099;                           //中队台账 module ID
        public static string ZDTZ_CY_CODE = "100.101.100";          //常用台账SubModule Code

        public static int QBYQ_ID = 10005119;                           //情报舆情 module ID
        public static string QbyqCase_CODE = "100.107.100";          //情报舆情SubModule Code

        public static int FZK_ID = 10005116;
        public static string FzkReleaseCar_CODE = "100.104.104";

        #region Zdtz

        // 权限
        public const string STR_AUTH_LOGBOOK_LISTALL = "yctraffic.SquadronLogbook.ListAll";
        public const string STR_AUTH_LOGBOOK_LISTCHARGEDEPTS = "yctraffic.SquadronLogbook.ListChargeDepts";
        public const string STR_AUTH_LOGBOOK_ADD = "yctraffic.SquadronLogbook.Add";
        public const string STR_AUTH_LOGBOOK_DELETE = "yctraffic.SquadronLogbook.Delete";
        public const string STR_AUTH_LOGBOOK_MODIFY = "yctraffic.SquadronLogbook.Modify";
        public const string STR_AUTH_LOGBOOK_BROWSE = "yctraffic.SquadronLogbook.Browse";
        public const string STR_AUTH_LOGBOOK_DEAL = "yctraffic.SquadronLogbook.Deal";
        public const string STR_AUTH_LOGBOOK_VERIFY = "yctraffic.SquadronLogbook.Verify";

        // 状态
        //常用台账：隐患排查状态
        public const int INT_DANGER_DEAL_SATUS_NEW = 1;
        public const int INT_DANGER_DEAL_SATUS_SUB_LEADER_APPROVE = 2;
        public const int INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY = 3;
        public const int INT_DANGER_DEAL_SATUS_END = 4;

        #endregion

        #region Zhzx

        public static int ZHZX_ID = 10005120;                           //指挥中心 module ID
        public static string ZHZX_REQUEST_CODE = "100.108.101";          //办公设备申请SubModule Code

        // 角色
        public static int ROLEID_ZHZX_ZR = 10000058;  // 指挥中心主任
        public static int ROLEID_ZHZX_SCRY = 10000067;  // 指挥中心上传人员
        public static int ROLEID_ZHZX_SHRY = 10000068;  // 指挥中心审核人员

        // 权限
        public const string STR_AUTH_ZHZX_REQUEST_LISTALL = "yctraffic.Zhzx.Request.ListAll";
        public const string STR_AUTH_ZHZX_REQUEST_BROWSE = "yctraffic.Zhzx.Request.Browse";
        public const string STR_AUTH_ZHZX_REQUEST_ADD = "yctraffic.Zhzx.Request.Add";
        public const string STR_AUTH_ZHZX_REQUEST_MODIFY = "yctraffic.Zhzx.Request.Modify";
        public const string STR_AUTH_ZHZX_REQUEST_DELETE = "yctraffic.Zhzx.Request.Delete";
        public const string STR_AUTH_ZHZX_REQUEST_DEAL = "yctraffic.Zhzx.Request.Deal";

        // 状态
        public const int INT_ZHZX_WORKFLOW_ALL = 0;
        public const int INT_ZHZX_WORKFLOW_UPLOAD_PENDING = 1;
        public const int INT_ZHZX_WORKFLOW_APPROVE_PENDING = 2;
        public const int INT_ZHZX_WORKFLOW_UPLOAD_REJECT = 3;
        public const int INT_ZHZX_WORKFLOW_FILTERED = 4;
        public const int INT_ZHZX_WORKFLOW_APPROVE_REJECT = 5;
        public const int INT_ZHZX_WORKFLOW_APPROVED = 6;

        public const string STR_ZHZX_WORKFLOW_ALL = "全部";
        public const string STR_ZHZX_WORKFLOW_UPLOAD_PENDING = "待上传";
        public const string STR_ZHZX_WORKFLOW_APPROVE_PENDING = "待审核";
        public const string STR_ZHZX_WORKFLOW_UPLOAD_REJECT = "上传打回";
        public const string STR_ZHZX_WORKFLOW_FILTERED = "被过滤";
        public const string STR_ZHZX_WORKFLOW_APPROVE_REJECT = "审核打回";
        public const string STR_ZHZX_WORKFLOW_APPROVED = "审核通过";

        // 违章类型
        //public const int INT_ZHZX_VIOTYPE_ALL = 0;
        //public const int INT_ZHZX_VIOTYPE_REDLIGHT = 1;
        //public const int INT_ZHZX_VIOTYPE_WRONGROAD = 2;
        //public const int INT_ZHZX_VIOTYPE_REVERSE = 3;
        //public const int INT_ZHZX_VIOTYPE_YELLOWLINE = 4;
        //public const int INT_ZHZX_VIOTYPE_WHITELINE = 5;
        //public const int INT_ZHZX_VIOTYPE_CHANGEROAD = 6;
        //public const int INT_ZHZX_VIOTYPE_NOBELT = 7;

        public const string STR_ZHZX_VIOTYPE_ALL = "全部";
        public const string STR_ZHZX_VIOTYPE_REDLIGHT = "闯红灯";
        public const string STR_ZHZX_VIOTYPE_WRONGROAD = "不按车道行驶";
        public const string STR_ZHZX_VIOTYPE_REVERSE = "逆行";
        public const string STR_ZHZX_VIOTYPE_YELLOWLINE = "压黄线";
        public const string STR_ZHZX_VIOTYPE_WHITELINE = "压线";
        public const string STR_ZHZX_VIOTYPE_CHANGEROAD = "违章变道";
        public const string STR_ZHZX_VIOTYPE_NOBELT = "不系安全带";

        //办公设备申请流程
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

        #endregion

        #region Kjss

        // 角色
        public static int ROLEID_KJK_ZR = 10000055;  // 科技科主任
        public static int ROLEID_KJK_KY = 10000062;  // 科技科员工

        public static int ROLEID_SSK_ZR = 10000083;  // 设施科主任
        public static int ROLEID_SSK_KY = 10000084;  // 设施科员工

        // 权限
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

        //科技设施申请流程
        public const int INT_KJSS_REQSTAT_NULL = 0;
        public const int INT_KJSS_REQSTAT_SUB_LEADER_APPROVE = 1;
        public const int INT_KJSS_REQSTAT_DDZ_APPROVE = 2;
        public const int INT_KJSS_REQSTAT_BGS_EXECUTE = 3;
        public const int INT_KJSS_REQSTAT_ZHZX_EXECUTE = 4;
        public const int INT_KJSS_REQSTAT_COMPLETED = 5;

        #endregion

        #region Zxk

        // 角色
        public static int ROLEID_ZXK_KZ = 10000057;  // 秩序科科长
        public static int ROLEID_ZXK_YG = 10000064;  // 秩序科员工

        #endregion

        #region Gggs

        public static string STR_AUTH_PUBLISHNOTICE_DELETE = "yctraffic.Gggs.PublishNotice.Delete";
        public static string STR_AUTH_PUBLISHNOTICE_MODIFY = "yctraffic.Gggs.PublishNotice.Modify";
        public static string STR_AUTH_PUBLISHNOTICE_ADD = "yctraffic.Gggs.PublishNotice.Add";
        public static string STR_AUTH_PUBLISHNOTICE_APPROVE = "yctraffic.Gggs.PublishNotice.Approve";

        public static string STR_PUBLISHNOTICE_APPROVED = "审核通过";
        public static string STR_PUBLISHNOTICE_NOAPPROVED = "审核不通过";

        public static string STR_GGGS_CATEGORY_ALL = "全部";
        public static string STR_GGGS_CATEGORY_1 = "警务公告";
        public static string STR_GGGS_CATEGORY_2 = "法律法规";
        public static string STR_GGGS_CATEGORY_3 = "其他";


        #endregion
        //标签样式
        public static List<MapMarkerStyle> StaticMarkerStyleList = new List<MapMarkerStyle> 
        {
            new MapMarkerStyle(Convert.ToInt32(MapMarkerType.MinorAccident), "小型事故", "/CustomMarkers/bigMarkerGreen.png", MapMarkerClass.Accident),
            new MapMarkerStyle(Convert.ToInt32(MapMarkerType.MediumAccident), "中型事故", "/CustomMarkers/bigMarkerBlue.png", MapMarkerClass.Accident),
            new MapMarkerStyle(Convert.ToInt32(MapMarkerType.SeriousAccident), "大型事故", "/CustomMarkers/bigMarkerPink.png", MapMarkerClass.Accident),
            new MapMarkerStyle(Convert.ToInt32(MapMarkerType.ExtraSeriousAccident), "特大型事故", "/CustomMarkers/bigMarkerRed.png", MapMarkerClass.Accident),                
            new MapMarkerStyle(Convert.ToInt32(MapMarkerType.NonMotorAccident), "非机动车事故", "/CustomMarkers/bigMarkerOrange.png", MapMarkerClass.Accident),
            new MapMarkerStyle(Convert.ToInt32(MapMarkerType.PedestrianAccident), "行人事故", "/CustomMarkers/bigMarkerPople.png", MapMarkerClass.Accident),
            new MapMarkerStyle(Convert.ToInt32(MapMarkerType.SignalLamp), "信号灯", "/CustomMarkers/SignalLamp.png", MapMarkerClass.Equipment),
            new MapMarkerStyle(Convert.ToInt32(MapMarkerType.Camera), "摄相头", "/CustomMarkers/Camera.png", MapMarkerClass.Equipment),
            new MapMarkerStyle(Convert.ToInt32(MapMarkerType.Bayonet), "卡口", "/CustomMarkers/Bayonet.png", MapMarkerClass.Equipment)
        };
    }

    public enum HotLineStatus
    {
        ////待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  归档 :8
        WaitDeal = 1,
        ToDDZ,
        ToZW,
        ZWToKS,
        DDZToKS,
        ReplyDDZ,
        Dealed,
        Archive,
    }

    public enum MonthRegisterStatus //新建：1， 提交：2， 审批：3.
    {
        Create=1,
        Handon,
        Approve
    }

    public enum MapMarkerType
    {
        MinorAccident = 1,
        MediumAccident,
        SeriousAccident,
        ExtraSeriousAccident,
        NonMotorAccident,
        PedestrianAccident,
        SignalLamp,
        Camera,
        Bayonet,
        Other
    }

    public enum MapMarkerClass
    {
        Accident = 1,
        Equipment
    }

    //用于绑定combox标签类型列表
    public class MapMarkerStyle
    {
        public int Id {get; set;}
        public string StyleName { get; set; }
        public string ImgSource { get; set; }
        public MapMarkerClass ClassId { get; set; }

        public MapMarkerStyle(int id, string styleName, string imgSource, MapMarkerClass classId)
        {
            this.Id = id;
            this.StyleName = styleName;
            this.ImgSource = imgSource;
            this.ClassId = classId;            
        }

        public override string ToString()
        {
            return this.StyleName;
        }
    }

    //用于删除标签
    public class DeleteMapMarker
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public string title { get; set; }
    }

    public class SearchPlaceType
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public SearchPlaceType(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class SearchPlaceItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public enum EquipmentCheckStatus
    {
        Draft,
        ForApply,
        ForAudit,
        Audited,
        AuditNoPass,
        DueDate,
        ForSupervise,
        Supervised,
        Archive
    }
}
