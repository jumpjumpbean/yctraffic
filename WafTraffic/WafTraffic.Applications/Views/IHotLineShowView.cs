using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.Views
{
    public interface IHotLineShowView : IView
    {
        void ShowLoading(LoadingType type);
        void ShutdownLoading(LoadingType type);
    }
}
