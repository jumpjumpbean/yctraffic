using System.Windows;
using System.Windows.Markup;

namespace WafTraffic.Reporting.Presentation.Controls
{
    [ContentProperty("Content")]
    public class ContentElement : FrameworkElement
    {
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(FrameworkContentElement), typeof(ContentElement), new UIPropertyMetadata(null));


        public FrameworkContentElement Content
        {
            get { return (FrameworkContentElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
    }
}
