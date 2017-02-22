using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WafTraffic.Domain;
using System.Collections.ObjectModel;
using System.Data.Objects;

namespace WafTraffic.Applications.Services
{
    public interface IEntityService
    {
        ObservableCollection<MayorHotlineTaskTable> HotLineTasks { get; }
        IQueryable<MayorHotlineTaskTable> EnumHotLineTasks { get; }

        ObservableCollection<PersonArchiveTable> PersonArchives { get; }
        IQueryable<PersonArchiveTable> EnumPersonArchives { get; }

        IQueryable<HealthArchiveTable> EnumHealthArchives { get; }

        ObservableCollection<MonthRegisterTable> MonthRegisters { get; }
        IQueryable<MonthRegisterTable> EnumMonthRegisters { get; }
        List<MonthRegisterTable> GetMonthRegistersByUser(int userId, int year, int month);
        List<MayorHotlineTaskTable> GetHotLineMustDeal(int departmentId);
        List<MayorHotlineTaskTable> GetHotLineOverTime();
        //IQueryable<EquipmentCheckTable> GetEquipmentDueDate();

        IQueryable<ZgxcPublicityLogbook> QueryablePublicityLogbooks { get; }
        IQueryable<ZgxcAssistantCheckin> QueryableAssistantCheckins { get; }
        IQueryable<ZgxcPersonnelChange> QueryZgxcPersonnelChanges { get; }
        IQueryable<ZgxcAskForLeave> QueryZgxcAskForLeaves { get; }
        IQueryable<ZgxcAskForLeave> QueryZgxcWaitingForApprovalLeaves(string approver);

        List<MaterialDeclareGatherTable> GetGatherMaterialByIssueTime(DateTime startDate, DateTime endDate);
        List<MaterialDeclareGatherTable> GetGatherMaterialByDeclareTime(DateTime startDate, DateTime endDate);

        int GetMaterialDeclareNoApproveCount();

        List<MohthRgisterGatherTable> GetGatherApproves(int departmentId, DateTime minMonth, DateTime maxMonth);
        List<MohthRgisterChartTable> GetGatherChartSource(int departmentId, DateTime minMonth, DateTime maxMonth);
        string GetWorkContents(int selectYear, int selectMonth, string departmentCode);
        IEnumerable<MonthRegisterTable> EnumWorkBookReport { get; }

        List<ZhzxViolationGatherTable> GetViolationUploadGatherByCaptureTime(DateTime startDate, DateTime endDate);
        List<ZhzxViolationGatherTable> GetViolationApprovalGatherByCaptureTime(DateTime startDate, DateTime endDate);

        int ViolationCountPerLocateAndPlate(string checkPointName, string licensePlateNumber);
        //void RemoveOldestPicture(int cnt);
        //void RemoveOldestThumbnail(int cnt);

        IQueryable<ZhzxPicture> QueryableZhzxPicture { get; }
        IQueryable<ZhzxThumbnail> QueryableZhzxThumbnail { get; }

        IQueryable<ZhzxOfficeSupplyStock> QueryableZhzxOfficeSupplyStock { get; }
        IQueryable<ZhzxFixedAssetsRegister> QueryableZhzxFixedAssetsRegister { get; }
        IQueryable<ZhzxTotalViolation> QueryableZhzxTotalViolation { get; }
        IQueryable<ZhzxElectronMonitor> QueryableZhzxElectronMonitor { get; }


        //地图
        ObservableCollection<MapMarkersTable> MapMarkers { get; }
        IEnumerable<MapMarkersTable> EnumMapMarkers { get; }
        ObservableCollection<MapRouterTable> MapRouter { get; }
        IEnumerable<MapRouterTable> EnumMapRouter { get; }

        //科技设施科流程
        //ObservableCollection<EquipmentCheckTable> EquipmentChecks { get; }
        //IQueryable<EquipmentCheckTable> EnumEquipmentChecks { get; }

