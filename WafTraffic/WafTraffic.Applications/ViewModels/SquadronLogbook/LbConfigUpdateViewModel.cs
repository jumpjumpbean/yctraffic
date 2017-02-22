using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class LbConfigUpdateViewModel : LbBaseUpdateViewModel<ILbConfigUpdateView>
    {
        #region Data

        private ZdtzConfigTable mEntity;
        private List<ZdtzConfigTable> mParentNodeList;
        private ZdtzConfigTable mSelectedParent;
        //private bool mIsNodeLevelVisible;
        private bool mIsNodeLevelEnabled;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public LbConfigUpdateViewModel(ILbConfigUpdateView view)
            : base(view)
        {
            mEntity = new ZdtzConfigTable();
            //mIsNodeLevelVisible = true;
        }

        #endregion

        #region Properties

        public ZdtzConfigTable ConfigEntity
        {
            get { return mEntity; }
            set
            {
                if (mEntity != value)
                {
                    mEntity = value;
                    RaisePropertyChanged("ConfigEntity");
                }
            }
        }

        public List<ZdtzConfigTable> ParentNodeList
        {
            get { return mParentNodeList; }
            set
            {
                //if (mParentNodeList != value)
                //{
                    mParentNodeList = value;
                    RaisePropertyChanged("ParentNodeList");
                //}
            }
        }

        public ZdtzConfigTable SelectedParent
        {
            get { return mSelectedParent; }
            set
            {
                if (mSelectedParent != value)
                {
                    mSelectedParent = value;
                    RaisePropertyChanged("SelectedParent");
                }
            }
        }

        public bool IsNodeLevelEnabled
        {
            get { return mIsNodeLevelEnabled; }
            set
            {
                if (mIsNodeLevelEnabled != value)
                {
                    mIsNodeLevelEnabled = value;
                    RaisePropertyChanged("IsNodeLevelEnabled");
                }
            }
        }

        #endregion

    }
}
