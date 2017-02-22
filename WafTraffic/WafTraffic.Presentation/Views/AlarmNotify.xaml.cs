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
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Documents;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// AlarmNotifyView.xaml µÄ½»»¥Âß¼­
    /// </summary>
    [Export(typeof(IAlarmNotifyView))]
    public partial class AlarmNotifyView  : UserControl, IAlarmNotifyView
    {

        private readonly Lazy<AlarmNotifyViewModel> viewModel;
        public AlarmNotifyView()
        {
            InitializeComponent();
            viewModel = new Lazy<AlarmNotifyViewModel>(() => ViewHelper.GetViewModel<AlarmNotifyViewModel>(this));
        }

        public void InitializeFrequentUsedLinks()
        {
            hyperLink1.Inlines.Clear();
            hyperLink2.Inlines.Clear();
            hyperLink3.Inlines.Clear();
            hyperLink4.Inlines.Clear();
            hyperLink5.Inlines.Clear();
            hyperLink6.Inlines.Clear();
            hyperLink7.Inlines.Clear();
            hyperLink8.Inlines.Clear();
            hyperLink9.Inlines.Clear();
            hyperLink10.Inlines.Clear();

            try
            {
                if (!string.IsNullOrEmpty(viewModel.Value.FreqtUsdLnk.InlineText1))
                {
                    hyperLink1.Inlines.Add(new Run(viewModel.Value.FreqtUsdLnk.InlineText1));
                    hyperLink1.NavigateUri = new Uri(viewModel.Value.FreqtUsdLnk.NavigateUri1);
                }
                if (!string.IsNullOrEmpty(viewModel.Value.FreqtUsdLnk.InlineText2))
                {
                    hyperLink2.Inlines.Add(new Run(viewModel.Value.FreqtUsdLnk.InlineText2));
                    hyperLink2.NavigateUri = new Uri(viewModel.Value.FreqtUsdLnk.NavigateUri2);
                }
                if (!string.IsNullOrEmpty(viewModel.Value.FreqtUsdLnk.InlineText3))
                {
                    hyperLink3.Inlines.Add(new Run(viewModel.Value.FreqtUsdLnk.InlineText3));
                    hyperLink3.NavigateUri = new Uri(viewModel.Value.FreqtUsdLnk.NavigateUri3);
                }
                if (!string.IsNullOrEmpty(viewModel.Value.FreqtUsdLnk.InlineText4))
                {
                    hyperLink4.Inlines.Add(new Run(viewModel.Value.FreqtUsdLnk.InlineText4));
                    hyperLink4.NavigateUri = new Uri(viewModel.Value.FreqtUsdLnk.NavigateUri4);
                }
                if (!string.IsNullOrEmpty(viewModel.Value.FreqtUsdLnk.InlineText5))
                {
                    hyperLink5.Inlines.Add(new Run(viewModel.Value.FreqtUsdLnk.InlineText5));
                    hyperLink5.NavigateUri = new Uri(viewModel.Value.FreqtUsdLnk.NavigateUri5);
                }
                if (!string.IsNullOrEmpty(viewModel.Value.FreqtUsdLnk.InlineText6))
                {
                    hyperLink6.Inlines.Add(new Run(viewModel.Value.FreqtUsdLnk.InlineText6));
                    hyperLink6.NavigateUri = new Uri(viewModel.Value.FreqtUsdLnk.NavigateUri6);
                }
                if (!string.IsNullOrEmpty(viewModel.Value.FreqtUsdLnk.InlineText7))
                {
                    hyperLink7.Inlines.Add(new Run(viewModel.Value.FreqtUsdLnk.InlineText7));
                    hyperLink7.NavigateUri = new Uri(viewModel.Value.FreqtUsdLnk.NavigateUri7);
                }
                if (!string.IsNullOrEmpty(viewModel.Value.FreqtUsdLnk.InlineText8))
                {
                    hyperLink8.Inlines.Add(new Run(viewModel.Value.FreqtUsdLnk.InlineText8));
                    hyperLink8.NavigateUri = new Uri(viewModel.Value.FreqtUsdLnk.NavigateUri8);
                }
                if (!string.IsNullOrEmpty(viewModel.Value.FreqtUsdLnk.InlineText9))
                {
                    hyperLink9.Inlines.Add(new Run(viewModel.Value.FreqtUsdLnk.InlineText9));
                    hyperLink9.NavigateUri = new Uri(viewModel.Value.FreqtUsdLnk.NavigateUri9);
                }
                if (!string.IsNullOrEmpty(viewModel.Value.FreqtUsdLnk.InlineText10))
                {
                    hyperLink10.Inlines.Add(new Run(viewModel.Value.FreqtUsdLnk.InlineText10));
                    hyperLink10.NavigateUri = new Uri(viewModel.Value.FreqtUsdLnk.NavigateUri10);
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
        }


        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {

            Process.Start(e.Uri.OriginalString);

            e.Handled = true;
        }
    }
}
    