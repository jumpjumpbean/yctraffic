using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNet.Utilities;
using System.Data;

namespace DotNet.Business
{
    /// <summary>
    /// Add by ydf 2014-08-03 , for WAF use
    /// </summary>
    public class CurrentLoginService
    {
        private static CurrentLoginService instance = null;
        private static object locker = new object();

        public static CurrentLoginService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new CurrentLoginService();
                        }
                    }
                }
                return instance;
            }
        }


        public BaseUserInfo CurrentUserInfo { get; set; }
        public DataTable DTModule { get; set; }
        public DataTable DTPermission { get; set; }


        /// <summary>
        /// 是否有相应的权限
        /// </summary>
        /// <param name="permissionItemCode">权限编号</param>
        /// <returns>是否有权限</returns>
        //public bool IsAuthorized(string permissionItemCode, string permissionItemName = null) //C#4.0支持默认参数
        public bool IsAuthorized(string permissionItemCode)
        {
            DotNetService dotNetService = new DotNetService();
            return dotNetService.PermissionService.IsAuthorizedByUser(this.CurrentUserInfo, this.CurrentUserInfo.Id, permissionItemCode);
        }

        /// <summary>
        /// 是否有相应的模块权限
        /// </summary>
        /// <param name="moduleCode">模块编号</param>
        /// <returns>是否有权限</returns>
        public bool IsModuleAuthorized(string moduleCode)
        {
            if (this.CurrentUserInfo.IsAdministrator)
            {
                return true;
            }
            return BaseBusinessLogic.Exists(this.DTModule, BaseModuleEntity.FieldCode, moduleCode);
        }

        /// <summary>
        /// 获模块列表
        /// </summary>
        public void InitModuleDT()
        {

            this.DTModule = DotNetService.Instance.PermissionService.GetModuleDTByUser(this.CurrentUserInfo, this.CurrentUserInfo.Id); DTModule.DefaultView.Sort = BaseModuleEntity.FieldSortCode;
        }

        /// <summary>
        /// 当前操作员的权限数据
        /// </summary>
        public void InitDTPermission()
        {
            this.DTPermission = DotNetService.Instance.PermissionService.GetPermissionDTByUser(this.CurrentUserInfo, this.CurrentUserInfo.Id);
            this.DTPermission.DefaultView.Sort = BaseModuleEntity.FieldSortCode;
        }

        /// <summary>
        /// 必须在根据登录用户初始化，权限表之后再调用
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseModuleEntity> GetMainModuleList()
        {
            List<BaseModuleEntity> modules = new List<BaseModuleEntity>();
            DataRow[] drs = DTModule.Select("parentid='10005075'");
            foreach (DataRow dr in drs)
            {
                modules.Add(new BaseModuleEntity(dr));
            }
            return modules.AsEnumerable<BaseModuleEntity>();
        }

        public IEnumerable<BaseModuleEntity> GetMainModuleListWithEnable()
        {
            List<BaseModuleEntity> modules = new List<BaseModuleEntity>();
            List<BaseModuleEntity> permissionModules = GetMainModuleList().ToList();
            DataTable dtModule = DotNetService.Instance.ModuleService.GetDTByParent(this.CurrentUserInfo, "10005075");
            dtModule.DefaultView.Sort = BaseModuleEntity.FieldSortCode;
            foreach (DataRowView dr in dtModule.DefaultView)
            {
                BaseModuleEntity entity = new BaseModuleEntity(dr.Row);
                if (permissionModules.Contains(entity, PopupComparer.Default))
                {
                    entity.Enabled = 1;
                }
                else
                {
                    entity.Enabled = 0;
                }
                modules.Add(entity);
            }
            
            return modules.AsEnumerable<BaseModuleEntity>();
        }

        /// <summary>
        /// 必须在根据登录用户初始化，权限表之后再调用
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseModuleEntity> GetSecModuleList(string parentModuleId)
        {
            List<BaseModuleEntity> modules = new List<BaseModuleEntity>();
            DataRow[] drs = DTModule.Select("parentid='" + parentModuleId  + "'");
            foreach (DataRow dr in drs)
            {
                modules.Add(new BaseModuleEntity(dr));
            }
            return modules.AsEnumerable<BaseModuleEntity>();
        }

        public void LogException(Exception exception)
        {
            using (IDbHelper helper = DbHelperFactory.GetHelper(BaseSystemInfo.UserCenterDbType, null))
            {
                try
                {
                    try
                    {
                        helper.Open(BaseSystemInfo.UserCenterDbConnection);
                        BaseExceptionManager.LogException(helper, this.CurrentUserInfo, exception);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                finally
                {
                    helper.Close();
                }
            }
        }

        private class PopupComparer : IEqualityComparer<BaseModuleEntity>
        {
            public static PopupComparer Default = new PopupComparer();

            public bool Equals(BaseModuleEntity x, BaseModuleEntity y)
            {
                return x.Id.Equals(y.Id);
            }
            public int GetHashCode(BaseModuleEntity obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
