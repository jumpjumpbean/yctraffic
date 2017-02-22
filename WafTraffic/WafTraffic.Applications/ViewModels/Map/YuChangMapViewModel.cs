using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;
using DotNet.Business;
using System.Data;
using WafTraffic.Domain.Common;


namespace WafTraffic.Applications.ViewModels
{
    [Export]
    public class YuChangMapViewModel : ViewModel<IYuChangMapView>
    {       
        private MapMarkersTable mapMarker;
        private DeleteMapMarker delMapMarker;
        private MapMarkerStyle defaultMarkerStyle;
        private List<MapMarkerStyle> markerStyleList;
        private List<MapMarkerStyle> accidentMarkerStyleList;

        private MapRouterTable mapRouter;
        //private double posLat;
        //private double posLng;
        private IEnumerable<MapRouterTable> enumMapRouter;
        private IEnumerable<MapMarkersTable> enumMapMarkers;
        private List<SearchPlaceType> placeTypeList;
        private List<SearchPlaceItem> placeItemList;
        private SearchPlaceItem selectPlaceItem;

        private MapRouterTable selectMapRouter;

        private ICommand addMarkerCommand;
        private ICommand delMarkerCommand;

        private ICommand addRouterCommand;
        private ICommand deleteRouterCommand;

        [ImportingConstructor]
        public YuChangMapViewModel(IYuChangMapView view, IEntityService entityservice)
            : base(view)
        {
            placeItemList = new List<SearchPlaceItem>();
            selectPlaceItem = new SearchPlaceItem();
            selectMapRouter = new MapRouterTable();

            markerStyleList = new List<MapMarkerStyle>();
            string curDepart = CurrentLoginService.Instance.CurrentUserInfo.DepartmentName;

            if(CurrentLoginService.Instance.CurrentUserInfo.IsAdministrator)
            {
                markerStyleList = YcConstantTable.StaticMarkerStyleList;
            }
            else if (curDepart != YcConstantTable.ORGID_SGK.ToString())  //�¹ʿ�ID
            {
                markerStyleList = YcConstantTable.StaticMarkerStyleList.FindAll(entity => entity.ClassId == MapMarkerClass.Accident);
            }
            else if (curDepart != YcConstantTable.ORGID_KJSSK.ToString())  //�Ƽ���ʩ��ID
            {
                markerStyleList = YcConstantTable.StaticMarkerStyleList.FindAll(entity => entity.ClassId == MapMarkerClass.Equipment);
            }            

            defaultMarkerStyle = markerStyleList[0];
            if (defaultMarkerStyle.ClassId == MapMarkerClass.Accident)
            {
                this.AccidentDateState(true);
            }
            else
            {
                this.AccidentDateState(false);
            }

            accidentMarkerStyleList = YcConstantTable.StaticMarkerStyleList.FindAll(entity => entity.ClassId == MapMarkerClass.Accident);

            if (entityservice.EnumMapMarkers != null)
            {
                this.enumMapMarkers = entityservice.EnumMapMarkers;
            }
            else
            {
                this.enumMapMarkers = new List<MapMarkersTable>(); //�Է�û������ʱ�����쳣
            }

            if (entityservice.EnumMapRouter != null)
            {
                this.enumMapRouter = entityservice.EnumMapRouter;
            }
            else
            {
                this.enumMapRouter = new List<MapRouterTable>(); //�Է�û������ʱ�����쳣
            }

            placeTypeList = new List<SearchPlaceType>();
            placeTypeList.Add(new SearchPlaceType("����վ", "����վ"));
            placeTypeList.Add(new SearchPlaceType("ҽԺ��С����", "ҽԺ��С����"));
            placeTypeList.Add(new SearchPlaceType("���ڻ���", "���ڻ���"));
            placeTypeList.Add(new SearchPlaceType("��ҵ", "��ҵ"));
            placeTypeList.Add(new SearchPlaceType("��ί��", "��ί��"));
            placeTypeList.Add(new SearchPlaceType("�г�", "�г�"));
            placeTypeList.Add(new SearchPlaceType("ҩ��", "ҩ��"));
            placeTypeList.Add(new SearchPlaceType("��������", "��������"));
            placeTypeList.Add(new SearchPlaceType("��·��", "��·��"));
            placeTypeList.Add(new SearchPlaceType("ѧУ", "ѧУ"));
            placeTypeList.Add(new SearchPlaceType("�������ػ���ҵ��λ", "�������ػ���ҵ��λ")); 
        }

        public void AddRouter(double startLat, double startLng, double endLat, double endLng)
        {
            ViewCore.AddRouter(startLat, startLng, endLat, endLng);
        }

