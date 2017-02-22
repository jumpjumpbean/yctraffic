using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Domain;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;
using GMap.NET;
using WafTraffic.Presentation.MapSources;
using WafTraffic.Presentation.CustomMarkers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using WafTraffic.Domain.Common;
using System.Windows.Media;
using System.Windows.Input;
using DotNet.Business;

namespace WafTraffic.Presentation.Views
{
    /// <summary>
    /// YuChangMapView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IYuChangMapView))]
    public partial class YuChangMapView  : UserControl, IYuChangMapView
    {

        // marker
        GMapMarker currentMarker;
        PointLatLng start;
        PointLatLng end;
        double areaRadius = 38.7;
        double PosLat = 36.93274171075984;
        double PosLng = 116.63558006286621;
        string GeoserverUri = string.Empty;
        // zones list
        List<GMapMarker> Circles = new List<GMapMarker>();

        private readonly Lazy<YuChangMapViewModel> viewModel;

        private GMapMarker willDelMarker;
        public GMapMarker WillDelMarker 
        {
            get
            {
                return willDelMarker;
            }
            set
            {
                willDelMarker = value;
                if (willDelMarker != null)
                {
                    var tmpDelMapMarker = new DeleteMapMarker();
                    tmpDelMapMarker.lat = value.Position.Lat;
                    tmpDelMapMarker.lng = value.Position.Lng;

                    viewModel.Value.DelMapMarker = tmpDelMapMarker;
                }
                else
                {
                    viewModel.Value.DelMapMarker = null;
                }
            }
        }

