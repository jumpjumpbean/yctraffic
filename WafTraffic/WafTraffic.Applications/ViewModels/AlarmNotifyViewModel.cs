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
using System.Windows;
using System.Linq;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class AlarmNotifyViewModel : ViewModel<IAlarmNotifyView>
    {

        private IEnumerable<AlarmNotifyTable> alarmNotifyList;
        private AlarmNotifyTable selectedAlarmNotify;
        private FrequentUsedLink freqtUsdLnk;

        private string alarmCount;

        private ICommand alarmNotifyHandlerCommand;
        private ICommand alarmNotifyRefreshCommand;
        private ICommand settingCommand;

        private Visibility noAlarm;
        private Visibility canSettingShow;

        IEntityService entityservice;

        [ImportingConstructor]
        public AlarmNotifyViewModel(IAlarmNotifyView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (entityservice.Entities.FrequentUsedLinks.Count() != 0)
            {
                this.freqtUsdLnk = entityservice.QueryableFrequentUsedLinks.ToList()[0];
            }
            else
            {
                this.freqtUsdLnk = new FrequentUsedLink();
            }

            LoadFrequentUsedLinks();
        }

        public void LoadFrequentUsedLinks()
        {
            ViewCore.InitializeFrequentUsedLinks();
        }

        public IEnumerable<AlarmNotifyTable> AlarmNotifyList
        {
            get
            {
                return alarmNotifyList;
            }
            set
            {
                if (alarmNotifyList != value)
                {
                    alarmNotifyList = value;
                    RaisePropertyChanged("AlarmNotifyList");
                }
            }
        }

        public AlarmNotifyTable SelectedAlarmNotify
        {
            get { return selectedAlarmNotify; }
            set
            {
                if (selectedAlarmNotify != value)
                {
                    selectedAlarmNotify = value;
                    RaisePropertyChanged("SelectedAlarmNotify");
                }
            }
        }

        public FrequentUsedLink FreqtUsdLnk
        {
            get
            {
                return freqtUsdLnk;
            }
            set
            {
                if (freqtUsdLnk != value)
                {
                    freqtUsdLnk = value;
                    RaisePropertyChanged("FreqtUsdLnk");
                }
            }
        }

        public List<MonthRegisterTable> AlarmMonthRegisters
        {
            get { return entityservice.GetMonthRegistersByUser(Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id), DateTime.Now.Year, DateTime.Now.Month); }
        }

        public List<MayorHotlineTaskTable> AlarmHotLineNoDeal
        {
            get
            {
                return entityservice.GetHotLineMustDeal(Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId));
            }
        }

        public List<MayorHotlineTaskTable> AlarmHotLineOverTime
        {
            get
            {
                return entityservice.GetHotLineOverTime();
            }
        }

        public int MaterialDeclareNoApproveCount
        {
            get
            {
                return entityservice.GetMaterialDeclareNoApproveCount();
            }
        }

        /*
        public IQueryable<EquipmentCheckTable> EquipmentDueDate
        {
            get
            {
                return entityservice.GetEquipmentDueDate();
            }
        }*/

        public IQueryable<ZhzxEquipmentRequest> ZhzxRequestOverTime
        {
            get
            {
                return entityservice.QueryZhzxOverTimeRequests();
            }
        }

        public IQueryable<ZdtzCyDangerDeal> ZdtzCyDangerDealOverTime
        {
            get
            {
                return entityservice.QueryZdtzOverTimeCyDangerDeal();
            }
        }

        public IQueryable<ZgxcAskForLeave> ZgxcWaitingForApprovalLeaves
        {
            get
            {
                return entityservice.QueryZgxcWaitingForApprovalLeaves(CurrentLoginService.Instance.CurrentUserInfo.RealName);
            }
        }

        public IQueryable<ZhzxEquipmentRequest> ZhzxEquipReq_NeedDdzDeals
        {
            get
            {
                return entityservice.ZhzxEquipReq_NeedDdzDeals();
            }
        }

        public IQueryable<KjssEquipmentRequest> KjkEquipReq_NeedDdzDeals
        {
            get
            {
                return entityservice.KjkEquipReq_NeedDdzDeals();
            }
        }

        public IQueryable<FzkReleaseCar> FzkReleaseCarFdzDeals
        {
            get
            {
                return entityservice.FzkReleaseCarFdzDeals();
            }
        }

        public ICommand AlarmNotifyHandlerCommand
        {
            get { return alarmNotifyHandlerCommand; }
            set
            {
                if (alarmNotifyHandlerCommand != value)
                {
                    alarmNotifyHandlerCommand = value;
                    RaisePropertyChanged("AlarmNotifyHandlerCommand");
                }
            }
        }

        public ICommand AlarmNotifyRefreshCommand
        {
            get { return alarmNotifyRefreshCommand; }
            set
            {
                if (alarmNotifyRefreshCommand != value)
                {
                    alarmNotifyRefreshCommand = value;
                    RaisePropertyChanged("AlarmNotifyRefreshCommand");
                }
            }
        }

        public ICommand SettingCommand
        {
            get { return settingCommand; }
            set
            {
                if (settingCommand != value)
                {
                    settingCommand = value;
                    RaisePropertyChanged("SettingCommand");
                }
            }
        }

        public string WelcomePhrase
        { 
            get
            {
                int hour = System.DateTime.Now.Hour;

                if (hour >= 12 && hour < 18)
                {
                    return ("下午好，");
                }
                else if (hour >= 18)
                {
                    return ("晚上好，");
                }
                else if (hour < 5)
                {
                    return ("晚上好，");
                }
                else
                {
                    return ("上午好，");
                }
            }
        }

        public string RealName
        {
            get 
            {
                return CurrentLoginService.Instance.CurrentUserInfo.RealName;
            } 
        }

        public string AlarmCount
        {
            get
            {
                return alarmCount;
            }
            set 
            {
                if (alarmCount != value)
                {
                    alarmCount = value;
                    RaisePropertyChanged("AlarmCount");
                }
            }
        }

        public string YearMonth
        {
            get
            {
                return System.DateTime.Now.ToString("yyyy 年 MM 月");
            }
        }

        public string TheDate
        {
            get
            {
                return System.DateTime.Now.ToString("dd");
            }
        }

        public string TheWeek
        {
            get
            {
                switch (DateTime.Now.DayOfWeek.ToString("D"))
                {
                    case "0":
                        return "星 期 日 ";
                    case "1":
                        return "星 期 一 ";
                    case "2":
                        return "星 期 二 ";
                    case "3":
                        return "星 期 三 ";
                    case "4":
                        return "星 期 四 ";
                    case "5":
                        return "星 期 五 ";
                    case "6":
                        return "星 期 六 ";
                    default:
                        return "WTF??! Just to make the switch-case sentence block happy";
                }
            }
        }

        public Visibility NoAlarm
        {
            get
            {
                return noAlarm;
            }
            set
            {
                if (noAlarm != value)
                {
                    noAlarm = value;
                    RaisePropertyChanged("NoAlarm");
                }
            }
        }

        public Visibility CanSettingShow
        {
            get
            {
                return canSettingShow;
            }
            set
            {
                if (canSettingShow != value)
                {
                    canSettingShow = value;
                    RaisePropertyChanged("CanSettingShow");
                }
            }
        }

    }
}

    