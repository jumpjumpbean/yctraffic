using System;
using System.Windows;
using GMap.NET;
using WafTraffic.Domain.Common;

namespace WafTraffic.Presentation.MapSources
{
    public class Dummy
    {

    }

    public struct PointAndInfo
    {
        public PointLatLng Point;
        public string Info;
        public int MarkerTypeId;

        public PointAndInfo(PointLatLng point, string info, int markerTypeId)
        {
            Point = point;
            Info = info;
            MarkerTypeId = markerTypeId;
        }
    }


    public struct RouterPointAndInfo
    {
        public PointLatLng start;
        public PointLatLng end;
        public string Info;

        public RouterPointAndInfo(PointLatLng s, PointLatLng e, string info)
        {
            start = s;
            end = e;
            Info = info;
        }
    }
    
}
