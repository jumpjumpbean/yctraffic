using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using WafTraffic.Domain;
using DotNet.Utilities;
using DotNet.Business;
using System.Data.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Objects.DataClasses;
using System.Data.Objects.SqlClient;
using WafTraffic.Applications.Common;
using WafTraffic.Domain.Common;
using System.Linq.Expressions;
using LinqKit;

namespace WafTraffic.Applications.Services
{
    [Export(typeof(IEntityService)), Export]
    internal class EntityService : IEntityService
    {
        #region Data

        private int curUserId;
        private yctrafficEntities entities;
      
        private ObservableCollection<MayorHotlineTaskTable> hotLineTasks = null;
        private IQueryable<MayorHotlineTaskTable> enumhotLineTasks = null;

        private ObservableCollection<PersonArchiveTable> personArchives = null;
        private IQueryable<PersonArchiveTable> enumPersonArchives = null;
        private IEnumerable<MonthRegisterTable> enumWorkBookReport = null;

        // 中队台账类型Entity
        //private ObservableCollection<ZdtzConfigTable> squadronLogbookTypes = null;
        private IEnumerable<ZdtzConfigTable> enumSquadronLogbookTypes = null;
        // 常用台帐
        //private ObservableCollection<ZdtzConfigTable> squadronFrequentTypes = null;
        private IQueryable<ZdtzConfigTable> enumSquadronFrequentTypes = null;
        // 常用台帐:每天处罚情况台账
        private ObservableCollection<ZdtzCyPunish> lbPunishs = null;
        private IQueryable<ZdtzCyPunish> enumLbPunishs = null;
        // 常用台帐:道路巡查情况台账
        private ObservableCollection<ZdtzCyPatrol> lbPatrols = null;
        private IQueryable<ZdtzCyPatrol> enumLbPatrols = null;
        // 常用台帐:隐患排查台账
        private ObservableCollection<ZdtzCyDangerDeal> lbDangerDeals = null;
        private IQueryable<ZdtzCyDangerDeal> enumLbDangerDeals = null;
        // 中队人员基本信息台账Entity
        private ObservableCollection<ZdtzZdStaffInfo> lbStaffInfos = null;
        private IQueryable <ZdtzZdStaffInfo> enumLbStaffInfos = null;
        // 静态台账Entity
        //private ObservableCollection<ZdtzStaticTable> lbStatics = null;
        private ObjectSet<ZdtzStaticTable> lbStatics = null;

        // 中队台账配置
        private IQueryable<ZdtzConfigTable> enumLbConfigs = null;

        private IQueryable<HealthArchiveTable> enumHealthArchives = null;

        private ObservableCollection<MonthRegisterTable> monthRegisters = null;
        private IQueryable<MonthRegisterTable> enumMonthRegisters = null;

        //公告公示类型
        private IQueryable<GggsPublishNotice> queryableGggsPublishNotice;

        //材料报送类型
        private IQueryable<MaterialDeclareTable> enumMaterialDeclares = null;
        private IQueryable<MaterialDeclareTable> enumNoApproveMaterialDeclares = null;

        //宣传类型
        private IQueryable<ZgxcPublicityLogbook> queryablePublicityLogbooks = null;

        //协勤考察
        private IQueryable<ZgxcAssistantCheckin> queryableAssistantCheckins = null;

        private IQueryable<ZgxcPersonnelChange> queryZgxcPersonnelChanges = null;
        private IQueryable<ZgxcAskForLeave> queryZgxcAskForLeaves = null;

        //地图
        private ObservableCollection<MapMarkersTable> mapMarkers = null;
        private IEnumerable<MapMarkersTable> enumMapMarkers = null;
        private ObservableCollection<MapRouterTable> mapRouter = null;
        private IEnumerable<MapRouterTable> enumMapRouter = null;

        //科技设施科流程
        //private ObservableCollection<EquipmentCheckTable> equipmentChecks = null;
        //private IQueryable<EquipmentCheckTable> enumEquipmentChecks = null;

        private IQueryable<ZhzxTrafficViolation> enumZhzxTrafficViolations = null;
        private IQueryable<ZhzxTrafficViolation> queryableZhzxFakePlateViolations = null;
        private IQueryable<ZhzxOfficeSupplyStock> queryableZhzxOfficeSupplyStock = null;
        private IQueryable<ZhzxFixedAssetsRegister> queryableZhzxFixedAssetsRegister = null;
        private IQueryable<ZhzxTotalViolation> queryableZhzxTotalViolation = null;
        private IQueryable<ZhzxElectronMonitor> queryableZhzxElectronMonitor = null;

        private ObservableCollection<ZhzxRedNameList> zhzxRedNameLists = null;
        private IQueryable<ZhzxRedNameList> enumZhzxRedNameLists = null;

        private IQueryable<ZhzxEquipmentRequest> enumZhzxEquipmentRequests = null;

        private IQueryable<KjssEquipmentRequest> enumKjssEquipmentRequests = null;
        private IQueryable<SskEquipmentRequest> enumSskEquipmentRequests = null;

        private IQueryable<FzkPunishCase> enumFzkPunishCases = null;
        private IQueryable<FzkPetition> enumFzkPetitionCases = null;
        private IQueryable<FzkChangeMeasure> queryFzkChangeMeasures = null;
        private IQueryable<FzkConsultation> queryFzkConsultations = null;
        private IQueryable<FzkReleaseCar> queryFzkReleaseCars = null;
        private IQueryable<FzkLawQualityLogbook> queryableLawQualityLogbooks = null;

        private IQueryable<CgsVehicleAdminCase> enumCgsVehicleAdminCases = null;
        private IQueryable<CgsYellowMarkCar> queryCgsYellowMarkCars = null;
        private IQueryable<CgsKeyCompanyLogbook> queryCgsKeyCompanyLogbooks = null;
        private IQueryable<CgsKeyDriverLogbook> queryCgsKeyDriverLogbooks = null;
        private IQueryable<CgsKeyVehicleLogbook> queryCgsKeyVehicleLogbooks = null;

        private IQueryable<QbyqInfoAnalysisCase> enumQbyqInfoAnalysisCases = null;
        private IQueryable<QbyqInfoAnalysisCase> nonReadQbyqInfo_DDZ = null;
        private IQueryable<QbyqInfoAnalysisCase> nonReadQbyqInfo_ZW = null;

        private IQueryable<SgkAccidentCase> enumSgkAccidentCases = null;
        private IQueryable<SgkReleaseCar> querySgkReleaseCars = null;

        private IQueryable<ZxkOrderCase> enumZxkOrderCases = null;

        private IQueryable<FrequentUsedLink> queryableFrequentUsedLinks;

        #endregion

        #region Properties

        public yctrafficEntities Entities
        {
            get { return entities; }
            set { entities = value; }
        }
        
        
        public ObservableCollection<MayorHotlineTaskTable> HotLineTasks
        {
            get
            {
                if (hotLineTasks == null && entities != null)
                {
                    hotLineTasks = new EntityObservableCollection<MayorHotlineTaskTable>(entities.MayorHotlineTaskTables);
                }
                return hotLineTasks;
            }
        }

