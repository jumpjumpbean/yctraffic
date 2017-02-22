using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Waf;
using WafTraffic.Domain;
using DotNet.Utilities;
using WafTraffic.Presentation.Services;
using DotNet.Business;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// PersonArchiveListView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IPersonArchiveListView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class PersonArchiveListView : UserControl, IPersonArchiveListView
    {
        private List<Node> DepartmentItemList;
         private readonly Lazy<PersonArchiveListViewModel> viewModel;
        public PersonArchiveListView()
        {
            InitializeComponent();
            viewModel = new Lazy<PersonArchiveListViewModel>(() => ViewHelper.GetViewModel<PersonArchiveListViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            DepartmentItemList = new List<Node>();

            BaseOrganizeManager origanizeService = new BaseOrganizeManager();
            DataTable departmentDT = origanizeService.GetDT(BaseOrganizeEntity.FieldDeletionStateCode, "0"); //根节点 parnetid
            BaseOrganizeEntity entity;
            foreach (DataRow dr in departmentDT.Rows)
            {
                Node node = new Node();
                entity = new BaseOrganizeEntity(dr);
                node.ID = Convert.ToInt32(entity.Id);
                node.Code = entity.Code;
                node.Name = entity.FullName;
                node.ParentID = Convert.ToInt32(entity.ParentId);
                DepartmentItemList.Add(node);
            }

            tvDepartment.ItemsSource = this.Bind(DepartmentItemList); //绑定树

        }

         private void gridPersonArchiveList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridPersonArchiveList.Total)
            {
                startIndex = (gridPersonArchiveList.Total / args.PageSize) * args.PageSize;
                gridPersonArchiveList.PageIndex = (gridPersonArchiveList.Total % args.PageSize) == 0 ? (gridPersonArchiveList.Total / args.PageSize) : (gridPersonArchiveList.Total / args.PageSize) + 1;
                args.PageIndex = gridPersonArchiveList.PageIndex;
            }

             IQueryable<PersonArchiveTable> gridSource = viewModel.Value.PersonArchives;
             gridPersonArchiveList.Total = gridSource.Count();

             gridPersonArchiveList.ItemsSource = gridSource.OrderBy(p => p.Id).Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);

        }
        
        public void PagingReload()
        {
            gridPersonArchiveList.PageIndex = 1;
            gridPersonArchiveList.RaisePageChanged();
        }

        /// <summary>
        /// 绑定树
        /// </summary>
        List<Node> Bind(List<Node> nodes)
        {
            List<Node> outputList = new List<Node>();
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ParentID == 0)
                {
                    outputList.Add(nodes[i]);
                }
                else
                {
                    FindDownward(nodes, Convert.ToInt32(nodes[i].ParentID)).Nodes.Add(nodes[i]);
                }
            }
            return outputList;
        }
        /// <summary>
        /// 递归向下查找
        /// </summary>
        Node FindDownward(List<Node> nodes, int id)
        {
            if (nodes == null) return null;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ID == id)
                {
                    return nodes[i];
                }
                Node node = FindDownward(nodes[i].Nodes, id);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        private void tvDepartment_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                viewModel.Value.SelectedTreeNode = ((Node)e.NewValue);
            }
            catch
            {
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.Value.SelectedPolicalTypeId = 0;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            viewModel.Value.SelectedPolicalTypeId = 1;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            viewModel.Value.SelectedPolicalTypeId = 2;
        }
    }
    
}