        public YuChangMapView()
        {
            InitializeComponent();
            viewModel = new Lazy<YuChangMapViewModel>(() => ViewHelper.GetViewModel<YuChangMapViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
            MainMap.MouseMove += new System.Windows.Input.MouseEventHandler(MainMap_MouseMove);
            MainMap.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(MainMap_MouseLeftButtonDown);
            MainMap.MouseEnter += new MouseEventHandler(MainMap_MouseEnter);

            InitPage();
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            Loaded -= FirstTimeLoadedHandler;
           
            GeoserverUri = DotNet.Utilities.UserConfigHelper.GetValue("GeoserverUri");
            InitMap();
        }

        private void InitPage()
        {
            if (!CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator
                && CurrentLoginService.Instance.CurrentUserInfo.DepartmentId != YcConstantTable.ORGID_SGK
                && CurrentLoginService.Instance.CurrentUserInfo.DepartmentId != YcConstantTable.ORGID_KJSSK)
            {
                expanderMarker.Visibility = System.Windows.Visibility.Hidden;
            }

           
        }

        private void InitMap()
        {
            // set cache mode only if no internet avaible
            if (!Stuff.PingNetwork("localhost"))
            {
                MainMap.Manager.Mode = AccessMode.CacheOnly;
                MessageBox.Show("No internet connection available, going to CacheOnly mode.", "GMap.NET - Demo.WindowsPresentation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            MainMap.Manager.Mode = AccessMode.ServerAndCache;
            MainMap.ShowTileGridLines = false;

            // config map
            string wkt = "GEOGCS[\"WGS 84\", ";
            wkt += " DATUM[\"World Geodetic System 1984\", ";
            wkt += " SPHEROID[\"WGS 84\", 6378137.0, 298.257223563, AUTHORITY[\"EPSG\",\"7030\"]], ";
            wkt += " AUTHORITY[\"EPSG\",\"6326\"]], ";
            wkt += " PRIMEM[\"Greenwich\", 0.0, AUTHORITY[\"EPSG\",\"8901\"]], ";
            wkt += " UNIT[\"degree\", 0.017453292519943295], ";
            wkt += " AXIS[\"Geodetic longitude\", EAST], ";
            wkt += " AXIS[\"Geodetic latitude\", NORTH], ";
            wkt += " AUTHORITY[\"EPSG\",\"4326\"]] ";

            string wmsUri = GeoserverUri + "/wms";
            string layer = DotNet.Utilities.UserConfigHelper.GetValue("Layer");
            string zoom = DotNet.Utilities.UserConfigHelper.GetValue("Zoom");
            string bounds = DotNet.Utilities.UserConfigHelper.GetValue("Bounds"); 

            GMapProviders.WMSProviderdemoMap.Init("YuChang Map",
                        wmsUri,
                        layer,
                        zoom,
                        bounds,
                        wkt,
                        "png",
                        new List<GMapProvider>()
                       );

            MainMap.MapProvider = GMapProviders.WMSProviderdemoMap;
            MainMap.Position = new PointLatLng(PosLat, PosLng);
            MainMap.ScaleMode = ScaleModes.Dynamic;

            // setup zoom min/max
            sliderZoom.Maximum = MainMap.MaxZoom;
            sliderZoom.Minimum = MainMap.MinZoom;
            MainMap.Zoom = 19;
            //validator.Window = this;
            MainMap.CanDragMap = true;

            // set current marker 地市中心点
            currentMarker = new GMapMarker(MainMap.Position);
            {
                currentMarker.Shape = new CustomMarkerRed(this, currentMarker, "中心点");
                currentMarker.Offset = new System.Windows.Point(0, 0);
                currentMarker.ZIndex = int.MaxValue;
                MainMap.Markers.Add(currentMarker);
            }

            //List<PointAndInfo> objects = GetExistPointAndInfo();

            //AddDemoZone(areaRadius, MainMap.Position, objects);

            //List<RouterPointAndInfo> objectRouters = GetExistRouterPoint();

            //DrawRouterLineInfo(objectRouters);

            //if (MainMap.Markers.Count > 1)
            //{
            //    MainMap.ZoomAndCenterMarkers(null);
            //}
        }

        void DrawRouterLineInfo(List<RouterPointAndInfo> objects)
        {
            foreach (RouterPointAndInfo a in objects)
            {
                List<PointLatLng> list = new List<PointLatLng>();
                list.Add(a.start);
                list.Add(a.end);

                GMapRoute r = new GMapRoute(list);

                MainMap.Markers.Add(r);
            }
        }

        // add objects and zone around them
        void AddDemoZone(double areaRadius, PointLatLng center, List<PointAndInfo> objects)
        {
            var objectsInArea = from p in objects
                                where MainMap.MapProvider.Projection.GetDistance(center, p.Point) <= areaRadius
                                select new
                                {
                                    Obj = p,
                                    Dist = MainMap.MapProvider.Projection.GetDistance(center, p.Point)
                                };
            if (objectsInArea.Any())
            {
                var maxDistObject = (from p in objectsInArea
                                     orderby p.Dist descending
                                     select p).First();

                // add objects to zone
                foreach (var o in objectsInArea)
                {
                    GMapMarker it = new GMapMarker(o.Obj.Point);
                    {
                        string tmpImgSource = YcConstantTable.StaticMarkerStyleList.Find(entity => entity.Id == o.Obj.MarkerTypeId).ImgSource;
                        it.ZIndex = 55;
                        it.Offset = new System.Windows.Point(0, 0);
                        var s = new CustomMarkerDemo(this, it, o.Obj.Info, tmpImgSource);
                        it.Shape = s;                       
                    }

                    MainMap.Markers.Add(it);
                }

                // add zone circle
                if (false)  //暂时不加圈
                {
                    GMapMarker it = new GMapMarker(center);
                    it.ZIndex = -1;

                    Circle c = new Circle();
                    c.Center = center;
                    c.Bound = maxDistObject.Obj.Point;
                    c.Tag = it;
                    c.IsHitTestVisible = true;

                    UpdateCircle(c);
                    Circles.Add(it);

                    it.Shape = c;
                    MainMap.Markers.Add(it);
                }
            }
        }

        // calculates circle radius
        void UpdateCircle(Circle c)
        {
            var pxCenter = MainMap.FromLatLngToLocal(c.Center);
            var pxBounds = MainMap.FromLatLngToLocal(c.Bound);

            double a = (double)(pxBounds.X - pxCenter.X);
            double b = (double)(pxBounds.Y - pxCenter.Y);
            var pxCircleRadius = Math.Sqrt(a * a + b * b);

            c.Width = 55 + pxCircleRadius * 2;
            c.Height = 55 + pxCircleRadius * 2;
            (c.Tag as GMapMarker).Offset = new System.Windows.Point(-c.Width / 2, -c.Height / 2);
        }

        private List<PointAndInfo> GetExistPointAndInfo()
        {//从数据库中取出，添加的坐标点

            List<PointAndInfo> objects = new List<PointAndInfo>();

            foreach (MapMarkersTable tmpMarker in viewModel.Value.EnumMapMarkers.ToList<MapMarkersTable>())
            {
                PointLatLng pos = new PointLatLng(Convert.ToDouble(tmpMarker.lat), Convert.ToDouble(tmpMarker.lng));
                objects.Add(new PointAndInfo(pos, tmpMarker.Title, Convert.ToInt32(tmpMarker.StyleTypeId) ));
            }

            return objects;
        }

        private List<RouterPointAndInfo> GetExistRouterPoint()
        {//从数据库中取出，添加的Router坐标点

            List<RouterPointAndInfo> objects = new List<RouterPointAndInfo>();

            foreach (MapRouterTable tmpRouter in viewModel.Value.EnumMapRouter.ToList<MapRouterTable>())
            {
                PointLatLng posStart = new PointLatLng(Convert.ToDouble(tmpRouter.latStart), Convert.ToDouble(tmpRouter.lngStart));
                PointLatLng posEnd = new PointLatLng(Convert.ToDouble(tmpRouter.latEnd), Convert.ToDouble(tmpRouter.lngEnd));

                objects.Add(new RouterPointAndInfo(posStart, posEnd, tmpRouter.Title));
            }

            return objects;
        }

        public void AddRouter(double startLat, double startLng, double endLat, double endLng)
        {
            PointLatLng start = new PointLatLng(startLat, startLng);
            PointLatLng end = new PointLatLng(endLat, endLng);

            List<PointLatLng> list = new List<PointLatLng>();
            list.Add(start);
            list.Add(end);

            GMapRoute r = new GMapRoute(list);

            MainMap.Markers.Add(r);
        }

        public void AddMarker(double lat, double lng)
        {            
            //add marker at selected position
            GMapMarker it = new GMapMarker(new PointLatLng(lat, lng));
            //int curMarkerStyleId = Convert.ToInt32(CbxMarkerStyle.SelectedValue);

            //if (curMarkerStyleId == Convert.ToInt32(MapMarkerType.Type1))
            //{
            it.Shape = new CustomMarkerDemo(this, it, txtMarkerTitle.Text.Trim(), viewModel.Value.DefaultMarkerStyle.ImgSource);
            it.Offset = new System.Windows.Point(0, 0);
            it.ZIndex = 55;
            
            MainMap.Markers.Add(it);
            
        }

        public void DeleteMarker()
        {
            List<GMapMarker> delMarkerList = MainMap.Markers.ToList().FindAll
                (
                entity =>
                entity.Position.Lat == WillDelMarker.Position.Lat
                && entity.Position.Lng == WillDelMarker.Position.Lng
                );

            foreach (GMapMarker tmp in delMarkerList)
            {
                MainMap.Markers.Remove(tmp);
            }
        }

        // zoom changed
        private void sliderZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // updates circles on map
            foreach (var c in Circles)
            {
                UpdateCircle(c.Shape as Circle);
            }
        }

        // zoom up
        private void czuZoomUp_Click(object sender, RoutedEventArgs e)
        {
            MainMap.Zoom = ((int)MainMap.Zoom) + 1;
        }

        // zoom down
        private void czuZoomDown_Click(object sender, RoutedEventArgs e)
        {
            MainMap.Zoom = ((int)(MainMap.Zoom + 0.99)) - 1;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string selectKey = string.Empty;
            string tmpKey = txtSearchPoint.Text.Trim();
            if (string.IsNullOrEmpty(tmpKey))
            {
                MessageBox.Show("请输入搜索内容.");
                return;
            }

            IDictionary<string, Object> idc = new Dictionary<string, object>();
            try
            {
                if (CbxPlaceType.SelectedValue.ToString() == "加油站")
                {
                    idc = BuildSearchHandler("加油站");
                }
                else if (CbxPlaceType.SelectedValue.ToString() == "医院或小诊所")
                {
                    idc = BuildSearchHandler("医院或小诊所");
                }
                else if (CbxPlaceType.SelectedValue.ToString() == "金融机构")
                {
                    idc = BuildSearchHandler("金融机构");
                }
                else if (CbxPlaceType.SelectedValue.ToString() == "企业")
                {
                    idc = BuildSearchHandler("企业");
                }
                else if (CbxPlaceType.SelectedValue.ToString() == "居委会")
                {
                    idc = BuildSearchHandler("居委会");
                }
                else if (CbxPlaceType.SelectedValue.ToString() == "市场")
                {
                    idc = BuildSearchHandler("市场");
                }
                else if (CbxPlaceType.SelectedValue.ToString() == "药店")
                {
                    idc = BuildSearchHandler("药店");
                }
                else if (CbxPlaceType.SelectedValue.ToString() == "其他建筑")
                {
                    idc = BuildSearchHandler("其他建筑");
                }
                else if (CbxPlaceType.SelectedValue.ToString() == "学校")
                {
                    idc = BuildSearchHandler("学校");
                }
                else if (CbxPlaceType.SelectedValue.ToString() == "政府机关或事业单位")
                {
                    idc = BuildSearchHandler("政府机关或事业单位");
                }
                //......

                if (idc.Count <= 0)
                {
                    return;
                }
                                
                if (idc.ContainsKey(tmpKey))
                {
                    string cors = idc[tmpKey].ToString();
                    double lat = double.Parse(cors.Split(',')[1], CultureInfo.InvariantCulture);
                    double lng = double.Parse(cors.Split(',')[0], CultureInfo.InvariantCulture);

                    MainMap.Position = new PointLatLng(lat, lng);
                    selectKey = tmpKey;
                }

                viewModel.Value.PlaceItemList.Clear();
                List<string> keys = idc.Keys.ToList<string>();
                int keyCount = 0;
                foreach (string likeKey in keys)
                {
                    keyCount++;
                    if (likeKey.Contains(tmpKey) || tmpKey.Contains(likeKey))
                    {
                        SearchPlaceItem searchPlace = new SearchPlaceItem();
                        searchPlace.Id = keyCount.ToString();
                        searchPlace.Name = likeKey;
                        string cors = idc[likeKey].ToString();
                        searchPlace.Lat = double.Parse(cors.Split(',')[1], CultureInfo.InvariantCulture);
                        searchPlace.Lng = double.Parse(cors.Split(',')[0], CultureInfo.InvariantCulture);
                        viewModel.Value.PlaceItemList.Add(searchPlace);
                        if (selectKey == likeKey)
                        {
                            viewModel.Value.SelectPlaceItem = searchPlace;
                        }
                    }
                }
                lbSearchPlaceItems.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("incorrect format: " + ex.Message);
            }
        }

#region Map Search Handler
        /// <summary>
        /// 查找加油站图层
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, Object> BuildSearchHandler(string layerName)
        {
            IDictionary<string, Object> idc = new Dictionary<string, object>();
            string tmpName = string.Empty;
            string tmpCoordinates = string.Empty;

            string jyzUri = GeoserverUri + "/ows?service=WFS&version=2.0.0&request=GetFeature&typeName=yctraffic_map:" + layerName + "&maxFeatures=50&outputFormat=application/json&propertyName=建筑物名称,GPS定位坐,GPS定位_1";

            string gml = GetContentUsingHttp(jyzUri);
            JsonTextReader reader = new JsonTextReader(new StringReader(gml));
            while (reader.Read())
            {
                if (reader.TokenType.ToString() == "PropertyName")
                {
                    if (reader.Value.ToString() == "type") //'type':'Feature',
                    {
                        reader.Read();
                        if (reader.Value.ToString() == "Feature")
                        {
                            tmpName = string.Empty;
                            tmpCoordinates = string.Empty;
                        }
                    }

                    //if (reader.Value.ToString() == "coordinates") //坐标系
                    //{
                    //    reader.Read(); //skip "["
                    //    reader.Read(); //lat
                    //    tmpCoordinates = reader.Value.ToString();
                    //    reader.Read(); //lng
                    //    tmpCoordinates += "," + reader.Value.ToString();
                    //}

                    if (reader.Value.ToString() == "建筑物名称")
                    {
                        reader.Read();
                        tmpName = reader.Value.ToString();
                        if (idc.ContainsKey(tmpName))
                        {
                            tmpName += "_" + idc.Count.ToString();
                        }
                        //idc.Add(tmpName, tmpCoordinates);
                    }

                    if (reader.Value.ToString() == "GPS定位坐") //坐标系
                    {
                        reader.Read();
                        tmpCoordinates += reader.Value.ToString();
                    }

                    if (reader.Value.ToString() == "GPS定位_1") //坐标系
                    {
                        reader.Read(); //lng
                        tmpCoordinates += "," + reader.Value.ToString();
                        idc.Add(tmpName, tmpCoordinates);
                    }
                }
            }

            return idc;
        }
#endregion
        

        private string GetContentUsingHttp(string url)
        {
            string ret = string.Empty;


            WebRequest request = WebRequest.Create(url);

            using (var response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader read = new StreamReader(responseStream, System.Text.Encoding.UTF8))
                    {
                        ret = read.ReadToEnd();
                    }
                }

                response.Close();
            }

            return ret;
        }