        // 中队台账类型接口
        //ObservableCollection<ZdtzConfigTable> SquadronLogbookTypes { get; }
        IEnumerable<ZdtzConfigTable> EnumSquadronLogbookTypes { get; }
        // 常用台账类型接口
        //ObservableCollection<ZdtzConfigTable> SquadronFrequentTypes { get; }
        IQueryable<ZdtzConfigTable> EnumSquadronFrequentTypes { get; }
        // 常用台帐:每天处罚情况台账接口
        ObservableCollection<ZdtzCyPunish> LbPunishs { get; }
        IQueryable<ZdtzCyPunish> EnumLbPunishs { get; }
        IQueryable<ZdtzCyPunish> QueryPunishs(int deptId, DateTime startDate, DateTime endDate);
        List<ZdtzCyPunishGatherTable> GetGatherPunishByDate(DateTime startDate, DateTime endDate);
        // 常用台帐:道路巡查情况台账接口
        ObservableCollection<ZdtzCyPatrol> LbPatrols { get; }
        IQueryable<ZdtzCyPatrol> EnumLbPatrols { get; }
        IQueryable<ZdtzCyPatrol> QueryPatrols(int deptId, DateTime startDate, DateTime endDate);
        // 常用台帐:隐患排查台账接口
        ObservableCollection<ZdtzCyDangerDeal> LbDangerDeals { get; }
        IQueryable<ZdtzCyDangerDeal> EnumLbDangerDeals(int type);
        IQueryable<ZdtzCyDangerDeal> QueryDangerDeals(int deptId, DateTime startDate, DateTime endDate, int type, string title);
        IQueryable<ZdtzCyDangerDeal> QueryZdtzOverTimeCyDangerDeal();

        // 公告公示类型接口
        IQueryable<GggsPublishNotice> QueryableGggsPublishNotice { get; }

        // 材料申报类型接口
        IQueryable<MaterialDeclareTable> EnumMaterialDeclares { get; }
        IQueryable<MaterialDeclareTable> EnumNoApproveMaterialDeclares { get; }
        // 中队人员基本信息台账接口
        ObservableCollection<ZdtzZdStaffInfo> LbStaffInfos { get; }
        IQueryable<ZdtzZdStaffInfo> EnumLbStaffInfos { get; }
        IQueryable<ZdtzZdStaffInfo> QueryStaffInfos(int deptId, DateTime startDate, DateTime endDate);
        // 静态台账接口
        //ObservableCollection<ZdtzStaticTable> LbStatics { get; }
        ObjectSet<ZdtzStaticTable> LbStatics { get; }
        IQueryable<ZdtzStaticTable> EnumLbStatics(int logbookTypeId);
        IQueryable<ZdtzStaticTable> QueryStatics(int logbookTypeId, int deptId, DateTime startDate, DateTime endDate, string title);

        // 违章查询
        IQueryable<ZhzxTrafficViolation> EnumZhzxTrafficViolations { get; }
        IQueryable<ZhzxTrafficViolation> QueryableZhzxFakePlateViolations { get; }

        // 台账配置
        IQueryable<ZdtzConfigTable> EnumLbConfigs { get; }
        List<ZdtzConfigTable> QueryConfigParentNodes();
        int QueryConfigMaxId();
        string QueryConfigMaxCodeByParent(int parentId);
        int QueryConfigUsingCount(int parentId);
        int QueryStaticUsingCount(int id);
        IQueryable<ZdtzConfigTable> QueryLbConfigs(string title);

        //ObjectSet<ZhzxThumbnail> ZhzxThumbnails { get; }
        //IEnumerable<ZhzxThumbnail> EnumZhzxThumbnails { get; }

        ObservableCollection<ZhzxRedNameList> ZhzxRedNameLists { get; }
        IQueryable<ZhzxRedNameList> EnumZhzxRedNameLists { get; }
        IQueryable<ZhzxRedNameList> QueryZhzxRedNameLists(string plateNumber);

