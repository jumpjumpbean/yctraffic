using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace WafTraffic.Applications.Views
{
    public interface ILbZdStaffInfoQueryView : IView
    {
        void PagingReload();
    }
}
    