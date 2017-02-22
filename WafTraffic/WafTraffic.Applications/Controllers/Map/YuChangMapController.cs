//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.Composition;
//using System.ComponentModel.Composition.Hosting;
//using System.Linq;
//using System.Waf.Applications;
//using System.Waf.Applications.Services;
//using System.Waf.Foundation;
//using WafTraffic.Applications.Properties;
//using WafTraffic.Applications.Services;
//using WafTraffic.Applications.ViewModels;
//using WafTraffic.Applications.Views;
//using WafTraffic.Domain;
//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Data;
//using System.Globalization;
//using DotNet.Business;
//using WafTraffic.Domain.Common;

//namespace WafTraffic.Applications.Controllers
//{
//    [Export]
//    internal class YuChangMapController : Controller
//    {
//        private readonly CompositionContainer container;
//        private readonly IShellService shellService;
//        private readonly System.Waf.Applications.Services.IMessageService messageService;
//        private readonly IEntityService entityService;

//        //private MainFrameViewModel mainFrameViewModel;
//        private YuChangMapViewModel yuchengViewModel;

//        private readonly DelegateCommand addMarkerCommand;
//        private readonly DelegateCommand delMarkerCommand;

//        private readonly DelegateCommand addRouterCommand;
//        private readonly DelegateCommand delRouterCommand;

//        [ImportingConstructor]
//        public YuChangMapController(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
//        {
//            this.container = container;
//            this.messageService = messageService;
//            this.shellService = shellService;
//            this.entityService = entityService;

//            //mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();
//            yuchengViewModel = container.GetExportedValue<YuChangMapViewModel>();

//            this.addMarkerCommand = new DelegateCommand(() => AddMarkerOper(), null);
//            this.delMarkerCommand = new DelegateCommand(() => DelMarkerOper(), null);

//            this.addRouterCommand = new DelegateCommand(() => AddRouterOper(), null);
//            this.delRouterCommand = new DelegateCommand(() => DelRouterOper(), null);
//        }

//        public void Initialize()
//        {
//            AddWeakEventListener(yuchengViewModel, yuChengMapViewModelPropertyChanged);

//            yuchengViewModel.AddMarkerCommand = this.addMarkerCommand;
//            yuchengViewModel.DelMarkerCommand = this.delMarkerCommand;

//            yuchengViewModel.AddRouterCommand = this.addRouterCommand;
//            yuchengViewModel.DeleteRouterCommand = this.delRouterCommand;
//        }

//        public bool AddRouterOper()
//        {
//            bool newer = true;

//            if (string.IsNullOrEmpty(yuchengViewModel.MapRouter.latStart) || string.IsNullOrEmpty(yuchengViewModel.MapRouter.latEnd))
//            {
//                messageService.ShowMessage("请先选择起始点和终止点");
//                return false;
//            }

//            try
//            {
//                yuchengViewModel.MapRouter.DepartmentId = CurrentLoginService.Instance.CurrentUserInfo.DepartmentId;
//                yuchengViewModel.MapRouter.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.DepartmentName;

//                yuchengViewModel.MapRouter.IsDeleted = false;
//                yuchengViewModel.MapRouter.CreateUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
//                yuchengViewModel.MapRouter.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
//                yuchengViewModel.MapRouter.CreateTime = System.DateTime.Now;

//                yuchengViewModel.MapRouter.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
//                yuchengViewModel.MapRouter.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
//                yuchengViewModel.MapRouter.UpdateTime = System.DateTime.Now;

//                entityService.MapRouter.Add(yuchengViewModel.MapRouter);
//                entityService.Entities.SaveChanges();
//                //在页面上画出标记
//                yuchengViewModel.AddRouter(Convert.ToDouble(yuchengViewModel.MapRouter.latStart), Convert.ToDouble(yuchengViewModel.MapRouter.lngStart), Convert.ToDouble(yuchengViewModel.MapRouter.latEnd),
//                         Convert.ToDouble(yuchengViewModel.MapRouter.lngEnd));
//            }
//            catch (ValidationException e)
//            {
//                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
//                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidEntities, e.Message));
//            }
//            catch (UpdateException e)
//            {
//                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
//                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidFields, e.InnerException.Message));
//            }


//            return newer;
//        }


//        public bool AddMarkerOper()
//        {
//            bool newer = true;

//            if (string.IsNullOrEmpty(yuchengViewModel.MapMarker.Title))
//            {
//                messageService.ShowMessage("请输入【标记名称】");
//                return false;
//            }

//            if (yuchengViewModel.DefaultMarkerStyle.ClassId != MapMarkerClass.Accident)
//            {
//                yuchengViewModel.MapMarker.AccidentDate = Convert.ToDateTime("1900-1-1");
//            }
//            else if (yuchengViewModel.MapMarker.AccidentDate == null || yuchengViewModel.MapMarker.AccidentDate == DateTime.MinValue)
//            {
//                messageService.ShowMessage("请输入【事故发生时间】");
//                return false;
//            }