        private void btnAddRouterStart_Click(object sender, RoutedEventArgs e)
        {
            start = currentMarker.Position;
        }

        private void btnAddRouterEnd_Click(object sender, RoutedEventArgs e)
        {
            end = currentMarker.Position;
        }

        private void btnAddRouter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(start.Lat.ToString()) || string.IsNullOrEmpty(end.Lng.ToString()))
            {
                MessageBox.Show("请先选择起始点和终止点 ");
                return;
            }
            viewModel.Value.MapRouter = new MapRouterTable();
            viewModel.Value.MapRouter.latStart= start.Lat.ToString();
            viewModel.Value.MapRouter.lngStart = start.Lng.ToString();

            viewModel.Value.MapRouter.lngEnd = end.Lng.ToString();
            viewModel.Value.MapRouter.latEnd = end.Lat.ToString();

            viewModel.Value.MapRouter.Title = txtMarkerTitle.Text.Trim();
        }

        private void btnAddMarker_Click(object sender, RoutedEventArgs e)
        {
           viewModel.Value.MapMarker = new MapMarkersTable();
           viewModel.Value.MapMarker.lat = currentMarker.Position.Lat.ToString();
           viewModel.Value.MapMarker.lng = currentMarker.Position.Lng.ToString();
           viewModel.Value.MapMarker.Title = txtMarkerTitle.Text.Trim();
           viewModel.Value.MapMarker.AccidentDate = dtAccidentDate.Value;
           viewModel.Value.MapMarker.StyleTypeId = Convert.ToInt32(CbxMarkerStyle.SelectedValue);
        }

        void MainMap_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(MainMap);
            currentMarker.Position = MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
        }

        // move current marker with left holding
        void MainMap_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                System.Windows.Point p = e.GetPosition(MainMap);
                currentMarker.Position = MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
            }
        }

        void MainMap_MouseEnter(object sender, MouseEventArgs e)
        {
            MainMap.Focus();
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int offset = 22;

            if (MainMap.IsFocused)
            {
                if (e.Key == Key.Left)
                {
                    MainMap.Offset(-offset, 0);
                }
                else if (e.Key == Key.Right)
                {
                    MainMap.Offset(offset, 0);
                }
                else if (e.Key == Key.Up)
                {
                    MainMap.Offset(0, -offset);
                }
                else if (e.Key == Key.Down)
                {
                    MainMap.Offset(0, offset);
                }
                else if (e.Key == Key.Add)
                {
                    czuZoomUp_Click(null, null);
                }
                else if (e.Key == Key.Subtract)
                {
                    czuZoomDown_Click(null, null);
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                MainMap.Bearing--;
            }
            else if (e.Key == Key.Z)
            {
                MainMap.Bearing++;
            }
        }

        private void btnZoomCenter_Click(object sender, RoutedEventArgs e)
        {
            MainMap.ZoomAndCenterMarkers(null);
        }

        private void lbSearchPlaceItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbSearchPlaceItems.SelectedItem == null 
                || ((SearchPlaceItem)lbSearchPlaceItems.SelectedItem).Id == "")
            {
                return;
            }
            SearchPlaceItem searchPlace = (SearchPlaceItem)lbSearchPlaceItems.SelectedItem;
            MainMap.Position = new PointLatLng(searchPlace.Lat, searchPlace.Lng);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double lat = double.Parse(txtLat.Text, CultureInfo.InvariantCulture);
                double lng = double.Parse(txtLng.Text, CultureInfo.InvariantCulture);

                MainMap.Position = new PointLatLng(lat, lng);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误的经纬度坐标值,原因: " + ex.Message);
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {

        }

        public void AccidentDateState(bool flag)
        {
            dtAccidentDate.IsEnabled = flag;
            if (flag)
            {
                dtAccidentDate.Value = DateTime.Now;
            }
            else
            {
                dtAccidentDate.Value = null;
            }
        }

        private void gridRouterList_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > gridRouterList.Total)
            {
                startIndex = (gridRouterList.Total / args.PageSize) * args.PageSize;
                gridRouterList.PageIndex = (gridRouterList.Total % args.PageSize) == 0 ? (gridRouterList.Total / args.PageSize) : (gridRouterList.Total / args.PageSize) + 1;
                args.PageIndex = gridRouterList.PageIndex;
            }

            IEnumerable<MapRouterTable> gridSource = viewModel.Value.EnumMapRouter;

            gridRouterList.Total = gridSource.Count();
            gridRouterList.ItemsSource = viewModel.Value.EnumMapRouter.Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
           

        }        

        public void PagingdRouterReload()
        {
            gridRouterList.RaisePageChanged();
        }

        public void DeleteMapRouter(double latStart, double lngStart, double latEnd, double lngEnd)
        {
            foreach (var tmp in MainMap.Markers.ToList())
            {
                if (tmp is GMapRoute)
                {
                    GMapRoute tmpRouter = tmp as GMapRoute;

                    if (tmpRouter.Points.Exists(p => p.Lat == latStart && p.Lng == lngStart)
                        &&
                        tmpRouter.Points.Exists(p => p.Lat == latEnd && p.Lng == lngEnd)
                       )
                    {
                        MainMap.Markers.Remove(tmp);
                    }

                }

            }
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (CbxQueryMarkerStyle.SelectedIndex == -1)
            {
                MessageBox.Show("请选择标记类型");
                return;
            }

            if (dtAccidentStartDate.Value == null)
            {
                MessageBox.Show("请选择事故开始时间");
                return;
            }

            if (dtAccidentEndDate.Value == null)
            {
                MessageBox.Show("请选择事故结束时间");
                return;
            }

            int styleTypeId = Convert.ToInt32(CbxQueryMarkerStyle.SelectedValue);
            DateTime startAccidentDate = dtAccidentStartDate.Value.Value;
            DateTime endAccidentDate = dtAccidentEndDate.Value.Value;


            List<PointAndInfo> objects = GetExistPointAndInfo(styleTypeId, startAccidentDate, endAccidentDate);

            ClearMarkers();
            if (objects.Count > 0)
            {
                AddDemoZone(areaRadius, MainMap.Position, objects);
            }

            if (MainMap.Markers.Count > 1)
            {
                MainMap.ZoomAndCenterMarkers(null);
            }
        }

        private List<PointAndInfo> GetExistPointAndInfo(int styleTypeId, DateTime startAccidentDate, DateTime endAccidentDate)
        {//从数据库中取出，添加的坐标点

            List<PointAndInfo> objects = new List<PointAndInfo>();

            var tmpMapMarkers = viewModel.Value.EnumMapMarkers.Where
                (
                    entity=>
                        entity.StyleTypeId.Value == styleTypeId
                        && (entity.AccidentDate >= startAccidentDate && entity.AccidentDate <= endAccidentDate)
                );

            foreach (MapMarkersTable tmpMarker in tmpMapMarkers)
            {
                PointLatLng pos = new PointLatLng(Convert.ToDouble(tmpMarker.lat), Convert.ToDouble(tmpMarker.lng));
                objects.Add(new PointAndInfo(pos, tmpMarker.Title, Convert.ToInt32(tmpMarker.StyleTypeId)));
            }

            return objects;
        }

        private List<PointAndInfo> GetExistPointAndInfo(int styleTypeId)
        {//从数据库中取出，添加的坐标点

            List<PointAndInfo> objects = new List<PointAndInfo>();

            var tmpMapMarkers = viewModel.Value.EnumMapMarkers.Where(entity =>entity.StyleTypeId.Value == styleTypeId);

            foreach (MapMarkersTable tmpMarker in tmpMapMarkers)
            {
                PointLatLng pos = new PointLatLng(Convert.ToDouble(tmpMarker.lat), Convert.ToDouble(tmpMarker.lng));
                objects.Add(new PointAndInfo(pos, tmpMarker.Title, Convert.ToInt32(tmpMarker.StyleTypeId)));
            }

            return objects;
        }

        private void btnShowCamera_Click(object sender, RoutedEventArgs e)
        {
            List<PointAndInfo> objects = GetExistPointAndInfo( Convert.ToInt32(MapMarkerType.Camera) );

            ClearMarkers();
            if (objects.Count > 0)
            {
                AddDemoZone(areaRadius, MainMap.Position, objects);
            }

            if (MainMap.Markers.Count > 1)
            {
                MainMap.ZoomAndCenterMarkers(null);
            }
        }

        private void btnShowKaKou_Click(object sender, RoutedEventArgs e)
        {
            List<PointAndInfo> objects = GetExistPointAndInfo( Convert.ToInt32(MapMarkerType.Bayonet) );

            ClearMarkers();
            if (objects.Count > 0)
            {
                AddDemoZone(areaRadius, MainMap.Position, objects);
            }

            if (MainMap.Markers.Count > 1)
            {
                MainMap.ZoomAndCenterMarkers(null);
            }
        }

        private void btnShowLamb_Click(object sender, RoutedEventArgs e)
        {
            List<PointAndInfo> objects = GetExistPointAndInfo( Convert.ToInt32(MapMarkerType.SignalLamp) );

            ClearMarkers();
            if (objects.Count > 0)
            {
                AddDemoZone(areaRadius, MainMap.Position, objects);
            }

            if (MainMap.Markers.Count > 1)
            {
                MainMap.ZoomAndCenterMarkers(null);
            }
        }

        private void btnShowLines_Click(object sender, RoutedEventArgs e)
        {
            List<RouterPointAndInfo> objectRouters = GetExistRouterPoint();

            ClearMarkers();
            DrawRouterLineInfo(objectRouters);


            if (MainMap.Markers.Count > 1)
            {
                MainMap.ZoomAndCenterMarkers(null);
            }
        }

        private void ClearMarkers()
        {
            int i = 0;
            while(true)
            {
                if (MainMap.Markers.Count <= 1)
                {
                    break;
                }
                if (MainMap.Markers[i] != currentMarker)
                {
                    MainMap.Markers.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
        
        
    }
}
    