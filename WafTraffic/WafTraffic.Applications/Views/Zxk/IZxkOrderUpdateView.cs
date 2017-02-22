using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;
using WafTraffic.Applications.Common;


namespace WafTraffic.Applications.Views
{
    public interface IZxkOrderUpdateView : IView
    {
        void Show_Loading(LoadingType type);
        void Shutdown_Loading(LoadingType type);
    }
}
    