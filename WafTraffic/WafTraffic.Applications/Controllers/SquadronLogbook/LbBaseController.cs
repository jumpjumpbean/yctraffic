using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.Foundation;
using WafTraffic.Applications.Properties;
using WafTraffic.Applications.Services;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using DotNet.Business;
using System.IO;
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.Controllers
{
    [Export]
    internal class LbBaseController : Controller
    {
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private LogbookMainViewModel mLogbookMainViewModel;


        //private readonly DelegateCommand mNewCommand;
        //private readonly DelegateCommand mModifyCommand;
        //private readonly DelegateCommand mQueryCommand;
        //private readonly DelegateCommand mUploadCommand;
        //private readonly DelegateCommand mDisplayCommand;
        //private readonly DelegateCommand mDownloadCommand;
        //private readonly DelegateCommand mCloseCommand;


        [ImportingConstructor]
        public LbBaseController(CompositionContainer container, 
            System.Waf.Applications.Services.IMessageService messageService, 
            IShellService shellService, IEntityService entityService)
        {
            this.mContainer = container;
            this.mMessageService = messageService;
            this.mShellService = shellService;
            this.mEntityService = entityService;

            mLogbookMainViewModel = container.GetExportedValue<LogbookMainViewModel>();

            /*
            this.mNewCommand = new DelegateCommand(() => NewOper(), null);
            this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
            this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
            this.mSaveCommand = new DelegateCommand(() => Save(), null);
            this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
            this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
            this.mUploadCommand = new DelegateCommand(() => UploadOper(), null);
            this.mDisplayCommand = new DelegateCommand(() => DisplayOper(), null);
            this.mDownloadCommand = new DelegateCommand(() => DownloadOper(), null);
            this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
            this.mCloseCommand = new DelegateCommand(() => CloseOper(), null);
             */
        }

        public void Initialize()
        {
            /*
            mStaticLogbookUpdateViewModel.SaveCommand = this.mSaveCommand;
            mStaticLogbookUpdateViewModel.CancelCommand = this.mCancelCommand;
            mStaticLogbookUpdateViewModel.UploadCommand = this.mUploadCommand;

            mStaticLogbookDetailsViewModel.DisplayCommand = this.mDisplayCommand;
            mStaticLogbookDetailsViewModel.DownloadCommand = this.mDownloadCommand;
            mStaticLogbookDetailsViewModel.CancelCommand = this.mCancelCommand;

            mStaticLogbookDocViewModel.CloseCommand = this.mCloseCommand;

            mLbBaseQueryViewModel.NewCommand = this.mNewCommand;
            mLbBaseQueryViewModel.ModifyCommand = this.mModifyCommand;
            mLbBaseQueryViewModel.DeleteCommand = this.mDeleteCommand;
            mLbBaseQueryViewModel.QueryCommand = this.mQueryCommand;
            mLbBaseQueryViewModel.BrowseCommand = this.mBrowseCommand;
            RefreshQueryData();
             */

        }

 
    }

}