        //ObservableCollection<ZhzxEquipmentRequest> ZhzxEquipmentRequests { get; }
        IQueryable<ZhzxEquipmentRequest> EnumZhzxEquipmentRequests { get; }
        IQueryable<ZhzxEquipmentRequest> QueryZhzxEquipmentRequests(int status, DateTime startDate, DateTime endDate);
        IQueryable<ZhzxEquipmentRequest> QueryZhzxOverTimeRequests();
        IQueryable<ZhzxEquipmentRequest> ZhzxEquipReq_NeedDdzDeals();

        yctrafficEntities Entities { get; set; }

        #region Kjss

        IQueryable<KjssEquipmentRequest> EnumKjssEquipmentRequests { get; }
        IQueryable<KjssEquipmentRequest> QueryKjssEquipmentRequests(int status, DateTime startDate, DateTime endDate);
        IQueryable<KjssEquipmentRequest> KjkEquipReq_NeedDdzDeals();

        IQueryable<SskEquipmentRequest> EnumSskEquipmentRequests { get; }
        IQueryable<SskEquipmentRequest> QuerySskEquipmentRequests(int status, DateTime startDate, DateTime endDate);

        #endregion

        #region Fzk

        IQueryable<FzkPunishCase> EnumFzkPunishCases { get; }
        IQueryable<FzkPunishCase> QueryFzkPunishCases(DateTime startDate, DateTime endDate);

        IQueryable<FzkPetition> EnumFzkPetitionCases { get; }
        IQueryable<FzkPetition> QueryFzkPetitionCases(DateTime startDate, DateTime endDate);

        IQueryable<FzkChangeMeasure> QueryFzkChangeMeasures { get; }
        //IQueryable<FzkChangeMeasure> QueryFzkChangeMeasures(DateTime startDate, DateTime endDate);

        IQueryable<FzkConsultation> QueryFzkConsultations { get; }
        IQueryable<FzkReleaseCar> QueryFzkReleaseCars { get; }
        IQueryable<FzkReleaseCar> FzkReleaseCarFdzDeals();

        IQueryable<FzkLawQualityLogbook> QueryableLawQualityLogbooks { get; }

        #endregion

        #region Cgs
        IQueryable<CgsVehicleAdminCase> EnumCgsVehicleAdminCases { get; }
        IQueryable<CgsVehicleAdminCase> QueryCgsVehicleAdminCases(DateTime startDate, DateTime endDate);


        IQueryable<CgsYellowMarkCar> QueryCgsYellowMarkCars {get;}
        IQueryable<CgsKeyCompanyLogbook> QueryCgsKeyCompanyLogbooks { get; }
        IQueryable<CgsKeyDriverLogbook> QueryCgsKeyDriverLogbooks { get; }
        IQueryable<CgsKeyVehicleLogbook> QueryCgsKeyVehicleLogbooks { get; }
        #endregion

        #region Qbyq
        IQueryable<QbyqInfoAnalysisCase> EnumQbyqInfoAnalysisCases { get; }
        IQueryable<QbyqInfoAnalysisCase> QueryQbyqInfoAnalysisCases(DateTime startDate, DateTime endDate);
        IQueryable<QbyqInfoAnalysisCase> NonReadQbyqInfo_DDZ { get; }
        IQueryable<QbyqInfoAnalysisCase> NonReadQbyqInfo_ZW { get; }
        #endregion

        #region Sgk
        IQueryable<SgkAccidentCase> EnumSgkAccidentCases { get; }
        IQueryable<SgkAccidentCase> QuerySgkAccidentCases(DateTime startDate, DateTime endDate);

        IQueryable<SgkReleaseCar> QuerySgkReleaseCars { get; }
        #endregion

        #region Zxk
        IQueryable<ZxkOrderCase> EnumZxkOrderCases { get; }
        IQueryable<ZxkOrderCase> QueryZxkOrderCases(DateTime startDate, DateTime endDate);
        #endregion

        IQueryable<FrequentUsedLink> QueryableFrequentUsedLinks { get; }
    }
}
