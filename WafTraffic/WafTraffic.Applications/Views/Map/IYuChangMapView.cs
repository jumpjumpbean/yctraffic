using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace WafTraffic.Applications.Views
{
    public interface IYuChangMapView : IView
    {
        void AddMarker(double posLat, double posLng);
        void DeleteMarker();
        void AccidentDateState(bool flag);

        void AddRouter(double startLat, double startLng, double endLat, double endLng);
        void DeleteMapRouter(double latStart, double lngStart, double latEnd, double lngEnd);

        void PagingdRouterReload();
    }
}
    