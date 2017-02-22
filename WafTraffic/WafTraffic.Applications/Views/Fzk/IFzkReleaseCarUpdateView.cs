using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace WafTraffic.Applications.Views
{
    public interface IFzkReleaseCarUpdateView : IView
    {
        void Show_Loading();
        void Shutdown_Loading();
    }
}
    