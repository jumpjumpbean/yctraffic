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
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class ZhzxOfficeSupplyStockDetailViewModel : ViewModel<IZhzxOfficeSupplyStockDetailView>
    {
        private ZhzxOfficeSupplyStock zhzxOfficeSupplyStock;

        private ICommand saveCommand;
        private ICommand retreatCommand;

        private string operation;

        private Visibility canSave;
        private Visibility canCreatorVisibal;
        private Visibility canAboveSaveVisibal;

        private bool canBaseInfoEdit;

        [ImportingConstructor]
        public ZhzxOfficeSupplyStockDetailViewModel(IZhzxOfficeSupplyStockDetailView view)
            : base(view)
        {
            zhzxOfficeSupplyStock = new ZhzxOfficeSupplyStock();
        }

        public String Operation
        {
            get { return operation; }
            set
            {
                if (operation != value)
                {
                    operation = value;

                    RaisePropertyChanged("Operation");
                }
            }
        }

        public ZhzxOfficeSupplyStock ZhzxOfficeSupplyStock
        {
            get { return zhzxOfficeSupplyStock; }
             set
             {
                 if (zhzxOfficeSupplyStock != value)
                 {
                     zhzxOfficeSupplyStock = value;
                     RaisePropertyChanged("ZhzxOfficeSupplyStock");
                 }
             }
        }

         public ICommand SaveCommand
         {
             get { return saveCommand; }
             set
             {
                 if (saveCommand != value)
                 {
                     saveCommand = value;
                     RaisePropertyChanged("SaveCommand");
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

         public Visibility CanCreatorVisibal
         {
             get { return canCreatorVisibal; }
             set
             {
                 if (canCreatorVisibal != value)
                 {
                     canCreatorVisibal = value;
                     RaisePropertyChanged("CanCreatorVisibal");
                 }
             }
         }


        public Visibility CanAboveSaveVisibal
         {
             get { return canAboveSaveVisibal; }
             set
             {
                 if (canAboveSaveVisibal != value)
                 {
                     canAboveSaveVisibal = value;
                     RaisePropertyChanged("CanAboveSaveVisibal");
                 }
             }
         }
        public Visibility CanSave
        {
            get { return canSave; }
            set
            {
                if (canSave != value)
                {
                    canSave = value;
                    RaisePropertyChanged("CanSave");
                }
            }
        }


        public bool CanBaseInfoEdit
        {
            get { return canBaseInfoEdit; }
            set
            {
                if (canBaseInfoEdit != value)
                {
                    canBaseInfoEdit = value;
                    RaisePropertyChanged("CanBaseInfoEdit");
                }
            }
        }
    }
}