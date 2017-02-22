using System.Windows.Controls;

namespace WafTraffic.Presentation.CustomMarkers
{
    /// <summary>
    /// Cross.xaml 的交互逻辑
    /// </summary>
    public partial class Cross : UserControl
    {
        public Cross()
        {
            InitializeComponent();
            this.IsHitTestVisible = false;
        }
    }
}
