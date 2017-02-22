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

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class MainFrameViewModel : ViewModel<IMainFrameView>
    {
        private readonly IEnumerable<BaseModuleEntity> modules;
        private IEnumerable<BaseModuleEntity> submodules;
        private BaseModuleEntity selectModule;
        private BaseModuleEntity selectSubModule;
        private object contentView;

        IEntityService entityservice;

        [ImportingConstructor]
        public MainFrameViewModel(IMainFrameView view, IEntityService entityservice)
            : base(view)
        {
            this.entityservice = entityservice;

            if (CurrentLoginService.Instance.CurrentUserInfo != null)
            {
                this.modules = CurrentLoginService.Instance.GetMainModuleListWithEnable();
            }
        }

        public IEnumerable<BaseModuleEntity> Modules { get { return modules; } }

        public IEnumerable<BaseModuleEntity> SubModules 
        { 
            get { return submodules; }
            set
            {
                if (submodules != value)
                {
                    submodules = value;
                    RaisePropertyChanged("SubModules");
                }
            }
        }


        public BaseModuleEntity SelectModule
        {
            get { return selectModule; }
            set
            {
                if (selectModule != value)
                {
                    selectModule = value;
                    RaisePropertyChanged("SelectModule");
                }
            }
        }

        public BaseModuleEntity SelectSubModule
        {
            get { return selectSubModule; }
            set
            {
                if (selectSubModule != value)
                {
                    selectSubModule = value;
                    RaisePropertyChanged("SelectSubModule");
                }
            }
        }

        public void Focus()
        {
            ViewCore.FocusFirstItem();
        }

        public object ContentView
        {
            get { return contentView; }
            set
            {
                if (contentView != value)
                {
                    contentView = value;
                    RaisePropertyChanged("ContentView");
                }
            }
        }

        public string DocumentName { get { return "禹 城 交 警 信 息 化 综 合 管 理 平 台"; } }


    }
}