        public void AddMarker(double posLat, double posLng)
        {
            ViewCore.AddMarker(posLat, posLng);
        }

        public void DeleteMarker()
        {
            ViewCore.DeleteMarker();
        }

        public void DeleteMapRouter(double latStart, double lngStart, double latEnd, double lngEnd)
        {
            ViewCore.DeleteMapRouter(latStart, lngStart, latEnd, lngEnd);
        }

        public List<SearchPlaceType> PlaceTypeList
        {
            get
            {
                return placeTypeList;
            }
            set
            {
                placeTypeList = value;
                RaisePropertyChanged("PlaceTypeList");
            }
        }

        public List<SearchPlaceItem> PlaceItemList
        {
            get
            {
                return placeItemList;
            }
            set
            {
                placeItemList = value;
                RaisePropertyChanged("PlaceItemList");
            }
        }

        public SearchPlaceItem SelectPlaceItem
        {
            get
            {
                return selectPlaceItem;
            }
            set
            {
                selectPlaceItem = value;
                RaisePropertyChanged("SelectPlaceItem");
            }
        }


        public IEnumerable<MapRouterTable> EnumMapRouter
        {
            get
            {
                return enumMapRouter;
            }
            set
            {
                enumMapRouter = value;
                RaisePropertyChanged("EnumMapRouter");
            }
        }

        public IEnumerable<MapMarkersTable> EnumMapMarkers
        {
            get
            {
                return enumMapMarkers;
            }
            set
            {
                enumMapMarkers = value;
                RaisePropertyChanged("EnumMapMarkers");
            }
        }

        public MapMarkersTable MapMarker
        {
            get { return mapMarker; }
            set
            {
                if (mapMarker != value)
                {
                    mapMarker = value;
                    RaisePropertyChanged("MapMarker");
                }
            }
        }


        public MapRouterTable MapRouter
        {
            get { return mapRouter; }
            set
            {
                if (mapRouter != value)
                {
                    mapRouter = value;
                    RaisePropertyChanged("MapRouter");
                }
            }
        }

        public DeleteMapMarker DelMapMarker
        {
            get { return delMapMarker; }
            set
            {
                if (delMapMarker != value)
                {
                    delMapMarker = value;
                    RaisePropertyChanged("DelMapMarker");
                }
            }
        }


        public MapMarkerStyle DefaultMarkerStyle
        {
            get { return defaultMarkerStyle; }
            set
            {
                if (!object.Equals(defaultMarkerStyle, value) )
                {
                    defaultMarkerStyle = value;
                    RaisePropertyChanged("DefaultMarkerStyle");
                }
            }
        }

        public void AccidentDateState(bool flag)
        {
            ViewCore.AccidentDateState(flag);
        }

        public void PagingdRouterReload()
        {
            ViewCore.PagingdRouterReload();
        }

        public List<MapMarkerStyle> MarkerStyleList
        {
            get { return markerStyleList; }            
        }

        /// <summary>
        /// �¹����ͣ����ڰ󶨲�ѯ�б�
        /// </summary>
        public List<MapMarkerStyle> AccidentMarkerStyleList
        {
            get { return accidentMarkerStyleList; }
        }

        public ICommand AddRouterCommand
        {
            get { return addRouterCommand; }
            set
            {
                if (addRouterCommand != value)
                {
                    addRouterCommand = value;
                    RaisePropertyChanged("AddRouterCommand");
                }
            }
        }

        public ICommand AddMarkerCommand
        {
            get { return addMarkerCommand; }
            set
            {
                if (addMarkerCommand != value)
                {
                    addMarkerCommand = value;
                    RaisePropertyChanged("AddMarkerCommand");
                }
            }
        }

        public ICommand DelMarkerCommand
        {
            get { return delMarkerCommand; }
            set
            {
                if (delMarkerCommand != value)
                {
                    delMarkerCommand = value;
                    RaisePropertyChanged("DelMarkerCommand");
                }
            }
        }

       
        public ICommand DeleteRouterCommand
        {
            get { return deleteRouterCommand; }
            set
            {
                if (deleteRouterCommand != value)
                {
                    deleteRouterCommand = value;
                    RaisePropertyChanged("DeleteRouterCommand");
                }
            }
        }

       
        public MapRouterTable SelectMapRouter
        {
            get { return selectMapRouter; }
            set
            {
                if (selectMapRouter != value)
                {
                    selectMapRouter = value;
                    RaisePropertyChanged("SelectMapRouter");
                }
            }
        }

    }
}
    