using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Domain;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// MaterialDeclareGatherView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IMaterialDeclareGatherView))]
    public partial class MaterialDeclareGatherView  : UserControl, IMaterialDeclareGatherView
    {
        private readonly Lazy<MaterialDeclareGatherViewModel> viewModel;

        public MaterialDeclareGatherView()
        {
            InitializeComponent();
            viewModel = new Lazy<MaterialDeclareGatherViewModel>(() => ViewHelper.GetViewModel<MaterialDeclareGatherViewModel>(this));
        }


        private void btnScoreQueryGather_Click(object sender, RoutedEventArgs e)
        {

            if (tbIssueStartDate.Value == null)
            {
                viewModel.Value.IssueStartDate = DateTime.Parse("1990-01-01");
            }
            else
            {
                viewModel.Value.IssueStartDate = Convert.ToDateTime(tbIssueStartDate.Value);
            }

            if (tbIssueEndDate.Value == null)
            {
                viewModel.Value.IssueEndDate = DateTime.Parse("2099-12-31");
            }
            else
            {
                viewModel.Value.IssueEndDate = Convert.ToDateTime(tbIssueEndDate.Value);
            }
        }

        private void btnAmountQueryGather_Click(object sender, RoutedEventArgs e)
        {

            if (tbDeclareStartDate.Value == null)
            {
                viewModel.Value.DeclareStartDate = DateTime.Parse("1990-01-01");
            }
            else
            {
                viewModel.Value.DeclareStartDate = Convert.ToDateTime(tbDeclareStartDate.Value);
            }

            if (tbDeclareEndDate.Value == null)
            {
                viewModel.Value.DeclareEndDate = DateTime.Parse("2099-12-31");
            }
            else
            {
                viewModel.Value.DeclareEndDate = Convert.ToDateTime(tbDeclareEndDate.Value);
            }
        }
    }
}
    