using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using DotNet.Business;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// UserAdminView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IUserAdminView))]
    public partial class UserAdminView  : UserControl, IUserAdminView
    {
        private readonly Lazy<UserAdminViewModel> viewModel;
        public UserAdminView()
        {
            InitializeComponent();
            viewModel = new Lazy<UserAdminViewModel>(() => ViewHelper.GetViewModel<UserAdminViewModel>(this));
        }

        private void gridUserList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridUserList.Total)
            {
                startIndex = (gridUserList.Total / args.PageSize) * args.PageSize;
                gridUserList.PageIndex = (gridUserList.Total % args.PageSize) == 0 ? (gridUserList.Total / args.PageSize) : (gridUserList.Total / args.PageSize) + 1;
                args.PageIndex = gridUserList.PageIndex;
            }

            IList<BaseUserEntity> gridSource = viewModel.Value.Users;

            gridUserList.Total = gridSource.Count();
            gridUserList.ItemsSource = gridSource.Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
        }

        public void PagingReload()
        {
            gridUserList.PageIndex = 1;
            gridUserList.RaisePageChanged();
        }
    }
}
    