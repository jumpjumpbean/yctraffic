using System;
using System.Linq;
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
    public class FrequentMainViewModel : ViewModel<IFrequentMainView>
    {
        #region Data

        private ZdtzConfigTable mSelectedFrequent;
        private IEnumerable<ZdtzConfigTable> mSquadronFrequentList;
        private List<ZdtzConfigTable> mRootNodes;
        private object mContentView;

        IEntityService entityservice;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public FrequentMainViewModel(IFrequentMainView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;
            this.mRootNodes = new List<ZdtzConfigTable>();

            if (entityservice.EnumSquadronFrequentTypes != null)
            {
                this.mSquadronFrequentList = entityservice.EnumSquadronFrequentTypes;
                List<ZdtzConfigTable> nodes = this.mSquadronFrequentList.ToList();
                foreach (ZdtzConfigTable node in this.mSquadronFrequentList)
                {
                    if (node.ParentId == 0)
                    {
                        this.mRootNodes.Add(node);
                        // List的Add是添加引用，遍历后可完成树的构造
                        node.AddChildren(nodes);
                    }
                }
            }
            else
            {
                this.mSquadronFrequentList = new List<ZdtzConfigTable>(); //以防没有数据时出现异常
            }
        }

        #endregion

        #region Properties

        public IEnumerable<ZdtzConfigTable> RootNodes
        {
            get
            {
                return mRootNodes;
            }
        }

        public ZdtzConfigTable SelectedFrequent
        {
            get { return mSelectedFrequent; }
            set
            {
                if (mSelectedFrequent != value)
                {
                    mSelectedFrequent = value;
                    RaisePropertyChanged("SelectedFrequent");
                }
            }
        }

        public object ContentView
        {
            get { return mContentView; }
            set
            {
                if (mContentView != value)
                {
                    mContentView = value;
                    RaisePropertyChanged("ContentView");
                }
            }
        }

        #endregion

    }
}
