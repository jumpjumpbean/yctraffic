using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;
using DotNet.Business;
using System.Data;
using WafTraffic.Domain.Common;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class MonthRegisterGatherViewModel : ViewModel<IMonthRegisterGatherView>
    {
        private List<BaseOrganizeEntity> departmentList;
        private int selectDepartId;
        private string selectDepartName;
        ObservableCollection<ChartSeries> monthRegisterSeries;
        ObservableCollection<ChartReport> monthRegisterReports;
        private IEnumerable<MohthRgisterGatherTable> gatherApproveList;
        private List<MohthRgisterChartTable> gatherChartSource;
        private DateTime minMonth;
        private DateTime maxMonth;

        private ICommand queryGatherCommand;
        private ICommand retreatCommand;

        IEntityService entityservice;

        [ImportingConstructor]
        public MonthRegisterGatherViewModel(IMonthRegisterGatherView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;
            
            this.minMonth = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-01 00:00:00");
            this.maxMonth = DateTime.Now.AddDays(1);

            departmentList = new List<BaseOrganizeEntity>();

            BaseOrganizeManager origanizeService = new BaseOrganizeManager();
            
            BaseOrganizeEntity entity;

            if (CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator || (CurrentLoginService.Instance.CurrentUserInfo.DepartmentId == null) )
            {
                DataTable departmentDT = origanizeService.GetDepartmentDT(""); //根节点 parnetid
                foreach (DataRow dr in departmentDT.Rows)
                {
                    entity = new BaseOrganizeEntity(dr);
                    departmentList.Add(entity);
                }
            }
            else 
            {
                departmentList.Add(origanizeService.GetEntity(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId.ToString()));

                DataTable departmentDT = origanizeService.GetDepartmentDT(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId.ToString()); //根节点 parnetid
                
                foreach (DataRow dr in departmentDT.Rows)
                {
                    entity = new BaseOrganizeEntity(dr);
                    departmentList.Add(entity);
                }

                //加上分管部门
                List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;
                if (depts != null)
                {
                    var companyEntity = depts.Find(en => en.Id == YcConstantTable.COMPANY_ID);
                    if (companyEntity != null)
                    {
                        depts.Remove(companyEntity);
                    }
                    foreach (var en in depts)
                    {
                        if (!departmentList.Contains(en))
                        {
                            departmentList.Add(en);
                        }
                    }
                }
            }

           

            if (departmentList.Count > 0)
            {
                if (CurrentLoginService.Instance.CurrentUserInfo.DepartmentId != null && CurrentLoginService.Instance.CurrentUserInfo.DepartmentId != 0)
                {
                    this.selectDepartId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }
                else
                {
                    this.selectDepartId = Convert.ToInt32(departmentList[0].Id);
                }
            }

            gatherApproveList = entityservice.GetGatherApproves(this.SelectDepartId, this.minMonth, this.maxMonth);
            gatherChartSource = entityservice.GetGatherChartSource(this.SelectDepartId, this.minMonth, this.maxMonth);
            monthRegisterSeries = GetMonthRegisterSeries();
            monthRegisterReports = GetMonthRegisterReports();
        }

        public List<BaseOrganizeEntity> DepartmentList
        {
            get { return departmentList; }
            set
            {
                if (departmentList != value)
                {
                    departmentList = value;
                    RaisePropertyChanged("DepartmentList");
                }
            }
        }

        public int SelectDepartId
        {
            get { return selectDepartId; }
            set
            {
                if (selectDepartId != value)
                {
                    selectDepartId = value;
                    this.selectDepartName = departmentList.Find(entity => entity.Id == selectDepartId).FullName;
                    RaisePropertyChanged("SelectDepartId");
                }
            }
        }

        public string SelectDepartName
        {
            get { return selectDepartName; }
            set
            {
                if (selectDepartName != value)
                {
                    selectDepartName = value;
                    RaisePropertyChanged("SelectDepartName");
                }
            }
        }

        public IEnumerable<MohthRgisterGatherTable> GatherApproveList
        {
            get { return gatherApproveList; }
            set
            {
                if (gatherApproveList != value)
                {
                    gatherApproveList = value;
                    RaisePropertyChanged("GatherApproveList");
                }
            }
        }

        public List<MohthRgisterChartTable> GatherChartSource
        {
            get { return gatherChartSource; }
            set
            {
                if (gatherChartSource != value)
                {
                    gatherChartSource = value;
                    RaisePropertyChanged("GatherChartSource");
                }
            }
        }

        public ObservableCollection<ChartSeries> GetMonthRegisterSeries()
        {
            ObservableCollection<ChartSeries> chartSeries = new ObservableCollection<ChartSeries>();
            foreach (MohthRgisterGatherTable tmpRec in gatherApproveList)
            {
                ObservableCollection<MonthRegisterChart> chartItems = new ObservableCollection<MonthRegisterChart>();
                chartItems.Add(new MonthRegisterChart() { Category = "非常优秀", Number = tmpRec.Excel });
                chartItems.Add(new MonthRegisterChart() { Category = "优秀", Number = tmpRec.Well });
                chartItems.Add(new MonthRegisterChart() { Category = "良好", Number = tmpRec.Good });
                chartItems.Add(new MonthRegisterChart() { Category = "一般", Number = tmpRec.Normal });
                chartItems.Add(new MonthRegisterChart() { Category = "差", Number = tmpRec.Bad });

                chartSeries.Add(new ChartSeries { UserName = tmpRec.UserName, ChartItems = chartItems });
            }
            return chartSeries;
        }


        public ObservableCollection<ChartReport> GetMonthRegisterReports()
        {
            ObservableCollection<ChartReport> chartReports = new ObservableCollection<ChartReport>();
            ChartReport chartReport = new ChartReport();
            foreach (MohthRgisterGatherTable tmpRec in gatherApproveList)
            {
                chartReport = new ChartReport();
                chartReport.UserName = tmpRec.UserName;
                chartReport.Category = "非常优秀";
                chartReport.Number = tmpRec.Excel;
                chartReports.Add(chartReport);

                chartReport = new ChartReport();
                chartReport.UserName = tmpRec.UserName;
                chartReport.Category = "优秀";
                chartReport.Number = tmpRec.Well;
                chartReports.Add(chartReport);

                chartReport = new ChartReport();
                chartReport.UserName = tmpRec.UserName;
                chartReport.Category = "良好";
                chartReport.Number = tmpRec.Good;
                chartReports.Add(chartReport);

                chartReport = new ChartReport();
                chartReport.UserName = tmpRec.UserName;
                chartReport.Category = "一般";
                chartReport.Number = tmpRec.Normal;
                chartReports.Add(chartReport);

                chartReport = new ChartReport();
                chartReport.UserName = tmpRec.UserName;
                chartReport.Category = "差";
                chartReport.Number = tmpRec.Bad;
                chartReports.Add(chartReport);
            }
            return chartReports;
        }

        public DateTime MinMonth
        {
            get { return minMonth; }
            set
            {
                if (minMonth != value)
                {
                    minMonth = value;
                    RaisePropertyChanged("MinMonth");
                }
            }
        }

        public DateTime MaxMonth
        {
            get { return maxMonth; }
            set
            {
                if (maxMonth != value)
                {
                    maxMonth = value;
                    RaisePropertyChanged("MaxMonth");
                }
            }
        }

        public ObservableCollection<ChartSeries> MonthRegisterSeries
        {
            get
            {
                return monthRegisterSeries;
            }
            set
            {
                if (monthRegisterSeries != value)
                {
                    monthRegisterSeries = value;
                    RaisePropertyChanged("MonthRegisterSeries");
                }
            }

        }

        public ObservableCollection<ChartReport> MonthRegisterReports
        {
            get
            {
                return monthRegisterReports;
            }
            set
            {
                if (monthRegisterReports != value)
                {
                    monthRegisterReports = value;
                    RaisePropertyChanged("MonthRegisterReports");
                }
            }

        }

        public ICommand QueryGatherCommand
        {
            get { return queryGatherCommand; }
            set
            {
                if (queryGatherCommand != value)
                {
                    queryGatherCommand = value;
                    RaisePropertyChanged("QueryGatherCommand");
                }
            }
        }

        public ICommand RetreatCommand
        {
            get { return retreatCommand; }
            set
            {
                if (retreatCommand != value)
                {
                    retreatCommand = value;
                    RaisePropertyChanged("RetreatCommand");
                }
            }
        }

        public void ReprotRefresh()
        {
            ViewCore.ReportReload();
        }

        public void Close()
        {
            ViewCore.Close();
        }
    }
   
}
    