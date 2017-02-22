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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class MaterialDeclareGatherViewModel : ViewModel<IMaterialDeclareGatherView>
    {

        private IEnumerable<MaterialDeclareGatherTable> gatherMaterialScoreList;
        private IEnumerable<MaterialDeclareGatherTable> gatherMaterialAmountList;

        private DateTime issueStartDate = DateTime.Parse("1990-01-01");
        private DateTime issueEndDate = DateTime.Parse("2099-12-31");

        private DateTime declareStartDate = DateTime.Parse("1990-01-01");
        private DateTime declareEndDate = DateTime.Parse("2099-12-31");

        private IEntityService entityservice;


        private ICommand scoreQueryGatherCommand;
        private ICommand amountQueryGatherCommand;
        private ICommand gatherRetreatCommand;


        [ImportingConstructor]
        public MaterialDeclareGatherViewModel(IMaterialDeclareGatherView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;
        }

        public ICommand ScoreQueryGatherCommand
        {
            get { return scoreQueryGatherCommand; }
            set
            {
                if (scoreQueryGatherCommand != value)
                {
                    scoreQueryGatherCommand = value;
                    RaisePropertyChanged("ScoreQueryGatherCommand");
                }
            }
        }

        public ICommand AmountQueryGatherCommand
        {
            get { return amountQueryGatherCommand; }
            set
            {
                if (amountQueryGatherCommand != value)
                {
                    amountQueryGatherCommand = value;
                    RaisePropertyChanged("AmountQueryGatherCommand");
                }
            }
        }

        public ICommand GatherRetreatCommand
        {
            get { return gatherRetreatCommand; }
            set
            {
                if (gatherRetreatCommand != value)
                {
                    gatherRetreatCommand = value;
                    RaisePropertyChanged("GatherRetreatCommand");
                }
            }
        }

        public IEnumerable<MaterialDeclareGatherTable> GatherMaterialScoreList
        {
            get { return gatherMaterialScoreList; }
            set
            {
                if (gatherMaterialScoreList != value)
                {
                    gatherMaterialScoreList = value;
                    RaisePropertyChanged("GatherMaterialScoreList");
                }
            }
        }

        public IEnumerable<MaterialDeclareGatherTable> GatherMaterialAmountList
        {
            get { return gatherMaterialAmountList; }
            set
            {
                if (gatherMaterialAmountList != value)
                {
                    gatherMaterialAmountList = value;
                    RaisePropertyChanged("GatherMaterialAmountList");
                }
            }
        }

        public DateTime IssueStartDate
        {
            get { return issueStartDate; }
            set
            {
                if (issueStartDate != value)
                {
                    issueStartDate = value;
                    RaisePropertyChanged("IssueStartDate");
                }
            }
        }

        public DateTime IssueEndDate
        {
            get { return issueEndDate; }
            set
            {
                if (issueEndDate != value)
                {
                    issueEndDate = value;
                    RaisePropertyChanged("IssueEndDate");
                }
            }
        }

        public DateTime DeclareStartDate
        {
            get { return declareStartDate; }
            set
            {
                if (declareStartDate != value)
                {
                    declareStartDate = value;
                    RaisePropertyChanged("DeclareStartDate");
                }
            }
        }

        public DateTime DeclareEndDate
        {
            get { return declareEndDate; }
            set
            {
                if (declareEndDate != value)
                {
                    declareEndDate = value;
                    RaisePropertyChanged("DeclareEndDate");
                }
            }
        }
    }
}
    