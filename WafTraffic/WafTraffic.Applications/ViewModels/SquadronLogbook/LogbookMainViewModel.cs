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
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class LogbookMainViewModel : ViewModel<ILogbookMainView>
    {
        #region Data

        private ZdtzConfigTable mSelectedLogbook;
        private IEnumerable<ZdtzConfigTable> mSquadronLogbookList;
        private List<ZdtzConfigTable> mRootNodes;
        private object mContentView;

        IEntityService entityservice;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LogbookMainViewModel(ILogbookMainView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;
            this.mRootNodes = new List<ZdtzConfigTable>();

            if (entityservice.EnumSquadronLogbookTypes != null)
            {
                try
                {
                    this.mSquadronLogbookList = entityservice.EnumSquadronLogbookTypes;

                    foreach (ZdtzConfigTable node in this.mSquadronLogbookList)
                    {
                        if (node.ParentId == 0)
                        {
                            this.mRootNodes.Add(node);
                            // 目前因为台账只有两层，所以只对一级节点AddChildren，提高性能
                            // 台账类型超过两层时，应放到if外
                            if (node.LogbookType != YcConstants.INT_LBCONFIG_TYPE_FREQUENT)
                            {
                                node.AddChildren(this.mSquadronLogbookList.ToList());
                            }
                        }
                        /*
                        // List的Add是添加引用，遍历后可完成树的构造
                        node.AddChildren(this.mSquadronLogbookList.ToList());
                        */
                    }
                }
                catch (Exception)
                {
                
                }
            }
            else
            {
                this.mSquadronLogbookList = new List<ZdtzConfigTable>(); //以防没有数据时出现异常
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

        public ZdtzConfigTable SelectedLogbook
        {
            get { return mSelectedLogbook; }
            set
            {
                if (mSelectedLogbook != value)
                {
                    mSelectedLogbook = value;
                    RaisePropertyChanged("SelectedLogbook");
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