//            try
//            {
//                yuchengViewModel.MapMarker.ClassId = Convert.ToInt32(yuchengViewModel.DefaultMarkerStyle.ClassId);
                
//                yuchengViewModel.MapMarker.DepartmentId = CurrentLoginService.Instance.CurrentUserInfo.DepartmentId;
//                yuchengViewModel.MapMarker.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.DepartmentName;

//                yuchengViewModel.MapMarker.IsDeleted = false;
//                yuchengViewModel.MapMarker.CreateUserId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
//                yuchengViewModel.MapMarker.CreateUserName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
//                yuchengViewModel.MapMarker.CreateTime = System.DateTime.Now;

//                yuchengViewModel.MapMarker.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
//                yuchengViewModel.MapMarker.UpdaterName = CurrentLoginService.Instance.CurrentUserInfo.RealName;
//                yuchengViewModel.MapMarker.UpdateTime = System.DateTime.Now;

//                entityService.MapMarkers.Add(yuchengViewModel.MapMarker);
//                entityService.Entities.SaveChanges();
//                //在页面上画出标记
//                yuchengViewModel.AddMarker(Convert.ToDouble(yuchengViewModel.MapMarker.lat), Convert.ToDouble(yuchengViewModel.MapMarker.lng));
//            }
//            catch (ValidationException e)
//            {
//                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
//                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidEntities, e.Message));
//            }
//            catch (UpdateException e)
//            {
//                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
//                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidFields, e.InnerException.Message));
//            }
            

//            return newer;
//        }

//        public bool DelMarkerOper()
//        {
//            bool deler = true;
//            int count = 0;

//            if (yuchengViewModel.DelMapMarker == null)
//            {
//                messageService.ShowMessage("请选择要删除的标记。");
//                return false;
//            }
            
//            try
//            {
//                while (true)
//                {
//                    int index = entityService.MapMarkers.ToList<MapMarkersTable>().FindIndex
//                        (
//                        entity =>
//                            entity.lat == yuchengViewModel.DelMapMarker.lat.ToString()
//                            && entity.lng == yuchengViewModel.DelMapMarker.lng.ToString()
//                            && entity.IsDeleted == false
//                        );
//                    if (index != -1)
//                    {
//                        entityService.MapMarkers[index].IsDeleted = true;
//                        count++;
//                    }
//                    else
//                    {
//                        break;
//                    }
//                }

//                if (count == 0)
//                {
//                    messageService.ShowMessage("数据库中未查到此标记。");
//                    return false;
//                }

//                entityService.Entities.SaveChanges();
//                //在页面上删除标记
//                yuchengViewModel.DeleteMarker();
//            }
//            catch (ValidationException e)
//            {
//                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
//                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidEntities, e.Message));
//            }
//            catch (UpdateException e)
//            {
//                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
//                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidFields, e.InnerException.Message));
//            }
//            return deler;
//        }

//        public bool DelRouterOper()
//        {
//            bool deler = true;

//            if (yuchengViewModel.SelectMapRouter == null || yuchengViewModel.SelectMapRouter.Id == 0)
//            {
//                messageService.ShowMessage("请选择要删除的线路。");
//                return false;
//            }

//            try
//            {
//                yuchengViewModel.SelectMapRouter.IsDeleted = true;
//                entityService.Entities.SaveChanges();
//                //在页面上删除标记
//                yuchengViewModel.DeleteMapRouter(
//                    Convert.ToDouble(yuchengViewModel.SelectMapRouter.latStart),
//                    Convert.ToDouble(yuchengViewModel.SelectMapRouter.lngStart),
//                    Convert.ToDouble(yuchengViewModel.SelectMapRouter.latEnd),
//                    Convert.ToDouble(yuchengViewModel.SelectMapRouter.lngEnd)
//                    );
//                //刷新列表
//                yuchengViewModel.PagingdRouterReload();
//            }
//            catch (ValidationException e)
//            {
//                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
//                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidEntities, e.Message));
//            }
//            catch (UpdateException e)
//            {
//                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
//                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidFields, e.InnerException.Message));
//            }
//            return deler;
//        }

//        private void yuChengMapViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (e.PropertyName == "DefaultMarkerStyle")
//            {
//                if (yuchengViewModel.DefaultMarkerStyle.ClassId == MapMarkerClass.Accident)
//                {
//                    yuchengViewModel.AccidentDateState(true);
//                }
//                else
//                {
//                    yuchengViewModel.AccidentDateState(false);
//                }
//            }
//        }

//    }
//}
    