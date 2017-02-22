using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WafTraffic.Domain.Properties;
using System.Waf.Foundation;
using System.Windows;
using DotNet.Business;

namespace WafTraffic.Domain
{
    public partial class AlarmNotifyTable
    {
        public string AlarmMessage { get; set; }
        public string AlarmTag { get; set; }
    }
}

