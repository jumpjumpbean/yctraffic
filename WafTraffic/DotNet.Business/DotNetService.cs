//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Bop Tech, Ltd. 
//-----------------------------------------------------------------

/// 修改纪录
/// 
///		2012.06.29 版本：2.0 sunmiao 修改
///		
/// 版本：2.0
/// 

namespace DotNet.Business
{
    using System;
    using DotNet.Utilities;

    public class DotNetService : AbstractServiceFactory
    {
        private static DotNetService instance = null;
        private static object locker = new object();
        private IServiceFactory serviceFactory;
        private static readonly string serviceFactoryClass = BaseSystemInfo.ServiceFactory;
        private static readonly string servicePath = BaseSystemInfo.Service;

        public DotNetService()
        {
            this.serviceFactory = base.GetServiceFactory(servicePath, serviceFactoryClass);
        }

        public void InitService()
        {
            this.serviceFactory.InitService();
        }

        public IDbHelperService BusinessDbHelperService
        {
            get
            {
                return this.serviceFactory.CreateBusinessDbHelperService();
            }
        }

        public IExceptionService ExceptionService
        {
            get
            {
                return this.serviceFactory.CreateExceptionService();
            }
        }

        public IFileService FileService
        {
            get
            {
                return this.serviceFactory.CreateFileService();
            }
        }

        public IFolderService FolderService
        {
            get
            {
                return this.serviceFactory.CreateFolderService();
            }
        }

        public static DotNetService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new DotNetService();
                        }
                    }
                }
                return instance;
            }
        }

        public IItemDetailsService ItemDetailsService
        {
            get
            {
                return this.serviceFactory.CreateItemDetailsService();
            }
        }

        public IItemsService ItemsService
        {
            get
            {
                return this.serviceFactory.CreateItemsService();
            }
        }

        public ILogOnService LogOnService
        {
            get
            {
                return this.serviceFactory.CreateLogOnService();
            }
        }

        public ILogService LogService
        {
            get
            {
                return this.serviceFactory.CreateLogService();
            }
        }

        public IMessageService MessageService
        {
            get
            {
                return this.serviceFactory.CreateMessageService();
            }
        }

        public IModuleService ModuleService
        {
            get
            {
                return this.serviceFactory.CreateModuleService();
            }
        }

        public IOrganizeService OrganizeService
        {
            get
            {
                return this.serviceFactory.CreateOrganizeService();
            }
        }

        public IParameterService ParameterService
        {
            get
            {
                return this.serviceFactory.CreateParameterService();
            }
        }

        public IPermissionItemService PermissionItemService
        {
            get
            {
                return this.serviceFactory.CreatePermissionItemService();
            }
        }

        public IPermissionService PermissionService
        {
            get
            {
                return this.serviceFactory.CreatePermissionService();
            }
        }

        public IRoleService RoleService
        {
            get
            {
                return this.serviceFactory.CreateRoleService();
            }
        }

        public ISequenceService SequenceService
        {
            get
            {
                return this.serviceFactory.CreateSequenceService();
            }
        }

        public IStaffService StaffService
        {
            get
            {
                return this.serviceFactory.CreateStaffService();
            }
        }

        public ITableColumnsService TableColumnsService
        {
            get
            {
                return this.serviceFactory.CreateTableColumnsService();
            }
        }

        public IDbHelperService UserCenterDbHelperService
        {
            get
            {
                return this.serviceFactory.CreateUserCenterDbHelperService();
            }
        }

        public IUserService UserService
        {
            get
            {
                return this.serviceFactory.CreateUserService();
            }
        }

        public IWorkFlowActivityAdminService WorkFlowActivityAdminService
        {
            get
            {
                return this.serviceFactory.CreateWorkFlowActivityAdminService();
            }
        }

        public IWorkFlowCurrentService WorkFlowCurrentService
        {
            get
            {
                return this.serviceFactory.CreateWorkFlowCurrentService();
            }
        }

        public IWorkFlowProcessAdminService WorkFlowProcessAdminService
        {
            get
            {
                return this.serviceFactory.CreateWorkFlowProcessAdminService();
            }
        }
    }
}