        /// <summary>
        /// 用于查询
        /// </summary>
        public IQueryable<MayorHotlineTaskTable> EnumHotLineTasks 
        {
            get
            {
                entities.MayorHotlineTaskTables.MergeOption = MergeOption.OverwriteChanges;

                int statusWaitDeal = Convert.ToInt32(HotLineStatus.WaitDeal);
                int statusReplyDDZ = Convert.ToInt32(HotLineStatus.ReplyDDZ);
                int statusDealed =  Convert.ToInt32(HotLineStatus.Dealed);
                int statusToDDZ = Convert.ToInt32(HotLineStatus.ToDDZ);
                int statusToZW = Convert.ToInt32(HotLineStatus.ToZW);

                int curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                if (entities != null)
                { ////待处理 :1， 发给大队长：2， 发给政委：3， 政委发给科室：4， 大队长发给科室：5， 回复大队长：6，  已处理 :7，  归档 :8
                    if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                    {
                        enumhotLineTasks = entities.MayorHotlineTaskTables.Where(
                            entity => (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                        );
                    }
                    else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.hotline.ListAll")) //政工科
                    {
                        //enumhotLineTasks = entities.MayorHotlineTaskTables;
                        enumhotLineTasks = entities.MayorHotlineTaskTables.Where(
                            entity => ( object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false )
                        );
                    }
                    //else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.hotline.ListOverDue") && CurrentLoginService.Instance.CurrentUserInfo.RoleId == 10000047) //大队长
                    else if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_DDZ) //大队长
                    {
                        //过期的，大队长会看到
                        enumhotLineTasks = entities.MayorHotlineTaskTables.Where(
                            entity =>
                                (entity.DueDate <= DateTime.Now
                                || entity.StatusId == statusReplyDDZ
                                || (entity.StatusId == statusDealed && entity.SovleUserId == curUserId)
                                || entity.StatusId == statusToDDZ)
                                && (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                            );

                    }
                    else if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_ZW) //政委
                    {
                        enumhotLineTasks = entities.MayorHotlineTaskTables.Where(
                            entity =>
                                (entity.StatusId == statusToZW
                                || (entity.StatusId == statusDealed && entity.SovleUserId == curUserId)
                                )
                                && (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                            );

                    }
                    else
                    {
                        //ownerdepart 是本部门
                        enumhotLineTasks = entities.MayorHotlineTaskTables.Where
                            (
                                entity =>
                                    (
                                        (!object.Equals(null, entity.OwnDepartmentId) 
                                            && entity.OwnDepartmentId == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId
                                            && entity.StatusId > statusWaitDeal && entity.StatusId < statusDealed)
                                        || (entity.StatusId == statusDealed && entity.SovleUserId == curUserId)
                                    )
                                    && (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                            );
                    }
                }
                return enumhotLineTasks;
            }
        }

        public ObservableCollection<PersonArchiveTable> PersonArchives
        {
            get
            {
                if (personArchives == null && entities != null)
                {
                    personArchives = new EntityObservableCollection<PersonArchiveTable>(entities.PersonArchiveTables);
                }
                return personArchives;
            }
        }

        public List<MonthRegisterTable> GetMonthRegistersByUser(int userId, int year, int month)
        {
            List<MonthRegisterTable> userMonthRegisters;
            using (var context = new yctrafficEntities())
            {

                string strSql = "select * from MonthRegisterTable  ";
                strSql += "where year(WhichMonth) >= @year and month(WhichMonth) <=@month ";
                strSql += "and userId=@userId and (IsDeleted=0 or IsDeleted is NULL)";

                var args = new DbParameter[] 
                {
                      new SqlParameter {ParameterName = "year", Value = year},
                      new SqlParameter {ParameterName = "month", Value = month},
                      new SqlParameter {ParameterName = "userId", Value = userId}
                };
                userMonthRegisters = context.ExecuteStoreQuery<MonthRegisterTable>(strSql, args).ToList<MonthRegisterTable>();

            }
            if (userMonthRegisters == null)
            {
                userMonthRegisters = new List<MonthRegisterTable>();
            }
            return userMonthRegisters;
        }

        public List<MayorHotlineTaskTable> GetHotLineMustDeal(int departmentId)
        {
            List<MayorHotlineTaskTable> hotLineMustDeal;
            using (var context = new yctrafficEntities())
            {

                //string strSql = "select * from MayorHotlineTaskTable where ( getdate()<=DueDate and getdate()>=DATEADD(DAY, -2, DueDate)) and OwnDepartmentId is not null and OwnDepartmentId=@departmentId and (statusid=3 or statusid=4) and (IsDeleted=0 or IsDeleted is NULL) ";
                //string strSql = "select * from MayorHotlineTaskTable where (getdate()>=DATEADD(DAY, -2, DueDate)) and OwnDepartmentId is not null and OwnDepartmentId=@departmentId and (statusid=" + HotLineStatus.DDZToKS.ToString() + " or statusid=" + HotLineStatus.ZWToKS.ToString() + ") and (IsDeleted=0 or IsDeleted is NULL) ";
                string strSql = "select * from MayorHotlineTaskTable where (getdate()>=DATEADD(DAY, -2, DueDate)) and OwnDepartmentId is not null and OwnDepartmentId=@departmentId and (statusid>" +  Convert.ToInt32(HotLineStatus.WaitDeal) + " and statusid<" + Convert.ToInt32(HotLineStatus.Dealed) + ") and (IsDeleted=0 or IsDeleted is NULL) ";

                var args = new DbParameter[] 
                {
                      new SqlParameter {ParameterName = "departmentId", Value = departmentId}
                };
                hotLineMustDeal = context.ExecuteStoreQuery<MayorHotlineTaskTable>(strSql, args).ToList<MayorHotlineTaskTable>();

            }
            if (hotLineMustDeal == null)
            {
                hotLineMustDeal = new List<MayorHotlineTaskTable>();
            }
            return hotLineMustDeal;
        }

        public List<MayorHotlineTaskTable> GetHotLineOverTime()
        {
            List<MayorHotlineTaskTable> hotLineMustDeal;
            using (var context = new yctrafficEntities())
            {

                string strSql = "select * from MayorHotlineTaskTable where DueDate < getdate() and statusid>" + Convert.ToInt32(HotLineStatus.WaitDeal) + " and statusid<" + Convert.ToInt32(HotLineStatus.Dealed) + " and (IsDeleted=0 or IsDeleted is NULL) ";

                hotLineMustDeal = context.ExecuteStoreQuery<MayorHotlineTaskTable>(strSql).ToList<MayorHotlineTaskTable>();

            }
            if (hotLineMustDeal == null)
            {
                hotLineMustDeal = new List<MayorHotlineTaskTable>();
            }
            return hotLineMustDeal;
        }

        /*
        public IQueryable<EquipmentCheckTable> GetEquipmentDueDate()
        {

            int stDueDate = Convert.ToInt32(EquipmentCheckStatus.DueDate);
            var results = from u in entities.EquipmentCheckTables
                          where (u.StatusId == stDueDate)
                          && u.IsDeleted == false
                          select u;

            return results;
        }*/

        public List<MohthRgisterGatherTable> GetGatherApproves(int departmentId, DateTime minMonth, DateTime maxMonth)
        {
            List<MohthRgisterGatherTable> gatherApproves;
            using (var context = new yctrafficEntities())
            {

                string strSql = "select ";
                strSql += "UserId, ";
                strSql += "UserName, ";
                strSql += "SUM(case when ApproveResult = '非常优秀' then 1 else 0 end) as Excel, ";
                strSql += "SUM(case when ApproveResult = '优秀' then 1 else 0 end) as Well, ";
                strSql += "SUM(case when ApproveResult = '良好' then 1 else 0 end) as Good, ";
                strSql += "SUM(case when ApproveResult = '一般' then 1 else 0 end) as Normal, ";
                strSql += "SUM(case when ApproveResult = '差' then 1 else 0 end) as Bad ";
                strSql += "from MonthRegisterTable  ";
                strSql += "where (DepartmentId is not null and DepartmentId=@departmentId) and WhichMonth >= @minMonth and WhichMonth <=@maxMonth and (IsDeleted=0 or IsDeleted is NULL) ";
                strSql += "group by UserId, UserName";

                var args = new DbParameter[] 
                {
                    new SqlParameter {ParameterName = "departmentId", Value = departmentId},
                      new SqlParameter {ParameterName = "minMonth", Value = minMonth},
                      new SqlParameter {ParameterName = "maxMonth", Value = maxMonth}
                };
                gatherApproves = context.ExecuteStoreQuery<MohthRgisterGatherTable>(strSql, args).ToList<MohthRgisterGatherTable>();
               
            }
            return gatherApproves;
        }

        public List<MohthRgisterChartTable> GetGatherChartSource(int departmentId, DateTime minMonth, DateTime maxMonth)
        {
            List<MohthRgisterChartTable> gatherChartSource;
            using (var context = new yctrafficEntities())
            {

                //string strSql = "select UserId, UserName, WhichMonth, ApproveResult from MonthRegisterTable ";                
                string strSql = "select ";
                //strSql += "( ";
                //strSql += "case when ApproveResult='非常优秀' then 5  ";
                //strSql += "	else case when ApproveResult='优秀' then 4  ";
                //strSql += "		else case when ApproveResult='良好' then 3  ";
                //strSql += "			else case when ApproveResult='一般' then 2 ";
                //strSql += "				else case when ApproveResult='差' then 1 end ";
                //strSql += "			end  ";
                //strSql += "		end ";
                //strSql += "	end	 ";
                //strSql += "end ";
                //strSql += ") as ResultCode, ";
                strSql += "0 as ResultCode, UserId, UserName, WhichMonth, ApproveResult, '' as ShortMonth from MonthRegisterTable ";
                strSql += "where (DepartmentId is not null and DepartmentId=@departmentId) and WhichMonth >= @minMonth and WhichMonth <=@maxMonth and (IsDeleted=0 or IsDeleted is NULL) order by WhichMonth asc ";
                
                var args = new DbParameter[] 
                {
                    new SqlParameter {ParameterName = "departmentId", Value = departmentId},
                      new SqlParameter {ParameterName = "minMonth", Value = minMonth},
                      new SqlParameter {ParameterName = "maxMonth", Value = maxMonth}
                };
                gatherChartSource = context.ExecuteStoreQuery<MohthRgisterChartTable>(strSql, args).ToList<MohthRgisterChartTable>();

                foreach (MohthRgisterChartTable entity in gatherChartSource)
                {
                    entity.ShortMonth = Convert.ToDateTime(entity.WhichMonth).ToString("yyyy-MM");
                    if (entity.ApproveResult == "非常优秀")
                    {
                        entity.ResultCode = 5;
                    }
                    else if (entity.ApproveResult == "优秀")
                    {
                        entity.ResultCode = 4;
                    }
                    else if (entity.ApproveResult == "良好")
                    {
                        entity.ResultCode = 3;
                    }
                    else if (entity.ApproveResult == "一般")
                    {
                        entity.ResultCode = 2;
                    }
                    else if (entity.ApproveResult == "差")
                    {
                        entity.ResultCode = 1;
                    }
                }

            }
            return gatherChartSource;
        }

        public IEnumerable<MonthRegisterTable> EnumWorkBookReport
        {
            get
            {
                return enumWorkBookReport;
            }
            set
            {
                enumWorkBookReport = value;
            }
        }

        public List<MaterialDeclareGatherTable> GetGatherMaterialByIssueTime(DateTime startDate, DateTime endDate)
        {
            List<MaterialDeclareGatherTable> gatherMaterials;
            using (var context = new yctrafficEntities())
            {

                string strSql = "select ";
                strSql += "DepartmentId, ";
                strSql += "DepartmentName, ";
                strSql += "COUNT (DepartmentId) as CountPerDept, SUM(ISNULL(Score, 0)) as ScorePerDept ";
                strSql += "from MaterialDeclareTable where (MaterialIssueTime >= @startDate and MaterialIssueTime<= @endDate and IsDeleted = 0)";
                strSql += "group by DepartmentId, DepartmentName";

                var args = new DbParameter[] 
                {
                      new SqlParameter {ParameterName = "startDate", Value = startDate},
                      new SqlParameter {ParameterName = "endDate", Value = endDate.AddDays(1)}
                };

                gatherMaterials = context.ExecuteStoreQuery<MaterialDeclareGatherTable>(strSql, args).ToList<MaterialDeclareGatherTable>();

            }
            return gatherMaterials;
        }

        public List<MaterialDeclareGatherTable> GetGatherMaterialByDeclareTime(DateTime startDate, DateTime endDate)
        {
            List<MaterialDeclareGatherTable> gatherMaterials;
            using (var context = new yctrafficEntities())
            {

                string strSql = "select ";
                strSql += "DepartmentId, ";
                strSql += "DepartmentName, ";
                strSql += "COUNT (DepartmentId) as CountPerDept, SUM(ISNULL(Score, 0)) as ScorePerDept ";
                strSql += "from MaterialDeclareTable where (MaterialDeclareTime >= @startDate and MaterialDeclareTime<= @endDate and IsDeleted = 0)";
                strSql += "group by DepartmentId, DepartmentName";

                var args = new DbParameter[] 
                {
                      new SqlParameter {ParameterName = "startDate", Value = startDate},
                      new SqlParameter {ParameterName = "endDate", Value = endDate.AddDays(1)}
                };

                gatherMaterials = context.ExecuteStoreQuery<MaterialDeclareGatherTable>(strSql, args).ToList<MaterialDeclareGatherTable>();

            }
            return gatherMaterials;
        }

        public int GetMaterialDeclareNoApproveCount()
        {
            int cnt;

            using (var context = new yctrafficEntities())
            {

                string strSql = "select count(Id) from MaterialDeclareTable where (Score is NULL and IsDeleted = 0)";

                cnt = context.ExecuteStoreQuery<int>(strSql).ToList()[0];

            }

            return cnt;
        }

        public string GetWorkContents(int selectYear, int selectMonth, string departmentCode)
        {
            string workContents = string.Empty;
            List<string> tmpList = new List<string>();
            List<MonthRegisterTable> tmpMRList = new List<MonthRegisterTable>();
            using (var context = new yctrafficEntities())
            {

                string strSql = "select (WorkSummary+CHAR(10)+CHAR(9)+' -- '+UserName+'('+isnull(PoliceNumber,0)+')'+CHAR(10)+CHAR(10)) as workcontents from MonthRegisterTable ";
                strSql += " where year(whichMonth) = @selectYear and MONTH(WhichMonth) = @selectMonth and PATINDEX(@departmentCode, DepartmentCode) = 1 and (IsDeleted=0 or IsDeleted is null)";

                var args = new DbParameter[] 
                {
                      new SqlParameter {ParameterName = "selectYear", Value = selectYear},
                      new SqlParameter {ParameterName = "selectMonth", Value = selectMonth},
                       new SqlParameter {ParameterName = "departmentCode", Value = departmentCode}
                };

                tmpList = context.ExecuteStoreQuery<string>(strSql, args).ToList<string>();
            }

            using (var context = new yctrafficEntities())
            {
                var args = new DbParameter[] 
                {
                      new SqlParameter {ParameterName = "selectYear", Value = selectYear},
                      new SqlParameter {ParameterName = "selectMonth", Value = selectMonth},
                       new SqlParameter {ParameterName = "departmentCode", Value = departmentCode}
                };

                string strSql2 = "select * from MonthRegisterTable ";
                strSql2 += " where year(whichMonth) = @selectYear and MONTH(WhichMonth) = @selectMonth and PATINDEX(@departmentCode, DepartmentCode) = 1";

                tmpMRList = context.ExecuteStoreQuery<MonthRegisterTable>(strSql2, args).ToList<MonthRegisterTable>();
            }

            this.enumWorkBookReport = new List<MonthRegisterTable>();
            DataTable dt = DotNetService.Instance.UserService.GetDTByDepartment(CurrentLoginService.Instance.CurrentUserInfo, YcConstantTable.DEPARTID_ZGK.ToString(), true); // "100000012" --政工科
            if (tmpList == null || dt.Rows.Count > tmpList.Count) //有人未填写
            {
                workContents = string.Empty;                
            }
            else
            {
                if (tmpList != null && tmpList.Count > 0)
                {
                    foreach (string tmpstr in tmpList)
                    {
                            workContents += tmpstr;
                        }
                    this.enumWorkBookReport = tmpMRList;
                        }
                else
                {
                    workContents = string.Empty;
                }
            }
            return workContents;
        }

        /// <summary>
        /// 用于查询
        /// </summary>
        public IQueryable<PersonArchiveTable> EnumPersonArchives
        {
            get
            {
                entities.PersonArchiveTables.MergeOption = MergeOption.OverwriteChanges;
                
                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.PersonArchive.ListAll")) //大队长和政委
                {
                    enumPersonArchives = entities.PersonArchiveTables.Where
                        (
                            entity => (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                        );
                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.PersonArchive.ListDepartAll")) //直属领导
                { //
                    string departCode = string.Empty;
                    BaseOrganizeEntity organize = DotNetService.Instance.OrganizeService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo, CurrentLoginService.Instance.CurrentUserInfo.DepartmentId.ToString());
                    if (organize != null && organize.Id > 0)
                    {
                        departCode = organize.Code;
                    }
                    enumPersonArchives = entities.PersonArchiveTables.Where
                        (
                            entity => (
                                (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                                && entity.DepartmentCode.IndexOf(departCode) == 0
                            )
                        );
                }
                else
                {
                    curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    //每个人只查看自己的健康信息
                    enumPersonArchives = entities.PersonArchiveTables.Where
                            (
                                entity => entity.CreateUserId == curUserId
                            );
                }

                
                return enumPersonArchives;
            }
        }

        /// <summary>
        /// 用于查询
        /// </summary>
        public IQueryable<HealthArchiveTable> EnumHealthArchives
        {
            get
            {
                //if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                //{
                //    enumHealthArchives = entities.HealthArchiveTables.Where<HealthArchiveTable>
                //    (
                //        entity => (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                //    );
                //    return enumHealthArchives;
                //}
                entities.HealthArchiveTables.MergeOption = MergeOption.OverwriteChanges;

                if (CurrentLoginService.Instance.IsAuthorized("yctraffic.HealthArchive.ListAll")) //大队长
                {
                    enumHealthArchives = entities.HealthArchiveTables.Where<HealthArchiveTable>
                    (
                        entity => (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                    );
                    return enumHealthArchives;
                }

                //每个人只查看自己的健康信息
                enumHealthArchives = entities.HealthArchiveTables.Where
                        (
                            entity => entity.Name == CurrentLoginService.Instance.CurrentUserInfo.RealName
                                && (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                        );

                return enumHealthArchives;
            }
        }

        public ObservableCollection<MonthRegisterTable> MonthRegisters
        {
            get
            {
                if (monthRegisters == null && entities != null)
                {
                    monthRegisters = new EntityObservableCollection<MonthRegisterTable>(entities.MonthRegisterTables);
                }
                return monthRegisters;
            }
        }
        
        /// <summary>
        /// 用于查询
        /// </summary>
        public IQueryable<MonthRegisterTable> EnumMonthRegisters
        {
            get
            {
                entities.MonthRegisterTables.MergeOption = MergeOption.OverwriteChanges;

                List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                if (depts != null)
                {
                    var companyEntity = depts.Find(en => en.Id == YcConstantTable.COMPANY_ID);
                    if (companyEntity != null)
                    {
                        depts.Remove(companyEntity);
                    }
                }
                var predicate = PredicateBuilder.True<MonthRegisterTable>();

                int createStatus = Convert.ToInt32(MonthRegisterStatus.Create);
                int handonStatus = Convert.ToInt32(MonthRegisterStatus.Handon);

                int curUserId = object.Equals(null, CurrentLoginService.Instance.CurrentUserInfo) == true ? 0 : Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);

                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator == true) 
                { //
                    enumMonthRegisters = entities.MonthRegisterTables.Where
                           (
                               entity => (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)                              
                           );
                }
                else if (CurrentLoginService.Instance.CurrentUserInfo.RoleId == YcConstantTable.ROLEID_DDZ)
                { //大队长看到所有提交的记录
                    enumMonthRegisters = entities.MonthRegisterTables.Where
                           (
                               entity => (                                   
                                   ( (entity.UserId.Value != curUserId && entity.StatusId > createStatus ) || (entity.UserId.Value == curUserId) )
                                   && (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                                   )
                           );
                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ListAllDepartAll")) //政工科长看科所队长及下属人员的, 已经看到所有科室不用再判断分管科室权限
                {//除大队长，副大队长 和 政委 以外的所有人员
                    RoleService roleService = new RoleService();
                    string[] ksdzArray = roleService.GetRoleUserIds(CurrentLoginService.Instance.CurrentUserInfo, YcConstantTable.GROUPID_KSDZ.ToString()); //科所队长
                    List<string> ksdzDepartCodeList = new List<string>();
                    BaseUserEntity tmpUserEntity = new BaseUserEntity();
                    foreach (string tmpuserID in ksdzArray)
                    {
                        //找到科所队长的部门code
                        tmpUserEntity = DotNetService.Instance.UserService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo, tmpuserID);
                        string tmpCode = DotNetService.Instance.OrganizeService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo, tmpUserEntity.DepartmentId.ToString()).Code;
                        ksdzDepartCodeList.Add(tmpCode);
                    }

                    //列出科所队长及其部门人员的登记记录
                    enumMonthRegisters = entities.MonthRegisterTables.Where
                           (delegate(MonthRegisterTable entity)
                           {
                               if (entity.IsDeleted == true) //说明已经删除了
                               {
                                   return false;
                               }

                               if (entity.CreateUserId == curUserId)
                               {
                                   return true;
                               }
                               else
                               {
                                   if (entity.StatusId > createStatus) //提交之后的才能看到
                                   {
                                       foreach (string dcode in ksdzDepartCodeList)
                                       {
                                           if (entity.DepartmentCode != null && entity.DepartmentCode.IndexOf(dcode) == 0)
                                           {
                                               return true;
                                           }
                                       }
                                   }
                               }
                               return false;
                           }).AsQueryable();

                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ListDZAll") &&
                    CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ListDepartAll")) //即是领导班子，又是科所队长
                {
                    BaseOrganizeManager orgManager = new BaseOrganizeManager();
                    string curDepartCode = orgManager.GetCodeById(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId.ToString());

                    RoleService roleService = new RoleService();
                    string[] ksdzArray = roleService.GetRoleUserIds(CurrentLoginService.Instance.CurrentUserInfo, YcConstantTable.GROUPID_KSDZ.ToString()); //科所队长
                    List<int?> ksdzList = new List<int?>();
                    foreach (string tmpuserID in ksdzArray)
                    {
                        ksdzList.Add(Convert.ToInt32(tmpuserID));
                    }

                    predicate = predicate.And(entity => (
                                   ((entity.CreateUserId == curUserId 
                                   || (entity.StatusId > createStatus && entity.DepartmentCode.IndexOf(curDepartCode) == 0) 
                                   || (entity.StatusId > createStatus && ksdzList.Contains(entity.UserId)))  //entity.StatusId == handonStatus
                                    && (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false))
                                   )
                              );


                    if (depts != null && depts.Count > 0)
                    {
                        foreach (BaseOrganizeEntity dept in depts)
                        {
                            BaseOrganizeEntity tempDept = dept;
                            predicate = predicate.Or(p => p.DepartmentCode.IndexOf(tempDept.Code) == 0);
                        }
                    }

                    enumMonthRegisters = entities.MonthRegisterTables.AsExpandable().Where(predicate);
                   

                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ListDZAll")) //领导班子只列出科所队长提交的记录
                {
                    string[] ksdzArray = DotNetService.Instance.RoleService.GetRoleUserIds(CurrentLoginService.Instance.CurrentUserInfo, YcConstantTable.GROUPID_KSDZ.ToString()); //科所队长
                    List<int?> ksdzList = new List<int?>();
                    foreach (string tmpuserID in ksdzArray)
                    {
                        ksdzList.Add(Convert.ToInt32(tmpuserID));
                    }

                    predicate = predicate.And(entity => (
                                   ((entity.CreateUserId == curUserId
                                   || (entity.StatusId > createStatus && ksdzList.Contains(entity.UserId)))  //entity.StatusId == handonStatus
                                   && (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false))
                                   )
                              );


                    if (depts != null && depts.Count > 0)
                    {
                        foreach (BaseOrganizeEntity dept in depts)
                        {
                            BaseOrganizeEntity tempDept = dept;
                            predicate = predicate.Or(p => p.DepartmentCode.IndexOf(tempDept.Code) == 0);
                        }
                    }

                    enumMonthRegisters = entities.MonthRegisterTables.AsExpandable().Where(predicate);


                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.MonthRegister.ListDepartAll")) //科所队长只列出本部门的记录
                {
                    BaseOrganizeManager orgManager = new BaseOrganizeManager();
                    string curDepartCode = orgManager.GetCodeById(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId.ToString());

                    predicate = predicate.And(entity => (
                                   ((entity.CreateUserId == curUserId || (entity.StatusId > createStatus && entity.DepartmentCode.IndexOf(curDepartCode) == 0))
                                    && (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false))
                                  )
                             );


                    if (depts != null && depts.Count > 0)
                    {
                        foreach (BaseOrganizeEntity dept in depts)
                        {
                            BaseOrganizeEntity tempDept = dept;
                            predicate = predicate.Or(p => p.DepartmentCode.IndexOf(tempDept.Code) == 0);
                        }
                    }

                    enumMonthRegisters = entities.MonthRegisterTables.AsExpandable().Where(predicate);

                }
                else
                {
                    curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    //每个人只查看自己的登记信息
                    enumMonthRegisters = entities.MonthRegisterTables.Where
                            (
                                entity => entity.CreateUserId == curUserId
                                    && (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                            );
                }
                
                return enumMonthRegisters;
            }
        }

        /// <summary>
        /// 地图Marker列表
        /// </summary>
        public ObservableCollection<MapMarkersTable> MapMarkers
        {
            get
            {
                if (mapMarkers == null && entities != null)
                {
                    mapMarkers = new EntityObservableCollection<MapMarkersTable>(entities.MapMarkersTables); ;
                }
                return mapMarkers;
            }
        }

        /// <summary>
        /// 地图Router列表
        /// </summary>
        public ObservableCollection<MapRouterTable> MapRouter
        {
            get
            {
                if (mapRouter == null && entities != null)
                {
                    mapRouter = new EntityObservableCollection<MapRouterTable>(entities.MapRouterTables); ;
                }
                return mapRouter;
            }
        }

        /// <summary>
        /// 地图Marker列表
        /// </summary>
        public IEnumerable<MapMarkersTable> EnumMapMarkers
        {
            get
            {
                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    enumMapMarkers = entities.MapMarkersTables.Where
                        (
                            entity => (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                        );
                }
                else
                {
                    enumMapMarkers = entities.MapMarkersTables.Where<MapMarkersTable>
                    (
                        entity => 
                            entity.DepartmentId == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId
                            && 
                            (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                    );
                }

                return enumMapMarkers;
            }
        }

        /// <summary>
        /// 地图Router列表
        /// </summary>
        public IEnumerable<MapRouterTable> EnumMapRouter
        {
            get
            {
                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    enumMapRouter = entities.MapRouterTables.Where
                        (
                            entity => (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                        );
                }
                else
                {
                    enumMapRouter = entities.MapRouterTables.Where<MapRouterTable>
                    (
                        entity =>
                            entity.DepartmentId == CurrentLoginService.Instance.CurrentUserInfo.DepartmentId
                            &&
                            (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                    );
                }

                return enumMapRouter;
            }
        }

        /*
        /// <summary>
        /// 科技设施科流程
        /// </summary>
        public ObservableCollection<EquipmentCheckTable> EquipmentChecks
        {
            get
            {
                if (equipmentChecks == null && entities != null)
                {
                    equipmentChecks = new EntityObservableCollection<EquipmentCheckTable>(entities.EquipmentCheckTables);
                }
                return equipmentChecks;
            }
        }

        /// <summary>
        /// 科技设施科流程
        /// </summary>
        public IQueryable<EquipmentCheckTable> EnumEquipmentChecks
        {
            get
            {
                if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
                {
                    enumEquipmentChecks = entities.EquipmentCheckTables.Where
                        (
                            entity => (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                        );
                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.EquipmentCheck.Modify"))
                {
                    int curDepartId = CurrentLoginService.Instance.CurrentUserInfo.DepartmentId.Value;
                    enumEquipmentChecks = entities.EquipmentCheckTables.Where
                        (
                            entity => (
                                (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                                && entity.DepartmentId.Value == curDepartId
                                )
                        );
                }
                else if ( CurrentLoginService.Instance.IsAuthorized("yctraffic.EquipmentCheck.Audit") )
                {
                    int auditStatus = Convert.ToInt32(EquipmentCheckStatus.ForAudit);
                    enumEquipmentChecks = entities.EquipmentCheckTables.Where
                        (
                            entity => (
                                (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                                && (entity.StatusId == auditStatus)
                                )
                        );
                }
                else if ( CurrentLoginService.Instance.IsAuthorized("yctraffic.EquipmentCheck.Supervise") )
                {
                    int superviseStatus = Convert.ToInt32(EquipmentCheckStatus.ForSupervise);
                    enumEquipmentChecks = entities.EquipmentCheckTables.Where
                        (
                            entity => (
                                (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                                && (entity.StatusId == superviseStatus)
                                )
                        );
                }
                else if (CurrentLoginService.Instance.IsAuthorized("yctraffic.EquipmentCheck.Audit") 
                    && CurrentLoginService.Instance.IsAuthorized("yctraffic.EquipmentCheck.Supervise") )
                {
                    int auditStatus = Convert.ToInt32(EquipmentCheckStatus.ForAudit);
                    int superviseStatus = Convert.ToInt32(EquipmentCheckStatus.ForSupervise);
                    enumEquipmentChecks = entities.EquipmentCheckTables.Where
                        (
                            entity => (
                                (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                                &&( entity.StatusId == auditStatus || entity.StatusId == superviseStatus)
                                )
                        );
                }
                else
                {
                    enumEquipmentChecks = entities.EquipmentCheckTables.Where<EquipmentCheckTable>
                    (
                        entity =>
                            (object.Equals(null, entity.IsDeleted) || entity.IsDeleted == false)
                    );
                }

                return enumEquipmentChecks;
            }
        }
        */

        #region SquadronLogbookProperties
        
        /*
        /// <summary>
        /// 中队台账类型
        /// </summary>
        public ObservableCollection<ZdtzConfigTable> SquadronLogbookTypes
        {
            get
            {
                if (squadronLogbookTypes == null && entities != null)
                {
                    squadronLogbookTypes = new EntityObservableCollection<ZdtzConfigTable>(entities.ZdtzConfigTables);
                }
                return squadronLogbookTypes;
            }
        }
        */

        /// <summary>
        /// 中队台账类型
        /// </summary>
        public IEnumerable<ZdtzConfigTable> EnumSquadronLogbookTypes
        {
            get
            {
                entities.ZdtzConfigTables.MergeOption = MergeOption.OverwriteChanges;

                enumSquadronLogbookTypes = entities.ZdtzConfigTables.Where
                    (
                        entity => entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                    ).OrderBy(entity => entity.Code);

                return enumSquadronLogbookTypes;
            }
        }

        /*
        /// <summary>
        /// 常用台账类型
        /// </summary>
        public ObservableCollection<ZdtzConfigTable> SquadronFrequentTypes
        {
            get
            {
                if (squadronFrequentTypes == null && entities != null)
                {
                    squadronFrequentTypes = new EntityObservableCollection<ZdtzConfigTable>(entities.ZdtzConfigTables);
                }
                return squadronFrequentTypes;
            }
        }
        */

        /// <summary>
        /// 常用台账类型
        /// </summary>
        public IQueryable<ZdtzConfigTable> EnumSquadronFrequentTypes
        {
            get
            {
                entities.ZdtzConfigTables.MergeOption = MergeOption.OverwriteChanges;

                enumSquadronFrequentTypes = entities.ZdtzConfigTables.Where
                    (
                        entity => (entity.LogbookType == YcConstants.INT_LBCONFIG_TYPE_FREQUENT 
                            && entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                    ).OrderBy(entity => entity.Code);

                return enumSquadronFrequentTypes;
            }
        }

        /// <summary>
        /// 常用台账类型:每天处罚情况
        /// </summary>
        public ObservableCollection<ZdtzCyPunish> LbPunishs
        {
            get
            {
                if (lbPunishs == null && entities != null)
                {
                    lbPunishs = new EntityObservableCollection<ZdtzCyPunish>(entities.ZdtzCyPunishes);
                }
                return lbPunishs;
            }
        }

        /// <summary>
        /// 常用台账类型:每天处罚情况
        /// </summary>
        public IQueryable<ZdtzCyPunish> EnumLbPunishs
        {
            get
            {
                entities.ZdtzCyPunishes.MergeOption = MergeOption.OverwriteChanges;

                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    enumLbPunishs = entities.ZdtzCyPunishes.Where
                    (
                        entity => entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                    );

                }
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
                {
                    List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                    if (depts != null && depts.Count > 0)
                    {
                        var predicate = PredicateBuilder.False<ZdtzCyPunish>();
                        foreach (BaseOrganizeEntity dept in depts)
                        {
                            BaseOrganizeEntity tempDept = dept;
                            predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);
                        }
                        predicate = predicate.And(p => p.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE);
                        enumLbPunishs = entities.ZdtzCyPunishes.AsExpandable().Where(predicate);
                    }
                }
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_BROWSE))
                {
                    int curDeptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                    // 只查看自己中队的台账
                    enumLbPunishs = entities.ZdtzCyPunishes.Where
                            (
                                entity =>
                                    ((entity.OwnDepartmentId == curDeptId || entity.OwnDepartmentId == YcConstants.INT_COMPANY_ID)
                                    && entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                            );
                }

                return enumLbPunishs;
            }
        }

        public List<ZdtzCyPunishGatherTable> GetGatherPunishByDate(DateTime startDate, DateTime endDate)
        {
            List<ZdtzCyPunishGatherTable> gatherPunishes;
            using (var context = new yctrafficEntities())
            {

                string strSql = "select ";
                strSql += "OwnDepartmentName as DepartmentName, SUM(CommonProcedure) as CommonCnt, SUM(SimpleProcedure) as SimpleCnt ";
                strSql += "from ZdtzCyPunish where (PatrolDate >= @startDate and PatrolDate<= @endDate and IsDeleted = 0)";
                strSql += "group by OwnDepartmentName";

                var args = new DbParameter[] 
                {
                      new SqlParameter {ParameterName = "startDate", Value = startDate},
                      new SqlParameter {ParameterName = "endDate", Value = endDate.AddDays(1)}
                };

                gatherPunishes = context.ExecuteStoreQuery<ZdtzCyPunishGatherTable>(strSql, args).ToList<ZdtzCyPunishGatherTable>();

            }
            return gatherPunishes;
        }

        /// <summary>
        /// 常用台账类型:道路巡查情况
        /// </summary>
        public ObservableCollection<ZdtzCyPatrol> LbPatrols
        {
            get
            {
                if (lbPatrols == null && entities != null)
                {
                    lbPatrols = new EntityObservableCollection<ZdtzCyPatrol>(entities.ZdtzCyPatrols);
                }
                return lbPatrols;
            }
        }

        /// <summary>
        /// 常用台账类型:道路巡查情况
        /// </summary>
        public IQueryable<ZdtzCyPatrol> EnumLbPatrols
        {
            get
            {
                entities.ZdtzCyPatrols.MergeOption = MergeOption.OverwriteChanges;

                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    enumLbPatrols = entities.ZdtzCyPatrols.Where
                    (
                        entity => entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                    );

                }
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
                {
                    List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                    if (depts != null && depts.Count > 0)
                    {
                        var predicate = PredicateBuilder.False<ZdtzCyPatrol>();
                        foreach (BaseOrganizeEntity dept in depts)
                        {
                            BaseOrganizeEntity tempDept = dept;
                            predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);
                        }
                        predicate = predicate.And(p => p.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE);
                        enumLbPatrols = entities.ZdtzCyPatrols.AsExpandable().Where(predicate);
                    }
                }
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_BROWSE))
                {
                    int curDeptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                    // 只查看自己中队的台账
                    enumLbPatrols = entities.ZdtzCyPatrols.Where
                            (
                                entity =>
                                    ((entity.OwnDepartmentId == curDeptId || entity.OwnDepartmentId == YcConstants.INT_COMPANY_ID)
                                    && entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                            );
                }

                return enumLbPatrols;
            }
        }

        /// <summary>
        /// 常用台账类型:隐患排查
        /// </summary>
        public ObservableCollection<ZdtzCyDangerDeal> LbDangerDeals
        {
            get
            {
                if (lbDangerDeals == null && entities != null)
                {
                    lbDangerDeals = new EntityObservableCollection<ZdtzCyDangerDeal>(entities.ZdtzCyDangerDeals);
                }
                return lbDangerDeals;
            }
        }

        /// <summary>
        /// 常用台账类型:隐患排查
        /// </summary>
        public IQueryable<ZdtzCyDangerDeal> EnumLbDangerDeals(int type)
        {
            entities.ZdtzCyDangerDeals.MergeOption = MergeOption.OverwriteChanges;
            int curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
            if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
            {
                enumLbDangerDeals = entities.ZdtzCyDangerDeals.Where
                (
                    entity => 
                        (entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                        && entity.LogbookType == type)
                );

            }
            else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
            {
                List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                if (depts != null && depts.Count > 0)
                {
                    var predicate = PredicateBuilder.False<ZdtzCyDangerDeal>();
                    foreach (BaseOrganizeEntity dept in depts)
                    {
                        BaseOrganizeEntity tempDept = dept;
                        predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);
                    }
                    predicate = predicate.Or(p => p.SubLeaderId == curUserId);
                    predicate = predicate.And(p => (p.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE && p.LogbookType == type));
                    enumLbDangerDeals = entities.ZdtzCyDangerDeals.AsExpandable().Where(predicate);
                }
            }
            else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_BROWSE))
            {
                int curDeptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                // 只查看自己中队的台账
                enumLbDangerDeals = entities.ZdtzCyDangerDeals.Where
                        (
                            entity =>
                                ((entity.OwnDepartmentId == curDeptId
                                || entity.SubLeaderId == curUserId
                                || entity.OwnDepartmentId == YcConstants.INT_COMPANY_ID)
                                && entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                                && entity.LogbookType == type)
                        );
            }

            return enumLbDangerDeals;
        }


        /// <summary>
        /// 宣传台账
        /// </summary>
        public IQueryable<ZgxcPublicityLogbook> QueryablePublicityLogbooks
        {
            get
            {
                entities.ZgxcPublicityLogbooks.MergeOption = MergeOption.OverwriteChanges;
                queryablePublicityLogbooks = entities.ZgxcPublicityLogbooks.Where
                    (
                        entity => entity.IsDeleted == false
                    );

                return queryablePublicityLogbooks;
            }
        }

        /// <summary>
        /// 协勤考察
        /// </summary>
        public IQueryable<ZgxcAssistantCheckin> QueryableAssistantCheckins
        {
            get
            {
                entities.ZgxcAssistantCheckins.MergeOption = MergeOption.OverwriteChanges;
                queryableAssistantCheckins = entities.ZgxcAssistantCheckins.Where
                    (
                        entity => entity.IsDeleted == false
                    );

                return queryableAssistantCheckins;
            }
        }

        public IQueryable<ZgxcPersonnelChange> QueryZgxcPersonnelChanges
        {
            get
            {
                entities.ZgxcPersonnelChanges.MergeOption = MergeOption.OverwriteChanges;
                queryZgxcPersonnelChanges = entities.ZgxcPersonnelChanges.Where
                    (
                        entity => entity.IsDeleted == false
                    );

                return queryZgxcPersonnelChanges;
            }
        }


        public IQueryable<ZgxcAskForLeave> QueryZgxcAskForLeaves
        {
            get
            {
                entities.ZgxcAskForLeaves.MergeOption = MergeOption.OverwriteChanges;
                queryZgxcAskForLeaves = entities.ZgxcAskForLeaves.Where
                    (
                        entity => entity.IsDeleted == false
                    );

                return queryZgxcAskForLeaves;
            }
        }

        public IQueryable<ZgxcAskForLeave> QueryZgxcWaitingForApprovalLeaves(string approver)
        {
            var results = from u in entities.ZgxcAskForLeaves
                          where (String.IsNullOrEmpty(u.ApproveComments)
                          && u.IsDeleted == false
                          && u.ApproverName == approver)
                          select u;

            return results;
        }


        /// <summary>
        /// 材料报送类型
        /// </summary>
        public IQueryable<MaterialDeclareTable> EnumMaterialDeclares
        {
            get
            {
                entities.MaterialDeclareTables.MergeOption = MergeOption.OverwriteChanges;
                enumMaterialDeclares = entities.MaterialDeclareTables.Where
                    (
                        entity => entity.IsDeleted == 0
                    );

                return enumMaterialDeclares;
            }
        }

        /// <summary>
        /// 材料申报类型
        /// </summary>
        public IQueryable<MaterialDeclareTable> EnumNoApproveMaterialDeclares
        {
            get
            {
                enumNoApproveMaterialDeclares = entities.MaterialDeclareTables.Where
                    (
                        entity => (entity.IsDeleted == 0 && entity.Score == null)
                    );

                return enumNoApproveMaterialDeclares;
            }
        }

        /// <summary>
        /// 大队长未读的情报舆情
        /// </summary>
        public IQueryable<QbyqInfoAnalysisCase> NonReadQbyqInfo_DDZ
        {
            get
            {
                nonReadQbyqInfo_DDZ = entities.QbyqInfoAnalysisCases.Where
                    (
                        entity => (entity.IsRead1 == false && entity.IsDeleted == false)
                    );

                return nonReadQbyqInfo_DDZ;
            }
        }

        /// <summary>
        /// 政委未读的情报舆情
        /// </summary>
        public IQueryable<QbyqInfoAnalysisCase> NonReadQbyqInfo_ZW
        {
            get
            {
                nonReadQbyqInfo_ZW = entities.QbyqInfoAnalysisCases.Where
                    (
                        entity => (entity.IsRead2 == false && entity.IsDeleted == false)
                    );

                return nonReadQbyqInfo_ZW;
            }
        }

        /// <summary>
        /// 中队人员基本信息台账
        /// </summary>
        public ObservableCollection<ZdtzZdStaffInfo> LbStaffInfos
        {
            get
            {
                if (lbStaffInfos == null && entities != null)
                {
                    lbStaffInfos = new EntityObservableCollection<ZdtzZdStaffInfo>(entities.ZdtzZdStaffInfoes);
                }
                return lbStaffInfos;
            }
        }

        /// <summary>
        /// 中队台账类型
        /// </summary>
        public IQueryable <ZdtzZdStaffInfo> EnumLbStaffInfos
        {
            get
            {
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    enumLbStaffInfos = entities.ZdtzZdStaffInfoes.Where
                    (
                        entity => entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                    );

                }
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
                {
                    List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                    if (depts != null && depts.Count > 0)
                    {
                        var predicate = PredicateBuilder.False<ZdtzZdStaffInfo>();
                        foreach (BaseOrganizeEntity dept in depts)
                        {
                            BaseOrganizeEntity tempDept = dept;
                            predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);
                        }
                        predicate = predicate.And(p => p.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE);
                        enumLbStaffInfos = entities.ZdtzZdStaffInfoes.AsExpandable().Where(predicate);
                    }
                }
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_BROWSE))
                {
                    int curDeptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                    // 只查看自己中队的台账
                    enumLbStaffInfos = entities.ZdtzZdStaffInfoes.Where
                            (
                                entity =>
                                    ((entity.OwnDepartmentId == curDeptId || entity.OwnDepartmentId == YcConstants.INT_COMPANY_ID)
                                    && entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                            );
                }
                return enumLbStaffInfos;
            }
        }

