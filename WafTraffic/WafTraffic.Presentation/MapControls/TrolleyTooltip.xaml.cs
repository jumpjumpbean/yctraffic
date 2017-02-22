using System.Windows.Controls;
using GMap.NET;
using WafTraffic.Presentation.MapSources;

namespace WafTraffic.Presentation.MapControls
{
    /// <summary>
    /// TrolleyTooltip.xaml 的交互逻辑
    /// </summary>
    public partial class TrolleyTooltip : UserControl
    {
        public TrolleyTooltip()
        {
            InitializeComponent();
        }

        public void SetValues(string type, VehicleData vl)
        {
            Device.Text = vl.Id.ToString();
            LineNum.Text = type + " " + vl.Line;
            StopName.Text = vl.LastStop;
            TrackType.Text = vl.TrackType;
            TimeGps.Text = vl.Time;
            Area.Text = vl.AreaName;
            Street.Text = vl.StreetName;
        }
    }
}