        /// <summary>
        /// 静态台账
        /// </summary>
        public ObjectSet<ZdtzStaticTable> LbStatics
        {
            get
            {
                if (lbStatics == null && entities != null)
                {
                    lbStatics = entities.ZdtzStaticTables;
                }
                return lbStatics;
            }
        }

        /// <summary>
        /// 台账配置
        /// </summary>
        public IQueryable<ZdtzConfigTable> EnumLbConfigs
        {
            get
            {
                entities.ZdtzConfigTables.MergeOption = MergeOption.OverwriteChanges;
                enumLbConfigs = entities.ZdtzConfigTables.Where
                (
                    entity => (entity.LogbookType == YcConstants.INT_LBCONFIG_TYPE_NORMAL 
                        && entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                );
                return enumLbConfigs;
            }
        }

        #endregion

        #region Zhzx

        //public ObjectSet<ZhzxTrafficViolation> ZhzxTrafficViolations
        //{
        //    get
        //    {
        //        if (zhzxTrafficViolations == null && entities != null)
        //        {
        //            zhzxTrafficViolations = entities.ZhzxTrafficViolations;
        //        }
        //        return zhzxTrafficViolations;
        //    }
        //}

        public IQueryable<ZhzxTrafficViolation> EnumZhzxTrafficViolations
        {
            get
            {
                enumZhzxTrafficViolations = entities.ZhzxTrafficViolations;
                return enumZhzxTrafficViolations;
            }
        }

        public IQueryable<ZhzxTrafficViolation> QueryableZhzxFakePlateViolations
        {
            get
            {
                queryableZhzxFakePlateViolations = entities.ZhzxTrafficViolations.Where
                (
                    entity => entity.IsFakeNumber == true
                );
                return queryableZhzxFakePlateViolations;
            }
        }

        public IQueryable<ZhzxPicture> QueryableZhzxPicture
        {
            get
            {
                return entities.ZhzxPictures;
            }
        }

        public IQueryable<ZhzxThumbnail> QueryableZhzxThumbnail
        {
            get
            {
                return entities.ZhzxThumbnails;
            }
        }

        public IQueryable<ZhzxOfficeSupplyStock> QueryableZhzxOfficeSupplyStock
        {
            get
            {
                entities.ZhzxOfficeSupplyStocks.MergeOption = MergeOption.OverwriteChanges;
                queryableZhzxOfficeSupplyStock = entities.ZhzxOfficeSupplyStocks.Where
                (
                    entity => entity.IsDeleted == false
                );
                return queryableZhzxOfficeSupplyStock;
            }
        }

        public IQueryable<ZhzxElectronMonitor> QueryableZhzxElectronMonitor
        {
            get
            {
                entities.ZhzxElectronMonitors.MergeOption = MergeOption.OverwriteChanges;
                queryableZhzxElectronMonitor = entities.ZhzxElectronMonitors.Where
                (
                    entity => entity.IsDeleted == false
                );
                return queryableZhzxElectronMonitor;
            }
        }

        public IQueryable<ZhzxFixedAssetsRegister> QueryableZhzxFixedAssetsRegister
        {
            get
            {
                entities.ZhzxFixedAssetsRegisters.MergeOption = MergeOption.OverwriteChanges;
                queryableZhzxFixedAssetsRegister = entities.ZhzxFixedAssetsRegisters.Where
                (
                    entity => entity.IsDeleted == false
                );
                return queryableZhzxFixedAssetsRegister;
            }
        }

        public IQueryable<ZhzxTotalViolation> QueryableZhzxTotalViolation
        {
            get
            {
                entities.ZhzxTotalViolations.MergeOption = MergeOption.OverwriteChanges;
                queryableZhzxTotalViolation = entities.ZhzxTotalViolations.Where
                (
                    entity => entity.IsDeleted == false
                );
                return queryableZhzxTotalViolation;
            }
        }



        public ObservableCollection<ZhzxRedNameList> ZhzxRedNameLists
        {
            get
            {
                if (zhzxRedNameLists == null && entities != null)
                {
                    zhzxRedNameLists = new EntityObservableCollection<ZhzxRedNameList>(entities.ZhzxRedNameLists);
                }
                return zhzxRedNameLists;
            }
        }

        public IQueryable<ZhzxRedNameList> EnumZhzxRedNameLists
        {
            get
            {
                enumZhzxRedNameLists = entities.ZhzxRedNameLists.Where
                (
                    entity => entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                );
                return enumZhzxRedNameLists;
            }
        }

        /*
        public ObservableCollection<ZhzxEquipmentRequest> ZhzxEquipmentRequests
        {
            get
            {
                if (zhzxEquipmentRequests == null && entities != null)
                {
                    zhzxEquipmentRequests = new EntityObservableCollection<ZhzxEquipmentRequest>(entities.ZhzxEquipmentRequests);
                }
                return zhzxEquipmentRequests;
            }
        }*/

        public IQueryable<ZhzxEquipmentRequest> EnumZhzxEquipmentRequests
        {
            get
            {
                entities.ZhzxEquipmentRequests.MergeOption = MergeOption.OverwriteChanges;
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_ZHZX_REQUEST_LISTALL))
                {
                    enumZhzxEquipmentRequests = entities.ZhzxEquipmentRequests.Where
                    (
                        entity => entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                    );

                }
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_ZHZX_REQUEST_BROWSE))
                {
                    int curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    enumZhzxEquipmentRequests = entities.ZhzxEquipmentRequests.Where
                            (
                                entity =>
                                    ((entity.CreateId == curUserId || entity.SubLeaderId == curUserId)
                                    && entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                            );
                }

                return enumZhzxEquipmentRequests;
            }
        }

        #endregion

        #region Kjss

        public IQueryable<KjssEquipmentRequest> EnumKjssEquipmentRequests
        {
            get
            {
                entities.KjssEquipmentRequests.MergeOption = MergeOption.OverwriteChanges;
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_KJK_REQUEST_LISTALL))
                {
                    enumKjssEquipmentRequests = entities.KjssEquipmentRequests.Where
                    (
                        entity => entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                    );

                }
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_KJK_REQUEST_BROWSE))
                {
                    int curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    enumKjssEquipmentRequests = entities.KjssEquipmentRequests.Where
                            (
                                entity =>
                                    ((entity.CreateId == curUserId || entity.SubLeaderId == curUserId)
                                    && entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                            );
                }

                return enumKjssEquipmentRequests;
            }
        }

        public IQueryable<SskEquipmentRequest> EnumSskEquipmentRequests
        {
            get
            {
                entities.SskEquipmentRequests.MergeOption = MergeOption.OverwriteChanges;
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_SSK_REQUEST_LISTALL))
                {
                    enumSskEquipmentRequests = entities.SskEquipmentRequests.Where
                    (
                        entity => entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                    );

                }
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_SSK_REQUEST_BROWSE))
                {
                    int curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    enumSskEquipmentRequests = entities.SskEquipmentRequests.Where
                            (
                                entity =>
                                    ((entity.CreateId == curUserId || entity.SubLeaderId == curUserId)
                                    && entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                            );
                }

                return enumSskEquipmentRequests;
            }
        }

        #endregion

        #region Fzk

        public IQueryable<FzkPunishCase> EnumFzkPunishCases
        {
            get
            {
                entities.FzkPunishCases.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    enumFzkPunishCases = entities.FzkPunishCases.Where(u => u.IsDeleted == false);

                }
                return enumFzkPunishCases;
            }
        }

        public IQueryable<FzkPetition> EnumFzkPetitionCases
        {
            get
            {
                entities.FzkPetitions.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    enumFzkPetitionCases = entities.FzkPetitions.Where(u => u.IsDeleted == false);

                }
                return enumFzkPetitionCases;
            }
        }

        public IQueryable<FzkChangeMeasure> QueryFzkChangeMeasures
        {
            get
            {
                entities.FzkChangeMeasures.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    queryFzkChangeMeasures = entities.FzkChangeMeasures.Where(u => u.IsDeleted == false);

                }
                return queryFzkChangeMeasures;
            }
        }

        public IQueryable<FzkConsultation> QueryFzkConsultations
        {
            get
            {
                entities.FzkConsultations.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    queryFzkConsultations = entities.FzkConsultations.Where(u => u.IsDeleted == false);

                }
                return queryFzkConsultations;
            }
        }

        public IQueryable<FzkReleaseCar> QueryFzkReleaseCars
        {
            get
            {
                entities.FzkReleaseCars.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    queryFzkReleaseCars = entities.FzkReleaseCars.Where(u => u.IsDeleted == false);

                }
                return queryFzkReleaseCars;
            }
        }

        public IQueryable<FzkLawQualityLogbook> QueryableLawQualityLogbooks
        {
            get
            {
                entities.FzkLawQualityLogbooks.MergeOption = MergeOption.OverwriteChanges;
                queryableLawQualityLogbooks = entities.FzkLawQualityLogbooks.Where
                    (
                        entity => entity.IsDeleted == false
                    );

                return queryableLawQualityLogbooks;
            }
        }

        #endregion

        #endregion

        #region Member

        #region SquadronLogbookMember

        public IQueryable<ZdtzZdStaffInfo> QueryStaffInfos(int deptId, DateTime startDate, DateTime endDate)
        {

            var results = from u in entities.ZdtzZdStaffInfoes
                            where u.RecordTime >= startDate && u.RecordTime <= endDate && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                            select u;
            if (deptId > 0)
            {
                results = results.Where(p => p.OwnDepartmentId == deptId);
            }
            else if (deptId == 0 && CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
            {
                List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                if (depts != null && depts.Count > 0)
                {
                    var predicate = PredicateBuilder.False<ZdtzZdStaffInfo>();
                    foreach (BaseOrganizeEntity dept in depts)
                    {
                        BaseOrganizeEntity tempDept = dept;
                        predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);
                    }
                    results = results.AsExpandable().Where(predicate);
                }

            }

            return results;
        }

        /**
         * 常用台帐:每天处罚情况
         * 
         */
        public IQueryable<ZdtzCyPunish> QueryPunishs(int deptId, DateTime startDate, DateTime endDate)
        {
            var results = from u in entities.ZdtzCyPunishes
                          where u.PatrolDate >= startDate && u.PatrolDate <= SqlFunctions.DateAdd("day", 1, endDate) && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                          select u;
            if (deptId > 0)
            {
                results = results.Where(p => p.OwnDepartmentId == deptId);
            }
            else if (deptId == 0 && CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
            {
                List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                if (depts != null && depts.Count > 0)
                {
                    var predicate = PredicateBuilder.False<ZdtzCyPunish>();
                    foreach (BaseOrganizeEntity dept in depts)
                    {
                        BaseOrganizeEntity tempDept = dept;
                        predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);
                    }
                    results = results.AsExpandable().Where(predicate);
                }
            }

            return results;
        }

        /**
         * 常用台帐:道路巡查情况
         * 
         */
        public IQueryable<ZdtzCyPatrol> QueryPatrols(int deptId, DateTime startDate, DateTime endDate)
        {
            var results = from u in entities.ZdtzCyPatrols
                          where u.PatrolDate >= startDate && u.PatrolDate <= SqlFunctions.DateAdd("day", 1, endDate) && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                          select u;
            if (deptId > 0)
            {
                results = results.Where(p => p.OwnDepartmentId == deptId);
            }
            else if (deptId == 0 && CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
            {
                List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                if (depts != null && depts.Count > 0)
                {
                    var predicate = PredicateBuilder.False<ZdtzCyPatrol>();
                    foreach (BaseOrganizeEntity dept in depts)
                    {
                        BaseOrganizeEntity tempDept = dept;
                        predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);
                    }
                    results = results.AsExpandable().Where(predicate);
                }
            }

            return results;
        }

        /**
         * 常用台帐:隐患排查情况
         * 
         */
        public IQueryable<ZdtzCyDangerDeal> QueryDangerDeals(int deptId, DateTime startDate, DateTime endDate, int type, string title)
        {
            int curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
            entities.ZdtzCyDangerDeals.MergeOption = MergeOption.OverwriteChanges;

            var results = from u in entities.ZdtzCyDangerDeals
                          where (((u.HappenDate >= startDate && u.HappenDate <= SqlFunctions.DateAdd("day", 1, endDate)) 
                          || u.SubLeaderId == curUserId)
                          && u.LogbookType == type
                          && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                          select u;
            if (deptId > 0)
            {
                results = results.Where(p => p.OwnDepartmentId == deptId);
            }
            else if (deptId == 0 && CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
            {
                List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                if (depts != null && depts.Count > 0)
                {
                    var predicate = PredicateBuilder.False<ZdtzCyDangerDeal>();
                    foreach (BaseOrganizeEntity dept in depts)
                    {
                        BaseOrganizeEntity tempDept = dept;
                        predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);
                    }
                    results = results.AsExpandable().Where(predicate);
                }
            }

            if (!string.IsNullOrEmpty(title))
            {
                results = results.Where(p => p.Location.Contains(title));
            }

            return results;
        }

        public IQueryable<ZdtzStaticTable> EnumLbStatics(int logbookTypeId)
        {
            IQueryable<ZdtzStaticTable> queryResults = null;

            entities.ZdtzStaticTables.MergeOption = MergeOption.OverwriteChanges;
            if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
            {
                queryResults = from u in entities.ZdtzStaticTables
                               where u.ConfigId == logbookTypeId && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                               select u;

            }
            else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
            {
                List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                if (depts != null && depts.Count > 0)
                {
                    var predicate = PredicateBuilder.False<ZdtzStaticTable>();
                    foreach (BaseOrganizeEntity dept in depts)
                    {
                        BaseOrganizeEntity tempDept = dept;
                        predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);
                    }
                    predicate = predicate.And(p => p.ConfigId == logbookTypeId);
                    predicate = predicate.And(p => p.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE);
                    queryResults = entities.ZdtzStaticTables.AsExpandable().Where(predicate);
                }
            }
            else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_BROWSE))
            {
                int curDeptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                // 只查看自己中队的台账
                queryResults = from u in entities.ZdtzStaticTables
                               where u.ConfigId == logbookTypeId 
                               && (u.OwnDepartmentId == curDeptId || u.OwnDepartmentId == YcConstants.INT_COMPANY_ID) 
                               && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                               select u;
            }

            queryResults = queryResults.OrderByDescending(entity => entity.RecordDate);
            return queryResults;
        }

        public IQueryable<ZdtzStaticTable> QueryStatics(int logbookTypeId, int deptId, DateTime startDate, DateTime endDate, string title)
        {
            IQueryable<ZdtzStaticTable> queryResults = null;
            entities.ZdtzStaticTables.MergeOption = MergeOption.OverwriteChanges;
            var results = from u in entities.ZdtzStaticTables
                          where u.ConfigId == logbookTypeId && u.RecordDate >= startDate && u.RecordDate <= endDate && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                          select u;
            if (deptId > 0)
            {
                results = results.Where(p => p.OwnDepartmentId == deptId);
            }
            else if (deptId == 0 && CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))
            {
                List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                if (depts != null && depts.Count > 0)
                {
                    var predicate = PredicateBuilder.False<ZdtzStaticTable>();
                    foreach (BaseOrganizeEntity dept in depts)
                    {
                        BaseOrganizeEntity tempDept = dept;
                        predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);
                    }
                    results = results.AsExpandable().Where(predicate);
                }

            }

            if (!string.IsNullOrEmpty(title))
            {
                results = results.Where(p => p.Title.Contains(title));
            }

            queryResults = results.OrderByDescending(entity => entity.RecordDate);
            return queryResults;
        }

        public IQueryable<ZdtzCyDangerDeal> QueryZdtzOverTimeCyDangerDeal()
        {
            DateTime dtDeadLine = DateTime.Today.AddDays(1);

            var results = from u in entities.ZdtzCyDangerDeals
                            where (u.Status == YcConstants.INT_DANGER_DEAL_SATUS_WAIT_FOR_VERIFY)
                            && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                            && dtDeadLine >= u.DealDate
                            select u;

            return results;
        }

        public List<ZdtzConfigTable> QueryConfigParentNodes()
        {
            List<ZdtzConfigTable> queryResults = null;

            entities.ZdtzConfigTables.MergeOption = MergeOption.OverwriteChanges;
            var results = from u in entities.ZdtzConfigTables
                          where u.NodeLevel == YcConstants.INT_LBCONFIG_NODE_LEVEL_1 
                          && u.LogbookType == YcConstants.INT_LBCONFIG_TYPE_NORMAL
                          && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                          orderby u.Code
                          select u;

            if (results != null && results.Count() > 0) queryResults = results.ToList();
            return queryResults;
        }

        public int QueryConfigMaxId()
        {
            int maxId = 0;

            var results = (from u in entities.ZdtzConfigTables
                          orderby u.Id descending
                          select u.Id).Take(1);

            if (results != null && results.Count() > 0) maxId = results.First();
            return maxId;
        }

        public string QueryConfigMaxCodeByParent(int parentId)
        {
            string code = string.Empty;

            var results = (from u in entities.ZdtzConfigTables
                           where u.ParentId == parentId && u.LogbookType == YcConstants.INT_LBCONFIG_TYPE_NORMAL
                           orderby u.Code descending
                           select u.Code).Take(1);

            if (results != null && results.Count() > 0) code = results.First();
            return code;
        }

        public int QueryConfigUsingCount(int parentId)
        {
            int count = 0;

            count = entities.ZdtzConfigTables.Count(u => u.ParentId == parentId && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE);
            return count;
        }

        public int QueryStaticUsingCount(int id)
        {
            int count = 0;

            count = entities.ZdtzStaticTables.Count(u => u.ConfigId == id && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE);
            return count;
        }

        public IQueryable<ZdtzConfigTable> QueryLbConfigs(string title)
        {
            entities.ZdtzConfigTables.MergeOption = MergeOption.OverwriteChanges;
            var results = from u in entities.ZdtzConfigTables
                          where u.LogbookType == YcConstants.INT_LBCONFIG_TYPE_NORMAL && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                          select u;

            if (!ValidateUtil.IsBlank(title))
            {
                results = results.Where(p => p.Title.Contains(title)).OrderBy(p => p.Code);
            }

            return results;
        }

        #endregion

        #region ZHZX

        //public List<ZhzxThumbnail> GetThumbnailByCode(string Code)
        //{
        //    List<ZhzxThumbnail> zhzxThumbnail;

        //    using (var context = new ycpictureEntities())
        //    {
        //        string strSql = "select * from ZhzxThumbnail where Code = @Code";


        //        var args = new DbParameter[] 
        //        {
        //              new SqlParameter {ParameterName = "Code", Value = Code}
        //        };

        //        zhzxThumbnail = context.ExecuteStoreQuery<ZhzxThumbnail>(strSql, args).ToList<ZhzxThumbnail>();

        //    }
        //    if (zhzxThumbnail == null)
        //    {
        //        zhzxThumbnail = new List<ZhzxThumbnail>();
        //    }
        //    return zhzxThumbnail;
        //}

        //public List<ZhzxPicture> GetPictureByCode(string Code)
        //{
        //    List<ZhzxPicture> zhzxPicture;

        //    using (var context = new ycpictureEntities())
        //    {
        //        string strSql = "select * from ZhzxPicture where Code = @Code";


        //        var args = new DbParameter[] 
        //        {
        //              new SqlParameter {ParameterName = "Code", Value = Code}
        //        };

        //        zhzxPicture = context.ExecuteStoreQuery<ZhzxPicture>(strSql, args).ToList<ZhzxPicture>();

        //    }
        //    if (zhzxPicture == null)
        //    {
        //        zhzxPicture = new List<ZhzxPicture>();
        //    }
        //    return zhzxPicture;
        //}

        ////public Byte[] GetComposedPictureByCode(string Code)
        ////{
        ////    List<Byte[]> composedPictureList;
        ////    Byte[] composedPicture;

        ////    using (var context = new ycpictureEntities())
        ////    {
        ////        string strSql = "select ComposedPicture from ZhzxPicture where Code = @Code";


        ////        var args = new DbParameter[] 
        ////        {
        ////              new SqlParameter {ParameterName = "Code", Value = Code}
        ////        };

        ////        composedPictureList = context.ExecuteStoreQuery<Byte[]>(strSql, args).ToList<Byte[]>();

        ////        if (composedPictureList.Count != 0)
        ////        {
        ////            composedPicture = composedPictureList[0];
        ////        }
        ////        else
        ////        {
        ////            composedPicture = null;
        ////        }
        ////    }

        ////    return composedPicture;
        ////}

        //public int GetPictureCount()
        //{
        //    int cnt;

        //    using (var context = new ycpictureEntities())
        //    {

        //        string strSql = "select count(*) from ZhzxPicture";

        //        cnt = context.ExecuteStoreQuery<int>(strSql).ToList()[0];

        //    }

        //    return cnt;
        //}

        //public void RemoveOldestPicture(int cnt)
        //{
        //    using (var context = new ycpictureEntities())
        //    {

        //        string strSql = 
        //            string.Format("delete from ZhzxPicture where Id in (select top {0} Id from ZhzxPicture order by CreateTime)", cnt);

        //        //var args = new DbParameter[] 
        //        //{
        //        //      new SqlParameter {ParameterName = "cnt", Value = cnt}
        //        //};

        //        context.ExecuteStoreQuery<object>(strSql);
        //    }
        //}

        //public void RemoveOldestThumbnail(int cnt)
        //{
        //    using (var context = new ycpictureEntities())
        //    {
        //        string strSql = 
        //            string.Format("delete from ZhzxThumbnail where Id in (select top {0} Id from ZhzxThumbnail order by CreateTime)", cnt);

        //        //string strSql = "delete from ZhzxThumbnail where Id in (select top @cnt Id from ZhzxThumbnail order by CreateTime)";

        //        //var args = new DbParameter[] 
        //        //{
        //        //      new SqlParameter {ParameterName = "cnt", Value = cnt}
        //        //};

        //        context.ExecuteStoreQuery<object>(strSql);
        //    }
        //}


        //public void InsertTrafficViolationPicture(ZhzxPicture picEntity, ycpictureEntities pictureContext)
        //{       
        //    string strSql = "Insert into ZhzxPicture (Code, ComposedPicture, SourcePicture1,";
        //    strSql += "SourcePicture2, SourcePicture3, IsDeleted, UpdaterId, UpdateTime, CreateId, CreateTime)";
        //    strSql += "values(";
        //    strSql += "@Code,";
        //    strSql += "@ComposedPicture,";
        //    strSql += "@SourcePicture1,";
        //    strSql += "@SourcePicture2,";
        //    strSql += "@SourcePicture3,";
        //    strSql += "@IsDeleted,";
        //    strSql += "@UpdaterId,";
        //    strSql += "@UpdateTime,";
        //    strSql += "@CreateId,";
        //    strSql += "@CreateTime)";

        //    var args = new DbParameter[] 
        //    {
        //        //new SqlParameter {ParameterName = "Code", Value = picEntity.Code},
        //        //new SqlParameter {ParameterName = "ComposedPicture", Value = (picEntity.ComposedPicture == null)? new byte[0]: picEntity.ComposedPicture},
        //        //new SqlParameter {ParameterName = "SourcePicture1", Value = (picEntity.SourcePicture1 == null)? new byte[0]: picEntity.SourcePicture1},
        //        //new SqlParameter {ParameterName = "SourcePicture2", Value = (picEntity.SourcePicture2 == null)? new byte[0]: picEntity.SourcePicture2},
        //        //new SqlParameter {ParameterName = "SourcePicture3", Value = (picEntity.SourcePicture3 == null)? new byte[0]: picEntity.SourcePicture3},
        //        //new SqlParameter {ParameterName = "IsDeleted", Value = picEntity.IsDeleted},
        //        //new SqlParameter {ParameterName = "UpdaterId", Value = picEntity.UpdaterId},
        //        //new SqlParameter {ParameterName = "UpdateTime", Value = picEntity.UpdateTime},
        //        //new SqlParameter {ParameterName = "CreateId", Value = picEntity.CreateId},
        //        //new SqlParameter {ParameterName = "CreateTime", Value = picEntity.CreateTime}
        //    };

        //    foreach (DbParameter p in args)
        //    {
        //        if (p.Value == null)
        //        {
        //            p.Value = DBNull.Value;
        //        }
        //    }

        //    pictureContext.ExecuteStoreCommand(strSql, args);
            
        //}

        //public void InsertTrafficViolationThumbnail(ZhzxThumbnail thumEntity, ycpictureEntities pictureContext)
        //{
        //    string strSql = "Insert into ZhzxThumbnail (Code, ComposedThumbnail, SourceThumbnail1,";
        //    strSql += "SourceThumbnail2, SourceThumbnail3, IsDeleted, UpdaterId, UpdateTime, CreateId, CreateTime)";
        //    strSql += "values(";
        //    strSql += "@Code,";
        //    strSql += "@ComposedThumbnail,";
        //    strSql += "@SourceThumbnail1,";
        //    strSql += "@SourceThumbnail2,";
        //    strSql += "@SourceThumbnail3,";
        //    strSql += "@IsDeleted,";
        //    strSql += "@UpdaterId,";
        //    strSql += "@UpdateTime,";
        //    strSql += "@CreateId,";
        //    strSql += "@CreateTime)";

        //    var args = new DbParameter[] 
        //    {
        //        //new SqlParameter {ParameterName = "Code", Value = thumEntity.Code},
        //        //new SqlParameter {ParameterName = "ComposedThumbnail", Value = (thumEntity.ComposedThumbnail == null)? new byte[0]: thumEntity.ComposedThumbnail},
        //        //new SqlParameter {ParameterName = "SourceThumbnail1", Value = (thumEntity.SourceThumbnail1 == null)? new byte[0]: thumEntity.SourceThumbnail1},
        //        //new SqlParameter {ParameterName = "SourceThumbnail2", Value = (thumEntity.SourceThumbnail2 == null)? new byte[0]: thumEntity.SourceThumbnail2},
        //        //new SqlParameter {ParameterName = "SourceThumbnail3", Value = (thumEntity.SourceThumbnail3 == null)? new byte[0]: thumEntity.SourceThumbnail3},
        //        //new SqlParameter {ParameterName = "IsDeleted", Value = thumEntity.IsDeleted},
        //        //new SqlParameter {ParameterName = "UpdaterId", Value = thumEntity.UpdaterId},
        //        //new SqlParameter {ParameterName = "UpdateTime", Value = thumEntity.UpdateTime},
        //        //new SqlParameter {ParameterName = "CreateId", Value = thumEntity.CreateId},
        //        //new SqlParameter {ParameterName = "CreateTime", Value = thumEntity.CreateTime}
        //    };

        //    foreach (DbParameter p in args)
        //    {
        //        if (p.Value == null)
        //        {
        //            p.Value = DBNull.Value;
        //        }
        //    }

        //    pictureContext.ExecuteStoreCommand(strSql, args);
        //}


        public List<ZhzxViolationGatherTable> GetViolationUploadGatherByCaptureTime(DateTime startDate, DateTime endDate)
        {
            List<ZhzxViolationGatherTable> violationGather;
            using (var context = new yctrafficEntities())
            {

                string  strSql = "select UploadName as Name,  WorkflowStatus, ViolationType,";
                        strSql += "COUNT (Id) as ViolationCnt from ZhzxTrafficViolation ";
                        strSql += "where UploadName is not null and CaptureTime >= @startDate and CaptureTime < @endDate ";
                        strSql += "group by UploadName,  WorkflowStatus, ViolationType order by UploadName"; 


                var args = new DbParameter[] 
                {
                      new SqlParameter {ParameterName = "startDate", Value = startDate},
                      new SqlParameter {ParameterName = "endDate", Value = endDate}
                };

                violationGather = context.ExecuteStoreQuery<ZhzxViolationGatherTable>(strSql, args).ToList<ZhzxViolationGatherTable>();

            }
            return violationGather;
        }

        public List<ZhzxViolationGatherTable> GetViolationApprovalGatherByCaptureTime(DateTime startDate, DateTime endDate)
        {
            List<ZhzxViolationGatherTable> violationGather;
            using (var context = new yctrafficEntities())
            {

                string strSql = "select ApprovalName as Name,  WorkflowStatus, ViolationType,";
                strSql += "COUNT (Id) as ViolationCnt from ZhzxTrafficViolation ";
                strSql += "where ApprovalName is not null and CaptureTime >= @startDate and CaptureTime < @endDate ";
                strSql += "group by ApprovalName,  WorkflowStatus, ViolationType order by ApprovalName";


                var args = new DbParameter[] 
                {
                      new SqlParameter {ParameterName = "startDate", Value = startDate},
                      new SqlParameter {ParameterName = "endDate", Value = endDate}
                };

                violationGather = context.ExecuteStoreQuery<ZhzxViolationGatherTable>(strSql, args).ToList<ZhzxViolationGatherTable>();

            }
            return violationGather;
        }

        public int ViolationCountPerLocateAndPlate(string checkPointName, string licensePlateNumber)
        {
            int cnt;

            using (var context = new yctrafficEntities())
            {

                string strSql = "select count(Id) from ZhzxTrafficViolation ";
                strSql += "where (CheckpointName = @checkPointName and LicensePlateNumber = @licensePlateNumber and WorkflowStatus = @approved)";

                var args = new DbParameter[] 
                {
                      new SqlParameter {ParameterName = "checkPointName", Value = checkPointName},
                      new SqlParameter {ParameterName = "licensePlateNumber", Value = licensePlateNumber},
                      new SqlParameter {ParameterName = "approved", Value = YcConstantTable.INT_ZHZX_WORKFLOW_APPROVED}
                };

                cnt = context.ExecuteStoreQuery<int>(strSql, args).ToList()[0];

            }

            return cnt;
        }



        public IQueryable<ZhzxRedNameList> QueryZhzxRedNameLists(string plateNumber)
        {

            var results = from u in entities.ZhzxRedNameLists
                          where u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                          select u;

            if (!ValidateUtil.IsBlank(plateNumber))
            {
                results = results.Where(p => p.LicensePlateNumber.Contains(plateNumber));
            }

            return results;
        }

        public IQueryable<ZhzxEquipmentRequest> QueryZhzxEquipmentRequests(int status, DateTime startDate, DateTime endDate)
        {
            IQueryable<ZhzxEquipmentRequest> queryResults = null;

            entities.ZhzxEquipmentRequests.MergeOption = MergeOption.OverwriteChanges;
            if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_ZHZX_REQUEST_BROWSE))
            {
                var results = from u in entities.ZhzxEquipmentRequests
                              where u.RequestTime >= startDate && u.RequestTime <= endDate && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                              select u;

                if (status > 0)
                {
                    results = results.Where(p => p.Status == status);
                }
                if (!CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_ZHZX_REQUEST_LISTALL))
                {
                    int curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    results = results.Where(p => (p.CreateId == curUserId || p.SubLeaderId == curUserId));
                }

                queryResults = results;
            }

            return queryResults;
        }

        public IQueryable<ZhzxEquipmentRequest> QueryZhzxOverTimeRequests()
        {
            DateTime dtDeadLine = DateTime.Today.AddDays(1);

            var results = from u in entities.ZhzxEquipmentRequests
                          where (u.Status == YcConstants.INT_ZHZX_REQSTAT_ZHZX_EXECUTE)
                          && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                          && dtDeadLine >= u.Deadline
                          select u;

            return results;
        }

        public IQueryable<ZhzxEquipmentRequest> ZhzxEquipReq_NeedDdzDeals()
        {
            var results = from u in entities.ZhzxEquipmentRequests
                          where (u.Status == YcConstants.INT_ZHZX_REQSTAT_DDZ_APPROVE
                          && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                          select u;

            return results;
        }

        #endregion

        #region Kjss

        public IQueryable<KjssEquipmentRequest> QueryKjssEquipmentRequests(int status, DateTime startDate, DateTime endDate)
        {
            IQueryable<KjssEquipmentRequest> queryResults = null;

            entities.KjssEquipmentRequests.MergeOption = MergeOption.OverwriteChanges;
            if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_KJK_REQUEST_BROWSE))
            {
                var results = from u in entities.KjssEquipmentRequests
                              where u.RequestTime >= startDate && u.RequestTime <= endDate && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                              select u;

                if (status > 0)
                {
                    results = results.Where(p => p.Status == status);
                }
                if (!CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_KJK_REQUEST_LISTALL))
                {
                    int curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    results = results.Where(p => (p.CreateId == curUserId || p.SubLeaderId == curUserId));
                }

                queryResults = results;
            }

            return queryResults;
        }

        public IQueryable<SskEquipmentRequest> QuerySskEquipmentRequests(int status, DateTime startDate, DateTime endDate)
        {
            IQueryable<SskEquipmentRequest> queryResults = null;

            entities.SskEquipmentRequests.MergeOption = MergeOption.OverwriteChanges;
            if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_SSK_REQUEST_BROWSE))
            {
                var results = from u in entities.SskEquipmentRequests
                              where u.RequestTime >= startDate && u.RequestTime <= endDate && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE
                              select u;

                if (status > 0)
                {
                    results = results.Where(p => p.Status == status);
                }
                if (!CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_SSK_REQUEST_LISTALL))
                {
                    int curUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    results = results.Where(p => (p.CreateId == curUserId || p.SubLeaderId == curUserId));
                }

                queryResults = results;
            }

            return queryResults;
        }

        public IQueryable<KjssEquipmentRequest> KjkEquipReq_NeedDdzDeals()
        {
            var results = from u in entities.KjssEquipmentRequests
                          where (u.Status == YcConstants.INT_KJSS_REQSTAT_DDZ_APPROVE
                          && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)
                          select u;

            return results;
        }

        #endregion

        #region Fzk

        public IQueryable<FzkPunishCase> QueryFzkPunishCases(DateTime startDate, DateTime endDate)
        {
            IQueryable<FzkPunishCase> results = from u in entities.FzkPunishCases
                          where u.CaseTime >= startDate && u.CaseTime <= endDate && u.IsDeleted == false
                          select u;

            return results;
        }

        public IQueryable<FzkPetition> QueryFzkPetitionCases(DateTime startDate, DateTime endDate)
        {
            IQueryable<FzkPetition> results = from u in entities.FzkPetitions
                                                where u.PetitionTime >= startDate && u.PetitionTime <= endDate && u.IsDeleted == false
                                                select u;

            return results;
        }

        //public IQueryable<FzkChangeMeasure> QueryFzkChangeMeasures(DateTime startDate, DateTime endDate)
        //{
        //    IQueryable<FzkChangeMeasure> results = from u in entities.FzkChangeMeasures
        //                                      where u.ApplyDate >= startDate && u.ApplyDate <= endDate && u.IsDeleted == false
        //                                      select u;

        //    return results;
        //}

        public IQueryable<FzkReleaseCar> FzkReleaseCarFdzDeals()
        {
            var results = from u in entities.FzkReleaseCars
                          where (u.ApproveResult == null && u.IsChargeSigned == false
                          && u.IsDeleted == false)
                          select u;

            return results;
        }

        #endregion

        #endregion

        #region Cgs

        public IQueryable<CgsVehicleAdminCase> EnumCgsVehicleAdminCases
        {
            get
            {
                entities.CgsVehicleAdminCases.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    enumCgsVehicleAdminCases = entities.CgsVehicleAdminCases.Where(u => u.IsDeleted == false);

                }
                return enumCgsVehicleAdminCases;
            }
        }

        public IQueryable<CgsVehicleAdminCase> QueryCgsVehicleAdminCases(DateTime startDate, DateTime endDate)
        {
            IQueryable<CgsVehicleAdminCase> results = from u in entities.CgsVehicleAdminCases
                                                where u.CaseTime >= startDate && u.CaseTime <= endDate && u.IsDeleted == false
                                                select u;

            return results;
        }


        public IQueryable<CgsYellowMarkCar> QueryCgsYellowMarkCars
        {
            get
            {
                entities.CgsYellowMarkCars.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    queryCgsYellowMarkCars = entities.CgsYellowMarkCars.Where(u => u.IsDeleted == false);

                }
                return queryCgsYellowMarkCars;
            }
        }

        public IQueryable<CgsKeyCompanyLogbook> QueryCgsKeyCompanyLogbooks
        {
            get
            {
                entities.CgsKeyCompanyLogbooks.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    queryCgsKeyCompanyLogbooks = entities.CgsKeyCompanyLogbooks.Where(u => u.IsDeleted == false);

                }
                return queryCgsKeyCompanyLogbooks;
            }
        }

        public IQueryable<CgsKeyDriverLogbook> QueryCgsKeyDriverLogbooks
        {
            get
            {
                entities.CgsKeyDriverLogbooks.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    queryCgsKeyDriverLogbooks = entities.CgsKeyDriverLogbooks.Where(u => u.IsDeleted == false);

                }
                return queryCgsKeyDriverLogbooks;
            }
        }

        public IQueryable<CgsKeyVehicleLogbook> QueryCgsKeyVehicleLogbooks
        {
            get
            {
                entities.CgsKeyVehicleLogbooks.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    queryCgsKeyVehicleLogbooks = entities.CgsKeyVehicleLogbooks.Where(u => u.IsDeleted == false);

                }
                return queryCgsKeyVehicleLogbooks;
            }
        }

        #endregion

        #region Gggs

        /// <summary>
        /// 公告公示类型
        /// </summary>
        public IQueryable<GggsPublishNotice> QueryableGggsPublishNotice
        {
            get
            {
                entities.GggsPublishNotices.MergeOption = MergeOption.OverwriteChanges;

                curUserId = int.Parse(CurrentLoginService.Instance.CurrentUserInfo.Id);

                if (CurrentLoginService.Instance.IsAuthorized(YcConstantTable.STR_AUTH_PUBLISHNOTICE_APPROVE))      //有审核权限的，可见
                {
                    queryableGggsPublishNotice = entities.GggsPublishNotices.Where
                        (
                            entity =>
                                (entity.IsDeleted == 0)

                        );
                }
                else
                {
                    queryableGggsPublishNotice = entities.GggsPublishNotices.Where
                        (
                            entity =>
                                (entity.IsDeleted == 0 &&
                                    (entity.Status == YcConstantTable.STR_PUBLISHNOTICE_APPROVED ||         // 审核通过，可见
                                        entity.CreateId == curUserId))  //创建人，可见
                        );
                }

                return queryableGggsPublishNotice;
            }
        }

        #endregion


        #region Qbyq

        public IQueryable<QbyqInfoAnalysisCase> EnumQbyqInfoAnalysisCases
        {
            get
            {
                entities.QbyqInfoAnalysisCases.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    enumQbyqInfoAnalysisCases = entities.QbyqInfoAnalysisCases.Where(u => u.IsDeleted == false);

                }
                return enumQbyqInfoAnalysisCases;
            }
        }

        public IQueryable<QbyqInfoAnalysisCase> QueryQbyqInfoAnalysisCases(DateTime startDate, DateTime endDate)
        {
            IQueryable<QbyqInfoAnalysisCase> results = from u in entities.QbyqInfoAnalysisCases
                                               where u.CaseTime >= startDate && u.CaseTime <= endDate && u.IsDeleted == false
                                               select u;

            return results;
        }

        #endregion

        #region Sgk

        public IQueryable<SgkAccidentCase> EnumSgkAccidentCases
        {
            get
            {
                entities.SgkAccidentCases.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    enumSgkAccidentCases = entities.SgkAccidentCases.Where(u => u.IsDeleted == false);

                }
                return enumSgkAccidentCases;
            }
        }

        public IQueryable<SgkAccidentCase> QuerySgkAccidentCases(DateTime startDate, DateTime endDate)
        {
            IQueryable<SgkAccidentCase> results = from u in entities.SgkAccidentCases
                                                      where u.CaseTime >= startDate && u.CaseTime <= endDate && u.IsDeleted == false
                                                      select u;

            return results;
        }

        public IQueryable<SgkReleaseCar> QuerySgkReleaseCars
        {
            get
            {
                entities.SgkReleaseCars.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    querySgkReleaseCars = entities.SgkReleaseCars.Where(u => u.IsDeleted == false);

                }
                return querySgkReleaseCars;
            }
        }

        #endregion

        #region Zxk

        public IQueryable<ZxkOrderCase> EnumZxkOrderCases
        {
            get
            {
                entities.ZxkOrderCases.MergeOption = MergeOption.OverwriteChanges;
                //if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))
                {
                    enumZxkOrderCases = entities.ZxkOrderCases.Where(u => u.IsDeleted == false);

                }
                return enumZxkOrderCases;
            }
        }

        public IQueryable<ZxkOrderCase> QueryZxkOrderCases(DateTime startDate, DateTime endDate)
        {
            IQueryable<ZxkOrderCase> results = from u in entities.ZxkOrderCases
                                                  where u.CaseTime >= startDate && u.CaseTime <= endDate && u.IsDeleted == false
                                                  select u;

            return results;
        }

        #endregion

        #region FreqUsedLink
        public IQueryable<FrequentUsedLink> QueryableFrequentUsedLinks
        {
            get
            {
                entities.FrequentUsedLinks.MergeOption = MergeOption.OverwriteChanges;

                queryableFrequentUsedLinks = entities.FrequentUsedLinks;

                return queryableFrequentUsedLinks;
            }
        }
        #endregion
    }
}
